import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { CheckboxModule } from 'primeng/checkbox';
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { TabViewModule } from 'primeng/tabview';
import { AuthService } from '../../../core/authentication/auth.service';
import { Router } from '@angular/router';
import { UserService } from '../../user-management/services/user.service';
import { User } from '../../../shared/models/user.model';

// PrimeNG Tag bileşeni için geçerli severity tipleri
type TagSeverity = 'success' | 'info' | 'warn' | 'danger' | 'secondary' | 'contrast' | undefined;

interface DashboardPage {
  id: number;
  name: string;
  description: string;
  route: string;
  isActive: boolean;
  requiredPermission: string;
  createdAt: Date;
  updatedAt: Date;
  users?: User[]; // Sayfaya erişim izni olan kullanıcılar
}

@Component({
  selector: 'app-dashboard-management',
  templateUrl: './dashboard-management.component.html',
  styleUrls: ['./dashboard-management.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    CardModule,
    ButtonModule,
    TableModule,
    ToastModule,
    InputTextModule,
    DropdownModule,
    FormsModule,
    DialogModule,
    CheckboxModule,
    TagModule,
    TooltipModule,
    TabViewModule
  ],
  providers: [MessageService]
})
export class DashboardManagementComponent implements OnInit, AfterViewInit {
  // Dashboard sayfaları listesi
  dashboardPages: DashboardPage[] = [];
  
  // Filtrelenmiş dashboard sayfaları
  filteredDashboardPages: DashboardPage[] = [];
  
  // Arama metni
  searchText: string = '';
  
  // Seçilen dashboard sayfası
  selectedPage: DashboardPage | null = null;
  
  // Dialog görünürlüğü
  displayDialog: boolean = false;
  
  // Yeni/düzenlenen sayfa
  page: DashboardPage = {
    id: 0,
    name: '',
    description: '',
    route: '',
    isActive: true,
    requiredPermission: '',
    createdAt: new Date(),
    updatedAt: new Date(),
    users: []
  };
  
  // Dialog başlığı
  dialogHeader: string = '';
  
  // Yükleniyor durumu
  loading: boolean = true;
  
  // İzin seçenekleri
  permissionOptions: any[] = [];

  // Aktif sekme indeksi
  activeTabIndex: number = 0;

  // Kullanıcı listesi
  users: User[] = [];

  // Filtrelenmiş kullanıcı listesi
  filteredUsers: User[] = [];

  // Kullanıcı arama metni
  userSearchText: string = '';

  // Sayfa kullanıcıları
  pageUsers: User[] = [];

  constructor(
    private messageService: MessageService,
    private authService: AuthService,
    private router: Router,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.loadDashboardPages();
    this.loadPermissionOptions();
    this.loadUsers();
  }

  ngAfterViewInit(): void {
    // Dropdown panel stillerini programatik olarak ayarla
    setTimeout(() => {
      this.updateDropdownPanelStyles();
    }, 100);
  }

  // Dropdown panel stillerini güncelle
  updateDropdownPanelStyles(): void {
    const dropdownPanels = document.querySelectorAll('.dashboard-permission-dropdown-panel');
    dropdownPanels.forEach(panel => {
      if (panel instanceof HTMLElement) {
        panel.style.backgroundColor = '#ffffff';
        panel.style.color = '#333333';
        panel.style.maxHeight = '300px';
        panel.style.overflowY = 'auto';
        panel.style.maxWidth = '450px';
        panel.style.width = 'auto';
        panel.style.zIndex = '10001';
        panel.style.position = 'fixed';
        panel.style.boxShadow = '0 2px 12px rgba(0, 0, 0, 0.1)';
        panel.style.border = '1px solid #ced4da';
        panel.style.borderRadius = '4px';
        
        // Panel içindeki öğelerin stillerini güncelle
        const dropdownItems = panel.querySelectorAll('.p-dropdown-item');
        dropdownItems.forEach(item => {
          if (item instanceof HTMLElement) {
            item.style.backgroundColor = '#ffffff';
            item.style.color = '#333333';
            item.style.padding = '0.75rem 1rem';
            item.style.whiteSpace = 'normal';
            item.style.wordBreak = 'break-word';
          }
        });
      }
    });
  }

