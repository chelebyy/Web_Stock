# Angular 19 ve PrimeNG 19 Güncellemesi

## Güncelleme Tarihi
Mart 2024

## Güncellenen Paketler
- `@angular/compiler` 18.2.13 -> 19.2.1
- `@angular/core` 18.2.13 -> 19.2.1
- `@angular/platform-browser` 18.2.13 -> 19.2.1
- `@angular/platform-browser-dynamic` 18.2.13 -> 19.2.1
- `@angular/router` 18.2.13 -> 19.2.1
- `primeng` 18.0.2 -> 19.0.9
- `zone.js` 0.14.10 -> 0.15.0

## Güncelleme Süreci

### 1. Paketlerin Güncellenmesi
```bash
npm install @angular/compiler@19.2.1 @angular/core@19.2.1 @angular/platform-browser@19.2.1 @angular/platform-browser-dynamic@19.2.1 @angular/router@19.2.1 primeng@19.0.9 zone.js@0.15.0
```

### 2. Git İşlemleri
Node modüllerinin Git tarafından izlenmemesi için aşağıdaki adımlar uygulandı:

1. `node_modules` klasörünü Git'in izleme listesinden çıkarmak için:
```bash
git rm --cached -r node_modules
```

2. Sadece `package.json` ve `package-lock.json` dosyalarını commit etmek için:
```bash
git add package.json package-lock.json
git commit -m "PrimeNG ve Angular 19 güncellemesi"
```

## Dikkat Edilmesi Gereken Noktalar

### Angular 19 Değişiklikleri
- Angular 19, performans iyileştirmeleri ve yeni özellikler içerir.
- Bazı API'ler değişmiş olabilir, bu nedenle uygulamanın tüm bölümlerinin test edilmesi önemlidir.
- Daha fazla bilgi için [Angular 19 Değişiklik Günlüğü](https://github.com/angular/angular/blob/main/CHANGELOG.md) incelenebilir.

### PrimeNG 19 Değişiklikleri
- PrimeNG 19, yeni bileşenler ve mevcut bileşenlerde iyileştirmeler içerir.
- Bazı bileşenlerin API'leri değişmiş olabilir.
- Daha fazla bilgi için [PrimeNG 19 Değişiklik Günlüğü](https://github.com/primefaces/primeng/blob/master/CHANGELOG.md) incelenebilir.

## Olası Sorunlar ve Çözümleri

### Stil Sorunları
- PrimeNG güncellemesi ile bazı bileşenlerin stilleri değişmiş olabilir.
- Özel stil tanımlamaları gözden geçirilmeli ve gerekirse güncellenmelidir.

### Bileşen API Değişiklikleri
- Bazı bileşenlerin API'leri değişmiş olabilir, bu nedenle konsol hataları kontrol edilmelidir.
- Değişen API'ler için PrimeNG dokümantasyonu incelenmelidir.

### Performans İyileştirmeleri
- Angular 19 ve PrimeNG 19, performans iyileştirmeleri içerir.
- Uygulamanın performansı izlenmeli ve gerekirse ek optimizasyonlar yapılmalıdır.

## Test Süreci
- Tüm sayfalar ve bileşenler manuel olarak test edilmelidir.
- Özellikle karmaşık formlar ve tablolar gibi bileşenler dikkatle kontrol edilmelidir.
- Konsol hataları ve uyarıları izlenmelidir.

## Geri Alma Planı
Eğer güncelleme sonrası kritik sorunlar yaşanırsa, aşağıdaki adımlar izlenebilir:

1. Önceki sürümlere geri dönmek için:
```bash
npm install @angular/compiler@18.2.13 @angular/core@18.2.13 @angular/platform-browser@18.2.13 @angular/platform-browser-dynamic@18.2.13 @angular/router@18.2.13 primeng@18.0.2 zone.js@0.14.10
```

2. Değişiklikleri commit etmek için:
```bash
git add package.json package-lock.json
git commit -m "Angular ve PrimeNG sürümlerini geri alma"
``` 