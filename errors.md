# Hata Kayıtları ve Çözümleri

## Kod İyileştirme Çalışmaları

### Frontend Servis İyileştirmeleri
**Tarih:** 20 Haziran 2025
**Sorun:** Frontend servislerinde gereksiz console.log ifadeleri, magic string/number kullanımı ve tutarsız hata yönetimi vardı.
**Nedeni:** Geliştirme sürecinde debug amaçlı eklenen console.log ifadeleri temizlenmemiş, sabit değerler doğrudan kodda kullanılmış ve hata yönetimi her serviste farklı şekilde uygulanmıştı.
**Çözüm:**
1. Tüm servis dosyalarından gereksiz console.log, console.warn ve console.error ifadeleri kaldırıldı.
2. HTTP durum kodları ve hata kodları için sabit değişkenler oluşturuldu:
   ```typescript
   export const HttpStatusCodes = {
     OK: 200,
     BAD_REQUEST: 400,
     UNAUTHORIZED: 401,
     FORBIDDEN: 403,
     NOT_FOUND: 404,
     SERVER_ERROR: 500,
     CONNECTION_ERROR: 0
   };
   
   export const ErrorCodes = {
     DUPLICATE_SICIL: 'DuplicateSicil',
     DUPLICATE_USERNAME: 'DuplicateUsername'
   };
   ```
3. Tüm servislerde tutarlı bir hata yönetimi yaklaşımı uygulandı:
   ```typescript
   private handleError(error: HttpErrorResponse): Observable<never> {
     let errorMessage = 'Bir hata oluştu';
     
     if (error.error instanceof ErrorEvent) {
       // İstemci taraflı hata
       errorMessage = `Hata: ${error.error.message}`;
     } else {
       // Sunucu taraflı hata
       if (error.status === HttpStatusCodes.UNAUTHORIZED) {
         errorMessage = 'Oturum süresi dolmuş veya yetkiniz yok. Lütfen tekrar giriş yapın.';
       } else if (error.status === HttpStatusCodes.FORBIDDEN) {
         errorMessage = 'Bu işlemi gerçekleştirmek için yetkiniz yok.';
       } else if (error.status === HttpStatusCodes.NOT_FOUND) {
         errorMessage = 'İstek yapılan kaynak bulunamadı.';
       } else if (error.status === HttpStatusCodes.SERVER_ERROR) {
         errorMessage = 'Sunucu hatası. Lütfen daha sonra tekrar deneyin.';
       } else if (error.status === HttpStatusCodes.CONNECTION_ERROR) {
         errorMessage = 'Sunucuya bağlanılamadı. İnternet bağlantınızı kontrol edin.';
       } else {
         errorMessage = `Sunucu hatası: ${error.status}, mesaj: ${error.message}`;
       }
     }
     
     return throwError(() => new Error(errorMessage));
   }
   ```
4. API yanıtlarını işleyen normalizeResponse metodu iyileştirildi ve tüm servislerde tutarlı hale getirildi.
5. Tüm HTTP isteklerinde Authorization token'ı ve Content-Type header'ları eklendi.
6. İyileştirilen dosyalar:
   - user.service.ts
   - role.service.ts
   - permission.service.ts
   - user-permission.service.ts
   - password.service.ts

**Öğrenilen Dersler:**
1. Üretim kodunda console.log ifadeleri bulunmamalıdır. Geliştirme sürecinde eklenen log ifadeleri, geliştirme tamamlandığında temizlenmelidir.
2. Magic string ve magic number kullanımından kaçınılmalı, bunun yerine anlamlı isimlerle sabit değişkenler kullanılmalıdır.
3. Hata yönetimi tüm uygulamada tutarlı olmalıdır. Benzer hatalar için benzer mesajlar gösterilmelidir.
4. HTTP istekleri için standart bir yaklaşım benimsenmeli ve tüm servislerde uygulanmalıdır.
5. Kod tekrarından kaçınmak için ortak işlevsellik (örneğin, API yanıt işleme) merkezileştirilmelidir.

**Sonraki Adımlar:**
1. Ortak işlevselliği içeren bir BaseService sınıfı oluşturulabilir.
2. Tip güvenliği artırılabilir (any tipinin azaltılması).
3. HTTP istekleri için interceptor oluşturulabilir.
4. Daha kapsamlı hata yönetimi ve loglama stratejisi uygulanabilir.

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
formData.username = formData.fullName;

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

## PostgreSQL Veri Tipi Dönüşüm Hatası

### Hata Mesajı
```
42804: column "UserId" cannot be cast automatically to type integer
Hint: You might need to specify "USING "UserId"::integer".
```

### Sorun Açıklaması
`FixDatabaseRelationships` migrasyonu uygulanırken, `AuditLogs` tablosundaki `UserId` sütununu string tipinden integer tipine dönüştürmeye çalışırken hata aldık. PostgreSQL, farklı veri tipleri arasında otomatik dönüşüm yapmaz ve bu durumda özel bir USING cümlesi kullanmamız gerektiğini belirtti.

### Çözüm Adımları
1. Migrasyonu kaldırdık: `dotnet ef migrations remove -p ../Stock.Infrastructure`
2. Yeni bir migrasyon oluşturduk: `dotnet ef migrations add FixDatabaseRelationships -p ../Stock.Infrastructure`
3. Migrasyon dosyasını düzenleyerek, `AlterColumn` komutunu özel bir SQL komutuyla değiştirdik:

```csharp
// Özel SQL komutu kullanarak string'den integer'a dönüşüm
migrationBuilder.Sql(@"
    ALTER TABLE ""AuditLogs"" 
    ALTER COLUMN ""UserId"" TYPE integer 
    USING CASE WHEN ""UserId"" ~ '^[0-9]+$' THEN ""UserId""::integer ELSE NULL END;
    
    ALTER TABLE ""AuditLogs"" 
    ALTER COLUMN ""UserId"" DROP NOT NULL;
");
```

