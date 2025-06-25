import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { permissionGuard } from '../../core/guards/permission.guard';

export const DASHBOARD_ROUTES: Routes = [
  {
    path: 'admin-dashboard',
    loadComponent: () => import('./components/admin-dashboard.component').then(m => m.AdminDashboardComponent),
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPermission: 'Pages.AdminDashboard'
    }
  },
  {
    path: 'user-dashboard',
    loadComponent: () => import('./components/user-dashboard.component').then(m => m.UserDashboardComponent),
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPermission: 'Pages.UserDashboard'
    }
  },
  {
    path: '',
    redirectTo: 'user-dashboard',
    pathMatch: 'full'
  }
]; 