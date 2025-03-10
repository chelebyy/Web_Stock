# Hata Kayıtları

## Angular 19 Geçişi Sonrası Tasarım Sorunları

### Hata
Angular 19 ve PrimeNG 19'a geçiş sonrasında admin dashboard'daki kartlarda siyah arka plan rengi sorunu oluştu. Eski tasarımda kartlar beyaz arka plana sahipken, geçiş sonrası siyah arka plana dönüştü.

### Nedeni
1. PrimeNG 19'da tema yapılandırması değişti ve CSS değişken isimleri `--p-` öneki ile kullanılmaya başlandı.
2. Angular 19'un yeni kontrol akış sözdizimi (`@if`, `@for` vb.) DOM yapısını değiştirdi.
3. PrimeNG 19'da kart bileşenlerinin varsayılan arka plan renkleri değişti.

### Çözüm
1. `app.config.ts` dosyasında PrimeNG tema yapılandırması güncellendi:
```typescript
providePrimeNG({
  theme: {
    preset: Aura
  }
})
```

2. `styles.scss` dosyasında CSS değişkenleri PrimeNG 19'a uygun hale getirildi:
```scss
:root {
  font-size: 14px;
  --p-font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
}
```

3. Bileşen stillerinde arka plan renkleri açık renklerle güncellendi:
```scss
:host ::ng-deep .p-card .p-card-content {
  padding: 1.25rem;
  background-color: #ffffff;
}

.card-content {
  flex: 1;
  padding: 1.5rem;
  background-color: white;
}
```

4. Şifre değiştirme formu gibi koyu renkli alanlar açık renklerle güncellendi:
```scss
.password-change-form {
  background: rgba(255, 255, 255, 0.95);
  /* Diğer stiller */
}
```

### Öğrenilen Dersler
1. Angular sürüm geçişlerinde tema ve stil değişikliklerini kontrol etmek önemlidir.
2. PrimeNG gibi UI kütüphanelerinin sürüm geçişlerinde CSS değişken isimlerindeki değişikliklere dikkat edilmelidir.
3. Yeni kontrol akış sözdizimi kullanıldığında, DOM yapısındaki değişiklikler nedeniyle CSS seçicilerin çalışma şekli değişebilir.
4. Tema değişikliklerini global olarak `app.config.ts` dosyasında yapılandırmak ve bileşen düzeyinde özelleştirmek gerekebilir.

## Angular 19 Geçişi Sonrası Şifre Değiştirme Formu Tasarım Sorunları

### Sorun
Angular 19 ve PrimeNG 19'a geçiş sonrasında şifre değiştirme formunun tasarımında sorunlar oluştu. İlk olarak form koyu renkli arka plana sahipti, sonra açık renkli temaya uygun olarak beyaz arka plana çevrildi. Ancak beyaz arka plan üzerinde bazı yazılar ve butonlar (örneğin "İptal" butonu ve "Şifre Değiştir" başlığı) yeterince okunabilir değildi.

### Çözüm
Şifre değiştirme formunun CSS stillerini koyu renkli modern bir tema ile yeniden düzenledik:

```scss
.password-change-form {
  background: #1e293b;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
  border: 1px solid rgba(255, 255, 255, 0.1);
  /* Diğer stiller */
}

.password-form-header h2 {
  color: #ffffff;
  font-weight: 600;
}

.modern-input {
  background-color: rgba(255, 255, 255, 0.1) !important;
  border: 1px solid rgba(255, 255, 255, 0.2) !important;
  color: #ffffff !important;
  /* Diğer stiller */
}

.change-button {
  background-color: rgba(34, 197, 94, 0.15) !important;
  border: 1px solid rgba(34, 197, 94, 0.6) !important;
  color: rgba(34, 197, 94, 1) !important;
  /* Diğer stiller */
}

.change-button:hover {
  background-color: rgba(34, 197, 94, 0.25) !important;
  border-color: rgba(34, 197, 94, 0.8) !important;
  color: rgba(255, 255, 255, 1) !important;
}

.cancel-button {
  border-color: rgba(255, 99, 99, 0.5) !important;
  color: rgba(255, 99, 99, 0.9) !important;
  background-color: rgba(255, 99, 99, 0.05) !important;
  /* Diğer stiller */
}

.cancel-button:hover {
  background-color: rgba(255, 99, 99, 0.15) !important;
  border-color: rgba(255, 99, 99, 0.7) !important;
  color: rgba(255, 255, 255, 1) !important;
}
```