4. Bu SQL komutu şunları yapıyor:
   - `UserId` sütununu integer tipine dönüştürüyor
   - Dönüşüm sırasında, sayısal değer içeren string'leri integer'a çeviriyor
   - Sayısal olmayan değerleri NULL olarak ayarlıyor
   - Sütunu NULL değerlere izin verecek şekilde değiştiriyor

5. Migrasyonu uyguladık: `dotnet ef database update -c ApplicationDbContext`

### Öğrenilen Dersler
1. PostgreSQL, farklı veri tipleri arasında otomatik dönüşüm yapmaz. Özellikle string'den sayısal tiplere dönüşüm yaparken dikkatli olunmalıdır.
2. Veri tipi dönüşümleri için özel SQL komutları kullanmak gerekebilir.
3. Entity Framework Core migrasyonlarında, `migrationBuilder.Sql()` metodu ile özel SQL komutları ekleyebiliriz.
4. Veri tipi dönüşümlerinde, dönüştürülemeyen değerler için bir strateji belirlenmeli (NULL atama, varsayılan değer atama vb.).
5. Migrasyon dosyalarını manuel olarak düzenlemek, Entity Framework Core'un otomatik olarak oluşturamadığı veya algılayamadığı değişiklikleri yapmak için kullanışlıdır.

## TypeScript Tip Hataları ve Çözümleri

### Hata Mesajı
```
[ERROR] TS2339: Property 'getRoleById' does not exist on type 'RoleService'.
[ERROR] TS7006: Parameter 'role' implicitly has an 'any' type.
[ERROR] TS7006: Parameter 'error' implicitly has an 'any' type.
```

### Hatanın Nedeni
1. `permission-management.component.ts` dosyasında `roleService.getRoleById` metodu çağrılıyordu, ancak `RoleService` sınıfında böyle bir metot bulunmuyordu. Bunun yerine `getRole` metodu vardı.
2. Bazı parametrelerin tip tanımlamaları eksikti, bu da TypeScript'in katı tip kontrolü nedeniyle hatalara neden oluyordu.
3. Bileşende tanımlanan `Permission` interface'i ile `shared/models/permission.model.ts` dosyasındaki `Permission` interface'i arasında bir uyumsuzluk vardı. Bileşendeki `Permission` interface'inde `isGranted` alanı vardı, ancak modeldeki `Permission` interface'inde bu alan yoktu.

### Çözüm
1. `roleService.getRoleById` metodu çağrılarını `roleService.getRole` olarak değiştirdik:
   ```typescript
   // Eski kod
   this.roleService.getRoleById(this.entityId).subscribe({...});
   
   // Yeni kod
   this.roleService.getRole(this.entityId).subscribe({...});
   ```

2. Tüm parametrelere tip tanımlamaları ekledik:
   ```typescript
   // Eski kod
   next: (role) => {...}
   error: (error) => {...}
   
   // Yeni kod
   next: (role: Role) => {...}
   error: (error: any) => {...}
   ```

3. Model ve bileşen arasındaki tip uyumsuzluğunu çözmek için:
   - Model tipini farklı bir isimle import ettik: `import { Permission as ModelPermission } from '../../../shared/models/permission.model';`
   - API'den gelen verileri bileşendeki tipe dönüştürdük:
   ```typescript
   this.allPermissions = permissions.map(p => ({
     id: p.id,
     name: p.name,
     description: p.description || '',
     group: p.group || '',
     isGranted: false
   }));
   ```

### Öğrenilen Dersler
1. Servis metodlarını çağırmadan önce, servis sınıfında bu metodların var olduğundan emin olunmalıdır.
2. TypeScript'in katı tip kontrolü sayesinde, tip hatalarını erken aşamada tespit edebiliriz. Bu nedenle tüm parametrelere ve değişkenlere uygun tip tanımlamaları eklemeliyiz.
3. Farklı modüllerde aynı isimde ancak farklı yapıda interface'ler olduğunda, bunları farklı isimlerle import ederek çakışmaları önleyebiliriz.
4. API'den gelen verileri bileşende kullanılan tiplere dönüştürürken, eksik alanlar için varsayılan değerler sağlamalıyız.
5. Hata mesajlarını dikkatlice okuyarak, sorunun kaynağını tespit edebiliriz.

### Sonraki Adımlar
1. Tüm bileşenlerde benzer tip hatalarını kontrol etmek ve düzeltmek.
2. Model ve bileşen arasındaki tip uyumsuzluklarını gidermek için ortak bir tip tanımı oluşturmak.
3. Servis metodlarının dokümantasyonunu iyileştirmek ve tutarlı isimlendirme kuralları uygulamak.

## Kullanıcı Yönetimi Sayfası Sorunları

### Sorun 1: Kullanıcılar otomatik olarak yüklenmiyor
**Tarih:** 2023-11-15

**Sorun Açıklaması:**  
Kullanıcı yönetimi sayfasına girildiğinde kullanıcılar otomatik olarak yüklenmiyordu.

**Çözüm:**  
`user-management.component.ts` dosyasında `ngOnInit` metodunda `loadUsers()` çağrısı yorum satırı haline getirilmişti. Bu çağrı aktif hale getirildi. Böylece sayfa yüklendiğinde kullanıcılar otomatik olarak yüklenecek.

```typescript
ngOnInit() {
  console.log('UserManagementComponent initialized');
  
  // Önce rolleri yükle
  this.loadRoles();
  
  // Kullanıcıları yükle - artık loadRoles içinde çağrılıyor olsa da
  // burada da çağıralım, böylece roller yüklenemese bile kullanıcılar yüklenebilir
  this.loadUsers();
  
  // Form başlatma
  this.initForm();
  
  // ...
}
```

### Sorun 2: Oluşturulan roller kullanıcı yönetimi sayfasında görünmüyor
**Tarih:** 2023-11-15

**Sorun Açıklaması:**  
Oluşturulan roller kullanıcı yönetimi sayfasında görüntülenmiyordu. Kullanıcıların rol bilgileri "Bilinmeyen Rol" olarak gösteriliyordu.

**Çözüm:**  
İki sorun tespit edildi ve çözüldü:

