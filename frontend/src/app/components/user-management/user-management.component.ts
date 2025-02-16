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
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';

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
    ToolbarModule
  ],
  providers: [ConfirmationService, MessageService]
})
export class UserManagementComponent implements OnInit {
  users: User[] = [];
  userDialog: boolean = false;
  userForm: FormGroup;
  editMode: boolean = false;
  loading: boolean = false;

  constructor(
    private userService: UserService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder
  ) {
    this.userForm = this.fb.group({
      id: [null],
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      isAdmin: [false]
    });
  }

  ngOnInit(): void {
    this.loadUsers();
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
          detail: 'Kullanıcılar yüklenirken bir hata oluştu'
        });
        this.loading = false;
      }
    });
  }

  openDialog(user?: User) {
    this.editMode = !!user;
    if (user) {
      this.userForm.patchValue({
        id: user.id,
        username: user.username,
        isAdmin: user.isAdmin
      });
      // Düzenleme modunda şifre zorunlu değil
      this.userForm.get('password')?.setValidators(null);
    } else {
      this.userForm.reset({
        id: null,
        username: '',
        password: '',
        isAdmin: false
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
      this.userService.createUser(userData).subscribe({
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
    this.confirmationService.confirm({
      message: 'Bu kullanıcıyı silmek istediğinizden emin misiniz?',
      header: 'Onay',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.userService.deleteUser(user.id).subscribe({
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
}
