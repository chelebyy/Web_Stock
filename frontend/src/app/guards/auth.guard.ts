import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    console.log('AuthGuard.canActivate çalıştı');
    console.log('Hedef route:', state.url);
    console.log('Token var mı:', this.authService.getToken() ? 'Evet' : 'Hayır');
    console.log('Kullanıcı giriş yapmış mı:', this.authService.isLoggedIn());
    console.log('Kullanıcı admin mi:', this.authService.isAdmin());

    const token = this.authService.getToken();
    if (!token) {
      console.log('Token bulunamadı, login sayfasına yönlendiriliyor');
      this.router.navigate(['/login']);
      return false;
    }

    if (!this.authService.isLoggedIn()) {
      console.log('Token geçersiz, login sayfasına yönlendiriliyor');
      this.router.navigate(['/login']);
      return false;
    }

    // Admin yetkisi gerektiren rotalar için kontrol
    if (route.data['requiresAdmin'] && !this.authService.isAdmin()) {
      console.log('Admin yetkisi gerekiyor, ana sayfaya yönlendiriliyor');
      this.router.navigate(['/']);
      return false;
    }

    console.log('Yetkilendirme başarılı, route erişimine izin verildi');
    return true;
  }
}