### Öğrenilen Dersler
1. Koyu renkli arka planlar üzerinde açık renkli yazılar daha iyi okunabilirlik sağlar.
2. Form tasarımlarında kontrast oranlarına dikkat edilmelidir.
3. Butonlar ve diğer etkileşimli öğeler, kullanıcıya görünür ve erişilebilir olmalıdır.
4. Tasarım değişikliklerinde, tüm bileşenlerin birbiriyle uyumlu olduğundan emin olunmalıdır.
5. Farklı işlevlere sahip butonları (onay/iptal) farklı renklerle ayırt etmek kullanıcı deneyimini iyileştirir.
6. Butonların tutarlı bir görsel stile sahip olması, kullanıcı arayüzünün profesyonel görünmesini sağlar.
7. Yeşil renk genellikle onay ve olumlu eylemleri, kırmızı renk ise iptal ve olumsuz eylemleri temsil eder, bu renk kodlaması kullanıcıların arayüzü daha sezgisel bir şekilde kullanmalarını sağlar.

## Angular 19 Geçişi Sonrası Hata Mesajları Okunabilirlik Sorunu

### Sorun
Angular 19 ve PrimeNG 19'a geçiş sonrasında hata mesajlarının (Toast bileşeni) arka plan rengi çok açık olduğu için içindeki yazılar yeterince okunabilir değildi. Özellikle "Şifre en az 6 karakter olmalıdır!" gibi hata mesajları pembe/açık kırmızı arka plan üzerinde yeterince kontrast oluşturmuyordu.

### Çözüm
Global stil dosyasında (styles.scss) Toast bileşenlerinin stillerini daha koyu ve okunabilir hale getirdik:

```scss
/* Toast Mesajları Stilleri */
.p-toast {
  .p-toast-message {
    border-radius: 8px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    
    &.p-toast-message-error {
      background: #b91c1c !important; /* Daha koyu kırmızı */
      border: 1px solid rgba(220, 38, 38, 0.3) !important;
      color: #ffffff !important;
      
      .p-toast-message-content {
        .p-toast-message-text {
          .p-toast-summary {
            font-weight: 600 !important;
          }
          .p-toast-detail {
            font-weight: 400 !important;
            opacity: 0.95 !important;
          }
        }
      }
    }
    
    &.p-toast-message-success {
      background: #15803d !important; /* Daha koyu yeşil */
      border: 1px solid rgba(34, 197, 94, 0.3) !important;
      color: #ffffff !important;
    }
    
    &.p-toast-message-info {
      background: #0369a1 !important; /* Daha koyu mavi */
      border: 1px solid rgba(14, 165, 233, 0.3) !important;
      color: #ffffff !important;
    }
    
    &.p-toast-message-warn {
      background: #b45309 !important; /* Daha koyu turuncu */
      border: 1px solid rgba(234, 88, 12, 0.3) !important;
      color: #ffffff !important;
    }
  }
}
```

### Öğrenilen Dersler
1. Hata mesajları gibi kritik bildirimler için yeterli kontrast oranı sağlanmalıdır.
2. Yazı ve arka plan rengi arasında en az 4.5:1 kontrast oranı olmalıdır (WCAG 2.0 AA standardı).
3. Bildirim türlerine göre renk kodlaması yaparken (hata: kırmızı, başarı: yeşil, vb.) renklerin yeterince koyu olduğundan emin olunmalıdır.
4. Global stil dosyasında yapılan değişiklikler, tüm uygulamadaki bileşenleri etkileyeceği için dikkatli olunmalıdır.
5. PrimeNG sürüm güncellemelerinde, bileşenlerin varsayılan stillerinin değişebileceği göz önünde bulundurulmalıdır.

