import { Routes } from '@angular/router';

export const DASHBOARD_ROUTES: Routes = [
  {
    path: 'admin-dashboard',
    loadComponent: () => import('./components/admin-dashboard.component').then(m => m.AdminDashboardComponent)
  },
  {
    path: 'user-dashboard',
    loadComponent: () => import('./components/user-dashboard.component').then(m => m.UserDashboardComponent)
  },
  {
    path: '',
    redirectTo: 'user-dashboard',
    pathMatch: 'full'
  }
]; 