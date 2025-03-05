import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from '../models/user.model';
import { CreateUserRequest } from '../models/auth.model';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

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

  createUser(user: User): Observable<any> {
    const createUserRequest: CreateUserRequest = {
      username: user.username,
      password: user.password || user.passwordHash || '', // Önce password, yoksa passwordHash kullan
      sicil: user.sicil,
      isAdmin: user.isAdmin
    };
    
    // Kontrol amaçlı log
    console.log('Gönderilen kullanıcı verisi:', createUserRequest);
    
    const options = { headers: this.getHeaders() };
    return this.http.post<any>(`${this.apiUrl}/auth/create-user`, createUserRequest, options);
  }

  updateUser(id: number, user: User): Observable<void> {
    const options = { headers: this.getHeaders() };
    return this.http.put<void>(`${this.apiUrl}/Users/${id}`, user, options);
  }

  deleteUser(id: number): Observable<void> {
    const options = { headers: this.getHeaders() };
    return this.http.delete<void>(`${this.apiUrl}/Users/${id}`, options);
  }
}
