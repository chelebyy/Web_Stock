import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../shared/models/user.model';
import { CreateUserRequest } from '../core/authentication/auth.model';
import { environment } from '../../environments/environment';
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
export class UserService extends BaseHttpService<User> {
  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService, `${environment.apiUrl}/api/Users`);
  }

  getUsers(): Observable<User[]> {
    return this.get<User[]>('');
  }

  getUser(id: number): Observable<User> {
    return this.get<User>(`${id}`);
  }

  getUserById(id: number): Observable<User> {
    return this.getUser(id).pipe(
      map(user => {
        // Backend'den gelen User nesnesinde adi ve soyadi alanları kontrol et
        // Yoksa alternatif alanlardan oluştur
        if (user) {
          if (!user.adi && !user.soyadi) {
            if (user.firstName && user.lastName) {
              user.adi = user.firstName;
              user.soyadi = user.lastName;
            } else if (user.username) {
              const nameParts = user.username.split(' ');
              if (nameParts.length >= 2) {
                user.adi = nameParts[0];
                user.soyadi = nameParts.slice(1).join(' ');
              } else {
                user.adi = user.username;
                user.soyadi = '';
              }
            }
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
      adi: user.adi,
      soyadi: user.soyadi,
      isAdmin: user.isAdmin,
      roleId: user.roleId
    };
    
    // Temel doğrulamalar
    if (!createUserRequest.sicil || createUserRequest.sicil.trim() === '') {
      return throwError(() => new Error('Sicil numarası boş olamaz'));
    }
    
    if (!createUserRequest.password || createUserRequest.password.trim() === '') {
      return throwError(() => new Error('Şifre boş olamaz'));
    }
    
    return this.post<any>('auth/create-user', createUserRequest);
  }

  updateUser(id: number, user: User): Observable<void> {
    return this.put<void>(`${id}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this.delete<void>(`${id}`);
  }

  /**
   * Kullanıcının profil resmini getirir
   * @param userId Kullanıcı ID
   * @returns Profil resmi blob verisi
   */
  getProfilePicture(userId: number): Observable<Blob> {
    return this.get<Blob>(`${userId}/profile-picture`, undefined);
  }

  /**
   * Kullanıcının profil resmini günceller
   * @param userId Kullanıcı ID
   * @param file Yeni profil resmi
   * @returns Güncelleme sonucu
   */
  updateProfilePicture(userId: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    
    return this.post<any>(`${userId}/profile-picture`, formData);
  }
}
