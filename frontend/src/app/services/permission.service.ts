import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Permission, PermissionGroup } from '../models/permission.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private apiUrl = `${environment.apiUrl}/api/permissions`;

  constructor(private http: HttpClient) { }

  getAllPermissions(): Observable<Permission[]> {
    return this.http.get<Permission[]>(this.apiUrl);
  }

  getPermissionsByGroups(): Observable<PermissionGroup[]> {
    return this.http.get<PermissionGroup[]>(`${this.apiUrl}/groups`);
  }

  getPermissionsByRoleId(roleId: number): Observable<Permission[]> {
    return this.http.get<Permission[]>(`${this.apiUrl}/role/${roleId}`);
  }

  assignPermissionsToRole(roleId: number, permissionIds: number[]): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/role/${roleId}/assign`, { permissionIds });
  }
} 