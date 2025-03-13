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
    .HasFilter("\"IsDeleted\" = false"); // Sadece silinmemiş kullanıcılar için benzersizlik kontrolü
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
- Eğer veritabanında Username alanı için benzersizlik kısıtlaması varsa, aynı ada sahip iki kullanıcı oluşturmaya çalışıldığında "500 Internal Server Error" hatası alınabilir.
- Bu durumda veritabanındaki Username alanı için benzersizlik kısıtlamasının kaldırılması gerekecektir.

### Öğrenilen Dersler
- Kullanıcı deneyimini iyileştirmek için, kullanıcı adları mümkün olduğunca basit ve anlaşılır olmalıdır.
- Frontend ve backend arasındaki kısıtlamaların uyumlu olması gerekir.
- Kullanıcı gereksinimlerinin açık bir şekilde anlaşılması, gereksiz değişikliklerin ve hataların önüne geçer.
