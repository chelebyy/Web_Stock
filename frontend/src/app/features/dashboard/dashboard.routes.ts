import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';
import { PermissionGuard } from '../../core/guards/permission.guard';

export const DASHBOARD_ROUTES: Routes = [
  {
    path: 'admin-dashboard',
    loadComponent: () => import('./components/admin-dashboard.component').then(m => m.AdminDashboardComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Pages.AdminDashboard']
    }
  },
  {
    path: 'user-dashboard',
    loadComponent: () => import('./components/user-dashboard.component').then(m => m.UserDashboardComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Pages.UserDashboard']
    }
  },
  {
    path: '',
    redirectTo: 'user-dashboard',
    pathMatch: 'full'
  }
]; 