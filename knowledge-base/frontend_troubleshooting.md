# Frontend Sorunları ve Çözümleri

## Beyaz Sayfa Sorunu (21.02.2025)

### Sorun:
Frontend uygulaması başlatıldığında, http://localhost:4200/ adresinde beyaz bir sayfa görünüyor ve uygulama yüklenmiyor.

### Nedeni:
1. Angular uygulaması derleme sırasında bütçe sınırlarını aşıyor
2. Uygulama bileşenleri doğru şekilde yüklenemiyor
3. Angular 17.3.12 sürümü kullanılıyor ve PrimeNG 17.18.15 ile uyumluluk sorunları olabilir

### Çözüm:
1. Angular yapılandırma dosyasında (angular.json) bütçe sınırlarını artırdık:
```json
"budgets": [
  {
    "type": "initial",
    "maximumWarning": "1MB",
    "maximumError": "2MB"
  },
  {
    "type": "anyComponentStyle",
    "maximumWarning": "8kB",
    "maximumError": "16kB"
  }
]
```

2. Uygulamayı yeniden derleyip başlattık:
```powershell
npm run build
ng serve
```

3. Backend servisini de başlattık:
```powershell
cd backend/StockAPI
dotnet run
```

### Önemli Notlar:
- Angular uygulamaları büyüdükçe bütçe sınırlarını aşabilir
- Derleme sırasında oluşan hatalar uygulamanın çalışmasını engelleyebilir
- Frontend ve backend servislerinin aynı anda çalışıyor olması gerekir
- Angular 17+ sürümlerinde standalone bileşenler ve yeni router yapısı kullanılıyor

## Kontrol Listesi
- [x] Angular sürümü kontrol edildi (17.3.12)
- [x] Bütçe sınırları artırıldı
- [x] Uygulama yeniden derlendi
- [x] Frontend servisi başlatıldı
- [x] Backend servisi başlatıldı
- [x] Tarayıcı önbelleği temizlendi
- [x] Tarayıcı geliştirici araçlarında hata kontrolü yapıldı
- [x] Sorun çözüldü: Giriş sayfası başarıyla yüklendi 

## Chart.js Bağımlılık Sorunu (03.03.2025)

### Sorun:
Frontend uygulaması başlatıldığında aşağıdaki hata mesajı alınıyor:
```
X [ERROR] Could not resolve "chart.js/auto"
    node_modules/primeng/fesm2022/primeng-chart.mjs:5:18:
      5 │ import Chart from 'chart.js/auto';
        ╵                   ~~~~~~~~~~~~~~~
```

### Nedeni:
1. PrimeNG Chart bileşeni, chart.js kütüphanesine bağımlıdır
2. chart.js kütüphanesi projeye eklenmemiş
3. PrimeNG 19.0.6 sürümü, chart.js kütüphanesini otomatik olarak yüklemiyor

### Çözüm:
1. chart.js kütüphanesini projeye ekledik:
```powershell
cd frontend
npm install chart.js --save
```

2. Çalışan tüm node işlemlerini durdurduk:
```powershell
Get-Process -Name "node" -ErrorAction SilentlyContinue | Stop-Process -Force
```

3. Uygulamayı farklı bir portta başlattık:
```powershell
cd frontend
ng serve --port 4202
```

### Önemli Notlar:
- PrimeNG'nin bazı bileşenleri ek bağımlılıklar gerektirebilir
- Chart.js, PrimeNG Chart bileşeni için gerekli bir bağımlılıktır
- PowerShell'de çalışan işlemleri durdurmak için Get-Process ve Stop-Process komutları kullanılabilir
- Port çakışması durumunda farklı bir port kullanılabilir
- PowerShell'de komutları birleştirmek için && yerine ; kullanılmalıdır

## PowerShell Komut Çalıştırma Sorunları (03.03.2025)

### Sorun:
PowerShell'de && operatörü kullanıldığında aşağıdaki hata alınıyor:
```
At line:1 char:13
+ cd frontend && ng serve
+             ~~
The token '&&' is not a valid statement separator in this version.
```

### Nedeni:
1. PowerShell, Bash veya CMD'den farklı olarak && operatörünü desteklemiyor
2. PowerShell'de komutları birleştirmek için farklı bir sözdizimi kullanılıyor

### Çözüm:
1. PowerShell'de komutları birleştirmek için ; (noktalı virgül) kullanın:
```powershell
cd frontend; ng serve
```

2. Alternatif olarak, komutları ayrı ayrı çalıştırın:
```powershell
cd frontend
ng serve
```

### Önemli Notlar:
- PowerShell sözdizimi, Bash veya CMD'den farklıdır
- PowerShell'de komutları birleştirmek için ; kullanılır
- PowerShell'de koşullu komut çalıştırmak için && yerine if bloğu kullanılabilir 