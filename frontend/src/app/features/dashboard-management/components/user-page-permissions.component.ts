import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { InputTextModule } from 'primeng/inputtext';
import { TooltipModule } from 'primeng/tooltip';
import { RippleModule } from 'primeng/ripple';
import { MessageService } from 'primeng/api';
import { DashboardPermissionService, DashboardUser, PagePermission } from '../services/dashboard-permission.service';
import { finalize, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-user-page-permissions',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    ToastModule,
    InputTextModule,
    TooltipModule,
    RippleModule
  ],
  providers: [MessageService],
  templateUrl: './user-page-permissions.component.html',
  styleUrls: ['./user-page-permissions.component.scss']
})
export class UserPagePermissionsComponent implements OnInit {
  pageId = 0;
  pageName = '';
  loading = true;
  processing = false;
  
  // Aktif sayfa (users veya permissions)
  activePage = 'users';
  
  // Kullanıcılar sayfası için
  users: DashboardUser[] = [];
  filteredUsers: DashboardUser[] = [];
  usersSearchText = '';
  currentUserPage = 1;
  totalUserPages = 1;
  usersPerPage = 12;
  
  // İzinler sayfası için
  pagePermissions: PagePermission[] = [];
  filteredPagePermissions: PagePermission[] = [];
  permissionsSearchText = '';
  currentPermissionPage = 1;
  totalPermissionPages = 1;
  permissionsPerPage = 12;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private permissionService: DashboardPermissionService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.pageId = +params['pageId'];
      this.pageName = params['pageName'] || 'Bilinmeyen Sayfa';
      
