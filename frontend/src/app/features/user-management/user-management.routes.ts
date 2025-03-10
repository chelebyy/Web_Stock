import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';

export const USER_MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/user-management.component').then(m => m.UserManagementComponent),
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true
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
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true
    }
  },
  {
    path: 'users/:userId/page-permissions',
    loadComponent: () => import('./components/user-page-permissions.component').then(m => m.UserPagePermissionsComponent),
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true
    }
  }
]; 