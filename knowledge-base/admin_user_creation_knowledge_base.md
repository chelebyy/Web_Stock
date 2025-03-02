# Admin Kullanıcısı Oluşturma İşlemi

Bu belge, Stock uygulamasında Admin kullanıcısı oluşturma işlemi ile ilgili bilgileri içermektedir.

## Tarih: 2025-03-02

## Yapılan İşlemler

1. **Veritabanı Sıfırlama ve Yeniden Oluşturma**
   - Veritabanı `dotnet ef database drop --force` komutu ile silindi
   - Veritabanı `dotnet ef database update` komutu ile yeniden oluşturuldu ve migrationlar uygulandı

2. **Admin Kullanıcısı Oluşturma Sınıfı**
   - `CreateAdminUser.cs` adında bir sınıf oluşturuldu
   - Bu sınıf, veritabanında Admin kullanıcısı olup olmadığını kontrol ediyor
   - Eğer Admin kullanıcısı yoksa, yeni bir Admin kullanıcısı oluşturuyor
   - Şifre BCrypt ile hashleniyor

3. **Program.cs Güncelleme**
   - `Program.cs` dosyası güncellenerek Admin kullanıcısı oluşturma kodu eklendi
   - Uygulama başlatıldığında Admin kullanıcısı otomatik olarak oluşturuluyor
   - Hata durumunda loglama yapılıyor

4. **BCrypt.Net-Next Paketi Ekleme**
   - Şifre hashleme için BCrypt.Net-Next paketi eklendi
   - `dotnet add package BCrypt.Net-Next` komutu ile paket eklendi

## Karşılaşılan Sorunlar ve Çözümleri

1. **401 Unauthorized Hatası**
   - Sorun: Kullanıcı giriş yaparken 401 Unauthorized hatası alınıyordu
   - Nedeni: Veritabanında Admin kullanıcısı bulunmuyordu
   - Çözüm: Admin kullanıcısı otomatik olarak oluşturulacak şekilde kod eklendi

2. **CreateAdminUser Sınıfı Bulunamadı Hatası**
   - Sorun: Program.cs dosyasında CreateAdminUser sınıfı bulunamadı hatası alındı
   - Nedeni: CreateAdminUser.cs dosyası yanlış konumda oluşturulmuştu
   - Çözüm: Dosya doğru konuma (Stock.API dizinine) taşındı

3. **BCrypt Paketi Eksikliği**
   - Sorun: BCrypt.Net.BCrypt sınıfı bulunamadı
   - Nedeni: BCrypt.Net-Next paketi eklenmemişti
   - Çözüm: `dotnet add package BCrypt.Net-Next` komutu ile paket eklendi

## Kullanıcı Bilgileri

Admin kullanıcısı aşağıdaki bilgilerle oluşturuldu:
- **Kullanıcı Adı:** Admin
- **Şifre:** Admin123
- **Admin Yetkisi:** Evet

## Önemli Notlar

1. Admin kullanıcısı uygulama ilk çalıştırıldığında otomatik olarak oluşturulur
2. Eğer veritabanında zaten bir Admin kullanıcısı varsa, yeni bir kullanıcı oluşturulmaz
3. Şifre BCrypt ile güvenli bir şekilde hashlenir
4. Admin kullanıcısı oluşturma işlemi Program.cs dosyasında uygulama başlatılırken gerçekleştirilir
5. Hata durumunda loglama yapılır

## Sonraki Adımlar

1. Kullanıcı yönetimi için bir arayüz oluşturulabilir
2. Şifre değiştirme ve sıfırlama özellikleri eklenebilir
3. Rol tabanlı yetkilendirme sistemi geliştirilebilir
4. Kullanıcı profil bilgileri eklenebilir 