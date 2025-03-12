import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';

// PrimeNG Modülleri
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { InputSwitchModule } from 'primeng/inputswitch';
import { DividerModule } from 'primeng/divider';
import { TooltipModule } from 'primeng/tooltip';

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
  selector: 'app-dashboard-edit',
  templateUrl: './dashboard-edit.component.html',
  styleUrls: ['./dashboard-edit.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    CardModule,
    ButtonModule,
    InputTextModule,
    TextareaModule,
    DropdownModule,
    ToastModule,
    InputSwitchModule,
    DividerModule,
    TooltipModule
  ],
  providers: [MessageService]
})
export class DashboardEditComponent implements OnInit {
  // Form grubu
  dashboardForm!: FormGroup;
  
  // Sayfa ID'si
  pageId: number = 0;
  
  // Yükleniyor durumu
  loading: boolean = true;
  
  // Kaydediliyor durumu
  saving: boolean = false;
  
  // İzin seçenekleri
  permissionOptions: any[] = [];
  
  // Mevcut sayfa
  currentPage: DashboardPage | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    // Form oluştur
    this.initForm();
    
    // İzin seçeneklerini yükle
    this.loadPermissionOptions();
    
    // Route parametrelerini al
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.pageId = +params['id'];
        this.loadDashboardPage(this.pageId);
      } else {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Sayfa ID bulunamadı'
        });
        this.navigateBack();
      }
    });
  }

  // Form başlatma
  initForm(): void {
    this.dashboardForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(250)]],
      route: ['', [Validators.required, Validators.maxLength(100)]],
      requiredPermission: ['', Validators.required],
      isActive: [true]
    });
  }

  // İzin seçeneklerini yükle
  loadPermissionOptions(): void {
    // Gerçek uygulamada API'den veri çekilecek
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

  // Dashboard sayfasını yükle
  loadDashboardPage(id: number): void {
    // Gerçek uygulamada API'den veri çekilecek
    setTimeout(() => {
      // Örnek veri
      const dashboardPages = [
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
      
      // ID'ye göre sayfayı bul
      const page = dashboardPages.find(p => p.id === id);
      
      if (page) {
        this.currentPage = page;
        
        // Form değerlerini doldur
        this.dashboardForm.patchValue({
          name: page.name,
          description: page.description,
          route: page.route,
          requiredPermission: page.requiredPermission,
          isActive: page.isActive
        });
        
        this.loading = false;
      } else {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `ID: ${id} olan dashboard sayfası bulunamadı`
        });
        this.navigateBack();
      }
    }, 500);
  }

  // Formu kaydet
  saveDashboardPage(): void {
    if (this.dashboardForm.invalid) {
      // Form geçerli değilse, tüm hataları göster
      Object.keys(this.dashboardForm.controls).forEach(key => {
        const control = this.dashboardForm.get(key);
        if (control) {
          control.markAsTouched();
        }
      });
      
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Lütfen form alanlarını doğru şekilde doldurun'
      });
      
      return;
    }
    
    this.saving = true;
    
    // Form değerlerini al
    const formValues = this.dashboardForm.value;
    
    // Gerçek uygulamada API'ye kaydetme isteği gönderilecek
    setTimeout(() => {
      // Başarılı kaydetme simülasyonu
      this.saving = false;
      
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: 'Dashboard sayfası başarıyla güncellendi'
      });
      
      // Kısa bir süre sonra geri dön
      setTimeout(() => {
        this.navigateBack();
      }, 1500);
    }, 1000);
  }

  // Geri dön
  navigateBack(): void {
    this.router.navigate(['/dashboard-management']);
  }

  // Form kontrollerinin geçerliliğini kontrol et
  isFieldInvalid(fieldName: string): boolean {
    const field = this.dashboardForm.get(fieldName);
    return field ? field.invalid && (field.dirty || field.touched) : false;
  }

  // Form kontrollerinin hata mesajlarını al
  getErrorMessage(fieldName: string): string {
    const field = this.dashboardForm.get(fieldName);
    
    if (!field) return '';
    
    if (field.hasError('required')) {
      return 'Bu alan zorunludur';
    }
    
    if (field.hasError('maxlength')) {
      const maxLength = field.errors?.['maxlength'].requiredLength;
      return `Bu alan en fazla ${maxLength} karakter olmalıdır`;
    }
    
    return '';
  }
} 