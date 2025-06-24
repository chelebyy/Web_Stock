# Dashboard Yönlendirme Bilgi Tabanı

## Genel Bakış
Bu belge, kullanıcı girişi sonrası dashboard yönlendirme işlemlerinin nasıl yapılandırıldığını ve yönetildiğini açıklar.

## Yönlendirme Yapılandırması

### Route Tanımları
```typescript
export const routes: Routes = [
  { 
    path: 'admin-dashboard', 
    component: AdminDashboardComponent,
    canActivate: [AuthGuard],
    data: { requiresAdmin: true }
  },
  { 
    path: 'user-dashboard', 
    component: UserDashboardComponent,
    canActivate: [AuthGuard]
  }
];
```

### Login Component'inde Yönlendirme
```typescript
// Kullanıcı rolüne göre yönlendirme
const user = this.authService.getCurrentUser();
const targetRoute = user?.isAdmin ? '/admin-dashboard' : '/user-dashboard';
console.log('Yönlendiriliyor:', targetRoute);

setTimeout(() => {
  this.router.navigate([targetRoute])
    .then(() => console.log('Yönlendirme başarılı'))
    .catch(err => console.error('Yönlendirme hatası:', err));
}, 1000);
```

## Güvenlik

### AuthGuard Yapılandırması
- Her dashboard rotası AuthGuard ile korunur
- Admin dashboard için ekstra requiresAdmin kontrolü yapılır
- Yetkisiz erişim girişimleri login sayfasına yönlendirilir

### Rol Bazlı Erişim
- Admin kullanıcıları: /admin-dashboard
- Normal kullanıcılar: /user-dashboard
- Yetkilendirme AuthService üzerinden yönetilir

## Hata Yönetimi

### Loglama
- Yönlendirme başlangıcında hedef rota loglanır
- Başarılı yönlendirme loglanır
- Hata durumunda detaylı hata mesajı loglanır

### Yaygın Hatalar ve Çözümleri
1. "Cannot match any routes" hatası:
   - Route tanımlarını kontrol et
   - Yönlendirme path'inin doğruluğunu kontrol et
   - Component'lerin import edildiğinden emin ol

2. Yetkilendirme hataları:
   - AuthGuard yapılandırmasını kontrol et
   - Kullanıcı rolünün doğru atandığından emin ol
   - Token'ın geçerli olduğunu kontrol et

## Best Practices
1. Her zaman AuthGuard kullan
2. Rol bazlı erişim için data özelliğini kullan
3. Yönlendirme işlemlerini detaylı logla
4. Hata durumlarını ele al ve kullanıcıya bildir
5. Yönlendirme öncesi kullanıcı bilgilerini doğrula

## Geliştirme İpuçları
1. Route tanımlarını tek bir dosyada tut
2. AuthGuard'ı modüler ve genişletilebilir tasarla
3. Yönlendirme mantığını AuthService ile senkronize et
4. Hata mesajlarını kullanıcı dostu yap
5. Yönlendirme gecikmesi için uygun süre kullan (1000ms)

## Gelecek Geliştirmeler
1. Daha granüler rol bazlı erişim
2. Dashboard özelleştirme seçenekleri
3. Yönlendirme animasyonları
4. Breadcrumb navigasyonu
5. Son ziyaret edilen sayfayı hatırlama 