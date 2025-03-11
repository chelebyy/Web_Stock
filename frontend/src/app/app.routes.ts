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
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES),
    canActivate: [AuthGuard]
  },
  
  // User Management Module - Lazy Loading
  {
    path: 'user-management',
    loadChildren: () => import('./features/user-management/user-management.routes').then(m => m.USER_MANAGEMENT_ROUTES),
    canActivate: [AuthGuard]
  },
  
  // Role Management Module - Lazy Loading
  {
    path: 'role-management',
    loadChildren: () => import('./features/role-management/role-management.routes').then(m => m.ROLE_MANAGEMENT_ROUTES),
    canActivate: [AuthGuard],
    data: {
      requiresAdmin: true
    }
  },
  
  // Dashboard Management Module - Lazy Loading
  {
    path: 'dashboard-management',
    loadChildren: () => import('./features/dashboard-management/dashboard-management.routes').then(m => m.DASHBOARD_MANAGEMENT_ROUTES),
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      requiredPermission: 'Pages.DashboardManagement'
    }
  },
  
  // Bilgi İşlem Module - Lazy Loading
  {
    path: 'bilgi-islem',
    loadChildren: () => import('./features/bilgi-islem/bilgi-islem.routes').then(m => m.BILGI_ISLEM_ROUTES),
    canActivate: [AuthGuard]
  },
  
  // Revir Module - Lazy Loading
  {
    path: 'revir',
    loadChildren: () => import('./features/revir/revir.routes').then(m => m.REVIR_ROUTES),
    canActivate: [AuthGuard]
  },
  
  // Inventory Module - Lazy Loading
  {
    path: 'inventory',
    loadChildren: () => import('./features/inventory/inventory.routes').then(m => m.INVENTORY_ROUTES),
    canActivate: [AuthGuard]
  },
  
  // Catch-all route - Kullanıcı giriş yapmışsa dashboard'a, yapmamışsa login'e yönlendir
  { 
    path: '**', 
    redirectTo: '/login'
  }
];
