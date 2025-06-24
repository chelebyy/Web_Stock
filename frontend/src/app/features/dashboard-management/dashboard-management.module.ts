import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// PrimeNG Modülleri
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';
import { TextareaModule } from 'primeng/textarea';
import { InputSwitchModule } from 'primeng/inputswitch';
import { DividerModule } from 'primeng/divider';
import { TooltipModule } from 'primeng/tooltip';
import { TabViewModule } from 'primeng/tabview';

// Servisler
import { MessageService } from 'primeng/api';

// Bileşenler
import { DashboardManagementComponent } from './components/dashboard-management.component';
import { UserPagePermissionsComponent } from './components/user-page-permissions.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    DropdownModule,
    CardModule,
    ToastModule,
    TagModule,
    RippleModule,
    TextareaModule,
    InputSwitchModule,
    DividerModule,
    TooltipModule,
    TabViewModule,
    DashboardManagementComponent,
    UserPagePermissionsComponent
  ],
  providers: [
    MessageService
  ]
})
export class DashboardManagementModule { } 