  // Dashboard sayfalarını yükle
  loadDashboardPages(): void {
    // Gerçek uygulamada API'den veri çekilecek
    setTimeout(() => {
      this.dashboardPages = [
        {
          id: 1,
          name: 'Admin Dashboard',
          description: 'Yönetici kontrol paneli',
          route: '/dashboard/admin-dashboard',
          isActive: true,
          requiredPermission: 'Pages.AdminDashboard',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 2,
          name: 'Kullanıcı Yönetimi',
          description: 'Kullanıcıları görüntüle, düzenle ve yönet',
          route: '/user-management',
          isActive: true,
          requiredPermission: 'Pages.UserManagement',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 3,
          name: 'Rol Yönetimi',
          description: 'Sistem rollerini ve izinleri yönet',
          route: '/role-management',
          isActive: true,
          requiredPermission: 'Pages.RoleManagement',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 4,
          name: 'Dashboard Yönetimi',
          description: 'Dashboard sayfalarını ve erişim izinlerini yönetin',
          route: '/dashboard-management',
          isActive: true,
          requiredPermission: 'Pages.DashboardManagement',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 5,
          name: 'Sayfa Yönetimi',
          description: 'Sayfalara erişim izinlerini düzenle ve yönet',
          route: '/admin/page-management',
          isActive: true,
          requiredPermission: 'Pages.PageManagement',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 6,
          name: 'Sistem Ayarları',
          description: 'Sistem yapılandırmasını ve parametrelerini yönet',
          route: '/admin/settings',
          isActive: true,
          requiredPermission: 'Pages.Settings',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 7,
          name: 'Raporlar',
          description: 'Sistem raporlarını görüntüle ve yönet',
          route: '/admin/reports',
          isActive: true,
          requiredPermission: 'Pages.Reports',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 8,
          name: 'Sistem Logları',
          description: 'Sistem loglarını görüntüle ve yönet',
          route: '/admin/logs',
          isActive: true,
          requiredPermission: 'Pages.Logs',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 9,
          name: 'Yedekleme',
          description: 'Sistem yedekleme işlemlerini yönet',
          route: '/admin/backup',
          isActive: true,
          requiredPermission: 'Pages.Backup',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 10,
          name: 'Yardım',
          description: 'Sistem yardım dokümanlarını görüntüle',
          route: '/admin/help',
          isActive: true,
          requiredPermission: 'Pages.Help',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 11,
          name: 'Aktivite Logları',
          description: 'Kullanıcı aktivite loglarını görüntüle',
          route: '/admin/activity-logs',
          isActive: true,
          requiredPermission: 'Pages.ActivityLogs',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        },
        {
          id: 12,
          name: 'Bildirimler',
          description: 'Sistem bildirimlerini görüntüle ve yönet',
          route: '/admin/notifications',
          isActive: true,
          requiredPermission: 'Pages.Notifications',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01'),
          users: []
        }
      ];
      
      this.filteredDashboardPages = [...this.dashboardPages];
      this.loading = false;
    }, 1000);
  }

  // İzin seçeneklerini yükle
  loadPermissionOptions(): void {
    this.permissionOptions = [
      { label: 'Pages.AdminDashboard', value: 'Pages.AdminDashboard' },
      { label: 'Pages.UserManagement', value: 'Pages.UserManagement' },
      { label: 'Pages.RoleManagement', value: 'Pages.RoleManagement' },
      { label: 'Pages.DashboardManagement', value: 'Pages.DashboardManagement' },
      { label: 'Pages.PageManagement', value: 'Pages.PageManagement' },
      { label: 'Pages.Settings', value: 'Pages.Settings' },
      { label: 'Pages.Reports', value: 'Pages.Reports' },
      { label: 'Pages.Logs', value: 'Pages.Logs' },
      { label: 'Pages.Backup', value: 'Pages.Backup' },
      { label: 'Pages.Help', value: 'Pages.Help' },
      { label: 'Pages.ActivityLogs', value: 'Pages.ActivityLogs' },
      { label: 'Pages.Notifications', value: 'Pages.Notifications' }
    ];
  }

