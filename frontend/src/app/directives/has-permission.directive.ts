import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[hasPermission]',
  standalone: true
})
export class HasPermissionDirective implements OnInit {
  @Input('hasPermission') permissionName!: string;
  @Input('hasPermissionElse') elseTemplate?: TemplateRef<any>;
  
  private hasView = false;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.updateView();
  }

  /**
   * İzin değiştiğinde görünümü günceller
   */
  private updateView(): void {
    const hasPermission = this.checkPermission();

    if (hasPermission && !this.hasView) {
      // İzin varsa template'i göster
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
    } else if (!hasPermission && this.hasView) {
      // İzin yoksa temizle
      this.viewContainer.clear();
      this.hasView = false;
      
      // Alternatif template varsa göster
      if (this.elseTemplate) {
        this.viewContainer.createEmbeddedView(this.elseTemplate);
      }
    }
  }
  
  /**
   * İzin kontrolü yapar
   */
  private checkPermission(): boolean {
    return this.authService.hasPermission(this.permissionName);
  }
  
  /**
   * Özel izin kontrol metodu - Sayfa erişim izni mi kontrol eder
   */
  static isPageAccessPermission(permissionName: string): boolean {
    return permissionName.startsWith('Pages.');
  }
} 