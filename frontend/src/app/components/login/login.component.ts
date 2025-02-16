import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';

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
    private messageService: MessageService
  ) {}

  onSubmit(): void {
    // TODO: Gerçek kimlik doğrulama işlemi eklenecek
    if (this.username === 'admin' && this.password === 'admin') {
      this.messageService.add({
        severity: 'success',
        summary: 'Başarılı',
        detail: 'Giriş başarılı!',
        life: 3000
      });
      this.router.navigate(['/user-management']);
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Kullanıcı adı veya şifre hatalı!',
        life: 3000
      });
    }
  }
}