## Angular 19 Geçişi Sonrası Checkbox Tasarım Sorunları

### Sorun
Angular 19 ve PrimeNG 19'a geçiş sonrasında Kullanıcı Yönetimi sayfasındaki checkbox'lar siyah arka plana sahip oldu. Bu durum, sayfa tasarımını bozdu ve kullanıcı deneyimini olumsuz etkiledi.

### Nedeni
1. PrimeNG 19'da tema yapılandırması değişti ve CSS değişken isimleri `--p-` öneki ile kullanılmaya başlandı.
2. Angular 19'un yeni kontrol akış sözdizimi DOM yapısını değiştirdi.
3. PrimeNG 19'da checkbox bileşeninin varsayılan arka plan rengi değişti.

### Çözüm
Kullanıcı Yönetimi bileşeninin SCSS dosyasında checkbox stillerini düzenledik:

```scss
// Checkbox düzeltmeleri
.p-checkbox {
  .p-checkbox-box {
    background-color: var(--surface-card, #ffffff) !important;
    border: 1px solid var(--surface-border, #ced4da) !important;
    border-radius: 4px !important;
    
    &:hover {
      border-color: var(--primary-color, #3B82F6) !important;
    }
    
    &.p-highlight {
      background-color: var(--primary-color, #3B82F6) !important;
      border-color: var(--primary-color, #3B82F6) !important;
      
      .p-checkbox-icon {
        color: var(--primary-color-text, #ffffff) !important;
      }
    }
    
    .p-checkbox-icon {
      color: var(--primary-color-text, #ffffff) !important;
      font-size: 14px !important;
    }
  }
}
```

### Öğrenilen Dersler
1. Angular ve PrimeNG sürüm geçişlerinde bileşenlerin varsayılan stillerinin değişebileceğini göz önünde bulundurmak gerekir.
2. CSS değişkenlerini (CSS variables) kullanarak renkleri tanımlamak, tema değişikliklerinde daha tutarlı sonuçlar elde etmeyi sağlar.
3. `!important` kullanarak stil önceliğini artırmak, PrimeNG gibi UI kütüphanelerinin kendi stillerini geçersiz kılmak için gerekli olabilir.
4. Bileşenlerin görsel tutarlılığını sağlamak için, tüm bileşenlerin aynı tema ve renk şemasını kullanmasına dikkat edilmelidir.
5. Sürüm geçişlerinde, özellikle UI bileşenlerinin görünümünü kontrol etmek ve gerektiğinde düzeltmeler yapmak önemlidir.

## Angular 19 Geçişi Sonrası Dropdown Menü Tasarım Sorunları (Güncelleme)

### Sorun
Angular 19 ve PrimeNG 19'a geçiş sonrasında Kullanıcı Yönetimi sayfasındaki dropdown menüsü (rol filtreleme) siyah arka plana sahip oldu. Bileşen SCSS dosyasında yapılan düzeltmeler yeterli olmadı ve dropdown menüsü hala siyah arka plana sahipti.

### Nedeni
1. PrimeNG 19'da dropdown panel, DOM'a doğrudan eklenmek yerine, bir overlay olarak body'ye ekleniyor.
2. Bileşen SCSS dosyasındaki `::ng-deep` içindeki stil tanımları, overlay'e uygulanmıyor.
3. PrimeNG 19'da CSS değişken isimleri `--p-` öneki ile kullanılmaya başlandı.

### Çözüm
Global stil dosyasına (styles.scss) dropdown panel stillerini ekledik:

```scss
/* Dropdown Panel Düzeltmeleri */
body .p-dropdown-panel {
  background: var(--p-surface-overlay, #ffffff) !important;
  border: 1px solid var(--p-surface-border, #ced4da) !important;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1) !important;
}

body .p-dropdown-panel .p-dropdown-items {
  background: var(--p-surface-overlay, #ffffff) !important;
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-item {
  color: var(--p-text-color, #495057) !important;
  background: var(--p-surface-overlay, #ffffff) !important;
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover {
  background: var(--p-surface-hover, #f8f9fa) !important;
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight {
  background: rgba(59, 130, 246, 0.1) !important;
  color: var(--p-primary-color, #3B82F6) !important;
  font-weight: 500 !important;
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-empty-message {
  color: var(--p-text-color-secondary, #6c757d) !important;
}
```

