import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { DropdownModule } from 'primeng/dropdown';
import { Textarea } from 'primeng/inputtextarea';
import { TooltipModule } from 'primeng/tooltip';
import { CardModule } from 'primeng/card';
import { RippleModule } from 'primeng/ripple';
import { RoleService } from '../services/role.service';
import { Role, RoleWithUsers } from '../../../shared/models/role.model';
import { Router, RouterModule } from '@angular/router';
import { signal, computed } from '@angular/core';
import { DeleteConfirmationDialogComponent } from '../../../features/shared/components/delete-confirmation-dialog/delete-confirmation-dialog.component';

@Component({
    selector: 'app-role-management',
    templateUrl: './role-management.component.html',
    styleUrls: ['./role-management.component.scss'],
    standalone: true,
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
  // Direkt servislerden sinyalleri kullan
  public roleService = inject(RoleService);
  
  // Signal state değişkenleri
  searchText = signal<string>('');
  roleDialog = signal<boolean>(false);
  editMode = signal<boolean>(false);
  selectedRole: Role | null = null;
  
  // Silme dialogu için yeni değişkenler
  deleteDialogVisible: boolean = false;
  roleToDelete: Role | null = null;
  
  // Hesaplanmış değerler
  filteredRoles = computed(() => {
    const search = this.searchText().toLowerCase();
    if (!search) return this.roleService.roles();
    
    return this.roleService.roles().filter(role => 
      role.name.toLowerCase().includes(search) || 
      role.description.toLowerCase().includes(search)
    );
  });
  
  roleForm: FormGroup;
  
  constructor(
    private messageService: MessageService,
    public confirmationService: ConfirmationService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.roleForm = this.fb.group({
      id: [null],
      name: ['', [Validators.required]],
      description: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    // Rol verilerini Signal API üzerinden yükle
    this.roleService.loadRoles();
  }

  openDialog(role?: Role) {
    this.editMode.set(!!role);
    
    if (role) {
      this.roleForm.patchValue({
        id: role.id,
        name: role.name,
        description: role.description
      });
    } else {
      this.roleForm.reset({
        id: null,
        name: '',
        description: ''
      });
    }
    
    // Dialog görünürlüğünü açıkça true olarak ayarla
    this.roleDialog.set(true);
  }

  hideDialog() {
    this.roleDialog.set(false);
    this.roleForm.reset();
  }

  saveRole() {
    if (this.roleForm.invalid) {
      Object.keys(this.roleForm.controls).forEach(key => {
        const control = this.roleForm.get(key);
        if (control?.invalid) {
          control.markAsTouched();
        }
      });
      return;
    }

    const roleData = {
      id: this.roleForm.get('id')?.value,
      name: this.roleForm.get('name')?.value,
      description: this.roleForm.get('description')?.value
    };
    
    if (this.editMode()) {
      this.roleService.updateRole(roleData.id, roleData).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Rol güncellendi'
          });
          this.roleService.loadRoles();
          this.hideDialog();
        },
        error: (error) => {
          console.error('Error updating role:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Rol güncellenirken bir hata oluştu'
          });
        }
      });
    } else {
      this.roleService.createRole(roleData).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Rol oluşturuldu'
          });
          this.roleService.loadRoles();
          this.hideDialog();
        },
        error: (error) => {
          console.error('Error creating role:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Hata',
            detail: 'Rol oluşturulurken bir hata oluştu'
          });
        }
      });
    }
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
          detail: 'Rol silindi'
        });
        this.roleService.loadRoles();
        this.roleToDelete = null;
      },
      error: (error) => {
        console.error('Error deleting role:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Rol silinirken bir hata oluştu'
        });
        this.roleToDelete = null;
      }
    });
  }
  
  cancelDelete() {
    this.roleToDelete = null;
  }
  
  managePermissions(role: Role) {
    this.router.navigate([`/role-management/detail/${role.id}`]);
  }

  filterRoles(searchText: string): void {
    this.searchText.set(searchText);
  }

  /**
   * Önceki sayfaya veya dashboard'a geri döner
   */
  goBack(): void {
    this.router.navigate(['/dashboard/admin-dashboard']);
  }
} 