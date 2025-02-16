import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, CardModule, ButtonModule],
  template: `
    <div class="container">
      <p-card>
        <div class="header">
          <h1>Hoş Geldiniz</h1>
          <p-button 
            label="Çıkış" 
            icon="pi pi-sign-out"
            (onClick)="logout()"
            styleClass="p-button-text p-button-danger logout-button">
          </p-button>
        </div>
        <p>Merhaba, {{ username }}!</p>

        <div class="button-group">
          <p-button 
            *ngIf="isAdmin"
            label="Kullanıcı Yönetimi" 
            icon="pi pi-users"
            (onClick)="navigateToUserManagement()"
            styleClass="p-button-secondary">
          </p-button>
          
          <p-button 
            label="BİLGİ İŞLEM"
            icon="pi pi-desktop"
            (onClick)="navigateToITManagement()"
            styleClass="p-button-info">
          </p-button>
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
      position: relative;
      margin-bottom: 10px;
    }

    h1 {
      text-align: center;
      color: #2196F3;
      margin: 0;
    }

    p {
      text-align: center;
      color: #666;
      margin: 0 0 30px 0;
    }

    .button-group {
      display: flex;
      flex-direction: column;
      gap: 15px;
    }

    .logout-button {
      position: absolute;
      top: 0;
      right: 0;
    }

    :host ::ng-deep {
      .p-card {
        background: #fff;
        padding: 20px;
      }

      .p-button {
        width: 100%;
      }

      .p-button.p-button-text.p-button-danger {
        width: auto;
        padding: 0.5rem;
        
        .p-button-icon {
          font-size: 1rem;
        }
      }
    }
  `]
})
export class DashboardComponent {
  username: string = '';
  isAdmin: boolean = false;

  constructor(
    private router: Router,
    private authService: AuthService
  ) {
    const currentUser = this.authService.getCurrentUser();
    if (currentUser) {
      this.username = currentUser.username;
      this.isAdmin = currentUser.isAdmin;
    }
  }

  navigateToUserManagement(): void {
    this.router.navigate(['/user-management']);
  }

  navigateToITManagement(): void {
    this.router.navigate(['/it']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
