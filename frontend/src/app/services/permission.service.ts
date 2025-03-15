import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Permission, PermissionGroup } from '../shared/models/permission.model';
import { map } from 'rxjs/operators';
import { AuthService } from '../core/authentication/auth.service';
import { BaseHttpService } from '../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionService extends BaseHttpService {
  private endpoint = '/api/Permissions';

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }

  /**
   * Tüm izinleri getirir
   */
  getAllPermissions(): Observable<Permission[]> {
    return this.get<Permission[]>(this.endpoint);
  }

  /**
   * İzinleri gruplarına göre getirir
   */
  getPermissionsByGroups(): Observable<PermissionGroup[]> {
    return this.get<PermissionGroup[]>(`${this.endpoint}/groups`);
  }

  /**
   * Belirli bir rolün izinlerini getirir
   */
  getPermissionsByRoleId(roleId: number): Observable<Permission[]> {
    return this.get<Permission[]>(`${this.endpoint}/role/${roleId}`);
  }

  /**
   * Belirli bir kullanıcının izinlerini getirir
   */
  getPermissionsByUserId(userId: number): Observable<Permission[]> {
    return this.get<Permission[]>(`${this.endpoint}/user/${userId}`);
  }

  /**
   * Bir role izinler atar
   */
  assignPermissionsToRole(roleId: number, permissionIds: number[]): Observable<boolean> {
    return this.post<boolean>(`${this.endpoint}/role/${roleId}/assign`, { permissionIds });
  }

  /**
   * Bir kullanıcıya toplu izinler atar
   */
  assignPermissionsToUser(userId: number, permissionIds: number[]): Observable<boolean> {
    return this.post<boolean>(`${this.endpoint}/user/${userId}/assign`, { permissionIds });
  }

  /**
   * Bir kullanıcıya izin atar
   */
  assignPermissionToUser(userId: number, permissionId: number, isGranted: boolean = true): Observable<boolean> {
    return this.post<boolean>(`${this.endpoint}/user/${userId}/assign/${permissionId}?isGranted=${isGranted}`, {});
  }

  /**
   * Bir kullanıcının belirli bir iznini kaldırır
   */
  removeUserPermission(userId: number, permissionId: number): Observable<boolean> {
    return this.delete<boolean>(`${this.endpoint}/user/${userId}/permission/${permissionId}`);
  }

  /**
   * Bir kullanıcının tüm özel izinlerini kaldırır
   */
  resetUserPermissions(userId: number): Observable<boolean> {
    return this.post<boolean>(`${this.endpoint}/user/${userId}/reset`, {});
  }

  /**
   * Bir kullanıcının tüm özel izinlerini kaldırarak rol izinlerine sıfırlar
   */
  resetUserToRolePermissions(userId: number): Observable<boolean> {
    return this.post<boolean>(`${this.endpoint}/user/${userId}/reset`, {});
  }

  /**
   * Bir kullanıcının belirli bir izne sahip olup olmadığını kontrol eder
   */
  checkUserPermission(userId: number, permissionName: string): Observable<boolean> {
    return this.get<boolean>(`${this.endpoint}/user/${userId}/check/${permissionName}`);
  }
} 