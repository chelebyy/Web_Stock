import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-computers',
  standalone: true,
  imports: [CommonModule, CardModule],
  template: `
    <div class="computers-container">
      <p-card [header]="title()" styleClass="p-card-shadow">
        <div class="card-content">
          <p>Bu sayfa Bilgisayar Envanteri modülü için oluşturulmuştur. Şu anda içeriği boştur.</p>
          <p>Bu sayfaya erişim, kullanıcı izinleri ile kontrol edilmektedir.</p>
        </div>
      </p-card>
    </div>
  `,
  styles: [`
    .computers-container {
      padding: 2rem;
      background-color: var(--surface-ground);
      min-height: 100vh;
    }
    
    .card-content {
      padding: 1rem 0;
    }
    
    :host ::ng-deep .p-card {
      background-color: white;
      border-radius: 12px;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
      transition: all 0.3s ease;
    }
    
    :host ::ng-deep .p-card:hover {
      box-shadow: 0 8px 15px rgba(0, 0, 0, 0.08);
    }
    
    :host ::ng-deep .p-card-title {
      font-size: 1.5rem;
      font-weight: 700;
      color: #1e3a8a;
    }
  `]
})
export class ComputersComponent {
  title = signal('Bilgisayar Envanteri');
} 