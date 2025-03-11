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

İkon stillerinde yapılacak değişiklikler, ilgili modülün stil dosyasında yapılmalıdır. Bu şekilde, bir modüldeki değişiklikler diğer modülleri etkilemeyecektir.

Tüm modüllerde ortak olarak kullanılacak stil değişiklikleri için, her modülün stil dosyasını ayrı ayrı güncellemeniz gerekecektir.

## Sonuç

Bu yaklaşım, farklı modüller için farklı görsel kimlikler oluşturmamıza olanak tanırken, aynı zamanda her modül içinde tutarlı bir görünüm sağlar. Dashboard modülü içindeki tüm sayfalar benzer görünürken, bilgi işlem modülü kendi özgün görsel kimliğine sahip olabilir. 