import { NgModule } from '@angular/core';
import { RoleDetailComponent } from './components/role-management/role-detail/role-detail.component';
import { HasPermissionDirective } from './directives/has-permission.directive';
import { PermissionService } from './services/permission.service';

@NgModule({
  declarations: [
    RoleDetailComponent,
  ],
  imports: [
    HasPermissionDirective,
  ],
  providers: [
    PermissionService,
  ],
})
export class AppModule { } 