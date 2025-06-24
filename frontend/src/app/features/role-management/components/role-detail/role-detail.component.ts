import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Role } from '../../../../shared/models/role.model';
import { RoleService } from '../../services/role.service';
import { PermissionService } from '../../../../services/permission.service';
import { Permission } from '../../../../shared/models/permission.model';
import { catchError, forkJoin, of } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';

@Component({
    selector: 'app-role-detail',
    templateUrl: './role-detail.component.html',
    styleUrls: ['./role-detail.component.scss'],
    imports: [
        CommonModule,
        ButtonModule,
        ProgressSpinnerModule,
        ToastModule
    ]
})
export class RoleDetailComponent implements OnInit {
  roleId = 0;
  role: Role | null = null;
  loading = true;
  loadError = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private roleService: RoleService,
    private permissionService: PermissionService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.roleId = +params['roleId'];
      this.loadRoleData();
    });
  }

  loadRoleData(): void {
    this.loading = true;
    this.loadError = false;

    // Rol verilerini yükle
    this.roleService.getRoleById(this.roleId).pipe(
      catchError(error => {
        console.error('Rol yüklenirken hata:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: `Rol bilgileri yüklenemedi: ${error.message || 'Bilinmeyen hata'}`,
          life: 5000
        });
        this.loadError = true;
        return of(null);
      })
    ).subscribe({
      next: (data) => {
        this.role = data;
        
        if (!this.role) {
          this.loadError = true;
        }
        
        this.loading = false;
      },
      error: (error: any) => {
        console.error('Veri yüklenirken hata:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: 'Veriler yüklenirken bir hata oluştu',
          life: 5000
        });
        this.loadError = true;
        this.loading = false;
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/roles']);
  }
} 