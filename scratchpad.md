# Görev: Rol Yönetimi Modülü Optimizasyonu

Bu çalışma, `kod_iyilestirme_takip_ve_plan.md` belgesinde belirtilen hedefler doğrultusunda Rol Yönetimi (Role Management) modülünün performansını ve bakım kolaylığını artırmayı amaçlamaktadır. Kullanıcı ve Ürün yönetimi modüllerinde uygulanan optimizasyon modeli bu modül için de tekrarlanacaktır.

## Plan

### Faz 1: Backend İyileştirmeleri (API Yanıt Boyutunu Küçültme) - ✔️ Tamamlandı
- [X] `RoleSlimDto.cs` adında yeni bir DTO oluşturuldu. Bu DTO, rollerin liste görünümleri için sadece temel bilgileri (`Id`, `Name`) içeriyor.
- [X] `src/Stock.Application/Common/Mappings/MappingProfile.cs` içine `Role` entity'sinden `RoleSlimDto`'ya dönüşüm için mapping eklendi.
- [X] `src/Stock.Application/Features/Roles/Queries/GetAllRoles/GetAllRolesQueryHandler.cs` güncellenerek `PagedResponse<RoleSlimDto>` döndürmesi sağlandı ve spesifikasyon paterni ile filtreleme/sıralama yetenekleri eklendi.
- [X] `src/Stock.API/Controllers/RolesController.cs` içindeki `GetAllRoles` endpoint'i `RoleSlimDto` kullanacak ve sayfalama parametrelerini alacak şekilde düzenlendi.

### Faz 2: Frontend İyileştirmeleri (State Management ve Lazy Loading) - ✔️ Tamamlandı
- [X] `frontend/src/app/features/role-management/services/role-state.service.ts` adında yeni bir state management servisi oluşturuldu.
- [X] `frontend/src/app/features/role-management/services/role.service.ts` refactor edilerek state yönetimini yeni servise devretti ve sayfalama yeteneği kazandı.
- [X] `frontend/src/app/features/role-management/components/role-management.component.ts` state yönetimi için `RoleStateService`'i kullanacak ve lazy loading'i destekleyecek şekilde yeniden yapılandırıldı. Change Detection stratejisi `OnPush` olarak ayarlandı.
- [X] `frontend/src/app/features/role-management/components/role-management.component.html` şablonu, lazy loading, sinyal tabanlı state ve yeni DTO yapısıyla uyumlu hale getirildi.

### Faz 3: Belgelendirme ve Sonuçlandırma
- [ ] `scratchpad.md` dosyasını görev tamamlandığında temizle.
- [ ] `knowledge-base/feature_modules/role_management_optimization.md` dosyası oluşturularak yapılan tüm optimizasyonlar, teknik kararlar ve adımlar detaylı olarak belgelenecek.
- [ ] `kod_iyilestirme_takip_ve_plan.md` dosyasındaki "Rol Yönetimi" görevi tamamlandı olarak işaretlenecek.
