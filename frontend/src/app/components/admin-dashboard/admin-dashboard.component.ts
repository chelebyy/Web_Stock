import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { ChartModule } from 'primeng/chart';
import { TableModule } from 'primeng/table';
import { ProgressBarModule } from 'primeng/progressbar';
import { TagModule } from 'primeng/tag';
import { LogService, UserActivityLog } from '../../services/log.service';
import { PaginatorModule } from 'primeng/paginator';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';

// PrimeNG Tag bileşeni için geçerli severity tipleri
type TagSeverity = 'success' | 'info' | 'warning' | 'danger' | 'secondary' | 'contrast' | undefined;

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule, 
    CardModule, 
    ButtonModule, 
    FormsModule, 
    InputTextModule, 
    PasswordModule,
    ToastModule,
    ChartModule,
    TableModule,
    ProgressBarModule,
    TagModule,
    PaginatorModule,
    DropdownModule,
    CalendarModule,
    DialogModule
  ],
  providers: [MessageService],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  username: string = '';
  isAdmin: boolean = true;
  showPasswordChange: boolean = false;
  currentPassword: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  
  // Sistem özeti verileri
  systemSummary = {
    totalUsers: 0,
    activeUsers: 0,
    totalRoles: 0,
    totalEquipment: 0
  };
  
  // Kullanıcı aktivite logları
  userActivityLogs: UserActivityLog[] = [];
  filteredActivityLogs: UserActivityLog[] = [];
  
  // Log detay dialog
  showLogDetailsDialog: boolean = false;
  selectedLog: UserActivityLog | null = null;
  
  // Aktivite filtreleri
  activityFilter = {
    activityType: null as string | null,
    status: null as string | null,
    dateRange: null as Date[] | null,
    username: ''
  };
  
  // Sayfalama
  currentPage: number = 1;
  pageSize: number = 10;
  totalRecords: number = 0;
  totalFilteredRecords: number = 0;
  
  // Grafik verileri (eski)
  userActivityData: any;
  
  // Grafik seçenekleri (eski)
  chartOptions: any;
  
  // Son aktiviteler
  recentActivities: any[] = [];
  
  // Yükleniyor durumu
  loading: boolean = true;

  // Aktivite tipi seçenekleri
  activityTypeOptions = [
    { label: 'Kullanıcı Girişi', value: 'login' },
    { label: 'Kullanıcı Çıkışı', value: 'logout' },
    { label: 'Kullanıcı Ekleme', value: 'user_add' },
    { label: 'Kullanıcı Güncelleme', value: 'user_update' },
    { label: 'Rol Güncelleme', value: 'role_update' },
    { label: 'Şifre Değiştirme', value: 'password_change' },
    { label: 'Ayar Güncelleme', value: 'settings_update' },
    { label: 'Başarısız Giriş', value: 'login_failed' },
    { label: 'Sistem Yedekleme', value: 'system_backup' }
  ];
  
  // Durum seçenekleri
  statusOptions = [
    { label: 'Bilgi', value: 'info' },
    { label: 'Başarılı', value: 'success' },
    { label: 'Uyarı', value: 'warning' },
    { label: 'Hata', value: 'danger' }
  ];

  constructor(
    private router: Router,
    private authService: AuthService,
    private messageService: MessageService,
    private logService: LogService
  ) {
    // Grafik verilerini hazırla (eski)
    this.initChartData();
  }

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.username = user.username;
      this.isAdmin = user.isAdmin;
    }
    
    // Sistem verilerini yükle
    this.loadSystemData();
    
    // Grafik seçeneklerini ayarla (eski)
    this.setupChartOptions();
    
    // Bekleyen logları senkronize et
    this.logService.syncPendingLogs();
    
    // Kullanıcı aktivite loglarını yükle
    this.loadUserActivityLogs();
    
    // Giriş aktivitesini otomatik olarak logla
    this.logService.logUserActivity({
      activityType: 'dashboard_access',
      description: 'Yönetim paneline erişim sağlandı',
      status: 'info'
    }).subscribe();
  }
  
  // Kullanıcı aktivite loglarını yükle
  loadUserActivityLogs(): void {
    this.loading = true;
    this.logService.getUserActivityLogs(this.currentPage, this.pageSize)
      .subscribe({
        next: (response) => {
          if (response && response.Logs) {
            this.userActivityLogs = response.Logs;
            this.totalRecords = response.TotalItems || 0;
          } else {
            this.userActivityLogs = [];
            this.totalRecords = 0;
            console.warn('API yanıtında Logs verisi bulunamadı:', response);
          }
          this.applyFilters(); // Filtreleri uygula
          this.loading = false;
        },
        error: (error) => {
          console.error('Kullanıcı aktivite loglarını yükleme hatası:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Kullanıcı aktivite logları yüklenemedi.'
          });
          this.loading = false;
          
          // Hata durumunda örnek veriler göster
          this.userActivityLogs = this.generateSampleLogs();
          this.totalRecords = this.userActivityLogs.length;
          this.applyFilters(); // Filtreleri uygula
        }
      });
  }
  
  // Örnek log verileri oluştur (API bağlantısı olmadığında)
  private generateSampleLogs(): UserActivityLog[] {
    return [
      {
        username: 'admin',
        activityType: 'login',
        description: 'Kullanıcı giriş yaptı',
        timestamp: new Date(),
        status: 'success'
      },
      {
        username: 'ahmet.yilmaz',
        activityType: 'user_update',
        description: 'Profil bilgileri güncellendi',
        timestamp: new Date(Date.now() - 3600000),
        status: 'info'
      },
      {
        username: 'mehmet.kaya',
        activityType: 'login_failed',
        description: 'Başarısız giriş denemesi',
        timestamp: new Date(Date.now() - 7200000),
        status: 'danger'
      },
      {
        username: 'admin',
        activityType: 'role_update',
        description: 'Kullanıcı rolü güncellendi: ahmet.yilmaz -> Admin',
        timestamp: new Date(Date.now() - 86400000),
        status: 'success'
      },
      {
        username: 'system',
        activityType: 'system_backup',
        description: 'Otomatik sistem yedeklemesi tamamlandı',
        timestamp: new Date(Date.now() - 172800000),
        status: 'info'
      }
    ];
  }
  
  // Filtreleri uygula
  filterLogs(): void {
    this.applyFilters();
  }
  
  // Filtreleri temizle
  clearFilters(): void {
    this.activityFilter = {
      activityType: null,
      status: null,
      dateRange: null,
      username: ''
    };
    this.applyFilters();
    this.messageService.add({
      severity: 'info',
      summary: 'Bilgi',
      detail: 'Tüm filtreler temizlendi.'
    });
  }
  
  // Tarih filtresini temizle
  clearDateFilter(): void {
    this.activityFilter.dateRange = null;
    this.applyFilters();
  }
  
  // Aktif filtre var mı kontrol et
  hasActiveFilters(): boolean {
    return !!(
      this.activityFilter.activityType || 
      this.activityFilter.status || 
      this.activityFilter.dateRange || 
      this.activityFilter.username
    );
  }
  
  // Filtreleri uygula
  private applyFilters(): void {
    let filtered = [...this.userActivityLogs];
    
    // Aktivite tipi filtresi
    if (this.activityFilter.activityType) {
      filtered = filtered.filter(log => log.activityType === this.activityFilter.activityType);
    }
    
    // Durum filtresi
    if (this.activityFilter.status) {
      filtered = filtered.filter(log => log.status === this.activityFilter.status);
    }
    
    // Kullanıcı adı filtresi
    if (this.activityFilter.username) {
      const username = this.activityFilter.username.toLowerCase();
      filtered = filtered.filter(log => log.username.toLowerCase().includes(username));
    }
    
    // Tarih aralığı filtresi
    if (this.activityFilter.dateRange && this.activityFilter.dateRange.length === 2) {
      const startDate = new Date(this.activityFilter.dateRange[0]);
      startDate.setHours(0, 0, 0, 0);
      
      const endDate = new Date(this.activityFilter.dateRange[1]);
      endDate.setHours(23, 59, 59, 999);
      
      filtered = filtered.filter(log => {
        const logDate = new Date(log.timestamp);
        return logDate >= startDate && logDate <= endDate;
      });
    }
    
    this.filteredActivityLogs = filtered;
    this.totalFilteredRecords = filtered.length;
  }
  
  // Log detaylarını göster
  showLogDetails(log: UserActivityLog): void {
    this.selectedLog = log;
    this.showLogDetailsDialog = true;
  }
  
  // Aktivite tipi etiketini getir
  getActivityTypeLabel(type: string): string {
    const option = this.activityTypeOptions.find(opt => opt.value === type);
    return option ? option.label : type;
  }
  
  // Durum etiketini getir
  getStatusLabel(status: string): string {
    const option = this.statusOptions.find(opt => opt.value === status);
    return option ? option.label : status;
  }
  
  // Sayfa değiştiğinde
  onPageChange(event: any): void {
    this.currentPage = event.page + 1;
    this.pageSize = event.rows;
    this.loadUserActivityLogs();
  }
  
  private setupChartOptions(): void {
    this.chartOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'top',
          labels: {
            usePointStyle: true,
            padding: 20
          }
        },
        tooltip: {
          mode: 'index',
          intersect: false
        }
      },
      scales: {
        x: {
          grid: {
            display: false
          }
        },
        y: {
          beginAtZero: true,
          grid: {
            color: 'rgba(0, 0, 0, 0.05)'
          }
        }
      }
    };
  }
  
  private initChartData(): void {
    // Kullanıcı aktivite grafiği
    this.userActivityData = {
      labels: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz'],
      datasets: [
        {
          label: 'Aktif Kullanıcılar',
          data: [65, 59, 80, 81, 56, 55, 40],
          fill: false,
          borderColor: '#42A5F5',
          tension: 0.4,
          backgroundColor: 'rgba(66, 165, 245, 0.2)',
          borderWidth: 2,
          pointBackgroundColor: '#42A5F5',
          pointBorderColor: '#fff',
          pointHoverBackgroundColor: '#fff',
          pointHoverBorderColor: '#42A5F5'
        },
        {
          label: 'Yeni Kullanıcılar',
          data: [28, 48, 40, 19, 86, 27, 90],
          fill: false,
          borderColor: '#FFA726',
          tension: 0.4,
          backgroundColor: 'rgba(255, 167, 38, 0.2)',
          borderWidth: 2,
          pointBackgroundColor: '#FFA726',
          pointBorderColor: '#fff',
          pointHoverBackgroundColor: '#fff',
          pointHoverBorderColor: '#FFA726'
        }
      ]
    };
  }
  
  private loadSystemData(): void {
    // Gerçek uygulamada bu veriler API'den gelecek
    setTimeout(() => {
      this.systemSummary = {
        totalUsers: 125,
        activeUsers: 87,
        totalRoles: 8,
        totalEquipment: 0
      };
      
      this.recentActivities = [
        { type: 'login', user: 'ahmet.yilmaz', time: '10 dakika önce', status: 'success' },
        { type: 'user_add', user: 'admin', time: '15 dakika önce', status: 'success' },
        { type: 'role_update', user: 'admin', time: '25 dakika önce', status: 'info' },
        { type: 'user_update', user: 'admin', time: '1 saat önce', status: 'warning' },
        { type: 'system_backup', user: 'system', time: '3 saat önce', status: 'info' },
        { type: 'login_failed', user: 'unknown', time: '5 saat önce', status: 'danger' },
        { type: 'settings_update', user: 'admin', time: '1 gün önce', status: 'info' }
      ];
      
      this.loading = false;
    }, 1000);
  }

  navigateToUserManagement(): void {
    this.router.navigate(['/user-management']);
  }

  navigateToRoleManagement(): void {
    this.router.navigate(['/role-management']);
  }
  
  navigateToSettings(): void {
    // Ayarlar sayfası henüz oluşturulmadı
    this.messageService.add({
      severity: 'info',
      summary: 'Bilgi',
      detail: 'Sistem ayarları modülü geliştirme aşamasındadır.'
    });
    // Geliştirme tamamlandığında: this.router.navigate(['/settings']);
  }
  
  navigateToAuditLogs(): void {
    // Denetim kayıtları sayfası henüz oluşturulmadı
    this.messageService.add({
      severity: 'info',
      summary: 'Bilgi',
      detail: 'Denetim kayıtları modülü geliştirme aşamasındadır.'
    });
    // Geliştirme tamamlandığında: this.router.navigate(['/audit-logs']);
  }

  togglePasswordChange(): void {
    this.showPasswordChange = !this.showPasswordChange;
    if (!this.showPasswordChange) {
      this.currentPassword = '';
      this.newPassword = '';
      this.confirmPassword = '';
    }
  }

  changePassword(): void {
    if (this.newPassword !== this.confirmPassword) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Yeni şifreler eşleşmiyor!'
      });
      return;
    }
    
    if (this.newPassword.length < 6) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Şifre en az 6 karakter olmalıdır!'
      });
      return;
    }

    // API çağrısı yap
    this.authService.changePassword(this.currentPassword, this.newPassword)
      .subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Şifreniz başarıyla değiştirildi!'
          });
          this.togglePasswordChange();
          
          // Şifre değiştirme aktivitesini logla
          this.logService.logUserActivity({
            activityType: 'password_change',
            description: 'Kullanıcı şifresini değiştirdi',
            status: 'success'
          }).subscribe();
        },
        error: (err) => {
          console.error('Şifre değiştirme hatası:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: err.error?.message || 'Şifre değiştirme işlemi başarısız oldu!'
          });
          
          // Başarısız şifre değiştirme aktivitesini logla
          this.logService.logUserActivity({
            activityType: 'password_change',
            description: 'Şifre değiştirme başarısız oldu',
            status: 'danger',
            details: { error: err.error?.message }
          }).subscribe();
        }
      });
  }
  
  getActivityIcon(type: string): string {
    switch (type) {
      case 'login': return 'pi pi-sign-in';
      case 'logout': return 'pi pi-sign-out';
      case 'login_failed': return 'pi pi-lock';
      case 'user_add': return 'pi pi-user-plus';
      case 'user_update': return 'pi pi-user-edit';
      case 'role_update': return 'pi pi-id-card';
      case 'system_backup': return 'pi pi-database';
      case 'settings_update': return 'pi pi-cog';
      case 'password_change': return 'pi pi-key';
      case 'dashboard_access': return 'pi pi-desktop';
      default: return 'pi pi-info-circle';
    }
  }
  
  getActivitySeverity(status: string): TagSeverity {
    switch (status) {
      case 'success': return 'success';
      case 'warning': return 'warning';
      case 'danger': return 'danger';
      case 'info': return 'info';
      case 'secondary': return 'secondary';
      case 'contrast': return 'contrast';
      default: return 'info';
    }
  }

  logout(): void {
    // Çıkış aktivitesini logla
    this.logService.logUserActivity({
      activityType: 'logout',
      description: 'Kullanıcı çıkış yaptı',
      status: 'info'
    }).subscribe({
      next: () => {
        this.authService.logout();
        this.router.navigate(['/login']);
      },
      error: () => {
        // Hata olsa bile çıkış yap
        this.authService.logout();
        this.router.navigate(['/login']);
      }
    });
  }
}
