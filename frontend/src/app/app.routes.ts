import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { PermissionGuard } from './core/guards/permission.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  
  // Auth Module - Lazy Loading
  {
    path: 'login',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  
  // Dashboard Module - Lazy Loading
  {
    path: '',
    loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES)
  },
  
  // User Management Module - Lazy Loading
  {
    path: '',
    loadChildren: () => import('./features/user-management/user-management.routes').then(m => m.USER_MANAGEMENT_ROUTES)
  },
  
  // Role Management Module - Lazy Loading
  {
    path: '',
    loadChildren: () => import('./features/role-management/role-management.routes').then(m => m.ROLE_MANAGEMENT_ROUTES)
  },
  
  // Bilgi İşlem Module - Lazy Loading
  {
    path: '',
    loadChildren: () => import('./features/bilgi-islem/bilgi-islem.routes').then(m => m.BILGI_ISLEM_ROUTES)
  },
  
  // Revir Module - Lazy Loading
  {
    path: '',
    loadChildren: () => import('./features/revir/revir.routes').then(m => m.REVIR_ROUTES)
  },
  
  // Inventory Module - Lazy Loading
  {
    path: '',
    loadChildren: () => import('./features/inventory/inventory.routes').then(m => m.INVENTORY_ROUTES)
  },
  
  // Catch-all route
  { path: '**', redirectTo: '/login' }
];
