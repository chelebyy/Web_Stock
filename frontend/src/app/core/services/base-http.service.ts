import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { AuthService } from '../authentication/auth.service';

@Injectable()
export class BaseHttpService {
  protected apiUrl: string = environment.apiUrl;
  
  constructor(
    protected http: HttpClient,
    protected authService: AuthService
  ) { }

  /**
   * HTTP Headers oluşturur
   * @returns HttpHeaders - İçinde Content-Type ve Authorization bilgisi olan header
   */
  protected getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  /**
   * HTTP hatalarını işler
   * @param error - HTTP hata yanıtı
   * @returns Observable<never> - Hata Observable'ı
   */
  protected handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Bilinmeyen bir hata oluştu';
    let errorField = '';
    let errorCode = '';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side hata
      errorMessage = `Hata: ${error.error.message}`;
    } else {
      // Backend hatası
      errorMessage = `Sunucu hatası: ${error.status}, Mesaj: ${error.message}`;
      
      // Sunucudan gelen hata mesajını kontrol et
      if (error.error) {
        if (typeof error.error === 'string') {
          errorMessage = error.error;
        } else if (error.error.error) {
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
      }
      
      // HTTP durum kodu kontrolü
      if (error.status === 401) {
        errorMessage = 'Oturum süresi dolmuş veya yetkiniz yok. Lütfen tekrar giriş yapın.';
      } else if (error.status === 403) {
        errorMessage = 'Bu işlemi gerçekleştirmek için yetkiniz yok.';
      } else if (error.status === 404) {
        errorMessage = 'İstek yapılan kaynak bulunamadı.';
      } else if (error.status === 500) {
        if (error.error && error.error.details) {
          errorMessage = `Sunucu hatası: ${error.error.details}`;
        } else {
          errorMessage = 'Sunucu tarafında bir hata oluştu. Lütfen daha sonra tekrar deneyin.';
        }
      }
    }
    
    // Hata detaylarını logla
    console.error('HTTP İsteği Hatası:', {
      status: error.status,
      statusText: error.statusText,
      message: errorMessage,
      field: errorField,
      code: errorCode,
      url: error.url
    });
    
    // Özel hata nesnesi oluştur
    const customError = new Error(errorMessage);
    (customError as any).field = errorField;
    (customError as any).code = errorCode;
    (customError as any).status = error.status;
    
    return throwError(() => customError);
  }

  /**
   * API yanıtını normalize eder - ReferenceHandler.Preserve uyumluluğu için
   * @param response - API'den gelen yanıt
   * @returns T - Normalize edilmiş veri
   */
  protected normalizeResponse<T>(response: any): T {
    if (!response) return [] as unknown as T;
    
    // $values dizisi varsa
    if (response.$values) {
      return response.$values as T;
    }
    // Dizi ise doğrudan döndür
    else if (Array.isArray(response)) {
      return response as T;
    }
    // Başka özelliklerde veri varsa
    else if (response && typeof response === 'object') {
      // Nesne içinde dizi özelliği ara
      if (response.value && Array.isArray(response.value)) {
        return response.value as T;
      }
      
      if (response.Value && Array.isArray(response.Value)) {
        return response.Value as T;
      }
      
      // ID değeri varsa ve tek bir nesne ise
      if (response.id || response.Id) {
        return response as T;
      }
    }
    
    // Son çare: orijinal yanıtı döndür
    return response as T;
  }

  /**
   * GET isteği gönderir
   * @param endpoint - API endpoint'i
   * @returns Observable<T> - API yanıtı
   */
  protected get<T>(endpoint: string): Observable<T> {
    const options = { headers: this.getHeaders() };
    return this.http.get<any>(`${this.apiUrl}${endpoint}`, options)
      .pipe(
        map(response => this.normalizeResponse<T>(response)),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * POST isteği gönderir
   * @param endpoint - API endpoint'i
   * @param data - Gönderilecek veri
   * @returns Observable<T> - API yanıtı
   */
  protected post<T>(endpoint: string, data: any): Observable<T> {
    const options = { headers: this.getHeaders() };
    return this.http.post<any>(`${this.apiUrl}${endpoint}`, data, options)
      .pipe(
        map(response => this.normalizeResponse<T>(response)),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * PUT isteği gönderir
   * @param endpoint - API endpoint'i
   * @param data - Gönderilecek veri
   * @returns Observable<T> - API yanıtı
   */
  protected put<T>(endpoint: string, data: any): Observable<T> {
    const options = { headers: this.getHeaders() };
    return this.http.put<any>(`${this.apiUrl}${endpoint}`, data, options)
      .pipe(
        map(response => this.normalizeResponse<T>(response)),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * DELETE isteği gönderir
   * @param endpoint - API endpoint'i
   * @returns Observable<T> - API yanıtı
   */
  protected delete<T>(endpoint: string): Observable<T> {
    const options = { headers: this.getHeaders() };
    return this.http.delete<any>(`${this.apiUrl}${endpoint}`, options)
      .pipe(
        map(response => this.normalizeResponse<T>(response)),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Dosya yükleme isteği gönderir
   * @param endpoint - API endpoint'i
   * @param file - Yüklenecek dosya
   * @param additionalData - Ek veri (opsiyonel)
   * @returns Observable<T> - API yanıtı
   */
  protected uploadFile<T>(endpoint: string, file: File, additionalData?: Record<string, any>): Observable<T> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    
    // Ek veri varsa ekle
    if (additionalData) {
      Object.keys(additionalData).forEach(key => {
        formData.append(key, additionalData[key]);
      });
    }
    
    const token = this.authService.getToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    
    return this.http.post<any>(`${this.apiUrl}${endpoint}`, formData, { headers })
      .pipe(
        map(response => this.normalizeResponse<T>(response)),
        catchError(error => this.handleError(error))
      );
  }

  /**
   * Dosya indirme isteği gönderir
   * @param endpoint - API endpoint'i
   * @returns Observable<Blob> - İndirilen dosya
   */
  protected downloadFile(endpoint: string): Observable<Blob> {
    const options = { 
      headers: this.getHeaders(),
      responseType: 'blob' as 'json'
    };
    
    return this.http.get<Blob>(`${this.apiUrl}${endpoint}`, options)
      .pipe(
        catchError(error => this.handleError(error))
      );
  }
} 