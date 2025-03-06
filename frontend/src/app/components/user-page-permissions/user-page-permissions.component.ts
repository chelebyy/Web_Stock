import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

// PrimeNG Modülleri
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { CardModule } from 'primeng/card';
import { MessageService } from 'primeng/api';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { AccordionModule } from 'primeng/accordion';
import { DividerModule } from 'primeng/divider';

// Servisler
import { PermissionService } from '../../services/permission.service';
import { UserService } from '../../services/user.service';

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
}

@Component({
  selector: 'app-user-page-permissions',
  standalone: true,
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
    AccordionModule,
    DividerModule
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

          <p-accordion [multiple]="true" styleClass="custom-accordion">
            <p-accordionTab *ngFor="let group of permissionGroups" [header]="group.group" styleClass="permission-group">
              <div class="permission-list">
                <div class="permission-item" *ngFor="let permission of group.permissions">
                  <div class="permission-content">
                    <p-checkbox [(ngModel)]="permission.isGranted" 
                                [binary]="true" 
                                [label]="permission.description || permission.name"
                                styleClass="custom-checkbox">
                    </p-checkbox>
                    <small class="permission-name">{{ permission.name }}</small>
                  </div>
                </div>
              </div>
            </p-accordionTab>
          </p-accordion>
          
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
      background-color: var(--surface-ground);
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
    
    :host ::ng-deep .custom-accordion .p-accordion-header a {
      background-color: #f8fafc;
      color: #1e3a8a;
      font-weight: 600;
      padding: 1.25rem;
      border-radius: 8px;
      transition: all 0.2s ease;
    }
    
    :host ::ng-deep .custom-accordion .p-accordion-header:not(.p-disabled).p-highlight a {
      background-color: #eff6ff;
      border-color: #bfdbfe;
    }
    
    :host ::ng-deep .custom-accordion .p-accordion-header:not(.p-disabled) a:hover {
      background-color: #eff6ff;
    }
    
    :host ::ng-deep .custom-accordion .p-accordion-header-icon {
      color: #3b82f6;
    }
    
    :host ::ng-deep .custom-accordion .p-accordion-content {
      background-color: #ffffff;
      border: 1px solid #e2e8f0;
      border-top: none;
      border-radius: 0 0 8px 8px;
      padding: 1.5rem;
    }
    
    .permission-list {
      display: flex;
      flex-wrap: wrap;
      gap: 1rem;
    }
    
    .permission-item {
      flex: 1 1 300px;
      padding: 1rem;
      border: 1px solid #e2e8f0;
      border-radius: 8px;
      background-color: #f8fafc;
      transition: all 0.2s ease;
    }
    
    .permission-item:hover {
      background-color: #eff6ff;
      transform: translateY(-2px);
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
    }
    
    .permission-content {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }
    
    :host ::ng-deep .custom-checkbox .p-checkbox-box {
      border: 2px solid #cbd5e1;
    }
    
    :host ::ng-deep .custom-checkbox .p-checkbox-box.p-highlight {
      background-color: #3b82f6;
      border-color: #3b82f6;
    }
    
    .permission-name {
      display: block;
      color: #64748b;
      font-size: 0.8rem;
      padding-left: 1.75rem;
    }
    
    .action-buttons {
      display: flex;
      justify-content: flex-end;
      gap: 1rem;
      margin-top: 2rem;
      padding-top: 1.5rem;
      border-top: 1px solid #f1f5f9;
    }
    
    .save-button {
      background: linear-gradient(135deg, #22c55e, #16a34a);
      border: none;
      box-shadow: 0 4px 6px rgba(22, 163, 74, 0.2);
      border-radius: 8px;
      font-weight: 600;
      padding: 0.75rem 1.5rem;
      transition: all 0.3s ease;
    }
    
    .save-button:hover {
      background: linear-gradient(135deg, #16a34a, #15803d);
      transform: translateY(-2px);
      box-shadow: 0 6px 10px rgba(22, 163, 74, 0.25);
    }
    
    .reset-button {
      background-color: #f8fafc;
      color: #64748b;
      border: 1px solid #e2e8f0;
      border-radius: 8px;
      font-weight: 600;
      padding: 0.75rem 1.5rem;
      transition: all 0.3s ease;
    }
    
    .reset-button:hover {
      background-color: #f1f5f9;
      color: #475569;
      transform: translateY(-2px);
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
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
        padding: 1rem;
      }
      
      .permission-item {
        flex: 1 1 100%;
      }
      
      .action-buttons {
        flex-direction: column;
        gap: 1rem;
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
    console.log('groupPermissions çağrıldı, allPermissions:', this.allPermissions);
    
    // İzinleri gruplarına göre ayır
    const groups: { [key: string]: Permission[] } = {};
    
    if (!this.allPermissions || !Array.isArray(this.allPermissions)) {
      console.error('allPermissions dizi değil veya boş:', this.allPermissions);
      this.permissionGroups = [];
      return;
    }
    
    this.allPermissions.forEach(permission => {
      if (!permission.group) {
        console.warn('Grup bilgisi olmayan izin:', permission);
        permission.group = 'Diğer';
      }
      
      if (!groups[permission.group]) {
        groups[permission.group] = [];
      }
      groups[permission.group].push(permission);
    });
    
    // Grupları diziye dönüştür
    this.permissionGroups = Object.keys(groups).map(group => ({
      group,
      permissions: groups[group]
    }));
    
    // Grupları alfabetik sırala
    this.permissionGroups.sort((a, b) => a.group.localeCompare(b.group));
    
    console.log('Oluşturulan izin grupları:', this.permissionGroups);
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