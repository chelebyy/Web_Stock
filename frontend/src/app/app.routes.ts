import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { permissionGuard } from './core/guards/permission.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  
  // No-Layout Routes (Login)
  {
    path: 'login',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  
  // Main Layout Routes
  {
    path: 'app', // 'app' gibi bir ön ek ile gruplama
    loadComponent: () => import('./components/layout/layout.component').then(c => c.LayoutComponent),
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES),
      },
      {
        path: 'bilgi-islem',
        loadChildren: () => import('./features/bilgi-islem/bilgi-islem.routes').then(m => m.BILGI_ISLEM_ROUTES),
      },
      
      // Revir Module - Lazy Loading
      {
        path: 'revir',
        loadChildren: () => import('./features/revir/revir.routes').then(m => m.REVIR_ROUTES),
      },
      
      // Inventory Module - Lazy Loading
      {
        path: 'inventory',
        loadChildren: () => import('./features/inventory/inventory.routes').then(m => m.INVENTORY_ROUTES),
      },
    ]
  },
  
  // Admin (Layout-less) Routes
  {
    path: 'admin',
    canActivate: [authGuard],
    children: [
      {
        path: 'users',
        loadChildren: () => import('./features/user-management/user-management.routes').then(m => m.USER_MANAGEMENT_ROUTES),
        data: { requiredPermission: 'Pages.UserManagement' }
      },
      {
        path: 'roles',
        loadChildren: () => import('./features/role-management/role-management.routes').then(m => m.ROLE_MANAGEMENT_ROUTES),
      },
      // ... diğer admin rotaları
    ]
  },
  
  // Access Denied and Catch-all
  {
    path: 'access-denied',
    loadComponent: () => import('./components/access-denied/access-denied.component').then(c => c.AccessDeniedComponent)
  },
  { 
    path: '**', 
    redirectTo: 'login' // Veya 'app/dashboard'
  }
];
