import { Component, OnInit, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { DropdownModule } from 'primeng/dropdown';
import { TooltipModule } from 'primeng/tooltip';
import { firstValueFrom, forkJoin } from 'rxjs';
import { Router } from '@angular/router';

import { UserService } from '../services/user.service';
import { RoleService } from '../../../services/role.service';
import { AuthService } from '../../../core/authentication/auth.service';
import { DeleteConfirmationDialogComponent } from '../../../features/shared/components/delete-confirmation-dialog/delete-confirmation-dialog.component';
import { UserManagementStateService } from '../services/UserManagementStateService';
import { User } from '../../../shared/models/user.model';
import { Role } from '../../../shared/models/role.model';

@Component({
    selector: 'app-user-management',
    templateUrl: './user-management.component.html',
    styleUrls: ['./user-management.component.scss'],
    standalone: true,
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ButtonModule,
        InputTextModule,
        DialogModule,
        DropdownModule,
        ToastModule,
        ToolbarModule,
        TableModule,
        ConfirmDialogModule,
        CheckboxModule,
        PasswordModule,
        TooltipModule,
        DeleteConfirmationDialogComponent
    ],
    providers: [MessageService, ConfirmationService, UserManagementStateService],
    styles: [`
    .modern-dialog {
      border-radius: 12px;
      box-shadow: 0 6px 16px rgba(0, 0, 0, 0.1);
      overflow: hidden;
    }
    
    .modern-dialog .p-dialog-header {
      background-color: #ffffff;
      padding: 1.5rem 2rem 0.5rem;
      border-bottom: none;
    }
    
    .modern-dialog .p-dialog-title {
      font-size: 1.25rem;
      font-weight: 600;
      color: #333;
    }
    
    .modern-dialog .p-dialog-content {
      background-color: #ffffff;
      padding: 2rem;
    }
    
    .dialog-content {
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
    }
    
    .field {
      margin-bottom: 1.5rem;
    }
    
    .input-with-icon {
      position: relative;
      width: 100%;
      display: block;
    }
    
    .input-icon {
      position: absolute;
      left: 14px;
      top: 50%;
      transform: translateY(-50%);
      color: #6c757d;
      font-size: 1.1rem;
      z-index: 5;
    }
    
    .input-with-icon .modern-input,
    .input-with-icon .modern-select {
      padding-left: 42px;
      width: 100%;
      height: 48px;
      border-radius: 8px;
      border: 1px solid #ced4da;
      transition: all 0.3s;
      font-size: 1rem;
      color: #333;
      background-color: #ffffff;
    }
    
    .modern-input:focus,
    .modern-select:focus {
      border-color: #2196F3;
      box-shadow: 0 0 0 2px rgba(33, 150, 243, 0.2);
    }
    
    .modern-input:hover:not(:focus):not(:disabled),
    .modern-select:hover:not(:focus):not(:disabled) {
      border-color: #bbdefb;
    }
    
    .modern-select {
      appearance: none;
      background-image: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="%236c757d" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M6 9l6 6 6-6"/></svg>');
      background-repeat: no-repeat;
      background-position: right 14px center;
      padding-right: 40px;
    }
    
    .p-error {
      color: #f44336;
      font-size: 0.875rem;
      margin-top: 0.25rem;
      display: block;
    }
    
    .p-help {
      color: #6c757d;
      font-size: 0.875rem;
      margin-top: 0.25rem;
      display: flex;
      align-items: center;
    }
    
    .dialog-footer {
      display: flex;
      justify-content: flex-end;
      gap: 0.75rem;
      padding: 1rem 2rem 1.5rem;
      background-color: #ffffff;
    }
    
    .cancel-button, .save-button {
      min-width: 120px;
      height: 40px;
      border-radius: 8px;
      font-weight: 500;
      transition: all 0.3s;
    }
    
    .cancel-button:hover {
      background-color: rgba(0, 0, 0, 0.04);
    }
    
    .save-button:disabled {
      opacity: 0.6;
      cursor: not-allowed;
    }
    
    /* Dropdown özel stilleri */
    .modern-dropdown {
      width: 100% !important;
      height: 48px !important;
      position: relative !important;
    }
    
    .modern-dropdown .p-dropdown {
      width: 100% !important;
      height: 48px !important;
      border-radius: 8px !important;
      background-color: #ffffff !important;
      border: 1px solid #ced4da !important;
      display: flex !important;
      align-items: center !important;
    }
    
    .modern-dropdown .p-dropdown-label {
      padding-left: 42px !important;
      display: flex !important;
      align-items: center !important;
      color: #333333 !important;
      font-size: 1rem !important;
      font-weight: normal !important;
      height: 100% !important;
      background-color: transparent !important;
      z-index: 1 !important;
      position: relative !important;
      visibility: visible !important;
      opacity: 1 !important;
    }
    
    .modern-dropdown .p-dropdown-label.p-placeholder {
      color: #6c757d !important;
      visibility: visible !important;
      opacity: 1 !important;
      display: flex !important;
      align-items: center !important;
    }
    
    .modern-dropdown .p-dropdown-trigger {
      color: #6c757d !important;
      width: 3rem !important;
      z-index: 2 !important;
    }
    
    .input-with-icon .p-dropdown {
      background-color: #ffffff !important;
      position: relative !important;
    }
    
    .input-with-icon .p-dropdown .p-dropdown-label {
      padding-left: 42px !important;
      color: #333333 !important;
      font-size: 1rem !important;
      background-color: #ffffff !important;
      visibility: visible !important;
      opacity: 1 !important;
    }
    
    .input-with-icon .p-dropdown .p-dropdown-label.p-placeholder {
      color: #6c757d !important;
      visibility: visible !important;
      opacity: 1 !important;
    }
    
    /* Dropdown panel stilleri */
    .p-dropdown-panel {
      background-color: #ffffff !important;
      border: 1px solid #ced4da !important;
      border-radius: 8px !important;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
      z-index: 1000 !important;
    }
    
    .p-dropdown-panel .p-dropdown-items {
      padding: 0.5rem 0 !important;
      background-color: #ffffff !important;
    }
    
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item {
      padding: 0.75rem 1rem !important;
      color: #333333 !important;
      font-size: 1rem !important;
      border-bottom: 1px solid rgba(0, 0, 0, 0.05) !important;
      background-color: #ffffff !important;
    }
    
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover {
      background-color: #f8f9fa !important;
      color: #000000 !important;
    }
    
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight {
      background-color: rgba(59, 130, 246, 0.1) !important;
      color: #2563eb !important;
      font-weight: 600 !important;
    }
    
    /* Yüksek öncelikli dropdown panel seçicileri */
    html body .p-dropdown-panel,
    body .p-dropdown-panel,
    .p-dropdown-panel,
    html body .p-dropdown-items-wrapper,
    body .p-dropdown-items-wrapper,
    .p-dropdown-items-wrapper {
      background-color: #ffffff !important;
      border: 1px solid #ced4da !important;
      border-radius: 8px !important;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
    }
    
    html body .p-dropdown-panel .p-dropdown-items,
    body .p-dropdown-panel .p-dropdown-items,
    .p-dropdown-panel .p-dropdown-items,
    html body .p-dropdown-items-wrapper .p-dropdown-items,
    body .p-dropdown-items-wrapper .p-dropdown-items,
    .p-dropdown-items-wrapper .p-dropdown-items {
      background-color: #ffffff !important;
      padding: 0.5rem 0 !important;
    }
    
    html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item,
    body .p-dropdown-panel .p-dropdown-items .p-dropdown-item,
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item,
    html body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item,
    body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item,
    .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item {
      padding: 0.75rem 1rem !important;
      color: #333333 !important;
      font-size: 1rem !important;
      border-bottom: 1px solid rgba(0, 0, 0, 0.05) !important;
      background-color: #ffffff !important;
    }
    
    html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover,
    body .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover,
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover,
    html body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item:hover,
    body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item:hover,
    .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item:hover {
      background-color: #f8f9fa !important;
      color: #000000 !important;
    }
    
    html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight,
    body .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight,
    .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight,
    html body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item.p-highlight,
    body .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item.p-highlight,
    .p-dropdown-items-wrapper .p-dropdown-items .p-dropdown-item.p-highlight {
      background-color: rgba(59, 130, 246, 0.1) !important;
      color: #2563eb !important;
      font-weight: 600 !important;
    }
    
    /* Yüksek öncelikli input seçicileri */
    html body .input-with-icon .p-dropdown .p-dropdown-label,
    body .input-with-icon .p-dropdown .p-dropdown-label,
    .input-with-icon .p-dropdown .p-dropdown-label {
      padding-left: 42px !important;
      color: #333333 !important;
      font-size: 1rem !important;
      visibility: visible !important;
      opacity: 1 !important;
      display: flex !important;
      align-items: center !important;
    }
    
    html body .input-with-icon .p-dropdown .p-dropdown-label.p-placeholder,
    body .input-with-icon .p-dropdown .p-dropdown-label.p-placeholder,
    .input-with-icon .p-dropdown .p-dropdown-label.p-placeholder {
      color: #6c757d !important;
      visibility: visible !important;
      opacity: 1 !important;
      display: flex !important;
      align-items: center !important;
    }
  `]
})
export class UserManagementComponent implements OnInit {
  
