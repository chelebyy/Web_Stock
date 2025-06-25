import { inject } from '@angular/core';
import { CanActivateFn, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

/**
 * Kullanıcının bir rotaya erişim için gerekli izinlere sahip olup olmadığını kontrol eden fonksiyonel bir route guard.
 *
 * @returns Kullanıcının izni varsa `true`, yoksa `/access-denied` sayfasına yönlendirir ve `false` döner.
 */
export const permissionGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Admin kullanıcılar her zaman erişebilir.
  if (authService.isAdmin()) {
    return true;
  }

  // Rota verisinden gerekli izni (veya izinleri) al.
  const requiredPermission = route.data['requiredPermission'] as string | string[];

  // Herhangi bir izin gerekmiyorsa erişime izin ver.
  if (!requiredPermission || requiredPermission.length === 0) {
    return true;
  }

  let hasPermission = false;

  if (Array.isArray(requiredPermission)) {
    // Birden fazla izin varsa, kullanıcının en az birine sahip olması yeterlidir.
    hasPermission = requiredPermission.some(p => authService.hasPermission(p));
  } else {
    // Tek bir izin varsa, o iznin olup olmadığını kontrol et.
    hasPermission = authService.hasPermission(requiredPermission);
  }

  if (hasPermission) {
    return true;
  }

  // İzin yoksa erişimi reddet ve yönlendir.
  console.error(`PermissionGuard: Erişim reddedildi. Gerekli izin(ler): ${requiredPermission}`);
  router.navigate(['/access-denied']);
  return false;
}; 