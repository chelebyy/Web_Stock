import { Injectable, signal, computed, effect } from '@angular/core';
import { User } from '../../../shared/models/user.model';
import { Role } from '../../../shared/models/role.model';

export interface UserManagementState {
  users: User[];
  roles: Role[];
  searchText: string;
  selectedRole: Role | null;
  currentPage: number;
  rowsPerPage: number;
  loading: boolean;
  error: string | null;
}

const initialState: UserManagementState = {
  users: [],
  roles: [],
  searchText: '',
  selectedRole: null,
  currentPage: 1,
  rowsPerPage: 10,
  loading: false,
  error: null,
};

@Injectable({
  providedIn: 'root'
})
export class UserManagementStateService {
  // State Signals
  private state = signal(initialState);

  // Selectors (Public Signals)
  users = computed(() => this.state().users);
  roles = computed(() => this.state().roles);
  searchText = computed(() => this.state().searchText);
  selectedRole = computed(() => this.state().selectedRole);
  currentPage = computed(() => this.state().currentPage);
  rowsPerPage = computed(() => this.state().rowsPerPage);
  loading = computed(() => this.state().loading);
  error = computed(() => this.state().error);

  // Derived State (Computed Signals)
  filteredUsers = computed(() => {
    const users = this.users();
    const searchText = this.searchText().toLowerCase();
    const selectedRole = this.selectedRole();

    if (!searchText && !selectedRole) {
      return users;
    }

    return users.filter(user => {
      const searchMatch = !searchText || 
        (user.fullName && user.fullName.toLowerCase().includes(searchText)) ||
        (user.sicil && user.sicil.toLowerCase().includes(searchText));
      
      const roleMatch = !selectedRole || user.roleId === selectedRole.id;
      
      return searchMatch && roleMatch;
    });
  });

  totalPages = computed(() => {
    return Math.ceil(this.filteredUsers().length / this.rowsPerPage());
  });

  paginatedUsers = computed(() => {
    const page = this.currentPage();
    const rows = this.rowsPerPage();
    const users = this.filteredUsers();
    const startIndex = (page - 1) * rows;
    return users.slice(startIndex, startIndex + rows);
  });
  
  // Actions (Methods to update state)
  setUsers(users: User[]) {
    this.state.update(s => ({ ...s, users }));
  }
  
  setRoles(roles: Role[]) {
    this.state.update(s => ({ ...s, roles }));
  }

  setSearchText(searchText: string) {
    this.state.update(s => ({ ...s, searchText, currentPage: 1 }));
  }

  setSelectedRole(role: Role | null) {
    this.state.update(s => ({ ...s, selectedRole: role, currentPage: 1 }));
  }
  
  setCurrentPage(page: number) {
    this.state.update(s => ({ ...s, currentPage: page }));
  }
  
  setRowsPerPage(rows: number) {
    this.state.update(s => ({ ...s, rowsPerPage: rows, currentPage: 1 }));
  }

  setLoading(loading: boolean) {
    this.state.update(s => ({ ...s, loading }));
  }

  setError(error: string | null) {
    this.state.update(s => ({ ...s, error }));
  }
  
  constructor() {
    // You can add effects here to react to state changes, e.g., for debugging
    effect(() => {
      console.log('User management state changed:', this.state());
    });
  }
} 