# Temizleme Planı

Bu belge, Clean Architecture geçişi tamamlandıktan sonra eski yapının temizlenmesi için gereken adımları açıklar.

## Ön Koşullar

Aşağıdaki koşullar sağlanmadan temizleme işlemine başlanmamalıdır:

1. Tüm bileşenlerin başarıyla yeni yapıya taşınması
2. Tüm birim testlerin başarıyla tamamlanması
3. Tüm entegrasyon testlerin başarıyla tamamlanması


## Temizleme Süreci

### 1. Yedekleme

1. Tüm proje klasörünün tam bir yedeğini alın
   ```powershell
   $date = Get-Date -Format "yyyyMMdd_HHmmss"
   Compress-Archive -Path "." -DestinationPath "backup_before_cleanup_${date}.zip"
   ```

2. Veritabanının tam bir yedeğini alın
   ```powershell
   pg_dump -U postgres -d stockdb > stockdb_backup_${date}.sql
   ```

### 2. Backend Temizliği

1. Önce eski kodların kullanılmadığından emin olun
   - Tüm API endpoint'lerinin yeni yapıya yönlendirildiğini doğrulayın
   - Swagger üzerinden tüm endpoint'leri test edin

2. Eski backend klasöründeki dosyaları kaldırın
   ```powershell
   # Yedek almadan önce bir kontrol yapın
   Get-ChildItem -Path "backend" -Recurse | Measure-Object
   # Temizleme işlemi
   Remove-Item -Path "backend" -Recurse -Force
   ```

3. Solution dosyasını güncelleyin
   - Eski projelerin referanslarını kaldırın
   - Sadece yeni projeleri içerecek şekilde düzenleyin

4. Geçersiz veya gereksiz NuGet paketlerini kaldırın
   ```powershell
   dotnet restore --force
   ```

### 3. Frontend Güncellemesi

1. Frontend API servislerinin yeni endpoint'lere işaret ettiğini doğrulayın
   - `auth.service.ts`, `user.service.ts` ve diğer servisleri kontrol edin
   - Gerekirse `environment.ts` dosyasındaki API_URL değerini güncelleyin

2. Frontend'in yeni API'ler ile çalıştığını test edin
   - Tüm login sürecini test edin
   - CRUD işlemlerini test edin
   - Yetkilendirme kontrollerini test edin

### 4. Dökümantasyon Güncellemesi

1. README.md dosyasını güncelleyin
   - Yeni proje yapısını açıklayın
   - Clean Architecture mimarisini belgeyin

2. API dökümantasyonunu güncelleyin
   - Swagger yapılandırmasını kontrol edin
   - Eksik endpoint açıklamalarını tamamlayın

3. Bilgi tabanı (Knowledge Base) belgelerini güncelleyin
   - Eski yapıya referans veren belgeleri temizleyin
   - Clean Architecture özgü belgeleri ekleyin

### 5. Son Kontroller

1. Tüm projeyi temiz bir şekilde build edin
   ```powershell
   dotnet clean
   dotnet build
   ```

2. Tüm testleri çalıştırın
   ```powershell
   dotnet test
   ```

3. Uygulamayı çalıştırın ve manuel testler yapın
   ```powershell
   # Backend
   cd src/Stock.API
   dotnet run
   
   # Frontend
   cd frontend
   ng serve
   ```

## Sorun Giderme Planı

Temizlik işlemi sırasında veya sonrasında sorunlarla karşılaşılması durumunda:

1. Hemen yedekten geri dönün
   ```powershell
   # Önce mevcut kodu yedekleyin
   $date = Get-Date -Format "yyyyMMdd_HHmmss"
   Compress-Archive -Path "." -DestinationPath "failed_cleanup_${date}.zip"
   
   # Yedekten geri yükleyin
   Expand-Archive -Path "backup_before_cleanup_*.zip" -DestinationPath "." -Force
   ```

2. Sorunun kaynağını belirleyin
   - Eksik dosya/referans kontrolü
   - Yanlış yapılandırma kontrolü
   - API endpoint bağlantı kontrolü

3. Hatayı düzeltin ve temizlik işlemini tekrarlayın
   - Daha küçük adımlarla ilerleyin
   - Her adımdan sonra test edin

## Zamanlama

- **Planlanan Tarih:** Clean Architecture geçişi tamamlandıktan bir hafta sonra
- **Tahmini Süre:** 1 gün
- **Önerilen Zamanlama:** Hafta sonu veya düşük trafik saatlerinde

## Onay ve İzleme

1. Temizlik işlemi öncesi tüm ekip üyelerinden onay alın
2. İşlem sırasında anlık durum güncellemeleri yapın
3. İşlem sonrası en az bir hafta boyunca sistemi yakından izleyin

## Son Güncelleme
- Tarih: 2025-02-21
- Güncelleme Detayı: İlk belge oluşturuldu 

## Frontend Geliştirmesi

- [ ] **Frontend Geliştirmesi**
  - [ ] Dashboard ayrıştırması (Admin ve Kullanıcı dashboardları)
  - [ ] Component'lerin oluşturulması
  - [ ] Service'lerin implementasyonu
  - [ ] PrimeNG entegrasyonu
  - [ ] Routing yapılandırması 