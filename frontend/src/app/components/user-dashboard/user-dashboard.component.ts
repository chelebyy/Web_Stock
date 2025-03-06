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

// Servisler
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { PasswordService } from '../../services/password.service';

interface Activity {
  action: string;
  date: Date;
}

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DragDropModule,
    ToastModule,
    CardModule,
    TableModule,
    ButtonModule,
    InputTextModule
  ],
  providers: [MessageService],
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss']
})
export class UserDashboardComponent implements OnInit {
  username: string = '';
  profileImageUrl: string = '';
  showAdminMenu: boolean = false;
  showPasswordChange: boolean = false;
  
  // Şifre değiştirme formu için değişkenler
  currentPassword: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  
  // Şifre görünürlüğü için değişkenler
  showCurrentPassword: boolean = false;
  showNewPassword: boolean = false;
  showConfirmPassword: boolean = false;

  // Son aktiviteler için
  recentActivities: Activity[] = [];

  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService,
    private passwordService: PasswordService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.loadUserInfo();
    this.loadRecentActivities();
  }

  loadUserInfo(): void {
    const currentUser = this.authService.getCurrentUser();
    if (currentUser) {
      this.username = currentUser.username;
      
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

    this.passwordService.changePassword(userId, this.currentPassword, this.newPassword).subscribe({
      next: (response: any) => {
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

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
