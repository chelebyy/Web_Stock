import { Component, OnInit, ChangeDetectionStrategy, inject, signal, WritableSignal } from '@angular/core';
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
  isDirect: boolean;
}

interface PermissionGroup {
  group: string;
  permissions: Permission[];
}

@Component({
    selector: 'app-permission-management',
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
    changeDetection: ChangeDetectionStrategy.OnPush,
    template: `
    <div class="permission-management-container">
      <p-toast></p-toast>
      
      <div class="card">
        <p-toolbar>
          <div class="p-toolbar-group-start">
            <h2>{{ entityType() === 'role' ? 'Rol İzinleri' : 'Kullanıcı İzinleri' }}</h2>
          </div>
          <div class="p-toolbar-group-end">
            <button pButton icon="pi pi-save" label="Kaydet" (click)="savePermissions()" [disabled]="loading()"></button>
            <button pButton icon="pi pi-arrow-left" label="Geri Dön" class="p-button-secondary ml-2" (click)="goBack()"></button>
          </div>
        </p-toolbar>
        
        <div class="mt-3">
          <div *ngIf="entityType() === 'role'" class="mb-3">
            <h3>Rol: {{ roleName() }}</h3>
          </div>
          <div *ngIf="entityType() === 'user'" class="mb-3">
            <h3>Kullanıcı: {{ userName() }}</h3>
            <div class="mt-2">
              <button pButton icon="pi pi-refresh" label="Rol İzinlerine Sıfırla" 
                class="p-button-warning" (click)="resetToRolePermissions()" [disabled]="loading()"></button>
            </div>
          </div>
        </div>
        
        <div *ngIf="loading()" class="loading-container">
          <p-progressSpinner></p-progressSpinner>
          <span>Yükleniyor...</span>
        </div>
        
        <div *ngIf="!loading() && permissionGroups().length === 0" class="no-data">
          <p>İzin bulunamadı.</p>
        </div>
        
        <div *ngIf="!loading() && permissionGroups().length > 0" class="permission-groups">
          <p-accordion [multiple]="true" [activeIndex]="accordionActiveIndexes">
            <p-accordionTab *ngFor="let group of permissionGroups()" [header]="group.group">
              <div class="permission-list">
                <div *ngFor="let permission of group.permissions" class="permission-item" [class.indirect-permission]="!permission.isDirect && entityType() === 'user'">
                  <p-checkbox [(ngModel)]="permission.isGranted" [binary]="true" 
                    [inputId]="'permission_' + permission.id"></p-checkbox>
                  <label [for]="'permission_' + permission.id" class="ml-2">
                    <strong>{{ permission.name }}</strong>
                    <p class="description">{{ permission.description }}</p>
                    <span *ngIf="!permission.isDirect && entityType() === 'user'" class="indirect-badge">Rolden Geliyor</span>
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

    .indirect-permission {
      opacity: 0.7;
      background-color: #f8f9fa;
      border-left: 3px solid #ffc107;
      padding-left: 10px;
      margin-left: -13px;
    }

    .indirect-badge {
      font-size: 0.75rem;
      background-color: #ffc107;
      color: #212529;
      padding: 2px 6px;
      border-radius: 4px;
      margin-left: 8px;
      font-weight: 500;
    }
  `]
})
export class PermissionManagementComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly permissionService = inject(PermissionService);
  private readonly userService = inject(UserService);
  private readonly roleService = inject(RoleService);
  private readonly messageService = inject(MessageService);

  entityType: WritableSignal<'role' | 'user'> = signal('role');
  entityId = signal(0);
  roleName = signal('');
  userName = signal('');
  loading = signal(false);
  permissionGroups: WritableSignal<PermissionGroup[]> = signal([]);
  allPermissions: WritableSignal<Permission[]> = signal([]);
  
  get accordionActiveIndexes(): number[] {
    return Array.from({ length: this.permissionGroups().length }, (_, i) => i);
  }
  
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['roleId']) {
        this.entityType.set('role');
        this.entityId.set(+params['roleId']);
        this.loadRolePermissions();
      } else if (params['userId']) {
        this.entityType.set('user');
        this.entityId.set(+params['userId']);
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
    this.loading.set(true);
    
    // Önce rol bilgilerini al
    this.roleService.getRole(this.entityId()).subscribe({
      next: (role: Role) => {
        this.roleName.set(role.name);
        
        // Tüm izinleri al
        this.permissionService.getAllPermissions().subscribe({
          next: (permissions: ModelPermission[]) => {
            if (!permissions || !Array.isArray(permissions)) {
              this.allPermissions.set([]);
              this.loading.set(false);
              return;
            }
            
            const mappedPermissions = permissions.map(p => ({
              id: p.id,
              name: p.name,
              description: p.description || '',
              group: p.group || '',
              isGranted: false,
              isDirect: true
            }));
            this.allPermissions.set(mappedPermissions);
            
            // Rolün mevcut izinlerini al
            this.permissionService.getPermissionsByRoleId(this.entityId()).subscribe({
              next: (rolePermissions: ModelPermission[]) => {
                if (rolePermissions && Array.isArray(rolePermissions)) {
                  const rolePermissionIds = rolePermissions.map(p => p.id);
                  this.allPermissions.update(currentPermissions => 
                    currentPermissions.map(permission => ({
                      ...permission,
                      isGranted: rolePermissionIds.includes(permission.id)
                    }))
                  );
                }
                
                // İzinleri grupla
                this.groupPermissions();
                this.loading.set(false);
              },
              error: (error: any) => {
                this.loading.set(false);
                this.messageService.add({
                  severity: 'error',
                  summary: 'Hata',
                  detail: 'Rol izinleri yüklenirken bir hata oluştu.'
                });
              }
            });
          },
          error: (error: any) => {
            this.loading.set(false);
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Tüm izinler yüklenirken bir hata oluştu.'
            });
          }
        });
      },
      error: (error: any) => {
        this.loading.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Rol bilgileri alınamadı.'
        });
      }
    });
  }
  
  loadUserPermissions(): void {
    this.loading.set(true);
    
    this.userService.getUser(this.entityId()).subscribe({
        next: (user) => {
            this.userName.set(user.username || '');

            // Tüm izinleri al
            this.permissionService.getAllPermissions().subscribe({
                next: (allPermissions) => {
                    const mappedPermissions: Permission[] = allPermissions.map((p: ModelPermission) => ({
                        id: p.id,
                        name: p.name,
                        description: p.description || '',
                        group: p.group || '',
                        isGranted: false,
                        // TODO: Backend'i, kullanıcının doğrudan ve rolden gelen izinlerini ayırt edecek şekilde güncelle.
                        // Şimdilik hepsi doğrudan atanmış gibi görünüyor.
                        isDirect: true 
                    }));

                    // Kullanıcının mevcut izinlerini al
                    this.permissionService.getPermissionsByUserId(this.entityId()).subscribe({
                        next: (userPermissions) => {
                            const userPermissionIds = new Set(userPermissions.map((p: ModelPermission) => p.id));

                            const finalPermissions = mappedPermissions.map(p => {
                                return {
                                    ...p,
                                    isGranted: userPermissionIds.has(p.id),
                                };
                            });

                            this.allPermissions.set(finalPermissions);
                            this.groupPermissions();
                            this.loading.set(false);
                        },
                        error: (err: any) => {
                            this.loading.set(false);
                            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı izinleri alınamadı.' });
                        }
                    });
                },
                error: (err: any) => {
                    this.loading.set(false);
                    this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Tüm izinler yüklenirken bir hata oluştu.' });
                }
            });
        },
        error: (err: any) => {
            this.loading.set(false);
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı bilgileri alınamadı.' });
        }
    });
  }
  
  groupPermissions(): void {
    const grouped = this.allPermissions().reduce((acc, permission) => {
      const group = permission.group || 'Diğer';
      if (!acc[group]) {
        acc[group] = [];
      }
      acc[group].push(permission);
      return acc;
    }, {} as Record<string, Permission[]>);

    const sortedGroups = Object.keys(grouped).sort();

    const result: PermissionGroup[] = sortedGroups.map(groupName => ({
      group: groupName,
      permissions: grouped[groupName]
    }));
    
    this.permissionGroups.set(result);
  }
  
  savePermissions(): void {
    this.loading.set(true);
    const grantedPermissionIds = this.allPermissions()
      .filter(p => p.isGranted)
      .map(p => p.id);
      
    let saveObservable;
    
    if (this.entityType() === 'role') {
      saveObservable = this.permissionService.assignPermissionsToRole(this.entityId(), grantedPermissionIds);
    } else {
      // Kullanıcı için sadece doğrudan atanan izinleri gönder
      // TODO: Backend'de sadece direct permission'ları update edecek bir yapı kurulduğunda burayı güncelle.
      // Şimdilik tüm granted permission'lar gönderiliyor.
      saveObservable = this.permissionService.assignPermissionsToUser(this.entityId(), grantedPermissionIds);
    }
    
    saveObservable.subscribe({
      next: () => {
        this.loading.set(false);
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'İzinler başarıyla güncellendi.'
        });
      },
      error: (error: any) => {
        this.loading.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'İzinler güncellenirken bir hata oluştu.'
        });
      }
    });
  }
  
  resetToRolePermissions(): void {
      if (this.entityType() !== 'user') return;
      this.loading.set(true);

      this.permissionService.resetUserToRolePermissions(this.entityId()).subscribe({
          next: () => {
              this.loading.set(false);
              this.messageService.add({
                  severity: 'success',
                  summary: 'Başarılı',
                  detail: 'Kullanıcı izinleri rol izinlerine sıfırlandı.'
              });
              this.loadUserPermissions();
          },
          error: (err: any) => {
              this.loading.set(false);
              this.messageService.add({
                  severity: 'error',
                  summary: 'Hata',
                  detail: 'İzinler sıfırlanırken bir hata oluştu.'
              });
          }
      });
  }
  
  goBack(): void {
    if (this.entityType() === 'role') {
      this.router.navigate(['/role-management']);
    } else {
      this.router.navigate(['/user-management']);
    }
  }
} 