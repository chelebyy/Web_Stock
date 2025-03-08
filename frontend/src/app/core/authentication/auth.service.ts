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
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      if (decodedToken) {
        const user: User = {
          id: parseInt(decodedToken.nameid),
          username: decodedToken.name,
          sicil: decodedToken.sicil || '',
          isAdmin: decodedToken.role === 'Admin',
          createdAt: decodedToken.created_at || '',
          lastLoginAt: decodedToken.last_login_at
        };
        this.currentUserSubject.next(user);
        
        // İzinleri yükle
        this.permissions = this.extractPermissionsFromToken(decodedToken);
      }
    }
  }

  // Token'dan izinleri çıkartır
  private extractPermissionsFromToken(decodedToken: any): string[] {
    const permissions: string[] = [];
    
    // Permission claim'lerini bul
    if (decodedToken.Permission) {
      if (Array.isArray(decodedToken.Permission)) {
        // Birden fazla izin varsa
        permissions.push(...decodedToken.Permission);
      } else {
        // Tek bir izin varsa
        permissions.push(decodedToken.Permission);
      }
    }
    
    return permissions;
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
    // Admin her zaman tüm izinlere sahiptir
    if (this.isAdmin()) {
      return true;
    }
    
    // Token'dan izinleri kontrol et
    return this.permissions.includes(permissionName);
  }
} 