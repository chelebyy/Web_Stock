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