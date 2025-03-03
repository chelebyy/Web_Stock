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
    { label: 'Katkıda Bulunan', value: 'Contributor' },
    { label: 'İzleyici', value: 'Viewer' }
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
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.initForm();
    this.loadUsers();
  }

  initForm() {
    this.userForm = this.formBuilder.group({
      id: [null],
      email: ['', [Validators.required, Validators.email]],
      fullName: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      isAdmin: [false],
      isActive: [true]
    });
  }

  loadUsers() {
    // Örnek kullanıcı verileri
    this.users = [
      {
        id: 1,
        username: 'leslie',
        fullName: 'Leslie Maya',
        email: 'leslie@gmail.com',
        location: 'Los Angeles,CA',
        joinDate: 'October 2, 2010',
        permissions: 'Admin',
        avatar: 'avatar1.jpg'
      },
      {
        id: 2,
        username: 'josie',
        fullName: 'Josie Deck',
        email: 'josie@gmail.com',
        location: 'Cheyenne,WY',
        joinDate: 'October 3, 2011',
        permissions: 'Admin',
        avatar: 'avatar2.jpg'
      },
      {
        id: 3,
        username: 'alex',
        fullName: 'Alex Pfeiffer',
        email: 'alex@gmail.com',
        location: 'Cheyenne,WY',
        joinDate: 'May 20, 2015',
        permissions: 'Admin',
        avatar: 'avatar3.jpg'
      },
      {
        id: 4,
        username: 'mike',
        fullName: 'Mike Dean',
        email: 'mike@gmail.com',
        location: 'Syracuse,NY',
        joinDate: 'July 14, 2015',
        permissions: 'Contributor',
        avatar: 'avatar4.jpg'
      },
      {
        id: 5,
        username: 'mateus',
        fullName: 'Mateus Cunha',
        email: 'cunha@gmail.com',
        location: 'Luanda,AN',
        joinDate: 'October, 2016',
        permissions: 'Contributor',
        avatar: 'avatar5.jpg'
      },
      {
        id: 6,
        username: 'nzola',
        fullName: 'Nzola Uemo',
        email: 'nzola@gmail.com',
        location: 'Lagos,NG',
        joinDate: 'June 5, 2016',
        permissions: 'Viewer',
        avatar: 'avatar6.jpg'
      },
      {
        id: 7,
        username: 'antony',
        fullName: 'Antony Mack',
        email: 'mack@gmail.com',
        location: 'London,ENG',
        joinDate: 'June 15, 2015',
        permissions: 'Contributor',
        avatar: 'avatar7.jpg'
      },
      {
        id: 8,
        username: 'andre',
        fullName: 'André da Silva',
        email: 'andré@gmail.com',
        location: 'São Paulo,BR',
        joinDate: 'March 13, 2018',
        permissions: 'Contributor',
        avatar: 'avatar8.jpg'
      },
      {
        id: 9,
        username: 'jorge',
        fullName: 'Jorge Ferreira',
        email: 'jorge@gmail.com',
        location: 'Huambo,Angola',
        joinDate: 'March 14, 2018',
        permissions: 'Contributor',
        avatar: 'avatar9.jpg'
      }
    ];
    
    this.filteredUsers = [...this.users];
    this.updatePagination();
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
      email: this.user.email,
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
        this.users = this.users.filter(u => u.id !== user.id);
        this.applyFilters();
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Kullanıcı silindi',
          life: 3000
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
        this.users[index] = {
          ...this.users[index],
          email: formValues.email,
          fullName: formValues.fullName,
          permissions: formValues.isAdmin ? 'Admin' : 'Contributor',
          isActive: formValues.isActive
        };
        
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Kullanıcı güncellendi',
          life: 3000
        });
      }
    } else {
      // Yeni kullanıcı ekleme
      const newUser = {
        id: this.generateId(),
        email: formValues.email,
        fullName: formValues.fullName,
        location: 'İstanbul,TR',
        joinDate: new Date().toLocaleDateString('tr-TR', { year: 'numeric', month: 'long', day: 'numeric' }),
        permissions: formValues.isAdmin ? 'Admin' : 'Contributor',
        isActive: formValues.isActive,
        avatar: 'default-avatar.png'
      };
      
      this.users.unshift(newUser);
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: 'Kullanıcı eklendi',
        life: 3000
      });
    }
    
    this.applyFilters();
    this.userDialog = false;
    this.user = {};
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
        user.email.toLowerCase().includes(this.activeFilters.search)
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
    this.router.navigate(['/dashboard']);
  }
}
