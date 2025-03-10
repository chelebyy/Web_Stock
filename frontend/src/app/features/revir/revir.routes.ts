import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';
import { PermissionGuard } from '../../core/guards/permission.guard';

export const REVIR_ROUTES: Routes = [
  {
    path: 'revir',
    loadComponent: () => import('./components/revir.component').then(m => m.RevirComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.Revir']
    }
  },
  {
    path: '',
    redirectTo: 'revir',
    pathMatch: 'full'
  }
]; 