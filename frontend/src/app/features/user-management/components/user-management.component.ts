import { Component, OnInit, OnDestroy, ChangeDetectionStrategy, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { RippleModule } from 'primeng/ripple';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';

import { User } from '../../../shared/models/user.model';
import { Role } from '../../../shared/models/role.model';
import { UserService } from '../../../services/user.service';
import { RoleService } from '../../../services/role.service';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    ToolbarModule,
    InputTextModule,
    DialogModule,
    DropdownModule,
    ToastModule,
    ConfirmDialogModule,
    RippleModule,
    CardModule,
    TagModule
  ],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MessageService, ConfirmationService]
})
export class UserManagementComponent implements OnInit, OnDestroy {
  private readonly userService = inject(UserService);
  private readonly roleService = inject(RoleService);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);
  private readonly location = inject(Location);
  private readonly cdr = inject(ChangeDetectorRef);

  // Data properties
  users: User[] = [];
  roles: Role[] = [];
  selectedUsers: User[] = [];
  selectedUser: User | null = null;
  user: User = this.initUser();

  // Dialog properties
  userDialog = false;
  submitted = false;

  // Statistics
  totalUsers = 0;
  activeUsers = 0;
  inactiveUsers = 0;
  totalRoles = 0;
  onlineUsers = 1; // Sabit değer

  ngOnInit(): void {
    this.loadUsers();
    this.loadRoles();
  }
  
  ngOnDestroy(): void {
    // Cleanup if needed
  }

  initUser(): User {
    return {
      id: 0,
      sicil: '',
      fullName: '',
      adi: '',
      soyadi: '',
      isActive: true,
      isAdmin: false,
      createdAt: new Date(),
      roleId: 0,
      role: { id: 0, name: '', description: '' },
      password: '',
      username: '',
      email: ''
    };
  }

  exportUsersToCsv(): void {
    // Implementation of exportUsersToCsv method
  }

  private downloadCSV(csvData: string): void {
    // Implementation of downloadCSV method
  }

  loadUsers(): void {
    // Geçici olarak örnek veri kullanıyoruz çünkü backend henüz hazır değil
    this.users = [
      {
        id: 1,
        sicil: '001',
        adi: 'Ahmet',
        soyadi: 'Yılmaz',
        fullName: 'Ahmet Yılmaz',
        username: 'ahmet.yilmaz',
        email: 'ahmet@company.com',
        isActive: true,
        isAdmin: true,
        createdAt: new Date('2024-01-15'),
        roleId: 1,
        role: { id: 1, name: 'Admin', description: 'Sistem Yöneticisi' },
        password: ''
      },
      {
        id: 2,
        sicil: '002',
        adi: 'Fatma',
        soyadi: 'Kaya',
        fullName: 'Fatma Kaya',
        username: 'fatma.kaya',
        email: 'fatma@company.com',
        isActive: true,
        isAdmin: false,
        createdAt: new Date('2024-02-10'),
        roleId: 2,
        role: { id: 2, name: 'User', description: 'Standart Kullanıcı' },
        password: ''
      },
      {
        id: 3,
        sicil: '003',
        adi: 'Mehmet',
        soyadi: 'Demir',
        fullName: 'Mehmet Demir',
        username: 'mehmet.demir',
        email: 'mehmet@company.com',
        isActive: false,
        isAdmin: false,
        createdAt: new Date('2024-01-20'),
        roleId: 2,
        role: { id: 2, name: 'User', description: 'Standart Kullanıcı' },
        password: ''
      }
    ];
    
    this.updateStatistics();
    this.cdr.detectChanges();
    
    // Gerçek API çağrısı için:
    // this.userService.getUsers().subscribe({
    //   next: (users) => {
    //     this.users = Array.isArray(users) ? users : [];
    //     this.updateStatistics();
    //     this.cdr.detectChanges();
    //   },
    //   error: (error) => {
    //     this.users = []; // Hata durumunda boş array
    //     this.messageService.add({
    //       severity: 'error',
    //       summary: 'Hata',
    //       detail: 'Kullanıcılar yüklenirken hata oluştu'
    //     });
    //   }
    // });
  }

  loadRoles(): void {
    // Geçici olarak örnek rol verileri
    this.roles = [
      { id: 1, name: 'Admin', description: 'Sistem Yöneticisi' },
      { id: 2, name: 'User', description: 'Standart Kullanıcı' },
      { id: 3, name: 'Moderator', description: 'Moderatör' }
    ];
    this.updateStatistics();
    this.cdr.detectChanges();
    
    // Gerçek API çağrısı için:
    // this.roleService.getAllRoles().subscribe({
    //   next: (roles) => {
    //     this.roles = Array.isArray(roles) ? roles : [];
    //     this.updateStatistics();
    //     this.cdr.detectChanges();
    //   },
    //   error: (error) => {
    //     this.roles = []; // Hata durumunda boş array
    //     this.messageService.add({
    //       severity: 'error',
    //       summary: 'Hata',
    //       detail: 'Roller yüklenirken hata oluştu'
    //     });
    //   }
    // });
  }

  updateStatistics(): void {
    if (Array.isArray(this.users)) {
      this.totalUsers = this.users.length;
      this.activeUsers = this.users.filter(u => u.isActive).length;
      this.inactiveUsers = this.totalUsers - this.activeUsers;
    } else {
      this.totalUsers = 0;
      this.activeUsers = 0;
      this.inactiveUsers = 0;
    }
    
    if (Array.isArray(this.roles)) {
      this.totalRoles = this.roles.length;
    } else {
      this.totalRoles = 0;
    }
  }

  openNew(): void {
    this.user = this.initUser();
    this.submitted = false;
    this.userDialog = true;
  }

  deleteSelectedUsers(): void {
    this.confirmationService.confirm({
      message: 'Seçili kullanıcıları silmek istediğinizden emin misiniz?',
      header: 'Onayla',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        const deletePromises = this.selectedUsers
          .filter(user => user.id)
          .map(user => this.userService.deleteUser(user.id!));

        Promise.all(deletePromises).then(() => {
          this.loadUsers();
          this.selectedUsers = [];
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Seçili kullanıcılar silindi'
          });
        }).catch(() => {
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcılar silinirken hata oluştu'
          });
        });
      }
    });
  }

  editUser(user: User): void {
    this.user = { ...user };
    this.userDialog = true;
  }

  managePermissions(user: User): void {
    // Navigate to user permissions component
    this.messageService.add({ severity: 'info', summary: 'Bilgi', detail: `${user.fullName} için yetki yönetimi açılıyor...` });
  }

  getSeverity(isActive: boolean): 'success' | 'secondary' | 'info' | 'warn' | 'danger' | 'contrast' | undefined {
    return isActive ? 'success' : 'danger';
  }

  hideDialog(): void {
    this.userDialog = false;
    this.submitted = false;
  }

  saveUser(): void {
    this.submitted = true;

    if (this.isFormValid()) {
      if (this.user.id && this.user.id > 0) {
        // Güncelleme
        this.userService.updateUser(this.user.id, this.user).subscribe({
          next: () => {
            this.loadUsers();
            this.hideDialog();
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Kullanıcı güncellendi'
            });
          },
          error: (error) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Kullanıcı güncellenirken hata oluştu'
            });
          }
        });
      } else {
        // Yeni ekleme
        this.userService.createUser(this.user).subscribe({
          next: () => {
            this.loadUsers();
            this.hideDialog();
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Kullanıcı eklendi'
            });
          },
          error: (error) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Kullanıcı eklenirken hata oluştu'
            });
          }
        });
      }
    }
  }

  isFormValid(): boolean {
    return !!(this.user.sicil && this.user.adi && this.user.soyadi && this.user.roleId);
  }

  deleteUser(user: User): void {
    this.confirmationService.confirm({
      message: `${user.fullName || user.username} kullanıcısını silmek istediğinizden emin misiniz?`,
      header: 'Onayla',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        if (user.id) {
          this.userService.deleteUser(user.id).subscribe({
            next: () => {
              this.loadUsers();
              this.messageService.add({
                severity: 'success',
                summary: 'Başarılı',
                detail: 'Kullanıcı silindi'
              });
            },
            error: (error) => {
              this.messageService.add({
                severity: 'error',
                summary: 'Hata',
                detail: 'Kullanıcı silinirken hata oluştu'
              });
            }
          });
        }
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
