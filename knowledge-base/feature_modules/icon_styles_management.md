# İkon Stilleri Yönetimi

## Genel Bakış

Bu belge, projemizdeki ikon stillerinin nasıl yönetildiğini açıklamaktadır. Projemizde, farklı modüller için farklı ikon stilleri kullanılmaktadır. Bu stilller, modüllere özgü olarak tanımlanmış ve global stil dosyasına import edilmiştir.

## Stil Organizasyonu

Projemizde iki farklı ikon stil seti bulunmaktadır:

1. **Dashboard Modülü İkon Stilleri**: Tüm dashboard modüllerinde (user-management, role-management, dashboard-management vb.) ortak olarak kullanılan ikon stilleri.
2. **Bilgi İşlem Modülü İkon Stilleri**: Sadece bilgi işlem modülünde kullanılan özel ikon stilleri.

## Dosya Yapısı

```
frontend/
├── src/
│   ├── app/
│   │   ├── features/
│   │   │   ├── dashboard/
│   │   │   │   └── shared/
│   │   │   │       └── styles/
│   │   │   │           └── dashboard-icons.scss  # Dashboard modülü ikon stilleri
│   │   │   ├── bilgi-islem/
│   │   │   │   └── shared/
│   │   │   │       └── styles/
│   │   │   │           └── bilgi-islem-icons.scss  # Bilgi işlem modülü ikon stilleri
│   ├── styles.scss  # Global stil dosyası (ikon stillerini import eder)
```

## Stil İmport Mekanizması

Global stil dosyasında (`styles.scss`), modül-spesifik ikon stilleri import edilmiştir:

```scss
@import "./app/features/dashboard/shared/styles/dashboard-icons.scss";
@import "./app/features/bilgi-islem/shared/styles/bilgi-islem-icons.scss";
```

## Dashboard Modülü İkon Stilleri

Dashboard modülündeki tüm sayfalarda (user-management, role-management, dashboard-management vb.) aşağıdaki stil sınıfları kullanılmaktadır:

- `.dashboard-edit-icon`: Kalem (düzenle) ikonu için
- `.dashboard-delete-icon`: Çöp kutusu (sil) ikonu için
- `.dashboard-permission-icon`: Anahtar (izinler) ikonu için
- `.dashboard-add-icon`: Artı (ekle) ikonu için

Örnek kullanım:

```html
<button pButton pRipple icon="pi pi-pencil" class="p-button-rounded p-button-text p-button-info dashboard-edit-icon" 
        (click)="editUser(user)" pTooltip="Düzenle" tooltipPosition="top"></button>
```

## Bilgi İşlem Modülü İkon Stilleri

Bilgi işlem modülünde aşağıdaki stil sınıfları kullanılmaktadır:

- `.bilgi-islem-edit-icon`: Kalem (düzenle) ikonu için
- `.bilgi-islem-delete-icon`: Çöp kutusu (sil) ikonu için
- `.bilgi-islem-add-icon`: Artı (ekle) ikonu için

Örnek kullanım:

```html
<button pButton pRipple icon="pi pi-pencil" class="p-button-rounded p-button-text p-button-info bilgi-islem-edit-icon" 
        (click)="editItem(item)" pTooltip="Düzenle" tooltipPosition="top"></button>
```

## Stil Farklılıkları

### Dashboard Modülü İkonları

- Yuvarlak şekil (border-radius: 50%)
- Hafif gölge efekti
- Hover durumunda yukarı doğru hafif hareket
- Pastel renkler

### Bilgi İşlem Modülü İkonları

- Köşeli şekil (border-radius: 8px)
- Belirgin gradient arka plan
- Daha güçlü gölge efekti
- Hover durumunda daha belirgin yukarı hareket
- Canlı renkler ve beyaz ikon

## Yeni Bir Modül İçin İkon Stilleri Ekleme

Yeni bir modül için özel ikon stilleri eklemek istiyorsanız, aşağıdaki adımları izleyin:

1. Modül klasöründe bir `shared/styles` klasörü oluşturun:
   ```
   mkdir -p frontend/src/app/features/yeni-modul/shared/styles
   ```

2. Modüle özgü ikon stilleri için bir SCSS dosyası oluşturun:
   ```
   touch frontend/src/app/features/yeni-modul/shared/styles/yeni-modul-icons.scss
   ```

3. Stil dosyasını düzenleyin ve gerekli ikon stillerini tanımlayın.

4. Global stil dosyasına (`styles.scss`) yeni stil dosyasını import edin:
   ```scss
   @import "./app/features/yeni-modul/shared/styles/yeni-modul-icons.scss";
   ```

5. HTML dosyalarında yeni stil sınıflarını kullanın.

## Bakım ve Güncellemeler

İkon stilleri zaman içinde değişebilir veya yeni ikonlar eklenebilir. Bu durumda aşağıdaki adımları izleyin:

1. İlgili modülün stil dosyasını bulun (`dashboard-icons.scss` veya `bilgi-islem-icons.scss`)
2. Yeni ikon stillerini ekleyin veya mevcut stilleri güncelleyin
3. Değişiklikleri test edin
4. Değişiklikleri belgelendirin

### Yaygın Sorunlar ve Çözümleri

#### İkon İçinde Gereksiz Metin Görünmesi

**Sorun:** İkon butonlarının içinde gereksiz metin görünebilir. Örneğin, izinleri yönet ikonunun içinde "İzinler" yazısı görünmesi.

**Çözüm:** 
1. HTML'de buton içindeki gereksiz metin etiketlerini kaldırın:
```html
<!-- Hatalı Kullanım -->
<button pButton pRipple icon="pi pi-key" class="p-button-rounded p-button-text p-button-success dashboard-permission-icon" 
  (click)="manageUserPermissions(user)" pTooltip="İzinleri Yönet" tooltipPosition="top">
  <span class="permission-icon-text">İzinler</span>
</button>

<!-- Doğru Kullanım -->
<button pButton pRipple icon="pi pi-key" class="p-button-rounded p-button-text p-button-success dashboard-permission-icon" 
  (click)="manageUserPermissions(user)" pTooltip="İzinleri Yönet" tooltipPosition="top"></button>
```

2. CSS'de ilgili stil tanımlamalarını kaldırın:
```scss
// Kaldırılacak CSS
.permission-icon-text {
  display: none !important; /* İzinler yazısını tamamen gizle */
}
```

## Sonuç

Bu yaklaşım, modüller arasında görsel tutarlılığı korurken, her modülün kendi ihtiyaçlarına göre özelleştirilmiş ikonlara sahip olmasını sağlar. Dashboard modülü içindeki tüm sayfalar aynı ikon stillerini kullanırken, Bilgi İşlem modülü kendi özel stillerini kullanabilir. 