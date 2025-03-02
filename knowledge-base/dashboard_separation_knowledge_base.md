# Dashboard Separation Knowledge Base

Bu belge, Stock uygulamasında dashboard ayrımı işlemi ile ilgili bilgileri içermektedir.

## Tarih: 2025-03-02

## Yapılan İşlemler

1. **Yeni Bileşenler Oluşturma**
   - `AdminDashboardComponent` admin kullanıcıları için oluşturuldu
   - `UserDashboardComponent` standart kullanıcılar için oluşturuldu

2. **Routing Yapılandırması Güncelleme**
   - Eski `/dashboard` rotası kaldırıldı
   - Yeni `/admin-dashboard` ve `/user-dashboard` rotaları eklendi
   - Admin dashboard için yetkilendirme kontrolleri eklendi

3. **Servisler Güncelleme**
   - `AuthService` kullanıcıları rollerine göre yönlendirmek için güncellendi
   - `AuthGuard` standart kullanıcıların admin sayfalarına erişimini engellemek için güncellendi

## Karşılaşılan Sorunlar ve Çözümleri

1. **Routing Yönlendirme Sorunları**
   - Sorun: Kullanıcılar giriş yaptıktan sonra doğru dashboard'a yönlendirilmiyordu
   - Çözüm: AuthService'de login metodu güncellenerek kullanıcı rolüne göre yönlendirme eklendi

2. **Yetkilendirme Sorunları**
   - Sorun: Standart kullanıcılar admin dashboard'a erişebiliyordu
   - Çözüm: AuthGuard güncellenerek kullanıcı rolü kontrolü eklendi

3. **401 Unauthorized Hatası**
   - Sorun: Kullanıcı giriş yaparken 401 Unauthorized hatası alınıyordu
   - Nedeni: Veritabanında Admin kullanıcısı bulunmuyordu
   - Çözüm: 
     - Veritabanı sıfırlanıp yeniden oluşturuldu
     - Admin kullanıcısı otomatik olarak oluşturulacak şekilde kod eklendi
     - Program.cs dosyasına Admin kullanıcısı oluşturma kodu eklendi
     - Detaylı bilgi için `admin_user_creation_knowledge_base.md` dosyasına bakınız

4. **Swagger Erişim Sorunları**
   - Sorun: Swagger arayüzüne erişilemiyordu
   - Nedeni: 
     - API servisi çalışmıyor olabilir
     - Swagger paketleri eksik olabilir
     - Yanlış URL kullanılıyor olabilir
   - Çözüm:
     - Swagger paketleri eklendi
     - API servisi başlatıldı
     - Doğru URL kullanıldı: `http://localhost:5126/swagger/index.html`

## Önemli Notlar

1. Admin kullanıcısı bilgileri:
   - Kullanıcı Adı: Admin
   - Şifre: Admin123

2. API servisi 5126 portunda çalışıyor

3. Swagger UI'a erişmek için: `http://localhost:5126/swagger/index.html`

4. Veritabanı bağlantı bilgileri:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=czDb;Username=postgres;Password=19909687"
   }
   ```

## Sonraki Adımlar

1. Kullanıcı yönetimi için bir arayüz oluşturulabilir
2. Dashboard içerikleri zenginleştirilebilir
3. Rol tabanlı yetkilendirme sistemi geliştirilebilir
4. Kullanıcı profil bilgileri eklenebilir 