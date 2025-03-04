# Login Hatası: RolePermissions Tablosu Eksikliği

## Sorun

Kullanıcı giriş yaparken aşağıdaki hata alınıyor:

```
POST http://localhost:5037/api/auth/login 500 (Internal Server Error)
```

Backend loglarında ise şu hata görülüyor:

```
[21:35:39 ERR] Failed executing DbCommand (4ms) [Parameters=[@__roleId_0='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
SELECT p."Id", p."CreatedAt", p."CreatedBy", p."Description", p."Group", p."IsDeleted", p."Name", p."UpdatedAt", p."UpdatedBy"
FROM "RolePermissions" AS r
INNER JOIN "Permissions" AS p ON r."PermissionId" = p."Id"
WHERE r."RoleId" = @__roleId_0 AND NOT (p."IsDeleted")
[21:35:39 ERR] An exception occurred while iterating over the results of a query for context type 'Stock.Infrastructure.Data.ApplicationDbContext'.
Npgsql.PostgresException (0x80004005): 42P01: relation "RolePermissions" does not exist
```

## Nedeni

Kullanıcı başarılı bir şekilde kimlik doğrulaması yapıyor, ancak login işlemi sırasında kullanıcının rolüne ait izinler sorgulanırken `RolePermissions` tablosu veritabanında bulunamıyor. Bu tablo, Entity Framework Core migrasyonlarında oluşturulmamış.

## Çözüm Adımları

1. `RolePermissions` tablosunu oluşturmak için yeni bir migration ekledik:

```bash
dotnet ef migrations add AddRolePermissionsTable --project ../Stock.Infrastructure
```

2. Veritabanını güncelleyerek tabloyu oluşturduk:

```bash
dotnet ef database update
```

3. Backend API'yi yeniden başlattık:

```bash
dotnet run
```

## Teknik Detaylar

- `RolePermissions` tablosu, kullanıcı rollerine atanan izinleri saklamak için kullanılır
- `PermissionService` sınıfı, kullanıcı giriş yaptığında rolüne ait izinleri sorgulamaya çalışıyor
- Tablo olmadığında, PostgreSQL `42P01: relation "RolePermissions" does not exist` hatası veriyor
- Bu hata, login işleminin başarısız olmasına ve 500 Internal Server Error dönmesine neden oluyor

## Öğrenilen Dersler

1. Entity Framework Core migrasyonları oluştururken, tüm ilişkili tabloların doğru şekilde tanımlandığından emin olunmalı
2. Veritabanı şeması değişiklikleri için her zaman migration kullanılmalı
3. Hata mesajlarını dikkatlice inceleyerek kök nedeni tespit etmek önemli
4. Backend logları, hata ayıklama sürecinde çok değerli bilgiler sağlar

## Önleyici Tedbirler

1. Yeni bir entity veya ilişki eklendiğinde, hemen migration oluşturun
2. Geliştirme ortamında düzenli olarak veritabanı şemasını kontrol edin
3. Entity Framework Core ilişkilerini doğru şekilde tanımlayın
4. Kapsamlı hata yakalama ve loglama mekanizmaları kullanın 