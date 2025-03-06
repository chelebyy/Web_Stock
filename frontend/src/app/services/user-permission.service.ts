import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Permission } from '../models/permission.model';
import { UserPermission } from '../models/user-permission.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserPermissionService {
  private apiUrl = `${environment.apiUrl}/api/UserPermission`;

  constructor(private http: HttpClient) { }

  // API yanıtını normalize et
  private normalizeResponse<T>(response: any): T[] {
    if (!response) return [];
    
    // $values dizisi varsa
    if (response.$values) {
      return response.$values as T[];
    } 
    // Dizi ise doğrudan döndür
    else if (Array.isArray(response)) {
      return response as T[];
    } 
    // Başka özelliklerde veri varsa
    else if (typeof response === 'object') {
      // Nesne içinde dizi özelliği ara
      const arrayProps = Object.entries(response)
        .filter(([_, v]) => Array.isArray(v) || (v && typeof v === 'object' && '$values' in v));
      
      if (arrayProps.length > 0) {
        const [_, value] = arrayProps[0];
        
        if (Array.isArray(value)) {
          return value as T[];
        } else if (value && typeof value === 'object' && '$values' in value) {
          return (value as any).$values as T[];
        }
      }
    }
    
    // Son çare: boş dizi döndür
    return [];
  }

  /**
   * Kullanıcının tüm izinlerini getirir (rol izinleri + özel izinler)
   */
  getUserPermissions(userId: number): Observable<Permission[]> {
    return this.http.get<any>(`${this.apiUrl}/user/${userId}`).pipe(
      map(response => this.normalizeResponse<Permission>(response))
    );
  }

  /**
   * Kullanıcının sadece özel izinlerini getirir
   */
  getUserCustomPermissions(userId: number): Observable<UserPermission[]> {
    return this.http.get<any>(`${this.apiUrl}/user/${userId}/custom`).pipe(
      map(response => this.normalizeResponse<UserPermission>(response))
    );
  }

  /**
   * Kullanıcıya izin atar
   */
  assignPermissionToUser(userId: number, permissionId: number, isGranted: boolean = true): Observable<any> {
    return this.http.post(`${this.apiUrl}/user/${userId}/permission/${permissionId}?isGranted=${isGranted}`, {});
  }

  /**
   * Kullanıcıdan özel izni kaldırır
   */
  removeUserPermission(userId: number, permissionId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/user/${userId}/permission/${permissionId}`);
  }

  /**
   * Kullanıcının tüm özel izinlerini kaldırarak rol izinlerine sıfırlar
   */
  resetUserToRolePermissions(userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/user/${userId}/reset`, {});
  }

  /**
   * Kullanıcının belirli bir izne sahip olup olmadığını kontrol eder
   */
  hasPermission(userId: number, permissionName: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/user/${userId}/check/${permissionName}`);
  }
} 