import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private requestCount = 0;
  // Signal API kullanarak yükleme durumunu reaktif olarak yönetelim
  readonly isLoading = signal(false);

  /**
   * Yeni bir HTTP isteği başladığında çağrılır.
   * Aktif istek sayısını artırır ve gerekirse yükleme durumunu true yapar.
   */
  show(): void {
    this.requestCount++;
    if (this.requestCount === 1) {
      this.isLoading.set(true);
    }
    // console.log('Request started. Count:', this.requestCount, 'isLoading:', this.isLoading());
  }

  /**
   * Bir HTTP isteği tamamlandığında (başarılı veya hatalı) çağrılır.
   * Aktif istek sayısını azaltır ve gerekirse yükleme durumunu false yapar.
   */
  hide(): void {
    if (this.requestCount > 0) {
      this.requestCount--;
      if (this.requestCount === 0) {
        this.isLoading.set(false);
      }
    }
    // console.log('Request ended. Count:', this.requestCount, 'isLoading:', this.isLoading());
  }
} 