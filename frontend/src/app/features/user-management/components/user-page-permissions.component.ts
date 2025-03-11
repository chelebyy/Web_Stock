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

// Servisler
import { PermissionService } from '../../../services/permission.service';
import { UserService } from '../../../services/user.service';

interface Permission {
  id: number;
  name: string;
  description: string;
  group: string;
  isGranted: boolean;
}

interface PermissionGroup {
  group: string;
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
        RippleModule
    ],
    providers: [MessageService],
    animations: [
        trigger('expandCollapse', [
            state('collapsed', style({
                height: '0',
                overflow: 'hidden',
                opacity: '0',
                padding: '0 1.5rem'
            })),
            state('expanded', style({
                height: '*',
                overflow: 'hidden',
                opacity: '1',
                padding: '1.5rem'
            })),
            transition('collapsed <=> expanded', [
                animate('200ms ease-in-out')
            ])
        ])
    ],
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

          <div class="permission-groups">
            <div class="permission-group-card" *ngFor="let group of permissionGroups">
              <div class="group-header" (click)="toggleGroup(group)" pRipple>
                <span class="group-title">{{ group.group }}</span>
                <i class="pi" [ngClass]="group.expanded ? 'pi-chevron-down' : 'pi-chevron-right'"></i>
              </div>
              
              <div class="group-content" [@expandCollapse]="group.expanded ? 'expanded' : 'collapsed'">
                <div class="permission-list">
                  <div class="permission-item" *ngFor="let permission of group.permissions">
                    <div class="permission-content">
                      <p-checkbox [(ngModel)]="permission.isGranted" 
                                [binary]="true" 
                                [inputId]="'perm_' + permission.id"
                                styleClass="custom-checkbox">
                      </p-checkbox>
                      <label [for]="'perm_' + permission.id" class="permission-label">
                        {{ permission.description || permission.name }}
                        <small class="permission-name">{{ permission.name }}</small>
                      </label>
                    </div>
                  </div>
                </div>
              </div>
            </div>
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

    .permission-groups {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .permission-group-card {
      background-color: #ffffff;
      border: 1px solid #e2e8f0;
      border-radius: 8px;
      overflow: hidden;
      transition: all 0.2s ease;
    }

    .permission-group-card:hover {
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
    }

    .group-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 1.25rem 1.5rem;
      background-color: #f8fafc;
      cursor: pointer;
      user-select: none;
      transition: all 0.2s ease;
    }

    .group-header:hover {
      background-color: #f1f5f9;
    }

    .group-title {
      font-size: 1.1rem;
      font-weight: 600;
      color: #1e3a8a;
    }

    .group-header i {
      font-size: 1rem;
      color: #64748b;
      transition: transform 0.2s ease;
    }

    .group-content {
      background-color: #ffffff;
    }
    
    .permission-list {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 1rem;
    }
    
    .permission-item {
      background-color: #f8fafc;
      border-radius: 8px;
      padding: 1rem;
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
    
    :host ::ng-deep .custom-checkbox {
      .p-checkbox {
        width: 20px;
        height: 20px;
      }

      .p-checkbox-box {
        border-radius: 4px;
        border: 2px solid #cbd5e1;
        width: 20px;
        height: 20px;
        transition: all 0.2s ease;
        background-color: #ffffff !important;

        &.p-highlight {
          background-color: #3b82f6 !important;
          border-color: #3b82f6 !important;

          .p-checkbox-icon {
            color: #ffffff !important;
            font-size: 12px !important;
          }
        }

        &:not(.p-disabled):hover {
          border-color: #3b82f6 !important;
          background-color: #ffffff !important;
        }

        &:not(.p-disabled).p-focus {
          box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2) !important;
          border-color: #3b82f6 !important;
        }
      }
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
      
      .permission-list {
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
  userId: number = 0;
  userName: string = '';
  loading: boolean = true;
  permissionGroups: PermissionGroup[] = [];
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
        this.userName = user.username || `${user.firstName} ${user.lastName}`;
        
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
                
                this.groupPermissions();
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
  
  groupPermissions(): void {
    // Önce tüm izinleri gruplarına göre ayır
    const groupedPermissions: { [key: string]: Permission[] } = {};
    
    // Henüz oluşturulmamış sayfaların izinlerini filtrele
    const filteredPermissions = this.allPermissions.filter(permission => {
      // Henüz oluşturulmamış sayfaların izinlerini filtrele
      return !(
        permission.name.startsWith('Pages.Reports') ||
        permission.name.startsWith('Pages.Settings') ||
        permission.name.startsWith('Pages.Egitim') ||
        permission.name.startsWith('Pages.StockManagement') ||
        permission.group === 'Stok Yönetimi'
      );
    });
    
    // Filtrelenmiş izinleri gruplarına göre ayır
    filteredPermissions.forEach(permission => {
      if (!groupedPermissions[permission.group]) {
        groupedPermissions[permission.group] = [];
      }
      groupedPermissions[permission.group].push(permission);
    });
    
    // Grupları PermissionGroup dizisine dönüştür
    this.permissionGroups = Object.keys(groupedPermissions).map(group => {
      return {
        group,
        permissions: groupedPermissions[group],
        expanded: group === 'Sayfa Erişimi' // Sayfa Erişimi grubunu varsayılan olarak aç
      };
    });
    
    // Grupları alfabetik olarak sırala
    this.permissionGroups.sort((a, b) => a.group.localeCompare(b.group));
    
    // Sayfa Erişimi grubunu en üste taşı
    const pageAccessIndex = this.permissionGroups.findIndex(g => g.group === 'Sayfa Erişimi');
    if (pageAccessIndex > 0) {
      const pageAccessGroup = this.permissionGroups.splice(pageAccessIndex, 1)[0];
      this.permissionGroups.unshift(pageAccessGroup);
    }
  }

  toggleGroup(group: PermissionGroup): void {
    group.expanded = !group.expanded;
  }
  
  savePermissions(): void {
    this.loading = true;
    console.log('İzinler kaydediliyor, userId:', this.userId);
    
    // Seçili izinlerin ID'lerini al
    const selectedPermissionIds = this.allPermissions
      .filter(p => p.isGranted)
      .map(p => p.id);
    
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
} 