import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { RoleDetailComponent } from './components/role-management/role-detail/role-detail.component';

const routes: Routes = [
  { 
    path: 'roles/:id', 
    component: RoleDetailComponent, 
    canActivate: [AuthGuard], 
    data: { requiresAdmin: true, requiredPermission: 'Roles.View' } 
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { } 