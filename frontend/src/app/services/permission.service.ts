import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Permission, PermissionGroup } from '../shared/models/permission.model';
import { map, catchError } from 'rxjs/operators';
import { AuthService } from '../core/authentication/auth.service';
import { HttpStatusCodes } from './user.service';

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
   * Tüm izinleri getirir
   */
  getAllPermissions(): Observable<Permission[]> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(this.apiUrl, options).pipe(
      map(response => this.normalizeResponse<Permission>(response))
    );
  }

  /**
   * İzinleri gruplarına göre getirir
   */
  getPermissionsByGroups(): Observable<PermissionGroup[]> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(`${this.apiUrl}/groups`, options).pipe(
      map(response => this.normalizeResponse<PermissionGroup>(response))
    );
  }

  /**
   * Belirli bir rolün izinlerini getirir
   */
  getPermissionsByRoleId(roleId: number): Observable<Permission[]> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(`${this.apiUrl}/role/${roleId}`, options).pipe(
      map(response => this.normalizeResponse<Permission>(response))
    );
  }

  /**
   * Belirli bir kullanıcının izinlerini getirir
   */
  getPermissionsByUserId(userId: number): Observable<Permission[]> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(`${this.apiUrl}/user/${userId}`, options).pipe(
      map(response => this.normalizeResponse<Permission>(response))
    );
  }

  /**
   * Bir role izinler atar
   */
  assignPermissionsToRole(roleId: number, permissionIds: number[]): Observable<boolean> {
    const options = { headers: this.getHeaders() };
    return this.http.post<boolean>(`${this.apiUrl}/role/${roleId}/assign`, { permissionIds }, options);
  }

  /**
   * Bir kullanıcıya toplu izinler atar
   */
  assignPermissionsToUser(userId: number, permissionIds: number[]): Observable<boolean> {
    const options = { headers: this.getHeaders() };
    return this.http.post<boolean>(`${this.apiUrl}/user/${userId}/assign`, { permissionIds }, options);
  }

  /**
   * Bir kullanıcıya izin atar
   */
  assignPermissionToUser(userId: number, permissionId: number, isGranted: boolean = true): Observable<boolean> {
    const options = { headers: this.getHeaders() };
    return this.http.post<boolean>(`${this.apiUrl}/user/${userId}/assign/${permissionId}?isGranted=${isGranted}`, {}, options);
  }

  /**
   * Bir kullanıcının belirli bir iznini kaldırır
   */
  removeUserPermission(userId: number, permissionId: number): Observable<boolean> {
    const options = { headers: this.getHeaders() };
    return this.http.delete<boolean>(`${this.apiUrl}/user/${userId}/permission/${permissionId}`, options);
  }

  /**
   * Bir kullanıcının tüm özel izinlerini kaldırır
   */
  resetUserPermissions(userId: number): Observable<boolean> {
    const options = { headers: this.getHeaders() };
    return this.http.post<boolean>(`${this.apiUrl}/user/${userId}/reset`, {}, options);
  }

  /**
   * Bir kullanıcının tüm özel izinlerini kaldırarak rol izinlerine sıfırlar
   */
  resetUserToRolePermissions(userId: number): Observable<boolean> {
    const options = { headers: this.getHeaders() };
    return this.http.post<boolean>(`${this.apiUrl}/user/${userId}/reset`, {}, options);
  }

  /**
   * Bir kullanıcının belirli bir izne sahip olup olmadığını kontrol eder
   */
  checkUserPermission(userId: number, permissionName: string): Observable<boolean> {
    const options = { headers: this.getHeaders() };
    return this.http.get<boolean>(`${this.apiUrl}/user/${userId}/check/${permissionName}`, options);
  }
} 