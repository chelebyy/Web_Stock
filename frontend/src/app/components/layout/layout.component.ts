import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MenuModule } from 'primeng/menu';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { AuthService } from '../../core/authentication/auth.service';
import { LoadingIndicatorComponent } from '../../shared/components/loading-indicator/loading-indicator.component';
import { ToastModule } from 'primeng/toast';

@Component({
    selector: 'app-layout',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        MenuModule,
        SidebarModule,
        ButtonModule,
        ToolbarModule,
        LoadingIndicatorComponent,
        ToastModule
    ],
    template: `
    <app-loading-indicator></app-loading-indicator>
    <p-toast></p-toast>
    <div class="layout-container">
      <!-- Sol Menü -->
      <div class="layout-menu" [class.layout-menu-active]="sidebarVisible">
        <div class="menu-header">
          <h2>Bilgi İşlem</h2>
        </div>
        
        <div class="menu-content">
          <div class="menu-section">
            <h3>ENVANTER</h3>
            <ul>
              <li><a routerLink="/it/inventory/computers" routerLinkActive="active">Bilgisayarlar</a></li>
              <li><a routerLink="/it/inventory/printers" routerLinkActive="active">Yazıcılar</a></li>
              <li><a routerLink="/it/inventory/network" routerLinkActive="active">Ağ Cihazları</a></li>
            </ul>
          </div>

          <div class="menu-section">
            <h3>STOK</h3>
            <ul>
              <li><a routerLink="/it/stock/consumables" routerLinkActive="active">Sarf Malzemeler</a></li>
              <li><a routerLink="/it/stock/spare-parts" routerLinkActive="active">Yedek Parçalar</a></li>
            </ul>
          </div>

          <div class="menu-section">
            <h3>İŞLER</h3>
            <ul>
              <li><a routerLink="/it/tasks/active" routerLinkActive="active">Aktif İşler</a></li>
              <li><a routerLink="/it/tasks/completed" routerLinkActive="active">Tamamlanan İşler</a></li>
            </ul>
          </div>

          <div class="menu-section">
            <h3>AYARLAR</h3>
            <ul>
              <li><a routerLink="/it/settings/general" routerLinkActive="active">Genel Ayarlar</a></li>
              <li><a routerLink="/it/settings/categories" routerLinkActive="active">Kategoriler</a></li>
            </ul>
          </div>
        </div>
      </div>

      <!-- Ana İçerik -->
      <div class="layout-main">
        <!-- Üst Toolbar -->
        <div class="layout-topbar">
          <div class="topbar-left">
            <button pButton pRipple icon="pi pi-bars" 
                    class="p-button-text menu-button"
                    (click)="sidebarVisible = !sidebarVisible">
            </button>
            <h2 class="topbar-title">{{ pageTitle }}</h2>
          </div>
          <div class="topbar-right">
            <button pButton pRipple icon="pi pi-home" 
                    label="Ana Sayfa" 
                    class="p-button-secondary mr-2"
                    (click)="navigateToHome()">
            </button>
            <button pButton pRipple icon="pi pi-sign-out" 
                    label="Çıkış" 
                    class="p-button-danger"
                    (click)="logout()">
            </button>
          </div>
        </div>

        <!-- Sayfa İçeriği -->
        <div class="layout-content">
          <router-outlet></router-outlet>
        </div>
      </div>
    </div>
  `,
    styles: [`
    .layout-container {
      display: flex;
      min-height: 100vh;
      background-color: var(--surface-ground);
    }

    /* Sol Menü Stilleri */
    .layout-menu {
      width: 250px;
      background-color: var(--surface-overlay);
      border-right: 1px solid var(--surface-border);
      height: 100vh;
      position: fixed;
      left: 0;
      top: 0;
      transition: transform 0.3s;
      z-index: 1000;
    }

    .layout-menu-active {
      transform: translateX(0);
    }

    @media screen and (max-width: 768px) {
      .layout-menu {
        transform: translateX(-100%);
      }
    }

    .menu-header {
      padding: 1rem;
      border-bottom: 1px solid var(--surface-border);
      h2 {
        margin: 0;
        color: var(--primary-color);
      }
    }

    .menu-content {
      padding: 1rem 0;
    }

    .menu-section {
      margin-bottom: 1.5rem;
      padding: 0 1rem;

      h3 {
        margin: 0 0 0.5rem 0;
        color: var(--text-color-secondary);
        font-size: 0.875rem;
        font-weight: 600;
      }

      ul {
        list-style: none;
        padding: 0;
        margin: 0;

        li {
          margin: 0.25rem 0;

          a {
            display: block;
            padding: 0.5rem 1rem;
            color: var(--text-color);
            text-decoration: none;
            border-radius: var(--border-radius);
            transition: background-color 0.2s;

            &:hover {
              background-color: var(--surface-hover);
            }

            &.active {
              background-color: var(--primary-color);
              color: var(--primary-color-text);
            }
          }
        }
      }
    }

    /* Ana İçerik Stilleri */
    .layout-main {
      flex: 1;
      margin-left: 250px;
      min-height: 100vh;
      display: flex;
      flex-direction: column;
    }

    @media screen and (max-width: 768px) {
      .layout-main {
        margin-left: 0;
      }
    }

    /* Üst Toolbar Stilleri */
    .layout-topbar {
      background-color: var(--surface-card);
      padding: 1rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-bottom: 1px solid var(--surface-border);
    }

    .topbar-left {
      display: flex;
      align-items: center;
      gap: 1rem;
    }

    .topbar-right {
      display: flex;
      gap: 0.5rem;
    }

    .topbar-title {
      margin: 0;
      font-size: 1.25rem;
      font-weight: 500;
      color: var(--text-color);
    }

    .menu-button {
      padding: 0.5rem;
    }

    /* Sayfa İçeriği Stilleri */
    .layout-content {
      flex: 1;
      padding: 2rem;
      background-color: var(--surface-ground);
    }

    /* Responsive Düzenlemeler */
    @media screen and (max-width: 768px) {
      .topbar-title {
        font-size: 1rem;
      }

      .layout-topbar {
        padding: 0.5rem;
      }

      .layout-content {
        padding: 1rem;
      }
    }
  `]
})
export class LayoutComponent {
  sidebarVisible = true;
  pageTitle = 'Bilgi İşlem Yönetimi';

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  navigateToHome(): void {
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
} 