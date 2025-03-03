import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return false;
    }

    // Admin yetkisi gerektiren rotalar için kontrol
    if (route.data['requiresAdmin'] && !this.authService.isAdmin()) {
      this.router.navigate(['/user-dashboard']);
      return false;
    }

    // İzin gerektiren rotalar için kontrol
    if (route.data['requiredPermission']) {
      const requiredPermission = route.data['requiredPermission'];
      
      if (!this.authService.hasPermission(requiredPermission)) {
        this.router.navigate(['/user-dashboard']);
        return false;
      }
    }

    return true;
  }
}
