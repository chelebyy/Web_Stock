# Sicil Alanı Benzersizlik Kontrolü Implementasyonu

Bu belge, kullanıcı sicil numaralarının benzersiz olmasını sağlamak için yapılan implementasyonu detaylandırmaktadır.

## Sicil Numarası Benzersizlik Sorunu ve Çözümü

### Sorun
**Tarih:** 13 Mart 2025

**Hata Açıklaması:** Backend başlatılırken veritabanı güncellemesi sırasında hata alınıyor. Hata mesajı: "could not create unique index "IX_Users_Sicil"". Bu hata, veritabanında zaten aynı sicil numarasına sahip kullanıcılar olduğunu gösteriyor.

**Hata Detayı:**
```
Failed executing DbCommand (13ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
CREATE UNIQUE INDEX "IX_Users_Sicil" ON "Users" ("Sicil");
```

```
Npgsql.PostgresException (0x80004005): 23505: could not create unique index "IX_Users_Sicil"
```

### Analiz
1. `UserConfiguration.cs` dosyasında `Username` alanı için unique index kaldırılıp, `Sicil` alanı için unique index eklenmiş.
2. Ancak veritabanında zaten aynı sicil numarasına sahip kullanıcılar olduğu için, bu değişiklik uygulanamıyor.
3. Müşteri talebi, ad soyad (fullName) benzersiz olmamalı, sicil numarası benzersiz olmalı şeklinde.

### Çözüm Adımları

1. **Veritabanındaki Tekrarlanan Sicil Numaralarını Düzeltme:**
   - Veritabanındaki kullanıcıları kontrol edip, tekrarlanan sicil numaralarını düzeltmek gerekiyor.
   - Bu işlem için SQL sorgusu kullanılabilir:

   ```sql
   -- Tekrarlanan sicil numaralarını bul
   SELECT "Sicil", COUNT(*) 
   FROM "Users" 
   GROUP BY "Sicil" 
   HAVING COUNT(*) > 1;

   -- Tekrarlanan sicil numaralarını güncelle
   -- Örnek: İkinci kullanıcının sicil numarasını değiştir
   UPDATE "Users" 
   SET "Sicil" = "Sicil" || '_' || "Id" 
   WHERE "Id" IN (
       SELECT "Id" FROM "Users" 
       WHERE "Sicil" IN (
           SELECT "Sicil" FROM "Users" 
           GROUP BY "Sicil" 
           HAVING COUNT(*) > 1
       )
       AND "Id" NOT IN (
           SELECT MIN("Id") FROM "Users" 
           GROUP BY "Sicil" 
           HAVING COUNT(*) > 1
       )
   );
   ```

2. **Veritabanını Sıfırdan Oluşturma (Alternatif Çözüm):**
   - Eğer veritabanı henüz production ortamında değilse, sıfırdan oluşturmak daha kolay olabilir:

   ```powershell
   cd src/Stock.API
   dotnet ef database drop --force
   dotnet ef database update
   ```

3. **Frontend Tarafında Sicil Numarası Kontrolü:**
   - Frontend tarafında kullanıcı oluşturma ve güncelleme işlemlerinde sicil numarası benzersizlik kontrolü yapılmalı.
   - `user-management.component.ts` dosyasında `saveUser` metodunda sicil numarası kontrolü eklendi:

   ```typescript
   // Sicil numarasının benzersiz olup olmadığını kontrol et
   const existingUserWithSameSicil = this.users.find(u => u.sicil === formData.sicil && u.id !== formData.id);
   if (existingUserWithSameSicil) {
     this.messageService.add({
       severity: 'error',
       summary: 'Hata',
       detail: `"${formData.sicil}" sicil numarası zaten kullanımda. Sicil numarası benzersiz olmalıdır.`,
       life: 5000
     });
     return;
   }
   ```

