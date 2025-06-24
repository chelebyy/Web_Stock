import { Component, OnInit, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { DropdownModule } from 'primeng/dropdown';
import { Textarea } from 'primeng/inputtextarea';
import { TooltipModule } from 'primeng/tooltip';
import { CardModule } from 'primeng/card';
import { RippleModule } from 'primeng/ripple';
import { RoleService } from '../services/role.service';
import { Role } from '../../../shared/models/role.model';
import { Router, RouterModule } from '@angular/router';
import { signal } from '@angular/core';
import { DeleteConfirmationDialogComponent } from '../../../features/shared/components/delete-confirmation-dialog/delete-confirmation-dialog.component';
import { RoleStateService } from '../services/role-state.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
    selector: 'app-role-management',
    templateUrl: './role-management.component.html',
    styleUrls: ['./role-management.component.scss'],
    standalone: true,
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        ButtonModule,
        ConfirmDialogModule,
        DialogModule,
        InputTextModule,
        Textarea,
        TableModule,
        ToastModule,
        ToolbarModule,
        DropdownModule,
        TooltipModule,
        CardModule,
        RippleModule,
        DeleteConfirmationDialogComponent
    ],
    providers: [ConfirmationService, MessageService]
})
export class RoleManagementComponent implements OnInit {
  public roleService = inject(RoleService);
  public state = inject(RoleStateService);
  
  roleDialog = signal<boolean>(false);
  editMode = signal<boolean>(false);
  
  deleteDialogVisible = false;
  roleToDelete: Role | null = null;
  
  roleForm: FormGroup;
  
  private lastLazyLoadEvent: TableLazyLoadEvent = { first: 0, rows: 10 };
  private searchSubject = new Subject<string>();

  constructor(
    private messageService: MessageService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.roleForm = this.fb.group({
      id: [null],
      name: ['', [Validators.required]],
      description: ['', [Validators.required]]
    });

    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(searchValue => {
      this.lastLazyLoadEvent.globalFilter = searchValue;
      this.lastLazyLoadEvent.first = 0; // Reset to first page
      this.loadRoles(this.lastLazyLoadEvent);
    });
  }

  ngOnInit(): void {
    // Initial load will be triggered by the table's onLazyLoad event
  }

  loadRoles(event?: TableLazyLoadEvent) {
    // If event is provided, update the last known event
    if (event) {
      this.lastLazyLoadEvent = event;
    }
    
    const pageNumber = (this.lastLazyLoadEvent.first ?? 0) / (this.lastLazyLoadEvent.rows ?? 10) + 1;
    const pageSize = this.lastLazyLoadEvent.rows ?? 10;
    const sortField = this.lastLazyLoadEvent.sortField as string | undefined;
    const sortOrder = this.lastLazyLoadEvent.sortOrder === 1 ? 'asc' : 'desc';
    const globalFilter = (this.lastLazyLoadEvent.globalFilter as string) || undefined;

    this.roleService.loadRoles(pageNumber, pageSize, sortField, sortOrder, globalFilter);
  }

  onLazyLoad(event: TableLazyLoadEvent) {
    this.loadRoles(event);
  }

  onSearch(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    this.searchSubject.next(inputElement.value);
  }

  openDialog(role?: Role) {
    this.editMode.set(!!role);
    
    if (role) {
      this.roleForm.patchValue(role);
    } else {
      this.roleForm.reset();
    }
    
    this.roleDialog.set(true);
  }

  hideDialog() {
    this.roleDialog.set(false);
    this.roleForm.reset();
  }

  saveRole() {
    if (this.roleForm.invalid) return;
    
    const formValue = this.roleForm.value;
    
    const operation$ = this.editMode()
      ? this.roleService.updateRole(formValue.id, formValue)
      : this.roleService.createRole(formValue);

    operation$.subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: `Rol başarıyla ${this.editMode() ? 'güncellendi' : 'oluşturuldu'}`,
          life: 3000
        });
        this.hideDialog();
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `Rol ${this.editMode() ? 'güncellenirken' : 'oluşturulurken'} bir hata oluştu: ${error.message || 'Bilinmeyen hata'}`,
          life: 5000
        });
      }
    });
  }

  deleteRole(role: Role) {
    this.roleToDelete = role;
    this.deleteDialogVisible = true;
  }
  
  confirmDelete() {
    if (!this.roleToDelete) return;
    
    this.roleService.deleteRole(this.roleToDelete.id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: `${this.roleToDelete?.name} rolü başarıyla silindi`,
          life: 3000
        });
        this.roleToDelete = null;
        this.deleteDialogVisible = false;
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `Rol silinirken bir hata oluştu: ${error.message || 'Bilinmeyen hata'}`,
          life: 5000
        });
        this.deleteDialogVisible = false;
      }
    });
  }
  
  cancelDelete() {
    this.roleToDelete = null;
    this.deleteDialogVisible = false;
  }
  
  goBack(): void {
    this.router.navigate(['/dashboard/admin-dashboard']);
  }
} 