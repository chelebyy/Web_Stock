import { Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';
import { PermissionGuard } from '../../core/guards/permission.guard';

export const BILGI_ISLEM_ROUTES: Routes = [
  {
    path: 'bilgi-islem',
    loadComponent: () => import('./components/bilgi-islem.component').then(m => m.BilgiIslemComponent),
    canActivate: [AuthGuard, PermissionGuard],
    data: { 
      permissions: ['Pages.BilgiIslem.View']
    }
  },
  {
    path: '',
    redirectTo: 'bilgi-islem',
    pathMatch: 'full'
  }
]; 