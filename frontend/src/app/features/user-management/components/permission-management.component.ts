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
import { PermissionService } from '../../../services/permission.service';
import { UserService } from '../../../services/user.service';
import { RoleService } from '../../../services/role.service';
import { Role } from '../../../shared/models/role.model';
import { Permission as ModelPermission } from '../../../shared/models/permission.model';

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
    selector: 'app-permission-management',
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
    <div class="permission-management-container">
      <p-toast></p-toast>
      
      <div class="card">
        <p-toolbar>
          <div class="p-toolbar-group-start">
            <h2>{{ entityType === 'role' ? 'Rol İzinleri' : 'Kullanıcı İzinleri' }}</h2>
          </div>
          <div class="p-toolbar-group-end">
            <button pButton icon="pi pi-save" label="Kaydet" (click)="savePermissions()" [disabled]="loading"></button>
            <button pButton icon="pi pi-arrow-left" label="Geri Dön" class="p-button-secondary ml-2" (click)="goBack()"></button>
          </div>
        </p-toolbar>
        
        <div class="mt-3">
          <div *ngIf="entityType === 'role'" class="mb-3">
            <h3>Rol: {{ roleName }}</h3>
          </div>
          <div *ngIf="entityType === 'user'" class="mb-3">
            <h3>Kullanıcı: {{ userName }}</h3>
            <div class="mt-2">
              <button pButton icon="pi pi-refresh" label="Rol İzinlerine Sıfırla" 
                class="p-button-warning" (click)="resetToRolePermissions()" [disabled]="loading"></button>
            </div>
          </div>
        </div>
        
        <div *ngIf="loading" class="loading-container">
          <p-progressSpinner></p-progressSpinner>
          <span>Yükleniyor...</span>
        </div>
        
        <div *ngIf="!loading && permissionGroups.length === 0" class="no-data">
          <p>İzin bulunamadı.</p>
        </div>
        
        <div *ngIf="!loading && permissionGroups.length > 0" class="permission-groups">
          <p-accordion [multiple]="true">
            <p-accordionTab *ngFor="let group of permissionGroups" [header]="group.group">
              <div class="permission-list">
                <div *ngFor="let permission of group.permissions" class="permission-item">
                  <p-checkbox [(ngModel)]="permission.isGranted" [binary]="true" 
                    [inputId]="'permission_' + permission.id"></p-checkbox>
                  <label [for]="'permission_' + permission.id" class="ml-2">
                    <strong>{{ permission.name }}</strong>
                    <p class="description">{{ permission.description }}</p>
                  </label>
                </div>
              </div>
            </p-accordionTab>
          </p-accordion>
        </div>
      </div>
    </div>
  `,
    styles: [`
    .permission-management-container {
      padding: 1rem;
    }
    
    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 2rem;
    }
    
    .no-data {
      text-align: center;
      padding: 2rem;
      color: #666;
    }
    
    .permission-groups {
      margin-top: 1rem;
    }
    
    .permission-list {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }
    
    .permission-item {
      display: flex;
      align-items: flex-start;
    }
    
    .description {
      margin: 0.25rem 0 0 0;
      color: #666;
      font-size: 0.9rem;
    }
    
    .ml-2 {
      margin-left: 0.5rem;
    }
    
    .mt-2 {
      margin-top: 0.5rem;
    }
    
    .mt-3 {
      margin-top: 1rem;
    }
    
    .mb-3 {
      margin-bottom: 1rem;
    }
  `]
})
export class PermissionManagementComponent implements OnInit {
  entityType: 'role' | 'user' = 'role';
  entityId = 0;
  roleName = '';
  userName = '';
  loading = false;
  permissionGroups: PermissionGroup[] = [];
  allPermissions: Permission[] = [];
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private permissionService: PermissionService,
    private userService: UserService,
    private roleService: RoleService,
    private messageService: MessageService
  ) {}
  
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['roleId']) {
        this.entityType = 'role';
        this.entityId = +params['roleId'];
        this.loadRolePermissions();
      } else if (params['userId']) {
        this.entityType = 'user';
        this.entityId = +params['userId'];
        this.loadUserPermissions();
      } else {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Geçersiz parametre'
        });
        this.goBack();
      }
    });
  }
  
  loadRolePermissions(): void {
    this.loading = true;
    
    // Önce rol bilgilerini al
    this.roleService.getRole(this.entityId).subscribe({
      next: (role: Role) => {
        this.roleName = role.name;
        
        // Tüm izinleri al
        this.permissionService.getAllPermissions().subscribe({
          next: (permissions: ModelPermission[]) => {
            if (!permissions || !Array.isArray(permissions)) {
              this.allPermissions = [];
              this.loading = false;
              return;
            }
            
            this.allPermissions = permissions.map(p => ({
              id: p.id,
              name: p.name,
              description: p.description || '',
              group: p.group || '',
              isGranted: false
            }));
            
            // Rolün mevcut izinlerini al
            this.permissionService.getPermissionsByRoleId(this.entityId).subscribe({
              next: (rolePermissions: ModelPermission[]) => {
                if (rolePermissions && Array.isArray(rolePermissions)) {
                  const rolePermissionIds = rolePermissions.map(p => p.id);
                  this.allPermissions.forEach(permission => {
                    permission.isGranted = rolePermissionIds.includes(permission.id);
                  });
                }
                
                // İzinleri grupla
                this.groupPermissions();
                this.loading = false;
              },
              error: (error: any) => {
                this.loading = false;
                this.messageService.add({
                  severity: 'error',
                  summary: 'Hata',
                  detail: 'Rol izinleri yüklenirken bir hata oluştu'
                });
              }
            });
          },
          error: (error: any) => {
            this.loading = false;
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'İzinler yüklenirken bir hata oluştu'
            });
          }
        });
      },
      error: (error: any) => {
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Rol bilgisi yüklenirken bir hata oluştu'
        });
      }
    });
  }
  
  loadUserPermissions(): void {
    this.loading = true;
    
    // Önce kullanıcı bilgilerini al
    this.userService.getUser(this.entityId).subscribe({
      next: (user: any) => {
        this.userName = user.username || `Kullanıcı #${this.entityId}`;
        
        // Tüm izinleri al
        this.permissionService.getAllPermissions().subscribe({
          next: (permissions: ModelPermission[]) => {
            if (!permissions || !Array.isArray(permissions)) {
              this.allPermissions = [];
              this.loading = false;
              return;
            }
            
            this.allPermissions = permissions.map(p => ({
              id: p.id,
              name: p.name,
              description: p.description || '',
              group: p.group || '',
              isGranted: false
            }));
            
            // Kullanıcının mevcut izinlerini al
            this.permissionService.getPermissionsByUserId(this.entityId).subscribe({
              next: (userPermissions: ModelPermission[]) => {
                if (userPermissions && Array.isArray(userPermissions)) {
                  const userPermissionIds = userPermissions.map(p => p.id);
                  this.allPermissions.forEach(permission => {
                    permission.isGranted = userPermissionIds.includes(permission.id);
                  });
                }
                
                // İzinleri grupla
                this.groupPermissions();
                this.loading = false;
              },
              error: (error: any) => {
                this.loading = false;
                this.messageService.add({
                  severity: 'error',
                  summary: 'Hata',
                  detail: 'Kullanıcı izinleri yüklenirken bir hata oluştu'
                });
              }
            });
          },
          error: (error: any) => {
            this.loading = false;
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'İzinler yüklenirken bir hata oluştu'
            });
          }
        });
      },
      error: (error: any) => {
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
    // İzinleri gruplarına göre ayır
    const groups: Record<string, Permission[]> = {};
    
    if (!this.allPermissions || !Array.isArray(this.allPermissions)) {
      this.permissionGroups = [];
      return;
    }
    
    this.allPermissions.forEach(permission => {
      if (!permission.group) {
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
  }
  
  savePermissions(): void {
    this.loading = true;
    
    // Seçili izinlerin ID'lerini al
    const selectedPermissionIds = this.allPermissions
      .filter(p => p.isGranted)
      .map(p => p.id);
    
    if (this.entityType === 'role') {
      // Rol izinlerini kaydet
      this.permissionService.assignPermissionsToRole(this.entityId, selectedPermissionIds).subscribe({
        next: (result: boolean) => {
          this.loading = false;
          if (result) {
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Rol izinleri başarıyla kaydedildi'
            });
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Rol izinleri kaydedilirken bir hata oluştu'
            });
          }
        },
        error: (error: any) => {
          this.loading = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Rol izinleri kaydedilirken bir hata oluştu'
          });
        }
      });
    } else if (this.entityType === 'user') {
      // Kullanıcı izinlerini kaydet
      this.permissionService.assignPermissionsToUser(this.entityId, selectedPermissionIds).subscribe({
        next: (result: boolean) => {
          this.loading = false;
          if (result) {
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Kullanıcı izinleri başarıyla kaydedildi'
            });
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Kullanıcı izinleri kaydedilirken bir hata oluştu'
            });
          }
        },
        error: (error: any) => {
          this.loading = false;
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcı izinleri kaydedilirken bir hata oluştu'
          });
        }
      });
    }
  }
  
  resetToRolePermissions(): void {
    if (this.entityType !== 'user') return;
    
    this.loading = true;
    this.permissionService.resetUserToRolePermissions(this.entityId).subscribe({
      next: (result: boolean) => {
        this.loading = false;
        if (result) {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Kullanıcı izinleri rol izinlerine sıfırlandı'
          });
          // İzinleri yeniden yükle
          this.loadUserPermissions();
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcı izinleri sıfırlanırken bir hata oluştu'
          });
        }
      },
      error: (error: any) => {
        this.loading = false;
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcı izinleri sıfırlanırken bir hata oluştu'
        });
      }
    });
  }
  
  goBack(): void {
    if (this.entityType === 'role') {
      this.router.navigate(['/roles']);
    } else {
      this.router.navigate(['/user-management']);
    }
  }
} 