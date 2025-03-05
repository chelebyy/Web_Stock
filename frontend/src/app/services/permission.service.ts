import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Permission, PermissionGroup } from '../models/permission.model';
import { environment } from '../../environments/environment';

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
  private normalizeResponse<T>(response: any): T {
    if (!response) return [] as unknown as T;
    
    // $values dizisi varsa
    if (response.$values) {
      return response.$values as T;
    } 
    // Dizi ise doğrudan döndür
    else if (Array.isArray(response)) {
      return response as T;
    } 
    // Başka özelliklerde veri varsa
    else if (typeof response === 'object') {
      // Nesne içinde dizi özelliği ara
      const arrayProps = Object.entries(response)
        .filter(([_, v]) => Array.isArray(v) || (v && typeof v === 'object' && '$values' in v));
      
      if (arrayProps.length > 0) {
        const [_, value] = arrayProps[0];
        
        if (Array.isArray(value)) {
          return value as T;
        } else if (value && typeof value === 'object' && '$values' in value) {
          return (value as any).$values as T;
        }
      }
    }
    
    // Son çare: orijinal yanıtı döndür
    return response as T;
  }

  getAllPermissions(): Observable<Permission[]> {
    return this.http.get<any>(this.apiUrl)
      .pipe(map(response => this.normalizeResponse<Permission[]>(response)));
  }

  getPermissionsByGroups(): Observable<PermissionGroup[]> {
    return this.http.get<any>(`${this.apiUrl}/groups`)
      .pipe(map(response => this.normalizeResponse<PermissionGroup[]>(response)));
  }

  getPermissionsByRoleId(roleId: number): Observable<Permission[]> {
    return this.http.get<any>(`${this.apiUrl}/role/${roleId}`)
      .pipe(map(response => this.normalizeResponse<Permission[]>(response)));
  }

  assignPermissionsToRole(roleId: number, permissionIds: number[]): Observable<boolean> {
    return this.http.post<any>(`${this.apiUrl}/role/${roleId}/assign`, { permissionIds })
      .pipe(map(response => response === true || response === "true" || response === "True"));
  }
} 