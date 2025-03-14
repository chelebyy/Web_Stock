# API Endpoint Uyumluluğu

## Sorun

Kullanıcı yönetimi sayfasında roller yüklenirken 404 (Not Found) hatası alındı. Frontend'deki `role.service.ts` dosyasında API URL'si yanlış yapılandırılmıştı. Backend'de controller adı `RoleController` olduğu için, API endpoint'i `/api/role` olmalıydı, ancak `/api/roles` olarak tanımlanmıştı.

Benzer şekilde, izinler yüklenirken de 404 (Not Found) hatası alındı. Frontend'deki `permission.service.ts` dosyasında API URL'si yanlış yapılandırılmıştı. Backend'de controller adı `PermissionsController` olduğu için, API endpoint'i `/api/Permissions` olmalıydı, ancak `/api/permissions` olarak tanımlanmıştı.

Ayrıca, şifre sıfırlama işlemi için de 404 (Not Found) hatası alınabilir. Frontend'deki `password.service.ts` dosyasında şifre sıfırlama isteği için API endpoint'i yanlış yapılandırılmıştı. Şifre sıfırlama işlemi `AuthController`'da değil, `FixPasswordController`'da bulunmaktadır.

## Çözüm

`role.service.ts` dosyasındaki API URL'si düzeltildi:

```typescript
// Önceki hali
private apiUrl = `${environment.apiUrl}/api/roles`;

// Düzeltilmiş hali
private apiUrl = `${environment.apiUrl}/api/role`;
```

`permission.service.ts` dosyasındaki API URL'si düzeltildi:

```typescript
// Önceki hali
private apiUrl = `${environment.apiUrl}/api/permissions`;

// Düzeltilmiş hali
private apiUrl = `${environment.apiUrl}/api/Permissions`;
```

`password.service.ts` dosyasındaki şifre sıfırlama endpoint'i düzeltildi:

```typescript
// Önceki hali
return this.http.post(`${this.apiUrl}/auth/request-password-reset`, { email }, options)

// Düzeltilmiş hali
return this.http.post(`${this.apiUrl}/FixPassword/request-password-reset`, { email }, options)
```

## ASP.NET Core'da Route ve Controller İlişkisi

ASP.NET Core'da controller'lar için route tanımlaması yaparken, genellikle `[Route("api/[controller]")]` attribute'u kullanılır. Bu attribute, controller adını route'un bir parçası olarak kullanır. Burada dikkat edilmesi gereken nokta, controller adının "Controller" son eki olmadan kullanılmasıdır.

Örneğin:
- `UserController` için route: `/api/user`
- `RoleController` için route: `/api/role`
- `ProductController` için route: `/api/product`

## Frontend ve Backend Arasındaki Uyumluluk

Frontend ve backend arasındaki API endpoint'lerinin uyumlu olması çok önemlidir. Uyumsuzluk durumunda, API çağrıları başarısız olur ve 404 (Not Found) gibi hatalar alınır.

### Uyumluluk Kontrol Listesi

1. **Controller Adı ve Route Kontrolü**: Backend'deki controller adı ve route tanımlamasını kontrol edin.
2. **HTTP Metodu Kontrolü**: Doğru HTTP metodunun kullanıldığından emin olun (GET, POST, PUT, DELETE).
3. **URL Parametreleri Kontrolü**: URL parametrelerinin doğru formatta gönderildiğinden emin olun.
4. **Request Body Kontrolü**: POST ve PUT isteklerinde gönderilen verilerin doğru formatta olduğundan emin olun.
5. **Authentication Kontrolü**: Gerekli authentication token'larının gönderildiğinden emin olun.

## Hata Ayıklama İpuçları

API çağrıları başarısız olduğunda, aşağıdaki adımları izleyerek sorunu tespit edebilirsiniz:

1. **Tarayıcı Geliştirici Araçları**: Tarayıcının geliştirici araçlarını kullanarak Network sekmesinden API çağrılarını izleyin.
2. **Status Code Kontrolü**: 404 (Not Found), 401 (Unauthorized), 500 (Internal Server Error) gibi status code'ları kontrol edin.
3. **Request URL Kontrolü**: İsteğin doğru URL'ye gönderildiğinden emin olun.
4. **Backend Logları**: Backend tarafındaki logları kontrol ederek, isteğin backend'e ulaşıp ulaşmadığını ve nasıl işlendiğini görün.
5. **Postman veya Curl**: API'yi doğrudan test etmek için Postman veya Curl gibi araçları kullanın.

## Öğrenilen Dersler

- Frontend ve backend arasındaki API endpoint'lerinin uyumlu olması önemlidir.
- ASP.NET Core'da controller adı ve route arasındaki ilişkiye dikkat edilmelidir.
- API çağrıları başarısız olduğunda, ilk kontrol edilmesi gereken şey endpoint'in doğru olup olmadığıdır.
- Düzenli olarak API dokümantasyonu güncellemek ve frontend-backend ekipleri arasında iyi bir iletişim kurmak, bu tür sorunları önlemeye yardımcı olur.

## İyi Uygulamalar

1. **API Dokümantasyonu**: Tüm API endpoint'lerini, parametrelerini ve dönüş değerlerini belgeleyin.
2. **Swagger/OpenAPI**: API'nizi Swagger/OpenAPI ile dokümante edin ve frontend geliştiricilerin bu dokümantasyona erişmesini sağlayın.
3. **Endpoint Naming Conventions**: Tutarlı bir endpoint isimlendirme kuralı belirleyin ve buna uyun.
4. **Versiyonlama**: API'nizi versiyonlayarak, değişikliklerden etkilenmeden eski istemcilerin çalışmaya devam etmesini sağlayın.
5. **Hata Mesajları**: Anlaşılır hata mesajları döndürerek, frontend geliştiricilerin sorunları daha kolay tespit etmesini sağlayın. 