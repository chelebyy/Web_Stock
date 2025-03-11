import { Routes } from '@angular/router';
import { DashboardManagementComponent } from './components/dashboard-management.component';
import { AuthGuard } from '../../core/guards/auth.guard';
import { PermissionGuard } from '../../core/guards/permission.guard';

export const DASHBOARD_MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    component: DashboardManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      requiredPermission: 'Pages.DashboardManagement',
      breadcrumb: 'Dashboard Yönetimi'
    }
  }
]; 