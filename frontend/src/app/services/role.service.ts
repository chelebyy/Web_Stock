import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Role } from '../shared/models/role.model';
import { environment } from '../../environments/environment';
import { AuthService } from '../core/authentication/auth.service';
import { HttpStatusCodes } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private apiUrl = `${environment.apiUrl}/api/role`;

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

  // API yanıtını normalleştir
  private normalizeResponse<T>(response: any): T {
    // ReferenceHandler.Preserve formatını işle ($id ve $values)
    if (response && response.$values && Array.isArray(response.$values)) {
      return response.$values as T;
    }
    
    // Normal dizi yanıtını işle
    if (Array.isArray(response)) {
      return response as T;
    }
    
    // Diğer olası formatlara bak
    if (response && typeof response === 'object') {
      // value ya da Value içinde olabilir
      if (response.value && Array.isArray(response.value)) {
        return response.value as T;
      }
      
      if (response.Value && Array.isArray(response.Value)) {
        return response.Value as T;
      }
    }
    
    // Yanıt doğrudan bir nesne ise
    return response as T;
  }

  // Tüm rolleri getir
  getRoles(): Observable<Role[]> {
    const options = { headers: this.getHeaders() };
    
    return this.http.get<any>(this.apiUrl, options)
      .pipe(
        map(response => this.normalizeResponse<Role[]>(response)),
        catchError(this.handleError)
      );
  }

  // Belirli bir rolü getir
  getRole(id: number): Observable<Role> {
    const options = { headers: this.getHeaders() };
    
    return this.http.get<any>(`${this.apiUrl}/${id}`, options)
      .pipe(
        map(response => this.normalizeResponse<Role>(response)),
        catchError(this.handleError)
      );
  }

  // Yeni rol oluştur
  createRole(role: Role): Observable<Role> {
    const options = { headers: this.getHeaders() };
    
    return this.http.post<any>(this.apiUrl, role, options)
      .pipe(
        map(response => this.normalizeResponse<Role>(response)),
        catchError(this.handleError)
      );
  }

  // Rol güncelle
  updateRole(id: number, role: Role): Observable<Role> {
    const options = { headers: this.getHeaders() };
    
    return this.http.put<any>(`${this.apiUrl}/${id}`, role, options)
      .pipe(
        map(response => this.normalizeResponse<Role>(response)),
        catchError(this.handleError)
      );
  }

  // Rol sil
  deleteRole(id: number): Observable<void> {
    const options = { headers: this.getHeaders() };
    
    return this.http.delete<void>(`${this.apiUrl}/${id}`, options)
      .pipe(
        catchError(this.handleError)
      );
  }
} 