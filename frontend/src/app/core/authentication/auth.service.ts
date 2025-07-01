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
  private permissions: string[] = [];

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService
  ) {
    console.log('AuthService: Constructor çalıştı');
    this.loadStoredUser();
  }

  private loadStoredUser(): void {
    console.log('AuthService: loadStoredUser metodu çalıştı');
    const token = this.getToken();
    console.log('Token kontrolü:', token ? 'Token mevcut' : 'Token bulunamadı');
    
    if (!token) {
      console.log('Token bulunamadı, kullanıcı oturumu yok');
      this.currentUserSubject.next(null);
      this.permissions = [];
      return;
    }
    
    try {
      // Token süresi kontrolü
      if (this.jwtHelper.isTokenExpired(token)) {
        console.error('Token süresi dolmuş, token siliniyor');
        localStorage.removeItem('token');
        this.currentUserSubject.next(null);
        this.permissions = [];
        return;
      }
      
      const decodedToken = this.jwtHelper.decodeToken(token);
      console.log('Token çözümlendi:', decodedToken);
      
      if (!decodedToken) {
        console.error('Token çözümlenemedi, token siliniyor');
        localStorage.removeItem('token');
        this.currentUserSubject.next(null);
        this.permissions = [];
        return;
      }
      
      // Kullanıcı bilgilerini oluştur
      const user: User = {
        id: parseInt(decodedToken.nameid || decodedToken.sub || '0'),
        username: decodedToken.unique_name || decodedToken.name || 'Bilinmeyen Kullanıcı',
        sicil: decodedToken.sicil || '',
        isAdmin: decodedToken.role === 'Admin',
        createdAt: decodedToken.created_at || '',
        lastLoginAt: decodedToken.last_login_at || ''
      };
      
      console.log('Kullanıcı bilgileri:', user);
      this.currentUserSubject.next(user);

      // İzinleri yükle
      this.permissions = this.extractPermissionsFromToken(decodedToken);
      console.log('Kullanıcı izinleri (extract sonrası):', this.permissions);
      
      // Rol yönetimi izni kontrolü - admin kullanıcılar için otomatik ekle
      if (user.isAdmin && !this.permissions.includes('Pages.RoleManagement')) {
        console.log('Admin kullanıcı için rol yönetimi izni otomatik olarak ekleniyor (loadStoredUser)');
        this.permissions.push('Pages.RoleManagement');
      }
      console.log('Kullanıcı izinleri (loadStoredUser sonrası admin kontrolü):', this.permissions);
    } catch (error) {
      console.error('Token çözümleme hatası:', error);
      // Hata durumunda token'ı temizle
      localStorage.removeItem('token');
      this.currentUserSubject.next(null);
      this.permissions = [];
    }
  }

  // Token'dan izinleri çıkartır
  private extractPermissionsFromToken(decodedToken: any): string[] {
    const permissions: string[] = [];
    
    console.log('Token çözümlenmiş tam içerik:', JSON.stringify(decodedToken, null, 2));
    console.log('Token içindeki tüm claim anahtarları:', Object.keys(decodedToken));
    
    // Rol tabanlı izin kontrolü - Admin rolü için otomatik RoleManagement izni ekle
    if (decodedToken.role === 'Admin') {
      console.log('Admin rolü tespit edildi, Pages.RoleManagement izni ekleniyor');
      permissions.push('Pages.RoleManagement');
      permissions.push('Pages.AdminDashboard');
      permissions.push('Pages.UserManagement');
    }
    
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
    
    // Diğer olası izin claim'lerini kontrol et (bazı token yapılandırmalarında farklı isimler kullanılabilir)
    const possiblePermissionClaims = ['permissions', 'Permissions', 'claims', 'Claims', 'roles', 'Roles'];
    
    possiblePermissionClaims.forEach(claimName => {
      if (decodedToken[claimName]) {
        console.log(`${claimName} claim'i bulundu, içeriği:`, decodedToken[claimName]);
        
        if (typeof decodedToken[claimName] === 'string') {
          permissions.push(decodedToken[claimName]);
          console.log(`${claimName} string olarak eklendi:`, decodedToken[claimName]);
        }
        else if (Array.isArray(decodedToken[claimName])) {
          permissions.push(...decodedToken[claimName]);
          console.log(`${claimName} dizi olarak eklendi:`, decodedToken[claimName]);
        } else {
          try {
            const claimObj = JSON.parse(JSON.stringify(decodedToken[claimName]));
            if (claimObj && claimObj.$values && Array.isArray(claimObj.$values)) {
              permissions.push(...claimObj.$values);
              console.log(`${claimName} $values dizisi eklendi:`, claimObj.$values);
            }
          } catch (error) {
            console.error(`${claimName} içeriği parse edilemedi:`, error);
          }
        }
      }
    });
    
    // permissions içeriğini kontrol et
    console.log('Çıkarılan izinler (temizleme öncesi):', permissions);
    
    // Tekrarlayan izinleri temizle
    const uniquePermissions = [...new Set(permissions)];
    console.log('Çıkarılan benzersiz izinler (otomatik ekleme öncesi):', uniquePermissions);

    return uniquePermissions;
  }

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    console.log('AuthService: login metodu çalıştı');
    console.log('Login isteği:', loginRequest);
    
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, loginRequest)
      .pipe(
        tap(response => {
          console.log('Login yanıtı:', response);
          
          if (response.success && response.token) {
            localStorage.setItem('token', response.token);
            console.log('Token localStorage\'a kaydedildi');
            
            // Token'dan kullanıcı bilgilerini al
            const decodedToken = this.jwtHelper.decodeToken(response.token);
            console.log('Token çözümlendi:', decodedToken);
            
            const user: User = {
              id: parseInt(decodedToken.nameid || decodedToken.sub || '0'),
              username: decodedToken.unique_name || decodedToken.name || 'Bilinmeyen Kullanıcı',
              sicil: decodedToken.sicil || '',
              isAdmin: decodedToken.role === 'Admin',
              createdAt: new Date().toISOString(),
              lastLoginAt: new Date().toISOString()
            };
            console.log('Kullanıcı bilgileri:', user);
            
            this.currentUserSubject.next(user);
            
            // İzinleri yükle
            this.permissions = this.extractPermissionsFromToken(decodedToken);
            console.log('Kullanıcı izinleri (extract sonrası):', this.permissions);
            
            // Kullanıcı rolüne göre yönlendirme
            if (user.isAdmin && !this.permissions.includes('Pages.RoleManagement')) {
              console.log('Admin kullanıcı için rol yönetimi izni otomatik olarak ekleniyor (login)');
              this.permissions.push('Pages.RoleManagement');
            }
            console.log('Kullanıcı izinleri (login sonrası admin kontrolü):', this.permissions);

            const targetRoute = user.isAdmin ? '/app/dashboard/admin-dashboard' : '/app/dashboard/user-dashboard';
            console.log('Yönlendiriliyor:', targetRoute);
            this.router.navigate([targetRoute]);
          } else {
            console.warn('Response başarılı ancak token eksik veya geçersiz', response);
          }
        }),
        catchError(error => {
          console.error('Login hatası:', error);
          console.error('Hata detayları:', {
            status: error.status,
            statusText: error.statusText,
            error: error.error,
            url: error.url,
            type: error instanceof Error ? 'JS Error' : 'HTTP Error'
          });
          return throwError(() => error);
        })
      );
  }

  createUser(createUserRequest: CreateUserRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/create-user`, createUserRequest)
      .pipe(
        catchError(error => {
          console.error('Kullanıcı oluşturma hatası (AuthService):', error);
          
          // Hata mesajını düzenle
          let errorMessage = 'Kullanıcı oluşturulurken bir hata oluştu';
          let errorField = '';
          let errorCode = '';
          
          if (error.error) {
            // API'den gelen hata mesajını kullan
            if (error.error.error) {
              errorMessage = error.error.error;
            }
            
            // Alan bilgisi varsa kaydet
            if (error.error.field) {
              errorField = error.error.field;
            }
            
            // Hata kodu varsa kaydet
            if (error.error.code) {
              errorCode = error.error.code;
            }
            
            // Sicil numarası çakışma hatası için özel mesaj
            if (error.error.code === 'DuplicateSicil') {
              errorMessage = `Sicil numarası '${createUserRequest.sicil}' zaten kullanımda. Lütfen farklı bir sicil numarası girin.`;
              errorField = 'sicil';
            }
            
            // Kullanıcı adı çakışma hatası için özel mesaj
            if (error.error.code === 'DuplicateUsername' || 
                (error.error.error && error.error.error.includes('duplicate key')) ||
                (error.error.details && error.error.details.includes('IX_Users_Username'))) {
              errorMessage = `Kullanıcı adı '${createUserRequest.username}' zaten kullanımda. Lütfen farklı bir kullanıcı adı girin.`;
              errorField = 'username';
              errorCode = 'DuplicateUsername';
            }
          }
          
          // HTTP durum kodu kontrolü
          if (error.status === 401) {
            errorMessage = 'Oturum süresi dolmuş veya yetkiniz yok. Lütfen tekrar giriş yapın.';
          } else if (error.status === 403) {
            errorMessage = 'Bu işlemi gerçekleştirmek için yetkiniz yok.';
          } else if (error.status === 404) {
            errorMessage = 'İstek yapılan kaynak bulunamadı.';
          } else if (error.status === 500) {
            // Sunucu hatası için daha detaylı bilgi
            if (error.error && error.error.details) {
              errorMessage = `Sunucu hatası: ${error.error.details}`;
              
              // Veritabanı benzersizlik hatası kontrolü
              if (error.error.details.includes('duplicate key') || 
                  error.error.details.includes('IX_Users_Username')) {
                errorMessage = `Kullanıcı adı '${createUserRequest.username}' zaten kullanımda. Lütfen farklı bir kullanıcı adı girin.`;
                errorField = 'username';
                errorCode = 'DuplicateUsername';
              }
            } else {
              errorMessage = 'Sunucu tarafında bir hata oluştu. Lütfen daha sonra tekrar deneyin.';
            }
          }
          
          // Özel hata nesnesi döndür
          const customError = new Error(errorMessage);
          (customError as any).field = errorField;
          (customError as any).code = errorCode;
          
          // Observable olarak hata mesajını döndür
          return throwError(() => customError);
        })
      );
  }

  logout(): void {
    console.log('AuthService: logout metodu çalıştı');
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
    this.permissions = [];
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    const token = localStorage.getItem('token');
    console.log('AuthService: getToken metodu çalıştı, token:', token ? 'Token mevcut' : 'Token bulunamadı');
    return token;
  }

  isLoggedIn(): boolean {
    console.log('AuthService: isLoggedIn metodu çalıştı');
    const token = this.getToken();
    if (!token) {
      console.log('Token bulunamadı, kullanıcı giriş yapmamış');
      return false;
    }
    
    try {
      const isExpired = this.jwtHelper.isTokenExpired(token);
      console.log('Token süresi dolmuş mu:', isExpired ? 'Evet' : 'Hayır');
      
      if (isExpired) {
        console.log('Token süresi dolmuş, oturum kapatılıyor (isLoggedIn)');
        this.logout();
        return false;
      }
      
      // Kullanıcı bilgilerini kontrol et
      const currentUser = this.currentUserSubject.value;
      if (!currentUser) {
        console.log('Kullanıcı bilgileri bulunamadı, token yeniden yükleniyor');
        this.loadStoredUser();
        return this.currentUserSubject.value !== null;
      }
      
      console.log('Kullanıcı giriş yapmış');
      return true;
    } catch (error) {
      console.error('Token kontrolünde HATA (isLoggedIn):', error);
      console.log('Token kontrol hatası nedeniyle logout çağrılıyor (isLoggedIn)...');
      this.logout();
      return false;
    }
  }

  isAdmin(): boolean {
    console.log('AuthService: isAdmin metodu çalıştı');
    const currentUser = this.currentUserSubject.value;
    const isAdmin = currentUser ? currentUser.isAdmin : false;
    console.log('isAdmin kontrolü:', isAdmin ? 'Kullanıcı admin' : 'Kullanıcı admin değil');
    
    // Kullanıcı bilgileri yoksa ve token varsa, token'dan kullanıcı bilgilerini yükle
    if (!currentUser && this.getToken()) {
      console.log('Kullanıcı bilgileri bulunamadı ama token var, token yeniden yükleniyor');
      this.loadStoredUser();
      const updatedUser = this.currentUserSubject.value;
      const updatedIsAdmin = updatedUser ? updatedUser.isAdmin : false;
      console.log('Güncellenmiş isAdmin kontrolü:', updatedIsAdmin ? 'Kullanıcı admin' : 'Kullanıcı admin değil');
      return updatedIsAdmin;
    }
    
    return isAdmin;
  }

  getCurrentUser(): User | null {
    console.log('AuthService: getCurrentUser metodu çalıştı');
    const currentUser = this.currentUserSubject.value;
    console.log('Mevcut kullanıcı:', currentUser);
    
    // Kullanıcı bilgileri yoksa ve token varsa, token'dan kullanıcı bilgilerini yükle
    if (!currentUser && this.getToken()) {
      console.log('Kullanıcı bilgileri bulunamadı ama token var, token yeniden yükleniyor');
      this.loadStoredUser();
      const updatedUser = this.currentUserSubject.value;
      console.log('Güncellenmiş kullanıcı bilgileri:', updatedUser);
      return updatedUser;
    }
    
    return currentUser;
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
    console.log(`AuthService: hasPermission metodu çalıştı, izin: '${permissionName}'`);
    // Admin kullanıcısı tüm izinlere sahip olmalı varsayımını kaldır.
    // İzinler extractPermissionsFromToken içinde zaten role göre atanıyor.
    // if (this.isAdmin()) {
    //   console.log('Kullanıcı admin, tüm izinlere sahip');
    //   return true;
    // }

    // Kullanıcının bu izne sahip olup olmadığını kontrol et
    const hasPerm = this.permissions.includes(permissionName);
    console.log(`İzin '${permissionName}' kontrolü sonucu:`, hasPerm ? 'İzin var' : 'İzin yok');
    console.log('Mevcut izinler:', this.permissions);
    return hasPerm;
  }
} 