### Öğrenilen Dersler
1. PrimeNG 19'da, dropdown panel gibi bazı bileşenler overlay olarak body'ye ekleniyor. Bu nedenle, bileşen SCSS dosyasındaki stil tanımları yeterli olmayabilir.
2. Overlay bileşenleri için stil tanımlarını global stil dosyasına eklemek daha etkili olabilir.
3. Stil tanımlarının spesifikliğini artırmak için `body` seçicisini kullanmak ve `!important` eklemek gerekebilir.
4. PrimeNG 19'da CSS değişken isimlerinin `--p-` öneki ile kullanıldığını göz önünde bulundurmak gerekir.
5. Dinamik olarak oluşturulan bileşenlerin stillerini kontrol etmek için, bileşenin DOM'da nerede oluşturulduğunu anlamak önemlidir.

## Angular 19 Geçişi Sonrası Dropdown Menü Yazı Görünürlüğü Sorunu

### Sorun
Angular 19 ve PrimeNG 19'a geçiş sonrasında Kullanıcı Yönetimi sayfasındaki dropdown menüsünün (rol filtreleme) arka plan rengi beyaz olarak düzeltildi, ancak bu sefer içindeki yazılar ("Tümü", "Admin", "User") çok soluk veya görünmez hale geldi.

### Nedeni
1. Dropdown öğelerinin yazı rengi yeterince koyu değildi veya opaklığı düşüktü.
2. PrimeNG 19'da CSS değişken değerleri değişmiş olabilir.
3. Dropdown öğelerinin padding ve diğer stil özellikleri yeterince belirgin değildi.

### Çözüm
Global stil dosyasında (styles.scss) dropdown öğelerinin stillerini daha belirgin hale getirdik:

```scss
/* Dropdown Panel Düzeltmeleri */
body .p-dropdown-panel {
  background: var(--p-surface-overlay, #ffffff) !important;
  border: 1px solid rgba(0, 0, 0, 0.2) !important; /* Daha belirgin kenarlık */
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15) !important; /* Daha belirgin gölge */
  border-radius: 6px !important; /* Köşeleri yuvarlatma */
  overflow: hidden !important; /* Taşan içeriği gizleme */
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-item {
  color: #333333 !important; /* Daha koyu renk */
  background: var(--p-surface-overlay, #ffffff) !important;
  font-weight: 500 !important; /* Daha kalın yazı */
  font-size: 1rem !important; /* Daha büyük yazı */
  opacity: 1 !important; /* Tam opaklık */
  padding: 0.75rem 1rem !important; /* Daha fazla padding */
  border-bottom: 1px solid rgba(0, 0, 0, 0.05) !important; /* Öğeler arasında ince çizgi */
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-item * {
  color: inherit !important; /* Üst elementin rengini miras al */
  font-weight: inherit !important; /* Üst elementin yazı ağırlığını miras al */
  opacity: 1 !important; /* Tam opaklık */
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover {
  background: var(--p-surface-hover, #f8f9fa) !important;
  color: #000000 !important; /* Hover durumunda daha koyu renk */
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight {
  background: rgba(59, 130, 246, 0.1) !important;
  color: #2563eb !important; /* Daha koyu mavi */
  font-weight: 600 !important; /* Daha kalın yazı */
}

body .p-dropdown-panel .p-dropdown-items .p-dropdown-empty-message {
  color: #666666 !important; /* Orta tonda gri */
  font-style: italic !important; /* İtalik yazı */
  padding: 0.75rem 1rem !important; /* Padding */
  text-align: center !important; /* Ortalanmış metin */
}
```

