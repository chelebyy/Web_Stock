# Admin Dashboard Bilgi Tabanı

## Genel Bakış
Admin Dashboard, sistem yöneticilerinin kullanıcıları, rolleri ve sistem ayarlarını yönetmesini sağlayan merkezi bir kontrol panelidir. Ayrıca kullanıcı aktivitelerini izleme ve kaydetme özelliği de bulunmaktadır.

## Bileşenler

### 1. Kullanıcı Yönetimi
- Kullanıcı ekleme, düzenleme, silme
- Kullanıcı rollerini atama
- Kullanıcı durumunu izleme (aktif/pasif)

### 2. Rol Yönetimi
- Rol ekleme, düzenleme, silme
- Rol izinlerini yapılandırma
- Rol hiyerarşisini yönetme

### 3. Sistem Ayarları
- Genel sistem yapılandırması
- Güvenlik ayarları
- Bildirim ayarları

### 4. Kullanıcı Aktivitesi Logları
- Kullanıcı aktivitelerini kaydetme
- Aktivite loglarını görüntüleme
- Log filtreleme ve arama

## Kullanıcı Aktivitesi Log Sistemi (Güncelleme)

### Yapı
Admin Dashboard'da kullanıcı aktivitesi bölümü, manuel log girişi yerine otomatik log kaydetme ve görüntüleme sistemine dönüştürülmüştür. Bu sistem şu bileşenlerden oluşur:

1. **LogService**: Kullanıcı aktivitelerini otomatik olarak kaydetmek ve görüntülemek için API ile iletişim kurar.
2. **Filtreleme Paneli**: Logları aktivite tipi, durum, tarih aralığı ve kullanıcı adına göre filtrelemeyi sağlar.
3. **Log Tablosu**: Kaydedilen aktiviteleri görüntülemek için kullanılır.
4. **Log Detay Dialog**: Seçilen log kaydının detaylarını görüntülemek için kullanılır.
5. **Sayfalama**: Büyük log verileri için sayfalama desteği sağlar.

### Otomatik Log Kaydı
Sistem, aşağıdaki durumlarda otomatik olarak log kaydı yapar:

1. Kullanıcı giriş yaptığında
2. Kullanıcı çıkış yaptığında
3. Şifre değiştirildiğinde
4. Yönetim paneline erişildiğinde
5. Kullanıcı eklendiğinde veya güncellendiğinde
6. Rol güncellendiğinde
7. Sistem ayarları değiştirildiğinde

### Filtreleme Özellikleri
Kullanıcı aktivite logları aşağıdaki kriterlere göre filtrelenebilir:

1. **Aktivite Tipi**: Belirli bir aktivite tipine göre filtreleme
2. **Durum**: Başarılı, bilgi, uyarı veya hata durumlarına göre filtreleme
3. **Tarih Aralığı**: Belirli bir tarih aralığına göre filtreleme
4. **Kullanıcı Adı**: Belirli bir kullanıcıya göre filtreleme

### Log Detayları
Her log kaydı için aşağıdaki detaylar görüntülenebilir:

1. Kullanıcı adı ve ID
2. Aktivite tipi
3. Açıklama
4. Tarih ve saat
5. Durum
6. IP adresi
7. Ek detaylar (JSON formatında)

### Veri Modeli
```typescript
interface UserActivityLog {
  userId?: number;
  username: string;
  activityType: string;
  description: string;
  timestamp: Date;
  ipAddress?: string;
  status: string;
  details?: any;
}
```

### Aktivite Tipleri
- `login`: Kullanıcı girişi
- `logout`: Kullanıcı çıkışı
- `user_add`: Kullanıcı ekleme
- `user_update`: Kullanıcı güncelleme
- `role_update`: Rol güncelleme
- `password_change`: Şifre değiştirme
- `settings_update`: Ayar güncelleme
- `login_failed`: Başarısız giriş denemesi
- `system_backup`: Sistem yedekleme

### Durum Tipleri
- `success`: Başarılı işlem
- `info`: Bilgi
- `warning`: Uyarı
- `danger`: Hata

### Offline Destek
LogService, API'ye erişilemediğinde logları localStorage'a kaydeder ve bağlantı sağlandığında senkronize eder. Bu sayede internet bağlantısı kesilse bile log kaydı yapılabilir.

## Geliştirme Notları

### Eklenen Dosyalar
- `frontend/src/app/services/log.service.ts`: Log kaydetme ve görüntüleme servisi

### Güncellenen Dosyalar
- `frontend/src/app/components/admin-dashboard/admin-dashboard.component.ts`: Log işlevselliği eklendi
- `frontend/src/app/components/admin-dashboard/admin-dashboard.component.html`: Grafik yerine log arayüzü eklendi
- `frontend/src/app/components/admin-dashboard/admin-dashboard.component.scss`: Log arayüzü için stiller eklendi

### Backend Gereksinimleri
Backend tarafında aşağıdaki API endpoint'lerinin oluşturulması gerekiyor:
- `POST /api/logs/activity`: Tekil log kaydetme
- `POST /api/logs/bulk-activity`: Toplu log kaydetme
- `GET /api/logs/activity`: Log listeleme (sayfalama desteği ile)

## Kullanım Örnekleri

### Log Kaydetme
```typescript
this.logService.logUserActivity({
  activityType: 'user_update',
  description: 'Kullanıcı profili güncellendi',
  status: 'success'
}).subscribe();
```

### Logları Listeleme
```typescript
this.logService.getUserActivityLogs(1, 10).subscribe(response => {
  this.userActivityLogs = response.items;
  this.totalRecords = response.totalCount;
});
```

## Gelecek Geliştirmeler
1. Log filtreleme özelliği
2. Log dışa aktarma (CSV, Excel)
3. Gerçek zamanlı log görüntüleme (WebSocket)
4. Log detay görünümü
5. Log arşivleme ve temizleme

### Görünüm İyileştirmeleri
Kullanıcı aktivitesi panelinde aşağıdaki görünüm iyileştirmeleri yapılmıştır:

1. **Form ve Tablo Yapısı**: Form ve tablo için ayrı container'lar eklenerek daha düzenli bir görünüm sağlandı.
2. **Yükseklik Ayarları**: Kart yüksekliği auto olarak ayarlanarak içeriğin tam görünmesi sağlandı.
3. **Form Elemanları**: Form elemanlarının görünümü iyileştirildi, butonlar tam genişlikte gösterildi.
4. **Responsive Tasarım**: Mobil cihazlarda daha iyi görünüm için medya sorguları güncellendi.
5. **Tablo Görünümü**: Tablo başlıkları ve hücreleri için daha iyi kontrast ve okunabilirlik sağlandı.
6. **Sayfalama**: Sayfalama butonları için daha iyi görünüm ve konumlandırma yapıldı.

### CSS Sınıfları
Kullanıcı aktivitesi paneli için aşağıdaki CSS sınıfları oluşturulmuştur:

- `activity-log-card`: Ana kart için stil tanımları
- `log-form-container`: Form alanı için container
- `log-table-container`: Tablo alanı için container
- `paginator-container`: Sayfalama için container 