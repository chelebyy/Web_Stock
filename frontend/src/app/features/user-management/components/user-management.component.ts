import { Component, OnInit, AfterViewInit } from '@angular/core';
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
import { HostListener } from '@angular/core';
import { DeleteConfirmationDialogComponent } from '../../../features/shared/components/delete-confirmation-dialog/delete-confirmation-dialog.component';
import { firstValueFrom } from 'rxjs';

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
        TooltipModule,
        DeleteConfirmationDialogComponent
    ],
    providers: [MessageService, ConfirmationService],
    styles: [`
    .modern-dialog {
      border-radius: 12px;
      box-shadow: 0 6px 16px rgba(0, 0, 0, 0.1);
      overflow: hidden;
    }
    
    .modern-dialog .p-dialog-header {
      background-color: #ffffff;
      padding: 1.5rem 2rem 0.5rem;
      border-bottom: none;
    }
    
    .modern-dialog .p-dialog-title {
      font-size: 1.25rem;
      font-weight: 600;
      color: #333;
    }
    
    .modern-dialog .p-dialog-content {
      background-color: #ffffff;
      padding: 2rem;
    }
    
    .dialog-content {
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
    }
    
    .field {
      margin-bottom: 1.5rem;
    }
    
    .input-with-icon {
      position: relative;
      width: 100%;
      display: block;
    }
    
    .input-icon {
      position: absolute;
      left: 14px;
      top: 50%;
      transform: translateY(-50%);
      color: #6c757d;
      font-size: 1.1rem;
      z-index: 5;
    }
    
    .input-with-icon .modern-input,
    .input-with-icon .modern-select {
      padding-left: 42px;
      width: 100%;
      height: 48px;
      border-radius: 8px;
      border: 1px solid #ced4da;
      transition: all 0.3s;
      font-size: 1rem;
      color: #333;
      background-color: #ffffff;
    }
    
    .modern-input:focus,
    .modern-select:focus {
      border-color: #2196F3;
      box-shadow: 0 0 0 2px rgba(33, 150, 243, 0.2);
    }
    
    .modern-input:hover:not(:focus):not(:disabled),
    .modern-select:hover:not(:focus):not(:disabled) {
      border-color: #bbdefb;
    }
    
    .modern-select {
      appearance: none;
      background-image: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="%236c757d" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M6 9l6 6 6-6"/></svg>');
      background-repeat: no-repeat;
      background-position: right 14px center;
      padding-right: 40px;
    }
    
    .p-error {
      color: #f44336;
      font-size: 0.875rem;
      margin-top: 0.25rem;
      display: block;
    }
    
    .p-help {
      color: #6c757d;
      font-size: 0.875rem;
      margin-top: 0.25rem;
      display: flex;
      align-items: center;
    }
    
    .dialog-footer {
      display: flex;
      justify-content: flex-end;
      gap: 0.75rem;
      padding: 1rem 2rem 1.5rem;
      background-color: #ffffff;
    }
    
    .cancel-button, .save-button {
      min-width: 120px;
      height: 40px;
      border-radius: 8px;
      font-weight: 500;
      transition: all 0.3s;
    }
    
    .cancel-button:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }
    
    .save-button:disabled {
      opacity: 0.6;
      cursor: not-allowed;
    }
    
    /* Dropdown özel stilleri */
    .modern-dropdown {
      width: 100% !important;
      height: 48px !important;
      position: relative !important;
    }
    
    .modern-dropdown .p-dropdown {
      width: 100% !important;
      height: 48px !important;
      border-radius: 8px !important;
      background-color: #ffffff !important;
      border: 1px solid #ced4da !important;
      display: flex !important;
      align-items: center !important;
    }
    
    .modern-dropdown .p-dropdown-label {
      padding-left: 42px !important;
      display: flex !important;
      align-items: center !important;
      color: #333333 !important;
      font-size: 1rem !important;
      font-weight: normal !important;
      height: 100% !important;
      background-color: transparent !important;
      z-index: 1 !important;
      position: relative !important;
      visibility: visible !important;
      opacity: 1 !important;
    }
    
    .modern-dropdown .p-dropdown-label.p-placeholder {
      color: #6c757d !important;
      visibility: visible !important;
      opacity: 1 !important;
      display: flex !important;
      align-items: center !important;
    }
    
    .modern-dropdown .p-dropdown-trigger {
      color: #6c757d !important;
      width: 3rem !important;
      z-index: 2 !important;
    }
    
    .input-with-icon .p-dropdown {
      background-color: #ffffff !important;
      position: relative !important;
    }
    
    .input-with-icon .p-dropdown .p-dropdown-label {
      padding-left: 42px !important;
      color: #333333 !important;
      font-size: 1rem !important;
      background-color: #ffffff !important;
      visibility: visible !important;
      opacity: 1 !important;
    }
    
    .input-with-icon .p-dropdown .p-dropdown-label.p-placeholder {
      color: #6c757d !important;
      visibility: visible !important;
      opacity: 1 !important;
    }
    
    /* Dropdown panel stilleri */
    .p-dropdown-panel {
      background-color: #ffffff !important;
      border: 1px solid #ced4da !important;
      border-radius: 8px !important;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
      z-index: 1000 !important;
    }
    
    .p-dropdown-panel .p-dropdown-items {
      padding: 0.5rem 0 !important;
      background-color: #ffffff !important;
    }
    
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item {
      padding: 0.75rem 1rem !important;
      color: #333333 !important;
      font-size: 1rem !important;
      border-bottom: 1px solid rgba(0, 0, 0, 0.05) !important;
      background-color: #ffffff !important;
    }
    
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover {
      background-color: #f8f9fa !important;
      color: #000000 !important;
    }
    
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight {
      background-color: rgba(59, 130, 246, 0.1) !important;
      color: #2563eb !important;
      font-weight: 600 !important;
    }
    
    /* Yüksek öncelikli dropdown panel seçicileri */
    html body .p-dropdown-panel,
    body .p-dropdown-panel,
    .p-dropdown-panel,
    html body .p-dropdown-items-wrapper,
    body .p-dropdown-items-wrapper,
    .p-dropdown-items-wrapper {
      background-color: #ffffff !important;
      border: 1px solid #ced4da !important;
      border-radius: 8px !important;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
    }
    
    html body .p-dropdown-panel .p-dropdown-items,
    body .p-dropdown-panel .p-dropdown-items,
    .p-dropdown-panel .p-dropdown-items,
    html body .p-dropdown-items-wrapper .p-dropdown-items,
    body .p-dropdown-items-wrapper .p-dropdown-items,
    .p-dropdown-items-wrapper .p-dropdown-items {
      background-color: #ffffff !important;
      padding: 0.5rem 0 !important;
    }
    
    html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item,
    body .p-dropdown-panel .p-dropdown-items .p-dropdown-item,
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item,
    html body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item,
    body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item,
    .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item {
      padding: 0.75rem 1rem !important;
      color: #333333 !important;
      font-size: 1rem !important;
      border-bottom: 1px solid rgba(0, 0, 0, 0.05) !important;
      background-color: #ffffff !important;
    }
    
    html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover,
    body .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover,
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover,
    html body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item:hover,
    body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item:hover,
    .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item:hover {
      background-color: #f8f9fa !important;
      color: #000000 !important;
    }
    
    html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight,
    body .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight,
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight,
    html body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item.p-highlight,
    body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item.p-highlight,
    .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item.p-highlight {
      background-color: rgba(59, 130, 246, 0.1) !important;
      color: #2563eb !important;
      font-weight: 600 !important;
    }
    
    /* Yüksek öncelikli input seçicileri */
    html body .input-with-icon .p-dropdown .p-dropdown-label,
    body .input-with-icon .p-dropdown .p-dropdown-label,
    .input-with-icon .p-dropdown .p-dropdown-label {
      padding-left: 42px !important;
      color: #333333 !important;
      font-size: 1rem !important;
      visibility: visible !important;
      opacity: 1 !important;
      display: flex !important;
      align-items: center !important;
    }
    
    html body .input-with-icon .p-dropdown .p-dropdown-label.p-placeholder,
    body .input-with-icon .p-dropdown .p-dropdown-label.p-placeholder,
    .input-with-icon .p-dropdown .p-dropdown-label.p-placeholder {
      color: #6c757d !important;
      visibility: visible !important;
      opacity: 1 !important;
      display: flex !important;
      align-items: center !important;
    }
  `]
})
export class UserManagementComponent implements OnInit, AfterViewInit {
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
  
