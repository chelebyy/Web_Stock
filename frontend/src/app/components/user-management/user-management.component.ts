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
import { UserService } from '../../services/user.service';
import { RoleService } from '../../services/role.service';
import { User } from '../../models/user.model';
import { Role, RoleWithUsers } from '../../models/role.model';
import { Router } from '@angular/router';

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
    CheckboxModule,
    ConfirmDialogModule,
    DialogModule,
    InputTextModule,
    PasswordModule,
    TableModule,
    ToastModule,
    ToolbarModule,
    DropdownModule,
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
  
  // Filtre seçenekleri
  roleFilterOptions: any[] = [
    { label: 'Tümü', value: null },
    { label: 'Yönetici', value: 'Admin' },
    { label: 'Kullanıcı', value: 'Contributor' }
  ];
  
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
    private userService: UserService
  ) {}

  ngOnInit() {
    this.initForm();
    this.loadUsers();
  }

  initForm() {
    this.userForm = this.formBuilder.group({
      id: [null],
      sicil: ['', [Validators.required]],
      fullName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      isAdmin: [false],
      isActive: [true]
    });
  }

  loadUsers() {
    // Backend'den kullanıcıları çekelim
    this.userService.getUsers().subscribe({
      next: (users) => {
        if (users && users.length > 0) {
          // Backend'den gelen kullanıcıları formatlayalım
          this.users = users.map(user => ({
            id: user.id,
            sicil: user.sicil || '',
            fullName: user.username,
            permissions: user.isAdmin ? 'Admin' : user.roleId ? 'Contributor' : 'Viewer',
            isActive: true,
            joinDate: new Date(user.createdAt || new Date()).toLocaleDateString('tr-TR', { year: 'numeric', month: 'long', day: 'numeric' }),
            avatar: 'default-avatar.png'
          }));
          
          // Kullanıcıları localStorage'a kaydedelim
          localStorage.setItem('users', JSON.stringify(this.users));
          
          this.filteredUsers = [...this.users];
          this.updatePagination();
        } else {
          console.log('Backend\'den kullanıcı verisi alınamadı veya boş.');
          
          // Eğer backend'den veri gelmezse, localStorage'dan yüklemeyi deneyelim
          const storedUsers = localStorage.getItem('users');
          if (storedUsers) {
            this.users = JSON.parse(storedUsers);
            this.filteredUsers = [...this.users];
            this.updatePagination();
          }
        }
      },
      error: (error) => {
        console.error('Kullanıcıları yükleme hatası:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcılar yüklenirken bir hata oluştu.',
          life: 3000
        });
        
        // Hata durumunda localStorage'dan yüklemeyi deneyelim
        const storedUsers = localStorage.getItem('users');
        if (storedUsers) {
          this.users = JSON.parse(storedUsers);
          this.filteredUsers = [...this.users];
          this.updatePagination();
        }
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
    this.user = { ...user };
    this.editMode = true;
    this.userForm.patchValue({
      id: this.user.id,
      sicil: this.user.email,
      fullName: this.user.fullName,
      isAdmin: this.user.permissions === 'Admin',
      isActive: this.user.isActive
    });
    
    // Şifre alanını düzenleme modunda zorunlu olmaktan çıkar
    const passwordControl = this.userForm.get('password');
    if (passwordControl) {
      passwordControl.clearValidators();
      passwordControl.updateValueAndValidity();
    }
    
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

    const formValues = this.userForm.value;
    
    if (this.editMode) {
      // Kullanıcı güncelleme
      const index = this.users.findIndex(u => u.id === formValues.id);
      if (index !== -1) {
        // Backend'e gönderilecek kullanıcı nesnesi
        const userToUpdate: any = {
          id: formValues.id,
          username: formValues.fullName,
          isAdmin: formValues.isAdmin,
          sicil: formValues.sicil
        };
        
        // Backend API'ye güncelleme isteği gönder
        this.userService.updateUser(formValues.id, userToUpdate).subscribe({
          next: () => {
            // Başarılı güncelleme sonrası
            this.users[index] = {
              ...this.users[index],
              sicil: formValues.sicil,
              fullName: formValues.fullName,
              permissions: formValues.isAdmin ? 'Admin' : 'Contributor',
              isActive: formValues.isActive
            };
            
            // localStorage'a güncellenmiş kullanıcıları kaydet
            localStorage.setItem('users', JSON.stringify(this.users));
            
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Kullanıcı güncellendi',
              life: 3000
            });
            
            this.applyFilters();
            this.userDialog = false;
            this.user = {};
          },
          error: (error) => {
            console.error('Kullanıcı güncelleme hatası:', error);
            
            // API'dan dönen hata mesajını göster (varsa)
            const errorMessage = error.error?.error || 'Kullanıcı güncellenirken bir hata oluştu.';
            
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: errorMessage,
              life: 5000
            });
          }
        });
      }
    } else {
      // Yeni kullanıcı ekleme
      // Backend'e gönderilecek kullanıcı nesnesi
      const userToCreate: any = {
        username: formValues.fullName,
        password: formValues.password,
        isAdmin: formValues.isAdmin,
        sicil: formValues.sicil
      };
      
      console.log('Form değerleri:', formValues);
      console.log('Oluşturulacak kullanıcı:', userToCreate);
      
      // Backend API'ye ekleme isteği gönder
      this.userService.createUser(userToCreate).subscribe({
        next: (response) => {
          // Başarılı ekleme sonrası
          const newUser = {
            id: response.id || this.generateId(),
            sicil: formValues.sicil,
            fullName: formValues.fullName,
            location: 'İstanbul,TR',
            joinDate: new Date().toLocaleDateString('tr-TR', { year: 'numeric', month: 'long', day: 'numeric' }),
            permissions: formValues.isAdmin ? 'Admin' : 'Contributor',
            isActive: formValues.isActive,
            avatar: 'default-avatar.png'
          };
          
          this.users.unshift(newUser);
          
          // localStorage'a güncellenmiş kullanıcıları kaydet
          localStorage.setItem('users', JSON.stringify(this.users));
          
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kullanıcı eklendi',
            life: 3000
          });
          
          this.applyFilters();
          this.userDialog = false;
          this.user = {};
        },
        error: (error) => {
          console.error('Kullanıcı ekleme hatası:', error);
          
          // API'dan dönen hata mesajını göster (varsa)
          const errorMessage = error.error?.error || 'Kullanıcı eklenirken bir hata oluştu.';
          
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: errorMessage,
            life: 5000
          });
        }
      });
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
    let filtered = [...this.users];
    
    // Arama filtresi
    if (this.activeFilters.search) {
      filtered = filtered.filter(user => 
        user.fullName.toLowerCase().includes(this.activeFilters.search) ||
        user.sicil.toLowerCase().includes(this.activeFilters.search)
      );
    }
    
    // Rol filtresi
    if (this.activeFilters.role) {
      filtered = filtered.filter(user => user.permissions === this.activeFilters.role);
    }
    
    // Katılma tarihi filtresi
    if (this.activeFilters.joined) {
      const currentYear = new Date().getFullYear();
      const currentMonth = new Date().getMonth();
      
      filtered = filtered.filter(user => {
        const joinDate = new Date(user.joinDate);
        
        switch (this.activeFilters.joined) {
          case 'thisMonth':
            return joinDate.getMonth() === currentMonth && joinDate.getFullYear() === currentYear;
          case 'thisYear':
            return joinDate.getFullYear() === currentYear;
          case 'before2020':
            return joinDate.getFullYear() < 2020;
          default:
            return true;
        }
      });
    }
    
    this.filteredUsers = filtered;
    this.currentPage = 1;
    this.updatePagination();
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
}
