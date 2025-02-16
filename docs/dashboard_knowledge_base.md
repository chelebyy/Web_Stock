# Dashboard Sayfası Bilgi Tabanı

## Genel Bilgiler
- Rota: `/dashboard`
- Erişim: Sadece giriş yapmış kullanıcılar
- Rol gereksinimleri: Tüm kullanıcılar erişebilir

## Sayfa Bileşenleri
1. Karşılama Mesajı
   - Format: "Merhaba, {kullanıcı_adı}!"
   - Dinamik olarak giriş yapan kullanıcının adını gösterir

2. Ana Menü
   - Kullanıcı Yönetimi Butonu (Sadece admin kullanıcılar için)
   - Çıkış Yap Butonu (Tüm kullanıcılar için)

## Güvenlik
- Route Guard ile korunur (`AuthGuard`)
- JWT token kontrolü yapılır
- Admin yetkisi gerektiren butonlar dinamik olarak gösterilir/gizlenir

## İşlevsellik
1. Kullanıcı Yönetimi
   - Admin kullanıcılar için görünür
   - Kullanıcı CRUD işlemleri için yönlendirme

2. Oturum Kapatma
   - Token'ı siler
   - LocalStorage'ı temizler
   - Login sayfasına yönlendirir

## Hata Yönetimi
- Token geçersiz olduğunda otomatik çıkış
- Yetkisiz erişim denemelerinde uyarı mesajı
- Sayfa yüklenme hatalarında kullanıcı dostu mesajlar

## Stil ve Tasarım
- Responsive tasarım
- PrimeNG bileşenleri kullanımı
- Tutarlı renk şeması ve tipografi

## Test Senaryoları
1. Admin kullanıcı girişi
   - Tüm butonların görünürlüğü
   - Kullanıcı yönetimi erişimi

2. Normal kullanıcı girişi
   - Kısıtlı buton görünürlüğü
   - Yetkisiz alan erişim kontrolleri

3. Token yönetimi
   - Geçersiz token davranışı
   - Token yenileme süreci

## Öğrenilen Dersler
1. Rol tabanlı erişim kontrolü önemli
2. Kullanıcı deneyimi için hata mesajları açık olmalı
3. Güvenlik ve kullanıcı dostu arayüz dengesi korunmalı

## Planlanan İyileştirmeler
1. [ ] Dashboard istatistikleri ekleme
2. [ ] Kullanıcı profil yönetimi
3. [ ] Tema desteği
4. [ ] Dil desteği
5. [ ] Bildirim sistemi

## Notlar
- Her değişiklik için yetki kontrolü yapılmalı
- Kullanıcı geri bildirimleri toplanmalı
- Performans metrikleri izlenmeli
