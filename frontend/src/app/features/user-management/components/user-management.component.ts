import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { DropdownModule } from 'primeng/dropdown';
import { TooltipModule } from 'primeng/tooltip';
import { UserService } from '../services/user.service';
import { RoleService } from '../../../services/role.service';
import { User } from '../../../shared/models/user.model';
import { Role, RoleWithUsers } from '../../../shared/models/role.model';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/authentication/auth.service';

@Component({
    selector: 'app-user-management',
    templateUrl: './user-management.component.html',
    styleUrls: ['./user-management.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ButtonModule,
        InputTextModule,
        DialogModule,
        DropdownModule,
        ToastModule,
        ToolbarModule,
        TableModule,
        ConfirmDialogModule,
        CheckboxModule,
        PasswordModule,
        TooltipModule
    ],
    providers: [ConfirmationService, MessageService]
})
export class UserManagementComponent implements OnInit {
  users: any[] = [];
  filteredUsers: any[] = [];
  user: any = {};
  userDialog: boolean = false;
  submitted: boolean = false;
  editMode: boolean = false;
  userForm!: FormGroup;
  roles: any[] = []; // Roller için dizi
  roleFilterOptions: any[] = []; // Rol filtre seçenekleri
  searchText: string = ''; // Arama metni
  selectedRole: any = null; // Seçili rol
  
  // Filtre seçenekleri
  joinedFilterOptions: any[] = [
    { label: 'Herhangi', value: null },
    { label: 'Bu Ay', value: 'thisMonth' },
    { label: 'Bu Yıl', value: 'thisYear' },
    { label: '2020 Öncesi', value: 'before2020' }
  ];
  
  // Aktif filtreler
  activeFilters: {
    search: string;
    role: string | null;
    joined: string | null;
  } = {
    search: '',
    role: null,
    joined: null
  };
  
  // Sayfalama
  currentPage: number = 1;
  rowsPerPage: number = 10;
  totalPages: number = 1;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private userService: UserService,
    private roleService: RoleService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    // Giriş durumunu kontrol et
    const isLoggedIn = this.authService.isLoggedIn();
    const token = this.authService.getToken();
    console.log('Giriş durumu:', { isLoggedIn, hasToken: !!token });
    