### Öğrenilen Dersler
1. Beyaz arka plan üzerinde yazıların görünürlüğünü sağlamak için yeterince koyu renk kullanılmalıdır.
2. CSS değişkenleri yerine doğrudan renk değerleri kullanmak, tarayıcı ve tema farklılıklarından kaynaklanan sorunları önleyebilir.
3. Yazı ağırlığı (font-weight) ve boyutu (font-size) gibi özellikler, yazıların görünürlüğünü artırmada önemli rol oynar.
4. Dropdown öğeleri arasına ince çizgiler eklemek, öğelerin birbirinden ayrılmasını ve daha iyi görünmesini sağlar.
5. Dropdown panelinin kendisi için de belirgin kenarlık ve gölge eklemek, kullanıcı deneyimini iyileştirir.
6. İç içe geçmiş elementlerin stillerini kontrol etmek için, üst elementin stillerini alt elementlere miras bırakmak (`color: inherit` gibi) etkili bir yöntemdir.

## Angular 19 Geçişi Sonrası Dropdown Menü Koyu Tema Sorunu

### Sorun
Angular 19 ve PrimeNG 19'a geçiş sonrasında Kullanıcı Yönetimi sayfasındaki dropdown menüsü (rol filtreleme) koyu yeşil/siyah bir arka plana sahip oldu ve içindeki yazılar ("Tümü", "Admin", "User") beyaz renkte görünüyordu. Daha önce yapılan düzeltmeler yeterli olmadı.

### Nedeni
1. PrimeNG 19'da tema yapılandırması değişti ve bazı bileşenler koyu tema kullanıyor olabilir.
2. CSS seçicilerinin spesifikliği yeterli değildi ve başka stil tanımları bizim stillerimizi geçersiz kılıyordu.
3. Tema değişkenleri (`--p-surface-overlay` gibi) koyu tema değerlerine sahip olabilir.
4. Dropdown panel, DOM'a overlay olarak eklendiğinde farklı stil kurallarına tabi olabilir.

### Çözüm
Global stil dosyasında (styles.scss) dropdown öğelerinin stillerini daha spesifik ve güçlü CSS seçicileri kullanarak düzenledik:

```scss
/* Dropdown Panel Düzeltmeleri - Koyu Tema Geçersiz Kılma */
html body .p-dropdown-panel,
html body .p-dropdown-panel.p-component,
html body .p-dropdown-panel.p-component.p-dropdown-panel-overlay,
html body div.p-dropdown-panel,
html body div.p-dropdown-panel.p-component,
html body div.p-dropdown-panel.p-component.p-dropdown-panel-overlay {
  background-color: #ffffff !important;
  background: #ffffff !important;
  border: 1px solid rgba(0, 0, 0, 0.2) !important;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.15) !important;
  border-radius: 6px !important;
  overflow: hidden !important;
  color: #333333 !important;
}

html body .p-dropdown-panel .p-dropdown-items,
html body div.p-dropdown-panel .p-dropdown-items {
  background-color: #ffffff !important;
  background: #ffffff !important;
}

html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item,
html body div.p-dropdown-panel .p-dropdown-items .p-dropdown-item {
  color: #333333 !important;
  background-color: #ffffff !important;
  background: #ffffff !important;
  font-weight: 500 !important;
  font-size: 1rem !important;
  opacity: 1 !important;
  padding: 0.75rem 1rem !important;
  border-bottom: 1px solid rgba(0, 0, 0, 0.05) !important;
}

html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item *,
html body div.p-dropdown-panel .p-dropdown-items .p-dropdown-item * {
  color: #333333 !important;
  font-weight: inherit !important;
  opacity: 1 !important;
}

html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover,
html body div.p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover {
  background-color: #f8f9fa !important;
  background: #f8f9fa !important;
  color: #000000 !important;
}

html body .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight,
html body div.p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight {
  background-color: rgba(59, 130, 246, 0.1) !important;
  background: rgba(59, 130, 246, 0.1) !important;
  color: #2563eb !important;
  font-weight: 600 !important;
}
```

