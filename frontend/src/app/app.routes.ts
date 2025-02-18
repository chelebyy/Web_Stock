import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { RoleManagementComponent } from './components/role-management/role-management.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ComputersComponent } from './components/inventory/computers/computers.component';
import { ITDashboardComponent } from './components/it-dashboard/it-dashboard.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { 
    path: '', 
    component: DashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { 
        path: 'user-management', 
        component: UserManagementComponent,
        data: { requiresAdmin: true }
      },
      { 
        path: 'role-management', 
        component: RoleManagementComponent,
        data: { requiresAdmin: true }
      }
    ]
  },
  {
    path: 'bilgi-islem',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', component: ITDashboardComponent },
      { 
        path: 'envanter',
        children: [
          { path: 'bilgisayarlar', component: ComputersComponent },
          { path: 'yazicilar', component: ComputersComponent },
          { path: 'ag-cihazlari', component: ComputersComponent }
        ]
      },
      {
        path: 'stok',
        children: [
          { path: 'sarf-malzemeler', component: ComputersComponent },
          { path: 'yedek-parcalar', component: ComputersComponent }
        ]
      },
      {
        path: 'isler',
        children: [
          { path: 'aktif', component: ComputersComponent },
          { path: 'tamamlanan', component: ComputersComponent }
        ]
      },
      {
        path: 'ayarlar',
        children: [
          { path: 'genel', component: ComputersComponent },
          { path: 'kategoriler', component: ComputersComponent }
        ]
      }
    ]
  },
  { path: '**', redirectTo: '' }
];
