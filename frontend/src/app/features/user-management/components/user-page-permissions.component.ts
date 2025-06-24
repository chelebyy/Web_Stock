import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';

// PrimeNG Modülleri
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { CardModule } from 'primeng/card';
import { MessageService } from 'primeng/api';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { DividerModule } from 'primeng/divider';
import { RippleModule } from 'primeng/ripple';
import { TabViewModule } from 'primeng/tabview';

// Servisler ve Modeller
import { PermissionService } from '../../../services/permission.service';
import { UserService } from '../../../services/user.service';
import { User } from '../../../shared/models/user.model';

interface Permission {
  id: number;
  name: string;
  description: string;
  group: string;
  isGranted: boolean;
  resourceName?: string;
  resourceType?: string;
  action?: string;
}

interface PermissionGroup {
  group: string;
  permissions: Permission[];
  expanded: boolean;
}

// Yeni arayüz: Sayfa bazlı izin grupları
interface PagePermission {
  pageName: string;
  pageDescription: string;
  pageIcon: string;
  permissions: Permission[];
  expanded: boolean;
}

@Component({
    selector: 'app-user-page-permissions',
    imports: [
        CommonModule,
        FormsModule,
        TableModule,
        ButtonModule,
        CheckboxModule,
        ToastModule,
        ToolbarModule,
        CardModule,
        ProgressSpinnerModule,
        DividerModule,
        RippleModule,
        TabViewModule
    ],
    providers: [MessageService],
    template: `
    <div class="page-container">
      <div class="page-header">
        <div class="title-container">
          <h1 class="page-title">Kullanıcı Sayfa İzinleri</h1>
          <p class="text-muted user-info" *ngIf="userName">
            <span class="user-label">Kullanıcı:</span> 
            <span class="user-name">{{ userName }}</span>
          </p>
        </div>
        <button pButton pRipple icon="pi pi-arrow-left" label="Geri Dön" 
                class="p-button-outlined back-button" (click)="goBack()"></button>
      </div>
      
      <div class="content-card">
        <div class="loading-container" *ngIf="loading">
          <p-progressSpinner strokeWidth="4" 
                          fill="var(--surface-ground)" 
                          animationDuration=".5s"></p-progressSpinner>
          <p class="loading-text">İzinler yükleniyor...</p>
        </div>
        
        <div class="permissions-container" *ngIf="!loading">
          <div class="info-panel">
            <div class="info-icon">
              <i class="pi pi-info-circle"></i>
            </div>
            <div class="info-text">
              Bu sayfada kullanıcının erişebileceği sayfaları belirleyebilirsiniz. İzin vermek istediğiniz sayfaları işaretleyip "Kaydet" butonuna tıklayınız.
            </div>
          </div>

          <!-- Sayfa Bazlı İzin Kartları -->
          <div class="page-permissions-container">
            <div class="overlay" [class.active]="anyCardExpanded()" (click)="closeAllCards()"></div>
            
            <ng-container *ngFor="let page of pagePermissions; let i = index">
              <div class="page-permission-card" [attr.id]="'page-card-' + i" 
                   [class.card-expanded]="page.expanded"
                   [style.display]="anyCardExpanded() && !page.expanded ? 'none' : 'block'">
                <div class="page-card-header" (click)="togglePageExpanded(i)" [class.header-expanded]="page.expanded">
                  <div class="page-icon">
                    <i class="pi" [ngClass]="page.pageIcon"></i>
                  </div>
                  <div class="page-info">
                    <h3 class="page-name">{{ page.pageName }}</h3>
                    <p class="page-description">{{ page.pageDescription }}</p>
                  </div>
                  <div class="page-toggle">
                    <i class="pi" [ngClass]="page.expanded ? 'pi-chevron-up' : 'pi-chevron-down'"></i>
                  </div>
                </div>
                
                <div class="close-button" [class.active]="page.expanded" (click)="closeCard(i)">
                  <i class="pi pi-times"></i>
                </div>
                
                <div class="page-card-content" [class.expanded]="page.expanded" [attr.id]="'page-content-' + i"
                     [style.display]="page.expanded ? 'block' : 'none'">
                  <div class="page-permissions-list">
                    <div class="permission-item" *ngFor="let permission of page.permissions">
                      <div class="permission-content">
                        <div class="checkbox-wrapper">
                          <input type="checkbox" [id]="'perm_' + permission.id" [(ngModel)]="permission.isGranted">
                          <span class="checkmark"></span>
                        </div>
                        <label [for]="'perm_' + permission.id" class="permission-label">
                          {{ permission.description || permission.name }}
                          <small class="permission-name">{{ permission.name }}</small>
                        </label>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </ng-container>
          </div>
          
          <div class="action-buttons">
            <button pButton pRipple label="Kaydet" icon="pi pi-check" 
                    class="p-button-success save-button" (click)="savePermissions()"></button>
            <button pButton pRipple label="Rol İzinlerine Sıfırla" icon="pi pi-refresh" 
                    class="p-button-secondary reset-button" (click)="resetToRolePermissions()"></button>
          </div>
        </div>
      </div>
    </div>
    
    <p-toast position="top-right"></p-toast>
  `,
    styles: [`
    .page-container {
      padding: 2rem;
      background-color: #f8fafc;
      min-height: 100vh;
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
    }
    
    .page-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 1rem;
    }
    
    .title-container {
      display: flex;
      flex-direction: column;
    }
    
    .page-title {
      font-size: 2rem;
      font-weight: 700;
      color: #1e3a8a;
      margin: 0;
      margin-bottom: 0.5rem;
      position: relative;
    }
    
    .page-title::after {
      content: '';
      position: absolute;
      bottom: -10px;
      left: 0;
      width: 60px;
      height: 4px;
      background: linear-gradient(90deg, #3B82F6, #60a5fa);
      border-radius: 4px;
    }
    
    .user-info {
      margin-top: 1.25rem;
      font-size: 1.1rem;
      color: #64748b;
    }
    
    .user-label {
      font-weight: 600;
    }
    
    .user-name {
      font-weight: 500;
      color: #334155;
    }
    
    .back-button {
      transition: all 0.2s ease;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
      border: 1px solid #e2e8f0;
    }
    
    .back-button:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
      background-color: #f8fafc;
    }
    
    .content-card {
      background-color: white;
      border-radius: 12px;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
      padding: 2rem;
      transition: all 0.3s ease;
    }
    
    .content-card:hover {
      box-shadow: 0 8px 15px rgba(0, 0, 0, 0.08);
    }
    
    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 3rem;
    }
    
    .loading-text {
      margin-top: 1rem;
      color: #64748b;
      font-size: 1.1rem;
    }
    
    .info-panel {
      display: flex;
      gap: 1rem;
      padding: 1rem;
      background-color: #eff6ff;
      border-left: 4px solid #3b82f6;
      border-radius: 8px;
      margin-bottom: 1.5rem;
      align-items: center;
    }
    
    .info-icon {
      font-size: 1.5rem;
      color: #3b82f6;
    }
    
    .info-text {
      color: #334155;
      font-size: 0.95rem;
      line-height: 1.5;
    }
    
    .permissions-container {
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
    }

    /* Sayfa Bazlı İzin Kartları Stilleri */
    .page-permissions-container {
      position: relative;
      width: 100%;
      display: flex;
      flex-wrap: wrap;
      gap: 1rem;
      margin-bottom: 1.5rem;
    }

    .page-permission-card {
      flex: 0 0 calc(50% - 0.5rem); /* İki kolon yerleşim */
      margin-bottom: 1rem;
      background-color: #ffffff;
      border-radius: 12px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
      border: 1px solid #e2e8f0;
      overflow: hidden;
      position: relative;
      transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

    .page-permission-card.card-expanded {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      z-index: 10;
      box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
      border-color: #bfdbfe;
      overflow: visible;
    }

    @media (max-width: 992px) {
      .page-permission-card {
        flex: 0 0 100%; /* Tek kolon yerleşim */
      }
    }

    .page-card-header {
      display: flex;
      align-items: center;
      padding: 1.25rem;
      background-color: #f8fafc;
      cursor: pointer;
      user-select: none;
      transition: all 0.2s ease;
      position: relative;
      z-index: 3;
      border-radius: 12px 12px 0 0;
    }

    .page-card-header:hover {
      background-color: #f1f5f9;
    }

    .page-card-header.header-expanded {
      background-color: #eff6ff;
      border-bottom: 1px solid #bfdbfe;
    }

    .page-icon {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 48px;
      height: 48px;
      background-color: #e0f2fe;
      border-radius: 12px;
      margin-right: 1rem;
      flex-shrink: 0;
    }

    .page-icon i {
      font-size: 1.5rem;
      color: #0284c7;
    }

    .page-info {
      flex: 1;
    }

    .page-name {
      font-size: 1.1rem;
      font-weight: 600;
      color: #1e3a8a;
      margin: 0 0 0.25rem 0;
    }

    .page-description {
      font-size: 0.9rem;
      color: #64748b;
      margin: 0;
    }

    .page-toggle i {
      font-size: 1rem;
      color: #64748b;
      transition: transform 0.2s ease;
    }

    .page-card-content {
      background-color: #ffffff;
      overflow: hidden;
      max-height: 0;
      opacity: 0;
      transition: opacity 0.3s ease, max-height 0.3s ease, padding 0.3s ease;
      padding: 0;
      position: relative;
      z-index: 1;
    }
    
    .page-card-content.expanded {
      opacity: 1;
      padding: 1rem;
      max-height: 2000px;
      border-top: 1px solid #e2e8f0;
    }

    .overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0, 0, 0, 0.5);
      z-index: 5;
      display: none;
    }

    .overlay.active {
      display: block;
    }

    .page-permissions-list {
      display: grid;
      grid-template-columns: 1fr;
      gap: 0.75rem;
    }

    .close-button {
      position: absolute;
      top: 1rem;
      right: 1rem;
      width: 32px;
      height: 32px;
      border-radius: 50%;
      background-color: #f1f5f9;
      border: 1px solid #e2e8f0;
      display: flex;
      align-items: center;
      justify-content: center;
      cursor: pointer;
      z-index: 11;
      display: none;
    }

    .close-button.active {
      display: flex;
    }

    .permission-item {
      background-color: #f8fafc;
      border-radius: 8px;
      padding: 0.75rem;
      transition: all 0.2s ease;
      border: 1px solid #e2e8f0;
    }
    
    .permission-item:hover {
      background-color: #eff6ff;
      border-color: #bfdbfe;
      transform: translateY(-2px);
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
    }
    
    .permission-content {
      display: flex;
      align-items: flex-start;
      gap: 0.75rem;
    }
    
    .permission-label {
      display: flex;
      flex-direction: column;
      font-weight: 500;
      color: #334155;
      cursor: pointer;
      flex: 1;
    }
    
    .permission-name {
      color: #64748b;
      font-size: 0.8rem;
      margin-top: 0.25rem;
    }

    /* Özel checkbox stilleri */
    .checkbox-wrapper {
      position: relative;
      width: 20px;
      height: 20px;
      margin-top: 2px;
    }
    
    .checkbox-wrapper input[type="checkbox"] {
      opacity: 0;
      width: 0;
      height: 0;
      position: absolute;
    }
    
    .checkmark {
      position: absolute;
      top: 0;
      left: 0;
      height: 20px;
      width: 20px;
      background-color: #fff;
      border: 2px solid #cbd5e1;
      border-radius: 4px;
      transition: all 0.2s ease;
    }
    
    .checkbox-wrapper input[type="checkbox"]:checked ~ .checkmark {
      background-color: #3b82f6;
      border-color: #3b82f6;
    }
    
    .checkmark:after {
      content: "";
      position: absolute;
      display: none;
    }
    
    .checkbox-wrapper input[type="checkbox"]:checked ~ .checkmark:after {
      display: block;
      left: 6px;
      top: 2px;
      width: 5px;
      height: 10px;
      border: solid white;
      border-width: 0 2px 2px 0;
      transform: rotate(45deg);
    }
    
    .checkbox-wrapper:hover .checkmark {
      border-color: #3b82f6;
    }
    
    .action-buttons {
      display: flex;
      justify-content: flex-end;
      gap: 1rem;
      margin-top: 2rem;
    }
    
    .save-button, .reset-button {
      padding: 0.75rem 1.5rem;
      border-radius: 8px;
      font-weight: 500;
      transition: all 0.2s ease;
    }
    
    .save-button:hover, .reset-button:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }
    
    .save-button {
      background-color: #10b981;
      border-color: #10b981;
    }
    
    .save-button:hover {
      background-color: #059669;
      border-color: #059669;
    }
    
    .reset-button {
      background-color: #64748b;
      border-color: #64748b;
    }
    
    .reset-button:hover {
      background-color: #475569;
      border-color: #475569;
    }
    
    @media (max-width: 768px) {
      .page-container {
        padding: 1rem;
      }
      
      .page-header {
        flex-direction: column;
        align-items: flex-start;
        gap: 1rem;
      }
      
      .back-button {
        align-self: flex-start;
      }
      
      .content-card {
        padding: 1.5rem;
      }
      
      .page-permissions-container {
        grid-template-columns: 1fr;
      }
      
      .action-buttons {
        flex-direction: column;
      }
      
      .save-button, .reset-button {
        width: 100%;
      }
    }
  `]
})
export class UserPagePermissionsComponent implements OnInit {
  userId = 0;
  userName = '';
  loading = true;
  permissionGroups: PermissionGroup[] = [];
  pagePermissions: PagePermission[] = []; // Sayfa bazlı izinler
  allPermissions: Permission[] = [];
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private permissionService: PermissionService,
    private userService: UserService,
    private messageService: MessageService
  ) {}
  
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = +params['userId'];
      this.loadUserPermissions();
    });
  }
  
  loadUserPermissions(): void {
    this.loading = true;
    
    // Kullanıcı bilgilerini al
    this.userService.getUserById(this.userId).subscribe({
      next: (user) => {
        // Kullanıcı adını oluştur - öncelikle adi ve soyadi kullan, yoksa alternatif alanlar kullan
        if (user.adi && user.soyadi) {
          this.userName = `${user.adi} ${user.soyadi}`;
        } else if (user.fullName) {
          this.userName = user.fullName;
        } else if (user.firstName && user.lastName) {
          this.userName = `${user.firstName} ${user.lastName}`;
        } else if (user.username) {
          this.userName = user.username;
        } else {
          this.userName = `Kullanıcı #${user.id}`;
        }
        
        // Tüm izinleri al
        this.permissionService.getAllPermissions().subscribe({
          next: (permissions) => {
            console.log('getAllPermissions yanıtı:', permissions);
            if (!permissions || !Array.isArray(permissions)) {
              console.error('Permissions verisi dizi değil:', permissions);
              this.allPermissions = [];
              this.loading = false;
              return;
            }
            
            // Sadece sayfa izinlerini filtrele (Page tipindeki izinler)
            this.allPermissions = permissions
              .filter(p => p.resourceType === 'Page')
              .map(p => ({
                ...p,
                isGranted: false
              }));
            
            // Kullanıcının mevcut izinlerini al
            this.permissionService.getPermissionsByUserId(this.userId).subscribe({
              next: (userPermissions) => {
                console.log('getPermissionsByUserId yanıtı:', userPermissions);
                if (userPermissions && Array.isArray(userPermissions)) {
                  const userPermissionIds = userPermissions.map(p => p.id);
                  this.allPermissions.forEach(p => {
                    p.isGranted = userPermissionIds.includes(p.id);
                  });
                }
                
                this.groupPermissionsByPage(); // Sayfa bazlı gruplandırma
                this.loading = false;
              },
              error: (error) => {
                console.error('Kullanıcı izinleri yüklenirken hata oluştu:', error);
                this.loading = false;
                this.messageService.add({
                  severity: 'error',
                  summary: 'Hata',
                  detail: 'Kullanıcı izinleri yüklenirken bir hata oluştu'
                });
              }
            });
          },
          error: (error) => {
            console.error('Tüm izinler yüklenirken hata oluştu:', error);
            this.loading = false;
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'İzinler yüklenirken bir hata oluştu'
            });
          }
        });
      },
      error: (error) => {
        console.error('Kullanıcı bilgisi yüklenirken hata oluştu:', error);
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcı bilgisi yüklenirken bir hata oluştu'
        });
      }
    });
  }
  
  // Sayfa bazlı izin gruplandırma metodu
  groupPermissionsByPage(): void {
    // Henüz oluşturulmamış sayfaların izinlerini filtrele
    const filteredPermissions = this.allPermissions.filter(permission => {
      return !(
        permission.name.startsWith('Pages.Reports') ||
        permission.name.startsWith('Pages.Settings') ||
        permission.name.startsWith('Pages.Egitim') ||
        permission.name.startsWith('Pages.StockManagement') ||
        permission.name === 'Pages.AdminDashboard' ||
        permission.group === 'Stok Yönetimi'
      );
    });
    
    // Sayfa bazlı izin grupları
    const pageGroups: Record<string, PagePermission> = {};
    
    // Sayfa izinlerini gruplandır
    filteredPermissions.forEach(permission => {
      // Sayfa adını belirle (resourceName veya name'den çıkar)
      let pageName = permission.resourceName || '';
      let pageKey = '';
      
      if (!pageName && permission.name) {
        // İzin adından sayfa adını çıkar (Pages.UserManagement -> UserManagement)
        const nameParts = permission.name.split('.');
        if (nameParts.length > 1) {
          pageName = nameParts[1]; // Pages.UserManagement -> UserManagement
          
          // Eğer alt izinse (Pages.Revir.View gibi), ana sayfayı al
          if (nameParts.length > 2) {
            pageName = nameParts[1]; // Pages.Revir.View -> Revir
          }
        }
      }
      
      pageKey = pageName.toLowerCase();
      
      // Sayfa açıklaması ve ikonunu belirle
      let pageDescription = '';
      let pageIcon = 'pi-file';
      
      switch (pageKey) {
        case 'usermanagement':
          pageDescription = 'Kullanıcı yönetimi sayfası izinleri';
          pageIcon = 'pi-users';
          pageName = 'Kullanıcı Yönetimi';
          break;
        case 'rolemanagement':
          pageDescription = 'Rol yönetimi sayfası izinleri';
          pageIcon = 'pi-id-card';
          pageName = 'Rol Yönetimi';
          break;
        case 'userdashboard':
          pageDescription = 'Kullanıcı paneli sayfası izinleri';
          pageIcon = 'pi-home';
          pageName = 'Kullanıcı Paneli';
          break;
        case 'revir':
          pageDescription = 'Revir sayfası izinleri';
          pageIcon = 'pi-heart';
          pageName = 'Revir';
          break;
        case 'bilgiislem':
          pageDescription = 'Bilgi İşlem sayfası izinleri';
          pageIcon = 'pi-desktop';
          pageName = 'Bilgi İşlem';
          break;
        default:
          // Eğer özel bir durum yoksa, izin açıklamasını kullan
          pageDescription = `${pageName} sayfası izinleri`;
          
          // İzin adına göre ikon belirle
          if (pageKey.includes('dashboard')) {
            pageIcon = 'pi-th-large';
          } else if (pageKey.includes('user')) {
            pageIcon = 'pi-user';
          } else if (pageKey.includes('report')) {
            pageIcon = 'pi-chart-bar';
          } else if (pageKey.includes('setting')) {
            pageIcon = 'pi-cog';
          }
      }
      
      // Sayfa grubunu oluştur veya güncelle
      if (!pageGroups[pageKey]) {
        pageGroups[pageKey] = {
          pageName: pageName,
          pageDescription: pageDescription,
          pageIcon: pageIcon,
          permissions: [],
          expanded: false
        };
      }
      
      // İzni sayfa grubuna ekle
      pageGroups[pageKey].permissions.push(permission);
    });
    
    // Sayfa gruplarını diziye dönüştür
    this.pagePermissions = Object.values(pageGroups);
    
    // Tüm sayfaları kapalı olarak başlat
    this.pagePermissions.forEach(page => {
      page.expanded = false;
    });
    
    // Sayfa gruplarını alfabetik olarak sırala
    this.pagePermissions.sort((a, b) => a.pageName.localeCompare(b.pageName));
  }
  
  // Sayfa genişletme/daraltma
  togglePageExpanded(index: number): void {
    // Eğer zaten açıksa kapat
    if (this.pagePermissions[index].expanded) {
      // Direkt olarak kapat, animasyon olmadan
      this.pagePermissions[index].expanded = false;
      return;
    }
    
    // Tüm kartları kapat
    this.pagePermissions.forEach((page, i) => {
      if (page.expanded) {
        // Direkt olarak kapat, animasyon olmadan
        page.expanded = false;
      }
    });
    
    // 50ms gecikme ile seçili kartı aç (DOM'un güncellenmesi için)
    setTimeout(() => {
      this.pagePermissions[index].expanded = true;
      
      // DOM'un güncellenmesini bekle
      setTimeout(() => {
        const cardElement = document.getElementById(`page-card-${index}`);
        
        if (cardElement) {
          // Kart açıldıysa görünür alana getir
          cardElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
      }, 50);
    }, 10);
  }

  savePermissions(): void {
    this.loading = true;
    console.log('İzinler kaydediliyor, userId:', this.userId);
    
    // Tüm sayfa izinlerinden seçili olanları topla
    const selectedPermissionIds: number[] = [];
    
    this.pagePermissions.forEach(page => {
      page.permissions.forEach(permission => {
        if (permission.isGranted) {
          selectedPermissionIds.push(permission.id);
        }
      });
    });
    
    console.log('Seçili izin ID\'leri:', selectedPermissionIds);
    
    // Kullanıcı izinlerini kaydet
    this.permissionService.assignPermissionsToUser(this.userId, selectedPermissionIds).subscribe({
      next: (result) => {
        this.loading = false;
        if (result) {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kullanıcı sayfa izinleri başarıyla güncellendi'
          });
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcı sayfa izinleri güncellenirken bir hata oluştu'
          });
        }
      },
      error: (error) => {
        console.error('Kullanıcı izinleri kaydedilirken hata:', error);
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcı sayfa izinleri güncellenirken bir hata oluştu'
        });
      }
    });
  }
  
  resetToRolePermissions(): void {
    this.loading = true;
    console.log('Kullanıcı izinleri rol izinlerine sıfırlanıyor, userId:', this.userId);
    
    this.permissionService.resetUserToRolePermissions(this.userId).subscribe({
      next: (result) => {
        if (result) {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kullanıcı izinleri rol izinlerine sıfırlandı'
          });
          // İzinleri yeniden yükle
          this.loadUserPermissions();
        } else {
          this.loading = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'İzinler sıfırlanırken bir hata oluştu'
          });
        }
      },
      error: (error) => {
        console.error('İzinler sıfırlanırken hata:', error);
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: error.error?.message || 'İzinler sıfırlanırken bir hata oluştu'
        });
      }
    });
  }
  
  goBack(): void {
    this.router.navigate(['/user-management']);
  }

  // Herhangi bir kart açık mı?
  anyCardExpanded(): boolean {
    return this.pagePermissions.some(page => page.expanded);
  }
  
  // Tüm kartları kapat
  closeAllCards(): void {
    this.pagePermissions.forEach(page => {
      page.expanded = false;
    });
  }
  
  // Belirli bir kartı kapat
  closeCard(index: number): void {
    // Direkt olarak kapat
    this.pagePermissions[index].expanded = false;
  }
} 