1. `loadRoles` metodunda API'den gelen rol verilerinin işlenmesi sırasında sadece `label` ve `value` özellikleri kaydediliyordu. Buna `id` ve `name` özellikleri de eklendi.

```typescript
this.roles = roles.map(role => {
  console.log('İşlenen rol:', role);
  return {
    label: role.name,
    value: role.id,
    id: role.id,
    name: role.name
  };
});
```

2. `getRoleName` metodunda rol ID'si ile eşleşen rolü bulmak için sadece `value` özelliği kontrol ediliyordu. Hem `value` hem de `id` özelliklerini kontrol edecek şekilde güncellendi.

```typescript
// Rol ID'si ile eşleşen rolü bul (hem value hem de id özelliklerini kontrol et)
const role = this.roles.find(r => r.value === roleId || r.id === roleId);

if (role) {
  console.log('Rol bulundu:', role);
  return role.label || role.name;
} else {
  console.warn(`ID: ${roleId} için rol bulunamadı!`);
  return 'Bilinmeyen Rol';
}
```

3. Rol yükleme hatası durumunda daha iyi bir hata yönetimi eklendi ve varsayılan roller oluşturuldu.

**Öğrenilen Dersler:**
- Frontend'de veri yapılarının tutarlı olması önemlidir. Aynı veri farklı yerlerde farklı şekillerde (id/value, name/label) kullanılabilir, bu durumda her iki durumu da desteklemek gerekir.
- Hata durumlarında kullanıcıya bilgi vermek ve varsayılan değerler sağlamak önemlidir.
- Konsola detaylı log bilgileri yazmak hata ayıklamayı kolaylaştırır.

## Rol Yükleme Hatası

**Tarih:** 2023-11-15

**Sorun Açıklaması:**  
Kullanıcı yönetimi sayfasında roller yüklenirken 404 (Not Found) hatası alınıyordu.

```
GET http://localhost:5037/api/roles 404 (Not Found)
```

Hata mesajı:
```
Roller yüklenirken hata oluştu: Error: İstek yapılan kaynak bulunamadı.
```

**Sorunun Nedeni:**  
Frontend'deki `role.service.ts` dosyasında API URL'si yanlış yapılandırılmıştı. Backend'de controller adı `RoleController` olduğu için, API endpoint'i `/api/role` olmalıydı, ancak `/api/roles` olarak tanımlanmıştı.

**Çözüm:**  
`role.service.ts` dosyasındaki API URL'si düzeltildi:

```typescript
// Önceki hali
private apiUrl = `${environment.apiUrl}/api/roles`;

// Düzeltilmiş hali
private apiUrl = `${environment.apiUrl}/api/role`;
```

**Öğrenilen Dersler:**
- Frontend ve backend arasındaki API endpoint'lerinin uyumlu olması önemlidir.
- ASP.NET Core'da controller adı ve route arasındaki ilişkiye dikkat edilmelidir. `[Route("api/[controller]")]` attribute'u kullanıldığında, "Controller" son eki olmadan controller adı kullanılır.
- API çağrıları başarısız olduğunda, ilk kontrol edilmesi gereken şey endpoint'in doğru olup olmadığıdır.

## API Endpoint Uyumluluğu Sorunları

### Sorun 1: Roller Yüklenirken 404 Hatası

**Hata Mesajı:**
```
GET http://localhost:5037/api/roles 404 (Not Found)
```

**Nedeni:**
Frontend'deki `role.service.ts` dosyasında API URL'si yanlış yapılandırılmıştı. Backend'de controller adı `RoleController` olduğu için, API endpoint'i `/api/role` olmalıydı, ancak `/api/roles` olarak tanımlanmıştı.

**Çözüm:**
`role.service.ts` dosyasındaki API URL'si düzeltildi:

```typescript
// Önceki hali
private apiUrl = `${environment.apiUrl}/api/roles`;

// Düzeltilmiş hali
private apiUrl = `${environment.apiUrl}/api/role`;
```

### Sorun 2: İzinler Yüklenirken 404 Hatası

**Hata Mesajı:**
```
GET http://localhost:5037/api/permissions 404 (Not Found)
```

**Nedeni:**
Frontend'deki `permission.service.ts` dosyasında API URL'si yanlış yapılandırılmıştı. Backend'de controller adı `PermissionsController` olduğu için, API endpoint'i `/api/Permissions` olmalıydı, ancak `/api/permissions` olarak tanımlanmıştı. ASP.NET Core'da route'lar büyük/küçük harf duyarlıdır.

**Çözüm:**
`permission.service.ts` dosyasındaki API URL'si düzeltildi:

```typescript
// Önceki hali
private apiUrl = `${environment.apiUrl}/api/permissions`;

// Düzeltilmiş hali
private apiUrl = `${environment.apiUrl}/api/Permissions`;
```

**Öğrenilen Dersler:**
- Frontend ve backend arasındaki API endpoint'lerinin uyumlu olması önemlidir.
- ASP.NET Core'da controller adı ve route arasındaki ilişkiye dikkat edilmelidir.
- Controller adlarının büyük/küçük harf duyarlılığına dikkat edilmelidir.
- API çağrıları başarısız olduğunda, ilk kontrol edilmesi gereken şey endpoint'in doğru olup olmadığıdır.

### Sorun 3: Şifre Sıfırlama Endpoint'i Hatası

**Hata Mesajı:**
```
POST http://localhost:5037/api/auth/request-password-reset 404 (Not Found)
```

**Nedeni:**
Frontend'deki `password.service.ts` dosyasında şifre sıfırlama isteği için API endpoint'i yanlış yapılandırılmıştı. Şifre sıfırlama işlemi `AuthController`'da değil, `FixPasswordController`'da bulunmaktadır.

**Çözüm:**
`password.service.ts` dosyasındaki şifre sıfırlama endpoint'i düzeltildi:

```typescript
// Önceki hali
return this.http.post(`${this.apiUrl}/auth/request-password-reset`, { email }, options)

// Düzeltilmiş hali
return this.http.post(`${this.apiUrl}/FixPassword/request-password-reset`, { email }, options)
```

