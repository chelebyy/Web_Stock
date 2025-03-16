import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// PrimeNG Modules
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CardModule } from 'primeng/card';
import { PaginatorModule } from 'primeng/paginator';
import { CheckboxModule } from 'primeng/checkbox';
import { RadioButtonModule } from 'primeng/radiobutton';
import { CalendarModule } from 'primeng/calendar';
import { InputSwitchModule } from 'primeng/inputswitch';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { TooltipModule } from 'primeng/tooltip';
import { RippleModule } from 'primeng/ripple';
import { MenuModule } from 'primeng/menu';
import { TabViewModule } from 'primeng/tabview';
import { FileUploadModule } from 'primeng/fileupload';
import { MessageModule } from 'primeng/message';
import { MessagesModule } from 'primeng/messages';
import { ToolbarModule } from 'primeng/toolbar';
import { SplitButtonModule } from 'primeng/splitbutton';
import { InputNumberModule } from 'primeng/inputnumber';
import { MultiSelectModule } from 'primeng/multiselect';

// Shared Components
import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';
import { ErrorMessageComponent } from './components/error-message/error-message.component';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    
    // PrimeNG Modules
    ButtonModule,
    TableModule,
    InputTextModule,
    DropdownModule,
    DialogModule,
    ToastModule,
    ConfirmDialogModule,
    CardModule,
    PaginatorModule,
    CheckboxModule,
    RadioButtonModule,
    CalendarModule,
    InputSwitchModule,
    ProgressSpinnerModule,
    TooltipModule,
    RippleModule,
    MenuModule,
    TabViewModule,
    FileUploadModule,
    MessageModule,
    MessagesModule,
    ToolbarModule,
    SplitButtonModule,
    InputNumberModule,
    MultiSelectModule,
    
    // Standalone Components
    LoadingSpinnerComponent,
    ErrorMessageComponent,
    ConfirmationDialogComponent
  ],
  exports: [
    // Angular Modules
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    
    // PrimeNG Modules
    ButtonModule,
    TableModule,
    InputTextModule,
    DropdownModule,
    DialogModule,
    ToastModule,
    ConfirmDialogModule,
    CardModule,
    PaginatorModule,
    CheckboxModule,
    RadioButtonModule,
    CalendarModule,
    InputSwitchModule,
    ProgressSpinnerModule,
    TooltipModule,
    RippleModule,
    MenuModule,
    TabViewModule,
    FileUploadModule,
    MessageModule,
    MessagesModule,
    ToolbarModule,
    SplitButtonModule,
    InputNumberModule,
    MultiSelectModule,
    
    // Standalone Components
    LoadingSpinnerComponent,
    ErrorMessageComponent,
    ConfirmationDialogComponent
  ]
})
export class SharedModule { } 