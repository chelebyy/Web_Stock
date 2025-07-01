import { Injectable, signal, WritableSignal } from '@angular/core';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api';

import { PermissionService } from '../../../services/permission.service';
import { UserService } from '../../../services/user.service';
import { User } from '../../../shared/models/user.model';
import { Permission as ModelPermission } from '../../../shared/models/permission.model';
import { PagePermission } from '../components/permission-card/permission-card.component';

// `user-page-permissions.component` dosyasından taşınan arayüz
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

@Injectable()
export class UserPermissionStateService {
  private readonly permissionService = inject(PermissionService);
  private readonly userService = inject(UserService);
  private readonly messageService = inject(MessageService);

  // Sinyaller (Signals)
  public userId = signal(0);
  public userName = signal('');
  public loading = signal(true);
  public pagePermissions: WritableSignal<PagePermission[]> = signal([]);
  private allPermissions: WritableSignal<Permission[]> = signal([]);

  public async initialize(userId: number): Promise<void> {
    this.userId.set(userId);
    this.loading.set(true);
    await this.loadUserPermissions();
    this.loading.set(false);
  }

  public async loadUserPermissions(): Promise<void> {
    try {
      const user = await this.userService.getUserById(this.userId()).toPromise();
      if (user) {
        this.userName.set(user.fullName || `${user.adi} ${user.soyadi}` || user.sicil || '');
      }

      const permissions = await this.permissionService.getPermissionsByUserId(this.userId()).toPromise();
      if (permissions) {
        const mappedPermissions: Permission[] = permissions.map((p: ModelPermission) => ({
            id: p.id,
            name: p.name,
            description: p.description,
            group: p.group,
            isGranted: p.isGranted || false
        }));
        this.allPermissions.set(mappedPermissions);
        this.groupPermissionsByPage();
      }
    } catch (error) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İzinler yüklenirken bir hata oluştu.' });
      console.error(error);
    }
  }
  
  public groupPermissionsByPage(): void {
    const grouped = this.allPermissions().reduce((acc, permission) => {
        const pageName = permission.group || 'Diğer';
        if (!acc[pageName]) {
            acc[pageName] = {
                pageName: pageName,
                pageDescription: this.getPageDescription(pageName),
                pageIcon: this.getPageIcon(pageName),
                permissions: [],
                expanded: false,
            };
        }
        acc[pageName].permissions.push(permission);
        return acc;
    }, {} as { [key: string]: PagePermission });

    this.pagePermissions.set(Object.values(grouped));
  }

  public async savePermissions(): Promise<void> {
    const grantedPermissionIds = this.allPermissions()
      .filter(p => p.isGranted)
      .map(p => p.id);

    try {
      await this.permissionService.assignPermissionsToUser(this.userId(), grantedPermissionIds).toPromise();
      this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'İzinler başarıyla güncellendi.' });
    } catch (error) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İzinler güncellenirken bir hata oluştu.' });
      console.error(error);
    }
  }

  public async resetToRolePermissions(): Promise<void> {
    try {
        await this.permissionService.resetUserToRolePermissions(this.userId()).toPromise();
        this.messageService.add({ severity: 'info', summary: 'Başarılı', detail: 'İzinler rol varsayılanlarına sıfırlandı. Sayfa yenileniyor...' });
        // Yeniden yükle
        await this.loadUserPermissions();
    } catch (error) {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İzinler sıfırlanırken bir hata oluştu.' });
        console.error(error);
    }
  }
  
  public togglePageExpanded(index: number): void {
    this.pagePermissions.update(pages => {
        // Genişletilmiş kart varsa ve tıklanan o değilse, hiçbir şey yapma
        const isAnyExpanded = pages.some(p => p.expanded);
        if (isAnyExpanded && !pages[index].expanded) {
            return pages;
        }

        // Tıklanan kartı genişlet/daralt
        const newPages = pages.map((page, i) => ({
            ...page,
            expanded: i === index ? !page.expanded : page.expanded
        }));
        
        return newPages;
    });
  }

  public closeAllCards(): void {
    this.pagePermissions.update(pages => pages.map(p => ({ ...p, expanded: false })));
  }

  public closeCard(index: number): void {
    this.pagePermissions.update(pages => {
        pages[index].expanded = false;
        return [...pages];
    });
  }

  public anyCardExpanded(): boolean {
    return this.pagePermissions().some(p => p.expanded);
  }

  private getPageDescription(pageName: string): string {
    const descriptions: { [key: string]: string } = {
        'UserManagement': 'Kullanıcıları yönetin, roller atayın ve izinleri düzenleyin.',
        'RoleManagement': 'Roller oluşturun, düzenleyin ve rollere varsayılan izinler atayın.',
        'Dashboard': 'Sistemin genel durumunu ve istatistikleri görüntüleyin.',
        'ProductManagement': 'Ürünleri, kategorileri ve stok durumunu yönetin.'
    };
    return descriptions[pageName] || 'Çeşitli sistem izinleri.';
  }

  private getPageIcon(pageName: string): string {
    const icons: { [key: string]: string } = {
        'UserManagement': 'pi-users',
        'RoleManagement': 'pi-shield',
        'Dashboard': 'pi-chart-bar',
        'ProductManagement': 'pi-box'
    };
    return icons[pageName] || 'pi-cog';
  }
} 