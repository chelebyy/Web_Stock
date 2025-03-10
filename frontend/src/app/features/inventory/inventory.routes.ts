import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';
import { PermissionGuard } from '../../core/guards/permission.guard';

export const INVENTORY_ROUTES: Routes = [
  {
    path: 'computers',
    loadComponent: () => import('./components/computers/computers.component').then(m => m.ComputersComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.Computers']
    }
  },
  {
    path: '',
    redirectTo: 'computers',
    pathMatch: 'full'
  }
]; 