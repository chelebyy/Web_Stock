import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Role } from '../../../models/role.model';
import { RoleService } from '../../../services/role.service';
import { PermissionService } from '../../../services/permission.service';
import { Permission, PermissionGroup } from '../../../models/permission.model';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-role-detail',
  templateUrl: './role-detail.component.html',
  styleUrls: ['./role-detail.component.scss']
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
        this.permissionGroups = data.permissionGroups;
        
        // Seçili izinleri ayarla
        this.selectedPermissions = data.rolePermissions.map(p => p.id);
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
} 