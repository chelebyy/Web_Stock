import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from '../../../shared/models/user.model';
import { CreateUserRequest } from '../../../core/authentication/auth.model';
import { environment } from '../../../../environments/environment';
import { AuthService } from '../../../core/authentication/auth.service';
import { BaseHttpService } from '../../../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseHttpService<User> {
  private readonly apiRoute = 'api/Users';
  private readonly authRoute = 'api/Auth';

  constructor(
    http: HttpClient,
    authService: AuthService
  ) {
    super(http, authService, environment.apiUrl);
  }

  getUsers(): Observable<User[]> {
    console.log(`UserService: getUsers çağrıldı`);
    return super.get<User[]>(this.apiRoute);
  }

  getById(id: number): Observable<User> {
    console.log(`UserService: getUserById çağrıldı, id: ${id}`);
    return super.get<User>(`${this.apiRoute}/${id}`);
  }

  createUser(user: User): Observable<User> {
    const createUserRequest: CreateUserRequest = {
      username: user.username,
      password: user.password || user.passwordHash || '',
      sicil: user.sicil,
      adi: user.adi,
      soyadi: user.soyadi,
      isAdmin: user.isAdmin,
      roleId: user.roleId
    };
    
    if (!createUserRequest.username?.trim()) {
      return throwError(() => new Error('Kullanıcı adı boş olamaz'));
    }
    
    if (!createUserRequest.sicil?.trim()) {
      return throwError(() => new Error('Sicil numarası boş olamaz'));
    }
    
    if (!createUserRequest.password?.trim()) {
      return throwError(() => new Error('Şifre boş olamaz'));
    }
    
    if (!createUserRequest.roleId || createUserRequest.roleId <= 0) {
      return throwError(() => new Error('Geçerli bir rol seçilmelidir'));
    }
    
    console.log('UserService: createUser gönderilen veri:', createUserRequest);
    
    const createEndpoint = `${this.authRoute}/create-user`;
    return super.post<User>(createEndpoint, createUserRequest);
  }

  updateUser(id: number, user: Partial<User>): Observable<void> {
    console.log(`UserService: updateUser çağrıldı, id: ${id}, veri:`, user);
    const { password, passwordHash, ...userData } = user;
    return super.put<void>(`${this.apiRoute}/${id}`, userData);
  }

  deleteUser(id: number): Observable<void> {
    console.log(`UserService: deleteUser çağrıldı, id: ${id}`);
    return super.delete<void>(`${this.apiRoute}/${id}`);
  }

  getUserProfilePicture(userId: number): Observable<Blob> {
    const options = { headers: super.getHeaders(), responseType: 'blob' as 'json' };
    return this.http.get<Blob>(`${environment.apiUrl}/${this.apiRoute}/${userId}/profile-picture`, options)
      .pipe(
        // catchError(this.handleError<Blob>(`getUserProfilePicture id=${userId}`, new Blob())) // Kaldırıldı
      );
  }

  updateProfilePicture(userId: number, file: File): Observable<any> {
    console.log(`UserService: updateProfilePicture çağrıldı, userId: ${userId}`);
    const path = `${this.apiRoute}/${userId}/profile-picture`;
    return super.upload<any>(path, file);
  }
}
