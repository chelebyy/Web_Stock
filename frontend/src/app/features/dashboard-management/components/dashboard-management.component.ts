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
import { TagModule } from 'primeng/tag';
import { TooltipModule } from 'primeng/tooltip';
import { AuthService } from '../../../core/authentication/auth.service';
import { Router } from '@angular/router';

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
    TagModule,
    TooltipModule
  ],
  providers: [MessageService]
})
export class DashboardManagementComponent implements OnInit, AfterViewInit {
  // Dashboard sayfaları listesi
  dashboardPages: DashboardPage[] = [];
  
  // Filtrelenmiş dashboard sayfaları
  filteredDashboardPages: DashboardPage[] = [];
  
  // Arama metni
  searchText = '';
  
  // Seçilen dashboard sayfası
  selectedPage: DashboardPage | null = null;
  
  // Yükleniyor durumu
  loading = true;
  
  // İzin seçenekleri
  permissionOptions: any[] = [];

  constructor(
    private messageService: MessageService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadDashboardPages();
    this.loadPermissionOptions();
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
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 2,
          name: 'Kullanıcı Yönetimi',
          description: 'Kullanıcıları görüntüle, düzenle ve yönet',
          route: '/user-management',
          isActive: true,
          requiredPermission: 'Pages.UserManagement',
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

  // Sayfaya git
  navigateTo(route: string): void {
    this.router.navigate([route]);
  }

  // Durum etiketi getir
  getStatusSeverity(isActive: boolean): TagSeverity {
    return isActive ? 'success' : 'danger';
  }

  // Durum etiketi metni getir
  getStatusLabel(isActive: boolean): string {
    return isActive ? 'Aktif' : 'Pasif';
  }

  // Arama işlevi
  onSearch(event: Event): void {
    const searchValue = (event.target as HTMLInputElement).value.toLowerCase();
    this.searchText = searchValue;
    
    if (searchValue) {
      this.filteredDashboardPages = this.dashboardPages.filter(page => 
        page.name.toLowerCase().includes(searchValue) ||
        page.description.toLowerCase().includes(searchValue) ||
        page.route.toLowerCase().includes(searchValue) ||
        page.requiredPermission.toLowerCase().includes(searchValue)
      );
    } else {
      this.filteredDashboardPages = [...this.dashboardPages];
    }
  }

  // Arama alanını temizle
  clearSearch(): void {
    this.searchText = '';
    this.filteredDashboardPages = [...this.dashboardPages];
  }

  managePermissions(page: DashboardPage): void {
    // İzinler sayfasına yönlendir
    this.router.navigate(['/dashboard-management/permissions', page.id]);
  }

  navigateToPermissions(page: DashboardPage): void {
    // İzinler sayfasına yönlendir
    this.router.navigate(['/dashboard-management/permissions', page.id, page.name]);
  }
} 