# Dashboard Sayfası Bilgi Tabanı

## Genel Bilgiler
- Rota: `/dashboard`
- Erişim: Sadece giriş yapmış kullanıcılar
- Rol gereksinimleri: Tüm kullanıcılar erişebilir

## Sayfa Bileşenleri
1. Üst Bölüm
   - Hoş Geldiniz başlığı (mavi renk)
   - Sağ üstte sade çıkış butonu
   - Karşılama mesajı: "Merhaba, {kullanıcı_adı}!"

2. Ana Menü Butonları
   - Kullanıcı Yönetimi (Sadece admin kullanıcılar için)
   - Bilgi İşlem (Tüm kullanıcılar için)
   - Dikey yerleşim
   - Tam genişlikte butonlar

## Tasarım Detayları
1. Kart Yapısı
   - Beyaz arka plan
   - Maksimum 600px genişlik
   - 20px iç boşluk
   - Sayfa ortasında konumlandırma

2. Tipografi
   - Başlık: 2rem, mavi renk (#2196F3)
   - Alt metin: 1.1rem, gri renk (#666)
   - Merkezi hizalama

3. Butonlar
   - Çıkış butonu: Text stil, sağ üstte
   - Ana menü butonları: Tam genişlik
   - 15px butonlar arası boşluk
   - İkonlar ve metinler için uygun boşluklar

## Güvenlik
- Route Guard ile korunur (`AuthGuard`)
- JWT token kontrolü yapılır
- Admin yetkisi gerektiren butonlar dinamik olarak gösterilir/gizlenir

## İşlevsellik
1. Kullanıcı Yönetimi
   - Admin kullanıcılar için görünür
   - Kullanıcı CRUD işlemleri için yönlendirme

2. Bilgi İşlem
   - Tüm kullanıcılar için erişilebilir
   - Envanter ve iş takibi için yönlendirme

3. Oturum Kapatma
   - Token'ı siler
   - LocalStorage'ı temizler
   - Login sayfasına yönlendirir

## Hata Yönetimi
- Token geçersiz olduğunda otomatik çıkış
- Yetkisiz erişim denemelerinde uyarı mesajı
- Sayfa yüklenme hatalarında kullanıcı dostu mesajlar

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
4. Sade ve işlevsel tasarım önemli
5. Buton yerleşimleri kullanıcı alışkanlıklarına uygun olmalı

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
- Tasarım değişiklikleri kullanıcı deneyimini iyileştirmeli