**Öğrenilen Dersler:**
- Frontend ve backend arasındaki API endpoint'lerinin uyumlu olması önemlidir.
- API çağrıları başarısız olduğunda, ilk kontrol edilmesi gereken şey endpoint'in doğru olup olmadığıdır.
- Şifre yönetimi gibi kritik işlevler için doğru controller ve endpoint'lerin kullanılması gerekir.

## Frontend Bileşenlerinde Gereksiz Console.log İfadeleri

### Sorun
Frontend bileşenlerinde, özellikle `permission-management.component.ts`, `user-management.component.ts` ve `role-management.component.ts` dosyalarında çok sayıda gereksiz `console.log`, `console.error` ve `console.warn` ifadeleri bulunuyordu. Bu ifadeler geliştirme aşamasında faydalı olsa da, üretim ortamında performans sorunlarına yol açabilir ve güvenlik riskleri oluşturabilir.

### Çözüm
Tüm gereksiz konsol log ifadeleri kaldırıldı. Hata durumlarında kullanıcıya bilgi vermek için `MessageService` kullanıldı. Bu sayede:

1. Tarayıcı performansı iyileştirildi
2. Kod okunabilirliği arttı
3. Hassas bilgilerin konsola yazdırılması engellendi
4. Kullanıcıya daha anlamlı hata mesajları gösterildi

### Öğrenilen Dersler
- Üretim ortamında hata ayıklama amaçlı log ifadeleri bulunmamalıdır
- Hata durumlarında kullanıcıya anlamlı geri bildirim sağlanmalıdır
- Hassas bilgilerin konsola yazdırılması güvenlik riski oluşturabilir
- Geliştirme aşamasında eklenen log ifadeleri, üretim öncesi temizlenmelidir

### İlgili Dosyalar
- `frontend/src/app/features/user-management/components/permission-management.component.ts`
- `frontend/src/app/features/user-management/components/user-management.component.ts`
- `frontend/src/app/features/user-management/components/role-management.component.ts`

## Backend Controller İyileştirmeleri

### Sorun
Backend controller'larda çok sayıda gereksiz log ifadesi ve doğrudan kod içinde kullanılan magic string/number'lar bulunuyordu. Bu durum:
- Gereksiz log kayıtları oluşturarak performansı etkiliyordu
- Hassas bilgilerin log'lara yazılmasına neden oluyordu
- Kodun bakımını zorlaştırıyordu
- Hata mesajlarında tutarsızlıklara yol açıyordu

### Çözüm
1. Gereksiz log ifadeleri kaldırıldı:
   - Başarılı işlemlerin standart log kayıtları sadeleştirildi
   - Tekrarlayan log ifadeleri kaldırıldı
   - Hassas bilgileri içeren log ifadeleri düzenlendi

2. Magic string ve magic number'lar sabit değişkenlere dönüştürüldü:
   - Hata mesajları için sabit değişkenler oluşturuldu
   - HTTP durum kodları için StatusCodes sınıfı kullanıldı
   - Rol ve izin isimleri için sabit değişkenler oluşturuldu

3. Log formatı standardize edildi:
   - String interpolation ($"...") yerine yapılandırılmış loglama kullanıldı
   - Log mesajlarında tutarlı bir format sağlandı

### Etkilenen Dosyalar
- src/Stock.API/Controllers/UsersController.cs
- src/Stock.API/Controllers/RoleController.cs
- src/Stock.API/Controllers/PermissionsController.cs

### Faydaları
- Performans iyileştirmesi: Gereksiz log ifadelerinin kaldırılması
- Kod kalitesinin artması: Magic string ve magic number'ların sabit değişkenlere dönüştürülmesi
- Güvenlik iyileştirmesi: Hassas bilgilerin log'lara yazılmaması
- Daha iyi hata ayıklama: Standart bir loglama yaklaşımı
- Disk alanı tasarrufu: Gereksiz log kayıtlarının azaltılması

## Merkezi Loglama Mekanizması ve Global Exception Handler Eklenmesi

### Sorun
Uygulama genelinde tutarlı bir loglama yaklaşımı ve beklenmeyen hataların merkezi bir şekilde yakalanması ve işlenmesi için bir mekanizma bulunmuyordu.

### Çözüm
1. NLog kütüphanesi kullanılarak merkezi bir loglama mekanizması oluşturuldu.
2. ILoggerManager arayüzü ve LoggerManager sınıfı oluşturuldu.
3. NLog yapılandırma dosyası oluşturuldu.
4. Global exception handler middleware eklendi.
5. Program.cs dosyası güncellendi.
6. DependencyInjection.cs dosyası güncellendi.
7. UsersController güncellendi.

### Teknik Detaylar
- NLog kütüphanesi, farklı log hedeflerine (dosya, konsol, veritabanı vb.) log göndermeyi sağlar.
- ILoggerManager arayüzü, uygulamanın loglama ihtiyaçlarını karşılamak için tasarlanmıştır.
- LoggerManager sınıfı, ILoggerManager arayüzünü uygular ve NLog kütüphanesini kullanarak logları yönetir.
- ExceptionMiddleware, beklenmeyen hataları yakalamak ve uygun şekilde işlemek için tasarlanmıştır.
- MiddlewareExtensions, middleware'i uygulamaya eklemek için extension method sağlar.

### Faydaları
1. **Tutarlı Loglama**: Tüm uygulama genelinde tutarlı bir loglama yaklaşımı sağlar.
2. **Merkezi Hata Yönetimi**: Beklenmeyen hataları merkezi bir şekilde yakalar ve işler.
3. **Gelişmiş İzlenebilirlik**: Uygulamanın davranışını ve performansını izlemek için daha fazla bilgi sağlar.
4. **Yapılandırılabilir Log Seviyeleri**: Farklı ortamlar için farklı log seviyeleri yapılandırılabilir.
5. **Log Rotasyonu ve Arşivleme**: Logların otomatik olarak döndürülmesi ve arşivlenmesi sağlanır.
6. **Kullanıcı Dostu Hata Mesajları**: Son kullanıcılara daha anlaşılır hata mesajları sunulur.

