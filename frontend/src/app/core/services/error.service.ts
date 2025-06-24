import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
import { HttpStatusCodes } from './base-http.service'; // HttpStatusCodes enum'unu import et

/**
 * Uygulama genelinde hata yönetimini sağlayan servis.
 * HTTP hataları için özelleştirilmiş mesajlar gösterir ve
 * kullanıcıya anlamlı geri bildirimler sunar.
 */
@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  /**
   * ErrorService constructor
   * @param messageService PrimeNG toast mesajları için kullanılan servis
   */
  constructor(private messageService: MessageService) {}

  /**
   * HTTP hatalarını merkezi olarak işler ve kullanıcıya uygun bir mesaj gösterir.
   * @param error Yakalanan HttpErrorResponse nesnesi
   */
  handleError(error: HttpErrorResponse) {
    // Geliştirme ortamı için detaylı loglama
    console.error(`Hata Durumu: ${error.status}, URL: ${error.url}`, error);

    let summary = 'Hata';
    let detail = 'Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.';

    switch (error.status) {
      case HttpStatusCodes.CONNECTION_ERROR: // 0
        summary = 'Bağlantı Hatası';
        detail = 'Sunucuya ulaşılamıyor. İnternet bağlantınızı kontrol edin.';
        break;

      case HttpStatusCodes.BAD_REQUEST: // 400
        summary = 'Geçersiz İstek';
        // Backend'den gelen mesajı önceliklendir
        detail = this.extractBackendErrorMessage(error) || 'İstekte geçersiz veya eksik bilgi var.';
        break;

      case HttpStatusCodes.UNAUTHORIZED: // 401
        // AuthInterceptor zaten yönlendirme yapıyor. Burada ek mesaj göstermeyebiliriz.
        console.warn('401 Unauthorized hatası alındı, AuthInterceptor yönlendirme yapmalı.');
        return; // Toast mesajı gösterme

      case HttpStatusCodes.FORBIDDEN: // 403
        summary = 'Erişim Reddedildi';
        detail = 'Bu işlemi yapmaya veya bu kaynağı görüntülemeye yetkiniz yok.';
        break;

      case HttpStatusCodes.NOT_FOUND: // 404
        summary = 'Bulunamadı';
        detail = 'İstediğiniz kaynak sunucuda bulunamadı.';
        break;

      case HttpStatusCodes.SERVER_ERROR: // 500 ve üstü
      default:
        if (error.status >= HttpStatusCodes.SERVER_ERROR) {
          summary = 'Sunucu Hatası';
          detail = 'Sunucuda beklenmeyen bir hata oluştu. Sistem yöneticisi ile iletişime geçin.';
        } else if (error.status >= HttpStatusCodes.BAD_REQUEST) { // Diğer 4xx hataları
          summary = 'İstemci Hatası';
          detail = this.extractBackendErrorMessage(error) || 'İstek işlenirken bir sorun oluştu.';
        }
        // Diğer durumlar için varsayılan mesaj kalır
        break;
    }

    this.showError(summary, detail);
  }

  /**
   * HttpErrorResponse içinden backend tarafından gönderilen hata mesajını
   * güvenli bir şekilde çıkarmaya çalışır.
   * @param error HttpErrorResponse nesnesi
   * @returns Backend hata mesajı veya null
   */
  extractBackendErrorMessage(error: HttpErrorResponse): string | null {
    if (error.error) {
      // String olarak gelmişse
      if (typeof error.error === 'string') {
        return this.sanitizeErrorMessage(error.error);
      }
      
      // Obje içinde message veya detail varsa
      if (typeof error.error === 'object') {
        const message = error.error.message || error.error.detail || error.error.title;
        if (typeof message === 'string') {
          return this.sanitizeErrorMessage(message);
        }
        
        // FluentValidation hataları gibi Errors nesnesi varsa
        if (error.error.errors && typeof error.error.errors === 'object') {
          try {
            // Hataları birleştirerek tek bir string yap
            const errorMessages = Object.values(error.error.errors).flat().join(' \n');
            if (errorMessages) {
              return this.sanitizeErrorMessage(errorMessages);
            }
          } catch (e) {
            console.warn('Backend validation errors formatı beklenenden farklı:', error.error.errors);
          }
        }
      }
    }
    return null;
  }

  /**
   * Kullanıcıya gösterilecek hata mesajlarını temizler ve kısaltır.
   * @param message Ham hata mesajı
   * @returns Temizlenmiş ve kısaltılmış mesaj
   */
  sanitizeErrorMessage(message: string): string {
    // Çok uzun mesajları kısalt
    const maxLength = 200;
    const sanitized = message.length > maxLength ? message.substring(0, maxLength) + '...' : message;

    // İstenmeyen karakterleri veya HTML'i temizleme (örnek)
    // sanitized = sanitized.replace(/<[^>]*>/g, '');

    return sanitized;
  }

  /**
   * Kullanıcıya hata mesajı gösterir
   * @param summary Mesaj başlığı
   * @param detail Mesaj detayı
   */
  private showError(summary: string, detail: string) {
    this.messageService.add({
      severity: 'error',
      summary: summary,
      detail: detail,
      life: 5000
    });
  }

  /**
   * Kullanıcıya başarı mesajı gösterir
   * @param summary Mesaj başlığı
   * @param detail Mesaj detayı
   */
  showSuccess(summary: string, detail: string) {
    this.messageService.add({
      severity: 'success',
      summary: summary,
      detail: detail,
      life: 3000
    });
  }

  /**
   * Kullanıcıya bilgi mesajı gösterir
   * @param summary Mesaj başlığı
   * @param detail Mesaj detayı
   */
  showInfo(summary: string, detail: string) {
    this.messageService.add({
      severity: 'info',
      summary: summary,
      detail: detail,
      life: 3000
    });
  }

  /**
   * Kullanıcıya uyarı mesajı gösterir
   * @param summary Mesaj başlığı
   * @param detail Mesaj detayı
   */
  showWarn(summary: string, detail: string) {
    this.messageService.add({
      severity: 'warn',
      summary: summary,
      detail: detail,
      life: 4000
    });
  }
} 