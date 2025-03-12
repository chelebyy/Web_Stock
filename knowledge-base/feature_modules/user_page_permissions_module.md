# Kullanıcı Sayfa İzinleri Modülü

Versiyon: 1.0
Son Güncelleme: 12.03.2025

## İçindekiler
1. [Genel Bakış](#genel-bakış)
2. [Bileşen Yapısı](#bileşen-yapısı)
3. [Veri Modeli](#veri-modeli)
4. [API Entegrasyonu](#api-entegrasyonu)
5. [Kullanıcı Arayüzü](#kullanıcı-arayüzü)
6. [İşlevsellik](#işlevsellik)
7. [Hata Yönetimi](#hata-yönetimi)
8. [Performans Optimizasyonu](#performans-optimizasyonu)
9. [Erişilebilirlik](#erişilebilirlik)
10. [Test Stratejisi](#test-stratejisi)
11. [Gelecek Geliştirmeler](#gelecek-geliştirmeler)

## Genel Bakış

Kullanıcı Sayfa İzinleri modülü, sistem yöneticilerinin kullanıcılara belirli sayfalara erişim izinleri atamalarını sağlayan bir yönetim aracıdır. Bu modül, kullanıcı yönetimi ve sayfa erişim kontrolü arasında bir köprü görevi görür.

**Temel İşlevler:**
- Kullanıcıları listeleme ve filtreleme
- Sayfa izinlerini listeleme ve filtreleme
- Kullanıcılara sayfa izinleri atama/kaldırma
- Kullanıcı ve izin durumlarını görsel olarak gösterme

## Bileşen Yapısı

### Dosya Yapısı
```
frontend/src/app/features/dashboard-management/
├── components/
│   ├── user-page-permissions.component.html
│   ├── user-page-permissions.component.scss
│   └── user-page-permissions.component.ts
├── models/
│   ├── user.model.ts
│   └── page-permission.model.ts
├── services/
│   ├── user.service.ts
│   └── page-permission.service.ts
└── dashboard-management.routes.ts
```

### Bileşen Hiyerarşisi
- **UserPagePermissionsComponent**: Ana bileşen
  - **UserCardComponent**: Kullanıcı kartı (iç bileşen)
  - **PermissionCardComponent**: İzin kartı (iç bileşen)
  - **PaginationComponent**: Sayfalama kontrolleri (paylaşılan bileşen)

## Veri Modeli

### User Model
```typescript
export interface User {
  id: number;
  username: string;
  email: string;
  fullName: string;
  isActive: boolean;
  avatarUrl?: string;
  lastLogin?: Date;
}
```

### PagePermission Model
```typescript
export interface PagePermission {
  id: number;
  name: string;
  description: string;
  path: string;
  isAssigned: boolean;
  category: string;
}
```

## API Entegrasyonu

### Kullanıcı Verileri
- **Endpoint**: `/api/users`
- **Metot**: GET
- **Parametreler**: 
  - `search`: Arama terimi
  - `page`: Sayfa numarası
  - `pageSize`: Sayfa başına öğe sayısı

### Sayfa İzinleri
- **Endpoint**: `/api/users/{userId}/page-permissions`
- **Metot**: GET
- **Parametreler**:
  - `userId`: Kullanıcı ID'si

### İzin Atama
- **Endpoint**: `/api/users/{userId}/page-permissions/{permissionId}`
- **Metot**: POST
- **Gövde**: `{ "isAssigned": true }`

### İzin Kaldırma
- **Endpoint**: `/api/users/{userId}/page-permissions/{permissionId}`
- **Metot**: DELETE

## Kullanıcı Arayüzü

### Tasarım İlkeleri
- **Modern ve Temiz**: Kart tabanlı tasarım, yuvarlak köşeler, hafif gölgeler
- **Responsive**: Tüm ekran boyutlarına uyum sağlayan grid düzeni
- **Tutarlı**: Rol Yönetimi sayfası ile uyumlu tasarım dili
- **Sezgisel**: Kullanıcı dostu etkileşimler ve görsel ipuçları

### Renk Şeması
- **Ana Renk**: #1976d2 (Mavi)
- **Vurgu Rengi**: #3B82F6 (Parlak Mavi)
- **Arka Plan**: #ffffff (Beyaz)
- **Kart Çerçevesi**: #93c5fd (Açık Mavi)
- **Metin**: #333333 (Koyu Gri)

### Tipografi
- **Ana Font**: Roboto
- **Başlık Boyutu**: 1.5rem
- **Alt Başlık Boyutu**: 1.25rem
- **Metin Boyutu**: 1rem

## İşlevsellik

### Kullanıcı Listesi
- Kullanıcıları kart formatında görüntüleme
- İsim, e-posta veya kullanıcı adına göre arama
- Sayfalama ile büyük kullanıcı listelerini yönetme
- Kullanıcı kartına tıklayarak seçim yapma

### İzin Listesi
- Seçilen kullanıcı için mevcut sayfa izinlerini görüntüleme
- İzin adı veya açıklamasına göre arama
- Sayfalama ile büyük izin listelerini yönetme
- İzin atama/kaldırma butonları ile hızlı işlem yapma

### Sayfalama
- Her sayfada 12 kart gösterme
- İlk, önceki, sonraki ve son sayfa butonları
- Toplam öğe ve sayfa bilgisi gösterme

## Hata Yönetimi

### Kullanıcı Geri Bildirimi
- İşlem başarılı olduğunda bildirim gösterme
- Hata durumunda anlaşılır mesajlar sunma
- Yükleme durumlarında ilerleme göstergeleri kullanma

### Hata Senaryoları
- **API Bağlantı Hataları**: Sunucu bağlantısı kesildiğinde yeniden deneme seçeneği
- **Yetkilendirme Hataları**: Oturum süresi dolduğunda yeniden giriş yönlendirmesi
- **Veri Yükleme Hataları**: Alternatif veri kaynakları veya önbellek kullanımı

## Performans Optimizasyonu

### Veri Yükleme Stratejileri
- Tembel yükleme (lazy loading) ile ihtiyaç duyulduğunda veri getirme
- Sayfalama ile büyük veri setlerini yönetme
- Önbelleğe alma ile tekrarlanan API çağrılarını azaltma

### Render Optimizasyonu
- OnPush değişiklik algılama stratejisi kullanma
- Trackby fonksiyonu ile liste render performansını artırma
- Sanal kaydırma (virtual scrolling) ile büyük listeler için performans iyileştirme

## Erişilebilirlik

### WCAG Uyumluluğu
- Renk kontrastı: AA seviyesi uyumluluğu
- Klavye navigasyonu: Tüm işlevlere klavye ile erişim
- Ekran okuyucu desteği: ARIA etiketleri ve anlamlı metin alternatifleri

### Mobil Erişilebilirlik
- Dokunma hedefleri için yeterli boyut (en az 44x44px)
- Responsive tasarım ile tüm ekran boyutlarına uyum
- Dokunma geri bildirimi için görsel ipuçları

## Test Stratejisi

### Birim Testleri
- Bileşen işlevselliği için birim testleri
- Servis metotları için birim testleri
- Yardımcı fonksiyonlar için birim testleri

### Entegrasyon Testleri
- API entegrasyonu için entegrasyon testleri
- Bileşenler arası etkileşim testleri

### Kullanıcı Arayüzü Testleri
- Farklı ekran boyutlarında görünüm testleri
- Kullanıcı etkileşimi senaryoları testleri
- Erişilebilirlik testleri

## Gelecek Geliştirmeler

### Kısa Vadeli
- Toplu izin atama/kaldırma işlevselliği
- Gelişmiş filtreleme seçenekleri
- Kullanıcı aktivite geçmişi görüntüleme

### Orta Vadeli
- Sürükle ve bırak izin atama
- Rol tabanlı izin yönetimi entegrasyonu
- İzin şablonları oluşturma ve uygulama

### Uzun Vadeli
- Yapay zeka destekli izin önerileri
- Kullanıcı davranış analizine dayalı izin optimizasyonu
- Çoklu dil desteği

## Değişiklik Kaydı
- 12.03.2025: İlk versiyon oluşturuldu 