import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Permission } from '../shared/models/permission.model';
import { UserPermission } from '../shared/models/user-permission.model';
import { AuthService } from '../core/authentication/auth.service';
import { BaseHttpService } from '../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class UserPermissionService extends BaseHttpService {
  private endpoint = '/api/UserPermission';

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }

  /**
   * Kullanıcının tüm izinlerini getirir (rol izinleri + özel izinler)
   */
  getUserPermissions(userId: number): Observable<Permission[]> {
    return this.get<Permission[]>(`${this.endpoint}/user/${userId}`);
  }

  /**
   * Kullanıcının sadece özel izinlerini getirir
   */
  getUserCustomPermissions(userId: number): Observable<UserPermission[]> {
    return this.get<UserPermission[]>(`${this.endpoint}/user/${userId}/custom`);
  }

  /**
   * Kullanıcıya izin atar
   */
  assignPermissionToUser(userId: number, permissionId: number, isGranted: boolean = true): Observable<any> {
    return this.post<any>(`${this.endpoint}/user/${userId}/permission/${permissionId}?isGranted=${isGranted}`, {});
  }

  /**
   * Kullanıcıdan özel izni kaldırır
   */
  removeUserPermission(userId: number, permissionId: number): Observable<any> {
    return this.delete<any>(`${this.endpoint}/user/${userId}/permission/${permissionId}`);
  }

  /**
   * Kullanıcının tüm özel izinlerini kaldırarak rol izinlerine sıfırlar
   */
  resetUserToRolePermissions(userId: number): Observable<any> {
    return this.post<any>(`${this.endpoint}/user/${userId}/reset`, {});
  }

  /**
   * Kullanıcının belirli bir izne sahip olup olmadığını kontrol eder
   */
  hasPermission(userId: number, permissionName: string): Observable<boolean> {
    return this.get<boolean>(`${this.endpoint}/user/${userId}/check/${permissionName}`);
  }
} 