# Taşıma Sonrası Doğrulama Testleri

Bu belge, Clean Architecture geçişi sırasında ve sonrasında yapılması gereken doğrulama testlerini tanımlar.

## Test Stratejisi

Taşıma sonrası doğrulama testleri üç katmanda gerçekleştirilir:

1. **Birim Testleri:** Her bileşenin kendi içindeki işlevselliğini doğrular
2. **Entegrasyon Testleri:** Bileşenlerin birbirleriyle entegrasyonunu doğrular
3. **Uçtan Uca Testler:** Kullanıcı senaryolarını tam olarak doğrular

## 1. Birim Testleri

### Domain Layer

| Test ID | Test Adı | Açıklama | Kabul Kriterleri |
|---------|----------|----------|------------------|
| DT-001 | User Entity Test | User entity özelliklerini test eder | Tüm özellikler doğru çalışmalı, entity geçerli/geçersiz durumları doğru yönetmeli |
| DT-002 | Role Entity Test | Role entity özelliklerini test eder | Tüm özellikler doğru çalışmalı, entity geçerli/geçersiz durumları doğru yönetmeli |
| DT-003 | BaseEntity Test | BaseEntity davranışlarını test eder | CreatedAt ve IsDeleted alanları doğru çalışmalı |
| DT-004 | IRepository Interface Test | IRepository interface metodlarını test eder | Tüm CRUD operasyonları doğru çalışmalı |
| DT-005 | IUnitOfWork Interface Test | IUnitOfWork interface metodlarını test eder | SaveChanges ve transaction metodları doğru çalışmalı |

### Application Layer

| Test ID | Test Adı | Açıklama | Kabul Kriterleri |
|---------|----------|----------|------------------|
| AT-001 | UserDto Mapping Test | Entity-DTO dönüşümlerini test eder | Mapping doğru çalışmalı, tüm alanlar korunmalı |
| AT-002 | RoleDto Mapping Test | Entity-DTO dönüşümlerini test eder | Mapping doğru çalışmalı, tüm alanlar korunmalı |
| AT-003 | UserCommands Test | User Command handler'larını test eder | Command'lar doğru işlenmeli, doğrulama kuralları çalışmalı |
| AT-004 | UserQueries Test | User Query handler'larını test eder | Query'ler doğru sonuç döndürmeli |
| AT-005 | RoleCommands Test | Role Command handler'larını test eder | Command'lar doğru işlenmeli, doğrulama kuralları çalışmalı |
| AT-006 | RoleQueries Test | Role Query handler'larını test eder | Query'ler doğru sonuç döndürmeli |

### Infrastructure Layer

| Test ID | Test Adı | Açıklama | Kabul Kriterleri |
|---------|----------|----------|------------------|
| IT-001 | ApplicationDbContext Test | DbContext konfigürasyonlarını test eder | Entity ayarları doğru yapılmış olmalı |
| IT-002 | UserRepository Test | UserRepository implementasyonunu test eder | Tüm CRUD operasyonları doğru çalışmalı |
| IT-003 | RoleRepository Test | RoleRepository implementasyonunu test eder | Tüm CRUD operasyonları doğru çalışmalı |
| IT-004 | UnitOfWork Test | UnitOfWork implementasyonunu test eder | Transaction yönetimi doğru çalışmalı |
| IT-005 | HashService Test | Hash algoritmasını test eder | Hash sonuçları tutarlı olmalı, eski ve yeni hash'ler uyumlu olmalı |
| IT-006 | JwtService Test | Token üretimi ve doğrulamasını test eder | Token'lar doğru üretilmeli ve doğrulanmalı |

### API Layer

| Test ID | Test Adı | Açıklama | Kabul Kriterleri |
|---------|----------|----------|------------------|
| APIT-001 | AuthController Test | Kimlik doğrulama endpoint'lerini test eder | Login, register ve token endpoint'leri doğru çalışmalı |
| APIT-002 | UserController Test | Kullanıcı yönetimi endpoint'lerini test eder | CRUD operasyonları doğru çalışmalı |
| APIT-003 | Middleware Tests | Middleware'leri test eder | Exception handler, authentication ve logging middleware'leri doğru çalışmalı |
| APIT-004 | API Authorization Test | Yetkilendirme kontrollerini test eder | Role-based yetkilendirme doğru çalışmalı |

