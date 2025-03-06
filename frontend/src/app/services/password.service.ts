import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordService {
  private apiUrl = `${environment.apiUrl}/api`;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  // HTTP Headers oluştur
  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  /**
   * Kullanıcının şifresini değiştirir
   * @param userId Kullanıcı ID
   * @param currentPassword Mevcut şifre
   * @param newPassword Yeni şifre
   * @returns API yanıtı
   */
  changePassword(userId: number, currentPassword: string, newPassword: string): Observable<any> {
    const options = { headers: this.getHeaders() };
    const data = {
      userId,
      currentPassword,
      newPassword
    };
    
    return this.http.post(`${this.apiUrl}/auth/change-password`, data, options);
  }

  /**
   * Şifre sıfırlama isteği gönderir
   * @param email Kullanıcı e-posta adresi
   * @returns API yanıtı
   */
  requestPasswordReset(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/request-password-reset`, { email });
  }

  /**
   * Şifre sıfırlama işlemini tamamlar
   * @param token Şifre sıfırlama token'ı
   * @param newPassword Yeni şifre
   * @returns API yanıtı
   */
  completePasswordReset(token: string, newPassword: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/reset-password`, {
      token,
      newPassword
    });
  }
} 