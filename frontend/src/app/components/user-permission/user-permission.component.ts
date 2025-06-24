import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { UserService } from '../../services/user.service';
import { PermissionService } from '../../services/permission.service';
import { UserPermissionService } from '../../services/user-permission.service';
import { User } from '../../shared/models/user.model';
import { Permission, PermissionGroup } from '../../shared/models/permission.model';
import { UserPermission } from '../../shared/models/user-permission.model';

// PrimeNG modülleri
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { PickListModule } from 'primeng/picklist';
import { AccordionModule } from 'primeng/accordion';
import { DividerModule } from 'primeng/divider';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-user-permission',
  templateUrl: './user-permission.component.html',
  styleUrls: ['./user-permission.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    PickListModule,
    AccordionModule,
    DividerModule,
    ProgressSpinnerModule,
    ToastModule
  ],
  providers: [MessageService]
})
export class UserPermissionComponent implements OnInit {
  userId = 0;
  user: User | null = null;
  
  // Tüm izinler (gruplara ayrılmış)
  allPermissionsGroups: PermissionGroup[] = [];
  // Kullanıcının mevcut izinleri (rol + özel)
  userPermissions: Permission[] = [];
  // Kullanıcının özel izinleri
  userCustomPermissions: UserPermission[] = [];
  
  // Yeni atamalar için seçilen izinler
  selectedPermissions: Permission[] = [];
  // Kaldırılacak özel izinler
  selectedCustomPermissions: UserPermission[] = [];
  
  loading = false;
  saveLoading = false;
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private permissionService: PermissionService,
    private userPermissionService: UserPermissionService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.userId = +params['id'];
        this.loadData();
      }
    });
  }

  loadData(): void {
    this.loading = true;
    
    // Kullanıcı bilgilerini yükle
    this.userService.getUserById(this.userId).subscribe({
      next: (user) => {
        this.user = user;
      },
      error: (error) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı bilgileri yüklenemedi.' });
        console.error('Kullanıcı bilgileri yüklenirken hata:', error);
      }
    });
    
    // Tüm izinleri yükle
    this.permissionService.getPermissionsByGroups().subscribe({
      next: (groups) => {
        this.allPermissionsGroups = groups;
      },
      error: (error) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'İzinler yüklenemedi.' });
        console.error('İzinler yüklenirken hata:', error);
      }
    });
    
    // Kullanıcının mevcut izinlerini yükle
    this.userPermissionService.getUserPermissions(this.userId).subscribe({
      next: (permissions) => {
        this.userPermissions = permissions;
      },
      error: (error) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı izinleri yüklenemedi.' });
        console.error('Kullanıcı izinleri yüklenirken hata:', error);
      }
    });
    
    // Kullanıcının özel izinlerini yükle
    this.userPermissionService.getUserCustomPermissions(this.userId).subscribe({
      next: (customPermissions) => {
        this.userCustomPermissions = customPermissions;
        this.loading = false;
      },
      error: (error) => {
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı özel izinleri yüklenemedi.' });
        console.error('Kullanıcı özel izinleri yüklenirken hata:', error);
        this.loading = false;
      }
    });
  }

  // İzin kullanıcıda var mı kontrol eder
  hasPermission(permissionId: number): boolean {
    return this.userPermissions.some(p => p.id === permissionId);
  }
  
  // İzin özel olarak atanmış mı kontrol eder
  isCustomPermission(permissionId: number): boolean {
    return this.userCustomPermissions.some(p => p.permissionId === permissionId);
  }
  
  // Seçilen izinleri kullanıcıya atar
  assignSelectedPermissions(): void {
    if (this.selectedPermissions.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Uyarı', detail: 'Lütfen izin seçiniz.' });
      return;
    }
    
    this.saveLoading = true;
    const totalPermissions = this.selectedPermissions.length;
    let completedCount = 0;
    
    // Her izni ayrı ayrı ata
    this.selectedPermissions.forEach(permission => {
      this.userPermissionService.assignPermissionToUser(this.userId, permission.id, true).subscribe({
        next: () => {
          completedCount++;
          if (completedCount === totalPermissions) {
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'İzinler kullanıcıya atandı.' });
            this.selectedPermissions = [];
            this.loadData();
            this.saveLoading = false;
          }
        },
        error: (error) => {
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: `İzin atanırken hata: ${permission.name}` });
          console.error('İzin atanırken hata:', error);
          completedCount++;
          if (completedCount === totalPermissions) {
            this.saveLoading = false;
          }
        }
      });
    });
  }
  
  // Seçilen özel izinleri kullanıcıdan kaldırır
  removeSelectedCustomPermissions(): void {
    if (this.selectedCustomPermissions.length === 0) {
      this.messageService.add({ severity: 'warn', summary: 'Uyarı', detail: 'Lütfen kaldırılacak özel izin seçiniz.' });
      return;
    }
    
    this.saveLoading = true;
    const totalPermissions = this.selectedCustomPermissions.length;
    let completedCount = 0;
    
    // Her izni ayrı ayrı kaldır
    this.selectedCustomPermissions.forEach(customPermission => {
      this.userPermissionService.removeUserPermission(this.userId, customPermission.permissionId).subscribe({
        next: () => {
          completedCount++;
          if (completedCount === totalPermissions) {
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Seçilen özel izinler kaldırıldı.' });
            this.selectedCustomPermissions = [];
            this.loadData();
            this.saveLoading = false;
          }
        },
        error: (error) => {
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: `İzin kaldırılırken hata: ${customPermission.permissionName}` });
          console.error('İzin kaldırılırken hata:', error);
          completedCount++;
          if (completedCount === totalPermissions) {
            this.saveLoading = false;
          }
        }
      });
    });
  }
  
  // Kullanıcının tüm özel izinlerini kaldırır ve rol izinlerine sıfırlar
  resetToRolePermissions(): void {
    if (confirm('Kullanıcının tüm özel izinleri kaldırılacak ve sadece rol izinleri kalacaktır. Emin misiniz?')) {
      this.saveLoading = true;
      
      this.userPermissionService.resetUserToRolePermissions(this.userId).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı izinleri rol izinlerine sıfırlandı.' });
          this.loadData();
          this.saveLoading = false;
        },
        error: (error) => {
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı izinleri sıfırlanırken bir hata oluştu.' });
          console.error('Kullanıcı izinleri sıfırlanırken hata:', error);
          this.saveLoading = false;
        }
      });
    }
  }
  
  // Kullanıcı yönetimi sayfasına geri döner
  goBack(): void {
    this.router.navigate(['/user-management']);
  }
  
  // Atanabilir izinleri filtreler
  getFilteredPermissions(): Permission[] {
    if (!this.allPermissionsGroups || this.allPermissionsGroups.length === 0) {
      return [];
    }
    
    // Önce tüm izinleri düz bir diziye dönüştür
    const allPermissions: Permission[] = [];
    this.allPermissionsGroups.forEach(group => {
      if (group.permissions && Array.isArray(group.permissions)) {
        group.permissions.forEach(permission => {
          allPermissions.push(permission);
        });
      }
    });
    
    // Kullanıcının henüz sahip olmadığı izinleri filtrele
    return allPermissions.filter(p => !this.hasPermission(p.id));
  }
} 