    if (!isLoggedIn) {
      console.error('Kullanıcı girişi yapılmamış! Lütfen önce giriş yapın.');
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Oturum açık değil. Lütfen giriş yapın.'
      });
      // Giriş sayfasına yönlendir
      this.router.navigate(['/login']);
      return;
    }
    
    this.initForm();
    this.loadUsers();
    this.loadRoles();
  }

  initForm() {
    // Temel form alanları
    this.userForm = this.formBuilder.group({
      fullName: ['', Validators.required],
      sicil: ['', Validators.required],
      roleId: [null, Validators.required]
    });

    // Şifre alanı, düzenleme modunda opsiyonel, yeni kullanıcı oluşturmada zorunlu
    if (this.editMode) {
      this.userForm.addControl('password', 
        this.formBuilder.control('', [Validators.minLength(6)]));
    } else {
      this.userForm.addControl('password', 
        this.formBuilder.control('', [Validators.required, Validators.minLength(6)]));
    }
  }

  loadUsers() {
    console.log('Kullanıcılar yükleniyor...');
    
    this.userService.getUsers().subscribe({
      next: (data) => {
        console.log('Kullanıcı verileri alındı:', data);
        
        this.users = data.map(user => {
          // Kullanıcı verilerini dönüştür
          return {
            id: user.id,
            fullName: user.username, // Kullanıcı adını fullName alanına at
            sicil: user.sicil || `N/A-${user.id}`,
            permissions: user.isAdmin ? 'Admin' : (user.roleName || 'Kullanıcı'),
            roleId: user.roleId || (user.isAdmin ? 1 : 2),
            isAdmin: user.isAdmin,
            isActive: true,
            createdAt: user.createdAt || new Date()
          };
        });
        
        console.log('Kullanıcı verileri işlendi:', this.users);
        this.filteredUsers = [...this.users];
        this.updatePagination();
      },
      error: (error) => {
        console.error('Kullanıcı verileri yüklenirken hata oluştu:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcı verileri yüklenemedi.'
        });
        
        // Hata durumunda test verilerini kullan
        this.users = [
          { 
            id: 1, 
            fullName: 'Admin Kullanıcı', 
            sicil: 'A001',
            permissions: 'Admin', 
            isAdmin: true, 
            roleId: 1, 
            isActive: true,
            createdAt: new Date()
          },
          { 
            id: 2, 
            fullName: 'Test Kullanıcı 1', 
            sicil: 'U001',
            permissions: 'Kullanıcı', 
            isAdmin: false, 
            roleId: 2, 
            isActive: true,
            createdAt: new Date()
          }
        ];
        
        this.filteredUsers = [...this.users];
        this.updatePagination();
      }
    });
  }

  get f() {
    return this.userForm.controls;
  }

  openNewUserDialog() {
    this.user = {};
    this.editMode = false;
    this.submitted = false;
    this.userForm.reset();
    this.userForm.patchValue({
      isAdmin: false,
      isActive: true
    });
    this.userDialog = true;
  }

  editUser(user: any) {
    this.editMode = true;
    this.user = {...user};
    
    // Form oluştur
    this.initForm();
    
    // Form değerlerini doldur
    this.userForm.patchValue({
      fullName: user.fullName || user.username,
      sicil: user.sicil,
      roleId: user.roleId
    });
    
    // Form'da password alanını boş bırak - opsiyonel olacak
    this.userForm.get('password')?.setValue('');
    
    this.userDialog = true;
  }

  deleteUser(user: any) {
    this.confirmationService.confirm({
      message: `${user.fullName} adlı kullanıcıyı silmek istediğinizden emin misiniz?`,
      header: 'Silme Onayı',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Evet',
      rejectLabel: 'Hayır',
      accept: () => {
        // Backend API'ye silme isteği gönder
        this.userService.deleteUser(user.id).subscribe({
          next: () => {
            // Başarılı silme işlemi sonrası
            this.users = this.users.filter(u => u.id !== user.id);
            
            // localStorage'a güncellenmiş kullanıcıları kaydet
            localStorage.setItem('users', JSON.stringify(this.users));
            
            this.applyFilters();
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Kullanıcı silindi',
              life: 3000
            });
          },
          error: (error) => {
            console.error('Kullanıcı silme hatası:', error);
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Kullanıcı silinirken bir hata oluştu.',
              life: 3000
            });
          }
        });
      }
    });
  }

  hideDialog() {
    this.userDialog = false;
    this.submitted = false;
  }

  saveUser() {
    this.submitted = true;

    if (this.userForm.invalid) {
      return;
    }

    const formData = { ...this.userForm.value };
    
    // Düzenleme modunda id değerini ekleyelim
    if (this.editMode) {
      formData.id = this.user.id; // ID'yi request body'ye ekliyoruz
      
      // Backend'in beklediği formata uygun olacak şekilde username ve isAdmin alanlarını ekle
      formData.username = this.user.username; // Mevcut kullanıcı adını koru
      formData.isAdmin = this.user.isAdmin; // Admin durumunu koru
    } else {
      // Yeni kullanıcı oluşturma
      formData.username = formData.fullName; // fullName'i username olarak kullan
      formData.isAdmin = false; // Varsayılan olarak admin değil
    }
    
    console.log('Gönderilecek veri:', formData);
    
    // Şifre kontrolü
    if (this.editMode && formData.password === '') {
      // Düzenleme modunda şifre boş ise, gönderimden çıkaralım
      const { password, ...dataToSend } = formData;
      
      this.userService.updateUser(this.user.id, dataToSend).subscribe({
        next: (response) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kullanıcı başarıyla güncellendi',
            life: 3000
          });
          this.loadUsers();
          this.hideDialog();
        },
        error: (error) => {
          console.error('Kullanıcı güncellenirken hata oluştu:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: error.message || 'Kullanıcı güncellenirken bir hata oluştu',
            life: 5000
          });
        }
      });
    } else {
      // Yeni kullanıcı oluşturma veya şifre güncellemeli düzenleme
      const saveMethod = this.editMode
        ? this.userService.updateUser(this.user.id, formData)
        : this.userService.createUser(formData);

      saveMethod.subscribe({
        next: (response) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: this.editMode
              ? 'Kullanıcı başarıyla güncellendi'
              : 'Kullanıcı başarıyla oluşturuldu',
            life: 3000
          });
          this.loadUsers();
          this.hideDialog();
        },
        error: (error) => {
          console.error('Kullanıcı işlemi sırasında hata oluştu:', error);
          
          // Hata mesajını göster (throwError ile oluşturulan Error nesnesinin message özelliği)
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: error.message || (this.editMode ? 'Kullanıcı güncellenirken bir hata oluştu' : 'Kullanıcı oluşturulurken bir hata oluştu'),
            life: 7000
          });
        }
      });
    }
  }
  
  /**
   * İzin etiketi oluşturur
   */
  getUserPermissionLabel(isAdmin: boolean, roleId: number | null): string {
    if (isAdmin) {
      return 'Admin';
    }
    
    // roleId'ye göre değerlendirme yap
    switch (roleId) {
      case 1:
        return 'Admin';
      case 2:
        return 'Kullanıcı';
      default:
        return 'Kullanıcı'; // Varsayılan
    }
  }

  generateId(): number {
    return Math.floor(Math.random() * 1000);
  }

  // Arama işlevi
  applyFilterGlobal(event: any, filterType: string) {
    this.activeFilters.search = event.target.value.toLowerCase();
    this.applyFilters();
  }

  // Rol filtreleme
  filterByRole(event: any) {
    this.activeFilters.role = event.value;
    this.applyFilters();
  }
  
  // Katılma tarihine göre filtreleme
  filterByJoined(event: any) {
    this.activeFilters.joined = event.value;
    this.applyFilters();
  }
  
  // Tüm filtreleri uygula
  applyFilters() {
    // Önce tüm kullanıcıları al
    this.filteredUsers = [...this.users];
    
    // Arama metnine göre filtrele
    if (this.searchText && this.searchText.trim() !== '') {
      const searchLower = this.searchText.toLowerCase().trim();
      this.filteredUsers = this.filteredUsers.filter(user => 
        user.fullName.toLowerCase().includes(searchLower) || 
        user.sicil.toLowerCase().includes(searchLower)
      );
    }
    
    // Eğer rol filtresi aktifse, rol filtresini de uygula
    if (this.selectedRole !== null) {
      this.applyRoleFilter({ value: this.selectedRole });
    }
  }
  
  // Sayfalama işlevleri
  updatePagination() {
    this.totalPages = Math.ceil(this.filteredUsers.length / this.rowsPerPage);
  }
  
  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }
  
  get paginatedUsers() {
    const startIndex = (this.currentPage - 1) * this.rowsPerPage;
    const endIndex = startIndex + this.rowsPerPage;
    return this.filteredUsers.slice(startIndex, endIndex);
  }
  
  changeRowsPerPage(event: any) {
    this.rowsPerPage = event.value;
    this.updatePagination();
    this.currentPage = 1;
  }

  goBack() {
    this.router.navigate(['/admin-dashboard']);
  }

  manageUserPermissions(user: any) {
    this.router.navigate([`/users/${user.id}/page-permissions`]);
  }

  // Tüm kullanıcıları temizleme metodu
  clearAllUsers() {
    this.confirmationService.confirm({
      message: 'Tüm kullanıcıları silmek istediğinizden emin misiniz?',
      header: 'Tümünü Silme Onayı',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Evet',
      rejectLabel: 'Hayır',
      accept: () => {
        // Tüm kullanıcılar için silme istekleri gönder
        const deletePromises = this.users.map(user => {
          return new Promise<void>((resolve, reject) => {
            this.userService.deleteUser(user.id).subscribe({
              next: () => resolve(),
              error: (err) => {
                console.error(`Kullanıcı silme hatası (ID: ${user.id}):`, err);
                reject(err);
              }
            });
          });
        });

        // Tüm silme istekleri tamamlandığında
        Promise.all(deletePromises)
          .then(() => {
            this.users = [];
            this.filteredUsers = [];
            
            // localStorage'a boş kullanıcı dizisini kaydet
            localStorage.setItem('users', JSON.stringify(this.users));
            
            this.updatePagination();
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Tüm kullanıcılar silindi',
              life: 3000
            });
          })
          .catch(error => {
            console.error('Toplu silme işlemi sırasında hata:', error);
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Bazı kullanıcılar silinirken hata oluştu.',
              life: 3000
            });
            // Hata olsa bile güncel listeyi yeniden yükle
            this.loadUsers();
          });
      }
    });
  }

  loadRoles() {
    // Backend'den rolleri çekelim
    this.roleService.getRoles().subscribe({
      next: (roles) => {
        if (roles && roles.length > 0) {
          console.log('Roller yüklendi:', roles);
          
          // Rolleri dropdown için formatlayalım
          this.roles = roles.map(role => ({
            label: role.name,
            value: role.id
          }));
          
          // Rol filtre seçeneklerini güncelle
          this.roleFilterOptions = [
            { label: 'Tümü', value: null },
            ...this.roles
          ];
        } else {
          console.log('Backend\'den rol verisi alınamadı veya boş.');
          
          // Varsayılan roller
          this.roles = [
            { label: 'Yönetici', value: 1, name: 'Yönetici' },
            { label: 'Kullanıcı', value: 2, name: 'Kullanıcı' }
          ];
          
          // Rol filtre seçeneklerini güncelle
          this.roleFilterOptions = [
            { label: 'Tümü', value: null },
            ...this.roles.map(role => ({
              label: role.label,
              value: role.value
            }))
          ];
        }
      },
      error: (error) => {
        console.error('Rolleri yükleme hatası:', error);
        
        // Hata durumunda varsayılan rolleri kullan
        this.roles = [
          { label: 'Yönetici', value: 1, name: 'Yönetici' },
          { label: 'Kullanıcı', value: 2, name: 'Kullanıcı' }
        ];
        
        // Rol filtre seçeneklerini güncelle
        this.roleFilterOptions = [
          { label: 'Tümü', value: null },
          ...this.roles.map(role => ({
            label: role.label,
            value: role.value
          }))
        ];
      }
    });
  }

  // Rol filtresini uygula
  applyRoleFilter(event: any) {
    const selectedRoleId = event.value;
    
    if (selectedRoleId === null) {
      // Tüm roller seçiliyse filtreleme yapma
      this.applyFilters();
    } else {
      // Seçilen role göre filtrele
      this.filteredUsers = this.users.filter(user => {
        // Admin kullanıcılar için özel kontrol
        if (user.permissions === 'Admin' && selectedRoleId === 1) {
          return true;
        }
        
        // Diğer roller için kontrol
        if (user.roleId === selectedRoleId) {
          return true;
        }
        
        // Permissions değerine göre kontrol
        if (selectedRoleId === 2 && (user.permissions === 'Kullanıcı' || user.permissions === 'Contributor')) {
          return true;
        }
        
        return false;
      });
    }
  }
}
