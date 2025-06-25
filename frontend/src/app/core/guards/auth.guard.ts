import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

/**
 * Kullanıcının kimliğinin doğrulanıp doğrulanmadığını kontrol eden fonksiyonel bir route guard.
 * Eğer kullanıcı giriş yapmamışsa, '/login' sayfasına yönlendirir.
 *
 * @returns Kullanıcı giriş yapmışsa `true`, aksi takdirde `false` döner.
 */
export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;
  }

  // Kullanıcı giriş yapmamış, login sayfasına yönlendir
  console.error('AuthGuard: Kullanıcı giriş yapmamış, login sayfasına yönlendiriliyor.');
  router.navigate(['/login']);
  return false;
}; 