      if (isNaN(this.pageId) || this.pageId <= 0) {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Geçersiz sayfa ID\'si.'
        });
        this.navigateBack();
        return;
      }
      
      this.loadData();
    });
  }

  loadData(): void {
    this.loading = true;
    
    // Kullanıcıları yükle
    this.permissionService.getUsers(this.pageId)
      .pipe(
        catchError(error => {
          console.error('Kullanıcı verileri yüklenirken hata:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcı verileri yüklenirken bir hata oluştu.'
          });
          return of([]);
        }),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe(users => {
        console.log('Yüklenen kullanıcılar:', users);
        this.users = users;
        this.filteredUsers = [...users];
        this.calculateUserPages();
      });

    // İzinleri yükle
    this.permissionService.getPagePermissions(this.pageId)
      .pipe(
        catchError(error => {
          console.error('Sayfa izinleri yüklenirken hata:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Sayfa izinleri yüklenirken bir hata oluştu.'
          });
          return of([]);
        })
      )
      .subscribe(permissions => {
        console.log('Yüklenen izinler:', permissions);
        this.pagePermissions = permissions;
        this.filteredPagePermissions = [...permissions];
        this.calculatePermissionPages();
      });
  }

  // Sayfa geçişi
  switchPage(page: string): void {
    if (this.activePage !== page) {
      this.activePage = page;
      
      // Sayfa değiştiğinde sayfalama bilgilerini sıfırla
      if (page === 'users') {
        this.currentUserPage = 1;
        this.calculateUserPages();
      } else {
        this.currentPermissionPage = 1;
        this.calculatePermissionPages();
      }
    }
  }

  // Kullanıcı sayfalaması
  calculateUserPages(): void {
    this.totalUserPages = Math.ceil(this.filteredUsers.length / this.usersPerPage);
    if (this.totalUserPages === 0) this.totalUserPages = 1;
  }

  goToUserPage(page: number): void {
    if (page >= 1 && page <= this.totalUserPages) {
      this.currentUserPage = page;
    }
  }

  // İzin sayfalaması
  calculatePermissionPages(): void {
    this.totalPermissionPages = Math.ceil(this.filteredPagePermissions.length / this.permissionsPerPage);
    if (this.totalPermissionPages === 0) this.totalPermissionPages = 1;
  }

  goToPermissionPage(page: number): void {
    if (page >= 1 && page <= this.totalPermissionPages) {
      this.currentPermissionPage = page;
    }
  }

  // Kullanıcı arama
  searchUsers(event: any): void {
    const searchText = this.usersSearchText.toLowerCase();
    this.filteredUsers = this.users.filter(user =>
      (user.username && user.username.toLowerCase().includes(searchText)) ||
      user.fullName.toLowerCase().includes(searchText) ||
      user.sicil.toLowerCase().includes(searchText)
    );
    
    // Arama sonuçlarına göre sayfalama bilgilerini güncelle
    this.currentUserPage = 1;
    this.calculateUserPages();
  }

  // İzin arama
  searchPermissions(event: any): void {
    const searchText = this.permissionsSearchText.toLowerCase();
    this.filteredPagePermissions = this.pagePermissions.filter(permission =>
      (permission.username && permission.username.toLowerCase().includes(searchText)) ||
      permission.fullName.toLowerCase().includes(searchText) ||
      permission.sicil.toLowerCase().includes(searchText)
    );
    
    // Arama sonuçlarına göre sayfalama bilgilerini güncelle
    this.currentPermissionPage = 1;
    this.calculatePermissionPages();
  }

  clearUsersSearch(): void {
    this.usersSearchText = '';
    this.filteredUsers = [...this.users];
    this.currentUserPage = 1;
    this.calculateUserPages();
  }

  clearPermissionsSearch(): void {
    this.permissionsSearchText = '';
    this.filteredPagePermissions = [...this.pagePermissions];
    this.currentPermissionPage = 1;
    this.calculatePermissionPages();
  }

  grantPermission(user: DashboardUser): void {
    if (!user.id || user.hasPagePermission || !user.isActive) return;

    this.processing = true;
    this.permissionService.grantPermission(this.pageId, user.id)
      .pipe(
        catchError(error => {
          console.error('İzin verilirken hata:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'İzin verilirken bir hata oluştu.'
          });
          return of(null);
        }),
        finalize(() => {
          this.processing = false;
        })
      )
      .subscribe(result => {
        if (result) {
          // Kullanıcının izin durumunu güncelle
          user.hasPagePermission = true;
          
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: `${user.fullName} kullanıcısına sayfa izni verildi.`
          });
          
          // İzinler listesini güncelle
          const newPermission: PagePermission = {
            id: 0, // Backend tarafından atanacak
            userId: user.id,
            pageId: this.pageId,
            username: user.username || '',
            fullName: user.fullName,
            sicil: user.sicil,
            grantedAt: new Date()
          };
          
          this.pagePermissions = [...this.pagePermissions, newPermission];
          this.filteredPagePermissions = [...this.pagePermissions];
          this.calculatePermissionPages();
        }
      });
  }

  revokePermission(permission: PagePermission): void {
    this.processing = true;
    this.permissionService.revokePermission(this.pageId, permission.userId)
      .pipe(
        catchError(error => {
          console.error('İzin kaldırılırken hata:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'İzin kaldırılırken bir hata oluştu.'
          });
          return of(null);
        }),
        finalize(() => {
          this.processing = false;
        })
      )
      .subscribe(result => {
        if (result) {
          // Kullanıcının izin durumunu güncelle
          const user = this.users.find(u => u.id === permission.userId);
          if (user) {
            user.hasPagePermission = false;
          }
          
          // İzinler listesinden kaldır
          this.pagePermissions = this.pagePermissions.filter(p => p.userId !== permission.userId);
          this.filteredPagePermissions = this.filteredPagePermissions.filter(p => p.userId !== permission.userId);
          
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: `${permission.fullName} kullanıcısının sayfa izni kaldırıldı.`
          });
          
          // Sayfalama bilgilerini güncelle
          this.calculatePermissionPages();
          
          // Eğer mevcut sayfa boşsa, bir önceki sayfaya git
          if (this.currentPermissionPage > this.totalPermissionPages) {
            this.currentPermissionPage = this.totalPermissionPages;
          }
        }
      });
  }

  navigateBack(): void {
    this.router.navigate(['/dashboard-management']);
  }
} 