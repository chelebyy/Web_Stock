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
    <div class="animated-background">
      <div class="cube"></div>
      <div class="cube"></div>
      <div class="cube"></div>
      <div class="cube"></div>
      <div class="cube"></div>
      <div class="lines"></div>
    </div>

    <p-toast></p-toast>
    <div class="dashboard-container">
      <div class="glass-card">
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
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      min-height: 100vh;
    }

    .animated-background {
      position: fixed;
      width: 100vw;
      height: 100vh;
      top: 0;
      left: 0;
      background: linear-gradient(45deg, #1a1f3c, #2a2f4c);
      overflow: hidden;
      z-index: 0;
    }

    .cube {
      position: absolute;
      top: 80vh;
      left: 45vw;
      width: 10px;
      height: 10px;
      border: solid 1px rgba(255, 255, 255, 0.2);
      transform-origin: top left;
      transform: scale(0) rotate(0deg) translate(-50%, -50%);
      animation: cube 12s ease-in forwards infinite;
      
      &:nth-child(2) {
        animation-delay: 2s;
        left: 25vw;
        top: 40vh;
      }
      
      &:nth-child(3) {
        animation-delay: 4s;
        left: 75vw;
        top: 50vh;
      }
      
      &:nth-child(4) {
        animation-delay: 6s;
        left: 90vw;
        top: 10vh;
      }
      
      &:nth-child(5) {
        animation-delay: 8s;
        left: 10vw;
        top: 85vh;
      }
    }

    .lines {
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background: repeating-linear-gradient(
        0deg,
        transparent 0%,
        rgba(255, 255, 255, 0.05) 0.5%,
        transparent 1%
      );
      animation: lines 20s linear infinite;
      background-size: 100% 50px;
    }

    @keyframes lines {
      0% {
        background-position: 0 0;
      }
      100% {
        background-position: 0 1000px;
      }
    }

    @keyframes cube {
      from {
        transform: scale(0) rotate(0deg) translate(-50%, -50%);
        opacity: 1;
      }
      to {
        transform: scale(20) rotate(960deg) translate(-50%, -50%);
        opacity: 0;
      }
    }

    .dashboard-container {
      position: relative;
      z-index: 1;
      max-width: 600px;
      margin: 50px auto;
      padding: 0 20px;
      animation: fadeIn 0.5s ease-out;
    }

    .glass-card {
      background: rgba(255, 255, 255, 0.1);
      backdrop-filter: blur(10px);
      -webkit-backdrop-filter: blur(10px);
      border-radius: 16px;
      padding: 2rem;
      box-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.15);
      border: 1px solid rgba(255, 255, 255, 0.18);
    }

    .header {
      margin-bottom: 20px;
      padding-bottom: 20px;
      border-bottom: 1px solid rgba(255, 255, 255, 0.3);
    }

    .header-title {
      text-align: center;
      margin-bottom: 15px;

      h1 {
        color: #fff;
        margin: 0;
        font-size: 28px;
      }
    }

    .header-buttons {
      display: flex;
      justify-content: center;
      gap: 10px;
    }

    .button-group {
      display: flex;
      flex-direction: column;
      gap: 15px;
      margin-bottom: 30px;
    }

    .password-change-section {
      background: rgba(255, 255, 255, 0.1);
      padding: 25px;
      border-radius: 12px;
      margin-top: 20px;

      h2 {
        color: #fff;
        text-align: center;
        margin-bottom: 20px;
      }
    }

    @keyframes fadeIn {
      from {
        opacity: 0;
        transform: translateY(20px);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }

    :host ::ng-deep {
      .p-button {
        width: 100%;
      }

      .header-buttons {
        .p-button {
          width: auto;
          min-width: 120px;
          padding: 0.5rem 1rem;
        }
      }

      .p-password input {
        background: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.2);
        color: #fff;

        &::placeholder {
          color: rgba(255, 255, 255, 0.5);
        }
      }

      label {
        color: #fff;
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

