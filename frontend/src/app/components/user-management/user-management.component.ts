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
import { UserService } from '../../services/user.service';
import { RoleService } from '../../services/role.service';
import { User } from '../../models/user.model';
import { Role, RoleWithUsers } from '../../models/role.model';

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
    DropdownModule
  ],
  providers: [ConfirmationService, MessageService]
})
export class UserManagementComponent implements OnInit {
  users: User[] = [];
  roles: RoleWithUsers[] = [];
  userDialog: boolean = false;
  userForm: FormGroup;
  editMode: boolean = false;
  loading: boolean = false;

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder
  ) {
    this.userForm = this.fb.group({
      id: [null],
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      isAdmin: [false],
      roleId: [null]
    });
  }

  ngOnInit(): void {
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers() {
    this.loading = true;
    this.userService.getUsers().subscribe({
      next: (data) => {
        console.log('Loaded users:', data);
        this.users = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `Kullanıcılar yüklenirken bir hata oluştu: ${error.status === 401 ? 'Yetkilendirme hatası. Lütfen giriş yapın.' : error.message}`
        });
        this.loading = false;
      }
    });
  }

  loadRoles() {
    this.roleService.getRoles().subscribe({
      next: (data) => {
        console.log('Loaded roles:', data);
        this.roles = data;
      },
      error: (error) => {
        console.error('Error loading roles:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Roller yüklenirken bir hata oluştu'
        });
      }
    });
  }

  openDialog(user?: User) {
    this.editMode = !!user;
    if (user) {
      this.userForm.patchValue({
        id: user.id,
        username: user.username,
        isAdmin: user.isAdmin,
        roleId: user.roleId
      });
      // Düzenleme modunda şifre zorunlu değil
      this.userForm.get('password')?.setValidators(null);
    } else {
      this.userForm.reset({
        id: null,
        username: '',
        password: '',
        isAdmin: false,
        roleId: null
      });
      // Yeni kullanıcı için şifre zorunlu
      this.userForm.get('password')?.setValidators([Validators.required]);
    }
    this.userForm.get('password')?.updateValueAndValidity();
    this.userDialog = true;
  }

  hideDialog() {
    this.userDialog = false;
    this.userForm.reset();
  }

  saveUser() {
    if (this.userForm.invalid) {
      Object.keys(this.userForm.controls).forEach(key => {
        const control = this.userForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
      return;
    }

    const userData = this.userForm.value;
    
    if (this.editMode) {
      // Şifre alanını PasswordHash olarak gönder
      if (userData.password) {
        userData.passwordHash = userData.password;
        delete userData.password;
      }

      this.userService.updateUser(userData.id, userData).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kullanıcı güncellendi'
          });
          this.loadUsers();
          this.hideDialog();
        },
        error: (error) => {
          console.error('Error updating user:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcı güncellenirken bir hata oluştu'
          });
        }
      });
    } else {
      // Yeni kullanıcı oluşturma
      const newUser = {
        username: userData.username,
        passwordHash: userData.password,
        isAdmin: userData.isAdmin,
        roleId: userData.roleId,
        createdAt: new Date()
      };

      this.userService.createUser(newUser).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kullanıcı oluşturuldu'
          });
          this.loadUsers();
          this.hideDialog();
        },
        error: (error) => {
          console.error('Error creating user:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcı oluşturulurken bir hata oluştu'
          });
        }
      });
    }
  }

  deleteUser(user: User) {
    if (!user.id) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Kullanıcı ID bulunamadı'
      });
      return;
    }

    this.confirmationService.confirm({
      message: 'Bu kullanıcıyı silmek istediğinizden emin misiniz?',
      header: 'Onay',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.userService.deleteUser(user.id!).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Kullanıcı silindi'
            });
            this.loadUsers();
          },
          error: (error) => {
            console.error('Error deleting user:', error);
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Kullanıcı silinirken bir hata oluştu'
            });
          }
        });
      }
    });
  }

  getRoleName(roleId: number | undefined): string {
    if (!roleId) return 'Rol atanmamış';
    const role = this.roles.find(r => r.id === roleId);
    return role ? role.name : 'Rol bulunamadı';
  }
}
