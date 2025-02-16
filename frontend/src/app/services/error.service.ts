import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  constructor(private messageService: MessageService) {}

  handleError(error: any) {
    console.error('Bir hata oluştu:', error);

    if (error instanceof HttpErrorResponse) {
      let errorMessage = 'Bir hata oluştu';
      
      if (error.error?.error?.message) {
        errorMessage = error.error.error.message;
      } else if (typeof error.error === 'string') {
        errorMessage = error.error;
      }

      switch (error.status) {
        case 401:
          this.showError('Yetkilendirme Hatası', 'Bu işlem için yetkiniz bulunmamaktadır.');
          break;
        case 403:
          this.showError('Erişim Reddedildi', 'Bu kaynağa erişim izniniz bulunmamaktadır.');
          break;
        case 404:
          this.showError('Bulunamadı', 'İstenen kayıt bulunamadı.');
          break;
        case 400:
          this.showError('Geçersiz İstek', errorMessage);
          break;
        case 500:
          this.showError('Sunucu Hatası', 'İşlem sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.');
          break;
        default:
          this.showError('Hata', errorMessage);
          break;
      }
    } else {
      this.showError('Hata', 'Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.');
    }
  }

  private showError(summary: string, detail: string) {
    this.messageService.add({
      severity: 'error',
      summary: summary,
      detail: detail,
      life: 5000
    });
  }

  showSuccess(summary: string, detail: string) {
    this.messageService.add({
      severity: 'success',
      summary: summary,
      detail: detail,
      life: 3000
    });
  }

  showInfo(summary: string, detail: string) {
    this.messageService.add({
      severity: 'info',
      summary: summary,
      detail: detail,
      life: 3000
    });
  }

  showWarn(summary: string, detail: string) {
    this.messageService.add({
      severity: 'warn',
      summary: summary,
      detail: detail,
      life: 4000
    });
  }
}