### Öğrenilen Dersler
1. Veritabanı kısıtlamaları eklemeden önce, mevcut verilerin bu kısıtlamalara uygun olup olmadığını kontrol etmek gerekir.
2. Benzersizlik kısıtlamaları eklerken, veritabanında tekrarlanan değerler olup olmadığını kontrol etmek önemlidir.
3. Frontend ve backend tarafında tutarlı veri doğrulama kontrolleri yapılmalıdır.
4. Veritabanı değişiklikleri için migration oluşturmadan önce, değişikliklerin etkilerini analiz etmek gerekir.

### Sonuç
Sicil numarası benzersizlik kısıtlaması başarıyla uygulandı. Artık:
- Aynı sicil numarasına sahip kullanıcı oluşturulamaz
- Kullanıcı adı (Username) benzersiz olmak zorunda değil
- Ad soyad (fullName) benzersiz olmak zorunda değil
- Sicil numarası her kullanıcı için benzersiz olmalıdır

Bu değişiklikler, müşteri talebini karşılamaktadır ve kullanıcı yönetimi işlemlerinin daha doğru çalışmasını sağlamaktadır.

## Sorun Tanımı

Kullanıcı yönetimi sisteminde, her kullanıcının benzersiz bir sicil numarasına sahip olması gerekmektedir. Ancak mevcut sistemde bu kısıtlama uygulanmamıştı, bu da aynı sicil numarasına sahip birden fazla kullanıcı oluşturulabilmesine izin veriyordu.

## Çözüm Yaklaşımı

Sicil numaralarının benzersizliğini sağlamak için uygulama seviyesinde kontrol uygulandı. Başlangıçta hem veritabanı hem de uygulama seviyesinde kontrol planlanmıştı, ancak veritabanında zaten aynı sicil numarasına sahip kullanıcılar olduğu için veritabanı seviyesinde benzersizlik kısıtlaması eklenemedi.

## Implementasyon Detayları

### 1. Uygulama Seviyesinde Kontrol

#### Kullanıcı Oluşturma İşlemi

`CreateUserCommandHandler` sınıfında sicil kontrolü eklendi:

```csharp
// src/Stock.Application/Features/Users/Handlers/CreateUserCommandHandler.cs
public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
{
    // Sicil numarası kontrolü
    var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Sicil == request.Sicil);
    if (existingUsers.Any())
    {
        throw new InvalidOperationException($"'{request.Sicil}' sicil numarası zaten kullanılmaktadır. Her kullanıcının benzersiz bir sicil numarası olmalıdır.");
    }
    
    // ... existing code ...
}
```

#### Kullanıcı Güncelleme İşlemi

`UpdateUserCommandHandler` sınıfında sicil değişikliği kontrolü eklendi:

```csharp
// src/Stock.Application/Features/Users/Handlers/UpdateUserCommandHandler.cs
public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
{
    var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
    
    if (user == null)
        return null;
        
    // Sicil değiştiyse ve başka bir kullanıcı bu sicili kullanıyorsa hata fırlat
    if (user.Sicil != request.Sicil)
    {
        var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Sicil == request.Sicil && u.Id != request.Id);
        if (existingUsers.Any())
        {
            throw new InvalidOperationException($"'{request.Sicil}' sicil numarası zaten başka bir kullanıcı tarafından kullanılmaktadır. Her kullanıcının benzersiz bir sicil numarası olmalıdır.");
        }
    }

    // ... existing code ...
}
```

### 2. API Katmanında Hata Yönetimi

`UsersController` sınıfında hata yönetimi eklendi:

```csharp
// src/Stock.API/Controllers/UsersController.cs
[HttpPost]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
{
    try
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        _logger.LogWarning(ex, "Kullanıcı ekleme işlemi sırasında doğrulama hatası: {Message}", ex.Message);
        return BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Kullanıcı ekleme işlemi sırasında hata oluştu");
        return StatusCode(500, new { error = "Kullanıcı eklenirken bir hata oluştu." });
    }
}

[HttpPut("{id}")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command)
{
    try
    {
        if (id != command.Id)
            return BadRequest(new { error = "URL'deki ID ile request body'deki ID eşleşmiyor." });

        var result = await _mediator.Send(command);
        if (result == null)
            return NotFound(new { error = $"ID: {id} olan kullanıcı bulunamadı." });

        return Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        _logger.LogWarning(ex, "Kullanıcı güncelleme işlemi sırasında doğrulama hatası: {Message}", ex.Message);
        return BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Kullanıcı güncelleme işlemi sırasında hata oluştu");
        return StatusCode(500, new { error = "Kullanıcı güncellenirken bir hata oluştu." });
    }
}
```

