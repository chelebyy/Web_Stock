import { Component, OnInit, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ToastModule } from 'primeng/toast';
import { RippleModule } from 'primeng/ripple';

import { UserPermissionStateService } from '../services/user-permission-state.service';
import { PermissionCardComponent } from './permission-card/permission-card.component';

@Component({
  selector: 'app-user-page-permissions',
  standalone: true,
  imports: [
    CommonModule,
    ButtonModule,
    ProgressSpinnerModule,
    ToastModule,
    RippleModule,
    PermissionCardComponent,
  ],
  providers: [MessageService, UserPermissionStateService],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="page-container">
      <div class="page-header">
        <div class="title-container">
          <h1 class="page-title">Kullanıcı Sayfa İzinleri</h1>
          <p class="text-muted user-info" *ngIf="state.userName()">
            <span class="user-label">Kullanıcı:</span>
            <span class="user-name">{{ state.userName() }}</span>
          </p>
        </div>
        <button pButton pRipple icon="pi pi-arrow-left" label="Geri Dön"
                class="p-button-outlined back-button" (click)="goBack()"></button>
      </div>

      <div class="content-card">
        <div class="loading-container" *ngIf="state.loading()">
          <p-progressSpinner strokeWidth="4"
                           fill="var(--surface-ground)"
                           animationDuration=".5s"></p-progressSpinner>
          <p class="loading-text">İzinler yükleniyor...</p>
        </div>

        <div class="permissions-container" *ngIf="!state.loading()">
          <div class="info-panel">
            <div class="info-icon">
              <i class="pi pi-info-circle"></i>
            </div>
            <div class="info-text">
              Bu sayfada kullanıcının erişebileceği sayfaları belirleyebilirsiniz. İzin vermek istediğiniz sayfaları işaretleyip "Kaydet" butonuna tıklayınız.
            </div>
          </div>

          <div class="page-permissions-container">
            <div class="overlay" [class.active]="state.anyCardExpanded()" (click)="state.closeAllCards()"></div>

            <app-permission-card
              *ngFor="let page of state.pagePermissions(); let i = index"
              [page]="page"
              [index]="i"
              [anyCardExpanded]="state.anyCardExpanded()"
              (toggleCard)="state.togglePageExpanded($event)"
              (closeCard)="state.closeCard($event)">
            </app-permission-card>
          </div>

          <div class="action-buttons">
            <button pButton pRipple label="Kaydet" icon="pi pi-check"
                    class="p-button-success save-button" (click)="state.savePermissions()"></button>
            <button pButton pRipple label="Rol İzinlerine Sıfırla" icon="pi pi-refresh"
                    class="p-button-secondary reset-button" (click)="state.resetToRolePermissions()"></button>
          </div>
        </div>
      </div>
    </div>

    <p-toast position="top-right"></p-toast>
  `,
  styleUrls: ['./user-page-permissions.component.scss']
})
export class UserPagePermissionsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  public readonly state = inject(UserPermissionStateService);

  ngOnInit(): void {
    const userId = this.route.snapshot.paramMap.get('userId');
    if (userId) {
      this.state.initialize(+userId);
    }
  }

  goBack(): void {
    this.router.navigate(['/user-management']);
  }
} 