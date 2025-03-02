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
    ToastModule
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

  constructor(
    private router: Router,
    private authService: AuthService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.username = user.username;
      this.isAdmin = user.isAdmin;
    }
  }

  navigateToUserManagement(): void {
    this.router.navigate(['/user-management']);
  }

  navigateToRoleManagement(): void {
    this.router.navigate(['/role-management']);
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

    // Burada şifre değiştirme API çağrısı yapılacak
    this.messageService.add({
      severity: 'success',
      summary: 'Başarılı',
      detail: 'Şifreniz başarıyla değiştirildi!'
    });
    this.togglePasswordChange();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
