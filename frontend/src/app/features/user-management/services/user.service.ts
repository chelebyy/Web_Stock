import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from '../../../shared/models/user.model';
import { CreateUserRequest } from '../../../core/authentication/auth.model';
import { AuthService } from '../../../core/authentication/auth.service';
import { BaseHttpService } from '../../../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseHttpService {
  private endpoint = '/api/Users';
  private authEndpoint = '/api/auth';

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }

  getUsers(): Observable<User[]> {
    console.log(`API URL: ${this.apiUrl}${this.endpoint}`);
    
    return this.get<User[]>(this.endpoint)
      .pipe(
        catchError(error => {
          console.error('Kullanıcıları getirirken hata oluştu:', error);
          // Boş dizi döndür ki uygulama çökmeden devam edebilsin
          return of([] as User[]);
        })
      );
  }

  getUser(id: number): Observable<User> {
    return this.get<User>(`${this.endpoint}/${id}`);
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
    
    return this.post<any>(`${this.authEndpoint}/create-user`, createUserRequest);
  }

  updateUser(id: number, user: User): Observable<void> {
    return this.put<void>(`${this.endpoint}/${id}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this.delete<void>(`${this.endpoint}/${id}`);
  }

  getUserProfilePicture(userId: number): Observable<Blob> {
    return this.downloadFile(`${this.endpoint}/${userId}/profile-picture`);
  }

  updateProfilePicture(userId: number, file: File): Observable<any> {
    return this.uploadFile<any>(`${this.endpoint}/${userId}/profile-picture`, file);
  }
}
