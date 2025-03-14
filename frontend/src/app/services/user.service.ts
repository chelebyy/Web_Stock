import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from '../shared/models/user.model';
import { CreateUserRequest } from '../core/authentication/auth.model';
import { environment } from '../../environments/environment';
import { AuthService } from '../core/authentication/auth.service';

// HTTP durum kodları için sabitler
export const HttpStatusCodes = {
  OK: 200,
  BAD_REQUEST: 400,
  UNAUTHORIZED: 401,
  FORBIDDEN: 403,
  NOT_FOUND: 404,
  SERVER_ERROR: 500,
  CONNECTION_ERROR: 0
};

// Hata kodları için sabitler
export const ErrorCodes = {
  DUPLICATE_SICIL: 'DuplicateSicil',
  DUPLICATE_USERNAME: 'DuplicateUsername'
};

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
    
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getUsers(): Observable<User[]> {
    const options = { headers: this.getHeaders() };
    
    return this.http.get<any>(`${this.apiUrl}/Users`, options)
      .pipe(
        map(response => {
          // ReferenceHandler.Preserve formatını işle ($id ve $values)
          if (response && response.$values && Array.isArray(response.$values)) {
            return response.$values as User[];
          }
          
          // Normal dizi yanıtını işle
          if (Array.isArray(response) && response.length > 0) {
            return response as User[];
          } else if (Array.isArray(response) && response.length === 0) {
            return [] as User[];
          } else {
            // Diğer olası formatlara bak
            if (response && typeof response === 'object') {
              // value ya da Value içinde olabilir
              if (response.value && Array.isArray(response.value)) {
                return response.value as User[];
              }
              
              if (response.Value && Array.isArray(response.Value)) {
                return response.Value as User[];
              }
              
              // Yanıt doğrudan bir nesne ise ve kullanıcı gibi görünüyorsa
              if ('username' in response || 'id' in response) {
                return [response as User];
              }
            }
            
            return [] as User[];
          }
        }),
        catchError(error => {
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
    
    const options = { headers: this.getHeaders() };
    return this.http.post<any>(`${this.apiUrl}/auth/create-user`, createUserRequest, options)
      .pipe(
        catchError(error => {
          // Hata mesajını düzenle
          let errorMessage = 'Kullanıcı oluşturulurken bir hata oluştu';
          let errorField = '';
          
          if (error.error) {
            // API'den gelen hata mesajını kullan
            if (error.error.error) {
              errorMessage = error.error.error;
            }
            
            // Alan bilgisi varsa kaydet
            if (error.error.field) {
              errorField = error.error.field;
            }
            
            // Sicil numarası çakışma hatası için özel mesaj
            if (error.error.code === ErrorCodes.DUPLICATE_SICIL) {
              errorMessage = `Sicil numarası '${user.sicil}' zaten kullanımda. Lütfen farklı bir sicil numarası girin.`;
              errorField = 'sicil';
            }
          }
          
          // HTTP durum kodu kontrolü
          if (error.status === HttpStatusCodes.UNAUTHORIZED) {
            errorMessage = 'Oturum süresi dolmuş veya yetkiniz yok. Lütfen tekrar giriş yapın.';
          } else if (error.status === HttpStatusCodes.FORBIDDEN) {
            errorMessage = 'Bu işlemi gerçekleştirmek için yetkiniz yok.';
          } else if (error.status === HttpStatusCodes.NOT_FOUND) {
            errorMessage = 'İstek yapılan kaynak bulunamadı.';
          }
          
          // Özel hata nesnesi döndür
          const customError = new Error(errorMessage);
          (customError as any).field = errorField;
          
          // Observable olarak hata mesajını döndür
          return throwError(() => customError);
        })
      );
  }

  updateUser(id: number, user: User): Observable<void> {
    const options = { headers: this.getHeaders() };
    return this.http.put<void>(`${this.apiUrl}/Users/${id}`, user, options);
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
