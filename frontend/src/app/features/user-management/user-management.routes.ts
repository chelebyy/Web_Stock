import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { permissionGuard } from '../../core/guards/permission.guard';

export const USER_MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/user-management.component').then(m => m.UserManagementComponent),
    canActivate: [authGuard, permissionGuard],
    data: { 
      requiredPermission: 'Pages.UserManagement'
    }
  },
  {
    path: 'user-management',
    redirectTo: '',
    pathMatch: 'full'
  },
  {
    path: 'users/:userId/permissions',
    loadComponent: () => import('./components/permission-management.component').then(m => m.PermissionManagementComponent),
    canActivate: [authGuard, permissionGuard],
    data: { 
      requiredPermission: 'Pages.UserManagement'
    }
  },
  {
    path: 'users/:userId/page-permissions',
    loadComponent: () => import('./components/user-page-permissions.component').then(m => m.UserPagePermissionsComponent),
    canActivate: [authGuard, permissionGuard],
    data: { 
      requiredPermission: 'Pages.UserManagement'
    }
  }
]; 