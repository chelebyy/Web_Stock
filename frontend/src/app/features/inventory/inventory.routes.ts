import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { permissionGuard } from '../../core/guards/permission.guard';

export const INVENTORY_ROUTES: Routes = [
  {
    path: 'computers',
    loadComponent: () => import('./components/computers/computers.component').then(m => m.ComputersComponent),
    canActivate: [authGuard, permissionGuard],
    data: { 
      requiredPermission: 'Pages.Computers'
    }
  },
  {
    path: '',
    redirectTo: 'computers',
    pathMatch: 'full'
  }
]; 