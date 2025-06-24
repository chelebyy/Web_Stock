import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-access-denied',
  standalone: true,
  imports: [CommonModule, ButtonModule],
  template: `
    <div class="access-denied-container">
      <div class="access-denied-card">
        <div class="icon">
          <i class="pi pi-lock"></i>
        </div>
        <h1>Erişim Reddedildi</h1>
        <p>Bu sayfaya erişim yetkiniz bulunmamaktadır.</p>
        <div class="actions">
          <button pButton label="Ana Sayfaya Dön" icon="pi pi-home" (click)="navigateToHome()"></button>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .access-denied-container {
      display: flex;
      justify-content: center;
      align-items: center;
      height: 100vh;
      background-color: #f8f9fa;
    }
    
    .access-denied-card {
      background-color: white;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      padding: 2rem;
      text-align: center;
      max-width: 400px;
      width: 100%;
    }
    
    .icon {
      font-size: 4rem;
      color: #e74c3c;
      margin-bottom: 1rem;
    }
    
    h1 {
      color: #333;
      margin-bottom: 1rem;
    }
    
    p {
      color: #666;
      margin-bottom: 2rem;
    }
    
    .actions {
      display: flex;
      justify-content: center;
    }
  `]
})
export class AccessDeniedComponent {
  constructor(private router: Router) {}

  navigateToHome() {
    const isAdmin = localStorage.getItem('isAdmin') === 'true';
    this.router.navigate([isAdmin ? '/admin-dashboard' : '/user-dashboard']);
  }
} 