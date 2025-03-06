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
    <div class="card">
      <div class="card-header">
        <div class="d-flex justify-content-between align-items-center">
          <h2>Kullanıcı Sayfa İzinleri</h2>
          <button pButton pRipple icon="pi pi-arrow-left" label="Geri Dön" 
                  class="p-button-outlined" (click)="goBack()"></button>
        </div>
        <p class="text-muted" *ngIf="userName">Kullanıcı: {{ userName }}</p>
      </div>
      
      <div class="card-body">
        <p-progressSpinner *ngIf="loading" styleClass="w-4rem h-4rem" 
                          strokeWidth="8" fill="var(--surface-ground)" 
                          animationDuration=".5s"></p-progressSpinner>
        
        <div *ngIf="!loading">
          <p-accordion [multiple]="true">
            <p-accordionTab *ngFor="let group of permissionGroups" [header]="group.group">
              <div class="permission-list">
                <div class="permission-item" *ngFor="let permission of group.permissions">
                  <p-checkbox [(ngModel)]="permission.isGranted" 
                              [binary]="true" 
                              [label]="permission.description || permission.name">
                  </p-checkbox>
                  <small class="permission-name">{{ permission.name }}</small>
                </div>
              </div>
            </p-accordionTab>
          </p-accordion>
          
          <div class="button-container mt-4">
            <button pButton pRipple label="Kaydet" icon="pi pi-check" 
                    class="p-button-success" (click)="savePermissions()"></button>
            <button pButton pRipple label="Rol İzinlerine Sıfırla" icon="pi pi-refresh" 
                    class="p-button-secondary ml-2" (click)="resetToRolePermissions()"></button>
          </div>
        </div>
      </div>
    </div>
    
    <p-toast position="top-right"></p-toast>
  `,
  styles: [`
    .permission-list {
      display: flex;
      flex-wrap: wrap;
      gap: 1rem;
    }
    
    .permission-item {
      flex: 1 1 300px;
      padding: 0.5rem;
      border: 1px solid #f0f0f0;
      border-radius: 4px;
      background-color: #fafafa;
    }
    
    .permission-name {
      display: block;
      color: #666;
      margin-top: 0.25rem;
      font-size: 0.8rem;
    }
    
    .button-container {
      margin-top: 1rem;
      display: flex;
      justify-content: flex-end;
      gap: 0.5rem;
    }
    
    .ml-2 {
      margin-left: 0.5rem;
    }
    
    .mt-4 {
      margin-top: 1rem;
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