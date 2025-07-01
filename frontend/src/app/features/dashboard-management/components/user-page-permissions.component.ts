import { Component, OnInit, inject, signal, computed, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { InputTextModule } from 'primeng/inputtext';
import { TooltipModule } from 'primeng/tooltip';
import { RippleModule } from 'primeng/ripple';
import { MessageService } from 'primeng/api';
import { DashboardPermissionStateService } from '../services/dashboard-permission-state.service';
import { DashboardUser, PagePermission } from '../services/dashboard-permission.service';

@Component({
  selector: 'app-user-page-permissions',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    ToastModule,
    InputTextModule,
    TooltipModule,
    RippleModule,
    RouterModule
  ],
  providers: [MessageService, DashboardPermissionStateService],
  templateUrl: './user-page-permissions.component.html',
  styleUrls: ['./user-page-permissions.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserPagePermissionsComponent implements OnInit {
  public state = inject(DashboardPermissionStateService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  pageId = 0;
  pageName = '';
  
  activePage = signal('users');
  usersSearchText = signal('');
  permissionsSearchText = signal('');

  // Computed signals for filtering
  filteredUsers = computed(() => {
    const searchTerm = this.usersSearchText().toLowerCase();
    if (!searchTerm) return this.state.users();
    return this.state.users().filter(user =>
      user.fullName.toLowerCase().includes(searchTerm) ||
      user.sicil.toLowerCase().includes(searchTerm)
    );
  });

  filteredPermissions = computed(() => {
    const searchTerm = this.permissionsSearchText().toLowerCase();
    if (!searchTerm) return this.state.permissions();
    return this.state.permissions().filter(p =>
      p.fullName.toLowerCase().includes(searchTerm) ||
      p.sicil.toLowerCase().includes(searchTerm)
    );
  });

  constructor() {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.pageId = +params['pageId'];
      this.pageName = params['pageName'] || 'Bilinmeyen Sayfa';
      
      if (isNaN(this.pageId) || this.pageId <= 0) {
        // Error handling can be enhanced via state service if needed
        this.router.navigate(['/dashboard']);
        return;
      }
      
      this.state.loadInitialData(this.pageId);
    });
  }

  grantPermission(user: DashboardUser): void {
    if (!user.id || user.hasPagePermission || !user.isActive) return;
    this.state.grantPermission(this.pageId, user);
  }

  revokePermission(permission: PagePermission): void {
    this.state.revokePermission(this.pageId, permission);
  }

  onUsersSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.usersSearchText.set(value);
  }

  onPermissionsSearch(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.permissionsSearchText.set(value);
  }

  navigateBack(): void {
    this.router.navigate(['/dashboard/admin-dashboard']);
  }
} 