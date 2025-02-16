import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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

interface User {
  id: number;
  username: string;
  password?: string;
  isAdmin: boolean;
  createdAt: Date;
  lastLoginAt: Date;
}

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
  userForm: any;
  editMode: boolean = false;

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit(): void {
    // Mock data for testing
    this.users = [
      {
        id: 1,
        username: 'admin',
        isAdmin: true,
        createdAt: new Date(),
        lastLoginAt: new Date()
      },
      {
        id: 2,
        username: 'user',
        isAdmin: false,
        createdAt: new Date(),
        lastLoginAt: new Date()
      }
    ];
  }

  openDialog(user?: User) {
    if (user) {
      this.editMode = true;
      this.userForm = {
        id: user.id,
        username: user.username,
        password: '',
        isAdmin: user.isAdmin
      };
    } else {
      this.editMode = false;
      this.userForm = {
        id: null,
        username: '',
        password: '',
        isAdmin: false
      };
    }
    this.userDialog = true;
  }

  hideDialog() {
    this.userDialog = false;
  }

  saveUser() {
    const userData = this.userForm;
    if (this.editMode) {
      const index = this.users.findIndex(user => user.id === userData.id);
      if (index !== -1) {
        this.users[index] = {
          ...this.users[index],
          ...userData
        };
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Kullanıcı güncellendi'
        });
      }
    } else {
      const newUser: User = {
        ...userData,
        id: this.users.length + 1,
        createdAt: new Date(),
        lastLoginAt: new Date()
      };
      this.users.push(newUser);
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: 'Kullanıcı oluşturuldu'
      });
    }

    this.hideDialog();
  }

  deleteUser(user: User) {
    this.confirmationService.confirm({
      message: `${user.username} kullanıcısını silmek istediğinizden emin misiniz?`,
      header: 'Silme Onayı',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.users = this.users.filter(u => u.id !== user.id);
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Kullanıcı silindi'
        });
      }
    });
  }
}
