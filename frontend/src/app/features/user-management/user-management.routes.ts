import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';
import { PermissionGuard } from '../../core/guards/permission.guard';

export const USER_MANAGEMENT_ROUTES: Routes = [
  {
    path: 'user-management',
    loadComponent: () => import('./components/user-management.component').then(m => m.UserManagementComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.UserManagement']
    }
  },
  {
    path: 'admin/users',
    loadComponent: () => import('./components/user-management.component').then(m => m.UserManagementComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.UserManagement']
    }
  },
  {
    path: 'users/:userId/permissions',
    loadComponent: () => import('../../components/permission-management/permission-management.component').then(m => m.PermissionManagementComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Users.Permissions.Manage']
    }
  },
  {
    path: 'users/:userId/page-permissions',
    loadComponent: () => import('../../components/user-page-permissions/user-page-permissions.component').then(m => m.UserPagePermissionsComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Users.Permissions.Manage']
    }
  }
]; 