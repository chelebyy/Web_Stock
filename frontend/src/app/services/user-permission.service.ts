import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Permission } from '../models/permission.model';
import { UserPermission } from '../models/user-permission.model';

@Injectable({
  providedIn: 'root'
})
export class UserPermissionService {
  private apiUrl = `${environment.apiUrl}/api/UserPermission`;

  constructor(private http: HttpClient) { }

  /**
   * Kullanıcının tüm izinlerini getirir (rol izinleri + özel izinler)
   */
  getUserPermissions(userId: number): Observable<Permission[]> {
    return this.http.get<Permission[]>(`${this.apiUrl}/user/${userId}`);
  }

  /**
   * Kullanıcının sadece özel izinlerini getirir
   */
  getUserCustomPermissions(userId: number): Observable<UserPermission[]> {
    return this.http.get<UserPermission[]>(`${this.apiUrl}/user/${userId}/custom`);
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