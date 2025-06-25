import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { permissionGuard } from './core/guards/permission.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  
  // Auth Module (Login) - No guard needed
  {
    path: 'login',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  
  // Dashboard Module - Lazy Loading
  {
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES),
    canActivate: [authGuard]
  },
  
  // User Management Module - Lazy Loading
  {
    path: 'user-management',
    loadChildren: () => import('./features/user-management/user-management.routes').then(m => m.USER_MANAGEMENT_ROUTES),
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPermission: 'Pages.UserManagement' // Örnek bir izin adı
    }
  },
  
  // Role Management Module - Lazy Loading
  // permissionGuard, admin kontrolünü zaten yapıyor.
  {
    path: 'role-management',
    loadChildren: () => import('./features/role-management/role-management.routes').then(m => m.ROLE_MANAGEMENT_ROUTES),
    canActivate: [authGuard, permissionGuard]
  },
  
  // Dashboard Management Module - Lazy Loading
  {
    path: 'dashboard-management',
    loadChildren: () => import('./features/dashboard-management/dashboard-management.routes').then(m => m.DASHBOARD_MANAGEMENT_ROUTES),
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPermission: 'Pages.DashboardManagement'
    }
  },
  
  // Bilgi İşlem Module - Lazy Loading
  {
    path: 'bilgi-islem',
    loadChildren: () => import('./features/bilgi-islem/bilgi-islem.routes').then(m => m.BILGI_ISLEM_ROUTES),
    canActivate: [authGuard]
  },
  
  // Revir Module - Lazy Loading
  {
    path: 'revir',
    loadChildren: () => import('./features/revir/revir.routes').then(m => m.REVIR_ROUTES),
    canActivate: [authGuard]
  },
  
  // Inventory Module - Lazy Loading
  {
    path: 'inventory',
    loadChildren: () => import('./features/inventory/inventory.routes').then(m => m.INVENTORY_ROUTES),
    canActivate: [authGuard]
  },
  
  // Access Denied Page
  {
    path: 'access-denied',
    loadComponent: () => import('./components/access-denied/access-denied.component').then(c => c.AccessDeniedComponent)
  },

  // Catch-all route
  { 
    path: '**', 
    redirectTo: '/login' // veya daha mantıklı bir '404 not found' sayfasına
  }
];
