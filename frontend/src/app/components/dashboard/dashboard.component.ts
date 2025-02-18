import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule, 
    CardModule, 
    ButtonModule, 
    FormsModule, 
    InputTextModule, 
    PasswordModule,
    ToastModule
  ],
  providers: [MessageService],
  template: `
    <p-toast></p-toast>
    <div class="container">
      <p-card>
        <div class="header">
          <div class="header-title">
            <h1>Merhaba, {{ username }}</h1>
          </div>
          <div class="header-buttons">
            <p-button 
              label="Şifre Değiştir"
              icon="pi pi-key"
              (onClick)="togglePasswordChange()"
              [styleClass]="showPasswordChange ? 'p-button-text p-button-warning' : 'p-button-text p-button-help'">
            </p-button>
            <p-button 
              label="Çıkış" 
              icon="pi pi-sign-out"
              (onClick)="logout()"
              styleClass="p-button-text p-button-danger">
            </p-button>
          </div>
        </div>

        <div class="button-group">
          <p-button 
            *ngIf="isAdmin"
            label="Kullanıcı Yönetimi" 
            icon="pi pi-users"
            (onClick)="navigateToUserManagement()"
            styleClass="p-button-secondary">
          </p-button>

          <p-button 
            *ngIf="isAdmin"
            label="Kullanıcı Rolleri" 
            icon="pi pi-id-card"
            (onClick)="navigateToRoleManagement()"
            styleClass="p-button-help">
          </p-button>
          
          <p-button 
            label="BİLGİ İŞLEM"
            icon="pi pi-desktop"
            (onClick)="navigateToITManagement()"
            styleClass="p-button-info">
          </p-button>
        </div>

        <div class="password-change-section" *ngIf="showPasswordChange">
          <h2>Şifre Değiştir</h2>
          <div class="p-fluid">
            <div class="p-field">
              <label for="currentPassword">Mevcut Şifre</label>
              <p-password 
                id="currentPassword" 
                [(ngModel)]="currentPassword"
                [feedback]="false"
                [toggleMask]="true"
                placeholder="Mevcut şifrenizi girin">
              </p-password>
            </div>
            <div class="p-field">
              <label for="newPassword">Yeni Şifre</label>
              <p-password 
                id="newPassword" 
                [(ngModel)]="newPassword"
                [feedback]="true"
                [toggleMask]="true"
                placeholder="Yeni şifrenizi girin">
              </p-password>
            </div>
            <div class="p-field">
              <label for="confirmPassword">Yeni Şifre (Tekrar)</label>
              <p-password 
                id="confirmPassword" 
                [(ngModel)]="confirmPassword"
                [feedback]="false"
                [toggleMask]="true"
                placeholder="Yeni şifrenizi tekrar girin">
              </p-password>
            </div>
            <p-button 
              label="Şifreyi Güncelle" 
              icon="pi pi-check"
              (onClick)="changePassword()"
              [disabled]="!isPasswordFormValid()"
              styleClass="p-button-success">
            </p-button>
          </div>
        </div>
      </p-card>
    </div>
  `,
  styles: [`
    .container {
      max-width: 600px;
      margin: 50px auto;
      padding: 0 20px;
    }

    .header {
      margin-bottom: 20px;
      padding-bottom: 20px;
      border-bottom: 1px solid #eee;
    }

    .header-title {
      text-align: center;
      margin-bottom: 15px;
    }

    .header-buttons {
      display: flex;
      justify-content: center;
      gap: 10px;
    }

    h1 {
      color: #2196F3;
      margin: 0;
      font-size: 28px;
    }

    h2 {
      color: #2196F3;
      margin: 0 0 25px 0;
      font-size: 22px;
      text-align: center;
    }

    p {
      text-align: center;
      color: #666;
      margin: 0 0 30px 0;
      font-size: 16px;
    }

    .button-group {
      display: flex;
      flex-direction: column;
      gap: 15px;
      margin-bottom: 30px;
    }

    .password-change-section {
      margin: 0 auto;
      padding: 25px;
      border-radius: 8px;
      background-color: #f8f9fa;
      box-shadow: inset 0 2px 4px rgba(0,0,0,0.05);
      max-width: 400px;
      border: 1px solid #e9ecef;
    }

    .p-field {
      margin-bottom: 20px;
    }

    label {
      display: block;
      margin-bottom: 8px;
      color: #666;
      font-size: 14px;
      font-weight: 500;
    }

    :host ::ng-deep {
      .p-card {
        background: #fff;
        padding: 30px;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
      }

      .p-button {
        width: 100%;
      }

      .header-buttons {
        .p-button {
          width: auto;
          min-width: 120px;
          padding: 0.5rem 1rem;
          
          .p-button-icon {
            font-size: 1rem;
          }

          &.p-button-text {
            &:hover {
              background: rgba(0,0,0,0.05);
            }
          }

          &.p-button-text.p-button-warning {
            color: #f59e0b;
            &:hover {
              background: rgba(245,158,11,0.1);
            }
          }

          &.p-button-text.p-button-help {
            color: #7c3aed;
            &:hover {
              background: rgba(124,58,237,0.1);
            }
          }
        }
      }

      .p-password {
        width: 100%;

        input {
          width: 100%;
          padding: 12px;
          border-radius: 4px;
        }

        .p-password-panel {
          margin-top: 5px;
        }
      }

      .p-button.p-button-success {
        margin-top: 10px;
        padding: 12px;
        font-weight: 600;
      }

      .p-toast {
        .p-toast-message {
          border-radius: 4px;
        }
      }
    }
  `]
})
export class DashboardComponent {
  username: string = '';
  isAdmin: boolean = false;
  currentPassword: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  showPasswordChange: boolean = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    private messageService: MessageService
  ) {
    const currentUser = this.authService.getCurrentUser();
    if (currentUser) {
      this.username = currentUser.username;
      this.isAdmin = currentUser.isAdmin;
    }
  }

  togglePasswordChange(): void {
    this.showPasswordChange = !this.showPasswordChange;
    if (!this.showPasswordChange) {
      this.currentPassword = '';
      this.newPassword = '';
      this.confirmPassword = '';
    }
  }

  navigateToUserManagement(): void {
    this.router.navigate(['/user-management']);
  }

  navigateToRoleManagement(): void {
    this.router.navigate(['/role-management']);
  }

  navigateToITManagement(): void {
    this.router.navigate(['/it']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isPasswordFormValid(): boolean {
    return this.currentPassword.length > 0 && 
           this.newPassword.length > 0 && 
           this.confirmPassword.length > 0 && 
           this.newPassword === this.confirmPassword &&
           this.newPassword.length >= 6;
  }

  changePassword(): void {
    if (!this.isPasswordFormValid()) {
      return;
    }

    this.authService.changePassword(this.currentPassword, this.newPassword)
      .subscribe({
        next: (response) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Şifreniz başarıyla değiştirildi'
          });
          this.currentPassword = '';
          this.newPassword = '';
          this.confirmPassword = '';
          this.showPasswordChange = false;
        },
        error: (error) => {
          if (error.status === 401) {
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Oturumunuz sonlanmış, lütfen tekrar giriş yapın'
            });
            this.router.navigate(['/login']);
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: error.error?.message || 'Şifre değiştirme işlemi başarısız oldu'
            });
          }
        }
      });
  }
}
