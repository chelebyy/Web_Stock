import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  // Sayfa rotası - izin eşleştirmeleri
  private pagePermissionMap: { [key: string]: string } = {
    '/admin-dashboard': 'Pages.AdminDashboard',
    '/user-dashboard': 'Pages.UserDashboard',
    '/users': 'Pages.UserManagement',
    '/roles': 'Pages.RoleManagement',
    '/stocks': 'Pages.StockManagement',
    '/reports': 'Pages.Reports',
    '/settings': 'Pages.Settings'
  };

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    console.log('AuthGuard: Rota erişim kontrolü', state.url);
    
    // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
    if (!this.authService.isLoggedIn()) {
      console.log('AuthGuard: Kullanıcı giriş yapmamış, login sayfasına yönlendiriliyor');
      this.router.navigate(['/login']);
      return false;
    }

    // URL'den sayfa yolunu al
    const url = state.url.split('?')[0]; // Query parametrelerini kaldır
    console.log('AuthGuard: Kontrol edilen sayfa yolu:', url);
    
    // Admin yetkisi gerektiren rotalar için kontrol
    if (route.data['requiresAdmin'] && !this.authService.isAdmin()) {
      console.log('AuthGuard: Admin yetkisi gerekiyor fakat kullanıcı admin değil');
      this.router.navigate(['/user-dashboard']);
      return false;
    }

    // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
    if (url === '/user-dashboard') {
      return true;
    }

    // Route data'sında izin belirtilmişse kontrol et
    if (route.data['requiredPermission']) {
      const requiredPermission = route.data['requiredPermission'];
      
      // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
      if (requiredPermission === 'Pages.UserDashboard') {
        return true;
      }
      
      if (!this.authService.hasPermission(requiredPermission)) {
        console.log(`Erişim reddedildi: '${requiredPermission}' izni yok`);
        this.router.navigate(['/user-dashboard']);
        return false;
      }
      return true;
    }
    
    // Sayfa yoluna göre izin kontrolü
    const pagePermission = this.pagePermissionMap[url];
    console.log('AuthGuard: Sayfa için gerekli izin:', pagePermission);
    if (pagePermission) {
      // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
      if (pagePermission === 'Pages.UserDashboard') {
        console.log('AuthGuard: User dashboard sayfası için izin kontrolü bypass ediliyor');
        return true;
      }
      
      if (!this.authService.hasPermission(pagePermission)) {
        console.log(`AuthGuard: Erişim reddedildi: '${pagePermission}' izni yok`);
        
        // İzin yoksa varsayılan olarak kullanıcı dashboard'a yönlendir
        // Admin kullanıcıysa admin dashboard'a yönlendir
        const redirectUrl = this.authService.isAdmin() ? '/admin-dashboard' : '/user-dashboard';
        console.log('AuthGuard: Kullanıcı yönlendiriliyor:', redirectUrl);
        this.router.navigate([redirectUrl]);
        return false;
      }
    }

    console.log('AuthGuard: Erişim onaylandı');
    return true;
  }
} 