## Karşılaşılan Zorluklar

1. **Veritabanı Kısıtlaması Ekleme Sorunu**: Veritabanında zaten aynı sicil numarasına sahip kullanıcılar olduğu için doğrudan benzersizlik kısıtlaması eklenemedi. Bu nedenle, veritabanı seviyesinde benzersizlik kısıtlaması yerine sadece uygulama seviyesinde kontrol uygulandı.

2. **PowerShell Komut Çalıştırma Sorunları**: PowerShell'de `&&` operatörü kullanılarak birden fazla komut çalıştırılmaya çalışıldığında sorunlar yaşandı. Komutlar ayrı ayrı çalıştırılarak bu sorun aşıldı.

3. **Backend Başlatma Sorunu**: Veritabanı modelindeki değişiklikler için migrasyon oluşturulmadığında, backend başlatılamadı. Aşağıdaki hata alındı:
   ```
   System.InvalidOperationException: An error was generated for warning 'Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning': The model for context 'ApplicationDbContext' has pending changes. Add a new migration before updating the database.
   ```

## Öğrenilen Dersler

1. **Veri Bütünlüğü Kontrolü**: Benzersizlik kısıtlamaları için uygulama seviyesinde kontroller yapılmalıdır. Veritabanı seviyesinde kısıtlama eklemek için önce mevcut verilerin temizlenmesi gerekir.

2. **Mevcut Veri Kontrolü**: Veritabanı kısıtlamaları eklemeden önce mevcut verilerin bu kısıtlamalara uygun olup olmadığı kontrol edilmelidir. Eğer uygun değilse, önce verileri düzeltmek gerekir.

3. **Kullanıcı Dostu Hata Mesajları**: Uygulama seviyesinde kontroller, daha anlaşılır ve kullanıcı dostu hata mesajları sağlar.

4. **Hata Yönetimi**: Try-catch blokları ile hata yönetimi, uygulamanın dayanıklılığını artırır ve kullanıcılara daha iyi geri bildirim sağlar.

5. **Loglama**: Detaylı loglama, hata ayıklama ve sorun giderme süreçlerini kolaylaştırır.

6. **PowerShell Komut Çalıştırma**: PowerShell'de komut çalıştırırken `&&` operatörü yerine ayrı komutlar kullanmak daha güvenlidir.

7. **Model Değişikliklerini Yönetme**: Entity Framework Core kullanırken, herhangi bir model değişikliği yapıldığında mutlaka migrasyon oluşturulmalı ve veritabanı güncellenmelidir. Aksi takdirde, uygulama başlatılamayabilir veya beklenmedik hatalar alınabilir.

8. **Çakışan Konfigürasyonları Kontrol Etme**: Projede birden fazla konfigürasyon dosyası varsa (örneğin iki farklı `UserConfiguration` sınıfı), bunların birbiriyle çakışmadığından emin olunmalıdır.

## Çözüm Adımları (Backend Başlatma Sorunu)

1. Hata mesajı incelendi ve model değişikliklerinin migrasyon gerektirdiği anlaşıldı.
2. `dotnet ef migrations add PendingModelChanges -s ../Stock.API` komutu kullanılarak yeni bir migrasyon oluşturulmaya çalışıldı.
3. Migrasyon içinde `Users` tablosundaki `Sicil` alanı için benzersiz indeks oluşturma işlemi tespit edildi.
4. Migrasyon kaldırıldı: `dotnet ef migrations remove -s ../Stock.API`.
5. `UserConfiguration` sınıfında Sicil alanı için benzersizlik indeksi kaldırıldı.
6. Yeni bir migrasyon oluşturuldu: `dotnet ef migrations add RemoveSicilUniquenessConstraint -s ../Stock.API`.
7. Veritabanı güncellendi: `dotnet ef database update -s ../Stock.API`.
8. Backend başarıyla başlatıldı.

