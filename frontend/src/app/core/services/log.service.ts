import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from '../authentication/auth.service';

export interface UserActivityLog {
  userId?: number;
  username: string;
  activityType: string;
  description: string;
  timestamp: Date;
  ipAddress?: string;
  status: string;
  details?: any;
}

@Injectable({
  providedIn: 'root'
})
export class LogService {
  private apiUrl = 'http://localhost:5037/api';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  // Kullanıcı aktivitesini kaydet
  logUserActivity(activity: Partial<UserActivityLog>): Observable<any> {
    const currentUser = this.authService.getCurrentUser();
    
    const logEntry: UserActivityLog = {
      userId: currentUser?.id,
      username: currentUser?.username || 'anonymous',
      activityType: activity.activityType || 'unknown',
      description: activity.description || '',
      timestamp: new Date(),
      status: activity.status || 'info',
      details: activity.details || {}
    };

    console.log('Aktivite kaydediliyor:', logEntry);
    
    // API'ye log gönder
    return this.http.post(`${this.apiUrl}/logs/activity`, logEntry)
      .pipe(
        catchError(error => {
          // Hata durumunda bile konsola kaydet (veri kaybını önlemek için)
          console.error('Log kaydetme hatası:', error);
          
          // Hata durumunda localStorage'a yedekle
          this.saveLogToLocalStorage(logEntry);
          
          return throwError(() => error);
        })
      );
  }

  // API'ye erişilemediğinde localStorage'a kaydet
  private saveLogToLocalStorage(logEntry: UserActivityLog): void {
    try {
      const storedLogs = localStorage.getItem('pendingLogs');
      const pendingLogs = storedLogs ? JSON.parse(storedLogs) : [];
      pendingLogs.push(logEntry);
      
      // En fazla 100 log sakla
      if (pendingLogs.length > 100) {
        pendingLogs.shift(); // En eski logu çıkar
      }
      
      localStorage.setItem('pendingLogs', JSON.stringify(pendingLogs));
      console.log('Log localStorage\'a kaydedildi');
    } catch (error) {
      console.error('localStorage\'a log kaydetme hatası:', error);
    }
  }

  // Bekleyen logları gönder (uygulama başlangıcında çağrılabilir)
  syncPendingLogs(): void {
    try {
      const storedLogs = localStorage.getItem('pendingLogs');
      if (!storedLogs) return;
      
      const pendingLogs: UserActivityLog[] = JSON.parse(storedLogs);
      if (pendingLogs.length === 0) return;
      
      console.log(`${pendingLogs.length} adet bekleyen log senkronize ediliyor...`);
      
      // Toplu log gönderme
      this.http.post(`${this.apiUrl}/logs/bulk-activity`, pendingLogs)
        .subscribe({
          next: () => {
            console.log('Bekleyen loglar başarıyla senkronize edildi');
            localStorage.removeItem('pendingLogs');
          },
          error: (error) => {
            console.error('Bekleyen logları senkronize etme hatası:', error);
          }
        });
    } catch (error) {
      console.error('Log senkronizasyon hatası:', error);
    }
  }

  // Kullanıcı aktivite loglarını getir
  getUserActivityLogs(page = 1, pageSize = 10): Observable<any> {
    return this.http.get(`${this.apiUrl}/logs/activity`, {
      params: {
        page: page.toString(),
        pageSize: pageSize.toString()
      }
    }).pipe(
      catchError(error => {
        console.error('Aktivite loglarını getirme hatası:', error);
        return throwError(() => error);
      })
    );
  }
} 