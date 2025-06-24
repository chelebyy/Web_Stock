import { Injectable, signal, computed } from '@angular/core';
import { Role } from '../../../shared/models/role.model';
import { PagedResponse } from '../../../shared/models/paged-response.model';

export interface RoleState {
  roles: PagedResponse<Role> | null;
  loading: boolean;
  error: any | null;
}

@Injectable({
  providedIn: 'root'
})
export class RoleStateService {
  private state = signal<RoleState>({
    roles: null,
    loading: false,
    error: null,
  });

  // Selectors
  public roles = computed(() => this.state().roles?.items || []);
  public totalRecords = computed(() => this.state().roles?.totalRecords || 0);
  public loading = computed(() => this.state().loading);
  public error = computed(() => this.state().error);

  // Actions
  setRoles(roles: PagedResponse<Role>) {
    this.state.update(state => ({ ...state, roles }));
  }

  setLoading(loading: boolean) {
    this.state.update(state => ({ ...state, loading }));
  }

  setError(error: any) {
    this.state.update(state => ({ ...state, error }));
  }
} 