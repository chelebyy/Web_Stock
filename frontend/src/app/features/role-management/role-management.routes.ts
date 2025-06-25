import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { permissionGuard } from '../../core/guards/permission.guard';

export const ROLE_MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/role-management.component').then(m => m.RoleManagementComponent),
    canActivate: [authGuard, permissionGuard]
  }
]; 