import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Role } from '../shared/models/role.model';
import { environment } from '../../environments/environment';
import { AuthService } from '../core/authentication/auth.service';
import { BaseHttpService } from '../core/services/base-http.service';
import { PagedResponse } from '../shared/models/paged-response.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RoleService extends BaseHttpService<Role> {
  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService, `${environment.apiUrl}/api/roles`);
  }

  // Tüm rolleri getir
  getRoles(pageNumber: number = 1, pageSize: number = 10): Observable<PagedResponse<Role>> {
    let params = new HttpParams()
      .set('PageNumber', pageNumber.toString())
      .set('PageSize', pageSize.toString());
    return this.get<PagedResponse<Role>>('', params);
  }

  // Sayfalama olmadan tüm rolleri getir
  getAllRoles(): Observable<Role[]> {
    // Genellikle backend'de sayfalama olmadan tüm veriyi döndüren ayrı bir endpoint olur.
    // Örnek olarak, çok büyük bir pageSize ile tüm veriyi çekmeyi deneyebiliriz.
    // İdeal olan, backend'e /api/roles/all gibi bir endpoint eklemektir.
    // Geçici çözüm olarak, pageSize'ı yüksek bir değere ayarlıyoruz.
    let params = new HttpParams().set('PageSize', '1000'); // 1000 rol varsayımı
    return this.get<PagedResponse<Role>>('', params).pipe(
      map(pagedResponse => pagedResponse.items)
    );
  }

  // Belirli bir rolü getir
  getRole(id: number): Observable<Role> {
    return this.get<Role>(`${id}`);
  }

  // Yeni rol oluştur
  createRole(role: Role): Observable<Role> {
    return this.post<Role>('', role);
  }

  // Rol güncelle
  updateRole(id: number, role: Role): Observable<Role> {
    return this.put<Role>(`${id}`, role);
  }

  // Rol sil
  deleteRole(id: number): Observable<void> {
    return this.delete<void>(`${id}`);
  }
} 