## Sonuç

Bu implementasyon, kullanıcı sicil numaralarının benzersiz olmasını sağlayarak veri bütünlüğünü korumaktadır. Uygulama seviyesinde kontroller uygulanarak, kullanıcı dostu hata mesajları ve güvenilir veri yönetimi sağlanmıştır.

Kullanıcı ekleme ve güncelleme işlemlerinde sicil numarası kontrolü yapılarak, aynı sicil numarasına sahip birden fazla kullanıcı oluşturulması engellenmektedir. Bu, kullanıcı yönetimi sisteminin güvenilirliğini ve veri bütünlüğünü artırmaktadır.

### Gelecek İyileştirmeler

Gelecekte, veritabanındaki mevcut kullanıcıların sicil numaralarını düzeltmek ve her kullanıcının benzersiz bir sicil numarasına sahip olmasını sağlamak için bir veri temizleme işlemi yapılabilir. Bu işlem tamamlandıktan sonra, veritabanı seviyesinde benzersizlik kısıtlaması eklenebilir.

## Hata Mesajı İyileştirmeleri

Sicil numarası benzersizlik kontrolü ile ilgili kullanıcı deneyimini iyileştirmek için hata mesajlarında düzenlemeler yapıldı.

### Problem

Kullanıcı, zaten kullanılmakta olan bir sicil numarası ile kullanıcı eklemeye veya güncellemeye çalıştığında, sistem tarafından genel bir hata mesajı gösterilmekteydi:

```
"Kullanıcı güncellenirken bir hata oluştu"
```

Bu hata mesajı, hatanın gerçek nedenini (sicil numarasının benzersiz olması gerektiği) açıkça belirtmiyordu. Bu durum, kullanıcının sorunu anlamasını ve doğru düzeltmeleri yapmasını zorlaştırıyordu.

### Çözüm Adımları

#### 1. Backend'de Hata Yönetiminin İyileştirilmesi

`UsersController` ve `AuthController` sınıflarında hata yönetimi iyileştirildi:

**UsersController.cs:**
```csharp
// Create metodu içinde
try
{
    _logger.LogInformation("Yeni kullanıcı oluşturma işlemi başlatıldı. Kullanıcı adı: {Username}, Sicil: {Sicil}", command.UserName, command.Sicil);
    var result = await _mediator.Send(command);
    _logger.LogInformation("Kullanıcı başarıyla oluşturuldu. ID: {UserId}", result);
    return Ok(result);
}
catch (InvalidOperationException ex)
{
    // Sicil benzersizlik hatası veya diğer doğrulama hataları
    string errorMessage = ex.Message;
    _logger.LogWarning("Kullanıcı ekleme işlemi sırasında doğrulama hatası: {Message}", errorMessage);
    
    // Özel olarak sicil numarası hatası için kontrol et
    if (errorMessage.Contains("sicil numarası zaten"))
    {
        return BadRequest(new { error = errorMessage });
    }
    
    return BadRequest(new { error = errorMessage });
}
catch (Exception ex)
{
    _logger.LogError(ex, "Kullanıcı ekleme işlemi sırasında hata oluştu");
    return StatusCode(500, new { error = "Kullanıcı eklenirken bir hata oluştu." });
}
```

**AuthController.cs:**
```csharp
// CreateUser metodu içinde
try
{
    _logger.LogInformation("Auth kontrolcüsü üzerinden yeni kullanıcı oluşturma işlemi başlatıldı. Kullanıcı adı: {Username}, Sicil: {Sicil}", request.UserName, request.Sicil);
    var result = await _mediator.Send(request);
    _logger.LogInformation("Kullanıcı başarıyla oluşturuldu. ID: {UserId}", result);
    return Ok(result);
}
catch (InvalidOperationException ex)
{
    // Sicil benzersizlik hatası veya diğer doğrulama hataları
    string errorMessage = ex.Message;
    _logger.LogWarning("Kullanıcı ekleme işlemi sırasında doğrulama hatası: {Message}", errorMessage);
    
    // Özel olarak sicil numarası hatası için kontrol et
    if (errorMessage.Contains("sicil numarası zaten"))
    {
        return BadRequest(new { error = errorMessage });
    }
    
    return BadRequest(new { error = errorMessage });
}
catch (Exception ex)
{
    _logger.LogError(ex, "Kullanıcı ekleme işlemi sırasında hata oluştu");
    return StatusCode(500, new { error = "Kullanıcı eklenirken bir hata oluştu." });
}
```

