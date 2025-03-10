import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Permission, PermissionGroup } from '../shared/models/permission.model';
import { map } from 'rxjs/operators';

// ReferenceHandler.Preserve formatı için interface
interface PreserveFormat<T> {
  $id?: string;
  $values?: T[];
}

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private apiUrl = `${environment.apiUrl}/api/permissions`;

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
   * Tüm izinleri getirir
   */
  getAllPermissions(): Observable<Permission[]> {
    return this.http.get<any>(this.apiUrl).pipe(
      map(response => this.normalizeResponse<Permission>(response))
    );
  }

  /**
   * İzinleri gruplarına göre getirir
   */
  getPermissionsByGroups(): Observable<PermissionGroup[]> {
    return this.http.get<any>(`${this.apiUrl}/groups`).pipe(
      map(response => this.normalizeResponse<PermissionGroup>(response))
    );
  }

  /**
   * Belirli bir rolün izinlerini getirir
   */
  getPermissionsByRoleId(roleId: number): Observable<Permission[]> {
    return this.http.get<any>(`${this.apiUrl}/role/${roleId}`).pipe(
      map(response => this.normalizeResponse<Permission>(response))
    );
  }

  /**
   * Belirli bir kullanıcının izinlerini getirir
   */
  getPermissionsByUserId(userId: number): Observable<Permission[]> {
    return this.http.get<any>(`${this.apiUrl}/user/${userId}`).pipe(
      map(response => this.normalizeResponse<Permission>(response))
    );
  }

  /**
   * Bir role izinler atar
   */
  assignPermissionsToRole(roleId: number, permissionIds: number[]): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/role/${roleId}/assign`, { permissionIds });
  }

  /**
   * Bir kullanıcıya toplu izinler atar
   */
  assignPermissionsToUser(userId: number, permissionIds: number[]): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/user/${userId}/assign`, { permissionIds });
  }

  /**
   * Bir kullanıcıya izin atar
   */
  assignPermissionToUser(userId: number, permissionId: number, isGranted: boolean = true): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/user/${userId}/assign/${permissionId}?isGranted=${isGranted}`, {});
  }

  /**
   * Bir kullanıcının belirli bir iznini kaldırır
   */
  removeUserPermission(userId: number, permissionId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/user/${userId}/permission/${permissionId}`);
  }

  /**
   * Bir kullanıcının tüm özel izinlerini kaldırır
   */
  resetUserPermissions(userId: number): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/user/${userId}/reset`, {});
  }

  /**
   * Bir kullanıcının tüm özel izinlerini kaldırarak rol izinlerine sıfırlar
   */
  resetUserToRolePermissions(userId: number): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/user/${userId}/reset`, {});
  }

  /**
   * Bir kullanıcının belirli bir izne sahip olup olmadığını kontrol eder
   */
  checkUserPermission(userId: number, permissionName: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/user/${userId}/check/${permissionName}`);
  }
} 