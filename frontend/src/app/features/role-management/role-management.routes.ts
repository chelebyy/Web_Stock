import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';
import { PermissionGuard } from '../../core/guards/permission.guard';

export const ROLE_MANAGEMENT_ROUTES: Routes = [
  {
    path: 'role-management',
    loadComponent: () => import('./components/role-management.component').then(m => m.RoleManagementComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.RoleManagement']
    }
  },
  {
    path: 'roles',
    loadComponent: () => import('./components/role-management.component').then(m => m.RoleManagementComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.RoleManagement']
    }
  },
  {
    path: 'roles/:roleId',
    loadComponent: () => import('./components/role-detail/role-detail.component').then(m => m.RoleDetailComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.RoleManagement']
    }
  }
]; 