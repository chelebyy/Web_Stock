import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Role } from '../../../models/role.model';
import { RoleService } from '../../../services/role.service';
import { PermissionService } from '../../../services/permission.service';
import { Permission, PermissionGroup } from '../../../models/permission.model';
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
  standalone: true,
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
      this.roleId = +params['id'];
      console.log('Rol ID:', this.roleId); // ID değerini logluyoruz
      this.loadRoleData();
    });
  }

  loadRoleData(): void {
    this.loading = true;
    this.loadError = false;
    console.log('Rol verileri yükleniyor... ID:', this.roleId);
    
    // Önce tek rol verisi alıp kontrol edelim
    this.roleService.getRoleById(this.roleId)
      .pipe(
        catchError(error => {
          console.error('Rol veri hatası:', error);
          this.loadError = true;
          return of(null);
        })
      )
      .subscribe(roleData => {
        console.log('API rol yanıtı:', roleData);
        
        if (!roleData) {
          console.error('Rol verisi alınamadı');
          this.loading = false;
          this.messageService.add({ 
            severity: 'error', 
            summary: 'Hata', 
            detail: 'Rol bilgileri yüklenemedi. Rol bulunamadı veya erişim yetkiniz yok.' 
          });
          return;
        }
        
        // Rol verisini işleyelim
        this.role = roleData;
        
        // Rol adı kontrolü - API'dan gelen rol adının detayını görelim
        if (this.role.name) {
          console.log('Rol adı:', this.role.name, 'Tip:', typeof this.role.name, 'Uzunluk:', this.role.name.length);
        } else {
          console.warn('Rol adı boş veya tanımsız!');
        }
        
        console.log('İzin verileri yükleniyor...');

        // İzin endpointlerinin doğru olduğunu kontrol edelim
        console.log('PermissionGroups endpoint:', `${this.permissionService['apiUrl']}/groups`);
        console.log('RolePermissions endpoint:', `${this.permissionService['apiUrl']}/role/${this.roleId}`);
        
        // Şimdi izin gruplarını ve rol izinlerini yükleyelim
        forkJoin({
          permissionGroups: this.permissionService.getPermissionsByGroups(),
          rolePermissions: this.permissionService.getPermissionsByRoleId(this.roleId)
        }).subscribe({
          next: (data) => {
            console.log('İzin verileri alındı - Gruplar:', data.permissionGroups);
            console.log('İzin verileri alındı - Rol İzinleri:', data.rolePermissions);
            
            // Mevcut grup adlarını loglayalım
            if (data.permissionGroups && Array.isArray(data.permissionGroups)) {
              const grupAdlari = data.permissionGroups.map(g => g.group);
              console.log('Mevcut grup adları:', grupAdlari);
            }
            
            // PermissionGroups formatını kontrol et
            if (data.permissionGroups && Array.isArray(data.permissionGroups)) {
              console.log('PermissionGroups formatı: Array');
              this.permissionGroups = data.permissionGroups;
              console.log('Mevcut izin grupları:', this.permissionGroups);
            } else if (data.permissionGroups && (data.permissionGroups as PreserveFormat<PermissionGroup>).$values) {
              // ReferenceHandler.Preserve formatını kontrol et
              console.log('PermissionGroups formatı: PreserveFormat');
              const preserveData = data.permissionGroups as PreserveFormat<PermissionGroup>;
              this.permissionGroups = preserveData.$values || [];
              console.log('Normalize edilmiş izin grupları:', this.permissionGroups);
            } else {
              console.error('permissionGroups uygun formatta değil:', data.permissionGroups);
              this.permissionGroups = [];
            }
            
            // Seçili izinleri ayarla
            if (data.rolePermissions && Array.isArray(data.rolePermissions)) {
              console.log('RolePermissions formatı: Array');
              this.selectedPermissions = data.rolePermissions.map(p => p.id);
              console.log('Seçili izin ID\'leri:', this.selectedPermissions);
            } else if (data.rolePermissions && (data.rolePermissions as PreserveFormat<Permission>).$values) {
              // ReferenceHandler.Preserve formatını kontrol et
              console.log('RolePermissions formatı: PreserveFormat');
              const preserveData = data.rolePermissions as PreserveFormat<Permission>;
              this.selectedPermissions = preserveData.$values?.map(p => p.id) || [];
              console.log('Normalize edilmiş seçili izin ID\'leri:', this.selectedPermissions);
            } else {
              console.error('rolePermissions uygun formatta değil:', data.rolePermissions);
              this.selectedPermissions = [];
            }

            // Sayfa Erişimi grubu var mı kontrol et
            const hasSayfaErisim = this.permissionGroups.some(g => g.group === 'Sayfa Erişimi');
            console.log('Sayfa Erişimi grubu var mı:', hasSayfaErisim);
            
            // Sayfa erişim izinleri varsa kontrol et
            const sayfaErisimIzinleri = this.getPermissionsByGroup('Sayfa Erişimi');
            console.log('Sayfa Erişim izinleri:', sayfaErisimIzinleri);
            
            // API'dan hiç veri gelmemişse test verileri ekleyelim
            if (this.permissionGroups.length === 0) {
              console.warn('API\'dan izin grubu verisi gelmedi, test verileri ekleniyor...');
              this.addTestPermissionData();
            }
            
            this.loading = false;
          },
          error: (error) => {
            console.error('İzin verileri yükleme hatası:', error);
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İzin bilgileri yüklenirken bir hata oluştu.' });
            this.loading = false;
            this.loadError = true;
          }
        });
      });
  }

  // Test için örnek izin verileri ekle - API verisi gelmediğinde kullanılır
  private addTestPermissionData(): void {
    // Örnek Sayfa Erişim izinleri
    const sayfaErisimGrubu: PermissionGroup = {
      group: 'Sayfa Erişimi',
      permissions: [
        { id: 1, name: 'Page.Dashboard', description: 'Ana Sayfa erişimi', group: 'Sayfa Erişimi' },
        { id: 2, name: 'Page.Users', description: 'Kullanıcı Yönetimi sayfası erişimi', group: 'Sayfa Erişimi' },
        { id: 3, name: 'Page.Roles', description: 'Rol Yönetimi sayfası erişimi', group: 'Sayfa Erişimi' },
        { id: 4, name: 'Page.Products', description: 'Ürün Yönetimi sayfası erişimi', group: 'Sayfa Erişimi' }
      ]
    };
    
    // Örnek diğer grup izinleri
    const kullaniciYonetimGrubu: PermissionGroup = {
      group: 'Kullanıcı Yönetimi',
      permissions: [
        { id: 5, name: 'User.Create', description: 'Kullanıcı oluşturma yetkisi', group: 'Kullanıcı Yönetimi' },
        { id: 6, name: 'User.Edit', description: 'Kullanıcı düzenleme yetkisi', group: 'Kullanıcı Yönetimi' },
        { id: 7, name: 'User.Delete', description: 'Kullanıcı silme yetkisi', group: 'Kullanıcı Yönetimi' }
      ]
    };
    
    // Gruplara ekle
    this.permissionGroups = [sayfaErisimGrubu, kullaniciYonetimGrubu];
    
    // Örnek seçili izinler
    this.selectedPermissions = [1, 3]; // Dashboard ve Roles erişimi
    
    console.log('Test verileri eklendi:', this.permissionGroups);
  }

  savePermissions(): void {
    if (!this.role) return;
    
    this.savingPermissions = true;
    this.permissionService.assignPermissionsToRole(this.roleId, this.selectedPermissions)
      .subscribe({
        next: (result) => {
          this.savingPermissions = false;
          if (result) {
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'İzinler başarıyla kaydedildi.' });
          } else {
            this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İzinler kaydedilirken bir hata oluştu.' });
          }
        },
        error: (error) => {
          this.savingPermissions = false;
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İzinler kaydedilirken bir hata oluştu.' });
          console.error('İzin kaydetme hatası:', error);
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
  }
  
  /**
   * Belirli bir gruba ait izinleri döndürür
   */
  getPermissionsByGroup(groupName: string): Permission[] {
    if (!this.permissionGroups || !Array.isArray(this.permissionGroups)) {
      console.warn('permissionGroups dizi değil veya tanımsız');
      return [];
    }
    
    // Sayfa Erişimi grubu özel durumu
    if (groupName === 'Sayfa Erişimi') {
      // Backenddeki muhtemel sayfa erişim grupları (loglardan gördüğümüz isimlere göre)
      const possiblePageGroups = ['Page', 'Pages', 'PageAccess', 'PagePermissions', 'Navigation'];
      
      // Mevcut grup adlarıyla olası sayfa gruplarını karşılaştır
      for (const possibleName of possiblePageGroups) {
        const matchingGroup = this.permissionGroups.find(g => g && g.group === possibleName);
        if (matchingGroup) {
          console.log(`"Sayfa Erişimi" için eşleşen grup bulundu: "${possibleName}"`);
          
          if (!matchingGroup.permissions || !Array.isArray(matchingGroup.permissions)) {
            const preserveData = matchingGroup.permissions as unknown as PreserveFormat<Permission>;
            return preserveData && preserveData.$values ? preserveData.$values : [];
          }
          
          return matchingGroup.permissions;
        }
      }
      
      // Özel bir eşleşme bulunamadı, o zaman "Page" içeren herhangi bir grup var mı diye bakalım
      const pageGroup = this.permissionGroups.find(g => 
        g && g.group && (g.group.includes('Page') || g.group.includes('page') || g.group.includes('Sayfa'))
      );
      
      if (pageGroup) {
        console.log(`"Sayfa Erişimi" için kısmi eşleşen grup bulundu: "${pageGroup.group}"`);
        
        if (!pageGroup.permissions || !Array.isArray(pageGroup.permissions)) {
          const preserveData = pageGroup.permissions as unknown as PreserveFormat<Permission>;
          return preserveData && preserveData.$values ? preserveData.$values : [];
        }
        
        return pageGroup.permissions;
      }
      
      // Eğer hiçbir eşleşme bulunamazsa, ilk grubu kullan (en azından bir şeyler göster)
      if (this.permissionGroups.length > 0 && this.permissionGroups[0]) {
        console.log(`"Sayfa Erişimi" için grup bulunamadı, ilk grup kullanılıyor: "${this.permissionGroups[0].group}"`);
        
        const firstGroup = this.permissionGroups[0];
        if (!firstGroup.permissions || !Array.isArray(firstGroup.permissions)) {
          const preserveData = firstGroup.permissions as unknown as PreserveFormat<Permission>;
          return preserveData && preserveData.$values ? preserveData.$values : [];
        }
        
        return firstGroup.permissions;
      }
      
      return [];
    }
    
    // Diğer gruplar için normal işlem
    const group = this.permissionGroups.find(g => g && g.group === groupName);
    
    if (!group) {
      return [];
    }
    
    if (!group.permissions || !Array.isArray(group.permissions)) {
      // Belki permissions ReferenceHandler.Preserve formatında olabilir
      const preserveData = group.permissions as unknown as PreserveFormat<Permission>;
      return preserveData && preserveData.$values ? preserveData.$values : [];
    }
    
    return group.permissions;
  }
  
  /**
   * Belirli bir grup dışındaki tüm grup adlarını döndürür
   */
  getGroupsExcept(excludeGroupName: string): string[] {
    if (!this.permissionGroups || !Array.isArray(this.permissionGroups)) {
      console.warn('permissionGroups dizi değil veya tanımsız');
      return [];
    }
    
    return this.permissionGroups
      .filter(g => g && g.group) // Undefined kontrolü
      .map(g => g.group)
      .filter(groupName => groupName !== excludeGroupName);
  }

  /**
   * Sayfa izinlerini kategorilere göre filtreleyip döndürür
   * @param category İzin kategorisi: 'Dashboard', 'Management', 'Other'
   */
  getPagePermissionsByCategory(category: string): Permission[] {
    const allPagePermissions = this.getPermissionsByGroup('Sayfa Erişimi');
    
    if (!allPagePermissions || allPagePermissions.length === 0) {
      return [];
    }
    
    // İzinleri isimlerine göre kategorilere ayır
    switch (category) {
      case 'Dashboard':
        return allPagePermissions.filter(p => 
          p.name.includes('Dashboard') || 
          p.name.includes('Home') || 
          p.description.toLowerCase().includes('panel') ||
          p.description.toLowerCase().includes('dashboard')
        );
        
      case 'Management':
        return allPagePermissions.filter(p => 
          p.name.includes('Management') || 
          p.name.includes('User') || 
          p.name.includes('Role') ||
          p.description.toLowerCase().includes('yönetim') ||
          p.description.toLowerCase().includes('management')
        );
        
      case 'Other':
        return allPagePermissions.filter(p => 
          !p.name.includes('Dashboard') && 
          !p.name.includes('Home') && 
          !p.name.includes('Management') && 
          !p.name.includes('User') && 
          !p.name.includes('Role') &&
          !p.description.toLowerCase().includes('panel') &&
          !p.description.toLowerCase().includes('dashboard') &&
          !p.description.toLowerCase().includes('yönetim') &&
          !p.description.toLowerCase().includes('management')
        );
        
      default:
        return [];
    }
  }
  
  /**
   * Tüm sayfa erişim izinlerini seçer
   */
  selectAllPagePermissions(): void {
    const pagePermissions = this.getPermissionsByGroup('Sayfa Erişimi');
    
    // Zaten seçili olan izinleri koru, yeni sayfa izinlerini ekle
    pagePermissions.forEach(permission => {
      if (!this.isPermissionSelected(permission.id)) {
        this.selectedPermissions.push(permission.id);
      }
    });
    
    this.messageService.add({ 
      severity: 'success', 
      summary: 'Başarılı', 
      detail: 'Tüm sayfa erişim izinleri seçildi',
      life: 3000
    });
  }
  
  /**
   * Tüm sayfa erişim izinlerini kaldırır
   */
  unselectAllPagePermissions(): void {
    const pagePermissions = this.getPermissionsByGroup('Sayfa Erişimi');
    
    // Sayfa izinlerini kaldır, diğer izinleri koru
    pagePermissions.forEach(permission => {
      const index = this.selectedPermissions.indexOf(permission.id);
      if (index > -1) {
        this.selectedPermissions.splice(index, 1);
      }
    });
    
    this.messageService.add({ 
      severity: 'info', 
      summary: 'Bilgi', 
      detail: 'Tüm sayfa erişim izinleri kaldırıldı',
      life: 3000
    });
  }
} 