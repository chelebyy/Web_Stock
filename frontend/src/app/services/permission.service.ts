import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Permission, PermissionGroup } from '../shared/models/permission.model';
import { AuthService } from '../core/authentication/auth.service';
import { BaseHttpService } from '../core/services/base-http.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PermissionService extends BaseHttpService<Permission> {
  private readonly apiRoute = 'api/Permissions'; // API route

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) { 
    // BaseHttpService'e base API URL'sini veriyoruz
    super(http, authService, environment.apiUrl);
  }

  /**
   * Tüm izinleri getirir
   */
  getAllPermissions(): Observable<Permission[]> {
    // super.get metodu path bekliyor
    return super.get<Permission[]>(this.apiRoute);
  }

  /**
   * İzinleri gruplarına göre getirir
   */
  getPermissionsByGroups(): Observable<PermissionGroup[]> {
    // Path base URL'e göreceli olmalı
    return super.get<PermissionGroup[]>(`${this.apiRoute}/groups`);
  }

  /**
   * Belirli bir rolün izinlerini getirir
   */
  getPermissionsByRoleId(roleId: number): Observable<Permission[]> {
    return super.get<Permission[]>(`${this.apiRoute}/role/${roleId}`);
  }

  /**
   * Belirli bir kullanıcının izinlerini getirir
   */
  getPermissionsByUserId(userId: number): Observable<Permission[]> {
    return super.get<Permission[]>(`${this.apiRoute}/user/${userId}`);
  }

  /**
   * Bir role izinler atar
   */
  assignPermissionsToRole(roleId: number, permissionIds: number[]): Observable<boolean> {
    return super.post<boolean>(`${this.apiRoute}/role/${roleId}/assign`, { permissionIds })
      .pipe(map(response => !!response)); // Şimdilik map kalsın
  }

  /**
   * Bir kullanıcıya toplu izinler atar
   */
  assignPermissionsToUser(userId: number, permissionIds: number[]): Observable<boolean> {
    return super.post<boolean>(`${this.apiRoute}/user/${userId}/assign`, { permissionIds })
      .pipe(map(response => !!response)); // Şimdilik map kalsın
  }

  /**
   * Bir kullanıcıya izin atar
   */
  assignPermissionToUser(userId: number, permissionId: number, isGranted = true): Observable<boolean> {
    return super.post<boolean>(`${this.apiRoute}/user/${userId}/assign/${permissionId}?isGranted=${isGranted}`, {})
      .pipe(map(response => !!response)); // Şimdilik map kalsın
  }

  /**
   * Bir kullanıcının belirli bir iznini kaldırır
   */
  removeUserPermission(userId: number, permissionId: number): Observable<boolean> {
    return super.delete<boolean>(`${this.apiRoute}/user/${userId}/permission/${permissionId}`)
      .pipe(map(response => !!response)); // Şimdilik map kalsın
  }

  /**
   * Bir kullanıcının tüm özel izinlerini kaldırır
   */
  resetUserPermissions(userId: number): Observable<boolean> {
    return super.post<boolean>(`${this.apiRoute}/user/${userId}/reset`, {})
      .pipe(map(response => !!response)); // Şimdilik map kalsın
  }

  /**
   * Bir kullanıcının tüm özel izinlerini kaldırarak rol izinlerine sıfırlar
   */
  resetUserToRolePermissions(userId: number): Observable<boolean> {
    // Bu metodun endpoint'i resetUserPermissions ile aynı görünüyor. Backend'i kontrol etmek gerekebilir.
    // Şimdilik aynı endpoint'i kullanıyoruz.
    return super.post<boolean>(`${this.apiRoute}/user/${userId}/reset`, {})
      .pipe(map(response => !!response)); // Şimdilik map kalsın
  }

  /**
   * Bir kullanıcının belirli bir izne sahip olup olmadığını kontrol eder
   */
  checkUserPermission(userId: number, permissionName: string): Observable<boolean> {
    return super.get<boolean>(`${this.apiRoute}/user/${userId}/check/${permissionName}`)
      .pipe(map(response => !!response)); // Şimdilik map kalsın
  }
} 