  // Kullanıcıları yükle
  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.filteredUsers = [...this.users];
      },
      error: (error) => {
        console.error('Kullanıcıları yüklerken hata oluştu:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcılar yüklenirken bir hata oluştu.'
        });
        
        // Hata durumunda örnek kullanıcılar
        this.users = [
          {
            id: 1,
            username: 'Admin Kullanıcı',
            sicil: 'A001',
            isAdmin: true,
            createdAt: new Date('2025-01-01')
          },
          {
            id: 2,
            username: 'Test Kullanıcı',
            sicil: 'T001',
            isAdmin: false,
            createdAt: new Date('2025-01-01')
          },
          {
            id: 3,
            username: 'Genel Kullanıcı',
            sicil: 'G001',
            isAdmin: false,
            createdAt: new Date('2025-01-01')
          }
        ] as User[];
        
        this.filteredUsers = [...this.users];
      }
    });
  }

  // Kullanıcı arama
  onUserSearch(event: any): void {
    const searchText = this.userSearchText.toLowerCase();
    
    if (!searchText) {
      this.filteredUsers = [...this.users];
      return;
    }
    
    this.filteredUsers = this.users.filter(user => 
      user.username.toLowerCase().includes(searchText) ||
      user.sicil.toLowerCase().includes(searchText)
    );
  }

  // Sayfaya kullanıcı ekle
  addUserToPage(user: User): void {
    // Kullanıcı zaten ekli mi kontrol et
    const userExists = this.pageUsers.some(u => u.id === user.id);
    
    if (userExists) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Uyarı',
        detail: `${user.username} kullanıcısı zaten bu sayfaya eklenmiş.`
      });
      return;
    }
    
    // Kullanıcıyı sayfaya ekle
    this.pageUsers.push(user);
    
    // Sayfanın users dizisini güncelle
    this.page.users = [...this.pageUsers];
    
    this.messageService.add({
      severity: 'success',
      summary: 'Başarılı',
      detail: `${user.username} kullanıcısı sayfaya eklendi.`
    });
  }

  // Sayfadan kullanıcı kaldır
  removeUserFromPage(user: User): void {
    // Kullanıcıyı sayfadan kaldır
    this.pageUsers = this.pageUsers.filter(u => u.id !== user.id);
    
    // Sayfanın users dizisini güncelle
    this.page.users = [...this.pageUsers];
    
    this.messageService.add({
      severity: 'success',
      summary: 'Başarılı',
      detail: `${user.username} kullanıcısı sayfadan kaldırıldı.`
    });
  }

  // Arama
  onSearch(event: any): void {
    const searchText = this.searchText.toLowerCase();
    
    if (!searchText) {
      this.filteredDashboardPages = [...this.dashboardPages];
      return;
    }
    
    this.filteredDashboardPages = this.dashboardPages.filter(page => 
      page.name.toLowerCase().includes(searchText) ||
      page.description.toLowerCase().includes(searchText) ||
      page.route.toLowerCase().includes(searchText) ||
      page.requiredPermission.toLowerCase().includes(searchText)
    );
  }

  // Aramayı temizle
  clearSearch(): void {
    this.searchText = '';
    this.filteredDashboardPages = [...this.dashboardPages];
  }

  // Durum etiketi
  getStatusLabel(isActive: boolean): string {
    return isActive ? 'Aktif' : 'Pasif';
  }

  // Durum severity
  getStatusSeverity(isActive: boolean): TagSeverity {
    return isActive ? 'success' : 'danger';
  }

  // Sayfa düzenle
  editPage(page: DashboardPage): void {
    this.page = { ...page };
    this.dialogHeader = 'Sayfa Düzenle';
    this.displayDialog = true;
    
    // Sayfa kullanıcılarını yükle
    this.pageUsers = page.users || [];
    
    // Aktif sekmeyi sıfırla
    this.activeTabIndex = 0;
  }

  // Dialog'u gizle
  hideDialog(): void {
    this.displayDialog = false;
  }

  // Sayfa kaydet
  savePage(): void {
    if (!this.page.name || !this.page.route || !this.page.requiredPermission) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Lütfen zorunlu alanları doldurun.'
      });
      return;
    }
    
    if (this.page.id === 0) {
      // Yeni sayfa ekleme
      this.page.id = this.generateId();
      this.page.createdAt = new Date();
      this.page.updatedAt = new Date();
      this.dashboardPages.push(this.page);
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: 'Sayfa başarıyla eklendi.'
      });
    } else {
      // Mevcut sayfayı güncelleme
      const index = this.dashboardPages.findIndex(p => p.id === this.page.id);
      if (index !== -1) {
        this.page.updatedAt = new Date();
        this.dashboardPages[index] = this.page;
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Sayfa başarıyla güncellendi.'
        });
      }
    }
    
    this.filteredDashboardPages = [...this.dashboardPages];
    this.displayDialog = false;
    this.page = {
      id: 0,
      name: '',
      description: '',
      route: '',
      isActive: true,
      requiredPermission: '',
      createdAt: new Date(),
      updatedAt: new Date(),
      users: []
    };
  }

  // ID oluştur
  generateId(): number {
    return this.dashboardPages.length > 0 
      ? Math.max(...this.dashboardPages.map(page => page.id)) + 1 
      : 1;
  }

  // Sayfaya yönlendir
  navigateTo(route: string): void {
    this.router.navigate([route]);
  }
} 