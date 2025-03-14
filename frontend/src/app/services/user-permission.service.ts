import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Permission } from '../shared/models/permission.model';
import { UserPermission } from '../shared/models/user-permission.model';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from '../core/authentication/auth.service';
import { HttpStatusCodes } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class UserPermissionService {
  private apiUrl = `${environment.apiUrl}/api/UserPermission`;

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

  // API yanıtını normalize et
  private normalizeResponse<T>(response: any): T[] {
    if (!response) return [];
    
    // $values dizisi varsa (ReferenceHandler.Preserve formatı)
    if (response.$values && Array.isArray(response.$values)) {
      return response.$values as T[];
    } 
    // Dizi ise doğrudan döndür
    else if (Array.isArray(response)) {
      return response as T[];
    } 
    // Başka özelliklerde veri varsa
    else if (response && typeof response === 'object') {
      // value ya da Value içinde olabilir
      if (response.value && Array.isArray(response.value)) {
        return response.value as T[];
      }
      
      if (response.Value && Array.isArray(response.Value)) {
        return response.Value as T[];
      }
    }
    
    // Son çare: boş dizi döndür
    return [];
  }

  /**
   * Kullanıcının tüm izinlerini getirir (rol izinleri + özel izinler)
   */
  getUserPermissions(userId: number): Observable<Permission[]> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(`${this.apiUrl}/user/${userId}`, options).pipe(
      map(response => this.normalizeResponse<Permission>(response))
    );
  }

  /**
   * Kullanıcının sadece özel izinlerini getirir
   */
  getUserCustomPermissions(userId: number): Observable<UserPermission[]> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(`${this.apiUrl}/user/${userId}/custom`, options).pipe(
      map(response => this.normalizeResponse<UserPermission>(response))
    );
  }

  /**
   * Kullanıcıya izin atar
   */
  assignPermissionToUser(userId: number, permissionId: number, isGranted: boolean = true): Observable<any> {
    const options = { headers: this.getHeaders() };
    return this.http.post(`${this.apiUrl}/user/${userId}/permission/${permissionId}?isGranted=${isGranted}`, {}, options);
  }

  /**
   * Kullanıcıdan özel izni kaldırır
   */
  removeUserPermission(userId: number, permissionId: number): Observable<any> {
    const options = { headers: this.getHeaders() };
    return this.http.delete(`${this.apiUrl}/user/${userId}/permission/${permissionId}`, options);
  }

  /**
   * Kullanıcının tüm özel izinlerini kaldırarak rol izinlerine sıfırlar
   */
  resetUserToRolePermissions(userId: number): Observable<any> {
    const options = { headers: this.getHeaders() };
    return this.http.post(`${this.apiUrl}/user/${userId}/reset`, {}, options);
  }

  /**
   * Kullanıcının belirli bir izne sahip olup olmadığını kontrol eder
   */
  hasPermission(userId: number, permissionName: string): Observable<boolean> {
    const options = { headers: this.getHeaders() };
    return this.http.get<boolean>(`${this.apiUrl}/user/${userId}/check/${permissionName}`, options);
  }
} 