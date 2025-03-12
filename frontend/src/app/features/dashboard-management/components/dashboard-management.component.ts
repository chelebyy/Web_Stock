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
import { AuthService } from '../../../core/authentication/auth.service';
import { Router } from '@angular/router';
import { UserService } from '../../user-management/services/user.service';
import { MultiSelectModule } from 'primeng/multiselect';
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
  allowedUsers?: User[]; // İzin verilen kullanıcılar
  allowedUserIds?: number[]; // İzin verilen kullanıcı ID'leri
  createdAt: Date;
  updatedAt: Date;
}

interface UserSelectItem {
  label: string;
  value: number;
  sicil: string;
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
    MultiSelectModule
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
    allowedUserIds: [],
    createdAt: new Date(),
    updatedAt: new Date()
  };
  
  // Dialog başlığı
  dialogHeader: string = '';
  
  // Yükleniyor durumu
  loading: boolean = true;
  
  // İzin seçenekleri
  permissionOptions: any[] = [];
  
  // Kullanıcı seçenekleri
  userOptions: UserSelectItem[] = [];
  
  // Tüm kullanıcılar
  users: User[] = [];

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
      this.updateMultiSelectPanelStyles();
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

  // Kullanıcıları yükle
  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        // Kullanıcı seçeneklerini oluştur
        this.userOptions = users.map(user => ({
          label: `${user.firstName || ''} ${user.lastName || ''} (${user.sicil})`,
          value: user.id || 0,
          sicil: user.sicil
        }));
        console.log('Kullanıcılar yüklendi:', this.userOptions);
      },
      error: (error) => {
        console.error('Kullanıcıları yüklerken hata oluştu:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcılar yüklenirken bir hata oluştu.'
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
          allowedUserIds: [],
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 2,
          name: 'Kullanıcı Yönetimi',
          description: 'Kullanıcıları görüntüle, düzenle ve yönet',
          route: '/user-management',
          isActive: true,
          requiredPermission: 'Pages.UserManagement',
          allowedUserIds: [],
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 3,
          name: 'Rol Yönetimi',
          description: 'Sistem rollerini ve izinleri yönet',
          route: '/role-management',
          isActive: true,
          requiredPermission: 'Pages.RoleManagement',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 4,
          name: 'Dashboard Yönetimi',
          description: 'Dashboard sayfalarını ve erişim izinlerini yönetin',
          route: '/dashboard-management',
          isActive: true,
          requiredPermission: 'Pages.DashboardManagement',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 5,
          name: 'Sayfa Yönetimi',
          description: 'Sayfalara erişim izinlerini düzenle ve yönet',
          route: '/admin/page-management',
          isActive: true,
          requiredPermission: 'Pages.PageManagement',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 6,
          name: 'Sistem Ayarları',
          description: 'Sistem yapılandırmasını ve parametrelerini yönet',
          route: '/admin/settings',
          isActive: true,
          requiredPermission: 'Pages.Settings',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 7,
          name: 'Raporlar',
          description: 'Sistem raporlarını görüntüle ve yönet',
          route: '/admin/reports',
          isActive: true,
          requiredPermission: 'Pages.Reports',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 8,
          name: 'Sistem Logları',
          description: 'Sistem loglarını görüntüle ve yönet',
          route: '/admin/logs',
          isActive: true,
          requiredPermission: 'Pages.Logs',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 9,
          name: 'Yedekleme',
          description: 'Sistem yedekleme işlemlerini yönet',
          route: '/admin/backup',
          isActive: true,
          requiredPermission: 'Pages.Backup',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 10,
          name: 'Yardım',
          description: 'Sistem yardım dokümanlarını görüntüle',
          route: '/admin/help',
          isActive: true,
          requiredPermission: 'Pages.Help',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 11,
          name: 'Aktivite Logları',
          description: 'Kullanıcı aktivite loglarını görüntüle',
          route: '/admin/activity-logs',
          isActive: true,
          requiredPermission: 'Pages.ActivityLogs',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        }
      ];
      this.filteredDashboardPages = [...this.dashboardPages];
      this.loading = false;
    }, 200);
  }

  // İzin seçeneklerini yükle
  loadPermissionOptions(): void {
    // Gerçek uygulamada API'den veri çekilecek
    // Şimdilik örnek veriler kullanıyoruz
    this.permissionOptions = [
      { label: 'Admin Dashboard', value: 'Pages.AdminDashboard' },
      { label: 'Kullanıcı Yönetimi', value: 'Pages.UserManagement' },
      { label: 'Rol Yönetimi', value: 'Pages.RoleManagement' },
      { label: 'Dashboard Yönetimi', value: 'Pages.DashboardManagement' },
      { label: 'Sayfa Yönetimi', value: 'Pages.PageManagement' },
      { label: 'Sistem Ayarları', value: 'Pages.Settings' },
      { label: 'Raporlar', value: 'Pages.Reports' },
      { label: 'Sistem Logları', value: 'Pages.Logs' },
      { label: 'Yedekleme', value: 'Pages.Backup' },
      { label: 'Yardım', value: 'Pages.Help' },
      { label: 'Aktivite Logları', value: 'Pages.ActivityLogs' }
    ];
  }

  // Yeni dashboard sayfası ekle
  openNew(): void {
    this.page = {
      id: 0,
      name: '',
      description: '',
      route: '',
      isActive: true,
      requiredPermission: '',
      allowedUserIds: [],
      createdAt: new Date(),
      updatedAt: new Date()
    };
    this.dialogHeader = 'Yeni Dashboard Sayfası Ekle';
    this.displayDialog = true;
    
    // Dropdown stillerini düzenle
    setTimeout(() => {
      this.updateDropdownPanelStyles();
      this.updateMultiSelectPanelStyles();
    }, 100);
  }

  // Kullanıcı ID'sine göre etiket getir
  getUserLabelById(userId: number): string {
    const user = this.userOptions.find(u => u.value === userId);
    return user ? user.label : '';
  }

  // Dashboard sayfası düzenle
  editPage(page: DashboardPage): void {
    this.page = { ...page };
    this.dialogHeader = 'Dashboard Sayfası Düzenle';
    this.displayDialog = true;
    
    // Dropdown stillerini düzenle
    setTimeout(() => {
      this.updateDropdownPanelStyles();
      this.updateMultiSelectPanelStyles();
    }, 100);
  }

  // Dashboard sayfası sil
  deletePage(page: DashboardPage): void {
    // Silme işlemi için onay iste
    if (confirm(`"${page.name}" sayfasını silmek istediğinize emin misiniz?`)) {
      // Gerçek uygulamada API'ye silme isteği gönderilecek
      // Şimdilik yerel listeden çıkarıyoruz
      this.dashboardPages = this.dashboardPages.filter(p => p.id !== page.id);
      
      // Filtrelenmiş listeyi güncelle
      this.filteredDashboardPages = this.dashboardPages.filter(p => p.id !== page.id);
      
      // Eğer arama yapılmışsa, filtrelemeyi tekrar uygula
      if (this.searchText) {
        this.onSearch({ target: { value: this.searchText } } as unknown as Event);
      }
      
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: `"${page.name}" sayfası başarıyla silindi.`
      });
    }
  }

  // Dialog kapatıldığında
  hideDialog(): void {
    this.displayDialog = false;
    
    // Dropdown panelini kapat
    this.closeDropdownPanel();
  }

  // Dropdown panelini kapat
  closeDropdownPanel(): void {
    const dropdownPanels = document.querySelectorAll('.p-dropdown-panel');
    dropdownPanels.forEach(panel => {
      if (panel instanceof HTMLElement) {
        panel.style.display = 'none';
      }
    });
  }

  // Sayfayı kaydet
  savePage(): void {
    if (!this.page.name || !this.page.route || !this.page.requiredPermission) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Lütfen tüm zorunlu alanları doldurun.'
      });
      return;
    }

    // Yeni sayfa ekleme
    if (this.page.id === 0) {
      // Gerçek uygulamada API'ye gönderilecek
      this.page.id = this.dashboardPages.length + 1;
      this.page.createdAt = new Date();
      this.page.updatedAt = new Date();
      this.dashboardPages.push(this.page);
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: 'Dashboard sayfası başarıyla eklendi.'
      });
    } 
    // Mevcut sayfayı güncelleme
    else {
      // Gerçek uygulamada API'ye gönderilecek
      const index = this.dashboardPages.findIndex(p => p.id === this.page.id);
      if (index !== -1) {
        this.page.updatedAt = new Date();
        this.dashboardPages[index] = this.page;
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Dashboard sayfası başarıyla güncellendi.'
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
      allowedUserIds: [],
      createdAt: new Date(),
      updatedAt: new Date()
    };
  }

  // Sayfaya git
  navigateTo(route: string): void {
    this.router.navigate([route]);
  }

  // Durum etiketi
  getStatusLabel(isActive: boolean): string {
    return isActive ? 'Aktif' : 'Pasif';
  }

  // Durum severity
  getStatusSeverity(isActive: boolean): TagSeverity {
    return isActive ? 'success' : 'danger';
  }

  // Arama işlemi
  onSearch(event: Event): void {
    const searchValue = this.searchText.toLowerCase();
    if (searchValue) {
      this.filteredDashboardPages = this.dashboardPages.filter(page => 
        page.name.toLowerCase().includes(searchValue) ||
        (page.description && page.description.toLowerCase().includes(searchValue)) ||
        page.route.toLowerCase().includes(searchValue) ||
        page.requiredPermission.toLowerCase().includes(searchValue)
      );
    } else {
      this.filteredDashboardPages = [...this.dashboardPages];
    }
  }

  // Aramayı temizle
  clearSearch(): void {
    this.searchText = '';
    this.filteredDashboardPages = [...this.dashboardPages];
  }

  // MultiSelect panel stillerini güncelle
  updateMultiSelectPanelStyles(): void {
    const multiSelectPanels = document.querySelectorAll('.dashboard-users-multiselect-panel');
    multiSelectPanels.forEach(panel => {
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
      }
    });
  }
} 