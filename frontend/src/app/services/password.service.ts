import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { AuthService } from '../core/authentication/auth.service';
import { HttpStatusCodes } from './user.service';

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

  // Hata işleme
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Bir hata oluştu';
    
    if (error.error instanceof ErrorEvent) {
      // İstemci taraflı hata
      errorMessage = `Hata: ${error.error.message}`;
    } else {
      // Sunucu taraflı hata
      if (error.status === HttpStatusCodes.UNAUTHORIZED) {
        errorMessage = 'Oturum süresi dolmuş veya yetkiniz yok. Lütfen tekrar giriş yapın.';
      } else if (error.status === HttpStatusCodes.FORBIDDEN) {
        errorMessage = 'Bu işlemi gerçekleştirmek için yetkiniz yok.';
      } else if (error.status === HttpStatusCodes.NOT_FOUND) {
        errorMessage = 'İstek yapılan kaynak bulunamadı.';
      } else if (error.status === HttpStatusCodes.SERVER_ERROR) {
        errorMessage = 'Sunucu hatası. Lütfen daha sonra tekrar deneyin.';
      } else if (error.status === HttpStatusCodes.CONNECTION_ERROR) {
        errorMessage = 'Sunucuya bağlanılamadı. İnternet bağlantınızı kontrol edin.';
      } else {
        errorMessage = `Sunucu hatası: ${error.status}, mesaj: ${error.message}`;
      }
    }
    
    return throwError(() => new Error(errorMessage));
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
    
    return this.http.post(`${this.apiUrl}/Auth/change-password`, data, options)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Şifre sıfırlama isteği gönderir
   * @param email Kullanıcı e-posta adresi
   * @returns API yanıtı
   */
  requestPasswordReset(email: string): Observable<any> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post(`${this.apiUrl}/FixPassword/request-password-reset`, { email }, options)
      .pipe(
        catchError(this.handleError)
      );
  }

  /**
   * Şifre sıfırlama işlemini tamamlar
   * @param token Şifre sıfırlama token'ı
   * @param newPassword Yeni şifre
   * @returns API yanıtı
   */
  completePasswordReset(token: string, newPassword: string): Observable<any> {
    const options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };
    return this.http.post(`${this.apiUrl}/auth/reset-password`, {
      token,
      newPassword
    }, options)
      .pipe(
        catchError(this.handleError)
      );
  }
} 