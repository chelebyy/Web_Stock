import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    console.log('PermissionGuard: Rota izin kontrolü', route.url.join('/'));
    
    // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
    if (!this.authService.isLoggedIn()) {
      console.log('PermissionGuard: Kullanıcı giriş yapmamış, login sayfasına yönlendiriliyor');
      return this.router.createUrlTree(['/login']);
    }

    // Admin kullanıcılar her sayfaya erişebilir
    if (this.authService.isAdmin()) {
      console.log('PermissionGuard: Kullanıcı admin, erişim onaylandı');
      return true;
    }

    // Rota verilerinden gerekli izinleri al
    const requiredPermissions = route.data['permissions'] as string[];
    console.log('PermissionGuard: Gerekli izinler:', requiredPermissions);
    
    // İzin gerektirmeyen sayfalar
    if (!requiredPermissions || requiredPermissions.length === 0) {
      console.log('PermissionGuard: Sayfa izin gerektirmiyor, erişim onaylandı');
      return true;
    }

    // Kullanıcının gerekli izinlerden en az birine sahip olup olmadığını kontrol et
    const hasPermission = requiredPermissions.some(permission => {
      const hasOnePermission = this.authService.hasPermission(permission);
      console.log(`PermissionGuard: İzin kontrolü - "${permission}": ${hasOnePermission ? 'Var' : 'Yok'}`);
      return hasOnePermission;
    });
    
    console.log('PermissionGuard: İzin kontrolü sonucu:', hasPermission ? 'İzin var' : 'İzin yok');

    if (!hasPermission) {
      // İzin yoksa erişim reddedildi sayfasına yönlendir
      console.log('PermissionGuard: Erişim reddedildi, access-denied sayfasına yönlendiriliyor');
      return this.router.createUrlTree(['/access-denied']);
    }
    
    console.log('PermissionGuard: Erişim onaylandı');
    return true;
  }
} 