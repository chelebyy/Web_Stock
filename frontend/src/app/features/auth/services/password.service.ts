import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../core/authentication/auth.service';
import { BaseHttpService } from '../../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordService extends BaseHttpService<any> {

  constructor(http: HttpClient, authService: AuthService) {
    super(http, authService, environment.apiUrl);
  }

  /**
   * Kullanıcının şifresini değiştirir
   * @param userId Kullanıcı ID
   * @param currentPassword Mevcut şifre
   * @param newPassword Yeni şifre
   * @returns API yanıtı
   */
  changePassword(userId: number, currentPassword: string, newPassword: string): Observable<any> {
    const data = { userId, currentPassword, newPassword };
    return this.post<any>('api/auth/change-password', data);
  }

  /**
   * Şifre sıfırlama isteği gönderir
   * @param email Kullanıcı e-posta adresi
   * @returns API yanıtı
   */
  requestPasswordReset(email: string): Observable<any> {
    return this.post<any>('api/auth/request-password-reset', { email });
  }

  /**
   * Şifre sıfırlama işlemini tamamlar
   * @param token Şifre sıfırlama token'ı
   * @param newPassword Yeni şifre
   * @returns API yanıtı
   */
  completePasswordReset(token: string, newPassword: string): Observable<any> {
    const data = { token, newPassword };
    return this.post<any>('api/auth/reset-password', data);
  }
} 