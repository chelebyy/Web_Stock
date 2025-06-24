# Bileşen Taşıma Takip Tablosu

Bu belge, eski monolitik yapıdan Clean Architecture yapısına taşınan veya taşınacak bileşenlerin durumunu takip etmek için kullanılır.

## Taşıma Durumu

| Bileşen Adı | Eski Konum | Yeni Konum | Durum | Test | Sorumlu | Notlar |
|-------------|------------|------------|-------|------|---------|--------|
| **Backend Bileşenleri** |
| AuthController | backend/StockAPI/Controllers/AuthController.cs | src/Stock.API/Controllers/AuthController.cs | Beklemede | - | - | JWT işlemleri ve kullanıcı yönetimi |
| UserController | backend/StockAPI/Controllers/UserController.cs | src/Stock.API/Controllers/UserController.cs | Beklemede | - | - | Kullanıcı CRUD işlemleri |
| ApplicationDbContext | backend/StockAPI/Data/ApplicationDbContext.cs | src/Stock.Infrastructure/Data/ApplicationDbContext.cs | Kısmen Taşındı | - | - | Entity yapılandırmaları devam ediyor |
| HashService | backend/StockAPI/Services/HashService.cs | src/Stock.Infrastructure/Services/HashService.cs | Beklemede | - | - | Şifre hashleme işlemleri |
| JwtService | backend/StockAPI/Services/JwtService.cs | src/Stock.Infrastructure/Services/JwtService.cs | Beklemede | - | - | Token oluşturma ve doğrulama |
| User Entity | backend/StockAPI/Models/User.cs | src/Stock.Domain/Entities/User.cs | Tamamlandı | ✓ | - | BaseEntity'den türetildi |
| Role Entity | backend/StockAPI/Models/Role.cs | src/Stock.Domain/Entities/Role.cs | Tamamlandı | ✓ | - | BaseEntity'den türetildi |
| BaseEntity | backend/StockAPI/Models/BaseEntity.cs | src/Stock.Domain/Entities/BaseEntity.cs | Tamamlandı | ✓ | - | Core entity sınıfı |
| IRepository | backend/StockAPI/Interfaces/IRepository.cs | src/Stock.Domain/Interfaces/IRepository.cs | Tamamlandı | ✓ | - | Generic repository interface |
| IUnitOfWork | backend/StockAPI/Interfaces/IUnitOfWork.cs | src/Stock.Domain/Interfaces/IUnitOfWork.cs | Tamamlandı | ✓ | - | UnitOfWork pattern interface |
| UserRepository | backend/StockAPI/Repositories/UserRepository.cs | src/Stock.Infrastructure/Data/Repositories/UserRepository.cs | Beklemede | - | - | User entity için repository |
| RoleRepository | backend/StockAPI/Repositories/RoleRepository.cs | src/Stock.Infrastructure/Data/Repositories/RoleRepository.cs | Beklemede | - | - | Role entity için repository |
| UnitOfWork | backend/StockAPI/Data/UnitOfWork.cs | src/Stock.Infrastructure/Data/UnitOfWork.cs | Taşınıyor | - | - | Transaction yönetimi |
| SeedData | backend/StockAPI/Data/SeedData.cs | src/Stock.Infrastructure/Data/SeedData.cs | Beklemede | - | - | Initial data |
| UserDto | backend/StockAPI/Models/DTOs/UserDto.cs | src/Stock.Application/Models/UserDto.cs | Tamamlandı | ✓ | - | User için DTO |
| RoleDto | backend/StockAPI/Models/DTOs/RoleDto.cs | src/Stock.Application/Models/RoleDto.cs | Tamamlandı | ✓ | - | Role için DTO |

| **Frontend Bileşenleri** |
| AuthService | frontend/src/app/services/auth.service.ts | (Aynı konum) | Taşıma Gerekmiyor | ✓ | - | JWT kimlik doğrulama |
| UserService | frontend/src/app/services/user.service.ts | (Aynı konum) | Taşıma Gerekmiyor | ✓ | - | Kullanıcı işlemleri |
| Login Component | frontend/src/app/components/login/login.component.ts | (Aynı konum) | Taşıma Gerekmiyor | ✓ | - | Giriş sayfası |
| Dashboard Component | frontend/src/app/components/dashboard/dashboard.component.ts | (Aynı konum) | Taşıma Gerekmiyor | ✓ | - | Ana panel |
| User Management Component | frontend/src/app/components/user-management/user-management.component.ts | (Aynı konum) | Taşıma Gerekmiyor | ✓ | - | Kullanıcı yönetimi UI |
| Auth Guard | frontend/src/app/guards/auth.guard.ts | (Aynı konum) | Taşıma Gerekmiyor | ✓ | - | Yetkilendirme kontrolü |
| JWT Interceptor | frontend/src/app/interceptors/jwt.interceptor.ts | (Aynı konum) | Taşıma Gerekmiyor | ✓ | - | Token ekleme |

## Bileşen Taşıma Özeti
- **Toplam Bileşen Sayısı:** 18 (Backend)
- **Tamamlanan:** 6
- **Devam Eden:** 1
- **Bekleyen:** 11
- **Test Edilen:** 6

## Notlar
- Frontend bileşenleri mevcut yapıda kalacak, ancak API endpoint'leri yeni yapıya işaret edecek şekilde güncellenecek
- Öncelikli olarak taşınacak bileşenler: AuthController ve JwtService
- Taşıma sırasında birim testleri yazılacak
- Her bileşen taşındıktan sonra entegrasyon testleri yapılacak

## Son Güncelleme
- Tarih: 2025-02-21
- Güncelleme Detayı: İlk belge oluşturuldu 