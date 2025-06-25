import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { permissionGuard } from '../../core/guards/permission.guard';

export const REVIR_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/revir.component').then(m => m.RevirComponent),
    canActivate: [authGuard, permissionGuard],
    data: { 
      requiredPermission: 'Pages.Revir'
    }
  }
]; 