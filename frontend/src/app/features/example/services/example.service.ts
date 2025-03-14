import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/services/base-http.service';
import { AuthService } from '../../../core/authentication/auth.service';

// Örnek model
export interface ExampleItem {
  id: number;
  name: string;
  description: string;
  createdAt: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ExampleService extends BaseHttpService {
  private endpoint = '/api/example';

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }

  /**
   * Tüm örnek öğeleri getirir
   * @returns Observable<ExampleItem[]> - Örnek öğe listesi
   */
  getItems(): Observable<ExampleItem[]> {
    return this.get<ExampleItem[]>(this.endpoint);
  }

  /**
   * ID'ye göre örnek öğe getirir
   * @param id - Örnek öğe ID'si
   * @returns Observable<ExampleItem> - Örnek öğe
   */
  getItemById(id: number): Observable<ExampleItem> {
    return this.get<ExampleItem>(`${this.endpoint}/${id}`);
  }

  /**
   * Yeni bir örnek öğe oluşturur
   * @param item - Oluşturulacak örnek öğe
   * @returns Observable<ExampleItem> - Oluşturulan örnek öğe
   */
  createItem(item: Omit<ExampleItem, 'id' | 'createdAt'>): Observable<ExampleItem> {
    return this.post<ExampleItem>(this.endpoint, item);
  }

  /**
   * Örnek öğeyi günceller
   * @param id - Güncellenecek örnek öğe ID'si
   * @param item - Güncellenecek örnek öğe verileri
   * @returns Observable<ExampleItem> - Güncellenen örnek öğe
   */
  updateItem(id: number, item: Partial<ExampleItem>): Observable<ExampleItem> {
    return this.put<ExampleItem>(`${this.endpoint}/${id}`, item);
  }

  /**
   * Örnek öğeyi siler
   * @param id - Silinecek örnek öğe ID'si
   * @returns Observable<void>
   */
  deleteItem(id: number): Observable<void> {
    return this.delete<void>(`${this.endpoint}/${id}`);
  }

  /**
   * Örnek öğe için dosya yükler
   * @param id - Örnek öğe ID'si
   * @param file - Yüklenecek dosya
   * @returns Observable<any> - Yükleme yanıtı
   */
  uploadItemFile(id: number, file: File): Observable<any> {
    return this.uploadFile<any>(`${this.endpoint}/${id}/upload`, file);
  }

  /**
   * Örnek öğe için dosya indirir
   * @param id - Örnek öğe ID'si
   * @returns Observable<Blob> - İndirilen dosya
   */
  downloadItemFile(id: number): Observable<Blob> {
    return this.downloadFile(`${this.endpoint}/${id}/download`);
  }
} 