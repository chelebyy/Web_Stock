# Hata Kayıtları ve Çözümleri

## Kullanıcı Sayfa İzinleri Yönetimi Sayfası Sorunları ve Çözümleri

### Sorun: PrimeNG TabView Bileşeni Siyah Arka Plan Sorunu
**Tarih:** 10 Haziran 2025
**Hata:** PrimeNG TabView bileşeninin arka planı siyah renkte görünüyordu ve bu durum tasarımla uyumsuzdu.
**Nedeni:** PrimeNG'nin varsayılan tema ayarlarında bazı bileşenlerin arka plan renkleri siyah olarak tanımlanmıştı.
**Çözüm:**
1. CSS seçicileri kullanarak siyah arka planları mavi (#1976d2) ile değiştirdik.
2. Özellikle `[style*="background-color: #000"]` gibi seçiciler kullanarak tüm siyah arka planları hedefledik.
3. `!important` kullanarak stil önceliğini garantiledik.

```scss
[style*="background-color: #000"],
[style*="background-color: black"],
[style*="background-color: rgb(0, 0, 0)"],
[style*="background-color: rgba(0, 0, 0"],
[style*="background: #000"],
[style*="background: black"],
[style*="background: rgb(0, 0, 0)"],
[style*="background: rgba(0, 0, 0"] {
  background-color: #1976d2 !important;
  background: #1976d2 !important;
}
```

### Sorun: Tablo Tasarımından Kart Tasarımına Geçiş İhtiyacı
**Tarih:** 11 Haziran 2025
**Sorun:** Tablo tasarımı, kullanıcı ve izin bilgilerini görüntülemek için yeterince esnek ve modern değildi.
**Nedeni:** Tablolar, özellikle mobil cihazlarda iyi ölçeklenmiyor ve kullanıcı deneyimini sınırlıyordu.
**Çözüm:**
1. Tablo tasarımından kart tabanlı bir tasarıma geçtik.
2. CSS Grid kullanarak responsive bir kart düzeni oluşturduk.
3. Her kart için hover efektleri ve geçiş animasyonları ekledik.
4. Kullanıcı ve izin bilgilerini daha görsel bir şekilde sunmak için avatar, durum rozeti ve diğer görsel öğeler ekledik.

```scss
.user-cards {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
  
  .user-card {
    position: relative;
    background-color: #ffffff;
    border-radius: 12px;
    padding: 1.5rem;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05), 0 1px 3px rgba(0, 0, 0, 0.1);
    transition: all 0.2s ease;
    display: flex;
    flex-direction: column;
    height: 100%;
      
      &:hover {
      transform: translateY(-2px);
      box-shadow: 0 10px 15px rgba(0, 0, 0, 0.05), 0 4px 6px rgba(0, 0, 0, 0.05);
    }
  }
}
```

### Sorun: Kart Çerçevelerinin Belirginliği
**Tarih:** 12 Haziran 2025
**Sorun:** Kartlar sayfada yeterince belirgin görünmüyordu ve kullanıcılar için ayırt edilmesi zordu.
**Nedeni:** Kartların çerçeveleri yoktu veya çok hafifti, bu da kartların arka plandan yeterince ayrılmamasına neden oluyordu.
**Çözüm:**
1. Kartlara belirgin mavi çerçeveler ekledik (2px kalınlığında, #93c5fd renginde).
2. Hover durumunda çerçeve rengini daha koyu bir mavi (#3B82F6) olarak değiştirdik.
3. Böylece kartlar sayfada daha belirgin hale geldi ve kullanıcı etkileşimi daha görünür oldu.

```scss
.user-card, .permission-card {
  // ... existing styles ...
  border: 2px solid #93c5fd; /* Daha kalın ve daha belirgin mavi çerçeve */
  
  &:hover {
    // ... existing styles ...
    border-color: #3B82F6; /* Hover durumunda daha koyu mavi */
  }
}
```

### Sorun: PowerShell'de && Operatörü Kullanımı
**Tarih:** 12 Haziran 2025
**Hata:** PowerShell'de `cd frontend && npm run start` komutu çalıştırılamadı.
**Hata Mesajı:**
```
At line:1 char:13
+ cd frontend && npm run start
+             ~~
The token '&&' is not a valid statement separator in this version.
```
**Nedeni:** PowerShell, komutları birleştirmek için `&&` operatörünü desteklemiyor.
**Çözüm:**
1. PowerShell'de komutları birleştirmek için `;` (noktalı virgül) kullanıldı: `cd frontend; npm run start`
2. Alternatif olarak, komutlar ayrı ayrı çalıştırılabilir:
   ```powershell
   cd frontend
   npm run start
   ```

### Sorun: Sayfalama Kontrollerinin Tasarımı
**Tarih:** 12 Haziran 2025
**Sorun:** Sayfalama kontrolleri yeterince modern ve kullanıcı dostu değildi.
**Çözüm:**
1. Sayfalama kontrollerini modernize ettik (yuvarlak köşeler, gölgeler, hover efektleri).
2. İlk ve son sayfa butonları ekledik.
3. Sayfa başına gösterilen kart sayısını 10'dan 12'ye çıkardık.
4. Toplam sayfa ve mevcut sayfa bilgisini daha belirgin hale getirdik.

```scss
.pagination-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 2rem;
  padding-top: 1rem;
  border-top: 1px solid #e2e8f0;

  .pagination-controls {
  display: flex;
  align-items: center;
    gap: 0.75rem;
    background-color: #f8fafc;
    padding: 0.5rem;
  border-radius: 8px;
    box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
    
    // ... other styles ...
  }
}
```

### Sorun: Responsive Tasarım Sorunları
**Tarih:** 12 Haziran 2025
**Sorun:** Sayfa, mobil cihazlarda düzgün görüntülenmiyordu.
**Çözüm:**
1. Medya sorguları ekleyerek mobil cihazlar için özel stiller tanımladık.
2. Mobil görünümde kartları tek sütun olarak düzenledik.
3. Başlık ve alt başlık boyutlarını küçülttük.
4. Sayfalama kontrollerini dikey olarak düzenledik.

```scss
@media screen and (max-width: 768px) {
  .user-page-permissions-container {
    padding: 1rem;
  }
  
  .page-header {
  flex-direction: column;
    gap: 1rem;
    
    // ... other styles ...
  }
  
  .user-cards, .permission-cards {
    grid-template-columns: 1fr;
  }
  
  // ... other responsive styles ...
}
```

### Sorun: Kullanıcı Oluşturma İşleminde 500 Internal Server Error
**Tarih:** 13 Haziran 2025
**Hata:** Yeni kullanıcı oluşturulurken 500 Internal Server Error hatası alındı.
**Hata Mesajı:**
```
POST http://localhost:5037/api/auth/create-user 500 (Internal Server Error)
Kullanıcı oluşturma hatası: HttpErrorResponse {headers: _HttpHeaders, status: 500, statusText: 'Internal Server Error', url: 'http://localhost:5037/api/auth/create-user', ok: false, …}
Kullanıcı işlemi sırasında hata oluştu: Error: Kullanıcı oluşturulurken bir hata oluştu
```
**Nedeni:** Frontend tarafından gönderilen veriler, backend'in beklediği formatta değildi. Özellikle `roleId` alanı eksik veya geçersiz olabilir.
**Çözüm:**
1. Frontend tarafında `saveUser` metodunda, kullanıcı oluşturma işlemi sırasında gönderilen verileri kontrol ettik.
2. `roleId` ve `sicil` alanlarının doğru şekilde gönderildiğinden emin olduk.
3. `userService.createUser` ve `authService.createUser` metodlarında hata yakalama mekanizmalarını geliştirdik.
4. Hata mesajlarını daha detaylı hale getirdik.

```typescript
// Frontend tarafında veri kontrolü
if (!formData.roleId || formData.roleId <= 0) {
  this.messageService.add({
    severity: 'error',
    summary: 'Hata',
    detail: 'Geçerli bir rol seçilmelidir',
    life: 5000
  });
  return;
}

// Hata yakalama ve işleme
catchError(error => {
  console.error('Kullanıcı oluşturma hatası:', error);
  
  // Hata mesajını düzenle
  let errorMessage = 'Kullanıcı oluşturulurken bir hata oluştu';
  
  // HTTP durum kodu kontrolü
  if (error.status === 500) {
    // Sunucu hatası için daha detaylı bilgi
    if (error.error && error.error.details) {
      errorMessage = `Sunucu hatası: ${error.error.details}`;
    } else {
      errorMessage = 'Sunucu tarafında bir hata oluştu. Lütfen daha sonra tekrar deneyin.';
    }
  }
  
  // Observable olarak hata mesajını döndür
  return throwError(() => new Error(errorMessage));
})
```

### Sorun: Kullanıcı Adı Benzersizlik Kontrolü Sorunu
**Tarih:** 18 Haziran 2025
**Hata:** Kullanıcı oluşturma veya güncelleme sırasında 500 Internal Server Error hatası alınıyor.
**Hata Mesajı:**
```
POST http://localhost:5037/api/auth/create-user 500 (Internal Server Error)
Sunucu hatası: An error occurred while saving the entity changes. See the inner exception for details.
```
**Nedeni:** Veritabanında Username sütunu için bir benzersizlik kısıtlaması var. Kullanıcı adı olarak sadece kullanıcının tam adını (fullName) kullandığımızda, aynı ada sahip iki kullanıcı oluşturmaya çalıştığımızda bu kısıtlama ihlal ediliyor.

**Çözüm:**
1. Kullanıcı adı (username) alanını, tam ad (fullName) ve sicil numarasının birleşimi olarak oluşturuyoruz.
2. Bu, her kullanıcı için benzersiz bir kullanıcı adı garantiliyor.
3. Arayüzde hala kullanıcının tam adını gösteriyoruz, ancak veritabanında benzersiz bir kullanıcı adı saklıyoruz.

```typescript
// Kullanıcı adını oluşturma - fullName ve sicil birleştirerek
formData.username = `${formData.fullName}_${formData.sicil}`; // Benzersiz kullanıcı adı oluştur
```

**Öğrenilen Dersler:**
1. Veritabanı kısıtlamalarını frontend tarafında da dikkate almalıyız.
2. Arayüzde gösterilen bilgi ile veritabanında saklanan bilgi farklı olabilir.
3. Benzersizlik gerektiren alanlar için, benzersizliği garantileyecek stratejiler geliştirmeliyiz.
4. Sicil numarası gibi zaten benzersiz olan alanları kullanarak, diğer alanların benzersizliğini sağlayabiliriz.
5. Hata mesajları, sorunun kaynağını belirlemek için önemli ipuçları sağlar.

### Sorun: Kullanıcı Adı Karmaşıklığı
**Tarih:** 16 Haziran 2025
**Sorun:** Kullanıcı adları, sicil numarası eklendiğinde çok karmaşık hale geliyordu ve kullanıcı deneyimini olumsuz etkiliyordu.
**Nedeni:** Benzersizlik sağlamak için kullanıcı adının sonuna sicil numarası eklenmesi, kullanıcı adlarını karmaşık ve okunması zor hale getiriyordu.
**Çözüm:**
1. Kullanıcı adı oluşturma mantığını değiştirdik.
2. Kullanıcı adı olarak sadece kullanıcının tam adını (fullName) kullanıyoruz.
3. Aynı isimde bir kullanıcı varsa, kullanıcı adının sonuna sicil numarasını ekleyerek benzersizlik sağlıyoruz.
4. Bu şekilde, kullanıcı adları mümkün olduğunca basit ve anlaşılır kalıyor, ancak gerektiğinde benzersizlik de sağlanıyor.

```typescript
// Kullanıcı adı olarak sadece fullName kullanarak oluşturalım
formData.username = formData.fullName; // Boşlukları koruyoruz

// Aynı kullanıcı adı varsa, kullanıcı adının sonuna sicil numarasını ekleyelim
const existingUserWithSameUsername = this.users.find(u => u.username === formData.username);
if (existingUserWithSameUsername) {
  // Kullanıcı adının sonuna sicil numarasını ekle
  formData.username = `${formData.fullName}_${formData.sicil}`;
}
```

### Sorun: Silinen Kullanıcıların Sicil Numaralarının Tekrar Kullanılamaması
**Tarih:** 13 Mart 2025
**Hata:** Silinen bir kullanıcının sicil numarası ile yeni bir kullanıcı oluşturulamıyor.
**Hata Mesajı:**
```
Sicil numarası A789 zaten kullanımda.
```
**Nedeni:** Kullanıcılar tamamen silinmiyor, sadece "IsDeleted" alanı true olarak işaretleniyor (soft delete). Ancak sicil numarası benzersizlik kontrolü, silinmiş kullanıcıları da dikkate alıyordu.
**Çözüm:**
1. `UserConfiguration.cs` dosyasında sicil numarası benzersizlik kontrolünü sadece silinmemiş kullanıcılar için yapacak şekilde güncelledik:
```csharp
builder.HasIndex(x => x.Sicil)
    .IsUnique()
    .HasFilter(""IsDeleted" = false"); // Sadece silinmemiş kullanıcılar için benzersizlik kontrolü
```
2. Yeni bir migration oluşturup veritabanını güncelledik:
```powershell
dotnet ef migrations add UpdateSicilUniqueConstraintForActiveUsers -s ../Stock.API/Stock.API.csproj
dotnet ef database update -s ../Stock.API/Stock.API.csproj
```

**Öğrenilen Dersler:**
1. Soft delete (yumuşak silme) kullanırken, benzersizlik kısıtlamalarını dikkatli bir şekilde tasarlamalıyız.
2. Entity Framework Core'da `HasFilter` kullanarak, benzersizlik kısıtlamalarını belirli koşullara göre filtreleyebiliriz.
3. Silinen kayıtların tekrar kullanılabilmesi için, benzersizlik kontrollerini sadece aktif kayıtlar için yapmalıyız.
4. Veritabanı değişiklikleri için her zaman migration kullanmalıyız.
5. Değişiklikler öncesinde GitHub'a push etmek, sorun çıkması durumunda geri dönebilmek için önemlidir.

## Sorun: Kullanıcı Adı Formatı Değişikliği (19 Haziran 2025)

### Hata
Kullanıcı adı oluşturma sırasında fullName_sicil formatı kullanılıyordu, ancak iş gereksinimi sadece ad soyad bilgisinin kullanıcı adı olarak kullanılmasını gerektiriyordu.

### Çözüm
Kullanıcı adı oluşturma mantığı değiştirildi. Artık kullanıcı adı olarak **sadece** fullName (ad soyad) kullanılıyor, sicil numarası eklenmiyor.

```typescript
// ESKİ KOD:
formData.username = `${formData.fullName}_${formData.sicil}`;

// YENİ KOD:
formData.username = formData.fullName;
```

### Dikkat Edilmesi Gereken Noktalar
- Bu değişiklikle artık kullanıcı adı, sadece ad soyad bilgisinden oluşuyor.
- Eğer veritabanında Username alanı için benzersizlik kısıtlaması varsa, aynı ada sahip iki kullanıcı oluşturmaya çalıştığında "500 Internal Server Error" hatası alınabilir.
- Bu durumda veritabanındaki Username alanı için benzersizlik kısıtlamasının kaldırılması gerekecektir.

### Öğrenilen Ders
- Kullanıcı deneyimini iyileştirmek için, kullanıcı adları mümkün olduğunca basit ve anlaşılır olmalıdır.
- Frontend ve backend arasındaki kısıtlamaların uyumlu olması gerekir.
- Kullanıcı gereksinimlerinin açık bir şekilde anlaşılması, gereksiz değişikliklerin ve hataların önüne geçer.

## SCSS Bütçe Aşımı Hatası

### Hata Mesajı

```
Error: SCSS dosyaları bütçe sınırını aşıyor:
- user-management.component.scss: 69.78 kB (16 kB sınırını 53.77 kB aşıyor)
- user-page-permissions.component.scss: 17.54 kB (16 kB sınırını 1.53 kB aşıyor)
- role-management.component.scss: 16.29 kB (16 kB sınırını 287 bytes aşıyor)
```

### Hatanın Nedeni

Angular projelerinde, bileşen stil dosyaları için bir bütçe sınırı vardır. Bu sınır, performans sorunlarını önlemek için konulmuştur. Projemizdeki bazı SCSS dosyaları bu sınırı aşıyordu. Bunun nedeni:

1. Tekrarlanan stiller
2. Gereksiz derinlikte seçiciler
3. Aynı özelliklerin tekrarı
4. Çok fazla ::ng-deep kullanımı

### Çözüm

1. Ortak stilleri `frontend/src/app/shared/styles/common.scss` dosyasına taşıdık
2. PrimeNG özelleştirmelerini `frontend/src/app/shared/styles/primeng-theme.scss` dosyasına taşıdık
3. Ana stil dosyasına (`frontend/src/styles.scss`) bu ortak dosyaları ekledik
4. Bileşen SCSS dosyalarını optimize ederek sadece bileşene özel stilleri bıraktık
5. Angular bütçe ayarlarını güncelleyerek bileşen stil dosyaları için bütçe sınırını artırdık

Detaylı bilgi için: [SCSS Optimizasyonu ve Bütçe Aşımı Çözümü](knowledge-base/scss-optimization.md)

## SCSS Optimizasyonu Sonrası Tasarım Bozulması Sorunu

### Hata Mesajı

```
Kullanıcı yönetimi sayfasının tasarımı bozuldu. Tablo yapısı düzgün görüntülenmiyor.
```

### Hatanın Nedeni

SCSS optimizasyonu sırasında, ortak stilleri ayırırken bazı bileşene özel stiller eksik kalmıştı. Özellikle kullanıcı yönetimi sayfasında kullanılan tablo yapısına ait stiller, bileşenin kendi SCSS dosyasında bulunmuyordu. Bu durum, sayfanın tasarımının bozulmasına neden oldu.

Eksik kalan stiller şunlardı:
- Tablo başlık satırı stilleri
- Tablo veri satırları stilleri
- Hücre düzenleri
- Özel checkbox'lar
- Sayfalama kontrolleri
- İzin rozetleri
- İşlem butonları

### Çözüm

1. Eksik kalan tablo stillerini tespit ettik
2. Bu stilleri kullanıcı yönetimi bileşeninin SCSS dosyasına ekledik:

```scss
// Kullanıcı tablosu stilleri
.user-table {
  width: 100%;
  border-collapse: collapse;
  
  .table-header {
    display: flex;
    background-color: #f8fafc;
    border-bottom: 1px solid #e2e8f0;
    font-weight: 600;
    color: #475569;
    
    // ... diğer stiller ...
  }
  
  .table-row {
    display: flex;
    border-bottom: 1px solid #e2e8f0;
    transition: background-color 0.2s;
    
    // ... diğer stiller ...
  }
}

// ... diğer bileşene özel stiller ...
```

3. Sayfayı test ederek tasarımın düzgün görüntülendiğini doğruladık

### Öğrenilen Dersler

1. **Stil Envanteri Çıkarma**: SCSS optimizasyonu öncesinde, her bileşenin kullandığı tüm stillerin bir envanterini çıkarmak önemlidir.
2. **Bileşene Özel Stilleri Koruma**: Ortak stilleri ayırırken, bileşene özel olan stilleri bileşenin kendi SCSS dosyasında tutmaya devam etmek gerekir.
3. **Görsel Karşılaştırma**: Optimizasyon sonrasında, her bileşenin görünümünü optimizasyon öncesiyle karşılaştırarak eksik kalan stilleri tespit etmek faydalıdır.
4. **Test Süreci**: Her bileşeni farklı ekran boyutlarında test ederek stil sorunlarını erken tespit etmek önemlidir.
5. **Stil Kategorileri**: Stilleri "ortak", "tema" ve "bileşene özel" olarak kategorilere ayırmak, hangi stillerin nerede tanımlanması gerektiğini belirlemeye yardımcı olur.

Detaylı bilgi için: [SCSS Optimizasyonu ve Bütçe Aşımı Çözümü](knowledge-base/scss-optimization.md)

## SCSS Optimizasyonu Sonrası Rol Renklendirme Özelliğinin Kaybolması

### Hata
SCSS dosyalarının optimizasyonu ve ortak stillerin `common.scss` dosyasına taşınması sonrasında, kullanıcı yönetimi sayfasında izinler sütunundaki rol rozetlerinin otomatik renklendirme özelliği kayboldu.

### Nedeni
Rol renklendirme için gerekli olan CSS sınıfları (`admin-badge`, `contributor-badge`, `blue-badge`, vb.) `user-management.component.scss` dosyasından `common.scss` dosyasına taşınırken eksik kalmıştı. Bu nedenle, `getRoleColorClass` metodu tarafından döndürülen sınıf adları artık mevcut değildi.

### Çözüm
1. `knowledge-base/feature_modules/user_management_role_colors.md` dosyasındaki bilgiler incelendi.
2. `frontend/src/app/features/user-management/components/user-management.component.scss` dosyasına eksik olan renk sınıfları eklendi:
   ```scss
   .permission-badge {
     // Mevcut stiller...
     
     &.admin-badge {
       background-color: rgba(244, 67, 54, 0.1);
       color: #f44336; // Kırmızı
     }
     
     &.contributor-badge {
       background-color: rgba(33, 150, 243, 0.1);
       color: #2196f3; // Mavi
     }
     
     // Otomatik renk sınıfları
     &.blue-badge {
       background-color: rgba(33, 150, 243, 0.1);
       color: #2196f3; // Mavi
     }
     
     // Diğer renk sınıfları...
   }
   ```

### Öğrenilen Dersler
1. SCSS optimizasyonu yaparken, sadece genel stilleri değil, özel işlevsellik sağlayan sınıfları da dikkate almak gerekir.
2. Stil değişikliklerinden sonra, tüm özel işlevselliğin (renklendirme, animasyonlar, vb.) hala çalıştığından emin olmak için kapsamlı test yapılmalıdır.
3. Özel işlevsellik sağlayan stil sınıflarını belgelemek ve bu belgeleri SCSS optimizasyonu sırasında referans olarak kullanmak önemlidir.

## Entity Framework Core Tablo Çakışması Hatası

### Hata Mesajı
```
System.InvalidOperationException: Cannot use table 'RolePermission' for entity type 'RolePermission' since it is being used for entity type 'RolePermission' and potentially other entity types, but there is no linking relationship.
```

### Problem
Projede aynı isimde ancak farklı namespace'lerde sınıflar tanımlanmıştı:
- `Stock.Domain.Entities.RolePermission` ve `Stock.Domain.Entities.Permissions.RolePermission`
- `Stock.Domain.Entities.UserPermission` ve `Stock.Domain.Entities.Permissions.UserPermission`

Entity Framework Core, bu sınıfları aynı tablolara eşleştirmeye çalışıyor, ancak aralarında bir ilişki olmadığı için hata veriyordu.

### Çözüm Adımları
1. `UserPermissions` referanslarını `UserPermission` olarak değiştirdik.
2. `Stock.Domain.Entities.Permissions` namespace'indeki sınıfları kullanmaya karar verdik.
3. Tüm referansları buna göre güncelledik.
4. Eski sınıfları (`Stock.Domain.Entities` namespace'indeki) kaldırdık veya yeniden adlandırdık.
5. Veritabanı şemasını güncellemek için yeni bir migrasyon oluşturduk.
6. Tüm referansları tek bir namespace'e yönlendirdik.

### Öğrenilen Dersler
1. Aynı isimde ancak farklı namespace'lerde sınıflar oluşturmaktan kaçının.
2. Migrasyon oluşturmadan önce, modellerin ve veritabanı şemasının uyumlu olduğundan emin olun.
3. Migrasyon uygulamadan önce, migrasyon dosyasını dikkatlice inceleyin.
4. Veritabanı şemasını düzenli olarak kontrol edin ve kod tarafındaki modellerle senkronize olduğundan emin olun.

## Migrasyon Hatası: Sütun Zaten Var

### Hata Mesajı
```
42701: column "CreatedBy" of relation "UserPermissions" already exists
```

### Problem
Migrasyon dosyasında (`20250313222014_ConsolidatePermissionEntities.cs`), `UserPermissions` tablosuna `CreatedBy` sütunu eklenmeye çalışılıyor, ancak bu sütun zaten tabloda mevcut. Bu durum, veritabanı şemasının kod tarafındaki modelle senkronize olmadığını gösteriyor.

Migrasyon şu işlemleri yapmaya çalışıyor:
1. Eski `Permission`, `RolePermission` ve `UserPermission` tablolarını silme
2. Yeni `UserPermissions` tablosuna `CreatedBy`, `IsDeleted` ve `UpdatedBy` sütunlarını ekleme
3. Yeni `RolePermissions` tablosuna `CreatedBy`, `IsDeleted`, `UpdatedAt` ve `UpdatedBy` sütunlarını ekleme
4. `Permissions` tablosundaki bazı sütunların özelliklerini değiştirme

### Çözüm
1. Migrasyon dosyasını düzenleyerek, zaten var olan sütunları ekleme işlemlerini kaldırmalıyız.
2. Alternatif olarak, mevcut migrasyonu silip yeni bir migrasyon oluşturabiliriz.

### Adımlar
1. Mevcut migrasyonu kaldırın:
   ```
   dotnet ef migrations remove -c ApplicationDbContext
   ```

2. Veritabanı şemasını kontrol edin ve mevcut tabloların yapısını anlayın:
   ```sql
   SELECT column_name, data_type 
   FROM information_schema.columns 
   WHERE table_name = 'UserPermissions';
   ```

3. Modelleri ve DbContext'i güncelleyerek mevcut veritabanı şemasıyla uyumlu hale getirin.

4. Yeni bir migrasyon oluşturun:
   ```
   dotnet ef migrations add ConsolidatePermissionEntities -c ApplicationDbContext
   ```

5. Yeni migrasyonu dikkatlice inceleyin ve gerekirse düzenleyin.

6. Migrasyonu uygulayın:
   ```
   dotnet ef database update -c ApplicationDbContext
   ```

## Entity Framework Core Tablo Çakışması Hatası

### Hata Mesajı
```
The entity type 'RolePermission' is part of a foreign key relationship with 'RolePermission' but there is no entity type defined for navigation 'RolePermission.Permission'.
```

### Problem
Projede iki farklı namespace'te aynı isimde sınıflar bulunuyor:
- `Stock.Domain.Entities.RolePermission` ve `Stock.Domain.Entities.Permissions.RolePermission`
- `Stock.Domain.Entities.UserPermission` ve `Stock.Domain.Entities.Permissions.UserPermission`

Entity Framework Core, bu sınıfları aynı tabloya eşlemeye çalışıyor ancak aralarında ilişki tanımlanmadığı için hata veriyor.

### Çözüm Adımları
1. `UserPermissions` referanslarını `UserPermission` olarak değiştirdik.
2. `Stock.Domain.Entities.Permissions` namespace'indeki sınıfları kullanmaya karar verdik.
3. Tüm referansları buna göre güncelledik.
4. Eski sınıfları (`Stock.Domain.Entities` namespace'indeki) kaldırdık veya yeniden adlandırdık.
5. Veritabanı şemasını güncellemek için yeni bir migrasyon oluşturduk.
6. Tüm referansları tek bir namespace'e yönlendirdik.

### Öğrenilen Dersler
1. Aynı isimde ancak farklı namespace'lerde sınıflar oluşturmaktan kaçının.
2. Migrasyon oluşturmadan önce, modellerin ve veritabanı şemasının uyumlu olduğundan emin olun.
3. Migrasyon uygulamadan önce, migrasyon dosyasını dikkatlice inceleyin.
4. Veritabanı şemasını düzenli olarak kontrol edin ve kod tarafındaki modellerle senkronize olduğundan emin olun.

## Admin Kullanıcısı Giriş Sorunu

### Hata Mesajı
```
POST http://localhost:5037/api/auth/login 401 (Unauthorized)
```

### Sorun Açıklaması
Veritabanı yeniden oluşturulduktan sonra admin kullanıcısı giriş yapamıyordu. Sicil numarası "A001" ve şifre "Admin123" ile giriş denemesi yapıldığında 401 Unauthorized hatası alınıyordu. Sunucu loglarında "Kullanıcı bulunamadı: Sicil No: A001" hatası görülüyordu.

### Çözüm Adımları
1. Veritabanını sıfırladık: `dotnet ef database drop -f`
2. Migrasyonları yeniden uyguladık: `dotnet ef database update`
3. Uygulamayı yeniden başlattık: `dotnet run`

Bu işlemler sonucunda, veritabanı yeniden oluşturuldu ve seed data içinde admin kullanıcısı da oluşturuldu. Admin kullanıcısı artık "A001" sicil numarası ve "Admin123" şifresi ile giriş yapabilir hale geldi.

### Öğrenilen Dersler
1. Veritabanı şeması değiştiğinde, veritabanını tamamen sıfırlamak ve migrasyonları yeniden uygulamak bazen en temiz çözüm olabilir.
2. Seed data'nın doğru şekilde uygulandığından emin olmak için veritabanı işlemlerinden sonra kontrol yapmak önemlidir.
3. Kullanıcı kimlik doğrulama sorunlarında, sunucu loglarını incelemek sorunun kaynağını bulmak için kritik öneme sahiptir.

## ActivityLog Tablosu Yabancı Anahtar Hatası

### Hata Mesajı
```
POST http://localhost:5037/api/logs/bulk-activity 500 (Internal Server Error)
insert or update on table "ActivityLogs" violates foreign key constraint "FK_ActivityLogs_Users_UserId"
```

### Sorun Açıklaması
Admin paneline giriş yapıldığında, `LogService` sınıfındaki `syncPendingLogs` metodu çalışıyor ve localStorage'da bekleyen logları toplu olarak sunucuya göndermeye çalışıyordu. Ancak, frontend'den gelen log verilerindeki UserId değerleri, veritabanındaki Users tablosunda mevcut olmadığı için yabancı anahtar kısıtlaması hatası alınıyordu.

### Çözüm Adımları
1. `ActivityLogController.cs` dosyasını inceledik ve `CreateBulkActivityLogs` metodunu analiz ettik.
2. UserId kontrolünü güncelledik, veritabanında olmayan UserId değerleri için null atanmasını sağladık:

```csharp
if (dynamicLog.TryGetValue("userId", out var userIdElement) && userIdElement.ValueKind != JsonValueKind.Null)
{
    try
    {
        var userId = userIdElement.GetInt32();
        
        // Kullanıcının veritabanında var olup olmadığını kontrol et
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        
        if (userExists)
        {
            log.UserId = userId;
        }
        else
        {
            _logger.LogWarning($"Belirtilen userId ({userId}) veritabanında bulunamadı. UserId null olarak ayarlanıyor.");
            log.UserId = null; // Kullanıcı bulunamadıysa null olarak ayarla
        }
    }
    catch (Exception ex)
    {
        _logger.LogWarning($"userId alanı int'e dönüştürülemedi: {ex.Message}. UserId null olarak ayarlanıyor.");
        log.UserId = null; // Hata durumunda null olarak ayarla
    }
}
else
{
    _logger.LogWarning("Log kaydında userId bulunamadı veya null. UserId null olarak ayarlanıyor.");
    log.UserId = null; // UserId yoksa null olarak ayarla
}
```

3. `ActivityLog` entity sınıfında UserId alanını nullable yaptık: `public int? UserId { get; set; }`
4. Yeni bir migration oluşturduk: `dotnet ef migrations add FixActivityLogUserIdNullable -p ../Stock.Infrastructure`
5. Migration dosyasını düzenleyerek `ActivityLogs` tablosundaki UserId alanını nullable yapmak için SQL komutları ekledik:

```sql
-- Foreign key kısıtlamasını kaldır
ALTER TABLE "ActivityLogs" 
DROP CONSTRAINT "FK_ActivityLogs_Users_UserId";

-- UserId alanını nullable yap
ALTER TABLE "ActivityLogs" 
ALTER COLUMN "UserId" DROP NOT NULL;

-- Foreign key kısıtlamasını tekrar ekle (ON DELETE SET NULL ile)
ALTER TABLE "ActivityLogs" 
ADD CONSTRAINT "FK_ActivityLogs_Users_UserId" 
FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") 
ON DELETE SET NULL;
```

6. Veritabanını güncelledik: `dotnet ef database update`
7. Uygulamayı yeniden başlattık ve sorunun çözüldüğünü doğruladık.

### Öğrenilen Dersler
1. Yabancı anahtar ilişkilerinde, ilişkili kaydın var olup olmadığını kontrol etmek önemlidir.
2. Veritabanı kısıtlamalarını dikkate alarak, gerektiğinde null değerler atamak veri bütünlüğünü korur.
3. Hata durumlarını detaylı şekilde loglayarak, sorunların kaynağını daha kolay tespit edebiliriz.
4. Entity Framework Core'un `AnyAsync` gibi metodları, veritabanı sorgularını optimize etmek için kullanılabilir.
5. Veri dönüşümlerinde try-catch blokları kullanarak, beklenmeyen format hatalarına karşı koruma sağlayabiliriz.
6. Migration dosyalarını manuel olarak düzenleyerek, Entity Framework Core'un otomatik olarak oluşturamadığı veya algılayamadığı değişiklikleri yapabiliriz.

## ActivityLogController'da UserId Kontrolü Güncelleme Sorunu

### Hata Mesajı
```
POST http://localhost:5037/api/logs/bulk-activity 500 (Internal Server Error)
insert or update on table "ActivityLogs" violates foreign key constraint "FK_ActivityLogs_Users_UserId"
```

### Sorun Açıklaması
Admin paneline giriş yapıldığında, `LogService` sınıfındaki `syncPendingLogs` metodu çalışıyor ve localStorage'da bekleyen logları toplu olarak sunucuya göndermeye çalışıyordu. Ancak, frontend'den gelen log verilerindeki UserId değerleri, veritabanındaki Users tablosunda mevcut olmadığı için yabancı anahtar kısıtlaması hatası alınıyordu.

### Çözüm Adımları
1. `ActivityLogController.cs` dosyasını inceledik ve `CreateBulkActivityLogs` metodunu analiz ettik.
2. UserId kontrolünü güncelledik, veritabanında olmayan UserId değerleri için null atanmasını sağladık:

```csharp
if (dynamicLog.TryGetValue("userId", out var userIdElement) && userIdElement.ValueKind != JsonValueKind.Null)
{
    try
    {
        var userId = userIdElement.GetInt32();
        
        // Kullanıcının veritabanında var olup olmadığını kontrol et
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        
        if (userExists)
        {
            log.UserId = userId;
        }
        else
        {
            _logger.LogWarning($"Belirtilen userId ({userId}) veritabanında bulunamadı. UserId null olarak ayarlanıyor.");
            log.UserId = null; // Kullanıcı bulunamadıysa null olarak ayarla
        }
    }
    catch (Exception ex)
    {
        _logger.LogWarning($"userId alanı int'e dönüştürülemedi: {ex.Message}. UserId null olarak ayarlanıyor.");
        log.UserId = null; // Hata durumunda null olarak ayarla
    }
}
else
{
    _logger.LogWarning("Log kaydında userId bulunamadı veya null. UserId null olarak ayarlanıyor.");
    log.UserId = null; // UserId yoksa null olarak ayarla
}
```

3. `ActivityLog` entity sınıfında UserId alanını nullable yaptık: `public int? UserId { get; set; }`
4. Yeni bir migration oluşturduk: `dotnet ef migrations add FixActivityLogUserIdNullable -p ../Stock.Infrastructure`
5. Migration dosyasını düzenleyerek `ActivityLogs` tablosundaki UserId alanını nullable yapmak için SQL komutları ekledik:

```sql
-- Foreign key kısıtlamasını kaldır
ALTER TABLE "ActivityLogs" 
DROP CONSTRAINT "FK_ActivityLogs_Users_UserId";

-- UserId alanını nullable yap
ALTER TABLE "ActivityLogs" 
ALTER COLUMN "UserId" DROP NOT NULL;

-- Foreign key kısıtlamasını tekrar ekle (ON DELETE SET NULL ile)
ALTER TABLE "ActivityLogs" 
ADD CONSTRAINT "FK_ActivityLogs_Users_UserId" 
FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") 
ON DELETE SET NULL;
```

6. Veritabanını güncelledik: `dotnet ef database update`
7. Uygulamayı yeniden başlattık ve sorunun çözüldüğünü doğruladık.

### Öğrenilen Dersler
1. Yabancı anahtar ilişkilerinde, ilişkili kaydın var olup olmadığını kontrol etmek önemlidir.
2. Veritabanı kısıtlamalarını dikkate alarak, gerektiğinde null değerler atamak veri bütünlüğünü korur.
3. Hata durumlarını detaylı şekilde loglayarak, sorunların kaynağını daha kolay tespit edebiliriz.
4. Entity Framework Core'un `AnyAsync` gibi metodları, veritabanı sorgularını optimize etmek için kullanılabilir.
5. Veri dönüşümlerinde try-catch blokları kullanarak, beklenmeyen format hatalarına karşı koruma sağlayabiliriz.
6. Migration dosyalarını manuel olarak düzenleyerek, Entity Framework Core'un otomatik olarak oluşturamadığı veya algılayamadığı değişiklikleri yapabiliriz.
