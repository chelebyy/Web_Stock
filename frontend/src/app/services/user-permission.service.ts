import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Permission } from '../shared/models/permission.model';
import { UserPermission } from '../shared/models/user-permission.model';
import { AuthService } from '../core/authentication/auth.service';
import { BaseHttpService } from '../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class UserPermissionService extends BaseHttpService<UserPermission> {
  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService, `${environment.apiUrl}/api/UserPermission`);
  }

  /**
   * Kullanıcının tüm izinlerini getirir (rol izinleri + özel izinler)
   */
  getUserPermissions(userId: number): Observable<Permission[]> {
    return super.get<Permission[]>(`user/${userId}`);
  }

  /**
   * Kullanıcının sadece özel izinlerini getirir
   */
  getUserCustomPermissions(userId: number): Observable<UserPermission[]> {
    return super.get<UserPermission[]>(`user/${userId}/custom`);
  }

  /**
   * Kullanıcıya izin atar
   */
  assignPermissionToUser(userId: number, permissionId: number, isGranted = true): Observable<boolean> {
    return super.post<boolean>(`user/${userId}/permission/${permissionId}?isGranted=${isGranted}`, {})
      .pipe(map(response => !!response));
  }

  /**
   * Kullanıcıdan özel izni kaldırır
   */
  removeUserPermission(userId: number, permissionId: number): Observable<boolean> {
    return super.delete<boolean>(`user/${userId}/permission/${permissionId}`)
      .pipe(map(response => !!response));
  }

  /**
   * Kullanıcının tüm özel izinlerini kaldırarak rol izinlerine sıfırlar
   */
  resetUserToRolePermissions(userId: number): Observable<boolean> {
    return super.post<boolean>(`user/${userId}/reset`, {})
      .pipe(map(response => !!response));
  }

  /**
   * Kullanıcının belirli bir izne sahip olup olmadığını kontrol eder
   */
  hasPermission(userId: number, permissionName: string): Observable<boolean> {
    return super.get<boolean>(`user/${userId}/check/${permissionName}`);
  }
} 