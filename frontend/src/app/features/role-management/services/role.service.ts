import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Role } from '../../../shared/models/role.model';
import { PagedResponse } from '../../../shared/models/paged-response.model';
import { environment } from '../../../../environments/environment';
import { BaseHttpService } from '../../../core/services/base-http.service';
import { RoleStateService } from './role-state.service';
import { AuthService } from '../../../core/authentication/auth.service';

export interface RoleSlimDto {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class RoleService extends BaseHttpService<Role | RoleSlimDto> {
  private readonly stateService = inject(RoleStateService);
  private readonly apiRoute = 'api/Roles';

  constructor(
    http: HttpClient,
    authService: AuthService
  ) { 
    super(http, authService, environment.apiUrl);
  }

  loadRoles(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortField: string | null = 'name',
    sortOrder: string | null = 'asc',
    name: string | null = null
  ): void {
    this.stateService.setLoading(true);
    this.getRoles({ pageNumber, pageSize, sortField, sortOrder, name }).subscribe({
      next: (response) => {
        // The backend returns PagedResponse<RoleSlimDto>, we need PagedResponse<Role> for the state
        // This is a temporary workaround until the state is also updated to use RoleSlimDto
        const rolesResponse: PagedResponse<Role> = {
            ...response,
            items: response.items.map(item => ({...item, description: ''})) // Add dummy description
        };
        this.stateService.setRoles(rolesResponse);
        this.stateService.setLoading(false);
      },
      error: (error) => {
        this.stateService.setError(error);
        this.stateService.setLoading(false);
      }
    });
  }

  getRoles(params: {
    pageNumber: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    name: string | null
  }): Observable<PagedResponse<RoleSlimDto>> {
    let httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString());

    if (params.sortField) {
      httpParams = httpParams.set('sortField', params.sortField);
    }
    if (params.sortOrder) {
      httpParams = httpParams.set('sortOrder', params.sortOrder);
    }
    if (params.name) {
      httpParams = httpParams.set('name', params.name);
    }
    
    return this.get<PagedResponse<RoleSlimDto>>(this.apiRoute, httpParams);
  }

  // GET /api/Roles/{id} - Belirli bir rolü getir
  getRole(id: number): Observable<Role> {
    return this.get<Role>(`${this.apiRoute}/${id}`);
  }

  // POST /api/Roles - Yeni rol oluştur
  createRole(role: Omit<Role, 'id' | 'userCount' | 'permissions'>): Observable<Role> {
    return this.post<Role>(this.apiRoute, role).pipe(
      tap(() => this.loadRoles())
    );
  }

  // PUT /api/Roles/{id} - Rolü güncelle
  updateRole(id: number, role: Partial<Role>): Observable<Role> {
    return this.put<Role>(`${this.apiRoute}/${id}`, role).pipe(
      tap(() => this.loadRoles())
    );
  }

  // DELETE /api/Roles/{id} - Rolü sil
  deleteRole(id: number): Observable<void> {
    return this.delete<void>(`${this.apiRoute}/${id}`).pipe(
      tap(() => this.loadRoles())
    );
  }
} 