  public state = inject(UserManagementStateService);
  
  userDialog = false;
  submitted = false;
  editMode = false;
  userForm!: FormGroup;
  deleteDialogVisible = false;
  userToDelete: User | null = null;
  selectedUsers: User[] = [];
  
  private userService = inject(UserService);
  private roleService = inject(RoleService);
  private messageService = inject(MessageService);
  private confirmationService = inject(ConfirmationService);
  private formBuilder = inject(FormBuilder);
  private router = inject(Router);

  ngOnInit() {
    this.loadInitialData();
    this.initForm();
  }

  loadInitialData() {
    this.loadRoles();
    this.loadUsers();
  }

  async loadUsers() {
    this.state.setLoading(true);
    this.state.setError(null);
    try {
      const users = await firstValueFrom(this.userService.getUsers());
      this.state.setUsers(users);
    } catch (error) {
      const errorMessage = (error as any)?.message || 'Kullanıcılar yüklenemedi.';
      this.state.setError(errorMessage);
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: errorMessage });
    } finally {
      this.state.setLoading(false);
    }
  }

  async loadRoles() {
    try {
      const roles = await firstValueFrom(this.roleService.getRoles());
      this.state.setRoles(roles);
    } catch (error) {
       const errorMessage = (error as any)?.message || 'Roller yüklenemedi.';
       this.messageService.add({ severity: 'error', summary: 'Hata', detail: errorMessage });
    }
  }

  initForm() {
    this.userForm = this.formBuilder.group({
      id: [null],
      fullName: ['', Validators.required],
      sicil: ['', Validators.required],
      password: [''],
      roleId: [null, Validators.required]
    });
  }

  openNewUserDialog() {
    this.editMode = false;
    this.submitted = false;
    this.userForm.reset();
    this.userForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
    this.userForm.updateValueAndValidity();
    this.userDialog = true;
  }

  editUser(user: User) {
    this.editMode = true;
    this.submitted = false;
    this.userForm.reset({
        id: user.id,
        fullName: user.fullName,
        sicil: user.sicil,
        roleId: user.roleId
    });
    this.userForm.get('password')?.clearValidators();
    this.userForm.get('password')?.updateValueAndValidity();
    this.userDialog = true;
  }
  
  deleteUser(user: User) {
    if (user.id === undefined) {
      this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı ID\'si bulunamadı.' });
      return;
    }
    this.confirmationService.confirm({
      message: `${user.fullName} adlı kullanıcıyı silmek istediğinizden emin misiniz?`,
      header: 'Kullanıcı Silme Onayı',
      icon: 'pi pi-exclamation-triangle',
      accept: async () => {
        try {
          await firstValueFrom(this.userService.deleteUser(user.id!));
          this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı silindi.' });
          this.loadUsers(); // Refresh the list
        } catch (error) {
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı silinemedi.' });
        }
      }
    });
  }

  hideDialog() {
    this.userDialog = false;
    this.submitted = false;
  }

  async saveUser() {
    this.submitted = true;
    if (this.userForm.invalid) {
      return;
    }

    const formValue = this.userForm.value;
    
    try {
        if (this.editMode) {
            const updateUserPayload: Partial<User> = {
                id: formValue.id,
                fullName: formValue.fullName,
                sicil: formValue.sicil,
                roleId: formValue.roleId,
                ...(formValue.password && { password: formValue.password })
            };
            await firstValueFrom(this.userService.updateUser(updateUserPayload.id!, updateUserPayload));
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı güncellendi.' });
        } else {
            const createUserPayload: User = {
                id: 0, // Backend will assign ID
                fullName: formValue.fullName,
                sicil: formValue.sicil,
                roleId: formValue.roleId,
                password: formValue.password,
                roleName: '', 
                username: '',
                firstName: '',
                lastName: '',
                isAdmin: false,
                createdAt: new Date(),
                isActive: true
            };
            await firstValueFrom(this.userService.createUser(createUserPayload));
            this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı oluşturuldu.' });
        }
        this.loadUsers();
        this.userDialog = false;
    } catch(error) {
        const errorMessage = (error as any)?.message || 'İşlem başarısız.';
        this.messageService.add({ severity: 'error', summary: 'Hata', detail: errorMessage });
    }
  }

  onSearchTextChange(event: Event) {
    const searchText = (event.target as HTMLInputElement).value;
    this.state.setSearchText(searchText);
  }
  
  onRoleFilterChange(event: Event) {
    const roleId = (event.target as HTMLSelectElement).value;
    const role = this.state.roles().find(r => r.id === +roleId) || null;
    this.state.setSelectedRole(role);
  }
  
  onRowsPerPageChange(event: Event) {
    const rows = +(event.target as HTMLSelectElement).value;
    this.state.setRowsPerPage(rows);
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.state.totalPages()) {
        this.state.setCurrentPage(page);
    }
  }

  goBack() {
    this.router.navigate(['/dashboard']);
  }

  manageUserPermissions(user: User): void {
    this.router.navigate(['/user-permissions', user.id]);
  }
  
  getRoleName(roleId: number | null): string {
    if (roleId === null) return 'N/A';
    return this.state.roles().find(r => r.id === roleId)?.name || 'Bilinmeyen Rol';
  }

  getRoleColorClass(roleName?: string): string {
    if (!roleName) return 'p-badge-secondary';
    switch (roleName.toLowerCase()) {
      case 'admin':
        return 'p-badge-danger';
      case 'kullanıcı':
        return 'p-badge-info';
      default:
        return 'p-badge-secondary';
    }
  }

  // --- Methods for delete confirmation and bulk actions ---

  confirmDelete(): void {
    if (this.userToDelete && this.userToDelete.id !== undefined) {
      // Logic for deleting a single user
      this.userService.deleteUser(this.userToDelete.id).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Kullanıcı silindi' });
          this.loadUsers();
        },
        error: (err: any) => {
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcı silinirken bir hata oluştu' });
        }
      });
    } else if (this.selectedUsers.length > 0) {
      // Logic for bulk deleting selected users
      const usersToDelete = this.selectedUsers.filter(user => user.id !== undefined);
      if (usersToDelete.length === 0) {
        this.hideDeleteDialog();
        return;
      }
      const deleteObservables = usersToDelete.map(user => this.userService.deleteUser(user.id!));
      forkJoin(deleteObservables).subscribe({
        next: () => {
          this.messageService.add({ severity: 'success', summary: 'Başarılı', detail: 'Seçilen kullanıcılar silindi' });
          this.loadUsers(); // Refresh user list
          this.selectedUsers = [];
        },
        error: (err: any) => {
          this.messageService.add({ severity: 'error', summary: 'Hata', detail: 'Kullanıcılar silinirken bir hata oluştu' });
        }
      });
    }
    this.hideDeleteDialog();
  }

  cancelDelete(): void {
    this.hideDeleteDialog();
  }

  deleteSelectedUsers(): void {
    this.userToDelete = null; // Ensure we are in bulk delete mode
    this.deleteDialogVisible = true;
  }

  getSelectedUserCount(): number {
    return this.selectedUsers.length;
  }

  private hideDeleteDialog(): void {
    this.deleteDialogVisible = false;
    this.userToDelete = null;
  }
}
