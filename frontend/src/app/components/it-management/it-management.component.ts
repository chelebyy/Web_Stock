import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-it-management',
  standalone: true,
  imports: [CommonModule, CardModule, ButtonModule, ToolbarModule],
  template: `
    <div class="container mx-auto p-4">
      <p-toolbar styleClass="mb-4">
        <ng-template pTemplate="left">
          <h2>Bilgi İşlem Yönetimi</h2>
        </ng-template>
        <ng-template pTemplate="right">
          <p-button 
            label="Ana Sayfa" 
            icon="pi pi-home"
            (onClick)="navigateToDashboard()" 
            styleClass="p-button-secondary">
          </p-button>
        </ng-template>
      </p-toolbar>

      <div class="grid">
        <div class="col-12 md:col-6 lg:col-4">
          <p-card header="Bilgisayar İşlemleri" styleClass="h-full">
            <p>Bilgisayar envanter yönetimi ve takibi</p>
            <ng-template pTemplate="footer">
              <p-button 
                label="Görüntüle" 
                icon="pi pi-desktop"
                styleClass="p-button-info w-full">
              </p-button>
            </ng-template>
          </p-card>
        </div>

        <div class="col-12 md:col-6 lg:col-4">
          <p-card header="Yazıcı İşlemleri" styleClass="h-full">
            <p>Yazıcı envanter yönetimi ve takibi</p>
            <ng-template pTemplate="footer">
              <p-button 
                label="Görüntüle" 
                icon="pi pi-print"
                styleClass="p-button-info w-full">
              </p-button>
            </ng-template>
          </p-card>
        </div>

        <div class="col-12 md:col-6 lg:col-4">
          <p-card header="Ağ Ekipmanları" styleClass="h-full">
            <p>Ağ cihazları envanter yönetimi ve takibi</p>
            <ng-template pTemplate="footer">
              <p-button 
                label="Görüntüle" 
                icon="pi pi-wifi"
                styleClass="p-button-info w-full">
              </p-button>
            </ng-template>
          </p-card>
        </div>
      </div>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      height: 100vh;
      background-color: var(--surface-ground);
    }
    h2 {
      margin: 0;
      color: var(--text-color);
    }
    .grid {
      display: grid;
      gap: 1rem;
    }
    :host ::ng-deep .p-card {
      height: 100%;
    }
    :host ::ng-deep .p-card-content {
      padding-bottom: 4rem;
    }
    :host ::ng-deep .p-card-footer {
      position: absolute;
      bottom: 1rem;
      left: 1rem;
      right: 1rem;
    }
  `]
})
export class ITManagementComponent {
  constructor(private router: Router) {}

  navigateToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }
} 