import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
    if (!this.authService.isLoggedIn()) {
      return this.router.createUrlTree(['/login']);
    }

    // Admin kullanıcılar her sayfaya erişebilir
    if (this.authService.isAdmin()) {
      return true;
    }

    // Rota verilerinden gerekli izinleri al
    const requiredPermissions = route.data['permissions'] as string[];
    
    // İzin gerektirmeyen sayfalar
    if (!requiredPermissions || requiredPermissions.length === 0) {
      return true;
    }

    // Kullanıcının gerekli izinlerden en az birine sahip olup olmadığını kontrol et
    const hasPermission = requiredPermissions.some(permission => 
      this.authService.hasPermission(permission));
    
    if (!hasPermission) {
      // İzin yoksa erişim reddedildi sayfasına yönlendir
      return this.router.createUrlTree(['/access-denied']);
    }
    
    return true;
  }
} 