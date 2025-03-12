import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { User as BaseUser } from '../../../shared/models/user.model';

export interface DashboardUser extends Omit<BaseUser, 'id'> {
  id: number;
  hasPagePermission: boolean;
  fullName: string;
  sicil: string;
  isActive: boolean;
  isAdmin: boolean;
  createdAt: Date;
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
export class DashboardPermissionService {
  private apiUrl = `${environment.apiUrl}/api/dashboard-permissions`;

  constructor(private http: HttpClient) {
    console.log('[DashboardPermissionService] Initialized with API URL:', this.apiUrl);
  }

  // Sayfa detaylarını getir
  getPageDetails(pageId: number): Observable<DashboardPage> {
    // Gerçek uygulamada API'den veri çekilecek
    // Şimdilik örnek veri dönüyoruz
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
    return forkJoin({
      users: this.http.get<any>(`${environment.apiUrl}/api/users`).pipe(
        tap(response => {
          console.log('[DashboardPermissionService] Fetched users raw response:', response);
        }),
        map(response => {
          // API yanıtı $values içinde bir dizi olarak geliyorsa
          if (response && response.$values) {
            return response.$values;
          } else if (response && Array.isArray(response)) {
            return response;
          } else if (response && response.$id && response.$values && Array.isArray(response.$values)) {
            return response.$values;
          } else {
            console.warn('[DashboardPermissionService] Unexpected users response format:', response);
            return [];
          }
        }),
        tap(users => console.log('[DashboardPermissionService] Processed users array:', users))
      ),
      permissions: this.getPagePermissions(pageId)
    }).pipe(
      map(({ users, permissions }) => {
        console.log('[DashboardPermissionService] Processing users and permissions');
        const permissionUserIds = permissions.map(p => p.userId);
        
        if (!Array.isArray(users)) {
          console.warn('[DashboardPermissionService] Users is not an array:', users);
          return [];
        }
        
        return users.map(user => {
          if (!user || !user.id) {
            console.warn('[DashboardPermissionService] User without ID found:', user);
            return null;
          }
          
          // Ad Soyad oluşturma mantığını değiştir
          let fullName = `${user.firstName || ''} ${user.lastName || ''}`.trim();
          if (!fullName) {
            fullName = user.username; // Eğer ad soyad yoksa kullanıcı adını göster
          }
          
          const dashboardUser: DashboardUser = {
            ...user,
            id: user.id,
            hasPagePermission: permissionUserIds.includes(user.id),
            fullName: fullName,
            sicil: user.sicil || '',
            isActive: user.isActive !== undefined ? user.isActive : true,
            isAdmin: user.isAdmin !== undefined ? user.isAdmin : false,
            createdAt: user.createdAt || new Date()
          };
          console.log('[DashboardPermissionService] Processed user:', dashboardUser);
          return dashboardUser;
        }).filter((user): user is DashboardUser => user !== null);
      }),
      catchError(error => {
        console.error('[DashboardPermissionService] Error fetching users:', error);
        return of([]);
      })
    );
  }

  getPagePermissions(pageId: number): Observable<PagePermission[]> {
    console.log('[DashboardPermissionService] Fetching permissions for pageId:', pageId);
    
    // API endpoint bulunamadığı için mock veri dönelim
    // Gerçek API hazır olduğunda bu kısmı kaldırıp gerçek API çağrısını kullanabiliriz
    return of([]).pipe(
      tap(permissions => console.log('[DashboardPermissionService] Using mock permissions:', permissions))
    );
    
    // Gerçek API çağrısı (şu an 404 hatası veriyor)
    /*
    return this.http.get<PagePermission[]>(`${this.apiUrl}/page/${pageId}`).pipe(
      tap(permissions => console.log('[DashboardPermissionService] Fetched page permissions:', permissions)),
      catchError(error => {
        console.error('[DashboardPermissionService] Error fetching permissions:', error);
        return of([]);
      })
    );
    */
  }

  grantPermission(pageId: number, userId: number): Observable<any> {
    console.log('[DashboardPermissionService] Granting permission:', { pageId, userId });
    
    // API endpoint bulunamadığı için başarılı yanıt simüle edelim
    // Gerçek API hazır olduğunda bu kısmı kaldırıp gerçek API çağrısını kullanabiliriz
    return of({ success: true, pageId, userId }).pipe(
      tap(response => console.log('[DashboardPermissionService] Mock permission granted:', response))
    );
    
    // Gerçek API çağrısı (şu an 404 hatası verebilir)
    /*
    return this.http.post(`${this.apiUrl}`, { pageId, userId }).pipe(
      tap(response => console.log('[DashboardPermissionService] Permission granted:', response)),
      catchError(error => {
        console.error('[DashboardPermissionService] Error granting permission:', error);
        throw error;
      })
    );
    */
  }

  revokePermission(pageId: number, userId: number): Observable<any> {
    console.log('[DashboardPermissionService] Revoking permission:', { pageId, userId });
    
    // API endpoint bulunamadığı için başarılı yanıt simüle edelim
    // Gerçek API hazır olduğunda bu kısmı kaldırıp gerçek API çağrısını kullanabiliriz
    return of({ success: true, pageId, userId }).pipe(
      tap(response => console.log('[DashboardPermissionService] Mock permission revoked:', response))
    );
    
    // Gerçek API çağrısı (şu an 404 hatası verebilir)
    /*
    return this.http.delete(`${this.apiUrl}/${pageId}/${userId}`).pipe(
      tap(response => console.log('[DashboardPermissionService] Permission revoked:', response)),
      catchError(error => {
        console.error('[DashboardPermissionService] Error revoking permission:', error);
        throw error;
      })
    );
    */
  }
} 