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
    TagModule
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
  
  // Grafik verileri
  userActivityData: any;
  equipmentStatusData: any;
  
  // Son aktiviteler
  recentActivities: any[] = [];
  
  // Yükleniyor durumu
  loading: boolean = true;

  constructor(
    private router: Router,
    private authService: AuthService,
    private messageService: MessageService
  ) {
    // Grafik verilerini hazırla
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
          tension: 0.4
        },
        {
          label: 'Yeni Kullanıcılar',
          data: [28, 48, 40, 19, 86, 27, 90],
          fill: false,
          borderColor: '#FFA726',
          tension: 0.4
        }
      ]
    };
    
    // Ekipman durumu grafiği
    this.equipmentStatusData = {
      labels: ['Aktif', 'Bakımda', 'Arızalı', 'Emekli'],
      datasets: [
        {
          data: [540, 125, 40, 85],
          backgroundColor: ['#36A2EB', '#FFCE56', '#FF6384', '#4BC0C0'],
          hoverBackgroundColor: ['#36A2EB', '#FFCE56', '#FF6384', '#4BC0C0']
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
        totalEquipment: 790
      };
      
      this.recentActivities = [
        { type: 'login', user: 'ahmet.yilmaz', time: '10 dakika önce', status: 'success' },
        { type: 'equipment_add', user: 'mehmet.demir', time: '25 dakika önce', status: 'success' },
        { type: 'user_update', user: 'admin', time: '1 saat önce', status: 'warning' },
        { type: 'system_backup', user: 'system', time: '3 saat önce', status: 'info' },
        { type: 'login_failed', user: 'unknown', time: '5 saat önce', status: 'danger' }
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
  
  navigateToItManagement(): void {
    this.router.navigate(['/it/inventory/computers']);
  }
  
  navigateToReports(): void {
    // Raporlar sayfası henüz oluşturulmadı
    this.messageService.add({
      severity: 'info',
      summary: 'Bilgi',
      detail: 'Raporlar modülü geliştirme aşamasındadır.'
    });
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
        },
        error: (err) => {
          console.error('Şifre değiştirme hatası:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: err.error?.message || 'Şifre değiştirme işlemi başarısız oldu!'
          });
        }
      });
  }
  
  getActivityIcon(type: string): string {
    switch (type) {
      case 'login': return 'pi pi-sign-in';
      case 'login_failed': return 'pi pi-lock';
      case 'equipment_add': return 'pi pi-plus-circle';
      case 'user_update': return 'pi pi-user-edit';
      case 'system_backup': return 'pi pi-database';
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
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
