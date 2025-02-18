import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../services/auth.service';

// PrimeNG Modülleri
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    CardModule,
    CheckboxModule,
    InputTextModule,
    PasswordModule,
    ToastModule
  ],
  providers: [MessageService]
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  rememberMe: boolean = false;

  constructor(
    private router: Router,
    private messageService: MessageService,
    private authService: AuthService
  ) {
    console.log('LoginComponent initialized');
    console.log('Current route:', this.router.url);
    console.log('Is user logged in:', this.authService.isLoggedIn());
  }

  onSubmit(): void {
    console.log('Giriş denemesi başladı:', { username: this.username });
    
    this.authService.login({ username: this.username, password: this.password }).subscribe({
      next: (response) => {
        console.log('Token alındı:', response.token ? 'Evet' : 'Hayır');
        console.log('Kullanıcı bilgisi alındı:', response.user ? 'Evet' : 'Hayır');
        console.log('Kullanıcı admin mi:', response.user?.isAdmin);
        
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Giriş başarılı! Dashboard\'a yönlendiriliyorsunuz...',
          life: 3000
        });
        
        // Token'ı localStorage'a kaydet
        localStorage.setItem('token', response.token);
        
        // 1 saniye bekleyip yönlendirme yap
        setTimeout(() => {
          console.log('Yönlendirme başlıyor...');
          console.log('Hedef route:', '/dashboard');
          
          this.router.navigate(['/dashboard']).then(() => {
            console.log('Yönlendirme başarılı');
            console.log('Yeni route:', this.router.url);
          }).catch(err => {
            console.error('Yönlendirme hatası:', err);
            console.error('Mevcut route:', this.router.url);
          });
        }, 1000);
      },
      error: (error) => {
        console.error('Giriş hatası:', error);
        console.error('Hata detayı:', error.error);
        
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Kullanıcı adı veya şifre hatalı! Doğru format: Kullanıcı adı: admin, Şifre: admin123',
          life: 5000
        });
      }
    });
  }
}
