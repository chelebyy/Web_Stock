# Modül Bazlı Stil Yapılandırması

## Genel Bakış

Bu belge, projede kullanılan modül bazlı stil yapılandırmasını açıklamaktadır. Bu yaklaşım, her modülün kendi stil tanımlamalarını içermesini ve bu stillerin sadece ilgili modül içinde geçerli olmasını sağlar.

## Neden Modül Bazlı Stil Yapılandırması?

Büyük Angular projelerinde, global stil tanımlamaları çakışmalara ve beklenmeyen davranışlara yol açabilir. Modül bazlı stil yapılandırması, her modülün kendi stillerini içermesini sağlayarak:

1. Stil çakışmalarını önler
2. Modüller arası bağımlılıkları azaltır
3. Bakımı kolaylaştırır
4. Daha tutarlı bir kullanıcı deneyimi sağlar
5. Modüllerin bağımsız olarak geliştirilebilmesini sağlar

## Yapılandırma

Her modül için aşağıdaki yapılandırma kullanılmıştır:

### 1. Modül Bazlı Stil Dosyaları

Her modül için bir ortak stil dosyası oluşturulmuştur:

- `dashboard/dashboard-shared.scss`: Dashboard modülü için ortak stiller
- `user-management/user-management-shared.scss`: User Management modülü için ortak stiller
- `bilgi-islem/bilgi-islem-shared.scss`: Bilgi İşlem modülü için ortak stiller

### 2. Stil İmportları

Her bileşen, kendi modülünün ortak stil dosyasını import eder:

```scss
/* Dashboard bileşeni için */
@import '../dashboard-shared.scss';

/* User Management bileşeni için */
@import '../user-management-shared.scss';

/* Bilgi İşlem bileşeni için */
@import '../bilgi-islem-shared.scss';
```

### 3. Sınıf İsimlendirme Kuralları

Her modül için özel sınıf isimleri kullanılmıştır:

- Dashboard modülü: `.dashboard-*` (örn. `.dashboard-edit-button`)
- User Management modülü: `.user-management-*` (örn. `.user-management-edit-button`)
- Bilgi İşlem modülü: `.bilgi-islem-*` (örn. `.bilgi-islem-edit-button`)

## Örnek: Kalem İkonu Stilleri

Her modülde kalem ikonu için farklı stiller tanımlanmıştır:

### Dashboard Modülü

```scss
.dashboard-edit-button {
  box-shadow: 0 2px 5px rgba(76, 175, 80, 0.3);
  background: linear-gradient(145deg, #4caf50, #43a047);
  /* Diğer stiller */
}
```

### User Management Modülü

```scss
.user-management-edit-button {
  box-shadow: 0 2px 5px rgba(33, 150, 243, 0.3);
  background: linear-gradient(145deg, #2196F3, #1E88E5);
  /* Diğer stiller */
}
```

### Bilgi İşlem Modülü

```scss
.bilgi-islem-edit-button {
  box-shadow: 0 2px 5px rgba(255, 152, 0, 0.3);
  background: linear-gradient(145deg, #FF9800, #F57C00);
  /* Diğer stiller */
}
```

## HTML Kullanımı

Her modülde, ilgili sınıf ismi HTML elementlerine eklenir:

```html
<!-- Dashboard modülünde -->
<button pButton icon="pi pi-pencil" class="dashboard-edit-button"></button>

<!-- User Management modülünde -->
<button pButton icon="pi pi-pencil" class="user-management-edit-button"></button>

<!-- Bilgi İşlem modülünde -->
<button pButton icon="pi pi-pencil" class="bilgi-islem-edit-button"></button>
```

## Animasyonlar

Her modül için özel animasyonlar tanımlanmıştır:

```scss
/* Dashboard modülü için */
@keyframes dashboard-pulse { /* ... */ }

/* User Management modülü için */
@keyframes user-management-pulse { /* ... */ }

/* Bilgi İşlem modülü için */
@keyframes bilgi-islem-pulse { /* ... */ }
```

## Sonuç

Bu modül bazlı stil yapılandırması sayesinde:

1. Her modül kendi görsel kimliğine sahip olabilir
2. Stil çakışmaları önlenir
3. Modüller bağımsız olarak geliştirilebilir
4. Bakım ve güncelleme daha kolay hale gelir
5. Kullanıcı deneyimi daha tutarlı olur

## Dikkat Edilmesi Gerekenler

1. Her modül için benzersiz sınıf isimleri kullanın
2. Global stil tanımlamalarından kaçının
3. Modül bazlı stil dosyalarını düzenli olarak güncelleyin
4. Yeni bir modül eklerken, o modül için bir ortak stil dosyası oluşturun
5. ViewEncapsulation ayarlarına dikkat edin 