### Öğrenilen Dersler
1. CSS seçicilerinin spesifikliğini artırmak, başka stil tanımlarını geçersiz kılmak için önemlidir.
2. Tema değişkenleri yerine doğrudan renk değerleri kullanmak, tema farklılıklarından kaynaklanan sorunları önleyebilir.
3. `background-color` ve `background` gibi özellikleri ayrı ayrı tanımlamak, inline stilleri geçersiz kılmak için faydalı olabilir.
4. `html body` gibi üst seçicileri eklemek, CSS seçicilerinin spesifikliğini artırır.
5. Koyu tema kullanılan uygulamalarda, açık tema için özel stil tanımları eklemek gerekebilir.
6. PrimeNG 19'da, overlay bileşenleri için stil tanımlarını daha spesifik yapmak önemlidir.

## PrimeNG 19 ve Angular Güncellemesi Sonrası Dropdown Sorunları

### Sorun
PrimeNG 19 ve Angular güncellemesi sonrası dropdown bileşenlerinde stil sorunları oluştu. Özellikle:
1. Dropdown içindeki metin hizalaması bozuldu
2. Dropdown panelinin genişliği dropdown genişliğiyle eşleşmiyordu
3. Dropdown panelinin arka plan rengi siyah olarak görünüyordu
4. Dropdown içindeki öğeler doğru şekilde görüntülenmiyordu

### Çözüm
PrimeNG 19'da DOM yapısı değiştiği için CSS seçicileri güncellemek gerekti. Çözüm şu adımları içerdi:

1. Bileşene özgü SCSS dosyasında daha spesifik seçiciler kullanmak:
   - `[data-pc-section="label"]` gibi yeni veri özniteliklerini hedeflemek
   - `::ng-deep` kullanarak kapsülleme sorunlarını aşmak
   - Daha yüksek özgüllüğe sahip seçiciler kullanmak

2. HTML'de dropdown bileşenine ek özellikler eklemek:
   - `panelStyleClass` ile panel için özel sınıf adı belirtmek
   - `[panelStyle]` ile doğrudan stil özellikleri belirtmek
   - `appendTo="body"` ile panelin doğru konumlandırılmasını sağlamak
   - `ng-template` kullanarak özel içerik şablonları tanımlamak

3. TypeScript'te DOM manipülasyonu eklemek:
   - `ngAfterViewInit` içinde dropdown genişliğini hesaplamak
   - CSS değişkenlerini kullanarak genişliği panele aktarmak
   - MutationObserver kullanarak dropdown durumundaki değişiklikleri izlemek

### Öğrenilen Dersler
1. PrimeNG güncellemelerinde DOM yapısı değişebilir, bu nedenle CSS seçicileri daha esnek ve dayanıklı olmalıdır
2. Bileşene özgü SCSS dosyalarında stil tanımlamak, global stil çakışmalarını önler
3. CSS değişkenleri, JavaScript ile hesaplanan değerleri CSS'e aktarmak için kullanışlıdır
4. PrimeNG'nin yeni veri öznitelikleri (`data-pc-section`, `data-pc-name` vb.) seçicilerde kullanılmalıdır
5. MutationObserver, DOM değişikliklerini izlemek ve tepki vermek için güçlü bir araçtır

### Kod Örneği
```scss
.user-role-dropdown {
  position: relative !important;
  
  [data-pc-section="label"] {
    padding-left: 42px !important;
    display: flex !important;
    align-items: center !important;
  }
}

.p-dropdown-panel.user-role-dropdown-panel {
  background-color: #ffffff !important;
  width: var(--dropdown-width, 100%) !important;
  
  [data-pc-section="items"] {
    background-color: #ffffff !important;
    
    [data-pc-section="item"] {
      color: #333333 !important;
      background-color: #ffffff !important;
    }
  }
}
```

```typescript
ngAfterViewInit() {
  setTimeout(() => {
    const dropdown = document.querySelector('.user-role-dropdown') as HTMLElement;
    if (dropdown) {
      const width = dropdown.offsetWidth;
      dropdown.style.setProperty('--dropdown-width', `${width}px`);
    }
  }, 0);
}
``` 