import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, catchError, throwError } from 'rxjs';
import { LoginRequest, LoginResponse, User, CreateUserRequest } from '../models/auth.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/api`;
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  private jwtHelper = new JwtHelperService();

  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    console.log('AuthService başlatıldı');
    this.loadStoredUser();
  }

  private loadStoredUser(): void {
    console.log('loadStoredUser çalıştı');
    const token = this.getToken();
    console.log('Mevcut token:', token ? 'Var' : 'Yok');
    
    if (token) {
      try {
        const decodedToken = this.jwtHelper.decodeToken(token);
        console.log('Token decode edildi:', decodedToken);
        
        if (decodedToken) {
          const user: User = {
            id: parseInt(decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']),
            username: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
            isAdmin: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Admin',
            createdAt: decodedToken.created_at || new Date().toISOString(),
            lastLoginAt: decodedToken.last_login_at
          };
          console.log('Kullanıcı oluşturuldu:', user);
          this.currentUserSubject.next(user);
        }
      } catch (error) {
        console.error('Token decode hatası:', error);
        this.logout();
      }
    }
  }

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    console.log('Login isteği başlatıldı:', loginRequest.username);
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, loginRequest)
      .pipe(
        tap(response => {
          console.log('Login başarılı, token kaydediliyor');
          localStorage.setItem('token', response.token);
          this.currentUserSubject.next(response.user);
        }),
        catchError(error => {
          console.error('Login hatası:', error);
          return throwError(() => error);
        })
      );
  }

  createUser(createUserRequest: CreateUserRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/create-user`, createUserRequest);
  }

  logout(): void {
    console.log('Logout yapılıyor');
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    const token = localStorage.getItem('token');
    console.log('Token alındı:', token ? 'Var' : 'Yok');
    return token;
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) {
      console.log('Token yok, kullanıcı giriş yapmamış');
      return false;
    }
    
    try {
      const decodedToken = this.jwtHelper.decodeToken(token);
      console.log('Token içeriği:', decodedToken);
      
      // Token'ın exp claim'ini kontrol et
      const exp = decodedToken.exp;
      if (!exp) {
        console.log('Token son kullanma tarihi bulunamadı');
        return false;
      }

      // exp, Unix timestamp olarak gelir (saniye cinsinden)
      const expirationDate = new Date(exp * 1000);
      const currentTime = new Date();
      
      // Sistem saati sorunu için UTC kullan
      const utcExpiration = new Date(expirationDate.toUTCString());
      const utcNow = new Date(currentTime.toUTCString());
      
      console.log('Token son kullanma tarihi (UTC):', utcExpiration);
      console.log('Şu anki zaman (UTC):', utcNow);
      
      const isExpired = utcNow.getTime() > utcExpiration.getTime();
      console.log('Token süresi dolmuş mu:', isExpired);
      
      return !isExpired;
    } catch (error) {
      console.error('Token kontrolü sırasında hata:', error);
      return false;
    }
  }

  isAdmin(): boolean {
    const currentUser = this.currentUserSubject.value;
    const isAdmin = currentUser ? currentUser.isAdmin : false;
    console.log('Kullanıcı admin mi:', isAdmin);
    return isAdmin;
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  changePassword(currentPassword: string, newPassword: string): Observable<any> {
    console.log('Şifre değiştirme isteği başlatılıyor...');
    const token = this.getToken();
    if (!token) {
      console.error('Token bulunamadı!');
      return throwError(() => new Error('Oturum bulunamadı'));
    }
    console.log('Token mevcut, istek gönderiliyor...');

    return this.http.post(`${this.apiUrl}/auth/change-password`, {
      currentPassword,
      newPassword
    }).pipe(
      tap(response => {
        console.log('Şifre değiştirme başarılı:', response);
      }),
      catchError(error => {
        console.error('Şifre değiştirme hatası:', error);
        if (error.status === 401) {
          console.log('Oturum sonlandı, çıkış yapılıyor...');
          this.logout();
        }
        return throwError(() => error);
      })
    );
  }
}
