import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from '../shared/models/user.model';
import { CreateUserRequest } from '../core/authentication/auth.model';
import { AuthService } from '../core/authentication/auth.service';
import { BaseHttpService } from '../core/services/base-http.service';

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
export class UserService extends BaseHttpService {
  private usersEndpoint = '/api/Users';
  private authEndpoint = '/api/auth';

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }

  getUsers(): Observable<User[]> {
    return this.get<User[]>(this.usersEndpoint).pipe(
      catchError(error => {
        // Boş dizi döndür ki uygulama çökmeden devam edebilsin
        return of([] as User[]);
      })
    );
  }

  getUser(id: number): Observable<User> {
    return this.get<User>(`${this.usersEndpoint}/${id}`);
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
    
    return this.post<any>(`${this.authEndpoint}/create-user`, createUserRequest)
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
    return this.put<void>(`${this.usersEndpoint}/${id}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this.delete<void>(`${this.usersEndpoint}/${id}`);
  }

  /**
   * Kullanıcının profil resmini getirir
   * @param userId Kullanıcı ID
   * @returns Profil resmi blob verisi
   */
  getUserProfilePicture(userId: number): Observable<Blob> {
    return this.downloadFile(`${this.usersEndpoint}/${userId}/profile-picture`);
  }

  /**
   * Kullanıcının profil resmini günceller
   * @param userId Kullanıcı ID
   * @param file Profil resmi dosyası
   * @returns API yanıtı
   */
  updateProfilePicture(userId: number, file: File): Observable<any> {
    return this.uploadFile<any>(`${this.usersEndpoint}/${userId}/profile-picture`, file);
  }
}
