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
    console.log('---------------------------------------------------');
    console.log('AuthGuard: Rota erişim kontrolü başlıyor');
    console.log('Tam URL:', state.url);
    console.log('Rota verisi:', JSON.stringify(route.data, null, 2));
    console.log('---------------------------------------------------');
    
    // Token bilgisini kontrol et
    const token = this.authService.getToken();
    console.log('Token mevcut mu:', token ? 'Evet' : 'Hayır');
    if (!token) {
      console.error('AuthGuard: Token bulunamadı!');
    } else {
      // Token süresi kontrolü
      try {
        const isExpired = this.authService['jwtHelper'].isTokenExpired(token);
        console.log('Token süresi dolmuş mu:', isExpired ? 'Evet' : 'Hayır');
        
        if (isExpired) {
          console.error('AuthGuard: Token süresi dolmuş, login sayfasına yönlendiriliyor');
          this.router.navigate(['/login']);
          return false;
        }
      } catch (error) {
        console.error('AuthGuard: Token kontrolünde hata:', error);
      }
    }
    
    // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
    if (!this.authService.isLoggedIn()) {
      console.error('AuthGuard: Kullanıcı giriş yapmamış, login sayfasına yönlendiriliyor');
      this.router.navigate(['/login']);
      return false;
    }

    // Kullanıcı bilgilerini kontrol et
    const currentUser = this.authService.getCurrentUser();
    console.log('Mevcut kullanıcı:', currentUser);
    
    // Admin durumunu kontrol et
    const isAdmin = this.authService.isAdmin();
    console.log('Kullanıcı admin mi:', isAdmin ? 'Evet' : 'Hayır');

    // URL'den sayfa yolunu al
    const url = state.url.split('?')[0]; // Query parametrelerini kaldır
    console.log('AuthGuard: Kontrol edilen sayfa yolu:', url);
    
    // Admin yetkisi gerektiren rotalar için kontrol
    if (route.data['requiresAdmin'] && !isAdmin) {
      console.error('AuthGuard: Admin yetkisi gerekiyor fakat kullanıcı admin değil');
      this.router.navigate(['/user-dashboard']);
      return false;
    }

    // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
    if (url === '/user-dashboard') {
      console.log('AuthGuard: User dashboard sayfası için erişim doğrudan onaylandı');
      return true;
    }

    // Route data'sında izin belirtilmişse kontrol et
    if (route.data['requiredPermission']) {
      const requiredPermission = route.data['requiredPermission'];
      console.log('AuthGuard: Route data\'dan gerekli izin:', requiredPermission);
      
      // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
      if (requiredPermission === 'Pages.UserDashboard') {
        console.log('AuthGuard: User dashboard izni için bypass yapılıyor');
        return true;
      }
      
      // İzin kontrolü
      const hasPermission = this.authService.hasPermission(requiredPermission);
      console.log(`AuthGuard: '${requiredPermission}' iznine sahip mi:`, hasPermission ? 'Evet' : 'Hayır');
      
      if (!hasPermission) {
        console.error(`AuthGuard: Erişim reddedildi: '${requiredPermission}' izni yok`);
        this.router.navigate(['/user-dashboard']);
        return false;
      }
      
      console.log(`AuthGuard: '${requiredPermission}' izni var, erişim onaylandı`);
      return true;
    }
    
    // Sayfa yoluna göre izin kontrolü
    const pagePermission = this.pagePermissionMap[url];
    console.log('AuthGuard: Sayfa yoluna göre gerekli izin:', pagePermission || 'Belirtilmemiş');
    
    if (pagePermission) {
      // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
      if (pagePermission === 'Pages.UserDashboard') {
        console.log('AuthGuard: User dashboard sayfası için izin kontrolü bypass ediliyor');
        return true;
      }
      
      // İzin kontrolü
      const hasPermission = this.authService.hasPermission(pagePermission);
      console.log(`AuthGuard: '${pagePermission}' iznine sahip mi:`, hasPermission ? 'Evet' : 'Hayır');
      
      if (!hasPermission) {
        console.error(`AuthGuard: Erişim reddedildi: '${pagePermission}' izni yok`);
        
        // İzin yoksa varsayılan olarak kullanıcı dashboard'a yönlendir
        // Admin kullanıcıysa admin dashboard'a yönlendir
        const redirectUrl = this.authService.isAdmin() ? '/admin-dashboard' : '/user-dashboard';
        console.log('AuthGuard: Kullanıcı yönlendiriliyor:', redirectUrl);
        this.router.navigate([redirectUrl]);
        return false;
      }
    }

    console.log('AuthGuard: Erişim onaylandı');
    console.log('---------------------------------------------------');
    return true;
  }
} 