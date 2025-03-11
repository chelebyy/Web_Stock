import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../../core/authentication/auth.service';

@Directive({
  selector: '[hasPermission]',
  standalone: true
})
export class HasPermissionDirective implements OnInit {
  @Input() hasPermission: string | string[] = '';

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.updateView();
  }

  private updateView() {
    if (this.checkPermission()) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  private checkPermission(): boolean {
    // Eğer izin belirtilmemişse, içeriği gösterme
    if (!this.hasPermission) {
      return false;
    }

    // Admin kontrolünü kaldırdık - artık admin kullanıcılar da izin kontrolüne tabi
    // İzin bir dizi ise, herhangi birinin olması yeterli
    if (Array.isArray(this.hasPermission)) {
      return this.hasPermission.some(permission => 
        this.authService.hasPermission(permission));
    }

    // Tek bir izin kontrolü
    return this.authService.hasPermission(this.hasPermission);
  }

  /**
   * Özel izin kontrol metodu - Sayfa erişim izni mi kontrol eder
   */
  static isPageAccessPermission(permissionName: string): boolean {
    return permissionName.startsWith('Pages.');
  }
} 