## 2. Entegrasyon Testleri

| Test ID | Test Adı | Açıklama | Kabul Kriterleri |
|---------|----------|----------|------------------|
| INT-001 | User-Role Entegrasyonu | User ve Role ilişkisini test eder | Kullanıcı-rol ilişkisi doğru çalışmalı |
| INT-002 | Authentication Flow | Kimlik doğrulama akışını test eder | Login, token üretimi ve doğrulama akışı çalışmalı |
| INT-003 | API-Database Entegrasyonu | API ve veritabanı entegrasyonunu test eder | Veritabanı işlemleri doğru gerçekleşmeli |
| INT-004 | API-Frontend Entegrasyonu | API ve frontend bağlantısını test eder | Frontend istekleri doğru işlenmeli |
| INT-005 | Transaction Management | Transaction yönetimini test eder | İşlemler başarılı/başarısız durumlarında doğru sonuçlanmalı |

## 3. Uçtan Uca (E2E) Testler

| Test ID | Test Adı | Açıklama | Kabul Kriterleri |
|---------|----------|----------|------------------|
| E2E-001 | Kullanıcı Girişi | Login işlemini test eder | Kullanıcı girişi başarılı olmalı, yönlendirme doğru yapılmalı |
| E2E-002 | Yeni Kullanıcı Oluşturma | Kullanıcı oluşturma sürecini test eder | Yeni kullanıcı başarıyla oluşturulmalı |
| E2E-003 | Kullanıcı Güncelleme | Kullanıcı bilgilerinin güncellenmesini test eder | Bilgiler doğru güncellenebilmeli |
| E2E-004 | Kullanıcı Silme | Kullanıcı silme işlemini test eder | Kullanıcı başarıyla silinebilmeli |
| E2E-005 | Yetkilendirme Kontrolü | Yetkisiz erişim denemelerini test eder | Yetkisiz erişim denemeleri reddedilmeli |
| E2E-006 | Token Yenileme | Token süresi dolduğunda yenileme sürecini test eder | Token süresi dolduğunda yeni token alınabilmeli |

## Test Ortamı

| Ortam | Açıklama | Konfigürasyon |
|-------|----------|---------------|
| Geliştirme | Geliştirme sırasında birim testleri için | Yerel PostgreSQL, InMemory database |
| Test | Entegrasyon testleri için | Test veritabanı, Test API |
| UAT | E2E testleri için | Tam entegre sistem |

## Test Veri Stratejisi

1. **Birim Testleri:** Mock veri ve test fixture'ları kullanılır
2. **Entegrasyon Testleri:** Test veritabanına test verisi eklenir
3. **E2E Testleri:** UAT ortamında gerçekçi test verisi kullanılır

## Test Yürütme Planı

1. Her bileşen taşındıktan sonra birim testleri çalıştırılır
2. Bileşen grupları tamamlandıktan sonra ilgili entegrasyon testleri çalıştırılır
3. Tüm taşıma süreci tamamlandıktan sonra E2E testleri çalıştırılır
4. Regresyon testi olarak tüm testler tekrar çalıştırılır

## Otomasyon

Testlerin mümkün olduğunca otomatikleştirilmesi hedeflenir:

1. **Birim ve Entegrasyon Testleri:** xUnit test framework kullanılır
2. **E2E Testleri:** Selenium/Cypress kullanılır
3. **CI/CD Pipeline:** GitHub Actions ile test otomasyonu sağlanır

## Raporlama

Test sonuçları aşağıdaki formatta raporlanır:

1. Çalıştırılan test sayısı ve başarı oranı
2. Başarısız testlerin listesi ve nedenleri
3. Kritik hataların analizi
4. Çözüm önerileri

## Test Sertifikasyonu

Clean Architecture geçişinin tamamlanabilmesi için:

1. Tüm birim testlerin minimum %90 başarı oranına sahip olması
2. Tüm entegrasyon testlerin minimum %85 başarı oranına sahip olması
3. Tüm E2E testlerin minimum %95 başarı oranına sahip olması
4. Kritik hata bulunmaması

## Son Güncelleme
- Tarih: 2025-02-21
- Güncelleme Detayı: İlk belge oluşturuldu 