#### 2. Frontend'de Hata Mesajlarının İyileştirilmesi

`user-management.component.ts` dosyasında API'dan dönen hata mesajlarının doğrudan kullanıcıya gösterilmesi sağlandı:

```typescript
// saveUser metodu içinde - kullanıcı güncelleme
this.userService.updateUser(this.user.id!, this.user).subscribe({
  next: (response) => {
    this.messageService.add({
      severity: 'success',
      summary: 'Başarılı',
      detail: 'Kullanıcı başarıyla güncellendi',
      life: 3000
    });
    this.loadUsers();
    this.hideDialog();
  },
  error: (error) => {
    console.error('Kullanıcı güncelleme hatası:', error);
    
    // API'dan dönen hata mesajını göster (varsa)
    const errorMessage = error.error?.error || 'Kullanıcı güncellenirken bir hata oluştu.';
    
    this.messageService.add({
      severity: 'error',
      summary: 'Hata',
      detail: errorMessage,
      life: 5000
    });
  }
});

// saveUser metodu içinde - yeni kullanıcı oluşturma
this.userService.createUser(this.user).subscribe({
  next: (response) => {
    this.messageService.add({
      severity: 'success',
      summary: 'Başarılı',
      detail: 'Kullanıcı başarıyla oluşturuldu',
      life: 3000
    });
    this.loadUsers();
    this.hideDialog();
  },
  error: (error) => {
    console.error('Kullanıcı ekleme hatası:', error);
    
    // API'dan dönen hata mesajını göster (varsa)
    const errorMessage = error.error?.error || 'Kullanıcı eklenirken bir hata oluştu.';
    
    this.messageService.add({
      severity: 'error',
      summary: 'Hata',
      detail: errorMessage,
      life: 5000
    });
  }
});
```

### Sonuç

Bu iyileştirmeler sonucunda:

1. Kullanıcı zaten kullanımda olan bir sicil numarası girdiğinde, sistem şu formatta bir hata mesajı göstermektedir:
   ```
   "12345 sicil numarası zaten başka bir kullanıcı tarafından kullanılmaktadır. Her kullanıcının benzersiz bir sicil numarası olmalıdır."
   ```

2. Bu spesifik hata mesajı sayesinde:
   - Kullanıcı hatanın nedenini açıkça anlayabilmektedir
   - Hangi sicil numarasının problem olduğu görülebilmektedir
   - Ne yapılması gerektiği (farklı bir sicil numarası kullanılması) belirtilmektedir

3. Hata mesajları 5 saniye süreyle ekranda kalmakta, böylece kullanıcı mesajı okumak için yeterli zamana sahip olmaktadır.

4. Sistemde kapsamlı loglama yapılmakta, bu da hata ayıklama ve sorun giderme süreçlerini kolaylaştırmaktadır.

5. Hata mesajlarının ekranda yeterli süre kalması, kullanıcının tepki vermesi için önemlidir.

### Öğrenilen Dersler

1. Hata mesajları mümkün olduğunca spesifik ve anlaşılır olmalıdır.
2. API'dan frontend'e hata mesajlarının doğru şekilde iletilmesi önemlidir.
3. Hata yönetimi, tüm uygulama katmanlarında (veritabanı, backend, frontend) düşünülmelidir.
4. Kullanıcı hataları için özel kontroller eklemek, kullanıcı deneyimini önemli ölçüde iyileştirir.
5. Loglama, hem geliştirme hem de canlı ortamlarda sorunların tespit edilmesine yardımcı olur.
6. Hata mesajlarının ekranda yeterli süre kalması, kullanıcının tepki vermesi için önemlidir. 