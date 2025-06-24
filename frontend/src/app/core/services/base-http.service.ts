import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from '../authentication/auth.service';

/**
 * HTTP durum kodları için enum.
 * Yaygın kullanılan durum kodlarını ve açıklamalarını içerir.
 */
export enum HttpStatusCodes {
  OK = 200,
  CREATED = 201,
  NO_CONTENT = 204,
  BAD_REQUEST = 400,
  UNAUTHORIZED = 401,
  FORBIDDEN = 403,
  NOT_FOUND = 404,
  SERVER_ERROR = 500,
  CONNECTION_ERROR = 0
}

/**
 * HTTP isteklerini standartlaştıran temel servis sınıfı.
 * Tüm API servisleri için ortak fonksiyonellikleri sağlar.
 * @template T - Servisin işleyeceği temel veri tipi
 */
@Injectable({
  providedIn: 'root'
})
export abstract class BaseHttpService<T> {
  /**
   * BaseHttpService constructor
   * @param http - Angular HTTP istemcisi
   * @param authService - Kimlik doğrulama servisi
   * @param baseUrl - API temel URL'i
   */
  protected constructor(
    protected http: HttpClient,
    protected authService: AuthService,
    protected baseUrl: string
  ) {}

  /**
   * İstekler için gerekli olan HTTP başlıklarını oluşturur.
   * Authorization başlığını otomatik olarak ekler.
   * @returns Oluşturulan HTTP başlıkları
   */
  protected getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  /**
   * API yanıtını standart bir formata dönüştürür.
   * Farklı API cevap formatlarını (diziler, $values nesneleri vb.) işler.
   * @template R - Beklenen yanıt tipi
   * @param response - API'den gelen ham yanıt
   * @returns Normalize edilmiş yanıt
   */
  protected normalizeResponse<R>(response: any): R {
    // ReferenceHandler.Preserve formatını işle ($id ve $values)
    if (response && response.$values && Array.isArray(response.$values)) {
      return response.$values as R;
    }
    
    // Normal dizi yanıtını işle
    if (Array.isArray(response)) {
      return response as R;
    }
    
    // Diğer olası formatlara bak
    if (response && typeof response === 'object') {
      // value ya da Value içinde olabilir
      if (response.value && Array.isArray(response.value)) {
        return response.value as R;
      }
      
      if (response.Value && Array.isArray(response.Value)) {
        return response.Value as R;
      }
    }
    
    // Yanıt doğrudan bir nesne ise
    return response as R;
  }

  /**
   * GET isteği gerçekleştirir.
   * @template R - Beklenen yanıt tipi
   * @param path - API endpoint yolu
   * @param params - İsteğe bağlı URL parametreleri
   * @returns API yanıtını içeren Observable
   */
  public get<R>(path: string, params?: HttpParams): Observable<R> {
    const options = { headers: this.getHeaders(), params };
    const url = `${this.baseUrl}/${path}`;
    return this.http.get<any>(url, options).pipe(
      map(response => this.normalizeResponse<R>(response))
    );
  }

  /**
   * POST isteği gerçekleştirir.
   * @template R - Beklenen yanıt tipi
   * @param path - API endpoint yolu
   * @param body - İstek gövdesi
   * @returns API yanıtını içeren Observable
   */
  public post<R>(path: string, body: any): Observable<R> {
    const options = { headers: this.getHeaders() };
    const url = `${this.baseUrl}/${path}`;
    return this.http.post<any>(url, body, options).pipe(
      map(response => this.normalizeResponse<R>(response))
    );
  }

  /**
   * PUT isteği gerçekleştirir.
   * @template R - Beklenen yanıt tipi
   * @param path - API endpoint yolu
   * @param body - İstek gövdesi
   * @returns API yanıtını içeren Observable
   */
  public put<R>(path: string, body: any): Observable<R> {
    const options = { headers: this.getHeaders() };
    const url = `${this.baseUrl}/${path}`;
    return this.http.put<any>(url, body, options).pipe(
      map(response => this.normalizeResponse<R>(response))
    );
  }

  /**
   * DELETE isteği gerçekleştirir.
   * @template R - Beklenen yanıt tipi
   * @param path - API endpoint yolu
   * @returns API yanıtını içeren Observable
   */
  public delete<R>(path: string): Observable<R> {
    const options = { headers: this.getHeaders() };
    const url = `${this.baseUrl}/${path}`;
    return this.http.delete<any>(url, options).pipe(
      map(response => this.normalizeResponse<R>(response))
    );
  }

  /**
   * Sunucuya dosya yüklemek için kullanılır.
   * @template R - Beklenen yanıt tipi
   * @param path - API endpoint yolu
   * @param file - Yüklenecek dosya
   * @param fileKey - Sunucunun dosyayı alması için beklediği anahtar
   * @param additionalData - Dosya ile birlikte gönderilecek ek veriler
   * @returns API yanıtını içeren Observable
   */
  public upload<R>(path: string, file: File, fileKey = 'file', additionalData?: Record<string, any>): Observable<R> {
    const formData = new FormData();
    formData.append(fileKey, file, file.name);

    // Ek verileri FormData'ya ekle
    if (additionalData) {
      Object.keys(additionalData).forEach(key => {
        formData.append(key, additionalData[key]);
      });
    }

    // 'Content-Type' başlığını kaldırıyoruz,
    // Tarayıcı FormData için doğru 'multipart/form-data' başlığını otomatik olarak ayarlayacak
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const options = { headers: headers };
    const url = `${this.baseUrl}/${path}`;

    return this.http.post<any>(url, formData, options).pipe(
      map(response => this.normalizeResponse<R>(response))
    );
  }
} 