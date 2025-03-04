import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[hasPermission]',
  standalone: true
})
export class HasPermissionDirective implements OnInit {
  @Input() hasPermission: string | string[] = [];
  private isHidden = true;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.updateView();
    
    // Kullanıcı değiştiğinde görünürlüğü güncelle
    this.authService.currentUser$.subscribe(() => {
      this.updateView();
    });
  }

  private updateView(): void {
    if (this.checkPermission()) {
      if (this.isHidden) {
        this.viewContainer.createEmbeddedView(this.templateRef);
        this.isHidden = false;
      }
    } else {
      this.viewContainer.clear();
      this.isHidden = true;
    }
  }

  private checkPermission(): boolean {
    // Admin her zaman tüm izinlere sahiptir
    if (this.authService.isAdmin()) {
      return true;
    }
    
    // İzin kontrolü
    if (Array.isArray(this.hasPermission)) {
      return this.hasPermission.some(permission => this.authService.hasPermission(permission));
    }
    
    return this.authService.hasPermission(this.hasPermission);
  }
} 