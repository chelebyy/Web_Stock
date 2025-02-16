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
      <p-card header="Hoş Geldiniz">
        <div class="flex flex-column gap-3">
          <p>Merhaba, {{ username }}!</p>
          <div class="flex gap-2">
            <p-button 
              *ngIf="isAdmin" 
              label="Kullanıcı Yönetimi" 
              (onClick)="navigateToUserManagement()"
              styleClass="p-button-secondary">
            </p-button>
            <p-button 
              label="Çıkış Yap" 
              (onClick)="logout()"
              styleClass="p-button-danger">
            </p-button>
          </div>
        </div>
      </p-card>
    </div>
  `,
  styles: [`
    :host {
      display: block;
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

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
