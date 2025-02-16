import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ComputersComponent } from './components/inventory/computers/computers.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { 
    path: 'dashboard', 
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'user-management', 
    component: UserManagementComponent,
    canActivate: [AuthGuard],
    data: { requiresAdmin: true }
  },
  {
    path: 'it',
    component: LayoutComponent,
    canActivate: [AuthGuard],
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
  }
];
