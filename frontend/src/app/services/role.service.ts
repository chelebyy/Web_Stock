import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, map, throwError } from 'rxjs';
import { Role } from '../shared/models/role.model';
import { environment } from '../../environments/environment';
import { AuthService } from '../core/authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private apiUrl = `${environment.apiUrl}/api/Role`;

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

  // Hata işleme
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Bilinmeyen bir hata oluştu';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side hata
      errorMessage = `Hata: ${error.error.message}`;
    } else {
      // Backend hatası
      errorMessage = `Sunucu hatası: ${error.status}, Mesaj: ${error.message}`;
      
      // Sunucudan gelen hata mesajını kontrol et
      if (error.error && typeof error.error === 'string') {
        errorMessage = error.error;
      }
    }
    
    console.error('Rol servisi hatası:', errorMessage);
    return throwError(() => error);
  }

  // API yanıtını normalize et - ReferenceHandler.Preserve uyumluluğu için
  private normalizeResponse<T>(response: any): T {
    if (!response) return [] as unknown as T;
    
    // $values dizisi varsa
    if (response.$values) {
      console.log('$values dizisi bulundu:', response.$values.length, 'öğe var');
      return response.$values as T;
    }
    // Dizi ise doğrudan döndür
    else if (Array.isArray(response)) {
      console.log('Dizi yanıtı bulundu:', response.length, 'öğe var');
      return response as T;
    }
    // Başka özelliklerde veri varsa
    else if (response && typeof response === 'object') {
      // Nesne içinde dizi özelliği ara
      if (response.value && Array.isArray(response.value)) {
        console.log('value dizisi bulundu:', response.value.length, 'öğe var');
        return response.value as T;
      }
      
      if (response.Value && Array.isArray(response.Value)) {
        console.log('Value dizisi bulundu:', response.Value.length, 'öğe var');
        return response.Value as T;
      }
      
      // ID değeri varsa ve tek bir nesne ise
      if (response.id || response.Id) {
        console.log('Tek nesne bulundu (id var)');
        return response as T;
      }
    }
    
    // Son çare: orijinal yanıtı döndür
    return response as T;
  }

  getRoles(): Observable<Role[]> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(this.apiUrl, options)
      .pipe(
        map(response => this.normalizeResponse<Role[]>(response)),
        catchError(this.handleError)
      );
  }

  getRole(id: number): Observable<Role> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(`${this.apiUrl}/${id}`, options)
      .pipe(
        map(response => this.normalizeResponse<Role>(response)),
        catchError(this.handleError)
      );
  }

  getRoleById(id: number): Observable<Role> {
    return this.http.get<any>(`${this.apiUrl}/${id}`)
      .pipe(
        map(response => {
          console.log(`API rol yanıtı, ID ${id}:`, response);
          
          // API yanıtını temizle ve tek bir rol nesnesine dönüştür
          let role = this.normalizeResponse<Role>(response);
          
          if (!role || (typeof role === 'object' && Object.keys(role).length === 0)) {
            throw new Error(`ID ${id} ile rol bulunamadı`);
          }
          
          // Daha detaylı log ile rol adını inceleyelim
          if (role.name !== undefined) {
            console.log(`Rol adı: "${role.name}", tipi: ${typeof role.name}, uzunluk: ${role.name.length}`);
          } else {
            console.warn(`ID ${id} rolünün ismi tanımsız`);
          }
          
          // Veri geldi ama name alanı tanımsız veya boş string olabilir
          if (!role.name) {
            // Backend'den gelen API yanıtından rol adını bulmaya çalışalım
            if (response && typeof response === 'object') {
              // Yanıtta name alanı var mı kontrol et
              if (response.name) {
                role.name = response.name;
              }
              // Yanıtta Name alanı var mı kontrol et (C# modellerinde büyük harfle başlayabilir)
              else if (response.Name) {
                role.name = response.Name;
              }
              // Başka bir alanda rol adı olabilir, örneğin roleName
              else if (response.roleName) {
                role.name = response.roleName;
              }
              else if (response.RoleName) {
                role.name = response.RoleName;
              }
              // API yanıtında herhangi bir "name" içeren özellik ara
              else {
                const nameField = Object.keys(response).find(key => 
                  key.toLowerCase().includes('name') && typeof response[key] === 'string'
                );
                
                if (nameField) {
                  role.name = response[nameField];
                }
              }
            }
            
            // Hala rol adı bulunamadıysa varsayılan bir ad kullan
            if (!role.name) {
              role.name = `Rol ${id}`;
              console.log(`Rol adı bulunamadı, varsayılan ad kullanılıyor: ${role.name}`);
            }
          }
          
          return role;
        })
      );
  }

  createRole(role: Omit<Role, 'id'>): Observable<Role> {
    const options = { headers: this.getHeaders() };
    return this.http.post<any>(this.apiUrl, role, options)
      .pipe(
        map(response => this.normalizeResponse<Role>(response)),
        catchError(this.handleError)
      );
  }

  updateRole(id: number, role: Partial<Role>): Observable<Role> {
    const options = { headers: this.getHeaders() };
    return this.http.put<any>(`${this.apiUrl}/${id}`, role, options)
      .pipe(
        map(response => this.normalizeResponse<Role>(response)),
        catchError(this.handleError)
      );
  }

  deleteRole(id: number): Observable<void> {
    const options = { headers: this.getHeaders() };
    return this.http.delete<void>(`${this.apiUrl}/${id}`, options)
      .pipe(
        catchError(this.handleError)
      );
  }
} 