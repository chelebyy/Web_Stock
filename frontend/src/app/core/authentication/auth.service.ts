import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, catchError, throwError } from 'rxjs';
import { LoginRequest, LoginResponse, User, CreateUserRequest } from './auth.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5037/api'; // API URL'sini 5037 portuna güncelledim
  private currentUserSubject: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);
  public currentUser$: Observable<User | null> = this.currentUserSubject.asObservable();
  private jwtHelper = new JwtHelperService();
  private permissions: string[] = [];

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.loadStoredUser();
  }

  private loadStoredUser(): void {
    const token = this.getToken();
    console.log('Token kontrolü:', token ? 'Token mevcut' : 'Token bulunamadı');
    
    if (token) {
      try {
        const decodedToken = this.jwtHelper.decodeToken(token);
        console.log('Token çözümlendi:', decodedToken);
        
        if (decodedToken) {
          const user: User = {
            id: parseInt(decodedToken.nameid),
            username: decodedToken.name,
            sicil: decodedToken.sicil || '',
            isAdmin: decodedToken.role === 'Admin',
            createdAt: decodedToken.created_at || '',
            lastLoginAt: decodedToken.last_login_at
          };
          console.log('Kullanıcı bilgileri:', user);
          this.currentUserSubject.next(user);

          // İzinleri yükle
          this.permissions = this.extractPermissionsFromToken(decodedToken);
          console.log('Kullanıcı izinleri:', this.permissions);
        }
      } catch (error) {
        console.error('Token çözümleme hatası:', error);
      }
    }
  }

  // Token'dan izinleri çıkartır
  private extractPermissionsFromToken(decodedToken: any): string[] {
    const permissions: string[] = [];
    
    console.log('Token çözümlenmiş tam içerik:', JSON.stringify(decodedToken, null, 2));
    console.log('Token içindeki tüm claim anahtarları:', Object.keys(decodedToken));
    console.log('Token içindeki izin claim\'leri:', { 
      Permission: decodedToken.Permission, 
      PermissionNames: decodedToken.PermissionNames,
      role: decodedToken.role
    });
    
    // Permission claim'lerini kontrol et
    if (decodedToken.Permission) {
      console.log('Permission içeriği türü:', typeof decodedToken.Permission);
      
      // String formatında tek bir izin olabilir
      if (typeof decodedToken.Permission === 'string') {
        permissions.push(decodedToken.Permission);
        console.log('Tek string izin eklendi:', decodedToken.Permission);
      }
      // Array formatında birden çok izin olabilir
      else if (Array.isArray(decodedToken.Permission)) {
        permissions.push(...decodedToken.Permission);
        console.log('Dizi olarak izinler eklendi:', decodedToken.Permission);
      } else {
        console.log('Permission claim var ama tanınmayan formatta:', typeof decodedToken.Permission);
        
        // Özel bir objeyse, içeriğini kontrol et
        try {
          const permObj = JSON.parse(JSON.stringify(decodedToken.Permission));
          console.log('Permission objesi içeriği:', permObj);
          
          // Eğer objeyse ve $values gibi bir property varsa, bu koleksiyon olabilir
          if (permObj && permObj.$values && Array.isArray(permObj.$values)) {
            console.log('Permission $values dizisi bulundu, ekleniyor:', permObj.$values);
            permissions.push(...permObj.$values);
          }
        } catch (error) {
          console.error('Permission içeriği parse edilemedi:', error);
        }
      }
    } else {
      console.log('Permission claim bulunamadı');
    }
    
    // Rol tabanlı izin kontrolü - Admin rolü için otomatik RoleManagement izni ekle
    if (decodedToken.role === 'Admin') {
      console.log('Admin rolü tespit edildi, Pages.RoleManagement izni ekleniyor');
      permissions.push('Pages.RoleManagement');
    }
    
    // PermissionNames claim'ini de kontrol et (bazı token yapılandırmalarında kullanılabilir)
    if (decodedToken.PermissionNames) {
      console.log('PermissionNames içeriği türü:', typeof decodedToken.PermissionNames);
      
      if (typeof decodedToken.PermissionNames === 'string') {
        permissions.push(decodedToken.PermissionNames);
        console.log('PermissionNames string olarak eklendi:', decodedToken.PermissionNames);
      }
      else if (Array.isArray(decodedToken.PermissionNames)) {
        permissions.push(...decodedToken.PermissionNames);
        console.log('PermissionNames dizi olarak eklendi:', decodedToken.PermissionNames);
      } else {
        console.log('PermissionNames claim var ama tanınmayan formatta:', typeof decodedToken.PermissionNames);
        
        // Özel bir objeyse, içeriğini kontrol et
        try {
          const permObj = JSON.parse(JSON.stringify(decodedToken.PermissionNames));
          console.log('PermissionNames objesi içeriği:', permObj);
          
          // Eğer objeyse ve $values gibi bir property varsa, bu koleksiyon olabilir
          if (permObj && permObj.$values && Array.isArray(permObj.$values)) {
            console.log('PermissionNames $values dizisi bulundu, ekleniyor:', permObj.$values);
            permissions.push(...permObj.$values);
          }
        } catch (error) {
          console.error('PermissionNames içeriği parse edilemedi:', error);
        }
      }
    }
    
    // permissions içeriğini kontrol et
    console.log('Çıkarılan izinler (temizleme öncesi):', permissions);
    
    // Tekrarlayan izinleri temizle
    const uniquePermissions = [...new Set(permissions)];
    console.log('Çıkarılan benzersiz izinler:', uniquePermissions);
    
    return uniquePermissions;
  }

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, loginRequest)
      .pipe(
        tap(response => {
          if (response.success) {
            localStorage.setItem('token', response.token);
            
            // Token'dan kullanıcı bilgilerini al
            const decodedToken = this.jwtHelper.decodeToken(response.token);
            const user: User = {
              id: parseInt(decodedToken.nameid),
              username: decodedToken.unique_name,
              sicil: decodedToken.sicil || '',
              isAdmin: decodedToken.role === 'Admin',
              createdAt: new Date().toISOString(),
              lastLoginAt: new Date().toISOString()
            };
            this.currentUserSubject.next(user);
            
            // İzinleri yükle
            this.permissions = this.extractPermissionsFromToken(decodedToken);
            
            // Kullanıcı rolüne göre yönlendirme
            const targetRoute = user.isAdmin ? '/admin-dashboard' : '/user-dashboard';
            console.log('Yönlendiriliyor:', targetRoute);
            this.router.navigate([targetRoute])
              .then(() => console.log('Yönlendirme başarılı'))
              .catch(err => console.error('Yönlendirme hatası:', err));
          }
        }),
        catchError(error => {
          console.error('Login error:', error);
          return throwError(() => error);
        })
      );
  }

  createUser(createUserRequest: CreateUserRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/create-user`, createUserRequest);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    return token ? !this.jwtHelper.isTokenExpired(token) : false;
  }

  isAdmin(): boolean {
    const currentUser = this.currentUserSubject.value;
    return currentUser ? currentUser.isAdmin : false;
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  changePassword(currentPassword: string, newPassword: string): Observable<any> {
    const requestUrl = `${this.apiUrl}/auth/change-password`;
    const requestBody = { currentPassword, newPassword };
    
    console.log('Şifre değiştirme isteği başlatılıyor...');
    console.log('İstek URL:', requestUrl);
    console.log('İstek metodu: POST');
    console.log('İstek gövdesi:', requestBody);
    
    const token = this.getToken();
    if (!token) {
      console.error('Token bulunamadı!');
      return throwError(() => new Error('Oturum bulunamadı'));
    }
    
    const headers = {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };
    console.log('İstek başlıkları:', headers);

    return this.http.post(requestUrl, requestBody, { headers }).pipe(
      tap(response => {
        console.log('Sunucu yanıtı:', response);
      }),
      catchError(error => {
        console.error('Şifre değiştirme hatası:', error);
        console.error('Hata detayları:', {
          status: error.status,
          statusText: error.statusText,
          error: error.error,
          url: error.url,
          headers: error.headers?.keys().map((key: string) => `${key}: ${error.headers?.get(key)}`)
        });
        if (error.status === 401) {
          console.log('Oturum sonlandı, çıkış yapılıyor...');
          this.logout();
        }
        return throwError(() => error);
      })
    );
  }

  hasPermission(permissionName: string): boolean {
    console.log(`İzin kontrolü: '${permissionName}'`);
    
    // Admin her zaman tüm izinlere sahiptir
    if (this.isAdmin()) {
      console.log('Kullanıcı admin, tüm izinlere sahip');
      return true;
    }
    
    // Token'dan izinleri kontrol et
    const hasPermission = this.permissions.includes(permissionName);
    console.log(`İzin '${permissionName}' kontrolü sonucu:`, hasPermission ? 'İzin var' : 'İzin yok');
    console.log('Mevcut izinler:', this.permissions);
    
    return hasPermission;
  }
} 