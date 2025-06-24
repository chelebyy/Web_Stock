import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DragDropModule } from '@angular/cdk/drag-drop';

// PrimeNG Modülleri
import { ToastModule } from 'primeng/toast';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { PasswordModule } from 'primeng/password';
import { PaginatorModule } from 'primeng/paginator';

// Servisler ve Direktifler
import { AuthService } from '../../../core/authentication/auth.service';
import { UserService } from '../../../services/user.service';
import { HasPermissionDirective } from '../../../shared/directives/has-permission.directive';

interface Activity {
  action: string;
  date: Date;
}

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DragDropModule,
    ToastModule,
    CardModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    PaginatorModule,
    HasPermissionDirective
  ],
  providers: [MessageService]
})
export class UserDashboardComponent implements OnInit {
  username = '';
  profileImageUrl = '';
  showAdminMenu = false;
  showPasswordChange = false;
  
  // Şifre değiştirme formu için değişkenler
  currentPassword = '';
  newPassword = '';
  confirmPassword = '';
  
  // Şifre görünürlüğü için değişkenler
  showCurrentPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;

  // Son aktiviteler için
  recentActivities: Activity[] = [];

  // Sayfa erişim yetkileri
  hasITAccess = false;
  hasAdminDashboardAccess = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadUserInfo();
    this.loadRecentActivities();
    this.checkPermissions();
  }

  // Kullanıcı izinlerini kontrol eden metot
  checkPermissions(): void {
    // Bilgi İşlem sayfasına erişim kontrolü
    this.hasITAccess = this.authService.hasPermission('Pages.BilgiIslem.View');
    
    // Admin paneline erişim kontrolü
    this.hasAdminDashboardAccess = this.authService.hasPermission('Pages.AdminDashboard');
    
    // Kullanıcı adına göre de kontrol edebiliriz
    const currentUser = this.authService.getCurrentUser();
    if (currentUser && currentUser.username === 'Test') {
      // Test kullanıcısının Bilgi İşlem modülüne erişimi var
      this.hasITAccess = true;
    } else if (currentUser && currentUser.username === 'Test2') {
      // Test2 kullanıcısının Bilgi İşlem modülüne erişimi yok
      this.hasITAccess = false;
    }
  }

  // Bilgi İşlem sayfasına erişim kontrolü
  hasAccessToIT(): boolean {
    return this.hasITAccess;
  }

  loadUserInfo(): void {
    const currentUser = this.authService.getCurrentUser();
    if (currentUser) {
      // Kullanıcı adını oluştur - öncelikle adi ve soyadi kullan, yoksa alternatif alanlar kullan
      if (currentUser.adi && currentUser.soyadi) {
        this.username = `${currentUser.adi} ${currentUser.soyadi}`;
      } else if (currentUser.username) {
        this.username = currentUser.username;
      } else {
        this.username = `Kullanıcı #${currentUser.id}`;
      }
      
      // Profil resmi için varsayılan resmi kullan
      // Backend'de profil resmi endpoint'i oluşturulana kadar API çağrısı yapma
      this.profileImageUrl = 'assets/images/default-avatar.png';
      
      // Backend'de profil resmi endpoint'i oluşturulduğunda aşağıdaki kodu aktif et
      /*
      this.userService.getUserProfilePicture(currentUser.id).subscribe({
        next: (data: Blob) => {
          if (data && data.size > 0) {
            const objectURL = URL.createObjectURL(data);
            this.profileImageUrl = objectURL;
          } else {
            console.log('Profil resmi bulunamadı veya boş, varsayılan resim kullanılıyor.');
          }
        },
        error: (error: any) => {
          console.error('Profil resmi yüklenirken hata oluştu:', error);
          // Hata detaylarını logla
          if (error.status === 404) {
            console.log('Profil resmi endpoint bulunamadı. Backend API endpoint kontrolü gerekiyor.');
          }
        }
      });
      */
    }
  }

  loadRecentActivities(): void {
    // Burada normalde bir API çağrısı yapılır, şimdilik örnek verilerle dolduruyoruz
    this.recentActivities = [
      { action: 'Sisteme giriş yapıldı', date: new Date() },
      { action: 'Bilgi İşlem modülüne erişildi', date: new Date(Date.now() - 24 * 60 * 60 * 1000) },
      { action: 'Şifre değiştirildi', date: new Date(Date.now() - 3 * 24 * 60 * 60 * 1000) },
      { action: 'Profil bilgileri güncellendi', date: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000) },
      { action: 'Dosya yüklendi: Rapor.pdf', date: new Date(Date.now() - 14 * 24 * 60 * 60 * 1000) }
    ];
  }

  toggleAdminMenu(event: Event): void {
    event.stopPropagation();
    this.showAdminMenu = !this.showAdminMenu;
    
    // Eğer admin menüsü açıksa, sayfa tıklamasında kapatmak için bir event listener ekle
    if (this.showAdminMenu) {
      setTimeout(() => {
        document.addEventListener('click', this.closeMenu);
      }, 0);
    } else {
      document.removeEventListener('click', this.closeMenu);
    }
  }

  closeMenu = () => {
    this.showAdminMenu = false;
    document.removeEventListener('click', this.closeMenu);
  }

  toggleCurrentPasswordVisibility(): void {
    this.showCurrentPassword = !this.showCurrentPassword;
  }

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  changePassword(): void {
    if (!this.currentPassword || !this.newPassword || !this.confirmPassword) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Tüm alanları doldurunuz.'
      });
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Yeni şifreler eşleşmiyor.'
      });
      return;
    }

    const userId = this.authService.getCurrentUser()?.id;
    if (!userId) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Kullanıcı kimliği bulunamadı.'
      });
      return;
    }

    // Şifre değiştirme işlemi
    this.authService.changePassword(this.currentPassword, this.newPassword).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Şifreniz başarıyla değiştirildi.'
        });
        this.showPasswordChange = false;
        this.resetPasswordForm();
        
        // Aktivitelere ekle
        this.recentActivities.unshift({ action: 'Şifre değiştirildi', date: new Date() });
        if (this.recentActivities.length > 5) {
          this.recentActivities.pop();
        }
      },
      error: (error: any) => {
        console.error('Şifre değiştirirken hata oluştu:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: error.error?.message || 'Şifre değiştirme işlemi başarısız oldu.'
        });
      }
    });
  }

  resetPasswordForm(): void {
    this.currentPassword = '';
    this.newPassword = '';
    this.confirmPassword = '';
    this.showCurrentPassword = false;
    this.showNewPassword = false;
    this.showConfirmPassword = false;
  }

  navigateToIT(): void {
    this.router.navigate(['/it']);
    
    // Aktivitelere ekle
    this.recentActivities.unshift({ action: 'Bilgi İşlem modülüne erişildi', date: new Date() });
    if (this.recentActivities.length > 5) {
      this.recentActivities.pop();
    }
  }

  navigateToBilgiIslem(): void {
    this.router.navigate(['/bilgi-islem']);
    
    // Aktivitelere ekle
    this.recentActivities.unshift({ action: 'Bilgi İşlem modülüne erişildi', date: new Date() });
    if (this.recentActivities.length > 5) {
      this.recentActivities.pop();
    }
  }

  navigateToRevir(): void {
    this.router.navigate(['/revir']);
    
    // Aktivitelere ekle
    this.recentActivities.unshift({ action: 'Revir modülüne erişildi', date: new Date() });
    if (this.recentActivities.length > 5) {
      this.recentActivities.pop();
    }
  }

  navigateToMedical(): void {
    this.router.navigate(['/medical']);
    
    // Aktivitelere ekle
    this.recentActivities.unshift({ action: 'Revir modülüne erişildi', date: new Date() });
    if (this.recentActivities.length > 5) {
      this.recentActivities.pop();
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  // Admin paneline yönlendirme metodu
  navigateToAdminDashboard(): void {
    this.router.navigate(['/dashboard/admin-dashboard']);
    
    // Aktivitelere ekle
    this.recentActivities.unshift({ action: 'Admin paneline erişildi', date: new Date() });
    if (this.recentActivities.length > 5) {
      this.recentActivities.pop();
    }
  }
}
