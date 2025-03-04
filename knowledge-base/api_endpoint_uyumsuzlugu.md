# API Endpoint Uyumsuzluğu Çözümü

## Sorun

Frontend uygulamasından backend API'ye yapılan isteklerde 404 Not Found hatası alınıyordu:

```
GET http://localhost:5037/api/users 404 (Not Found)
```

## Nedeni

Frontend'deki UserService içinde API endpoint'leri küçük harfle (`/api/users`) yazılmışken, backend'deki controller büyük harfle (`/api/User`) tanımlanmıştı. ASP.NET Core'da controller adları ve route'lar büyük/küçük harf duyarlıdır.

## Çözüm Adımları

1. Frontend'deki UserService dosyasını inceledik ve API endpoint'lerinin küçük harfle yazıldığını tespit ettik:

```typescript
// Hatalı kod
getUsers(): Observable<User[]> {
  return this.http.get<User[]>(`${this.apiUrl}/users`);
}
```

2. Backend'deki UserController'ı inceledik ve route'un büyük harfle tanımlandığını gördük:

```csharp
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    // Controller metotları...
}
```

3. Frontend'deki UserService dosyasını düzelttik:

```typescript
// Düzeltilmiş kod
getUsers(): Observable<User[]> {
  console.log(`API URL: ${this.apiUrl}/User`);
  return this.http.get<User[]>(`${this.apiUrl}/User`);
}
```

4. Benzer şekilde diğer endpoint'leri de düzelttik:
   - `getUser`: `/api/users/${id}` → `/api/User/${id}`
   - `updateUser`: `/api/users/${id}` → `/api/User/${id}`
   - `deleteUser`: `/api/users/${id}` → `/api/User/${id}`

5. Frontend uygulamasını yeniden başlattık.

## Öğrenilen Dersler

1. ASP.NET Core'da controller adları ve route'lar büyük/küçük harf duyarlıdır.
2. Frontend ve backend arasındaki API endpoint'lerinin tam olarak eşleştiğinden emin olunmalıdır.
3. API isteklerinde 404 hatası alındığında, endpoint adının doğru olup olmadığı kontrol edilmelidir.
4. Tarayıcı konsolundaki hata mesajları, sorunun kaynağını bulmak için önemli ipuçları sağlar.

## Alternatif Çözümler

1. Backend'de route'ları küçük harfle tanımlamak:
```csharp
[Route("api/user")]
public class UserController : ControllerBase
{
    // Controller metotları...
}
```

2. ASP.NET Core'da route'ları büyük/küçük harf duyarsız hale getirmek (Program.cs veya Startup.cs içinde):
```csharp
services.AddRouting(options => options.LowercaseUrls = true);
```

## Önleyici Tedbirler

1. API endpoint'lerini tanımlarken, frontend ve backend arasında tutarlı bir isimlendirme standardı belirleyin.
2. API isteklerini test etmek için Postman veya Swagger gibi araçlar kullanın.
3. API endpoint'lerini bir constants dosyasında tanımlayarak, tüm uygulamada aynı endpoint'lerin kullanılmasını sağlayın.
4. API isteklerinde hata ayıklama için tarayıcı konsolunu ve network sekmesini kullanın. 