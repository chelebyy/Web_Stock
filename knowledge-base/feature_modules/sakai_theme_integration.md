# Sakai Teması Entegrasyonu

## Genel Bakış

Bu belge, PrimeNG'nin Sakai temasının admin dashboard'a entegrasyonunu açıklamaktadır. Sakai, PrimeNG tarafından sunulan ücretsiz bir Angular admin şablonudur ve modern, kullanıcı dostu bir arayüz sağlar.

## Entegrasyon Adımları

### 1. Sakai Temasının Yüklenmesi

Sakai teması, GitHub'dan klonlanarak projemize entegre edilmiştir:

```bash
git clone https://github.com/primefaces/sakai-ng.git sakai-temp
```

### 2. Tema Dosyalarının Kopyalanması

Sakai temasının stil dosyaları, projemizin assets klasörüne kopyalanmıştır:

```bash
mkdir -p frontend/src/assets/sakai-layout
cp -r sakai-temp/src/assets/layout/* frontend/src/assets/sakai-layout/
```

### 3. Ana Stil Dosyasının Oluşturulması

Sakai temasının tüm stil dosyalarını import eden bir ana stil dosyası oluşturulmuştur:

```scss
// frontend/src/assets/sakai-layout/sakai-layout.scss
@use './variables/variables' as *;
@use './mixins' as *;
@use './core';
@use './typography';
@use './main';
@use './topbar';
@use './menu';
@use './footer';
@use './responsive';
@use './utils';
@use './preloading';
```

### 4. Global Stil Dosyasına Entegrasyon

Sakai temasının stil dosyaları, global stil dosyasına import edilmiştir:

```scss
// frontend/src/styles.scss
@use "@angular/material/prebuilt-themes/indigo-pink.css";
@use "primeicons/primeicons.css";
@use "./app/features/dashboard/shared/styles/dashboard-icons.scss";
@use "./app/features/bilgi-islem/shared/styles/bilgi-islem-icons.scss";
@use "./assets/sakai-layout/sakai-layout.scss";

/* PrimeNG 19 Tema Ayarları */
// ...
```

### 5. Admin Dashboard Bileşeninin Güncellenmesi

Admin dashboard bileşeni, Sakai temasına uygun şekilde güncellenmiştir:

#### HTML Yapısı

```html
<div class="sakai-dashboard">
  <div class="dashboard-header">
    <!-- Header içeriği -->
  </div>
  
  <div class="dashboard-content">
    <div class="admin-modules">
      <!-- Admin modülleri -->
    </div>
    
    <div class="dashboard-sidebar">
      <!-- Sidebar içeriği -->
    </div>
  </div>
</div>
```

#### SCSS Stilleri

```scss
.sakai-dashboard {
  padding: 2rem;
  
  .dashboard-header {
    // Header stilleri
  }
  
  .dashboard-content {
    // İçerik stilleri
  }
  
  // Diğer stiller
}
```

#### TypeScript Güncellemeleri

```typescript
import { MenuItem } from 'primeng/api';
import { MenuModule } from 'primeng/menu';

// ...

export class AdminDashboardComponent implements OnInit {
  // ...
  
  userMenuItems: MenuItem[] = [];
  
  // ...
  
  ngOnInit(): void {
    // ...
    this.initUserMenuItems();
    // ...
  }
  
  initUserMenuItems(): void {
    this.userMenuItems = [
      {
        label: 'Profil',
        icon: 'pi pi-user',
        command: () => this.navigateToProfile()
      },
      // Diğer menü öğeleri
    ];
  }
  
  // ...
}
```

## Sakai Temasının Özellikleri

Sakai teması, aşağıdaki özellikleri sağlar:

1. **Modern ve Temiz Tasarım**: Kullanıcı dostu, modern bir arayüz.
2. **Responsive Tasarım**: Farklı ekran boyutlarına uyumlu.
3. **Tema Seçenekleri**: Açık ve koyu tema seçenekleri.
4. **Hazır Bileşenler**: Menü, sidebar, topbar gibi hazır bileşenler.
5. **PrimeNG Entegrasyonu**: PrimeNG bileşenleriyle tam uyumluluk.

## Kullanım Kılavuzu

### Sakai Temasını Diğer Sayfalara Entegre Etme

Sakai temasını diğer sayfalara entegre etmek için:

1. İlgili bileşenin HTML dosyasına Sakai temasının yapısını ekleyin.
2. Bileşenin SCSS dosyasına Sakai temasının stillerini ekleyin.
3. Bileşenin TypeScript dosyasına gerekli import'ları ve özellikleri ekleyin.

### Tema Özelleştirme

Sakai temasını özelleştirmek için:

1. `frontend/src/assets/sakai-layout/variables` klasöründeki değişkenleri güncelleyin.
2. Özel stiller eklemek için `frontend/src/assets/sakai-layout/sakai-layout.scss` dosyasını düzenleyin.

## Sonuç

Sakai teması, admin dashboard'a başarıyla entegre edilmiştir. Bu entegrasyon, kullanıcı deneyimini iyileştirmiş ve modern bir arayüz sağlamıştır. Tema, PrimeNG bileşenleriyle tam uyumlu çalışmakta ve responsive tasarım sunmaktadır.

## Kaynaklar

- [Sakai GitHub Reposu](https://github.com/primefaces/sakai-ng)
- [Sakai Dokümantasyonu](https://sakai.primeng.org/documentation)
- [PrimeNG Tema Dokümantasyonu](https://primeng.org/theming) 