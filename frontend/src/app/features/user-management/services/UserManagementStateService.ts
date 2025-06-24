import { Injectable, signal, computed } from '@angular/core';
import { User } from '../../../shared/models/user.model';
import { Role } from '../../../shared/models/role.model';

export interface UserManagementState {
  users: User[];
  roles: Role[];
  searchText: string;
  selectedRole: number | null;
  loading: boolean;
  error: string | null;
}

const initialState: UserManagementState = {
  users: [],
  roles: [],
  searchText: '',
  selectedRole: null,
  loading: false,
  error: null
};

@Injectable()
export class UserManagementStateService {

  private state = signal<UserManagementState>(initialState);

  // Selectors
  users = computed(() => this.state().users);
  roles = computed(() => this.state().roles);
  loading = computed(() => this.state().loading);
  error = computed(() => this.state().error);
  
  filteredUsers = computed(() => {
    const users = this.state().users;
    const text = this.state().searchText.toLowerCase();
    const roleId = this.state().selectedRole;

    return users.filter(user => {
      const nameMatch = user.fullName?.toLowerCase().includes(text) || user.sicil?.toLowerCase().includes(text);
      const roleMatch = !roleId || user.roleId === roleId;
      return nameMatch && roleMatch;
    });
  });

  // Actions
  setUsers(users: User[]) {
    this.state.update(s => ({ ...s, users }));
  }
  
  setRoles(roles: Role[]) {
    this.state.update(s => ({ ...s, roles }));
  }

  setSearchText(searchText: string) {
    this.state.update(s => ({ ...s, searchText }));
  }

  setSelectedRole(roleId: number | null) {
    this.state.update(s => ({ ...s, selectedRole: roleId }));
  }
  
  setLoading(loading: boolean) {
    this.state.update(s => ({ ...s, loading }));
  }
  
  setError(error: string | null) {
    this.state.update(s => ({ ...s, error }));
  }
} 