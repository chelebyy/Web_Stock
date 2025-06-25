import { Routes } from '@angular/router';
import { authGuard } from '../../core/guards/auth.guard';
import { permissionGuard } from '../../core/guards/permission.guard';

export const BILGI_ISLEM_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/bilgi-islem.component').then(m => m.BilgiIslemComponent),
    canActivate: [authGuard, permissionGuard],
    data: { 
      requiredPermission: 'Pages.BilgiIslem.View'
    }
  }
]; 