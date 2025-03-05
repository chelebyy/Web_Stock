import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Role } from '../../../models/role.model';
import { RoleService } from '../../../services/role.service';
import { PermissionService } from '../../../services/permission.service';
import { Permission, PermissionGroup } from '../../../models/permission.model';
import { forkJoin } from 'rxjs';
import { AccordionModule } from 'primeng/accordion';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
    ProgressSpinnerModule
  ]
})
export class RoleDetailComponent implements OnInit {
  roleId: number = 0;
  role: Role | null = null;
  loading: boolean = true;
  permissionGroups: PermissionGroup[] = [];
  selectedPermissions: number[] = [];
  savingPermissions: boolean = false;

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
      this.loadRoleData();
    });
  }

  loadRoleData(): void {
    this.loading = true;
    
    // Rol ve izin gruplarını paralel olarak yükle
    forkJoin({
      role: this.roleService.getRoleById(this.roleId),
      permissionGroups: this.permissionService.getPermissionsByGroups(),
      rolePermissions: this.permissionService.getPermissionsByRoleId(this.roleId)
    }).subscribe({
      next: (data) => {
        this.role = data.role;
        
        // PermissionGroups formatını kontrol et
        if (data.permissionGroups && Array.isArray(data.permissionGroups)) {
          this.permissionGroups = data.permissionGroups;
        } else if (data.permissionGroups && (data.permissionGroups as PreserveFormat<PermissionGroup>).$values) {
          // ReferenceHandler.Preserve formatını kontrol et
          const preserveData = data.permissionGroups as PreserveFormat<PermissionGroup>;
          this.permissionGroups = preserveData.$values || [];
        } else {
          console.error('permissionGroups uygun formatta değil:', data.permissionGroups);
          this.permissionGroups = [];
        }
        
        // Seçili izinleri ayarla
        if (data.rolePermissions && Array.isArray(data.rolePermissions)) {
          this.selectedPermissions = data.rolePermissions.map(p => p.id);
        } else if (data.rolePermissions && (data.rolePermissions as PreserveFormat<Permission>).$values) {
          // ReferenceHandler.Preserve formatını kontrol et
          const preserveData = data.rolePermissions as PreserveFormat<Permission>;
          this.selectedPermissions = preserveData.$values?.map(p => p.id) || [];
        } else {
          console.error('rolePermissions uygun formatta değil:', data.rolePermissions);
          this.selectedPermissions = [];
        }
        
        this.loading = false;
      },
      error: (error) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Rol bilgileri yüklenirken bir hata oluştu.' });
        this.loading = false;
        console.error('Rol yükleme hatası:', error);
      }
    });
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
} 