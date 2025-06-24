import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';

export const ROLE_MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/role-management.component').then(m => m.RoleManagementComponent),
    canActivate: [AuthGuard],
    data: {
      requiresAdmin: true
    }
  }
]; 