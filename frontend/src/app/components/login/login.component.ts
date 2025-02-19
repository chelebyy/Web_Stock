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
  ]
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  rememberMe: boolean = false;

  constructor(
    private router: Router,
    private messageService: MessageService,
    private authService: AuthService
  ) {}

  onSubmit(): void {
    if (!this.username || !this.password) {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Kullanıcı adı ve şifre gereklidir!',
        life: 3000
      });
      return;
    }

    console.log('Login isteği gönderiliyor:', { username: this.username, password: this.password });

    this.authService.login({ username: this.username, password: this.password }).subscribe({
      next: (response) => {
        console.log('Login başarılı:', response);
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Giriş başarılı!',
          life: 3000
        });
        
        setTimeout(() => {
          this.router.navigate(['/dashboard']);
        }, 1000);
      },
      error: (error) => {
        console.error('Login hatası:', error);
        let errorMessage = 'Bir hata oluştu!';
        
        if (error.status === 400) {
          errorMessage = error.error?.message || 'Geçersiz kullanıcı adı veya şifre!';
        } else if (error.status === 401) {
          errorMessage = 'Kullanıcı adı veya şifre hatalı!';
        }

        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: errorMessage,
          life: 3000
        });
      }
    });
  }
}
