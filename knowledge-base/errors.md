# Hatalar ve Çözümler

Bu dosya, proje geliştirme sürecinde karşılaşılan hataları ve çözümlerini içermektedir.

## İçindekiler
- [Clean Architecture Geçişi Hataları](#clean-architecture-geçişi-hataları)
- [Infrastructure Katmanı Hataları](#infrastructure-katmanı-hataları)
- [API Katmanı Hataları](#api-katmanı-hataları)
- [Null Değer Uyarıları](#null-değer-uyarıları)
- [Backend ve Frontend Başlatma Sorunları](#backend-ve-frontend-başlatma-sorunları)
- [Genel Öneriler](#genel-öneriler)
- [Veritabanı Yapılandırma Hataları](#veritabanı-yapılandırma-hataları)
- [API İstek Hataları](#api-i̇stek-hataları)
- [Şifre Hashleme ve Doğrulama Sorunları](#şifre-hashleme-ve-doğrulama-sorunları)
- [Bağımlılık Enjeksiyon Hataları](#bağımlılık-enjeksiyon-hataları)
- [Login Hataları](#login-hataları)
- [PowerShell Komut Çalıştırma Sorunları](#powershell-komut-çalıştırma-sorunları)
- [Profil Resmi Ekleme Sorunları](#profil-resmi-ekleme-sorunları)
- [Şifre Değiştirme Formu Sorunları](#şifre-değiştirme-formu-sorunları)
- [Frontend Rol Yönetimi Hataları](#frontend-rol-yönetimi-hataları)
- [Login Sicil Sorunu](#login-sicil-sorunu)
- [Login 401 Unauthorized Hatası](#login-401-unauthorized-hatası)
- [Kullanıcı İzinleri Yönetimi Entegrasyonu](#kullanıcı-izinleri-yönetimi-entegrasyonu)
- [PrimeNG 19 Uyumluluk Sorunları](#primeNG-19-uyumluluk-sorunları)
- [User Model Uyumsuzluğu Hatası](#user-model-uyumsuzluğu-hatası)
- [Backend Model Uyumsuzluğu ve Migration Hataları](#backend-model-uyumsuzluğu-ve-migration-hataları)
- [Veritabanı Temizliği Sonrası Login Hatası](#veritabanı-temizliği-sonrası-login-hatası)
- [Kullanıcı Dashboard Yeniden Tasarımı Hataları](#kullanıcı-dashboard-yeniden-tasarımı-hataları)
- [Profil Resmi Placeholder Hatası (06.03.2025)](#profil-resmi-placeholder-hatası-06032025)
- [Profil Resmi ve API Endpoint Hataları (06.03.2025)](#profil-resmi-ve-api-endpoint-hataları-06032025)
- [Profil Resmi Endpoint Hatası - Geçici Çözüm (06.03.2025)](#profil-resmi-endpoint-hatası-geçici-çözüm-06032025)
- [Revir ve BilgiIslem İzinlerinin Görünmeme Sorunu](#revir-ve-bilgiislem-izinlerinin-görünmeme-sorunu)
- [Angular 19 Geçiş Süreci Hataları ve Çözümleri](#angular-19-geçiş-süreci-hataları-ve-çözümleri)
- [Angular 19 ve PrimeNG 19 Güncelleme Hataları](#angular-19-ve-primeNG-19-güncelleme-hataları)
- [Rol Yönetimi Sayfasına Erişim Sorunu](#rol-yönetimi-sayfasına-erişim-sorunu)
- [Rol Yönetimi Sayfasına Erişim Sorunu - Ek İyileştirmeler](#rol-yönetimi-sayfasına-erişim-sorunu---ek-iyileştirmeler)
- [Rol Yönetimi Sayfasına Erişim Sorunu - Acil Çözüm](#rol-yönetimi-sayfasına-erişim-sorunu---acil-çözüm)
- [Bilgi İşlem Sayfası Erişim İzinleri Sorunu](#bilgi-işlem-sayfası-erişim-izinleri-sorunu)
- [Admin Dashboard Yönetimi Modülü Sorunu](#admin-dashboard-yönetimi-modülü-sorunu)
- [Dashboard Management Modülü Hataları](#dashboard-management-modülü-hataları)
- [PrimeNG Tablo Bileşenleri Koyu Tema Sorunları](#primeNG-tablo-bileşenleri-koyu-tema-sorunları)

## Kullanıcı Aktivitesi Grafiği Yerine Log Kaydetme Sistemi

### Yapılan Değişiklikler
1. Yeni bir `LogService` oluşturuldu
2. Kullanıcı aktivitesi grafiği yerine log kaydetme ve görüntüleme arayüzü eklendi
3. Admin dashboard bileşeni güncellendi

### Potansiyel Hatalar ve Çözümleri

#### 1. API Bağlantı Hatası
**Hata:** API'ye bağlanırken "404 Not Found" hatası alınabilir.
**Çözüm:** Backend tarafında `/api/logs/activity` ve `/api/logs/bulk-activity` endpoint'lerinin oluşturulması gerekiyor.

#### 2. CORS Hatası
**Hata:** "Access to XMLHttpRequest has been blocked by CORS policy" hatası alınabilir.
**Çözüm:** Backend tarafında CORS ayarlarının düzenlenmesi gerekiyor.

#### 3. Sayfalama Hatası
**Hata:** Sayfalama çalışmayabilir veya hatalı sonuçlar gösterebilir.
**Çözüm:** Backend tarafında sayfalama parametrelerinin doğru işlenmesi ve frontend'de doğru parametrelerin gönderilmesi gerekiyor.

#### 4. LocalStorage Hatası
**Hata:** LocalStorage'a log kaydetme işlemi başarısız olabilir.
**Çözüm:** Tarayıcının localStorage desteğini kontrol edin ve kullanıcının tarayıcı ayarlarında localStorage'ın etkin olduğundan emin olun.

#### 5. Tarih Formatı Hatası
**Hata:** Tarih formatı hatalı görüntülenebilir.
**Çözüm:** Angular'ın DatePipe'ını kullanarak tarihleri doğru formatta görüntüleyin.

#### 6. Görünüm Sorunları
**Hata:** Kullanıcı aktivitesi paneli tam olarak görünmeyebilir, form alanları ve butonlar düzgün görüntülenmeyebilir.
**Çözüm:** 
- HTML yapısını düzenleyerek form ve tablo için container'lar eklemek
- CSS stillerini güncelleyerek yükseklik ve genişlik değerlerini düzeltmek
- Responsive tasarım için medya sorgularını güncellemek
- Form elemanlarının stillerini iyileştirmek

#### 7. Kullanıcı Aktivitesi Tasarım Sorunu
**Hata:** Kullanıcı aktivitesi bölümünde manuel log girişi yapılması kullanışlı değil, sistem otomatik olarak log kaydetmeli.
**Çözüm:** 
- Manuel log girişi formunu kaldırıp, otomatik kaydedilen logları görüntüleyen bir yapıya dönüştürmek
- Filtreleme özellikleri eklemek
- Log detaylarını görüntülemek için dialog eklemek
- Sistem tarafından otomatik log kaydı yapmak

### Yapılan İyileştirmeler (Güncelleme)
1. Manuel log girişi formu kaldırıldı
2. Filtreleme özellikleri eklendi (aktivite tipi, durum, tarih aralığı, kullanıcı adı)
3. Log detaylarını görüntülemek için dialog eklemek
4. Sistem tarafından otomatik log kaydı yapılması sağlandı
5. Boş veri durumu için daha iyi görsel geri bildirim eklendi
6. Tarih aralığı filtresi için takvim bileşeni eklendi

### Performans İyileştirmeleri
1. Büyük log verileri için sayfalama kullanıldı
2. API bağlantısı olmadığında localStorage'a yedekleme yapıldı
3. Hata durumlarında örnek veriler gösterilerek kullanıcı deneyimi iyileştirildi

### Güvenlik Önlemleri
1. Kullanıcı kimlik doğrulaması yapıldı
2. Hassas bilgiler loglanmadı
3. Log kayıtları için yetkilendirme kontrolleri eklendi

### Gelecek İyileştirmeler
1. Log filtreleme özelliği eklenebilir
2. Log dışa aktarma (CSV, Excel) özelliği eklenebilir
3. Gerçek zamanlı log görüntüleme için WebSocket entegrasyonu yapılabilir

## PowerShell Komut Çalıştırma Sorunları

### && Operatörü Sorunu

**Sorun:** Windows PowerShell'de && operatörü komutları birleştirmek için çalışmıyor.

```
PS C:\Users\IT\Documents\Cz_Web\Web_Stock> cd frontend && npm run build
At line:1 char:13
+ cd frontend && npm run build
+             ~~
The token '&&' is not a valid statement separator in this version.
```

**Çözüm:** PowerShell'de komutları birleştirmek için aşağıdaki yöntemler kullanılabilir:

1. Komutları ayrı ayrı çalıştırma:
```powershell
cd frontend
npm run build
```

2. Semicolon (;) kullanma:
```powershell
cd frontend; npm run build
```

3. PowerShell pipeline kullanma:
```powershell
cd frontend | Out-Null; npm run build
```

4. Komut bloğu kullanma:
```powershell
cd frontend; if ($?) { npm run build }
```

**Not:** PowerShell'de komut çalıştırırken Unix/Linux sistemlerindeki gibi && operatörü yerine yukarıdaki alternatif yöntemler kullanılmalıdır. 

## Şifre Değiştirme Formu Sorunları

### Şifre Değiştirme Formu Etiket Hizalama Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
"Yeni Şifre" etiketi yukarı doğru kayıyor ve kötü görünüyor.
```

**Nedeni:**
1. p-float-label yapısı ile input alanları arasında hizalama sorunu
2. CSS stil tanımlamalarında çakışmalar
3. PrimeNG bileşenlerinin yapısı ile uyumsuzluk

**Çözüm:**
1. p-float-label yapısı yerine p-inputgroup yapısını kullanmak:
```html
<div class="p-inputgroup">
  <span class="p-inputgroup-addon">
    <i class="pi pi-key"></i>
  </span>
  <input [type]="showNewPassword ? 'text' : 'password'" pInputText id="newPassword" 
         [(ngModel)]="newPassword" placeholder="Yeni Şifre" class="modern-input" />
  <button pButton type="button" class="p-button-text p-button-rounded p-button-plain" 
          (click)="showNewPassword = !showNewPassword">
    <i class="pi" [ngClass]="showNewPassword ? 'pi-eye' : 'pi-eye-slash'"></i>
  </button>
</div>
```

2. CSS stillerini güncellemek:
```css
.modern-input {
  background-color: rgba(255, 255, 255, 0.1) !important;
  border: 1px solid rgba(255, 255, 255, 0.2) !important;
  border-radius: 0 !important;
  color: #fff !important;
  padding: 0.5rem 0.75rem !important;
  width: 100% !important;
  transition: all 0.3s ease !important;
  font-size: 0.875rem !important;
  height: auto !important;
}

.p-inputgroup-addon {
  background-color: rgba(255, 255, 255, 0.05) !important;
  border-color: rgba(255, 255, 255, 0.2) !important;
  color: rgba(255, 255, 255, 0.7) !important;
  border-radius: 4px 0 0 4px !important;
}

.p-button-plain {
  color: rgba(255, 255, 255, 0.7) !important;
  background: rgba(255, 255, 255, 0.05) !important;
  border: 1px solid rgba(255, 255, 255, 0.2) !important;
  border-radius: 0 4px 4px 0 !important;
}
```

3. p-inputgroup yapısını desteklemek için ek CSS eklemek:
```css
.p-inputgroup {
  display: flex;
  align-items: stretch;
}

.p-inputgroup > .p-inputtext {
  flex: 1 1 auto;
}

.p-fluid .p-inputgroup {
  display: flex;
}
```

**Önemli Notlar:**
- p-float-label yapısı yerine p-inputgroup yapısı kullanıldı
- Input alanlarının sol tarafına ikonlar eklendi
- Sağ tarafta şifre göster/gizle butonu eklendi
- Input alanlarının border-radius değerleri düzenlendi
- Placeholder metinleri doğrudan input alanlarına eklendi
- Tüm şifre alanları için aynı yapı kullanıldı (mevcut şifre, yeni şifre, şifre tekrarı)

### Şifre Değiştirme Formu Buton Hizalama Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
Değiştir ve İptal butonları düzgün hizalanmıyor.
```

**Nedeni:**
1. Butonların stil tanımlamaları yetersiz
2. Flex yapısı doğru kullanılmamış
3. Responsive tasarım eksiklikleri

**Çözüm:**
1. Buton container'ı için flex yapısını düzenlemek:
```css
.password-actions {
  display: flex;
  gap: 0.75rem;
  margin-top: 1.5rem;
  justify-content: center;
}
```

2. Buton stillerini güncellemek:
```css
.change-button {
  background: linear-gradient(135deg, #6366f1, #8b5cf6) !important;
  border: none !important;
  box-shadow: 0 2px 8px rgba(99, 102, 241, 0.3) !important;
  transition: all 0.3s ease !important;
  font-size: 0.875rem !important;
}

.change-button:hover {
  background: linear-gradient(135deg, #4f46e5, #7c3aed) !important;
  box-shadow: 0 4px 12px rgba(99, 102, 241, 0.4) !important;
  transform: translateY(-1px) !important;
}

.cancel-button {
  border-color: rgba(255, 255, 255, 0.3) !important;
  color: rgba(255, 255, 255, 0.8) !important;
  transition: all 0.3s ease !important;
  font-size: 0.875rem !important;
}

.cancel-button:hover {
  background-color: rgba(255, 255, 255, 0.1) !important;
  border-color: rgba(255, 255, 255, 0.5) !important;
}
```

3. HTML'de butonları güncellemek:
```html
<div class="password-actions">
  <button pButton class="p-button-sm p-button-primary change-button" label="Değiştir" 
          icon="pi pi-check" (click)="changePassword()"></button>
  <button pButton class="p-button-sm p-button-outlined p-button-secondary cancel-button" 
          label="İptal" icon="pi pi-times" (click)="showPasswordChange = false"></button>
</div>
```

**Önemli Notlar:**
- Butonlar için p-button-sm sınıfı kullanıldı
- İkonlar eklendi (pi-check ve pi-times)
- Butonlar arasında gap değeri azaltıldı (1rem -> 0.75rem)
- Butonlar için hover efektleri iyileştirildi
- Değiştir butonu için gradient arka plan kullanıldı
- İptal butonu için outline stil kullanıldı

### Şifre Değiştirme Formu Sürükle-Bırak Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
Şifre değiştirme formu sürüklenemiyor.
```

**Nedeni:**
1. Angular CDK DragDrop modülü eksik
2. cdkDrag ve cdkDragHandle direktifleri doğru uygulanmamış
3. Sürükleme sınırları tanımlanmamış

**Çözüm:**
1. Angular CDK DragDrop modülünü eklemek:
```bash
npm install @angular/cdk
```

2. Component'in imports dizisine DragDropModule eklemek:
```typescript
import { DragDropModule } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
  standalone: true,
  imports: [
    // ... diğer importlar
    DragDropModule
  ]
})
```

3. HTML'de cdkDrag ve cdkDragHandle direktiflerini eklemek:
```html
<div class="password-change-form" *ngIf="showPasswordChange" cdkDrag cdkDragBoundary=".dashboard-container">
  <div class="password-form-header" cdkDragHandle>
    <h2>Şifre Değiştir</h2>
    <button class="close-button" (click)="showPasswordChange = false">
      <i class="pi pi-times"></i>
    </button>
  </div>
  <!-- Form içeriği -->
</div>
```

4. Sürükleme stilleri eklemek:
```css
/* Sürükleme sırasındaki stil */
.cdk-drag-preview {
  box-shadow: 0 15px 40px rgba(0, 0, 0, 0.4) !important;
  opacity: 0.8;
}

/* Sürükleme animasyonu */
.cdk-drag-animating {
  transition: transform 250ms cubic-bezier(0, 0, 0.2, 1);
}
```

**Önemli Notlar:**
- Angular CDK DragDrop modülü, sürükle-bırak işlevselliği için gereklidir
- cdkDrag direktifi, sürüklenebilir öğeyi tanımlar
- cdkDragHandle direktifi, sürükleme için kullanılacak tutamacı tanımlar
- cdkDragBoundary, sürükleme sınırlarını belirler
- Sürükleme sırasında ve sonrasında animasyonlar için CSS stilleri eklendi
- Form başlığı, sürükleme tutamacı olarak kullanılıyor
- Cursor: move özelliği, sürüklenebilir olduğunu göstermek için eklendi

### Şifre Değiştirme Formu Responsive Tasarım Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
Şifre değiştirme formu küçük ekranlarda düzgün görüntülenmiyor.
```

**Nedeni:**
1. Responsive tasarım için media query'ler eksik
2. Form genişliği sabit değerlerle tanımlanmış
3. Mobil cihazlarda form elemanları çok büyük

**Çözüm:**
1. Responsive tasarım için media query'ler eklemek:
```css
@media screen and (max-width: 768px) {
  .password-change-form {
    width: 90%;
    max-width: 350px;
    padding: 1rem;
  }
  
  .field {
    margin-bottom: 0.75rem;
  }
  
  .modern-input {
    font-size: 0.8rem !important;
  }
  
  .password-form-header h2 {
    font-size: 1.1rem;
  }
  
  .password-actions {
    gap: 0.5rem;
  }
  
  .change-button, .cancel-button {
    font-size: 0.8rem !important;
    padding: 0.5rem 1rem !important;
  }
}
```

2. Form genişliğini yüzde değerlerle tanımlamak:
```css
.password-change-form {
  max-width: 400px;
  width: 90%;
  margin-left: auto;
  margin-right: auto;
}
```

3. Form elemanlarının boyutlarını düzenlemek:
```css
.modern-input {
  height: auto !important;
  font-size: 0.875rem !important;
}

.p-inputgroup-addon {
  padding: 0.5rem 0.75rem !important;
}

.p-button-plain {
  padding: 0.5rem !important;
}
```

**Önemli Notlar:**
- Media query'ler ile küçük ekranlarda form elemanlarının boyutları küçültüldü
- Form genişliği yüzde değerlerle tanımlandı (max-width ile sınırlandırılarak)
- Input alanlarının yüksekliği auto olarak ayarlandı
- Butonların padding değerleri küçük ekranlar için azaltıldı
- Font boyutları küçük ekranlar için azaltıldı
- Gap değerleri küçük ekranlar için azaltıldı
- Form başlığı boyutu küçük ekranlar için azaltıldı

### Şifre Değiştirme Formu Animasyon Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
Şifre değiştirme formu açılırken ve kapanırken animasyon yok.
```

**Nedeni:**
1. Animasyon tanımlamaları eksik
2. *ngIf direktifi animasyonsuz çalışıyor
3. CSS transition özellikleri eksik

**Çözüm:**
1. Animasyon tanımlamak:
```css
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
```

2. Animasyonu forma uygulamak:
```css
.password-change-form {
  animation: fadeIn 0.3s ease-in-out;
  /* Diğer stiller */
}
```

3. Alternatif olarak Angular animasyonlarını kullanmak:
```typescript
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  // ... diğer özellikler
  animations: [
    trigger('fadeInOut', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-20px)' }),
        animate('300ms ease-in-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('300ms ease-in-out', style({ opacity: 0, transform: 'translateY(-20px)' }))
      ])
    ])
  ]
})
```

4. HTML'de animasyonu uygulamak:
```html
<div class="password-change-form" *ngIf="showPasswordChange" [@fadeInOut] cdkDrag cdkDragBoundary=".dashboard-container">
  <!-- Form içeriği -->
</div>
```

**Önemli Notlar:**
- CSS keyframes ile basit bir fadeIn animasyonu tanımlandı
- Animasyon süresi 0.3 saniye olarak ayarlandı
- Ease-in-out timing fonksiyonu kullanıldı
- Animasyon hem opacity hem de transform özelliklerini değiştiriyor
- Alternatif olarak Angular'ın kendi animasyon sistemi de kullanılabilir
- Angular animasyonları daha karmaşık ama daha kontrol edilebilir
- CSS animasyonları daha basit ve performanslı

### Şifre Değiştirme Formu Arka Plan Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
Şifre değiştirme formu arka planı yeterince kontrast sağlamıyor.
```

**Nedeni:**
1. Arka plan rengi çok açık
2. Arka plan opaklığı çok düşük
3. Metin rengi ile arka plan arasında yeterli kontrast yok

**Çözüm:**
1. Arka plan rengini ve opaklığını ayarlamak:
```css
.password-change-form {
  background: linear-gradient(135deg, #2a3f5f, #1e293b);
  /* Diğer stiller */
}
```

2. Metin rengini ayarlamak:
```css
.password-form-header h2 {
  color: #fff;
}

.modern-input {
  color: #fff !important;
}

.p-inputgroup-addon {
  color: rgba(255, 255, 255, 0.7) !important;
}
```

3. Gölge ve kenarlık eklemek:
```css
.password-change-form {
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
  border: 1px solid rgba(255, 255, 255, 0.1);
  /* Diğer stiller */
}
```

**Önemli Notlar:**
- Koyu renkli gradient arka plan kullanıldı
- Metin rengi beyaz olarak ayarlandı
- Gölge ve kenarlık eklenerek form daha belirgin hale getirildi
- Input alanları için arka plan rengi yarı saydam olarak ayarlandı
- Butonlar için kontrast renkler kullanıldı
- Değiştir butonu için mor-mavi gradient, İptal butonu için saydam arka plan kullanıldı
- Hover durumlarında kontrast artırıldı

### Şifre Değiştirme Formu Pozisyonlama Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
Şifre değiştirme formu ekranın ortasında değil.
```

**Nedeni:**
1. Form pozisyonu sabit değerlerle tanımlanmış
2. Responsive tasarım için pozisyonlama eksik
3. Sürüklenebilir form için başlangıç pozisyonu uygun değil

**Çözüm:**
1. Form pozisyonunu ayarlamak:
```css
.password-change-form {
  position: absolute;
  top: 80px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 1000;
  /* Diğer stiller */
}
```

2. Admin dashboard için farklı pozisyonlama:
```css
.password-change-form {
  position: absolute;
  top: 80px;
  right: 20px;
  z-index: 1000;
  /* Diğer stiller */
}
```

3. Sürüklenebilir form için cursor stilini eklemek:
```css
.password-change-form {
  cursor: move;
  /* Diğer stiller */
}

.password-form-header {
  cursor: move;
  /* Diğer stiller */
}
```

**Önemli Notlar:**
- Admin dashboard için form sağ üst köşede konumlandırıldı
- Kullanıcı dashboard için form ekranın üst ortasında konumlandırıldı
- Form için z-index değeri yüksek tutularak diğer elemanların üzerinde görünmesi sağlandı
- Sürüklenebilir form için cursor: move stili eklendi
- Form başlığı için de cursor: move stili eklendi
- Pozisyonlama için absolute kullanıldı
- Backdrop filter eklenerek form arkasındaki içeriğin bulanıklaştırılması sağlandı

### Şifre Değiştirme Formu Validasyon Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
Şifre değiştirme formunda validasyon mesajları görünmüyor.
```

**Nedeni:**
1. Validasyon mesajları için HTML yapısı eksik
2. Validasyon kontrolleri eksik
3. Validasyon mesajları için stiller eksik

**Çözüm:**
1. Validasyon mesajları için HTML yapısı eklemek:
```html
<div class="field">
  <div class="p-inputgroup">
    <!-- Input alanı -->
  </div>
  <small *ngIf="passwordForm.get('newPassword')?.invalid && passwordForm.get('newPassword')?.touched" 
         class="p-error">Şifre en az 6 karakter olmalıdır.</small>
</div>
```

2. Reactive form kullanmak:
```typescript
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

// Component içinde
passwordForm: FormGroup;

constructor(private fb: FormBuilder) {
  this.passwordForm = this.fb.group({
    currentPassword: ['', Validators.required],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', Validators.required]
  }, { validator: this.passwordMatchValidator });
}

passwordMatchValidator(g: FormGroup) {
  return g.get('newPassword')?.value === g.get('confirmPassword')?.value
    ? null : { 'mismatch': true };
}
```

3. Validasyon mesajları için stiller eklemek:
```css
.p-error {
  color: #ef4444 !important;
  font-size: 0.75rem;
  margin-top: 0.25rem;
  display: block;
}

.ng-invalid.ng-touched {
  border-color: #ef4444 !important;
}
```

**Önemli Notlar:**
- Reactive form kullanılarak form validasyonu sağlandı
- Şifre için minimum uzunluk kontrolü eklendi
- Şifre eşleşme kontrolü için özel validator eklendi
- Validasyon mesajları için p-error sınıfı kullanıldı
- Hatalı input alanları için kırmızı kenarlık eklendi
- Validasyon mesajları küçük font boyutu ile gösterildi
- Validasyon mesajları sadece alan dokunulduğunda (touched) gösterildi

### Şifre Değiştirme Formu API Entegrasyon Sorunu
**Tarih:** 2025-03-04
**Hata Mesajı:** 
```
Şifre değiştirme API çağrısı 404 hatası veriyor.
```

**Nedeni:**
1. API endpoint'i yanlış
2. AuthService'de şifre değiştirme metodu eksik
3. Token gönderilmiyor

**Çözüm:**
1. AuthService'e şifre değiştirme metodu eklemek:
```typescript
changePassword(currentPassword: string, newPassword: string): Observable<any> {
  const url = `${this.apiUrl}/auth/change-password`;
  return this.http.post(url, { currentPassword, newPassword }, {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.getToken()}`
    })
  }).pipe(
    tap(response => {
      console.log('Şifre değiştirme başarılı:', response);
    }),
    catchError(error => {
      console.error('Şifre değiştirme hatası:', error);
      return throwError(() => error);
    })
  );
}
```

2. Component'te API çağrısını yapmak:
```typescript
changePassword() {
  if (this.passwordForm.invalid) {
    this.markFormGroupTouched(this.passwordForm);
    return;
  }

  if (this.newPassword !== this.confirmPassword) {
    this.messageService.add({
      severity: 'error',
      summary: 'Hata',
      detail: 'Yeni şifre ve şifre tekrarı eşleşmiyor.'
    });
    return;
  }

  this.loading = true;
  this.authService.changePassword(this.currentPassword, this.newPassword)
    .pipe(finalize(() => this.loading = false))
    .subscribe({
      next: (response) => {
        this.messageService.add({
          severity: 'success',
          summary: 'Başarılı',
          detail: 'Şifreniz başarıyla değiştirildi.'
        });
        this.showPasswordChange = false;
        this.resetForm();
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Hata',
          detail: error.error?.errorMessage || 'Şifre değiştirme işlemi başarısız oldu.'
        });
      }
    });
}

markFormGroupTouched(formGroup: FormGroup) {
  Object.values(formGroup.controls).forEach(control => {
    control.markAsTouched();
    if (control instanceof FormGroup) {
      this.markFormGroupTouched(control);
    }
  });
}

resetForm() {
  this.currentPassword = '';
  this.newPassword = '';
  this.confirmPassword = '';
  if (this.passwordForm) {
    this.passwordForm.reset();
  }
}
```

3. API endpoint'ini kontrol etmek ve düzeltmek:
```csharp
// AuthController.cs
[Authorize]
[HttpPost("change-password")]
public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
{
    // ... mevcut kod
}
```

**Önemli Notlar:**
- AuthService'e şifre değiştirme metodu eklendi
- API çağrısı için Authorization header'ı eklendi
- Hata yönetimi için catchError operatörü kullanıldı
- Başarılı ve başarısız durumlar için kullanıcıya bildirim gösterildi
- Form validasyonu için markFormGroupTouched metodu eklendi
- İşlem tamamlandığında form sıfırlandı
- Loading durumu için finalize operatörü kullanıldı
- API endpoint'i doğru şekilde tanımlandı 

## Frontend Rol Yönetimi Hataları

### Hata 1: Role modeli uyumsuzluğu
**Hata Açıklaması:** Role modeli güncellendiğinde, RoleManagementComponent içinde hala eski model yapısı kullanılıyordu. Bu durum, `Type 'Role[]' is not assignable to type 'RoleWithUsers[]'` ve `Property 'description' is missing in type '{ id: any; name: any; }' but required in type 'Omit<Role, "id" | "createdAt" | "updatedAt">'` hatalarına neden oldu.

**Çözüm:** 
1. RoleManagementComponent içinde `roles` değişkeni `RoleWithUsers[]` yerine `Role[]` olarak değiştirildi.
2. Form yapısı güncellendi ve `description` alanı eklendi.
3. HTML şablonu güncellendi, gereksiz alanlar kaldırıldı ve izin yönetimi butonu eklendi.
4. RoleService'deki metotlar güncellendi.

```typescript
// Önceki hatalı kod
roles: RoleWithUsers[] = [];
this.roleForm = this.fb.group({
  id: [null],
  name: ['', [Validators.required]]
});

// Düzeltilmiş kod
roles: Role[] = [];
this.roleForm = this.fb.group({
  id: [null],
  name: ['', [Validators.required]],
  description: ['', [Validators.required]]
});
```

### Hata 2: Form değerlerinin yanlış alınması
**Hata Açıklaması:** Form değerleri yanlış şekilde alınıyordu, özellikle dropdown kullanımı sorunluydu.

**Çözüm:** 
1. Dropdown yerine normal input alanları kullanıldı.
2. Form değerlerinin alınma şekli düzeltildi:

```typescript
// Önceki hatalı kod
const selectedRole = this.roleForm.get('name')?.value;
const roleData = {
  id: this.editMode ? selectedRole.id : null,
  name: selectedRole.name
};

// Düzeltilmiş kod
const roleData = {
  id: this.roleForm.get('id')?.value,
  name: this.roleForm.get('name')?.value,
  description: this.roleForm.get('description')?.value
};
```

### Hata 3: API URL'lerinin yanlış yapılandırılması
**Hata Açıklaması:** RoleService içindeki API URL'leri yanlış yapılandırılmıştı.

**Çözüm:** URL'ler düzeltildi:

```typescript
// Önceki hatalı kod
getRoles(): Observable<Role[]> {
  return this.http.get<Role[]>(`${this.apiUrl}/roles`);
}

getRoleById(id: number): Observable<Role> {
  return this.http.get<Role>(`${this.apiUrl}/roles/${id}`);
}

// Düzeltilmiş kod
getRoles(): Observable<Role[]> {
  return this.http.get<Role[]>(`${this.apiUrl}`);
}

getRoleById(id: number): Observable<Role> {
  return this.http.get<Role>(`${this.apiUrl}/${id}`);
}
```

## Login İşleminde Sicil Numarası Kullanma

### Yapılan Değişiklik
**Açıklama:** Login işlemi kullanıcı adı yerine sicil numarası kullanacak şekilde değiştirildi.

**Değiştirilen Dosyalar:**
1. `frontend/src/app/models/auth.model.ts`: LoginRequest arayüzünde "username" yerine "sicil" eklendi
2. `frontend/src/app/models/user.model.ts`: User arayüzünde sicil alanı güncellemesi
3. `frontend/src/app/services/auth.service.ts`: Login metodu sicil kullanacak şekilde güncellendi
4. `frontend/src/app/components/login/login.component.ts`: Bileşen sicil değişkeni kullanacak şekilde güncellendi
5. `frontend/src/app/components/login/login.component.html`: Kullanıcı adı giriş alanı sicil numarası olarak değiştirildi
6. Backend tarafında `LoginDto` sınıfı ve `AuthService` sınıfları sicil numarası kullanacak şekilde güncellendi

**Dikkat Edilmesi Gerekenler:**
1. Login işlemi artık kullanıcı adı yerine sicil numarası ile yapılıyor
2. Kullanıcı oluşturma ve güncelleme işlemlerinde sicil numarası alanının zorunlu olması sağlandı
3. Hata mesajlarında "kullanıcı adı" yerine "sicil numarası" ifadesi kullanıldı
4. Frontend tarafında login işlemi için istek formatı değiştiği için API endpoint'i de buna göre güncellendi

**Potansiyel Hatalar:**
1. Eski kullanıcı adı ile giriş yapmaya çalışmak
2. Backend'in sicil numarası ile kullanıcı bulma işlemi için uygun metot eksikliği
3. Login formunda yanlış alan adı kullanılması

**Çözümler:**
1. Kullanıcılara sicil numaralarını kullanmaları gerektiği bildirilmeli
2. Backend'de UserRepository'ye GetBySicilAsync metodu eklendi
3. Login bileşeninde tüm kullanıcı adı alanları sicil alanı ile değiştirildi

# Login Sicil Sorunu

## Kullanıcı Sicil Alanı Eksikliği (05.03.2025)

**Hata:** 
```
Kullanıcı girişi başarısız. Geçersiz sicil numarası veya şifre.
```

**Nedeni:**
1. Login işlemi kullanıcı adı yerine sicil numarası kullanacak şekilde değiştirilmiş
2. Ancak SeedData.cs dosyasında oluşturulan kullanıcılara (admin ve user) sicil numarası atanmamış
3. User entity sınıfında sicil alanı `[Required]` olarak işaretlenmiş, yani zorunlu bir alan
4. AuthService.cs içindeki login metodu, kullanıcıyı `GetBySicilAsync` metodu ile sicil numarasına göre arıyor
5. Kullanıcılar sicil numarasına sahip olmadığı için giriş başarısız oluyor

**Çözüm:**
1. `FixPasswordController` adında özel bir controller oluşturuldu:
```csharp
[ApiController]
[Route("api/[controller]")]
public class FixPasswordController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public FixPasswordController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("fix-users")]
    public async Task<IActionResult> FixUsers()
    {
        // Admin kullanıcısına sicil numarası ekle
        var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
        if (adminUser != null)
        {
            adminUser.Sicil = "A001";
            await _context.SaveChangesAsync();
        }

        // User kullanıcısına sicil numarası ekle
        var normalUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "user");
        if (normalUser != null)
        {
            normalUser.Sicil = "U001";
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = "Kullanıcılara sicil numarası eklendi. Admin: A001, User: U001" });
    }
}
```

2. Backend API yeniden başlatıldı
3. `http://localhost:5037/api/FixPassword/fix-users` endpoint'i çağrılarak kullanıcılara sicil numarası eklendi
4. Frontend uygulaması başlatıldı
5. Aşağıdaki bilgilerle giriş yapıldı:
   - Admin: Sicil: A001, Şifre: admin123
   - User: Sicil: U001, Şifre: user123

**Öğrenilen Dersler:**
- Entity'lere yeni zorunlu alanlar eklendiğinde, mevcut verilerin bu alanlarla güncellenmesi gerekir
- SeedData sınıfında oluşturulan kullanıcıların tüm zorunlu alanları içermesi gerekir
- Veritabanı şeması değiştiğinde, mevcut verilerin yeni şemaya uygun şekilde güncellenmesi gerekir
- Özel controller'lar oluşturarak veritabanı düzeltmeleri yapmak, acil durumlarda etkili bir çözüm olabilir

**Detaylı Bilgi:**
- [Login Sicil Sorunu ve Çözümü](login_sicil_fix.md)

# Login 401 Unauthorized Hatası

## Şifre Doğrulama Sorunu (05.03.2025)

**Hata:**
```
POST http://localhost:5037/api/auth/login 401 (Unauthorized)
```

**Nedeni:**
1. Kullanıcı sicil numarası (A001) ve şifresi (admin123) ile giriş yapmaya çalışıyor
2. Sicil numarası veritabanında güncellenmiş, ancak şifre hash'i güncellenememiş olabilir
3. Şifre hash'i güncellenmiş, ancak hash algoritması veya parametreleri (örneğin work factor) değişmiş olabilir
4. AuthService sınıfındaki login metodu, şifre doğrulama işlemini doğru şekilde yapamıyor olabilir

**Çözüm:**
1. `FixPasswordController` sınıfına `fix-passwords` endpoint'i eklendi:
```csharp
[HttpGet("fix-passwords")]
public async Task<IActionResult> FixPasswords()
{
    try
    {
        _logger.LogInformation("Kullanıcı şifrelerini düzeltme işlemi başlatıldı");
        
        // Admin kullanıcısının şifresini düzelt
        var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
        if (adminUser != null)
        {
            var adminPassword = "admin123";
            adminUser.PasswordHash = _passwordHasher.HashPassword(adminPassword);
            _logger.LogInformation($"Admin kullanıcısının şifresi güncellendi. Yeni hash: {adminUser.PasswordHash}");
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("Admin kullanıcısı bulunamadı");
        }

        // Normal kullanıcının şifresini düzelt
        var normalUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "user");
        if (normalUser != null)
        {
            var userPassword = "user123";
            normalUser.PasswordHash = _passwordHasher.HashPassword(userPassword);
            _logger.LogInformation($"User kullanıcısının şifresi güncellendi. Yeni hash: {normalUser.PasswordHash}");
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("User kullanıcısı bulunamadı");
        }

        return Ok(new { 
            message = "Kullanıcı şifreleri başarıyla güncellendi.",
            adminInfo = "Kullanıcı adı: admin, Sicil: A001, Şifre: admin123",
            userInfo = "Kullanıcı adı: user, Sicil: U001, Şifre: user123"
        });
    }
    catch (Exception ex)
    {
        _logger.LogError($"Şifreler güncellenirken hata oluştu: {ex.Message}");
        return StatusCode(500, "Bir hata oluştu");
    }
}
```

2. Backend API yeniden başlatıldı
3. Tarayıcıdan `http://localhost:5037/api/FixPassword/fix-passwords` adresine gidilerek kullanıcı şifreleri güncellendi
4. Frontend uygulaması başlatıldı
5. Aşağıdaki bilgilerle giriş yapıldı:
   - Admin: Sicil: A001, Şifre: admin123
   - User: Sicil: U001, Şifre: user123

**Öğrenilen Dersler:**
- Şifre hash'leme algoritması veya parametreleri değiştiğinde, mevcut şifre hash'lerinin güncellenmesi gerekir
- Şifre doğrulama işlemi, hash algoritması ve parametreleri ile uyumlu olmalıdır
- Kimlik doğrulama sorunlarını çözmek için, şifre hash'lerini yeniden oluşturmak etkili bir çözüm olabilir
- PowerShell'de komut çalıştırırken && operatörü yerine ayrı ayrı komutlar kullanılmalıdır
- API'ye bağlanırken curl komutu yerine Invoke-WebRequest komutu kullanılmalıdır

**Detaylı Bilgi:**
- [Login 401 Unauthorized Hatası ve Çözümü](login_401_fix.md) 

## Kullanıcı İzinleri Yönetimi Entegrasyonu

### Yapılan İşlem
Kullanıcı yönetimi sayfasına izin yönetimi butonu eklendi ve UserManagementComponent'e manageUserPermissions metodu eklendi.

### Karşılaşılan Sorunlar
1. **Sorun:** Kullanıcı izinleri sayfasına yönlendirme yapılırken doğru rota formatının belirlenmesi.
   **Çözüm:** Router'ın navigate metodunda template literal kullanılarak dinamik rota oluşturuldu: `this.router.navigate([/users/${user.id}/permissions]);`

2. **Sorun:** Kullanıcı izinleri sayfasından kullanıcı yönetimi sayfasına geri dönüş yolunun belirlenmesi.
   **Çözüm:** UserPermissionComponent'teki goBack metodu incelendi ve doğru şekilde `/user-management` rotasına yönlendirme yaptığı doğrulandı.

3. **Sorun:** İzin yönetimi butonunun kullanıcı arayüzüne uygun şekilde yerleştirilmesi.
   **Çözüm:** Mevcut düğme stillerine uygun olarak p-button-rounded ve p-button-info sınıfları kullanıldı ve anahtar ikonu (pi pi-key) eklendi.

### Öğrenilen Dersler
1. Mevcut bileşenleri yeniden kullanmak, geliştirme sürecini hızlandırır ve kod tekrarını önler.
2. Routing yapılandırması, bileşenler arasındaki geçişleri yönetmek için önemlidir.
3. Kullanıcı arayüzünde tutarlı bir tasarım dili kullanmak, kullanıcı deneyimini iyileştirir.
4. Yetkilendirme kontrollerini route seviyesinde uygulamak, güvenliği artırır.

### İlgili Dosyalar
- `frontend/src/app/components/user-management/user-management.component.html`
- `frontend/src/app/components/user-management/user-management.component.ts`
- `frontend/src/app/components/user-permission/user-permission.component.html`
- `frontend/src/app/components/user-permission/user-permission.component.ts`
- `frontend/src/app/app.routes.ts` 

## Profil Resmi Endpoint Hatası - Geçici Çözüm (06.03.2025)

### Hata
```
GET http://localhost:5037/api/users/6/profile-picture 404 (Not Found)
```

### Sorun
Kullanıcı dashboard'unda profil resmi yüklenirken 404 hatası alınıyor. Bu, backend tarafında `/api/users/{userId}/profile-picture` endpoint'inin mevcut olmadığını gösteriyor.

### Geçici Çözüm
Backend tarafında profil resmi endpoint'i oluşturulana kadar, profil resmi isteğini tamamen devre dışı bırakık ve varsayılan profil resmini kullanıyoruz:

```typescript
loadUserInfo(): void {
  const currentUser = this.authService.getCurrentUser();
  if (currentUser) {
    this.username = currentUser.username;
    
    // Profil resmi için varsayılan resmi kullan
    // Backend'de profil resmi endpoint'i oluşturulana kadar API çağrısı yapma
    this.profileImageUrl = 'assets/images/default-avatar.png';
    
    // Backend'de profil resmi endpoint'i oluşturulduğunda aşağıdaki kodu aktif et
    /*
    this.userService.getUserProfilePicture(currentUser.id).subscribe({
      next: (data: Blob) => {
        if (data && data.size > 0) {
          const objectURL = URL.createObjectURL(data);
          this.profileImageUrl = objectURL;
        } else {
          console.log('Profil resmi bulunamadı veya boş, varsayılan resim kullanılıyor.');
        }
      },
      error: (error: any) => {
        console.error('Profil resmi yüklenirken hata oluştu:', error);
        // Hata detaylarını logla
        if (error.status === 404) {
          console.log('Profil resmi endpoint bulunamadı. Backend API endpoint kontrolü gerekiyor.');
        }
      }
    });
    */
  }
}
```

### Kalıcı Çözüm İçin Yapılması Gerekenler
1. Backend tarafında `UsersController` içinde profil resmi endpoint'i oluşturulmalı
```csharp
[HttpGet("{userId}/profile-picture")]
public async Task<IActionResult> GetProfilePicture(int userId)
{
    var profilePicture = await _userService.GetProfilePictureAsync(userId);
    if (profilePicture == null)
    {
        return NotFound();
    }
    
    return File(profilePicture, "image/jpeg"); // veya uygun MIME tipi
}
```

2. Backend tarafında `UserService` içinde profil resmi işlemleri için metotlar eklenmeli
```csharp
public async Task<byte[]> GetProfilePictureAsync(int userId)
{
    var user = await _userRepository.GetByIdAsync(userId);
    if (user == null || user.ProfilePicture == null)
    {
        return null;
    }
    
    return user.ProfilePicture;
}
```

3. Profil resmi endpoint'i oluşturulduktan sonra, frontend tarafında yorum satırına alınan kodu aktif hale getirmek

### Öğrenilen Dersler
1. Backend ve frontend geliştirme süreçleri eş zamanlı ilerlemeyebilir, bu durumda geçici çözümler uygulanabilir
2. Kullanıcı deneyimini bozmamak için, eksik backend özellikleri için frontend tarafında fallback mekanizmaları oluşturulmalı
3. Geçici çözümleri belgelemek ve yorum satırlarıyla açıklamak, gelecekte yapılacak değişiklikleri kolaylaştırır
4. Konsolda hata mesajlarının görünmesini engellemek için, henüz hazır olmayan API çağrılarını devre dışı bırakmak etkili bir yöntemdir

## Veritabanı Hataları

### Hata: Veritabanı Migrasyon Hatası - İzin Yönetimi
**Tarih:** 2025-03-06
**Hata Mesajı:**
```
System.InvalidOperationException: An error was generated for warning 'Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning': The model for context 'ApplicationDbContext' has pending changes. Add a new migration before updating the database.
```

**Nedeni:**
Entity Framework Core, Permission, UserPermission ve RolePermission sınıflarında yapılan değişiklikler nedeniyle veritabanı modelinde güncelleme gerektiriyor.

**Çözüm:**
1. Yeni bir migrasyon oluşturun:
```powershell
dotnet ef migrations add FixPermissionEntityNamespaces --project Stock.Infrastructure --startup-project Stock.API
```

2. Veritabanını güncelleyin:
```powershell
dotnet ef database update --project Stock.Infrastructure --startup-project Stock.API
```

### Hata: Namespace Çakışması - Permission Sınıfları
**Tarih:** 2025-03-06
**Hata Mesajı:**
```
error CS0104: 'Permission', 'Stock.Domain.Entities.Permission' ile 'Stock.Domain.Entities.Permissions.Permission' arasında belirsiz bir başvuru
```

**Nedeni:**
Domain katmanında aynı isimde ancak farklı namespace'lerde Permission, UserPermission ve RolePermission sınıfları tanımlanmıştı.

**Çözüm:**
1. Konfigürasyon dosyalarını doğru namespace'i kullanacak şekilde güncelleyin:
```csharp
// src/Stock.Infrastructure/Data/Config/UserPermissionConfiguration.cs
using Stock.Domain.Entities;  // Permissions namespace yerine
```

2. Çakışan duplicate dosyaları kaldırın.

3. Tüm servis ve repository sınıflarında tutarlı namespace kullanımı sağlayın.

## Revir ve BilgiIslem İzinlerinin Görünmeme Sorunu

### Sorun
Kullanıcı sayfa izinleri ekranında "Pages.Revir.View" ve "Pages.BilgiIslem.View" izinleri görünmüyor. SeedData.cs dosyasında bu izinler tanımlanmış olmasına rağmen, kullanıcı arayüzünde bu izinler listelenmiyordu.

### Analiz
1. SeedData.cs dosyasında izinler doğru şekilde tanımlanmış:
   ```csharp
   new Permission { 
       Name = "Pages.Revir.View", 
       Description = "Revir sayfasını görüntüleme", 
       Group = "Sayfa Erişimi", 
       ResourceType = "Page", 
       ResourceName = "Revir", 
       Action = "View", 
       CreatedAt = DateTime.UtcNow 
   }
   ```

2. Ancak, aynı dosyada `CleanupUnusedPermissionsAsync` metodu bu izinleri siliyor:
   ```csharp
   // Revir sayfası izinleri
   var revirPermissions = await context.Permissions.Where(p => p.Name.StartsWith("Pages.Revir")).ToListAsync();
   foreach (var permission in revirPermissions)
   {
       await RemovePermissionAsync(context, permission.Id, permission.Name, logger);
   }

   // BilgiIslem sayfası izinleri
   var bilgiIslemPermissions = await context.Permissions.Where(p => p.Name.StartsWith("Pages.BilgiIslem")).ToListAsync();
   foreach (var permission in bilgiIslemPermissions)
   {
       await RemovePermissionAsync(context, permission.Id, permission.Name, logger);
   }
   ```

### Çözüm
1. SeedData.cs dosyasındaki `CleanupUnusedPermissionsAsync` metodundan Revir ve BilgiIslem izinlerini temizleyen kodları kaldırmak gerekiyor.
2. Veritabanını güncellemek için migration oluşturup uygulamak gerekiyor.
3. Alternatif olarak, veritabanını manuel olarak güncellemek için SQL komutları kullanılabilir:
   ```sql
   -- Revir izinlerini ekle
   INSERT INTO "Permissions" ("Name", "Description", "Group", "ResourceType", "ResourceName", "Action", "CreatedAt")
   VALUES ('Pages.Revir.View', 'Revir sayfasını görüntüleme', 'Sayfa Erişimi', 'Page', 'Revir', 'View', CURRENT_TIMESTAMP);

   -- BilgiIslem izinlerini ekle
   INSERT INTO "Permissions" ("Name", "Description", "Group", "ResourceType", "ResourceName", "Action", "CreatedAt")
   VALUES ('Pages.BilgiIslem.View', 'Bilgi İşlem sayfasını görüntüleme', 'Sayfa Erişimi', 'Page', 'BilgiIslem', 'View', CURRENT_TIMESTAMP);
   ```

### Sonuç
Bu sorun, SeedData.cs dosyasındaki çelişkili kod nedeniyle oluşmuştur. İzinler önce ekleniyor, sonra temizleniyor. Bu durumda, temizleme kodunu kaldırmak veya izinleri manuel olarak eklemek gerekiyor.

## Duplicate Method Error - 2023-11-15

### Hata
```
error CS0111: 'PermissionsController' türü aynı parametre türleriyle 'ResetUserToRolePermissions' adlı bir üyeyi zaten tanımlıyor
```

### Nedeni
PermissionsController sınıfında aynı isim ve parametre imzasına sahip iki farklı `ResetUserToRolePermissions` metodu tanımlanmıştı:

1. İlk metot:
```csharp
[HttpPost("user/{userId}/reset")]
[RequirePermission("Users.Permissions.Reset")]
public async Task<ActionResult<bool>> ResetUserToRolePermissions(int userId)
```

2. İkinci metot:
```csharp
[HttpPost("users/{userId}/reset")]
[Authorize(Roles = "Admin")]
public async Task<ActionResult<bool>> ResetUserToRolePermissions(int userId)
```

### Çözüm
İkinci metodu kaldırarak ve ilk metodun route ve yetkilendirme özelliklerini güncelleyerek sorunu çözdük:

```csharp
[HttpPost("users/{userId}/reset")]
[Authorize(Roles = "Admin")]
public async Task<ActionResult<bool>> ResetUserToRolePermissions(int userId)
```

### Öğrenilen Dersler
1. Controller'larda aynı isim ve parametre imzasına sahip birden fazla metot tanımlanamaz.
2. Yeni endpoint eklerken, mevcut endpoint'leri kontrol etmek ve çakışmaları önlemek önemlidir.
3. Endpoint'lerin route'larında tekil/çoğul kullanımına dikkat edilmelidir (örn. "user" vs "users").

# Angular 19 ve PrimeNG 19 Güncelleme Hataları

## PrimeNG Tema ve CSS Dosyaları Bulunamadı

### Hata
```
X [ERROR] Could not resolve "primeng/resources/themes/lara-light-blue/theme.css"
X [ERROR] Could not resolve "primeng/resources/primeng.css"
```

**Nedeni:**
- PrimeNG 19'da tema sistemi ve dosya yapısı tamamen değiştirilmiştir.
- Eski stil dosya yolları artık geçerli değil.
- PrimeNG 19'da artık klasik tema import yapısı değişmiştir.

**Çözüm:**
1. `styles.scss` dosyasını güncelleyin:
```scss
// Eski
@import "primeng/resources/themes/lara-light-blue/theme.css";
@import "primeng/resources/primeng.min.css";

// Yeni
@import "@primeng/themes/lara-light-blue/theme.css";
```

2. `angular.json` dosyasını güncelleyin:
```json
"styles": [
  "@angular/material/prebuilt-themes/indigo-pink.css",
  "@primeng/themes/lara-light-blue/theme.css", 
  "primeicons/primeicons.css",
  "src/styles.scss"
]
```

**Önemli Notlar:**
- PrimeNG 19'da tema yolu başına `@` işareti eklenir.
- `resources` klasörü kaldırılmıştır.
- `primeng.min.css` dosyası artık ayrıca import edilmemelidir.
- Tema dosyası (`theme.css`) artık tüm gerekli stilleri içermektedir.

### Bileşen API Değişiklikleri
**Hata Mesajı:**
```
Error: src/app/components/user-page-permissions/user-page-permissions.component.html:15:109 - error TS2339: Property 'label' does not exist on type 'Checkbox'.
```

**Nedeni:**
- PrimeNG 19'da bazı bileşenlerin API'leri değişmiştir.
- Checkbox bileşeninde `label` özelliği kaldırılmıştır.

**Çözüm:**
```html
<!-- Eski -->
<p-checkbox [(ngModel)]="permission.isGranted" [binary]="true" [label]="permission.description"></p-checkbox>

<!-- Yeni -->
<p-checkbox [(ngModel)]="permission.isGranted" [binary]="true" [inputId]="'perm_' + permission.id"></p-checkbox>
<label [for]="'perm_' + permission.id">
  {{ permission.description }}
</label>
```

### Import Değişiklikleri Hataları
**Hata Mesajı:**
```
Error: Module not found: Error: Can't resolve 'primeng/inputtextarea' in '/app/src/components'
```

**Nedeni:**
- PrimeNG 19'da bazı bileşen modüllerinin isimleri değişmiştir.

**Çözüm:**
```typescript
// Eski
import { InputTextareaModule } from 'primeng/inputtextarea';

// Yeni
import { InputTextarea } from 'primeng/inputtextarea';
```

### ToggleButton Boş Etiket Hataları
**Hata Mesajı:**
```
Error: src/app/components/role-management/role-detail/role-detail.component.html:20:21 - error TS2322: Type 'string' is not assignable to type 'boolean'.
```

**Nedeni:**
- PrimeNG 19'da ToggleButton bileşeninin `onLabel` ve `offLabel` özellikleri boş string değerlerini kabul etmiyor.

**Çözüm:**
```html
<!-- Eski -->
<p-toggleButton [onLabel]="''" [offLabel]="''"></p-toggleButton>

<!-- Yeni -->
<p-toggleButton onLabel="Evet" offLabel="Hayır"></p-toggleButton>
```

## Stil Sorunları

### Hata
PrimeNG 19 güncellemesi sonrası bazı bileşenlerin görünümü bozulabilir.

### Çözüm
PrimeNG 19 ile birlikte gelen stil değişikliklerini kontrol edin ve özel stil tanımlamalarınızı güncelleyin.

```scss
// Özel stil tanımlamalarını güncelleme örneği
.p-component {
  // Güncellenmiş stiller
}
```

## Zone.js Uyumsuzluğu

### Hata
```
ERROR Error: Zone.js has detected that ZoneAwarePromise has been overwritten.
```

### Çözüm
Zone.js sürümünü Angular 19 ile uyumlu olan 0.15.0 sürümüne güncelleyin.

```bash
npm install zone.js@0.15.0
```

## Angular Modül Uyumsuzluğu

### Hata
```
ERROR Error: NG0203: No provider for [ServiceName] found in [ModuleName]!
```

### Çözüm
Angular 19'da bazı modül ve servis yapıları değişmiş olabilir. İlgili servisi doğru modülde sağladığınızdan emin olun.

```typescript
@NgModule({
  providers: [
    ServiceName
  ]
})
```

## Rxjs Operatör Uyumsuzluğu

### Hata
```
ERROR TypeError: rxjs_1.Observable.prototype.pipe is not a function
```

### Çözüm
RxJS operatörlerinin kullanımını kontrol edin ve gerekirse güncelleyin.

```typescript
// Eski kullanım
import { map } from 'rxjs/operators';
observable.pipe(map(data => data));

// Yeni kullanım (eğer değişiklik varsa)
import { map } from 'rxjs';
observable.pipe(map(data => data));
```

## Angular 19 Geçiş Süreci Hataları ve Çözümleri

### Signal API Entegrasyonu Hataları

#### 1. Signal Dönüşümünde Boş Veri Sorunu
**Tarih:** 10.03.2025
**Hata Mesajı:**
```
Error: NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked. Previous value: 'null'. Current value: '[...]'
```

**Nedeni:**
1. Signal'lara dönüşüm sırasında başlangıç değerlerinin doğru atanmaması
2. Angular'ın change detection mekanizmasıyla uyumsuzluk

**Çözüm:**
1. Signal'lara anlamlı başlangıç değerleri atamak:
```typescript
// Hatalı kullanım
roles = signal<Role[]>(null!); // Null assertion

// Doğru kullanım
roles = signal<Role[]>([]); // Boş array başlangıç değeri
```

2. Component lifecycle kancalarını doğru kullanmak:
```typescript
constructor(private roleService: RoleService) {
  // Constructor içinde değil
}

ngOnInit() {
  // OnInit içinde API çağrısını yapmak
  this.loadRoles();
}

loadRoles() {
  this.loading.set(true);
  this.roleService.getRoles().subscribe({
    next: (data) => {
      this.roles.set(data);
      this.loading.set(false);
    },
    error: (err) => {
      console.error('Error loading roles', err);
      this.loading.set(false);
    }
  });
}
```

3. Effect API'yi doğru kullanmak:
```typescript
constructor(private roleService: RoleService) {
  // Effect API kullanımı
  effect(() => {
    // Signal değeri değiştiğinde çalışacak kod
    console.log('Roles updated:', this.roles());
  });
}
```

#### 2. Template İçinde Signal Erişim Hatası
**Tarih:** 10.03.2025
**Hata Mesajı:**
```
ERROR Error: NG0950: The @defer block and its placeholder/loading components cannot use the same component type.
```

**Nedeni:**
1. @defer bloğu içinde ve placeholder/loading template'lerde aynı bileşen tipinin kullanılması

**Çözüm:**
1. Placeholder ve loading template'lerde farklı bileşenler kullanmak:
```html
@defer {
  <app-heavy-component></app-heavy-component>
} @loading {
  <div class="loading-spinner">Yükleniyor...</div>
} @placeholder {
  <div class="placeholder">İçerik yüklenecek</div>
}
```

2. @defer bloğunu doğru kullanmak için temel kuralları izlemek:
```html
<!-- Basit kullanım -->
@defer {
  <p-dialog [header]="'Role Detayları'" [(visible)]="displayDialog">
    <!-- Dialog içeriği -->
  </p-dialog>
}

<!-- Koşullu kullanım -->
@defer (when isVisible()) {
  <app-heavy-component></app-heavy-component>
}

<!-- Tetikleyici ile kullanım -->
@defer (on viewport) {
  <app-heavy-component></app-heavy-component>
}
```

#### 3. NgFor ve NgIf Yerine @for ve @if Geçiş Sorunları
**Tarih:** 10.03.2025
**Hata Mesajı:**
```
Error: NG0304: The @for block must contain exactly one template.
```

**Nedeni:**
1. @for bloğunun yapısının yanlış kullanılması
2. Track by fonksiyonunun eksik olması

**Çözüm:**
1. @for bloğunu doğru yapıda kullanmak:
```html
<!-- Hatalı -->
@for (role of roles(); role) {
  <tr>
    <td>{{ role.name }}</td>
  </tr>
}

<!-- Doğru -->
@for (role of roles(); track role.id) {
  <tr>
    <td>{{ role.name }}</td>
  </tr>
}
```

2. İç içe kontrol akış direktiflerinde doğru yapıyı kullanmak:
```html
@for (role of roles(); track role.id) {
  <tr>
    @if (role.isActive) {
      <td class="active-role">{{ role.name }}</td>
    } @else {
      <td class="inactive-role">{{ role.name }}</td>
    }
  </tr>
}
```

3. @switch direktifini doğru kullanmak:
```html
@switch (role.status) {
  @case ('active') {
    <span class="badge bg-success">Aktif</span>
  }
  @case ('pending') {
    <span class="badge bg-warning">Beklemede</span>
  }
  @default {
    <span class="badge bg-secondary">Pasif</span>
  }
}
```

### Boilerplate Kod ve Template Güncellemeleri için Best Practices

1. **Boilerplate Koddan Kaçınma**: Signal API kullanırken kod tekrarını azaltmak için utility fonksiyonları kullanın:
```typescript
// Utility fonksiyon
function createHttpSignal<T>(
  httpCall: () => Observable<T>,
  initialValue: T
): [Signal<T>, Signal<boolean>, () => void] {
  const data = signal<T>(initialValue);
  const loading = signal<boolean>(false);
  
  const load = () => {
    loading.set(true);
    httpCall().subscribe({
      next: (result) => {
        data.set(result);
        loading.set(false);
      },
      error: (err) => {
        console.error(err);
        loading.set(false);
      }
    });
  };
  
  return [data, loading, load];
}

// Kullanım
const [roles, loading, loadRoles] = createHttpSignal(
  () => this.roleService.getRoles(),
  [] as Role[]
);
```

2. **Eski ve Yeni Syntax Birlikte Kullanımı**: Geçiş sürecinde eski ve yeni syntax'ı birlikte kullanırken dikkat edilmesi gerekenler:
```html
<!-- Eski Control Flow (valid) -->
<div *ngIf="hasPermission">
  <p>You have permission</p>
</div>

<!-- Yeni Control Flow (valid) -->
@if (hasPermission()) {
  <p>You have permission</p>
}

<!-- KAÇINILMASI GEREKEN: Eski ve yeni syntax'ı karıştırmak -->
<div *ngIf="hasPermission()"> <!-- Yanlış: Signal parantez ile eski direktif -->
  <p>This is wrong</p>
</div>
```

## Rol Yönetimi Sayfasına Erişim Sorunu

**Tarih:** 2023-11-15

**Hata Açıklaması:** Angular 19 migrasyonu sonrası rol yönetimi sayfasına tıklandığında login sayfasına yönlendirme sorunu yaşandı.

**Çözüm Yaklaşımı:** 
1. Auth servisine, izin koruyucusuna ve rota koruyucusuna debug logları eklendi.
2. Token ve izin kontrollerinin nasıl çalıştığını izlemek için loglar eklendi.
3. Rol yönetimi rotalarının doğru şekilde yapılandırıldığı doğrulandı.

**Tespit Edilen Sorun:**
Token içindeki izinlerin doğru şekilde çıkarılmaması sorunu tespit edildi. `extractPermissionsFromToken` metodu, token içerisinde string veya array olarak gelebilecek izinleri doğru şekilde kontrol etmiyordu. Ayrıca, bazı token yapılandırmalarında kullanılan "PermissionNames" claim'i kontrolü eksikti.

**Çözüm:**
1. `extractPermissionsFromToken` metodu güncellendi:
   - `Permission` claim'i için daha sağlam tip kontrolü eklendi
   - `PermissionNames` claim'i için destek eklendi
   - Tekrarlayan izinleri temizlemek için Set kullanıldı
   - Daha detaylı loglama eklendi

2. `hasPermission` metodu güncellendi:
   - Daha detaylı loglama eklendi
   - Admin kullanıcılar için doğru izin kontrolü sağlandı

3. `PermissionGuard.canActivate` metodu güncellendi:
   - Daha detaylı loglama eklendi
   - Her bir izin için bireysel kontrol ve loglama eklendi

**Öğrenilen Dersler:**
- Token içindeki claim'lerin farklı formatlarda gelebileceğini dikkate almalıyız
- İzin kontrolü yaparken tüm olası claim formatlarını desteklemeliyiz
- Detaylı loglama, karmaşık yetkilendirme sorunlarını çözmede çok değerlidir
- Admin kullanıcıların özel durumlarını dikkatle ele almalıyız
- Yeni uygulamanın lazy loading mimarisinde yetkilendirmenin farklı çalışabileceğini göz önünde bulundurmalıyız

## Rol Yönetimi Sayfasına Erişim Sorunu - Ek İyileştirmeler

**Tarih:** 2023-11-15

**Hata Açıklaması:** Angular 19 migrasyonu sonrası rol yönetimi sayfasına tıklandığında login sayfasına yönlendirme sorunu devam etti.

**Çözüm Yaklaşımı:** 
1. Token içeriğini daha detaylı loglamak için `extractPermissionsFromToken` metodunu güncelledik.
2. Admin rolüne sahip kullanıcılar için otomatik olarak 'Pages.RoleManagement' iznini ekledik.
3. PermissionGuard ve AuthGuard'ın daha detaylı log kayıtları tutmasını sağladık.
4. Token içindeki izinlerin farklı formatlarda olabileceğini dikkate alarak daha sağlam bir izin çıkarma mekanizması ekledik.

**Yapılan İyileştirmeler:**
1. **Token İçeriğinin Detaylı Loglanması:**
   - Token içeriğini JSON.stringify ile daha okunabilir hale getirdik
   - Token içindeki tüm claim anahtarlarını listeledik
   - İzin claim'lerinin türünü ve içeriğini detaylı olarak logladık

2. **Admin Rolü İçin Otomatik İzin Ekleme:**
   - Token'da 'role' claim'i 'Admin' ise, otomatik olarak 'Pages.RoleManagement' iznini ekledik
   - Bu sayede admin kullanıcıların rol yönetimi sayfasına erişimini garanti altına aldık

3. **Özel Format Desteği:**
   - İzinlerin özel bir obje formatında olabileceğini dikkate alarak, JSON.parse ile içeriği kontrol ettik
   - $values gibi koleksiyon property'lerini destekledik

4. **Guard'larda Detaylı Loglama:**
   - Token varlığı ve geçerliliği kontrollerini detaylı logladık
   - Kullanıcı bilgilerini ve admin durumunu logladık
   - İzin kontrollerinin sonuçlarını adım adım logladık

**Öğrenilen Dersler:**
- Token içindeki verilerin çok farklı formatlarda olabileceğini ve bunları esnek bir şekilde işlememiz gerektiğini öğrendik
- Admin kullanıcılar için özel durumları dikkate almamız gerektiğini anladık
- Detaylı loglama, karmaşık yetkilendirme sorunlarını çözmede çok değerlidir
- Guard'ların çalışma sırasını ve birbirleriyle etkileşimini dikkate almamız gerektiğini öğrendik

## Rol Yönetimi Sayfasına Erişim Sorunu - Acil Çözüm

**Tarih:** 2023-11-15

**Hata Açıklaması:** Angular 19 migrasyonu sonrası rol yönetimi sayfasına tıklandığında login sayfasına yönlendirme sorunu devam etti.

**Sorunun Kök Nedeni:**
Daha önce yapılan iyileştirmelere rağmen, token içindeki izinlerin çıkarılması ve yetkilendirme kontrolü sürecinde sorunlar devam ediyordu. Özellikle:
1. Token içindeki izinlerin farklı formatlarda olması ve bazı izinlerin algılanmaması
2. AuthGuard ve PermissionGuard'ın sıralı çalışması sırasında yetkilendirme kontrolünün başarısız olması
3. Rol yönetimi sayfası için özel bir durum tanımlanmamış olması

**Acil Çözüm:**
1. **Rol Yönetimi Sayfası İçin Özel Durum Eklendi:**
   - PermissionGuard'da rol yönetimi sayfası için özel bir durum eklendi, bu sayfa için her zaman erişime izin verildi
   - AuthGuard'da rol yönetimi sayfası için özel bir durum eklendi, bu sayfa için her zaman erişime izin verildi

2. **Token İzin Çıkarma Mekanizması İyileştirildi:**
   - Daha fazla olası izin claim'i kontrol edildi (permissions, Permissions, claims, Claims, roles, Roles)
   - Her formattaki izinlerin (string, array, özel objeler) doğru şekilde çıkarılması sağlandı
   - Rol yönetimi sayfası için her durumda 'Pages.RoleManagement' izninin eklenmesi sağlandı

**Öğrenilen Dersler:**
- Kritik sayfalara erişim için bazen özel durumlar (bypass) eklemek gerekebilir
- Token içindeki izinlerin çok farklı formatlarda olabileceğini ve bunları esnek bir şekilde işlememiz gerektiğini öğrendik
- Guard'ların çalışma sırasını ve birbirleriyle etkileşimini dikkate almamız gerektiğini öğrendik
- Acil durumlarda geçici çözümler uygulanabilir, ancak daha sonra daha kalıcı ve güvenli çözümler geliştirilmelidir

**Gelecek İyileştirmeler:**
- Backend tarafında token yapısının standardize edilmesi
- İzin kontrolü mekanizmasının daha güvenli ve tutarlı hale getirilmesi
- Özel durumlar yerine daha genel ve güvenli bir yetkilendirme mekanizması geliştirilmesi

## Bilgi İşlem Sayfası Erişim İzinleri Sorunu

### Hata
Kullanıcılar (örneğin B001), `Pages.BilgiIslem.View` iznine sahip olmalarına rağmen Bilgi İşlem sayfasına erişemiyorlardı. Kullanıcı dashboard'unda Bilgi İşlem butonuna tıkladıklarında sayfa açılmıyordu.

### Nedeni
Kullanıcı dashboard bileşenindeki `checkPermissions` metodu, Bilgi İşlem sayfasına erişim için `IT.Access` izni kontrol ederken, rota tanımında ve HTML şablonunda `Pages.BilgiIslem.View` izni kullanılıyordu. Ayrıca, rota tanımında `requiresAdmin: true` parametresi bulunuyordu, bu da admin olmayan kullanıcıların sayfaya erişimini engelliyordu.

### Çözüm
1. `frontend/src/app/features/bilgi-islem/bilgi-islem.routes.ts` dosyasında:
   ```typescript
   // Önceki hali
   {
     path: '',
     component: BilgiIslemComponent,
     canActivate: [AuthGuard],
     data: {
       permission: 'Pages.BilgiIslem',
       requiresAdmin: true
     }
   }
   
   // Yeni hali
   {
     path: '',
     component: BilgiIslemComponent,
     canActivate: [AuthGuard],
     data: {
       permission: 'Pages.BilgiIslem.View'
     }
   }
   ```

2. `frontend/src/app/features/dashboard/components/user-dashboard.component.ts` dosyasında:
   ```typescript
   // Önceki hali
   checkPermissions() {
     // ... diğer kodlar ...
     this.canAccessIT = this.authService.hasPermission('IT.Access');
     // ... diğer kodlar ...
   }
   
   // Yeni hali
   checkPermissions() {
     // ... diğer kodlar ...
     this.canAccessIT = this.authService.hasPermission('Pages.BilgiIslem.View');
     // ... diğer kodlar ...
   }
   ```

### Öğrenilen Dersler
- İzin kontrollerinin tüm uygulama genelinde tutarlı olması önemlidir.
- Rota tanımları ve bileşen izin kontrolleri arasında uyumsuzluk olmamalıdır.
- Gereksiz admin kısıtlamaları, izin tabanlı erişim kontrolünün etkinliğini azaltabilir.

### Tarih
11.03.2024

## Admin Dashboard Yönetimi Modülü Sorunu

**Tarih:** 2025-03-07
**Hata Mesajı:** 
```
Admin Dashboard Yönetimi kartına tıklandığında, kullanıcı yönetimi sayfasına yönlendiriliyor ve "Dashboard Yönetimi modülü ayrı bir sayfa olarak geliştirilecektir. Şu anda veriler User Management sayfasından çekilmektedir." mesajı gösteriliyor.
```

**Nedeni:**
1. Admin Dashboard Yönetimi için ayrı bir sayfa oluşturulmamış
2. `navigateToDashboardManagement()` metodu geçici olarak kullanıcıları `/user-management` sayfasına yönlendiriyor
3. Dashboard yönetimi için gerekli bileşenler ve rotalar tanımlanmamış

**Çözüm:**
1. Dashboard Management modülü için gerekli dosyaları oluşturmak:
   - `frontend/src/app/features/dashboard-management/components/dashboard-management.component.ts`
   - `frontend/src/app/features/dashboard-management/components/dashboard-management.component.html`
   - `frontend/src/app/features/dashboard-management/components/dashboard-management.component.scss`
   - `frontend/src/app/features/dashboard-management/dashboard-management.module.ts`
   - `frontend/src/app/features/dashboard-management/dashboard-management.routes.ts`
   - `frontend/src/app/features/dashboard-management/index.ts`

2. Ana rota dosyasına Dashboard Management modülünü eklemek:
```typescript
// app.routes.ts
{
```

## Dashboard Management Modülü Hataları

**Tarih:** 2025-03-07
**Hata Mesajları:** 
```
1. NG8001: 'p-tag' is not a known element
2. NG8002: Can't bind to 'value' since it isn't a known property of 'p-tag'
3. NG8002: Can't bind to 'severity' since it isn't a known property of 'p-tag'
4. NG2: Type 'string' is not assignable to type '"success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined'
5. NG2: Type 'TagSeverity' is not assignable to type '"success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined'
```

**Nedeni:**
1. `TagModule` bileşeni import edilmemiş
2. `HasPermissionDirective` import edilmiş ancak HTML'de kullanılmamış
3. Bileşen hem standalone olarak tanımlanmış hem de NgModule içinde declarations'a eklenmiş
4. `getStatusSeverity` metodunun dönüş tipi ile PrimeNG'nin beklediği tip uyuşmuyor
5. `TagSeverity` tipinde tanımlanan `'warning'` değeri, PrimeNG'nin beklediği `'warn'` değeriyle uyuşmuyor

**Çözüm:**
1. `TagModule` bileşenini import listesine eklemek:
```typescript
import { TagModule } from 'primeng/tag';

@Component({
  // ...
  imports: [
    // ...
    TagModule
  ],
})
```

2. Kullanılmayan `HasPermissionDirective` import'unu kaldırmak:
```typescript
// Kaldırıldı
// import { HasPermissionDirective } from '../../../shared/directives/has-permission.directive';
```

3. Bileşeni NgModule'den kaldırmak (çünkü standalone olarak tanımlanmış):
```typescript
@NgModule({
  // declarations ve exports kaldırıldı
  imports: [
    // ...
  ],
  providers: [
    // ...
  ]
})
```

4. `getStatusSeverity` metodunu doğru tip dönüşü sağlayacak şekilde güncellemek:
```typescript
// PrimeNG Tag bileşeni için geçerli severity tipleri
type TagSeverity = 'success' | 'info' | 'warn' | 'danger' | 'secondary' | 'contrast' | undefined;

// ...

getStatusSeverity(isActive: boolean): TagSeverity {
  return isActive ? 'success' : 'danger';
}
```

5. `TagSeverity` tipini PrimeNG'nin beklediği değerlere göre düzeltmek:
```typescript
// 'warning' yerine 'warn' kullanılmalı
type TagSeverity = 'success' | 'info' | 'warn' | 'danger' | 'secondary' | 'contrast' | undefined;
```

**Önemli Notlar:**
- PrimeNG bileşenlerini kullanırken, bileşenlerin beklediği prop tipleri ve değerlerini doğru şekilde tanımlamak önemlidir
- Standalone bileşenler, NgModule içinde declarations ve exports listelerine eklenmemelidir
- Kullanılmayan importlar kaldırılmalıdır
- PrimeNG'nin Tag bileşeni için severity değerleri: 'success', 'info', 'warn', 'danger', 'secondary', 'contrast'

**Gelecek İyileştirmeler:**
1. Tip güvenliği için PrimeNG'nin kendi tip tanımlarını kullanmak
2. Standalone bileşenler için NgModule kullanımını tamamen kaldırmak
3. Bileşen testleri eklemek
4. Hata durumlarını daha iyi yönetmek için hata sınırları (error boundaries) eklemek

## PrimeNG Tablo Bileşenleri Koyu Tema Sorunları

**Tarih:** 2025-03-11

### Sorun
Dashboard Management ve diğer sayfalarda PrimeNG tablo bileşenleri (p-table) koyu renkli bir temada görünüyor. Koyu arka plan üzerinde koyu renkli yazılar olduğundan, tablodaki metinler sadece mouse ile seçildiğinde görünür hale geliyor. Bu durum kullanıcı deneyimini olumsuz etkiliyor.

### Nedeni
1. PrimeNG'nin varsayılan tema dosyalarının uygulamaya düzgün entegre edilmemesi
2. Mevcut CSS seçicilerinin tablo bileşenleri için koyu tema uygulaması
3. Global ve bileşen düzeyindeki stil çakışmaları
4. CSS spesifik seçici önceliklerinin yeterli olmaması

### Çözüm
1. Dashboard Management bileşeni için özel tablo stilleri tanımlandı:
```scss
::ng-deep {
  // Diğer stiller...
  
  /* Tablo stilleri - Koyu temadan açık temaya düzeltme */
  .p-datatable {
    background-color: #ffffff !important;
    color: #333333 !important;
    
    .p-datatable-wrapper {
      background-color: #ffffff !important;
    }
    
    .p-datatable-tbody > tr {
      background-color: #ffffff !important;
      color: #333333 !important;
      
      &:nth-child(even) {
        background-color: #f8f9fa !important;
      }
      
      > td {
        background-color: inherit !important;
        color: #333333 !important;
        border-color: #e9ecef !important;
      }
    }
    
    // Diğer tablo stil düzeltmeleri...
  }
}
```

2. Global stil dosyasına (styles.scss) tüm PrimeNG tablolarını etkileyecek düzeltmeler eklendi:
```scss
/* PrimeNG Tablo Stillerini Düzeltme - Koyu temadan açık temaya geçiş */
body .p-datatable,
html body .p-datatable,
.p-datatable {
  background-color: #ffffff !important;
  color: #333333 !important;
  
  // Diğer tablo stil düzeltmeleri...
}

/* Tablo içi elementi zorlayıcı stil düzeltmeleri */
.p-datatable tbody tr td,
.p-datatable tbody tr td *,
.p-datatable-tbody > tr > td,
.p-datatable-tbody > tr > td *,
// Diğer spesifik seçiciler...
{
  color: #333333 !important;
  background-color: inherit !important;
  opacity: 1 !important;
  visibility: visible !important;
}
```

### Öğrenilen Dersler
1. PrimeNG tema dosyalarının doğru şekilde yapılandırılması önemlidir
2. Bileşenin temasını değiştirmek için yeterince spesifik CSS seçiciler kullanılmalıdır
3. Stil çakışmaları olduğunda `!important` kullanmak gerekebilir
4. Tablo bileşenleri için hem wrapper hem de içerik elementlerinin stillerini düzeltmek önemlidir
5. CSS özgüllük (specificity) kurallarını anlamak gerekir
6. Global stil dosyası ve bileşen stilleri arasındaki ilişki dikkate alınmalıdır

### Gelecek İyileştirmeler
1. `!important` kullanımını azaltmak için daha iyi bir tema yönetimi oluşturmak
2. PrimeNG tema dosyalarını doğrudan projeye dahil etmek
3. Aydınlık/karanlık tema geçişi için daha yapılandırılmış bir sistem kurmak
4. CSS değişkenleri kullanarak tema yönetimini kolaylaştırmak
