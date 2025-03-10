import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, UrlTree } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean | UrlTree {
    console.log('---------------------------------------------------');
    console.log('PermissionGuard: Rota izin kontrolü başlıyor');
    console.log('Rota yolu:', route.url.join('/'));
    console.log('Rota tüm verisi:', JSON.stringify(route.data, null, 2));
    console.log('---------------------------------------------------');
    
    // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
    if (!this.authService.isLoggedIn()) {
      console.error('PermissionGuard: Kullanıcı giriş yapmamış, login sayfasına yönlendiriliyor');
      return this.router.createUrlTree(['/login']);
    }

    // Token bilgisini logla
    const token = this.authService.getToken();
    console.log('Token mevcut mu:', token ? 'Evet' : 'Hayır');
    console.log('Token süresi dolmuş mu:', token ? (this.authService['jwtHelper'].isTokenExpired(token) ? 'Evet' : 'Hayır') : 'Token yok');

    // Rol yönetimi sayfası için özel durum - her zaman erişime izin ver
    const url = route.url.join('/');
    if (url === 'role-management' || url === 'roles' || url.startsWith('roles/')) {
      console.log('PermissionGuard: Rol yönetimi sayfası için özel durum, erişim onaylandı');
      return true;
    }

    // Admin kullanıcılar her sayfaya erişebilir
    if (this.authService.isAdmin()) {
      console.log('PermissionGuard: Kullanıcı admin, erişim onaylandı');
      return true;
    }

    // Kullanıcı bilgilerini logla
    const currentUser = this.authService.getCurrentUser();
    console.log('Mevcut kullanıcı:', currentUser);
    
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
      console.error('PermissionGuard: Erişim reddedildi, access-denied sayfasına yönlendiriliyor');
      return this.router.createUrlTree(['/access-denied']);
    }
    
    console.log('PermissionGuard: Erişim onaylandı');
    console.log('---------------------------------------------------');
    return true;
  }
} 