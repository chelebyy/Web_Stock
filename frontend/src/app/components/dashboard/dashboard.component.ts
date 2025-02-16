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
    <div class="container mx-auto p-4">
      <p-card header="Hoş Geldiniz" styleClass="dashboard-card">
        <div class="flex flex-column gap-3">
          <p class="welcome-text">Merhaba, {{ username }}!</p>
          <div class="grid">
            <div class="col-12 md:col-6 lg:col-4" *ngIf="isAdmin">
              <p-button 
                label="Kullanıcı Yönetimi" 
                (onClick)="navigateToUserManagement()"
                styleClass="p-button-secondary w-full">
              </p-button>
            </div>
            <div class="col-12 md:col-6 lg:col-4">
              <p-button 
                label="BİLGİ İŞLEM"
                (onClick)="navigateToITManagement()"
                styleClass="p-button-info w-full">
              </p-button>
            </div>
            <div class="col-12 md:col-6 lg:col-4">
              <p-button 
                label="Çıkış Yap" 
                (onClick)="logout()"
                styleClass="p-button-danger w-full">
              </p-button>
            </div>
          </div>
        </div>
      </p-card>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      height: 100vh;
      background-color: var(--surface-ground);
    }
    .dashboard-card {
      max-width: 1200px;
      margin: 2rem auto;
    }
    .welcome-text {
      font-size: 1.5rem;
      color: var(--text-color);
      margin-bottom: 1rem;
    }
    .grid {
      display: grid;
      gap: 1rem;
    }
    :host ::ng-deep .p-button {
      min-height: 3rem;
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
