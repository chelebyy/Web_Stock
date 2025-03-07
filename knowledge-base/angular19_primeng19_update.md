# Angular 19 ve PrimeNG 19 Güncellemesi

## Güncelleme Özeti
Bu belge, projenin Angular 18'den Angular 19'a ve PrimeNG 18'den PrimeNG 19'a güncellenmesi sürecini ve karşılaşılan sorunları belgelemektedir.

## Güncellenen Paketler
- `@angular/compiler`: 18.2.13 -> 19.2.1
- `@angular/core`: 18.2.13 -> 19.2.1
- `@angular/platform-browser`: 18.2.13 -> 19.2.1
- `@angular/platform-browser-dynamic`: 18.2.13 -> 19.2.1
- `@angular/router`: 18.2.13 -> 19.2.1
- `primeng`: 18.0.2 -> 19.0.9
- `zone.js`: 0.14.10 -> 0.15.0

## Güncelleme Süreci
1. Paketlerin güncellenmesi:
   ```bash
   npm install @angular/compiler@19.2.1 @angular/core@19.2.1 @angular/platform-browser@19.2.1 @angular/platform-browser-dynamic@19.2.1 @angular/router@19.2.1 primeng@19.0.9 zone.js@0.15.0
   ```

2. Bağımlılıkların yüklenmesi:
   ```bash
   npm install
   ```

3. Git işlemleri:
   - `node_modules` klasörünün Git'in izleme listesinden çıkarılması:
     ```bash
     git rm --cached -r node_modules
     ```
   - `.git/info/exclude` dosyasına `node_modules/` eklenmesi

4. Değişikliklerin commit edilmesi:
   ```bash
   git add package.json package-lock.json
   git commit -m "Angular 19 ve PrimeNG 19 güncellemesi"
   ```

5. PrimeNG 19 uyumluluğu için stil ve bileşen güncellemeleri:
   ```bash
   git add src/styles.scss src/app/components/role-management/role-management.component.ts
   git commit -m "PrimeNG 19 uyumluluğu için stil ve bileşen güncellemeleri"
   ```

## Karşılaşılan Sorunlar ve Çözümleri

### 1. PrimeNG Tema ve CSS Dosya Yolları
**Sorun:** PrimeNG 19'da tema ve CSS dosya yolları değişmiştir.

**Çözüm:** `styles.scss` dosyasındaki import yolları güncellendi:
```scss
// Eski (hatalı)
@import "node_modules/primeng/resources/themes/lara-light-blue/theme.css";
@import "node_modules/primeng/resources/primeng.min.css";

// Yeni (doğru)
@import "primeng/resources/themes/lara-light-blue/theme.css";
@import "primeng/resources/primeng.min.css";
```

**Not:** PrimeNG 19'da, import yollarında "node_modules/" öneki kullanılmamalıdır. Doğrudan "primeng/" ile başlayan yollar kullanılmalıdır.

### 2. PrimeNG Bileşen Modülleri
**Sorun:** Bazı PrimeNG bileşen modüllerinin isimleri değişmiştir.

**Çözüm:** Değişen modül isimleri güncellendi:
```typescript
// Eski
import { InputTextareaModule } from 'primeng/inputtextarea';

// Yeni
import { InputTextarea } from 'primeng/inputtextarea';
```

### 3. ToggleButton Özellikleri
**Sorun:** ToggleButton bileşeninin `onLabel` ve `offLabel` özellikleri boş string değerlerini kabul etmiyor.

**Çözüm:** Boş string yerine geçerli değerler atandı:
```html
<!-- Eski -->
<p-toggleButton [onLabel]="''" [offLabel]="''"></p-toggleButton>

<!-- Yeni -->
<p-toggleButton onLabel="Evet" offLabel="Hayır"></p-toggleButton>
```

### 4. Checkbox Label Sorunu
**Sorun:** PrimeNG 19'da Checkbox bileşeninin `label` özelliği kaldırılmıştır.

**Çözüm:** Label özelliği yerine HTML label elementi kullanıldı:
```html
<!-- Eski -->
<p-checkbox [(ngModel)]="permission.isGranted" [binary]="true" [label]="permission.description"></p-checkbox>

<!-- Yeni -->
<p-checkbox [(ngModel)]="permission.isGranted" [binary]="true" [inputId]="'perm_' + permission.id"></p-checkbox>
<label [for]="'perm_' + permission.id">{{ permission.description }}</label>
```

### 5. Kullanılmayan İmportlar
**Sorun:** Angular 19, kullanılmayan importları daha sıkı bir şekilde kontrol ediyor.

**Çözüm:** Kullanılmayan importlar (örneğin DatePipe) kaldırıldı:
```typescript
// Eski
import { CommonModule, DatePipe } from '@angular/common';

// Yeni
import { CommonModule } from '@angular/common';
```

### 6. PowerShell'de && Operatörü Sorunu
**Sorun:** PowerShell'de && operatörü çalışmıyor.

**Çözüm:** PowerShell'de komutları birleştirmek için ; (noktalı virgül) kullanılmalı:
```powershell
# Hatalı
cd frontend && git status

# Doğru
cd frontend; git status
```

## Dikkat Edilmesi Gereken Noktalar
1. PrimeNG 19'da birçok bileşenin API'si değişmiştir. Bileşenlerin güncel kullanımı için [PrimeNG dokümantasyonu](https://primeng.org/documentation) kontrol edilmelidir.
2. Angular 19, daha sıkı tip kontrolü yapmaktadır. Tip hatalarına dikkat edilmelidir.
3. Stil dosyalarında özelleştirmeler yapıldıysa, PrimeNG 19'daki CSS sınıf değişiklikleri kontrol edilmelidir.
4. Zone.js sürümü Angular 19 ile uyumlu olmalıdır.

## Test Süreci
1. Uygulamanın tüm bölümleri test edilmelidir.
2. Konsol hataları kontrol edilmelidir.
3. Stil sorunları giderilmelidir.
4. Performans iyileştirmeleri değerlendirilmelidir.

## Kaynaklar
- [Angular Güncelleme Kılavuzu](https://update.angular.io/)
- [PrimeNG Dokümantasyonu](https://primeng.org/documentation)
- [Angular 19 Sürüm Notları](https://blog.angular.io/angular-v19-is-now-available-18fb5f560d1e)
- [PrimeNG 19 Sürüm Notları](https://primeng.org/installation) 