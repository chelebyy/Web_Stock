import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, of, throwError } from 'rxjs';
import { map, catchError, tap, switchMap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { User as BaseUser } from '../../../shared/models/user.model';
import { AuthService } from '../../../core/authentication/auth.service';
import { BaseHttpService } from '../../../core/services/base-http.service';
import { UserService } from '../../user-management/services/user.service';
import { PagedResponse } from '../../../shared/models/paged-response.model';

export interface DashboardUser extends Omit<BaseUser, 'id'> {
  id: number;
  username: string;
  hasPagePermission: boolean;
  fullName: string;
  sicil: string;
  isActive: boolean;
  isAdmin: boolean;
  createdAt: Date;
  avatar?: string;
}

export interface PagePermission {
  id: number;
  userId: number;
  pageId: number;
  username: string;
  fullName: string;
  sicil: string;
  grantedAt: Date;
}

export interface DashboardPage {
  id: number;
  name: string;
  description: string;
  route: string;
  isActive: boolean;
  requiredPermission: string;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardPermissionService extends BaseHttpService<any> {
  private dashboardPermissionsPath = 'api/dashboard-permissions';
  private usersPath = 'api/users';

  private userService = inject(UserService);

  constructor(http: HttpClient, authService: AuthService) {
    super(http, authService, environment.apiUrl);
    console.log('[DashboardPermissionService] Initialized.');
  }

  getPageDetails(pageId: number): Observable<DashboardPage> {
    const mockPage: DashboardPage = {
      id: pageId,
      name: 'Admin Dashboard',
      description: 'Yönetici kontrol paneli',
      route: '/dashboard/admin-dashboard',
      isActive: true,
      requiredPermission: 'Pages.AdminDashboard'
    };
    return of(mockPage);
  }

  getUsers(pageId: number): Observable<DashboardUser[]> {
    console.log('[DashboardPermissionService] Fetching users for pageId:', pageId);
    
    return this.userService.getUsers(1, 1000).pipe(
      tap(response => console.log('[DashboardPermissionService] Fetched users via UserService:', response)),
      map(pagedResponse => pagedResponse.items),
      switchMap(users => {
        if (!Array.isArray(users)) {
          console.warn('[DashboardPermissionService] Users is not an array:', users);
          return of([]);
        }
        
        return this.getPagePermissions(pageId).pipe(
          map(permissions => {
            console.log('[DashboardPermissionService] Processing users and permissions');
            const permissionUserIds = permissions.map(p => p.userId);
            
            return users.map(user => {
              if (!user || !user.id) {
                console.warn('[DashboardPermissionService] User without ID found:', user);
                return null;
              }
              
              const fullName = (`${user.adi || ''} ${user.soyadi || ''}`.trim() || user.username) || '';
              const dashboardUser: DashboardUser = {
                ...user,
                id: user.id,
                username: user.username || '',
                hasPagePermission: permissionUserIds.includes(user.id),
                fullName: fullName,
                sicil: user.sicil || '',
                isActive: user.isActive !== undefined ? user.isActive : true,
                isAdmin: user.isAdmin !== undefined ? user.isAdmin : false,
                createdAt: user.createdAt || new Date()
              };
              return dashboardUser;
            }).filter((user): user is DashboardUser => user !== null);
          }),
          catchError(error => {
            console.error('[DashboardPermissionService] Error fetching permissions:', error.message || error);
            return of(users.map(user => {
              if (!user || !user.id) return null;
              
              const fullName = (`${user.adi || ''} ${user.soyadi || ''}`.trim() || user.username) || '';
              const dashboardUser: DashboardUser = {
                ...user,
                id: user.id,
                username: user.username || '',
                hasPagePermission: false,
                fullName: fullName,
                sicil: user.sicil || '',
                isActive: user.isActive !== undefined ? user.isActive : true,
                isAdmin: user.isAdmin !== undefined ? user.isAdmin : false,
                createdAt: user.createdAt || new Date()
              };
              return dashboardUser;
            }).filter((user): user is DashboardUser => user !== null));
          })
        );
      }),
      catchError(error => {
        console.error('[DashboardPermissionService] Error fetching users via UserService:', error.message || error);
        return of([]);
      })
    );
  }

  getPagePermissions(pageId: number): Observable<PagePermission[]> {
    console.log('[DashboardPermissionService] Fetching permissions for pageId:', pageId);
    const path = `${this.dashboardPermissionsPath}/page/${pageId}`;
    return this.get<PagePermission[]>(path).pipe(
      tap(permissions => console.log('[DashboardPermissionService] Fetched page permissions:', permissions)),
      catchError(error => {
        console.error('[DashboardPermissionService] Error fetching permissions:', error.message || error);
        return of([]);
      })
    );
  }

  grantPermission(pageId: number, userId: number): Observable<any> {
    console.log('[DashboardPermissionService] Granting permission:', { pageId, userId });
    return this.post<any>(this.dashboardPermissionsPath, { pageId, userId }).pipe(
      tap(response => console.log('[DashboardPermissionService] Permission granted:', response)),
      catchError(error => {
        console.error('[DashboardPermissionService] Error granting permission:', error.message || error);
        return throwError(() => error);
      })
    );
  }

  revokePermission(pageId: number, userId: number): Observable<any> {
    console.log('[DashboardPermissionService] Revoking permission:', { pageId, userId });
    const path = `${this.dashboardPermissionsPath}/${pageId}/${userId}`;
    return this.delete<any>(path).pipe(
      tap(response => console.log('[DashboardPermissionService] Permission revoked:', response)),
      catchError(error => {
        console.error('[DashboardPermissionService] Error revoking permission:', error.message || error);
        return throwError(() => error);
      })
    );
  }
} 