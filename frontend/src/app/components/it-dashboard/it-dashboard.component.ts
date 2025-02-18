import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-it-dashboard',
  standalone: true,
  imports: [CommonModule, CardModule, ButtonModule],
  template: `
    <div class="grid">
      <div class="col-12 md:col-6 lg:col-4">
        <p-card header="Envanter Yönetimi">
          <div class="button-group">
            <p-button label="Bilgisayarlar" icon="pi pi-desktop" (onClick)="navigate('envanter/bilgisayarlar')" styleClass="p-button-secondary"></p-button>
            <p-button label="Yazıcılar" icon="pi pi-print" (onClick)="navigate('envanter/yazicilar')" styleClass="p-button-secondary"></p-button>
            <p-button label="Ağ Cihazları" icon="pi pi-wifi" (onClick)="navigate('envanter/ag-cihazlari')" styleClass="p-button-secondary"></p-button>
          </div>
        </p-card>
      </div>

      <div class="col-12 md:col-6 lg:col-4">
        <p-card header="Stok Yönetimi">
          <div class="button-group">
            <p-button label="Sarf Malzemeler" icon="pi pi-box" (onClick)="navigate('stok/sarf-malzemeler')" styleClass="p-button-success"></p-button>
            <p-button label="Yedek Parçalar" icon="pi pi-cog" (onClick)="navigate('stok/yedek-parcalar')" styleClass="p-button-success"></p-button>
          </div>
        </p-card>
      </div>

      <div class="col-12 md:col-6 lg:col-4">
        <p-card header="İş Takibi">
          <div class="button-group">
            <p-button label="Aktif İşler" icon="pi pi-clock" (onClick)="navigate('isler/aktif')" styleClass="p-button-warning"></p-button>
            <p-button label="Tamamlanan İşler" icon="pi pi-check-circle" (onClick)="navigate('isler/tamamlanan')" styleClass="p-button-warning"></p-button>
          </div>
        </p-card>
      </div>
    </div>
  `,
  styles: [`
    .grid {
      padding: 2rem;
    }

    .button-group {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    :host ::ng-deep {
      .p-card {
        height: 100%;
        
        .p-card-header {
          padding: 1rem;
          background-color: var(--surface-ground);
          border-bottom: 1px solid var(--surface-border);
          font-weight: 600;
          font-size: 1.2rem;
          color: var(--text-color);
        }

        .p-card-content {
          padding: 1.5rem;
        }

        .p-button {
          width: 100%;
          justify-content: flex-start;

          .p-button-icon {
            font-size: 1.2rem;
            margin-right: 0.5rem;
          }
        }
      }
    }
  `]
})
export class ITDashboardComponent {
  constructor(private router: Router) {}

  navigate(path: string): void {
    this.router.navigate(['/bilgi-islem', path]);
  }
} 