# Kullanıcı İzinleri Kaydetme Hatası Çözümü

## Sorun

Kullanıcı izinleri sayfasında, izinleri kaydetmeye çalışırken 404 Not Found hatası alınıyordu. Frontend uygulaması `http://localhost:5037/api/permissions/user/20/assign` endpoint'ine POST isteği gönderiyor, ancak bu endpoint backend'de tanımlanmamıştı.

## Çözüm

PermissionsController'a yeni bir endpoint ekledik:

```csharp
[HttpPost("user/{userId}/assign")]
[RequirePermission("Users.Permissions.Assign")]
public async Task<ActionResult<bool>> AssignPermissionsToUser(int userId, [FromBody] AssignPermissionsRequest request)
{
    try
    {
        _logger.LogInformation($"Kullanıcıya izinler atanıyor - Kullanıcı ID: {userId}, İzin sayısı: {request.PermissionIds.Count}");
        
        bool result = true;
        foreach (var permissionId in request.PermissionIds)
        {
            var assignResult = await _permissionService.AssignPermissionToUserAsync(userId, permissionId, true);
            if (!assignResult)
            {
                result = false;
                _logger.LogWarning($"İzin atanamadı - Kullanıcı ID: {userId}, İzin ID: {permissionId}");
            }
        }
        
        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Kullanıcıya izinler atanırken hata oluştu - Kullanıcı ID: {userId}");
        return StatusCode(500, new { error = "Kullanıcıya izinler atanırken bir hata oluştu: " + ex.Message });
    }
}
```

Bu endpoint, frontend'den gelen izin ID'lerini alarak her biri için `_permissionService.AssignPermissionToUserAsync` metodunu çağırıyor.

## Öğrenilen Dersler

1. Frontend ve backend arasındaki API endpoint'lerinin uyumlu olması gerekir.
2. Frontend'in kullandığı tüm endpoint'lerin backend'de tanımlanmış olduğundan emin olunmalı.
3. API endpoint'lerini tasarlarken, tek bir kaynak için tekil işlemler yanında toplu işlemler için de endpoint'ler düşünülmeli.
4. Hata durumlarında detaylı loglama yapılmalı ve anlamlı hata mesajları döndürülmeli. 