  // Checkbox durumları için değişkenler
  selectAllChecked: boolean = false;
  selectedUsers: { [key: number]: boolean } = {};
  
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

  // Dropdown açma kapama state'leri
  roleDropdownOpen: boolean = false;
  rowsDropdownOpen: boolean = false;
  roleDialogDropdownOpen: boolean = false;

  // Silme dialogu için yeni değişkenler
  deleteDialogVisible: boolean = false;
  userToDelete: any = null;

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
    console.log('UserManagementComponent initialized');
    
    // Önce rolleri yükle
    this.loadRoles();
    
    // Kullanıcıları yükle - artık loadRoles içinde çağrılıyor
    // this.loadUsers();
    
    // Form başlatma
    this.initForm();
    
    // Placeholder görünürlüğünü artır
    this.enhancePlaceholderVisibility();
    
    // Döküman tıklamalarını dinle (dropdown'ları kapatmak için)
    document.addEventListener('click', this.onDocumentClick.bind(this));
  }

  ngAfterViewInit() {
    // PrimeNG 19 dropdown düzeltmesi
    setTimeout(() => {
      // Dropdown panelinin genişliğini ayarla
      const dropdown = document.querySelector('.user-role-dropdown') as HTMLElement;
      if (dropdown) {
        const width = dropdown.offsetWidth;
        dropdown.style.setProperty('--dropdown-width', `${width}px`);
        
        // Dropdown açıldığında panel genişliğini ayarla
        const observer = new MutationObserver((mutations) => {
          mutations.forEach((mutation) => {
            if (mutation.type === 'attributes' && mutation.attributeName === 'class') {
              const panel = document.querySelector('.user-role-dropdown-panel') as HTMLElement;
              if (panel) {
                panel.style.width = `${width}px`;
                panel.style.minWidth = `${width}px`;
                panel.style.backgroundColor = '#ffffff';
              }
            }
          });
        });
        
        observer.observe(dropdown, { attributes: true });
      }
    }, 0);

    // Placeholder metinlerinin görünürlüğünü artır
    setTimeout(() => {
      this.enhancePlaceholderVisibility();
    }, 100);
  }

  // Placeholder metinlerinin görünürlüğünü artıran fonksiyon
  private enhancePlaceholderVisibility() {
    // Şifre alanı
    const passwordInput = document.getElementById('password') as HTMLInputElement;
    if (passwordInput) {
      passwordInput.style.color = '#333333';
      passwordInput.addEventListener('focus', () => {
        passwordInput.style.color = '#333333';
      });
      passwordInput.addEventListener('blur', () => {
        passwordInput.style.color = passwordInput.value ? '#333333' : '#333333';
      });
    }
    
    // Kullanıcı rolü alanı
    const roleSelect = document.getElementById('roleId') as HTMLSelectElement;
    if (roleSelect) {
      roleSelect.style.color = '#333333';
      
      // Seçim yapıldığında rengi güncelle
      roleSelect.addEventListener('change', () => {
        roleSelect.style.color = roleSelect.value ? '#333333' : '#333333';
        
        // Seçeneklerin rengini güncelle
        Array.from(roleSelect.options).forEach(option => {
          option.style.color = '#333333';
          if (option.selected && option.value) {
            option.style.color = '#2563eb';
            option.style.fontWeight = '600';
          }
        });
      });
      
      // İlk yüklemede rengi ayarla
      roleSelect.style.color = roleSelect.value ? '#333333' : '#333333';
    }
  }

  initForm() {
    // Temel form alanları
    this.userForm = this.formBuilder.group({
      fullName: ['', Validators.required],
      sicil: ['', Validators.required],
      roleId: [2, Validators.required]  // Varsayılan olarak "Kullanıcı" rolünü seçiyoruz (id: 2)
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
        
        if (data && Array.isArray(data)) {
          this.users = data.map(user => {
            console.log('Kullanıcı ID tipi:', typeof user.id, 'Değer:', user.id);
            console.log('Kullanıcı rol bilgisi:', user.roleId, 'Rol adı:', this.getRoleName(user.roleId || null));
            
            // fullName alanını oluştur
            const fullName = user.firstName && user.lastName 
              ? `${user.firstName} ${user.lastName}` 
              : user.username;
            
            return {
              id: user.id,
              fullName: fullName,
              username: user.username,
              sicil: user.sicil,
              roleId: user.roleId,
              roleName: this.getRoleName(user.roleId || null),
              permissions: this.getUserPermissionLabel(user.isAdmin, user.roleId || null),
              isAdmin: user.isAdmin
            };
          });
          
          // Kullanıcılar yüklendiğinde selectedUsers nesnesini sıfırla
          this.clearSelectedUsers();
          
          console.log('Kullanıcı verileri işlendi:', this.users);
          this.applyFilters();
        } else {
          console.error('Geçersiz kullanıcı verisi:', data);
          this.users = [];
          this.filteredUsers = [];
        }
      },
      error: (error) => {
        console.error('Kullanıcı verileri alınırken hata oluştu:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcı verileri alınırken bir hata oluştu'
        });
        this.users = [];
        this.filteredUsers = [];
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
      isActive: true,
      roleId: 2  // Varsayılan olarak "Kullanıcı" rolünü seçiyoruz (id: 2)
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
    
    // Konsola bilgi yazdır
    console.log('Düzenlenecek kullanıcı:', user);
    console.log('Form değerleri:', this.userForm.value);
  }

  deleteUser(user: any) {
    this.userToDelete = user;
    this.deleteDialogVisible = true;
  }

  confirmDelete() {
    if (!this.userToDelete) return;
    
    if (this.userToDelete.id === 'all') {
      // Seçili kullanıcıları sil
      const selectedUserIds = Object.keys(this.selectedUsers)
        .filter(id => this.selectedUsers[parseInt(id)])
        .map(id => parseInt(id));
      
      if (selectedUserIds.length === 0) {
        this.messageService.add({
          severity: 'warn',
          summary: 'Uyarı',
          detail: 'Lütfen silmek için en az bir kullanıcı seçin'
        });
        this.deleteDialogVisible = false;
        return;
      }
      
      // Burada toplu silme işlemi yapılabilir
      // Örnek olarak her bir kullanıcıyı tek tek siliyoruz
      let successCount = 0;
      let errorCount = 0;
      
      const deletePromises = selectedUserIds.map(userId => 
        firstValueFrom(this.userService.deleteUser(userId))
          .then(() => { successCount++; })
          .catch(() => { errorCount++; })
      );
      
      Promise.all(deletePromises).then(() => {
        if (successCount > 0) {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: `${successCount} kullanıcı başarıyla silindi${errorCount > 0 ? `, ${errorCount} kullanıcı silinemedi` : ''}`
          });
        } else if (errorCount > 0) {
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: `${errorCount} kullanıcı silinemedi`
          });
        }
        
        this.loadUsers();
        this.clearSelectedUsers();
        this.deleteDialogVisible = false;
        this.userToDelete = null;
      });
    } else {
      // Tek kullanıcı silme işlemi (mevcut kod)
      this.userService.deleteUser(this.userToDelete.id).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kullanıcı silindi'
          });
          this.loadUsers();
          this.deleteDialogVisible = false;
          this.userToDelete = null;
        },
        error: (error) => {
          console.error('Error deleting user:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcı silinirken bir hata oluştu'
          });
          this.deleteDialogVisible = false;
          this.userToDelete = null;
        }
      });
    }
  }
  
  cancelDelete() {
    this.userToDelete = null;
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
      
      // Kullanıcı adını fullName olarak güncelle
      formData.username = formData.fullName;
      
      // Admin kullanıcısı için özel durum - admin kullanıcı adını "Admin" olarak güncelle
      if (this.user.isAdmin && this.user.username && this.user.username.toLowerCase() === 'admin') {
        formData.username = 'Admin'; // Admin kullanıcı adını büyük harfle başlat
      }
      
      formData.isAdmin = this.user.isAdmin; // Admin durumunu koru
      
      // Sicil alanının doğru şekilde gönderildiğinden emin olalım
      if (!formData.sicil || formData.sicil.trim() === '') {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Sicil numarası boş olamaz',
          life: 5000
        });
        return;
      }
      
      // Sicil numarasının benzersiz olup olmadığını kontrol et
      // Düzenleme modunda, kullanıcının kendi sicil numarasını değiştirmediği durumda hata vermeyelim
      const existingUserWithSameSicil = this.users.find(u => u.sicil === formData.sicil && u.id !== formData.id);
      if (existingUserWithSameSicil) {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `"${formData.sicil}" sicil numarası zaten kullanımda. Sicil numarası benzersiz olmalıdır.`,
          life: 5000
        });
        return;
      }
      
      // roleId'nin doğru şekilde gönderildiğinden emin olalım
      if (!formData.roleId || formData.roleId <= 0) {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Rol seçilmelidir',
          life: 5000
        });
        return;
      }
      
      console.log('Güncellenecek kullanıcı verisi:', formData);
    } else {
      // Yeni kullanıcı oluşturma
      // Kullanıcı adı olarak fullName kullan
      formData.username = formData.fullName;
      
      formData.isAdmin = false; // Varsayılan olarak admin değil
      
      // Sicil alanının doğru şekilde gönderildiğinden emin olalım
      if (!formData.sicil || formData.sicil.trim() === '') {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Sicil numarası boş olamaz',
          life: 5000
        });
        return;
      }
      
      // Sicil numarasının benzersiz olup olmadığını kontrol et
      const existingUserWithSameSicil = this.users.find(u => u.sicil === formData.sicil);
      if (existingUserWithSameSicil) {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `"${formData.sicil}" sicil numarası zaten kullanımda. Sicil numarası benzersiz olmalıdır.`,
          life: 5000
        });
        return;
      }
      
      // RoleId alanının doğru şekilde gönderildiğinden emin olalım
      if (!formData.roleId || formData.roleId <= 0) {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Rol seçilmelidir',
          life: 5000
        });
        return;
      }
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
          
          // Hata mesajını daha detaylı göster
          let errorMessage = 'Kullanıcı güncellenirken bir hata oluştu';
          
          if (error.error && error.error.error) {
            errorMessage = error.error.error;
          } else if (error.message) {
            errorMessage = error.message;
          }
          
          // Sunucu hatası için daha detaylı bilgi
          if (error.status === 500) {
            if (error.error && error.error.details) {
              errorMessage = `Sunucu hatası: ${error.error.details}`;
            } else {
              errorMessage = 'Sunucu tarafında bir hata oluştu. Lütfen daha sonra tekrar deneyin.';
            }
          }
          
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: errorMessage,
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
          
          // Hata mesajını daha detaylı göster
          let errorMessage = this.editMode 
            ? 'Kullanıcı güncellenirken bir hata oluştu' 
            : 'Kullanıcı oluşturulurken bir hata oluştu';
          
          if (error.error && error.error.error) {
            errorMessage = error.error.error;
          } else if (error.message) {
            errorMessage = error.message;
          }
          
          // Sunucu hatası için daha detaylı bilgi
          if (error.status === 500) {
            if (error.error && error.error.details) {
              errorMessage = `Sunucu hatası: ${error.error.details}`;
            } else {
              errorMessage = 'Sunucu tarafında bir hata oluştu. Lütfen daha sonra tekrar deneyin.';
            }
          }
          
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: errorMessage,
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
    if (isAdmin) return 'Admin';
    if (!roleId) return 'Kullanıcı';
    
    const role = this.roles.find(r => r.value === roleId);
    return role ? role.label : 'Kullanıcı';
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
    if (this.selectedRole !== null && this.selectedRole.value !== null) {
      this.filteredUsers = this.applyRoleFilter(this.selectedRole.value);
    }
    
    // Sayfalama bilgilerini güncelle
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

  manageUserPermissions(user: any) {
    this.router.navigate([`/user-management/users/${user.id}/page-permissions`]);
  }

  // Tüm kullanıcıları temizleme metodu
  clearAllUsers() {
    this.userToDelete = { id: 'all', fullName: 'tüm kullanıcıları' };
    this.deleteDialogVisible = true;
  }

  loadRoles() {
    // Role Service ile gerçek rollerini yüklemek burada yapılacak
    // Şimdilik statik veriler kullanıyoruz
    this.roleService.getRoles().subscribe({
      next: (roles) => {
        console.log('API\'den gelen roller:', roles);
        
        // API'den gelen roller için
        this.roles = roles.map(role => {
          console.log('İşlenen rol:', role);
          return {
            label: role.name,
            value: role.id
          };
        });
        
        // Rol filtre seçenekleri için "Tümü" seçeneğini ekle
        this.roleFilterOptions = [
          { label: 'Tümü', value: null },
          ...this.roles
        ];
        
        console.log('Roller yüklendi:', this.roles);
        console.log('Rol filtre seçenekleri:', this.roleFilterOptions);
        
        // Rolleri yükledikten sonra kullanıcıları yeniden yükle
        // Bu, kullanıcıların doğru rol bilgilerini almasını sağlar
        this.loadUsers();
      },
      error: (err) => {
        // Hata durumunda
        console.error('Roller yüklenirken hata oluştu:', err);
        
        // Hata durumunda varsayılan roller
        this.roles = [
          { label: 'Admin', value: 1 },
          { label: 'Kullanıcı', value: 2 }
        ];
        
        // Rol filtre seçenekleri için "Tümü" seçeneğini ekle
        this.roleFilterOptions = [
          { label: 'Tümü', value: null },
          ...this.roles
        ];
        
        console.log('Varsayılan roller yüklendi:', this.roles);
        console.log('Varsayılan rol filtre seçenekleri:', this.roleFilterOptions);
      }
    });
  }

  // Rol filtresini uygula
  applyRoleFilter(roleId: number) {
    console.log('Rol filtresi uygulanıyor:', roleId);
    
    if (roleId === null) {
      // Tüm roller seçiliyse filtreleme yapma
      return [...this.users];
    } else {
      // Seçilen role göre filtrele
      return this.users.filter(user => {
        // Admin kullanıcılar için özel kontrol
        if (user.permissions === 'Admin' && roleId === 1) {
          return true;
        }
        
        // Diğer roller için kontrol
        if (user.roleId === roleId) {
          return true;
        }
        
        // Permissions değerine göre kontrol
        if (roleId === 2 && (user.permissions === 'Kullanıcı' || user.permissions === 'Contributor')) {
          return true;
        }
        
        return false;
      });
    }
  }

  /**
   * Seçilen rol değerinin etiketini döndürür
   * Angular 19 PrimeNG dropdown görünürlük sorunu için çözüm
   */
  getSelectedRoleLabel(): string {
    if (this.selectedRole) {
      // roleFilterOptions içinde seçilen değerin etiketini bul
      const option = this.roleFilterOptions.find(opt => opt.value === this.selectedRole.value);
      return option ? option.label : this.selectedRole.label || '';
    }
    return 'Tümü';
  }

  // Rol dropdown'unu aç/kapat
  toggleRoleDropdown() {
    this.roleDropdownOpen = !this.roleDropdownOpen;
    // Diğer açık dropdownları kapat
    if (this.roleDropdownOpen) {
      this.rowsDropdownOpen = false;
      this.roleDialogDropdownOpen = false;
    }
  }

  // Sayfa başına satır dropdown'unu aç/kapat
  toggleRowsDropdown() {
    this.rowsDropdownOpen = !this.rowsDropdownOpen;
    // Diğer açık dropdownları kapat
    if (this.rowsDropdownOpen) {
      this.roleDropdownOpen = false;
      this.roleDialogDropdownOpen = false;
    }
  }

  // Dialog içindeki rol dropdown'unu aç/kapat
  toggleRoleDialogDropdown() {
    this.roleDialogDropdownOpen = !this.roleDialogDropdownOpen;
    // Diğer açık dropdownları kapat
    if (this.roleDialogDropdownOpen) {
      this.roleDropdownOpen = false;
      this.rowsDropdownOpen = false;
    }
  }

  /**
   * Rol seçimi yapar ve dropdown'ı kapatır
   */
  selectRole(role: any) {
    this.selectedRole = role;
    this.roleDropdownOpen = false;
    // Rol değişikliğini uygula
    if (role === null) {
      this.selectedRole = null;
    } else {
      this.selectedRole = role;
    }
    
    console.log('Rol seçildi:', this.selectedRole);
    this.currentPage = 1; // İlk sayfaya dön
    this.applyFilters(); // Filtreleri uygula
  }

  // Sayfa başına satır seç ve dropdown'u kapat
  selectRowsPerPage(rows: number) {
    this.rowsPerPage = rows;
    this.rowsDropdownOpen = false;
    // Satır sayısı değişikliğini uygula
    this.changeRowsPerPage({ value: rows });
  }

  // Dialog içinde rol seç
  selectRoleInDialog(role: any) {
    this.userForm.patchValue({
      roleId: role.value
    });
    this.roleDialogDropdownOpen = false;
  }

  // Dialog içinde seçilen rolü göster
  getSelectedRoleInDialog(): string {
    const roleId = this.userForm?.get('roleId')?.value;
    if (roleId) {
      const selectedRole = this.roles.find(r => r.value === roleId);
      return selectedRole ? selectedRole.label : '';
    }
    return 'Rol seçin';
  }

  // Dropdown dışına tıklandığında dropdownları kapat
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    // Eğer tıklanan element dropdown içinde değilse dropdown'u kapat
    const target = event.target as HTMLElement;
    
    // Rol dropdown'u için kontrol
    if (this.roleDropdownOpen && !target.closest('.custom-dropdown')) {
      this.roleDropdownOpen = false;
    }
    
    // Sayfa başına satır dropdown'u için kontrol
    if (this.rowsDropdownOpen && !target.closest('.rows-dropdown')) {
      this.rowsDropdownOpen = false;
    }
    
    // Dialog rol dropdown'u için kontrol
    if (this.roleDialogDropdownOpen && !target.closest('.dialog-dropdown')) {
      this.roleDialogDropdownOpen = false;
    }
  }

  // Tümü seçeneği var mı kontrolü
  get hasTumuOption(): boolean {
    return this.roleFilterOptions.some(role => role.label === 'Tümü' || role.value === null);
  }

  /**
   * Satır sayısı seçiminin değişimini işler
   */
  onRowsPerPageChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const value = Number(selectElement.value);
    this.rowsPerPage = value;
    this.currentPage = 1; // Sayfa değiştiğinde ilk sayfaya dön
    this.applyFilters(); // Filtreleri uygula, pagination'ı da güncelleyecek
  }

  /**
   * Rol filtresi seçiminin değişimini işler
   */
  onRoleFilterChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const value = selectElement.value;
    
    if (value === "") {
      // "Tümü" seçildi, filtre olmayacak
      this.selectedRole = null;
    } else {
      // Seçilen değere göre roleFilterOptions'dan rolü bul
      const numericValue = Number(value);
      const selectedRole = this.roleFilterOptions.find(role => role.value === numericValue);
      if (selectedRole) {
        this.selectedRole = selectedRole;
      }
    }
    
    console.log('Seçilen rol:', this.selectedRole);
    this.currentPage = 1; // İlk sayfaya dön
    this.applyFilters(); // Filtreleri uygula
  }

  // Checkbox metodları
  toggleSelectAll() {
    console.log('toggleSelectAll çağrıldı, önceki durum:', this.selectAllChecked);
    this.selectAllChecked = !this.selectAllChecked;
    console.log('toggleSelectAll sonrası durum:', this.selectAllChecked);
    
    // Tüm kullanıcıların checkbox durumunu güncelle
    this.filteredUsers.forEach(user => {
      this.selectedUsers[user.id] = this.selectAllChecked;
    });
    
    console.log('Seçili kullanıcılar:', this.selectedUsers);
  }
  
  toggleUserSelection(userId: number) {
    console.log('toggleUserSelection çağrıldı, kullanıcı ID:', userId);
    console.log('Önceki durum:', this.selectedUsers[userId]);
    
    // Kullanıcının checkbox durumunu tersine çevir
    this.selectedUsers[userId] = !this.selectedUsers[userId];
    
    console.log('Sonraki durum:', this.selectedUsers[userId]);
    
    // Tüm kullanıcılar seçili mi kontrol et
    this.checkIfAllSelected();
  }
  
  checkIfAllSelected() {
    // Tüm kullanıcılar seçili mi kontrol et
    this.selectAllChecked = this.filteredUsers.length > 0 && 
      this.filteredUsers.every(user => this.selectedUsers[user.id]);
  }
  
  isUserSelected(userId: number): boolean {
    return this.selectedUsers[userId] === true;
  }
  
  getSelectedUserCount(): number {
    return Object.values(this.selectedUsers).filter(selected => selected).length;
  }
  
  clearSelectedUsers() {
    this.selectAllChecked = false;
    this.selectedUsers = {};
  }
  
  deleteSelectedUsers() {
    const selectedUserIds = Object.keys(this.selectedUsers)
      .filter(id => this.selectedUsers[parseInt(id)])
        .map(id => parseInt(id));
    
    if (selectedUserIds.length === 0) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Uyarı',
        detail: 'Lütfen silmek için en az bir kullanıcı seçin'
      });
      return;
    }
    
    this.userToDelete = { id: 'all', fullName: `${selectedUserIds.length} kullanıcı` };
    this.deleteDialogVisible = true;
  }
  
  // Rol adını getiren yardımcı metod
  getRoleName(roleId: number | null): string {
    if (!roleId) return 'Rol Atanmamış';
    
    const role = this.roles.find(r => r.id === roleId);
    return role ? role.name : 'Bilinmeyen Rol';
  }

  // Rol adına göre renk sınıfı döndüren yardımcı metot
  getRoleColorClass(roleName: string): string {
    // Önceden tanımlanmış roller için sabit renkler
    if (roleName === 'Admin') return 'admin-badge';
    if (roleName === 'Contributor') return 'contributor-badge';
    
    // Diğer roller için rol adına göre otomatik renk ataması
    // Rol adının hash değerini hesaplayarak tutarlı bir renk elde ediyoruz
    const hash = this.hashString(roleName);
    
    // Önceden tanımlanmış renk sınıfları
    const colorClasses = [
      'blue-badge',    // Mavi
      'green-badge',   // Yeşil
      'purple-badge',  // Mor
      'orange-badge',  // Turuncu
      'teal-badge',    // Turkuaz
      'pink-badge',    // Pembe
      'indigo-badge',  // İndigo
      'amber-badge',   // Amber
      'cyan-badge',    // Camgöbeği
      'lime-badge'     // Limon
    ];
    
    // Hash değerine göre renk sınıfı seçimi
    const colorIndex = Math.abs(hash) % colorClasses.length;
    return colorClasses[colorIndex];
  }
  
  // String için basit bir hash fonksiyonu
  private hashString(str: string): number {
    let hash = 0;
    for (let i = 0; i < str.length; i++) {
      const char = str.charCodeAt(i);
      hash = ((hash << 5) - hash) + char;
      hash = hash & hash; // 32-bit integer'a dönüştür
    }
    return hash;
  }

  /**
   * Ad soyad değerinden benzersiz bir kullanıcı adı oluşturur
   * Kullanıcı adı olarak sadece ad soyadı kullanır, rastgele sayı eklemez
   */
  generateUniqueUsername(fullName: string): string {
    // Boşlukları koru ve ad soyadı olduğu gibi kullan
    return fullName.trim();
  }
}
