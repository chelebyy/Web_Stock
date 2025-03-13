import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from '../../../shared/models/user.model';
import { CreateUserRequest } from '../../../core/authentication/auth.model';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../../core/authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/api`;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  // HTTP Headers oluştur
  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    console.log('Kullanılan token:', token ? `${token.substring(0, 15)}...` : 'Token yok');
    
    if (!token) {
      console.warn('Auth token bulunamadı! Lütfen tekrar giriş yapın.');
    }
    
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getUsers(): Observable<User[]> {
    console.log(`API URL: ${this.apiUrl}/Users`);
    const options = { headers: this.getHeaders() };
    
    console.log('API isteği gönderiliyor...', {
      url: `${this.apiUrl}/Users`,
      headers: {
        contentType: options.headers.get('Content-Type'),
        authHeader: options.headers.has('Authorization') ? 'Bearer token mevcut' : 'Authorization header yok'
      }
    });
    
    return this.http.get<any>(`${this.apiUrl}/Users`, options)
      .pipe(
        map(response => {
          console.log('API yanıtı alındı:', response);
          
          // ReferenceHandler.Preserve formatını işle ($id ve $values)
          if (response && response.$values && Array.isArray(response.$values)) {
            console.log(`$values dizisinden ${response.$values.length} kullanıcı yüklendi`);
            return response.$values as User[];
          }
          
          // Normal dizi yanıtını işle
          if (Array.isArray(response) && response.length > 0) {
            console.log(`${response.length} kullanıcı başarıyla yüklendi`);
            return response as User[];
          } else if (Array.isArray(response) && response.length === 0) {
            console.warn('API yanıtı boş bir dizi döndürdü (hiç kullanıcı yok)');
            return [] as User[];
          } else {
            console.warn('API yanıtı beklenen formatta değil:', response);
            
            // Diğer olası formatlara bak
            if (response && typeof response === 'object') {
              // value ya da Value içinde olabilir
              if (response.value && Array.isArray(response.value)) {
                console.log('value dizisi bulundu, kullanılıyor');
                return response.value as User[];
              }
              
              if (response.Value && Array.isArray(response.Value)) {
                console.log('Value dizisi bulundu, kullanılıyor');
                return response.Value as User[];
              }
              
              // Yanıt doğrudan bir nesne ise ve kullanıcı gibi görünüyorsa
              if ('username' in response || 'id' in response) {
                console.log('Tek kullanıcı nesnesi bulundu, dizi haline getiriliyor');
                return [response as User];
              }
            }
            
            console.error('API yanıtı beklendiği gibi değil, dizi dönüştürülemedi');
            return [] as User[];
          }
        }),
        catchError(error => {
          console.error('Kullanıcıları getirirken hata oluştu:', error);
          console.error('Hata detayları:', {
            status: error.status,
            statusText: error.statusText,
            message: error.message,
            error: error.error
          });
          
          if (error.status === 401) {
            console.error('Yetkilendirme hatası (401). Token geçersiz veya eksik olabilir.');
          } else if (error.status === 404) {
            console.error('API endpoint bulunamadı (404). URL doğru mu?');
          } else if (error.status === 0) {
            console.error('Bağlantı hatası. Backend çalışıyor mu?');
          }
          
          // Boş dizi döndür ki uygulama çökmeden devam edebilsin
          return of([] as User[]);
        })
      );
  }

  getUser(id: number): Observable<User> {
    const options = { headers: this.getHeaders() };
    return this.http.get<User>(`${this.apiUrl}/Users/${id}`, options);
  }

  getUserById(id: number): Observable<User> {
    return this.getUser(id).pipe(
      map(user => {
        // Backend'den gelen User nesnesinde firstName ve lastName alanları olmayabilir
        // Kullanıcı adını parçalayarak bu alanları dolduralım
        if (user && !user.firstName && !user.lastName && user.username) {
          const nameParts = user.username.split(' ');
          if (nameParts.length >= 2) {
            user.firstName = nameParts[0];
            user.lastName = nameParts.slice(1).join(' ');
          } else {
            user.firstName = user.username;
            user.lastName = '';
          }
        }
        return user;
      })
    );
  }

  createUser(user: User): Observable<any> {
    // Gönderilecek veriyi hazırla
    const createUserRequest: CreateUserRequest = {
      username: user.username,
      password: user.password || user.passwordHash || '', // Önce password, yoksa passwordHash kullan
      sicil: user.sicil,
      isAdmin: user.isAdmin,
      roleId: user.roleId // Rol ID'si eklendi
    };
    
    // Veriyi göndermeden önce kontrol et
    // Temel doğrulamalar
    if (!createUserRequest.username || createUserRequest.username.trim() === '') {
      return throwError(() => new Error('Kullanıcı adı boş olamaz'));
    }
    
    if (!createUserRequest.password || createUserRequest.password.trim() === '') {
      return throwError(() => new Error('Şifre boş olamaz'));
    }
    
    if (!createUserRequest.sicil || createUserRequest.sicil.trim() === '') {
      return throwError(() => new Error('Sicil numarası boş olamaz'));
    }
    
    if (!createUserRequest.roleId || createUserRequest.roleId <= 0) {
      return throwError(() => new Error('Geçerli bir rol seçilmelidir'));
    }
    
    // Kontrol amaçlı log
    console.log('Gönderilen kullanıcı verisi:', createUserRequest);
    
    const options = { headers: this.getHeaders() };
    return this.http.post<any>(`${this.apiUrl}/auth/create-user`, createUserRequest, options)
      .pipe(
        catchError(error => {
          console.error('Kullanıcı oluşturma hatası:', error);
          
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
              errorMessage = `Sicil numarası '${user.sicil}' zaten kullanımda. Lütfen farklı bir sicil numarası girin.`;
              errorField = 'sicil';
            }
            
            // Kullanıcı adı çakışma hatası için özel mesaj
            if (error.error.code === 'DuplicateUsername' || 
                (error.error.error && error.error.error.includes('duplicate key')) ||
                (error.error.details && error.error.details.includes('IX_Users_Username'))) {
              errorMessage = `Kullanıcı adı '${user.username}' zaten kullanımda. Lütfen farklı bir kullanıcı adı girin.`;
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
                errorMessage = `Kullanıcı adı '${user.username}' zaten kullanımda. Lütfen farklı bir kullanıcı adı girin.`;
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

  updateUser(id: number, user: User): Observable<void> {
    const options = { headers: this.getHeaders() };
    
    // Kontrol amaçlı log
    console.log('Güncellenecek kullanıcı verisi:', user);
    
    return this.http.put<void>(`${this.apiUrl}/Users/${id}`, user, options)
      .pipe(
        catchError(error => {
          console.error('Kullanıcı güncelleme hatası:', error);
          
          // Hata mesajını düzenle
          let errorMessage = 'Kullanıcı güncellenirken bir hata oluştu';
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
              errorMessage = `Sicil numarası '${user.sicil}' zaten kullanımda. Lütfen farklı bir sicil numarası girin.`;
              errorField = 'sicil';
            }
            
            // Kullanıcı adı çakışma hatası için özel mesaj
            if (error.error.code === 'DuplicateUsername' || 
                (error.error.error && error.error.error.includes('duplicate key')) ||
                (error.error.details && error.error.details.includes('IX_Users_Username'))) {
              errorMessage = `Kullanıcı adı '${user.username}' zaten kullanımda. Lütfen farklı bir kullanıcı adı girin.`;
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
                errorMessage = `Kullanıcı adı '${user.username}' zaten kullanımda. Lütfen farklı bir kullanıcı adı girin.`;
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

  deleteUser(id: number): Observable<void> {
    const options = { headers: this.getHeaders() };
    return this.http.delete<void>(`${this.apiUrl}/Users/${id}`, options);
  }

  /**
   * Kullanıcının profil resmini getirir
   * @param userId Kullanıcı ID
   * @returns Profil resmi blob verisi
   */
  getUserProfilePicture(userId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/users/${userId}/profile-picture`, {
      responseType: 'blob'
    });
  }

  /**
   * Kullanıcının profil resmini günceller
   * @param userId Kullanıcı ID
   * @param file Profil resmi dosyası
   * @returns API yanıtı
   */
  updateProfilePicture(userId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    
    return this.http.post(`${this.apiUrl}/users/${userId}/profile-picture`, formData);
  }
}
