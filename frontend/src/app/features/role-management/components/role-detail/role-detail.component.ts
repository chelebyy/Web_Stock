import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Role } from '../../../../shared/models/role.model';
import { RoleService } from '../../services/role.service';
import { PermissionService } from '../../../../services/permission.service';
import { Permission, PermissionGroup } from '../../../../shared/models/permission.model';
import { forkJoin, catchError, of } from 'rxjs';
import { AccordionModule } from 'primeng/accordion';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastModule } from 'primeng/toast';

// ReferenceHandler.Preserve için ek type tanımı
interface PreserveFormat<T> {
  $id?: string;
  $values?: T[];
}

@Component({
    selector: 'app-role-detail',
    templateUrl: './role-detail.component.html',
    styleUrls: ['./role-detail.component.scss'],
    imports: [
        CommonModule,
        FormsModule,
        AccordionModule,
        ToggleButtonModule,
        CheckboxModule,
        ButtonModule,
        ProgressSpinnerModule,
        ToastModule
    ]
})
export class RoleDetailComponent implements OnInit {
  roleId: number = 0;
  role: Role | null = null;
  loading: boolean = true;
  permissionGroups: PermissionGroup[] = [];
  selectedPermissions: number[] = [];
  savingPermissions: boolean = false;
  loadError: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private roleService: RoleService,
    private permissionService: PermissionService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.roleId = +params['roleId'];
      this.loadRoleData();
    });
  }

  loadRoleData(): void {
    this.loading = true;
    this.loadError = false;

    // Rol ve izin verilerini paralel olarak yükle
    forkJoin({
      role: this.roleService.getRoleById(this.roleId).pipe(
        catchError(error => {
          console.error('Rol yüklenirken hata:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: `Rol bilgileri yüklenemedi: ${error.message || 'Bilinmeyen hata'}`,
            life: 5000
          });
          this.loadError = true;
          return of(null);
        })
      ),
      permissions: this.permissionService.getAllPermissions().pipe(
        catchError(error => {
          console.error('İzinler yüklenirken hata:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'İzin listesi yüklenemedi',
            life: 5000
          });
          this.loadError = true;
          return of([]);
        })
      ),
      rolePermissions: this.permissionService.getPermissionsByRoleId(this.roleId).pipe(
        catchError(error => {
          console.error('Rol izinleri yüklenirken hata:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Rol izinleri yüklenemedi',
            life: 5000
          });
          this.loadError = true;
          return of([]);
        })
      )
    }).subscribe({
      next: (data) => {
        this.role = data.role;
        
        if (!this.role) {
          this.loadError = true;
          this.loading = false;
          return;
        }
        
        // İzinleri gruplandır
        const permissions = data.permissions;
        const permissionMap = new Map<string, Permission[]>();
        
        permissions.forEach((permission: Permission) => {
          const groupName = permission.group || 'Diğer';
          if (!permissionMap.has(groupName)) {
            permissionMap.set(groupName, []);
          }
          permissionMap.get(groupName)?.push(permission);
        });
        
        // Grupları oluştur
        this.permissionGroups = Array.from(permissionMap.entries()).map(([groupName, perms]) => ({
          group: groupName,
          name: groupName,
          permissions: perms
        }));
        
        // Seçili izinleri ayarla
        this.selectedPermissions = data.rolePermissions.map((p: Permission) => p.id);
        
        // Test verileri ekle (geliştirme aşamasında)
        // this.addTestPermissionData();
        
        this.loading = false;
      },
      error: (error: any) => {
        console.error('Veri yüklenirken hata:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Veriler yüklenirken bir hata oluştu',
          life: 5000
        });
        this.loadError = true;
        this.loading = false;
      }
    });
  }

  private addTestPermissionData(): void {
    // Test verileri (geliştirme aşamasında)
    if (this.permissionGroups.length === 0) {
      this.permissionGroups = [
        {
          group: 'Kullanıcı Yönetimi',
          name: 'Kullanıcı Yönetimi',
          permissions: [
            { id: 1, name: 'Pages.UserManagement', displayName: 'Kullanıcı Yönetimi Sayfası', description: 'Kullanıcı yönetimi sayfasına erişim', group: 'Kullanıcı Yönetimi' },
            { id: 2, name: 'Users.Create', displayName: 'Kullanıcı Oluşturma', description: 'Yeni kullanıcı oluşturabilir', group: 'Kullanıcı Yönetimi' },
            { id: 3, name: 'Users.Edit', displayName: 'Kullanıcı Düzenleme', description: 'Kullanıcı bilgilerini düzenleyebilir', group: 'Kullanıcı Yönetimi' },
            { id: 4, name: 'Users.Delete', displayName: 'Kullanıcı Silme', description: 'Kullanıcıları silebilir', group: 'Kullanıcı Yönetimi' }
          ]
        },
        {
          group: 'Rol Yönetimi',
          name: 'Rol Yönetimi',
          permissions: [
            { id: 5, name: 'Pages.RoleManagement', displayName: 'Rol Yönetimi Sayfası', description: 'Rol yönetimi sayfasına erişim', group: 'Rol Yönetimi' },
            { id: 6, name: 'Roles.Create', displayName: 'Rol Oluşturma', description: 'Yeni rol oluşturabilir', group: 'Rol Yönetimi' },
            { id: 7, name: 'Roles.Edit', displayName: 'Rol Düzenleme', description: 'Rol bilgilerini düzenleyebilir', group: 'Rol Yönetimi' },
            { id: 8, name: 'Roles.Delete', displayName: 'Rol Silme', description: 'Rolleri silebilir', group: 'Rol Yönetimi' }
          ]
        }
      ];
      
      // Varsayılan seçili izinler
      this.selectedPermissions = [1, 5];
    }
  }

  savePermissions(): void {
    if (!this.role) return;
    
    this.savingPermissions = true;
    
    this.permissionService.assignPermissionsToRole(this.roleId, this.selectedPermissions).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Rol izinleri güncellendi',
          life: 3000
        });
        this.savingPermissions = false;
      },
      error: (error) => {
        console.error('İzinler güncellenirken hata:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'İzinler güncellenirken bir hata oluştu',
          life: 5000
        });
        this.savingPermissions = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/roles']);
  }

  isPermissionSelected(permissionId: number): boolean {
    return this.selectedPermissions.includes(permissionId);
  }

  togglePermission(permissionId: number): void {
    const index = this.selectedPermissions.indexOf(permissionId);
    if (index > -1) {
      this.selectedPermissions.splice(index, 1);
    } else {
      this.selectedPermissions.push(permissionId);
    }
    
    // Konsola seçili izinleri yazdır (debug için)
    console.log('Seçili izinler:', this.selectedPermissions);
  }

  getPermissionsByGroup(groupName: string): Permission[] {
    const group = this.permissionGroups.find(g => g.name === groupName);
    return group ? group.permissions : [];
  }

  /**
   * Sayfa izinlerini kategorilere göre gruplandırır
   * Örnek: Pages.UserManagement, Pages.RoleManagement -> Sayfalar kategorisi
   * @returns Kategori adları dizisi
   */
  getPagePermissionCategories(): string[] {
    const pagePermissions = this.permissionGroups
      .flatMap(group => group.permissions)
      .filter(permission => permission.name.startsWith('Pages.'));
    
    // Kategori adlarını çıkar (Pages. sonrası ilk kelime)
    const categories = pagePermissions.map(permission => {
      const parts = permission.name.split('.');
      if (parts.length >= 2) {
        // Pages.UserManagement -> User
        // Pages.RoleManagement -> Role
        // İlk kelimeyi al ve Management, Page, vs. kısmını çıkar
        let category = parts[1].replace('Management', '')
                              .replace('Page', '')
                              .replace('Dashboard', '');
        
        // Boş string olursa "Genel" olarak işaretle
        if (!category) {
          category = 'Genel';
        }
        
        return category;
      }
      return 'Diğer';
    });
    
    // Benzersiz kategorileri döndür
    return [...new Set(categories)].sort();
  }

  /**
   * Belirli bir grup dışındaki tüm grupları döndürür
   * @param excludeGroupName Hariç tutulacak grup adı
   * @returns Grup adları dizisi
   */
  getGroupsExcept(excludeGroupName: string): string[] {
    return this.permissionGroups
      .map(group => group.name)
      .filter(name => name !== excludeGroupName);
  }

  /**
   * Sayfa izinlerinin olup olmadığını kontrol eder
   * @returns Sayfa izinleri varsa true, yoksa false
   */
  hasPagePermissions(): boolean {
    return this.permissionGroups
      .some(group => group.permissions
        .some(permission => permission.name.startsWith('Pages.')));
  }

  /**
   * Belirli bir kategoriye ait sayfa izinlerini döndürür
   * @param category Kategori adı
   * @returns İzinler dizisi
   */
  getPagePermissionsByCategory(category: string): Permission[] {
    // Tüm sayfa izinlerini al
    const allPagePermissions = this.permissionGroups
      .flatMap(group => group.permissions)
      .filter(permission => permission.name.startsWith('Pages.'));
    
    // Kategoriye göre filtrele
    return allPagePermissions.filter(permission => {
      const parts = permission.name.split('.');
      if (parts.length >= 2) {
        const permCategory = parts[1].replace('Management', '')
                                    .replace('Page', '')
                                    .replace('Dashboard', '');
        
        // Boş kategori "Genel" olarak kabul edilir
        const normalizedPermCategory = permCategory || 'Genel';
        
        return normalizedPermCategory === category;
      }
      return false;
    });
  }

  /**
   * Sayfa izinleri dışındaki tüm izinleri döndürür
   * @returns İzinler dizisi
   */
  getNonPagePermissions(): Permission[] {
    return this.permissionGroups
      .flatMap(group => group.permissions)
      .filter(permission => !permission.name.startsWith('Pages.'));
  }

  /**
   * Tüm sayfa izinlerini seçer
   */
  selectAllPagePermissions(): void {
    const pagePermissionIds = this.permissionGroups
      .flatMap(group => group.permissions)
      .filter(permission => permission.name.startsWith('Pages.'))
      .map(permission => permission.id);
    
    // Mevcut seçili izinleri koru ve sayfa izinlerini ekle
    const currentNonPagePermissions = this.selectedPermissions
      .filter(id => {
        const permission = this.findPermissionById(id);
        return permission && !permission.name.startsWith('Pages.');
      });
    
    this.selectedPermissions = [...new Set([...currentNonPagePermissions, ...pagePermissionIds])];
  }

  /**
   * Tüm sayfa izinlerinin seçimini kaldırır
   */
  unselectAllPagePermissions(): void {
    // Sayfa izinleri dışındaki izinleri koru
    this.selectedPermissions = this.selectedPermissions
      .filter(id => {
        const permission = this.findPermissionById(id);
        return permission && !permission.name.startsWith('Pages.');
      });
  }

  /**
   * ID'ye göre izin nesnesini bulur
   * @param id İzin ID'si
   * @returns İzin nesnesi veya undefined
   */
  private findPermissionById(id: number): Permission | undefined {
    return this.permissionGroups
      .flatMap(group => group.permissions)
      .find(permission => permission.id === id);
  }
} 