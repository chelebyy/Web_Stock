import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../core/services/base-http.service';
import { AuthService } from '../core/authentication/auth.service';

// Backend DTO'suna karşılık gelen interface
export interface CategoryDto {
  id: number;
  name: string;
  description?: string;
}

// Backend Command'larına karşılık gelen interface'ler
export interface CreateCategoryCommand {
  name: string;
  description?: string;
}

export interface UpdateCategoryCommand {
  id: number; 
  name: string;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryService extends BaseHttpService<any> {
  constructor() {
    super(inject(HttpClient), inject(AuthService), 'api/categories');
  }

  getAllCategories(): Observable<CategoryDto[]> {
    return this.get<CategoryDto[]>('');
  }

  getCategoryById(id: number): Observable<CategoryDto> {
    return this.get<CategoryDto>(`${id}`);
  }

  createCategory(command: CreateCategoryCommand): Observable<CategoryDto> {
    return this.post<CategoryDto>('', command);
  }

  updateCategory(id: number, command: UpdateCategoryCommand): Observable<void> {
    return this.put<void>(`${id}`, command);
  }

  deleteCategory(id: number): Observable<void> {
    return this.delete<void>(`${id}`);
  }
} 