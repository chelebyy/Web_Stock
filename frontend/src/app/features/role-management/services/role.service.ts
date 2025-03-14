import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { Role } from '../../../shared/models/role.model';
import { AuthService } from '../../../core/authentication/auth.service';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { BaseHttpService } from '../../../core/services/base-http.service';

@Injectable({
  providedIn: 'root'
})
export class RoleService extends BaseHttpService {
  private endpoint = '/api/Role';
  
  // Signal tanımları
  private _roles = signal<Role[]>([]);
  private _loading = signal<boolean>(false);
  private _error = signal<string | null>(null);
  
  // Computed signals
  readonly roles = computed(() => this._roles());
  readonly loading = computed(() => this._loading());
  readonly error = computed(() => this._error());

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }

  // Signal ile roller yükle
  loadRoles(): void {
    this._loading.set(true);
    this._error.set(null);
    
    this.getRoles().subscribe({
      next: (data) => {
        this._roles.set(data);
        this._loading.set(false);
      },
      error: (error) => {
        console.error('Roller yüklenirken hata:', error);
        this._error.set('Roller yüklenirken bir hata oluştu.');
        this._loading.set(false);
      }
    });
  }

  // GET /api/Role - Tüm rolleri getir
  getRoles(): Observable<Role[]> {
    return this.get<Role[]>(this.endpoint);
  }

  getRole(id: number): Observable<Role> {
    return this.get<Role>(`${this.endpoint}/${id}`);
  }

  getRoleById(id: number): Observable<Role> {
    return this.getRole(id).pipe(
      catchError(error => {
        console.error(`ID ${id} ile rol getirilirken hata:`, error);
        throw error;
      })
    );
  }

  createRole(role: Omit<Role, 'id'>): Observable<Role> {
    return this.post<Role>(this.endpoint, role);
  }

  updateRole(id: number, role: Partial<Role>): Observable<Role> {
    return this.put<Role>(`${this.endpoint}/${id}`, role);
  }

  deleteRole(id: number): Observable<void> {
    return this.delete<void>(`${this.endpoint}/${id}`);
  }
} 