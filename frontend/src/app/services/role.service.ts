import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from '../shared/models/role.model';
import { environment } from '../../environments/environment';
import { AuthService } from '../core/authentication/auth.service';
import { BaseHttpService } from '../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService extends BaseHttpService<Role> {
  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService, `${environment.apiUrl}/api/role`);
  }

  // Tüm rolleri getir
  getRoles(): Observable<Role[]> {
    return this.get<Role[]>('');
  }

  // Belirli bir rolü getir
  getRole(id: number): Observable<Role> {
    return this.get<Role>(`${id}`);
  }

  // Yeni rol oluştur
  createRole(role: Role): Observable<Role> {
    return this.post<Role>('', role);
  }

  // Rol güncelle
  updateRole(id: number, role: Role): Observable<Role> {
    return this.put<Role>(`${id}`, role);
  }

  // Rol sil
  deleteRole(id: number): Observable<void> {
    return this.delete<void>(`${id}`);
  }
} 