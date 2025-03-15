import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../core/authentication/auth.service';
import { BaseHttpService } from '../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordService extends BaseHttpService {
  private endpoint = '/api/Auth';
  private fixPasswordEndpoint = '/api/FixPassword';

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }

  /**
   * Kullanıcının şifresini değiştirir
   * @param userId Kullanıcı ID
   * @param currentPassword Mevcut şifre
   * @param newPassword Yeni şifre
   * @returns API yanıtı
   */
  changePassword(userId: number, currentPassword: string, newPassword: string): Observable<any> {
    const data = {
      userId,
      currentPassword,
      newPassword
    };
    
    return this.post<any>(`${this.endpoint}/change-password`, data);
  }

  /**
   * Şifre sıfırlama isteği gönderir
   * @param email Kullanıcı e-posta adresi
   * @returns API yanıtı
   */
  requestPasswordReset(email: string): Observable<any> {
    // Bu istek için özel header kullanılıyor, token gerekmediği için
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    
    return this.http.post(`${this.apiUrl}${this.fixPasswordEndpoint}/request-password-reset`, { email }, { headers })
      .pipe(
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Şifre sıfırlama işlemini tamamlar
   * @param token Şifre sıfırlama token'ı
   * @param newPassword Yeni şifre
   * @returns API yanıtı
   */
  completePasswordReset(token: string, newPassword: string): Observable<any> {
    // Bu istek için özel header kullanılıyor, token gerekmediği için
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    
    return this.http.post(`${this.apiUrl}${this.endpoint}/reset-password`, {
      token,
      newPassword
    }, { headers })
      .pipe(
        catchError(error => this.handleError(error))
      );
  }
} 