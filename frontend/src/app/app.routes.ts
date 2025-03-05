import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { RoleManagementComponent } from './components/role-management/role-management.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ComputersComponent } from './components/inventory/computers/computers.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { RoleDetailComponent } from './components/role-management/role-detail/role-detail.component';
import { UserPermissionComponent } from './components/user-permission/user-permission.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { 
    path: 'admin-dashboard', 
    component: AdminDashboardComponent,
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true,
      requiredPermission: 'Pages.AdminDashboard'
    }
  },
  { 
    path: 'user-dashboard', 
    component: UserDashboardComponent,
    canActivate: [AuthGuard],
    data: {
      requiredPermission: 'Pages.UserDashboard'
    }
  },
  { 
    path: 'user-management', 
    component: UserManagementComponent,
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true,
      requiredPermission: 'Pages.UserManagement'
    }
  },
  { 
    path: 'admin/users', 
    component: UserManagementComponent,
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true,
      requiredPermission: 'Pages.UserManagement'
    }
  },
  { 
    path: 'role-management', 
    component: RoleManagementComponent,
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true,
      requiredPermission: 'Pages.RoleManagement'
    }
  },
  { 
    path: 'roles', 
    component: RoleManagementComponent,
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true,
      requiredPermission: 'Pages.RoleManagement'
    }
  },
  { 
    path: 'admin/roles', 
    component: RoleManagementComponent,
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true,
      requiredPermission: 'Pages.RoleManagement'
    }
  },
  { 
    path: 'roles/:id', 
    component: RoleDetailComponent,
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true,
      requiredPermission: 'Pages.RoleManagement'
    }
  },
  {
    path: 'it',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    data: {
      requiredPermission: 'Pages.StockManagement'
    },
    children: [
      { path: '', redirectTo: 'inventory/computers', pathMatch: 'full' },
      { path: 'inventory/computers', component: ComputersComponent },
      { path: 'inventory/printers', component: ComputersComponent },
      { path: 'inventory/network', component: ComputersComponent },
      { path: 'stock/consumables', component: ComputersComponent },
      { path: 'stock/spare-parts', component: ComputersComponent },
      { path: 'tasks/active', component: ComputersComponent },
      { path: 'tasks/completed', component: ComputersComponent },
      { path: 'settings/general', component: ComputersComponent },
      { path: 'settings/categories', component: ComputersComponent }
    ]
  },
  { 
    path: 'users/:id/permissions', 
    component: UserPermissionComponent,
    canActivate: [AuthGuard],
    data: { 
      requiresAdmin: true,
      requiredPermission: 'Pages.UserManagement'
    },
    providers: [
      // Gerekli servis sağlayıcılar burada belirtilebilir
    ]
  }
];