### Örnek Kullanım
```csharp
// Controller'da loglama kullanımı
[HttpGet]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> GetAll()
{
    _logger.LogInfo("Tüm kullanıcılar getiriliyor.");
    var query = new GetAllUsersQuery();
    var result = await _mediator.Send(query);
    _logger.LogInfo($"{result.Count} kullanıcı başarıyla getirildi.");
    return Ok(result);
}
```

### Referanslar
- [NLog Resmi Dokümantasyonu](https://github.com/NLog/NLog/wiki)
- [ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/)

## Veritabanı Sorgu Optimizasyonu Sırasında Karşılaşılan Hatalar

### Domain Katmanının Application Katmanına Bağımlılık Hatası

**Hata Mesajı:**
```
C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Domain\Interfaces\IUserRepository.cs(4,13): error CS0234: 'Application' tür veya ad alanı adı 'Stock' ad alanında yok (bir derleme başvurunuz mu eksik?)
C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Domain\Interfaces\IUserRepository.cs(18,26): error CS0246: 'UserSummaryDto' türü veya ad alanı adı bulunamadı (bir using yönergeniz veya derleme başvurunuz mu eksik?)
```

**Nedeni:**
Domain katmanı, Application katmanına referans vermediği için `UserSummaryDto` sınıfına erişilemiyor. Clean Architecture prensiplerine göre, domain katmanı diğer katmanlara bağımlı olmamalıdır.

**Çözüm:**
1. `IUserRepository` arayüzünden `UserSummaryDto` referansını kaldırdık.
2. `GetUserSummariesAsync` metodunu domain katmanına uygun olarak güncelledik, DTO yerine domain entity'lerini kullanacak şekilde değiştirdik.
3. Application katmanında, domain entity'lerini DTO'lara dönüştüren bir mapping mekanizması ekledik.

**Teknik Detaylar:**
```csharp
// Önceki hatalı kod
public interface IUserRepository : IRepository<User>
{
    // ...
    Task<IEnumerable<UserSummaryDto>> GetUserSummariesAsync();
}

// Düzeltilmiş kod
public interface IUserRepository : IRepository<User>
{
    // ...
    Task<IEnumerable<User>> GetUserSummariesAsync();
}
```

**Öğrenilen Dersler:**
1. Clean Architecture prensiplerine göre, iç katmanlar (domain) dış katmanlara (application, infrastructure) bağımlı olmamalıdır.
2. DTO'lar application katmanında tanımlanmalı ve kullanılmalıdır.
3. Domain katmanı, sadece domain entity'leri ve business kurallarını içermelidir.
4. Katmanlar arası veri dönüşümleri için mapping mekanizmaları kullanılmalıdır.

### Güvenlik Açıkları Uyarıları

**Uyarı Mesajları:**
```
C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Stock.Infrastructure.csproj : warning NU1903: 'Npgsql' 7.0.0 paketinde önem derecesi yüksek olan bilinen bir https://github.com/advisories/GHSA-x9vc-6hfv-hg8c güvenlik açığı var
C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Stock.Infrastructure.csproj : warning NU1903: 'System.Text.Json' 7.0.0 paketinde önem derecesi yüksek olan bilinen bir https://github.com/advisories/GHSA-hh2w-p6rv-4g7w güvenlik açığı var
```

**Nedeni:**
Kullanılan NuGet paketlerinde bilinen güvenlik açıkları bulunmaktadır.

**Çözüm:**
İlgili paketlerin güncel sürümlerine yükseltilmesi gerekmektedir.

**Yapılacak İşlemler:**
1. `Npgsql` paketini en son sürüme yükseltmek
2. `System.Text.Json` paketini en son sürüme yükseltmek

**Not:** Bu güvenlik açıkları, yüksek riskli iyileştirmeler aşamasında ele alınacaktır.

## User Sınıfı ve Repository Uyumsuzluğu

### Problem
`UserRepository` sınıfında, `User` domain sınıfında bulunmayan `Email` ve `IsActive` özellikleri kullanılıyordu. Bu durum, derleme hatalarına neden oluyordu.

### Çözüm
`UserRepository` sınıfındaki `GetUserSummariesAsync` metodu güncellenerek, `Email` ve `IsActive` özellikleri yerine `User` sınıfında bulunan `IsAdmin` özelliği kullanıldı. Ayrıca, `UserConfiguration` sınıfından da `Email` özelliğine ait konfigürasyonlar kaldırıldı.

### Teknik Detaylar
- `User` sınıfı incelendi ve mevcut özellikleri belirlendi (`Username`, `PasswordHash`, `IsAdmin`, `LastLoginAt`, `Sicil`, `RoleId`, `Role`, `UserPermissions`).
- `UserRepository` sınıfındaki `GetUserSummariesAsync` metodu güncellenerek, `Email` ve `IsActive` özellikleri yerine `IsAdmin` özelliği kullanıldı.
- `UserConfiguration` sınıfından da `Email` özelliğine ait konfigürasyonlar kaldırıldı.
- `ServiceCollectionExtensions` sınıfında `UnitOfWork` namespace'i düzeltildi.

### Faydalar
- Derleme hataları giderildi.
- Domain sınıfları ile repository sınıfları arasında tutarlılık sağlandı.
- Clean Architecture prensipleri doğrultusunda katmanlar arası bağımlılıklar düzgün şekilde yönetildi.

### Örnek Kullanım
```csharp
// Güncellenmiş GetUserSummariesAsync metodu
public async Task<IEnumerable<User>> GetUserSummariesAsync()
{
    return await _dbSet
        .Include(x => x.Role)
        .Select(u => new User
        {
            Id = u.Id,
            Username = u.Username,
            Sicil = u.Sicil,
            IsAdmin = u.IsAdmin,
            Role = new Role { Name = u.Role.Name }
        })
        .ToListAsync();
}
```

### Referanslar
- [Clean Architecture Principles](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Entity Framework Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

## Program.cs ve CreateAdminUser.cs Dosyalarındaki Sözdizimi Hataları

### Problem

1. `Program.cs` dosyasında `await` kullanımı ile ilgili sözdizimi hataları vardı.
2. `CreateAdminUser` sınıfında hem statik `Initialize` hem de instance `InitializeAsync` metotları bulunuyordu ve bu durum karışıklığa neden oluyordu.
3. `UserRepository.cs` dosyasında `GetAllUsersQuery` sınıfının kullanımında hata vardı.
4. Veritabanı modeli değişmiş ve migration oluşturulmamıştı.

### Çözüm

1. `Program.cs` dosyasında `await` yerine `.Wait()` kullanıldı.
2. `CreateAdminUser` sınıfında statik `Initialize` metodu kaldırıldı ve `InitializeAsync` metodu düzenlendi.
3. `UsersController.cs` dosyasında `GetAllUsersQuery` sınıfının doğru namespace'i kullanıldı ve `Count` yerine `Count()` metodu kullanıldı.
4. Geçici olarak `SeedData.InitializeAsync` içindeki migration kontrolü ve `Program.cs` içindeki `SeedData.InitializeAsync` çağrısı devre dışı bırakıldı.

### Teknik Detaylar

1. `Program.cs` dosyasında:
   - `adminCreator.InitializeAsync(services).Wait();` şeklinde düzeltildi.
   - `SeedData.InitializeAsync(app.Services).Wait();` geçici olarak devre dışı bırakıldı.

2. `CreateAdminUser.cs` dosyasında:
   - Statik `Initialize` metodu kaldırıldı.
   - `InitializeAsync` metodu düzenlendi ve hata yönetimi eklendi.

3. `UserRepository.cs` dosyasında:
   - `GetAllUsersQuery` sınıfının doğru namespace'i kullanıldı.
   - `result.Count` yerine `result.Count()` metodu kullanıldı.

4. `SeedData.cs` dosyasında:
   - `await context.Database.MigrateAsync();` geçici olarak devre dışı bırakıldı.

### Faydalar

1. Uygulama başarıyla derlenebiliyor ve çalışabiliyor.
2. Kod daha tutarlı ve anlaşılır hale geldi.
3. Hata mesajları daha açıklayıcı ve standart hale getirildi.

### Örnek Kullanım

```csharp
// CreateAdminUser.cs
public async Task InitializeAsync(IServiceProvider serviceProvider)
{
    try
    {
        _logger.LogInformation("Admin kullanıcısı oluşturma işlemi başlatılıyor...");
        
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Veritabanında Admin kullanıcısı var mı kontrol et
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");

            // Admin kullanıcısı yoksa oluştur
            if (adminUser == null)
            {
                _logger.LogInformation("Admin kullanıcısı bulunamadı, yeni admin kullanıcısı oluşturuluyor...");
                
                // BCrypt.Net-Next paketini kullanarak şifreyi hashle
                var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", 11);
                
                adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = passwordHash,
                    Sicil = "A001",
                    IsAdmin = true,
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow
                };
                
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
                _logger.LogInformation("Admin kullanıcısı başarıyla oluşturuldu.");
            }
            else
            {
                _logger.LogInformation("Admin kullanıcısı zaten mevcut.");
            }
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Admin kullanıcısı oluşturulurken bir hata oluştu.");
        throw;
    }
}
```

### Referanslar

- [C# Asenkron Programlama](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/)
- [Entity Framework Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

## Veritabanı Sorgu Optimizasyonu Sorunları

### Sorun: Sayfalama Parametrelerinin Doğrulanmaması
**Tarih:** 25 Haziran 2025
**Hata:** Sayfalama parametreleri (sayfa numarası ve sayfa boyutu) negatif veya çok büyük değerler olduğunda performans sorunları yaşanıyordu.
**Nedeni:** Sayfalama parametreleri için doğrulama yapılmıyordu ve bu durum, çok büyük veri kümelerinin sorgulanmasına neden olabiliyordu.
**Çözüm:**
1. Tüm repository sınıflarında sayfalama parametreleri için doğrulama eklendi:

```csharp
public async Task<(IEnumerable<T> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize)
{
    // Sayfalama parametrelerini doğrula
    pageNumber = Math.Max(1, pageNumber);
    pageSize = Math.Clamp(pageSize, 1, 100); // Sayfa boyutunu 1-100 arasında sınırla
    
    var query = _dbSet.AsNoTracking();
    var totalCount = await query.CountAsync();
    
    var items = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    
    return (items, totalCount);
}
```

2. Controller'larda da benzer doğrulamalar eklendi:

```csharp
[HttpGet("paginated")]
public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
{
    // Sayfalama parametrelerini doğrula
    if (pageNumber < 1)
    {
        _logger.LogWarning("Geçersiz sayfa numarası: {PageNumber}. Sayfa numarası 1 olarak ayarlandı.", pageNumber);
        pageNumber = 1;
    }
    
    if (pageSize < 1 || pageSize > 100)
    {
        _logger.LogWarning("Geçersiz sayfa boyutu: {PageSize}. Sayfa boyutu {DefaultPageSize} olarak ayarlandı.", pageSize, 10);
        pageSize = 10;
    }
    
    var result = await _userRepository.GetPaginatedAsync(pageNumber, pageSize);
    return Ok(new { Items = result.Items, TotalCount = result.TotalCount, PageNumber = pageNumber, PageSize = pageSize });
}
```

**Öğrenilen Dersler:**
1. Kullanıcı girdilerini her zaman doğrulamak önemlidir, özellikle de bu girdiler veritabanı sorgularını etkiliyorsa.
2. Sayfalama parametreleri için makul sınırlar belirlemek, performans sorunlarını önlemeye yardımcı olur.
3. Hatalı parametreler için varsayılan değerler kullanmak, uygulamanın daha sağlam olmasını sağlar.
4. Şüpheli parametre değerleri için log kayıtları tutmak, potansiyel güvenlik sorunlarını tespit etmeye yardımcı olabilir.

### Sorun: Gereksiz Veri Yükleme (N+1 Sorunu)
**Tarih:** 26 Haziran 2025
**Hata:** Kullanıcı listesi yüklenirken, her kullanıcı için ayrı bir sorgu ile rol bilgileri çekiliyordu, bu da çok sayıda veritabanı sorgusu yapılmasına neden oluyordu.
**Nedeni:** Entity Framework Core'un lazy loading özelliği nedeniyle, ilişkili veriler ihtiyaç duyulduğunda ayrı sorgularla yükleniyordu.
**Çözüm:**
1. Include kullanarak ilişkili verileri tek sorguda yükleme:

```csharp
public async Task<IEnumerable<User>> GetAllAsync()
{
    return await _dbSet
        .Include(u => u.Role) // Rol bilgilerini tek sorguda yükle
        .AsNoTracking() // Sadece okuma amaçlı olduğu için tracking'i devre dışı bırak
        .ToListAsync();
}
```

2. Projection (Select) kullanarak sadece ihtiyaç duyulan alanları seçme:

```csharp
public async Task<IEnumerable<UserSummaryDto>> GetUserSummariesAsync()
{
    return await _dbSet
        .AsNoTracking()
        .Select(u => new UserSummaryDto
        {
            Id = u.Id,
            Username = u.Username,
            Sicil = u.Sicil,
            RoleName = u.Role.Name // Navigation property üzerinden doğrudan erişim
        })
        .ToListAsync();
}
```

**Öğrenilen Dersler:**
1. Entity Framework Core'da N+1 sorunu, performans sorunlarının yaygın bir nedenidir.
2. Include kullanarak ilişkili verileri tek sorguda yüklemek, sorgu sayısını azaltır.
3. Projection (Select) kullanarak sadece ihtiyaç duyulan alanları seçmek, veri transferini minimize eder.
4. AsNoTracking kullanarak sadece okuma amaçlı sorgularda change tracking'i devre dışı bırakmak, performansı artırır.
5. Veritabanı profilleme araçları kullanarak N+1 sorunlarını tespit etmek önemlidir.

### Sorun: Veritabanı Bağlantısı Yönetimi
**Tarih:** 27 Haziran 2025
**Hata:** Uzun süren işlemlerde veritabanı bağlantısı zaman aşımına uğruyordu.
**Nedeni:** Veritabanı bağlantıları uzun süre açık tutuluyordu ve bağlantı havuzu tükeniyordu.
**Çözüm:**
1. DbContext'in doğru şekilde dispose edilmesini sağlamak için using bloklarının kullanılması:

```csharp
public async Task<IActionResult> LongRunningOperation()
{
    using (var scope = _serviceScopeFactory.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // İşlemler...
        await dbContext.SaveChangesAsync();
    }
    
    return Ok();
}
```

2. Bağlantı zaman aşımı süresinin yapılandırılması:

```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(60); // 60 saniye
            npgsqlOptions.EnableRetryOnFailure(3); // Başarısız olursa 3 kez yeniden dene
        }));
```

**Öğrenilen Dersler:**
1. Veritabanı bağlantılarını mümkün olduğunca kısa tutmak önemlidir.
2. Using bloklarını kullanarak kaynakların düzgün şekilde serbest bırakılmasını sağlamak gerekir.
3. Uzun süren işlemler için bağlantı zaman aşımı süresini uygun şekilde yapılandırmak önemlidir.
4. Bağlantı havuzu yapılandırmasını optimize etmek, uygulamanın ölçeklenebilirliğini artırır.
5. Retry mekanizmaları eklemek, geçici veritabanı hatalarına karşı uygulamanın dayanıklılığını artırır.

### Sorun: Büyük Veri Kümelerinde Bellek Tüketimi
**Tarih:** 28 Haziran 2025
**Hata:** Büyük veri kümeleri sorgulandığında, uygulama yüksek bellek tüketimi nedeniyle yavaşlıyordu.
**Nedeni:** Tüm veriler bir kerede belleğe yükleniyordu, bu da büyük veri kümeleri için bellek sorunlarına neden oluyordu.
**Çözüm:**
1. Streaming kullanarak verileri parça parça işleme:

```csharp
public async Task ExportLargeDataAsync()
{
    await using var stream = new FileStream("export.csv", FileMode.Create);
    await using var writer = new StreamWriter(stream);
    
    // Başlık satırını yaz
    await writer.WriteLineAsync("Id,Username,Sicil,RoleName");
    
    // Verileri sayfalayarak işle
    const int batchSize = 1000;
    int pageNumber = 1;
    bool hasMore = true;
    
    while (hasMore)
    {
        var users = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Role)
            .Skip((pageNumber - 1) * batchSize)
            .Take(batchSize)
            .ToListAsync();
        
        if (users.Count == 0)
        {
            hasMore = false;
            continue;
        }
        
        foreach (var user in users)
        {
            await writer.WriteLineAsync($"{user.Id},{user.Username},{user.Sicil},{user.Role?.Name ?? ""}");
        }
        
        pageNumber++;
    }
}
```

2. AsAsyncEnumerable kullanarak verileri asenkron olarak akışlı işleme:

```csharp
public async Task ProcessLargeDataAsync()
{
    await foreach (var user in _dbContext.Users
        .AsNoTracking()
        .AsAsyncEnumerable())
    {
        // Her kullanıcıyı tek tek işle
        await ProcessUserAsync(user);
    }
}
```

**Öğrenilen Dersler:**
1. Büyük veri kümeleri için tüm verileri bir kerede belleğe yüklemek yerine, verileri parça parça işlemek önemlidir.
2. Streaming ve asenkron akışlar, bellek tüketimini azaltmaya yardımcı olur.
3. Sayfalama, büyük veri kümelerini işlemek için etkili bir yöntemdir.
4. AsNoTracking kullanarak change tracking'i devre dışı bırakmak, bellek tüketimini azaltır.
5. Bellek kullanımını izlemek ve optimize etmek, uygulamanın ölçeklenebilirliğini artırır.

### Sorun: Karmaşık Sorgularda Performans Sorunları
**Tarih:** 29 Haziran 2025
**Hata:** Karmaşık filtreleme ve sıralama içeren sorgularda performans sorunları yaşanıyordu.
**Nedeni:** Karmaşık LINQ sorguları, verimsiz SQL sorgularına dönüştürülebilir ve bu da performans sorunlarına neden olabilir.
**Çözüm:**
1. Sorgu optimizasyonu için EXPLAIN ANALYZE kullanımı:

```csharp
public async Task<IEnumerable<User>> GetFilteredUsersAsync(string searchTerm, string sortBy, bool ascending)
{
    // Sorguyu oluştur
    var query = _dbContext.Users.AsNoTracking();
    
    // Filtreleme
    if (!string.IsNullOrEmpty(searchTerm))
    {
        searchTerm = searchTerm.ToLower();
        query = query.Where(u => 
            u.Username.ToLower().Contains(searchTerm) || 
            u.Sicil.ToLower().Contains(searchTerm));
    }
    
    // Sıralama
    query = sortBy?.ToLower() switch
    {
        "username" => ascending 
            ? query.OrderBy(u => u.Username) 
            : query.OrderByDescending(u => u.Username),
        "sicil" => ascending 
            ? query.OrderBy(u => u.Sicil) 
            : query.OrderByDescending(u => u.Sicil),
        _ => query.OrderBy(u => u.Id) // Varsayılan sıralama
    };
    
    // Sorguyu çalıştır
    return await query.ToListAsync();
}
```

2. İndeksleme stratejileri:

```csharp
// UserConfiguration.cs
public void Configure(EntityTypeBuilder<User> builder)
{
    // ... diğer konfigürasyonlar ...
    
    // Sık filtrelenen ve sıralanan alanlar için indeksler
    builder.HasIndex(u => u.Username);
    builder.HasIndex(u => u.Sicil);
    builder.HasIndex(u => new { u.IsAdmin, u.LastLoginAt }); // Bileşik indeks
}
```

**Öğrenilen Dersler:**
1. Karmaşık sorguları optimize etmek için EXPLAIN ANALYZE kullanmak, performans sorunlarını tespit etmeye yardımcı olur.
2. Sık filtrelenen ve sıralanan alanlar için indeksler oluşturmak, sorgu performansını artırır.
3. Bileşik indeksler, birden fazla alan üzerinde filtreleme yapılan sorgular için faydalıdır.
4. Sorgu planlarını analiz etmek, verimsiz sorguları tespit etmeye ve optimize etmeye yardımcı olur.
5. Veritabanı profilleme araçları kullanarak sorgu performansını izlemek önemlidir.

### Sorun: Önbellek Yönetimi Eksikliği
**Tarih:** 30 Haziran 2025
**Hata:** Sık erişilen veriler her seferinde veritabanından çekiliyordu, bu da gereksiz veritabanı yüküne neden oluyordu.
**Nedeni:** Önbellek mekanizması kullanılmıyordu ve bu durum, aynı verilerin tekrar tekrar sorgulanmasına neden oluyordu.
**Çözüm:**
1. In-memory önbellek kullanımı:

```csharp
public async Task<IEnumerable<Role>> GetAllRolesAsync()
{
    // Önbellekte var mı kontrol et
    if (!_memoryCache.TryGetValue("AllRoles", out List<Role> roles))
    {
        // Önbellekte yoksa veritabanından çek
        roles = await _dbContext.Roles.AsNoTracking().ToListAsync();
        
        // Önbelleğe ekle (10 dakika süreyle)
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
            .SetPriority(CacheItemPriority.Normal);
        
        _memoryCache.Set("AllRoles", roles, cacheOptions);
    }
    
    return roles;
}
```

2. Önbellek invalidasyonu:

```csharp
public async Task<IActionResult> UpdateRole(int id, RoleUpdateDto roleDto)
{
    // ... rol güncelleme işlemleri ...
    
    // Önbelleği invalide et
    _memoryCache.Remove("AllRoles");
    
    return Ok();
}
```

**Öğrenilen Dersler:**
1. Sık erişilen ve nadiren değişen veriler için önbellek kullanmak, veritabanı yükünü azaltır.
2. Önbellek süresi, verilerin değişim sıklığına göre ayarlanmalıdır.
3. Veriler güncellendiğinde önbelleği invalide etmek önemlidir.
4. Dağıtık sistemlerde, dağıtık önbellek çözümleri (Redis, Memcached vb.) kullanmak gerekebilir.
5. Önbellek kullanımını izlemek ve optimize etmek, uygulamanın performansını artırır.

### Sorun: Veritabanı Bağlantı Dizesi Güvenliği
**Tarih:** 1 Temmuz 2025
**Hata:** Veritabanı bağlantı dizesi, şifre dahil olmak üzere açık metin olarak yapılandırma dosyalarında saklanıyordu.
**Nedeni:** Bağlantı dizesi güvenliği için uygun önlemler alınmamıştı.
**Çözüm:**
1. Bağlantı dizesi şifresini ortam değişkenlerinden alma:

```csharp
public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
{
    // Veritabanı şifresini ortam değişkeninden al
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    
    // Bağlantı dizesini oluştur
    var connectionString = configuration.GetConnectionString("DefaultConnection")
        .Replace("{DB_PASSWORD}", dbPassword);
    
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
    
    // ... diğer servis kayıtları ...
    
    return services;
}
```

2. User Secrets kullanımı (Geliştirme ortamı için):

```json
// secrets.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=stockdb;Username=postgres;Password=your_secure_password"
  }
}
```

```csharp
// Program.cs
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
```

**Öğrenilen Dersler:**
1. Hassas bilgileri (şifreler, API anahtarları vb.) açık metin olarak yapılandırma dosyalarında saklamaktan kaçınmak önemlidir.
2. Ortam değişkenleri, hassas bilgileri saklamak için güvenli bir yöntemdir.
3. Geliştirme ortamı için User Secrets kullanmak, hassas bilgileri kaynak kontrolünden uzak tutmaya yardımcı olur.
4. Üretim ortamı için Azure Key Vault, AWS Secrets Manager gibi güvenli depolama çözümleri kullanmak önemlidir.
5. Bağlantı dizesi şifrelerini düzenli olarak değiştirmek, güvenliği artırır.
