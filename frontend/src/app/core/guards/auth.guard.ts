import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  // Sayfa rotası - izin eşleştirmeleri
  private pagePermissionMap: Record<string, string> = {
    '/dashboard/admin-dashboard': 'Pages.AdminDashboard',
    '/dashboard/user-dashboard': 'Pages.UserDashboard',
    '/users': 'Pages.UserManagement',
    '/roles': 'Pages.RoleManagement',
    '/stocks': 'Pages.StockManagement',
    '/reports': 'Pages.Reports',
    '/settings': 'Pages.Settings'
  };

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    console.log('---------------------------------------------------');
    console.log('AuthGuard: Rota erişim kontrolü başlıyor');
    console.log('Tam URL:', state.url);
    console.log('Rota verisi:', JSON.stringify(route.data, null, 2));
    console.log('---------------------------------------------------');
    
    // Login sayfası için özel durum - her zaman erişime izin ver
    if (state.url.includes('/login')) {
      console.log('AuthGuard: Login sayfası için özel durum, erişim onaylandı');
      return true;
    }
    
    // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
    if (!this.authService.isLoggedIn()) {
      console.error('AuthGuard: Kullanıcı giriş yapmamış, login sayfasına yönlendiriliyor');
      this.router.navigate(['/login']);
      return false;
    }

    // Token bilgisini kontrol et
    const token = this.authService.getToken();
    if (!token) {
      console.error('AuthGuard: Token bulunamadı!');
      this.router.navigate(['/login']);
      return false;
    }
    
    // Kullanıcı bilgilerini kontrol et
    const currentUser = this.authService.getCurrentUser();
    if (!currentUser) {
      console.error('AuthGuard: Kullanıcı bilgileri bulunamadı!');
      this.router.navigate(['/login']);
      return false;
    }
    
    console.log('AuthGuard: Kullanıcı bilgileri:', currentUser);
    
    // Rol yönetimi sayfası için özel durum kontrolü
    if (state.url.includes('/role-management')) {
      console.log('AuthGuard: Rol yönetimi sayfası için özel durum kontrolü');
      
      // Admin kontrolü
      const isAdmin = this.authService.isAdmin();
      console.log('AuthGuard: Rol yönetimi - Kullanıcı admin mi:', isAdmin ? 'Evet' : 'Hayır');
      
      if (!isAdmin) {
        console.error('AuthGuard: Rol yönetimi - Admin yetkisi gerekiyor fakat kullanıcı admin değil');
        this.router.navigate(['/dashboard/user-dashboard']);
        return false;
      }
      
      console.log('AuthGuard: Rol yönetimi - Tüm kontroller başarılı, erişim onaylandı');
      return true;
    }

    // Admin durumunu kontrol et
    const isAdmin = this.authService.isAdmin();
    console.log('Kullanıcı admin mi:', isAdmin ? 'Evet' : 'Hayır');

    // Admin yetkisi gerektiren rotalar için kontrol
    if (route.data['requiresAdmin']) {
      if (!isAdmin) {
        console.error('AuthGuard: Admin yetkisi gerekiyor fakat kullanıcı admin değil');
        this.router.navigate(['/dashboard/user-dashboard']);
        return false;
      }
      console.log('AuthGuard: Admin yetkisi kontrolü başarılı, erişim onaylandı');
      return true;
    }

    // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
    if (state.url === '/dashboard/user-dashboard') {
      console.log('AuthGuard: User dashboard sayfası için erişim doğrudan onaylandı');
      return true;
    }

    // Route data'sında izin belirtilmişse kontrol et
    if (route.data['requiredPermission']) {
      const requiredPermission = route.data['requiredPermission'];
      const hasPermission = this.authService.hasPermission(requiredPermission);
      
      if (!hasPermission) {
        console.error(`AuthGuard: Erişim reddedildi: '${requiredPermission}' izni yok`);
        this.router.navigate(['/dashboard/user-dashboard']);
        return false;
      }
      
      return true;
    }

    // Sayfa yoluna göre izin kontrolü
    const url = state.url.split('?')[0];
    const pagePermission = this.pagePermissionMap[url];
    
    if (pagePermission) {
      const hasPermission = this.authService.hasPermission(pagePermission);
      
      if (!hasPermission) {
        console.error(`AuthGuard: Erişim reddedildi: '${pagePermission}' izni yok`);
        const redirectUrl = isAdmin ? '/dashboard/admin-dashboard' : '/dashboard/user-dashboard';
        this.router.navigate([redirectUrl]);
        return false;
      }
    }

    console.log('AuthGuard: Tüm kontroller başarılı, erişim onaylandı');
    return true;
  }
} 