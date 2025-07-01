import { Injectable, computed, signal, inject } from '@angular/core';
import { DashboardPermissionService, DashboardUser, PagePermission } from './dashboard-permission.service';
import { catchError, finalize, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { MessageService } from 'primeng/api';

interface PermissionState {
  users: DashboardUser[];
  permissions: PagePermission[];
  loading: boolean;
  processing: boolean;
  error: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardPermissionStateService {
  private permissionService = inject(DashboardPermissionService);
  private messageService = inject(MessageService);

  // State
  private state = signal<PermissionState>({
    users: [],
    permissions: [],
    loading: false,
    processing: false,
    error: null
  });

  // Selectors
  users = computed(() => this.state().users);
  permissions = computed(() => this.state().permissions);
  loading = computed(() => this.state().loading);
  processing = computed(() => this.state().processing);
  error = computed(() => this.state().error);

  // Actions
  loadInitialData(pageId: number) {
    this.state.update(s => ({ ...s, loading: true, error: null }));

    this.permissionService.getUsers(pageId).pipe(
      catchError(err => this.handleError(err, 'Kullanıcılar yüklenemedi'))
    ).subscribe(users => {
      this.state.update(s => ({ ...s, users: users || [] }));
    });

    this.permissionService.getPagePermissions(pageId).pipe(
      catchError(err => this.handleError(err, 'İzinler yüklenemedi')),
      finalize(() => this.state.update(s => ({...s, loading: false})))
    ).subscribe(permissions => {
      this.state.update(s => ({ ...s, permissions: permissions || [] }));
    });
  }

  grantPermission(pageId: number, user: DashboardUser) {
    this.state.update(s => ({ ...s, processing: true }));
    this.permissionService.grantPermission(pageId, user.id).pipe(
      tap(() => {
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: `${user.fullName} için izin verildi.` });
        // Optimistic update
        this.state.update(s => ({
          ...s,
          users: s.users.map(u => u.id === user.id ? { ...u, hasPagePermission: true } : u),
          permissions: [...s.permissions, {
              id: 0, // Temp ID
              userId: user.id,
              pageId: pageId,
              username: user.username || '',
              fullName: user.fullName,
              sicil: user.sicil,
              grantedAt: new Date()
          }]
        }));
      }),
      catchError(err => {
        // Rollback on error if needed, for now just show error
        return this.handleError(err, 'İzin verilemedi');
      }),
      finalize(() => this.state.update(s => ({ ...s, processing: false })))
    ).subscribe();
  }

  revokePermission(pageId: number, permission: PagePermission) {
    this.state.update(s => ({ ...s, processing: true }));
    this.permissionService.revokePermission(pageId, permission.id).pipe(
      tap(() => {
        this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: `${permission.fullName} için izin kaldırıldı.` });
        // Optimistic update
        this.state.update(s => ({
          ...s,
          users: s.users.map(u => u.id === permission.userId ? { ...u, hasPagePermission: false } : u),
          permissions: s.permissions.filter(p => p.id !== permission.id)
        }));
      }),
      catchError(err => this.handleError(err, 'İzin kaldırılamadı')),
      finalize(() => this.state.update(s => ({ ...s, processing: false })))
    ).subscribe();
  }

  private handleError(error: any, defaultMessage: string) {
    const detail = error?.error?.message || defaultMessage;
    this.messageService.add({ severity: 'error', summary: 'Hata', detail });
    this.state.update(s => ({ ...s, error: detail, loading: false, processing: false }));
    return of(null);
  }
} 