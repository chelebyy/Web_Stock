import { Routes } from '@angular/router';
import { DashboardManagementComponent } from './components/dashboard-management.component';
import { UserPagePermissionsComponent } from './components/user-page-permissions.component';

export const DASHBOARD_MANAGEMENT_ROUTES: Routes = [
  {
    path: '',
    component: DashboardManagementComponent
  },
  {
    path: 'permissions/:pageId/:pageName',
    component: UserPagePermissionsComponent
  }
]; 