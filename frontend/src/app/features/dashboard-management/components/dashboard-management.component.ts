import { Component, OnInit } from '@angular/core';
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
    DialogModule,
    CheckboxModule,
    TagModule
  ],
  providers: [MessageService]
})
export class DashboardManagementComponent implements OnInit {
  // Dashboard sayfaları listesi
  dashboardPages: DashboardPage[] = [];
  
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
    updatedAt: new Date()
  };
  
  // Dialog başlığı
  dialogHeader: string = '';
  
  // Yükleniyor durumu
  loading: boolean = true;
  
  // İzin seçenekleri
  permissionOptions: any[] = [];

  constructor(
    private messageService: MessageService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Sayfa yüklendiğinde dashboard sayfalarını getir
    this.loadDashboardPages();
    
    // İzin seçeneklerini yükle
    this.loadPermissionOptions();
  }

  // Dashboard sayfalarını yükle
  loadDashboardPages(): void {
    // Gerçek uygulamada API'den veri çekilecek
    // Şimdilik örnek veriler kullanıyoruz
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
          name: 'Kullanıcı Dashboard',
          description: 'Kullanıcı kontrol paneli',
          route: '/dashboard/user-dashboard',
          isActive: true,
          requiredPermission: 'Pages.UserDashboard',
          createdAt: new Date('2025-01-01'),
          updatedAt: new Date('2025-01-01')
        },
        {
          id: 3,
          name: 'Bilgi İşlem Dashboard',
          description: 'Bilgi İşlem kontrol paneli',
          route: '/bilgi-islem',
          isActive: true,
          requiredPermission: 'Pages.BilgiIslem.View',
          createdAt: new Date('2025-02-15'),
          updatedAt: new Date('2025-02-15')
        },
        {
          id: 4,
          name: 'Revir Dashboard',
          description: 'Revir kontrol paneli',
          route: '/revir',
          isActive: true,
          requiredPermission: 'Pages.Revir.View',
          createdAt: new Date('2025-02-15'),
          updatedAt: new Date('2025-02-15')
        }
      ];
      this.loading = false;
    }, 1000);
  }

  // İzin seçeneklerini yükle
  loadPermissionOptions(): void {
    // Gerçek uygulamada API'den veri çekilecek
    // Şimdilik örnek veriler kullanıyoruz
    this.permissionOptions = [
      { label: 'Admin Dashboard', value: 'Pages.AdminDashboard' },
      { label: 'Kullanıcı Dashboard', value: 'Pages.UserDashboard' },
      { label: 'Bilgi İşlem Görüntüleme', value: 'Pages.BilgiIslem.View' },
      { label: 'Revir Görüntüleme', value: 'Pages.Revir.View' },
      { label: 'Kullanıcı Yönetimi', value: 'Pages.UserManagement' },
      { label: 'Rol Yönetimi', value: 'Pages.RoleManagement' },
      { label: 'Sistem Ayarları', value: 'Pages.Settings' }
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
      createdAt: new Date(),
      updatedAt: new Date()
    };
    this.dialogHeader = 'Yeni Dashboard Sayfası Ekle';
    this.displayDialog = true;
  }

  // Dashboard sayfası düzenle
  editPage(page: DashboardPage): void {
    // Seçilen sayfanın kopyasını oluştur
    this.page = { ...page };
    this.dialogHeader = 'Dashboard Sayfası Düzenle';
    this.displayDialog = true;
  }

  // Dashboard sayfası sil
  deletePage(page: DashboardPage): void {
    // Silme işlemi için onay iste
    if (confirm(`"${page.name}" sayfasını silmek istediğinize emin misiniz?`)) {
      // Gerçek uygulamada API'ye silme isteği gönderilecek
      // Şimdilik yerel listeden çıkarıyoruz
      this.dashboardPages = this.dashboardPages.filter(p => p.id !== page.id);
      
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
  }

  // Dashboard sayfası kaydet
  savePage(): void {
    // Form doğrulama
    if (!this.page.name || !this.page.route || !this.page.requiredPermission) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Lütfen tüm zorunlu alanları doldurun.'
      });
      return;
    }

    // Yeni kayıt mı, güncelleme mi?
    if (this.page.id === 0) {
      // Yeni kayıt
      // Gerçek uygulamada API'ye kayıt isteği gönderilecek
      // Şimdilik yerel listeye ekliyoruz
      this.page.id = this.dashboardPages.length + 1;
      this.page.createdAt = new Date();
      this.page.updatedAt = new Date();
      this.dashboardPages.push({ ...this.page });
      
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: `"${this.page.name}" sayfası başarıyla eklendi.`
      });
    } else {
      // Güncelleme
      // Gerçek uygulamada API'ye güncelleme isteği gönderilecek
      // Şimdilik yerel listeyi güncelliyoruz
      this.page.updatedAt = new Date();
      const index = this.dashboardPages.findIndex(p => p.id === this.page.id);
      if (index !== -1) {
        this.dashboardPages[index] = { ...this.page };
      }
      
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: `"${this.page.name}" sayfası başarıyla güncellendi.`
      });
    }

    // Dialog kapat
    this.displayDialog = false;
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
} 