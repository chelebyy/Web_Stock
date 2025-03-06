import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { RoleManagementComponent } from './components/role-management/role-management.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ComputersComponent } from './components/inventory/computers/computers.component';
import { AuthGuard } from './guards/auth.guard';
import { PermissionGuard } from './guards/permission.guard';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { RoleDetailComponent } from './components/role-management/role-detail/role-detail.component';
import { PermissionManagementComponent } from './components/permission-management/permission-management.component';
import { UserPagePermissionsComponent } from './components/user-page-permissions/user-page-permissions.component';
import { RevirComponent } from './components/revir/revir.component';
import { BilgiIslemComponent } from './components/bilgi-islem/bilgi-islem.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { 
    path: 'admin-dashboard', 
    component: AdminDashboardComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.AdminDashboard']
    }
  },
  { 
    path: 'user-dashboard', 
    component: UserDashboardComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Pages.UserDashboard']
    }
  },
  { 
    path: 'user-management', 
    component: UserManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.UserManagement']
    }
  },
  { 
    path: 'admin/users', 
    component: UserManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.UserManagement']
    }
  },
  { 
    path: 'role-management', 
    component: RoleManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.RoleManagement']
    }
  },
  { 
    path: 'roles', 
    component: RoleManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.RoleManagement']
    }
  },
  { 
    path: 'admin/roles', 
    component: RoleManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.RoleManagement']
    }
  },
  { 
    path: 'roles/:id', 
    component: RoleDetailComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Pages.RoleManagement']
    }
  },
  {
    path: 'bilgi-islem',
    component: BilgiIslemComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Pages.BilgiIslem.View']
    }
  },
  {
    path: 'revir',
    component: RevirComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['Pages.Revir.View']
    }
  },
  {
    path: 'it',
    component: LayoutComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: {
      permissions: ['IT.Access', 'Pages.StockManagement']
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
    path: 'access-denied',
    component: AdminDashboardComponent,
  },
  { 
    path: 'roles/:roleId/permissions', 
    component: PermissionManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Roles.Update']
    }
  },
  { 
    path: 'users/:userId/permissions', 
    component: PermissionManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Users.Permissions.Manage']
    }
  },
  { 
    path: 'users/:userId/page-permissions', 
    component: UserPagePermissionsComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiresAdmin: true,
      permissions: ['Users.Permissions.Manage']
    }
  }
];
