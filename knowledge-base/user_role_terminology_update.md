# Rol Terminolojisi Güncelleme

## Sorun Tanımı

Kullanıcı yönetimi arayüzünde, kullanıcı izinleri kısmında "Katkıda Bulunan" terimi kullanılıyordu. Ancak projenin genel terminolojisine göre iki ana rol bulunuyor: "Yönetici" ve "Kullanıcı". Bu nedenle, "Katkıda Bulunan" teriminin "Kullanıcı" olarak güncellenmesi gerekiyordu.

Daha sonra yapılan güncellemede "İzleyici" rolü tamamen kaldırıldı ve sistemde sadece "Yönetici" ve "Kullanıcı" rolleri bırakıldı.

## Yapılan Değişiklikler

### 1. Kullanıcı Yönetimi HTML Dosyası Düzenleme (İlk Güncelleme)

**Dosya**: `frontend/src/app/components/user-management/user-management.component.html`

**Değişiklik**:
```diff
{{ user.permissions === 'Admin' ? 'Yönetici' : 
-  user.permissions === 'Contributor' ? 'Katkıda Bulunan' : 
+  user.permissions === 'Contributor' ? 'Kullanıcı' : 
   user.permissions === 'Viewer' ? 'İzleyici' : user.permissions }}
```

### 2. Kullanıcı Yönetimi TypeScript Dosyası Düzenleme (İlk Güncelleme)

**Dosya**: `frontend/src/app/components/user-management/user-management.component.ts`

**Değişiklik**:
```diff
roleFilterOptions: any[] = [
  { label: 'Tümü', value: null },
  { label: 'Yönetici', value: 'Admin' },
-  { label: 'Katkıda Bulunan', value: 'Contributor' },
+  { label: 'Kullanıcı', value: 'Contributor' },
  { label: 'İzleyici', value: 'Viewer' }
];
```

### 3. İzleyici Rolünün Kaldırılması (İkinci Güncelleme)

**Dosya**: `frontend/src/app/components/user-management/user-management.component.ts`

**Değişiklik**:
```diff
roleFilterOptions: any[] = [
  { label: 'Tümü', value: null },
  { label: 'Yönetici', value: 'Admin' },
-  { label: 'Kullanıcı', value: 'Contributor' },
-  { label: 'İzleyici', value: 'Viewer' }
+  { label: 'Kullanıcı', value: 'Contributor' }
];
```

**Dosya**: `frontend/src/app/components/user-management/user-management.component.html`

**Değişiklik**:
```diff
<span class="permission-badge" [ngClass]="{
  'admin-badge': user.permissions === 'Admin',
-  'contributor-badge': user.permissions === 'Contributor',
-  'viewer-badge': user.permissions === 'Viewer'
+  'contributor-badge': user.permissions === 'Contributor'
}">
  {{ user.permissions === 'Admin' ? 'Yönetici' : 
     user.permissions === 'Contributor' ? 'Kullanıcı' : 
-    user.permissions === 'Viewer' ? 'İzleyici' : user.permissions }}
+    user.permissions }}
</span>
```

## Terminoloji Standardizasyonu

Projedeki rol terminolojisi şu şekilde standardize edilmiştir:

| Teknik Değer | Görünen Değer |
|--------------|---------------|
| Admin        | Yönetici      |
| Contributor  | Kullanıcı     |

Bu standardizasyon, projenin tüm arayüzlerinde tutarlı bir terminoloji kullanılmasını sağlar.

## Öğrenilen Dersler

1. Arayüz bileşenlerinde kullanılan terminolojinin proje genelindeki rol tanımlarıyla tutarlı olması önemlidir.
2. Kullanıcı deneyimi açısından, rol isimlerinin açık ve anlaşılır olması gerekir.
3. Teknik değerler (örn. "Contributor") ile kullanıcıya gösterilen değerler (örn. "Kullanıcı") arasında bir eşleştirme tablosu tutmak faydalıdır.
4. Kullanılmayan roller veya özellikler, kodun bakımını kolaylaştırmak ve kullanıcı deneyimini basitleştirmek için kaldırılmalıdır.

## İlgili Dosyalar

- frontend/src/app/components/user-management/user-management.component.html
- frontend/src/app/components/user-management/user-management.component.ts
- frontend/src/app/models/role.model.ts
- frontend/src/app/models/user.model.ts

## Sonraki Adımlar

1. Diğer bileşenlerdeki rol terminolojisinin de kontrol edilmesi ve gerekirse güncellenmesi.
2. Rol tabanlı erişim kontrol sistemi (RBAC) için kapsamlı bir dokümantasyon oluşturulması.
3. Rol ve izin yönetimi için daha gelişmiş bir arayüz tasarlanması. 