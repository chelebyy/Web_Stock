# Auth Modülü Bilgi Bankası

Bu belge, Stock projesinin kimlik doğrulama (Authentication) özelliği ve özellikle `AuthService` ile ilgili geliştirme süreçlerini, karşılaşılan hataları, çözümleri ve öğrenilen dersleri belgelemek amacıyla oluşturulmuştur.

## AuthService Unit Testleri

Bu bölümde, `AuthService` için yazılan unit testlerin detayları, karşılaşılan zorluklar ve uygulanan çözüm yöntemleri yer alacaktır.

### Test Edilen Metotlar ve Senaryolar

- **getToken()**:
    - localStorage'da token varsa doğru token'ı döndürmeli.
    - localStorage'da token yoksa null döndürmeli.
- **isLoggedIn()**:
    - Geçerli bir token varsa true döndürmeli.
    - Token yoksa false döndürmeli.
    - Süresi dolmuş bir token varsa false döndürmeli.
    - Geçersiz bir token varsa false döndürmeli.
- **loadStoredUser()**: (Constructor içinde çağrılır)
    - localStorage'da geçerli bir token varsa kullanıcı bilgilerini ve izinlerini yüklemeli.
    - Token yoksa kullanıcı yüklememeli.
    - Token süresi dolmuşsa token'ı silmeli ve kullanıcıyı null yapmalı.
    - Token geçersiz/çözümlenemez ise token'ı silmeli ve kullanıcıyı null yapmalı.
- **login(loginRequest: LoginRequest)**:
    - [✓] Başarılı giriş: Token kaydedilmeli, kullanıcı ve izinler güncellenmeli, doğru sayfaya yönlendirilmeli.
    - [✓] Başarısız giriş: Hata mesajı uygun şekilde işlenmeli.
    - [✓] HTTP isteği doğru endpoint'e, doğru metotla ve doğru body ile yapılmalı.
- **logout()**:
    - [✓] Token localStorage'dan silinmeli.
    - [✓] `currentUserSubject` null olmalı.
    - [✓] İzinler temizlenmeli.
    - [✓] Kullanıcı login sayfasına yönlendirilmeli.
- **createUser(createUserRequest: CreateUserRequest)**:
    - [✓] Başarılı kullanıcı oluşturma: API'ye doğru istek atılmalı.
    - [✓] Başarısız kullanıcı oluşturma: API'den dönen hata işlenmeli.
- **isAdmin()**:
    - [✓] Aktif kullanıcı admin ise true dönmeli.
    - [✓] Aktif kullanıcı admin değilse false dönmeli.
    - [✓] Aktif kullanıcı yoksa false dönmeli.
- **getCurrentUser()**: (loadStoredUser testleri kapsamında dolaylı olarak test ediliyor, ek senaryolar eklenebilir)
    - [✓] Aktif kullanıcı varsa doğru kullanıcı objesini dönmeli.
    - [✓] Aktif kullanıcı yoksa null dönmeli.
- **changePassword(currentPassword: string, newPassword: string)**:
    - [✓] Başarılı şifre değişikliği: API'ye doğru istek atılmalı.
    - [✓] Başarısız şifre değişikliği: API'den dönen hata işlenmeli.
- **hasPermission(permissionName: string)**: (loadStoredUser testleri kapsamında dolaylı olarak test ediliyor, ek senaryolar eklenebilir)
    - [✓] Kullanıcının sahip olduğu bir izin için true dönmeli.
    - [✓] Kullanıcının sahip olmadığı bir izin için false dönmeli.
    - [✓] Admin kullanıcının varsayılan izinleri için true dönmeli.

### Karşılaşılan Zorluklar ve Çözümleri

- `LoginRequest` modelinin `sicil` alanı beklediği, testlerde ise başlangıçta `username` kullanıldığı fark edildi. `auth.model.ts` kontrol edilerek `LoginRequest`'in doğru tanımı (`sicil` ve `password`) teyit edildi ve testlerdeki istek objeleri buna göre düzeltildi.
- `AuthService` içindeki `extractPermissionsFromToken` metodunun karmaşıklığı ve farklı senaryolarda (örneğin admin rolü, token'daki çeşitli permission claim formatları) nasıl izinler çıkardığı `hasPermission` testleri yazılırken dikkate alındı. Mock `JwtHelperService` içindeki `decodeToken` metodunun döndürdüğü veriler bu izin çıkarma mantığıyla tutarlı olacak şekilde ayarlandı veya test beklentileri buna göre düzenlendi.

### Öğrenilen Dersler

- Unit test yazarken, test edilen servisin bağımlı olduğu modellerin (örn: `LoginRequest`) tanımlarını doğrulamak önemlidir. Yanlış model kullanımı, yanıltıcı test sonuçlarına veya linter hatalarına yol açabilir.
- Karmaşık iç mantığa sahip private metotlar (örn: `extractPermissionsFromToken`), public metotlar aracılığıyla test edilirken, bu iç mantığın farklı dallarını tetikleyecek çeşitli senaryoların public metot testlerine dahil edilmesi gerekir. Bu, private metodun dolaylı olarak daha kapsamlı test edilmesini sağlar.
- `TestBed.inject(Service)` kullanımı, özellikle constructor'ında önemli başlangıç işlemleri (örn: `loadStoredUser` gibi) yapan servisler için her `it` bloğunda veya senaryoya özel `beforeEach` içinde çağrılarak servisin temiz bir durumda ve en güncel mock/spy konfigürasyonlarıyla başlamasını garanti altına almak açısından faydalı olabilir.

---
Son Güncelleme: 09.05.2025 