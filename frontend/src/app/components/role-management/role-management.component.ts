import { Component, OnInit } from '@angular/core';
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
import { RoleService } from '../../services/role.service';
import { Role } from '../../models/role.model';

@Component({
  selector: 'app-role-management',
  templateUrl: './role-management.component.html',
  styleUrls: ['./role-management.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    ConfirmDialogModule,
    DialogModule,
    InputTextModule,
    TableModule,
    ToastModule,
    ToolbarModule
  ],
  providers: [ConfirmationService, MessageService]
})
export class RoleManagementComponent implements OnInit {
  roles: Role[] = [];
  roleDialog: boolean = false;
  roleForm: FormGroup;
  editMode: boolean = false;
  loading: boolean = false;

  constructor(
    private roleService: RoleService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private fb: FormBuilder
  ) {
    this.roleForm = this.fb.group({
      id: [null],
      name: ['', [Validators.required]],
      description: ['']
    });
  }

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles() {
    this.loading = true;
    this.roleService.getRoles().subscribe({
      next: (data) => {
        console.log('Loaded roles:', data);
        this.roles = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading roles:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Roller yüklenirken bir hata oluştu'
        });
        this.loading = false;
      }
    });
  }

  openDialog(role?: Role) {
    this.editMode = !!role;
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
    this.roleDialog = true;
  }

  hideDialog() {
    this.roleDialog = false;
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

    const roleData = this.roleForm.value;
    
    if (this.editMode) {
      this.roleService.updateRole(roleData.id, roleData).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Başarılı',
            detail: 'Rol güncellendi'
          });
          this.loadRoles();
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
          this.loadRoles();
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
    this.confirmationService.confirm({
      message: 'Bu rolü silmek istediğinizden emin misiniz?',
      header: 'Onay',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.roleService.deleteRole(role.id).subscribe({
          next: () => {
            this.messageService.add({
              severity: 'success',
              summary: 'Başarılı',
              detail: 'Rol silindi'
            });
            this.loadRoles();
          },
          error: (error) => {
            console.error('Error deleting role:', error);
            this.messageService.add({
              severity: 'error',
              summary: 'Hata',
              detail: 'Rol silinirken bir hata oluştu'
            });
          }
        });
      }
    });
  }
} 