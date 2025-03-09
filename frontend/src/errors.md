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