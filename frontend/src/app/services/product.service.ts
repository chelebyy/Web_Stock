import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../core/authentication/auth.service';
import { environment } from '../../environments/environment';

// Backend DTO'suna karşılık gelen interface
export interface ProductDto {
  id: number;
  name: string;
  description: string;
  stockLevel: number;
  categoryId: number;
  categoryName: string;
}

// Backend Command'larına karşılık gelen interface'ler
export interface CreateProductCommand {
  name: string;
  description?: string;
  stockLevel: number;
  categoryId: number;
}

export interface UpdateProductCommand {
  id: number; // ID güncelleme komutunda da genellikle bulunur
  name: string;
  description: string;
  stockLevel: number;
  categoryId: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = `${environment.apiUrl}/api/products`;
  private http = inject(HttpClient);
  private authService = inject(AuthService);

  constructor() {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getAllProducts(): Observable<ProductDto[]> {
    return this.http.get<ProductDto[]>(`${this.apiUrl}`, { headers: this.getHeaders() });
  }

  getProductById(id: number): Observable<ProductDto> {
    return this.http.get<ProductDto>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }

  createProduct(command: CreateProductCommand): Observable<ProductDto> {
    return this.http.post<ProductDto>(`${this.apiUrl}`, command, { headers: this.getHeaders() });
  }

  updateProduct(id: number, command: UpdateProductCommand): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, command, { headers: this.getHeaders() });
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }
} 