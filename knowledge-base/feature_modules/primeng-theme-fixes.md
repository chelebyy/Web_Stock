# PrimeNG Tema Düzeltmeleri

## Genel Bakış

Bu belge, PrimeNG bileşenlerinin tema ve stil sorunlarını çözmek için yapılan değişiklikleri ve uygulamaları belgelemektedir. Özellikle Lara temasına geçiş ve siyah arka plan sorunlarının çözümü için yapılan düzenlemeleri içerir.

## Tema Değişikliği

### Aura'dan Lara'ya Geçiş

PrimeNG'nin varsayılan teması olan Aura, bazı bileşenlerde (özellikle dropdown ve dialog) siyah arka plan kullanmaktadır. Bu durum, metin okunabilirliğini ve kullanıcı deneyimini olumsuz etkilemektedir. Bu nedenle, daha modern ve açık renkli bir tema olan Lara'ya geçiş yapılmıştır.

**Değişiklik Yapılan Dosya:** `frontend/src/app/app.config.ts`

```typescript
// Eski
import Aura from '@primeng/themes/aura';

// Yeni
import Lara from '@primeng/themes/lara';

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    providePrimeNG({
      theme: {
        preset: Lara // Aura yerine Lara kullanılıyor
      }
    })
  ]
};
```

## Özel Stil Düzeltmeleri

Tema değişikliğine rağmen bazı bileşenlerde hala siyah arka plan sorunu yaşandığı için, global stil dosyasında özel düzeltmeler yapılmıştır.

### Dialog Bileşeni Düzeltmeleri

**Değişiklik Yapılan Dosya:** `frontend/src/styles.scss`

```scss
/* Dialog Düzeltmeleri */
.p-dialog {
  background-color: #ffffff !important;
  color: #333333 !important;
  z-index: 10000 !important;
}

.p-dialog .p-dialog-header,
.p-dialog .p-dialog-content,
.p-dialog .p-dialog-footer {
  background: #ffffff !important;
  color: #333333 !important;
  border-color: #e9ecef !important;
}
```

### TabView Bileşeni Düzeltmeleri

```scss
/* TabView Düzeltmeleri */
.p-tabview {
  background-color: #ffffff !important;
  color: #333333 !important;
}

.p-tabview .p-tabview-nav {
  background: #ffffff !important;
  border-color: #e9ecef !important;
}

.p-tabview .p-tabview-nav li .p-tabview-nav-link {
  background: #f8f9fa !important;
  color: #6c757d !important;
  border-color: #e9ecef !important;
}

.p-tabview .p-tabview-nav li.p-highlight .p-tabview-nav-link {
  background: #ffffff !important;
  color: #3B82F6 !important;
  border-color: #3B82F6 !important;
}

.p-tabview .p-tabview-panels {
  background: #ffffff !important;
  color: #333333 !important;
  border-color: #e9ecef !important;
}
```

### Özel Bileşen Stilleri

Belirli bileşenler için özel stil sınıfları oluşturulmuştur:

```scss
/* Dashboard Düzenleme Diyalogu Özel Stili */
.dashboard-edit-dialog {
  .p-dialog-header, 
  .p-dialog-content, 
  .p-dialog-footer {
    background-color: #ffffff !important;
    color: #333333 !important;
  }
  
  .p-tabview-panels,
  .p-tabview-nav,
  .p-tabview {
    background-color: #ffffff !important;
    color: #333333 !important;
  }
  
  input, 
  .p-dropdown, 
  .p-multiselect {
    background-color: #ffffff !important;
    color: #333333 !important;
  }
  
  label {
    color: #333333 !important;
    font-weight: 500 !important;
  }
  
  .p-checkbox-box {
    background-color: #ffffff !important;
    border-color: #ced4da !important;
  }
  
  .p-checkbox-box.p-highlight {
    background-color: #3B82F6 !important;
    border-color: #3B82F6 !important;
  }
}
```

## HTML Değişiklikleri

Dialog bileşenine özel stil sınıfı eklenmiştir:

**Değişiklik Yapılan Dosya:** `frontend/src/app/features/dashboard-management/components/dashboard-management.component.html`

```html
<p-dialog 
  [(visible)]="displayDialog" 
  [style]="{width: '650px'}" 
  [header]="dialogHeader" 
  [modal]="true" 
  styleClass="p-fluid dashboard-edit-dialog"
  [baseZIndex]="10000">
  <!-- ... -->
</p-dialog>
```

## Alternatif Çözümler

Eğer yukarıdaki düzeltmeler yeterli olmazsa, aşağıdaki alternatif çözümler denenebilir:

1. **Material Teması:** PrimeNG'nin Material teması da açık renkli bir temadır ve denenebilir.
2. **Özel Tema Oluşturma:** PrimeNG'nin `definePreset` fonksiyonu kullanılarak tamamen özel bir tema oluşturulabilir.
3. **CSS Değişkenleri:** PrimeNG'nin CSS değişkenleri doğrudan `:root` seçicisi içinde geçersiz kılınabilir.

## Ek Stil Düzeltmeleri (2025-03-10)

Lara temasına geçiş ve önceki düzeltmelere rağmen, bazı bileşenlerde hala siyah arka plan sorunu yaşandığı için, daha spesifik ve güçlü CSS seçiciler kullanılarak ek düzeltmeler yapılmıştır.

### Dialog Bileşeni İçin Daha Spesifik Seçiciler

```scss
/* Dialog Düzeltmeleri */
.p-dialog,
.p-dialog-mask .p-dialog,
.p-dialog-mask .p-component,
.dashboard-edit-dialog,
.p-dialog.dashboard-edit-dialog,
.p-dialog-mask .p-dialog.dashboard-edit-dialog {
  background-color: #ffffff !important;
  color: #333333 !important;
  z-index: 10000 !important;
}

.p-dialog .p-dialog-header,
.p-dialog .p-dialog-content,
.p-dialog .p-dialog-footer,
.p-dialog-mask .p-dialog .p-dialog-header,
.p-dialog-mask .p-dialog .p-dialog-content,
.p-dialog-mask .p-dialog .p-dialog-footer,
.dashboard-edit-dialog .p-dialog-header,
.dashboard-edit-dialog .p-dialog-content,
.dashboard-edit-dialog .p-dialog-footer {
  background: #ffffff !important;
  color: #333333 !important;
  border-color: #e9ecef !important;
}
```

### Dialog İçindeki Form Elemanları İçin Düzeltmeler

```scss
/* Dialog İçindeki Form Elemanları */
.p-dialog input,
.p-dialog .p-dropdown,
.p-dialog .p-multiselect,
.p-dialog .p-inputtext,
.p-dialog-mask .p-dialog input,
.p-dialog-mask .p-dialog .p-dropdown,
.p-dialog-mask .p-dialog .p-multiselect,
.p-dialog-mask .p-dialog .p-inputtext,
.dashboard-edit-dialog input,
.dashboard-edit-dialog .p-dropdown,
.dashboard-edit-dialog .p-multiselect,
.dashboard-edit-dialog .p-inputtext {
  background-color: #ffffff !important;
  color: #333333 !important;
  border: 1px solid #ced4da !important;
}

/* Dialog İçindeki Etiketler */
.p-dialog label,
.p-dialog-mask .p-dialog label,
.dashboard-edit-dialog label {
  color: #333333 !important;
  font-weight: 500 !important;
}

/* Dialog İçindeki Checkbox */
.p-dialog .p-checkbox-box,
.p-dialog-mask .p-dialog .p-checkbox-box,
.dashboard-edit-dialog .p-checkbox-box {
  background-color: #ffffff !important;
  border-color: #ced4da !important;
}

.p-dialog .p-checkbox-box.p-highlight,
.p-dialog-mask .p-dialog .p-checkbox-box.p-highlight,
.dashboard-edit-dialog .p-checkbox-box.p-highlight {
  background-color: #3B82F6 !important;
  border-color: #3B82F6 !important;
}
```

### Dialog İçindeki TabView Bileşeni İçin Düzeltmeler

```scss
/* Dialog İçindeki TabView */
.p-dialog .p-tabview,
.p-dialog-mask .p-dialog .p-tabview,
.dashboard-edit-dialog .p-tabview {
  background-color: #ffffff !important;
  color: #333333 !important;
  border: none !important;
}

.p-dialog .p-tabview .p-tabview-nav,
.p-dialog-mask .p-dialog .p-tabview .p-tabview-nav,
.dashboard-edit-dialog .p-tabview .p-tabview-nav {
  background: #ffffff !important;
  border-color: #e9ecef !important;
}

.p-dialog .p-tabview .p-tabview-nav li .p-tabview-nav-link,
.p-dialog-mask .p-dialog .p-tabview .p-tabview-nav li .p-tabview-nav-link,
.dashboard-edit-dialog .p-tabview .p-tabview-nav li .p-tabview-nav-link {
  background: #f8f9fa !important;
  color: #6c757d !important;
  border-color: #e9ecef !important;
}

.p-dialog .p-tabview .p-tabview-nav li.p-highlight .p-tabview-nav-link,
.p-dialog-mask .p-dialog .p-tabview .p-tabview-nav li.p-highlight .p-tabview-nav-link,
.dashboard-edit-dialog .p-tabview .p-tabview-nav li.p-highlight .p-tabview-nav-link {
  background: #ffffff !important;
  color: #3B82F6 !important;
  border-color: #3B82F6 !important;
}

.p-dialog .p-tabview .p-tabview-panels,
.p-dialog-mask .p-dialog .p-tabview .p-tabview-panels,
.dashboard-edit-dialog .p-tabview .p-tabview-panels {
  background: #ffffff !important;
  color: #333333 !important;
  border-color: #e9ecef !important;
  padding: 1.25rem !important;
}
```

## CSS Seçici Özgüllüğü (Specificity) Hakkında

PrimeNG bileşenlerinin stillerini düzeltirken, CSS seçici özgüllüğü (specificity) önemli bir rol oynar. Daha spesifik seçiciler, daha genel seçicileri geçersiz kılar. Bu nedenle, tema değişikliğine rağmen bazı bileşenlerin hala siyah arka plana sahip olmasının nedeni, PrimeNG'nin kendi stillerinin daha spesifik seçicilere sahip olması olabilir.

Bu sorunu çözmek için:

1. Daha spesifik seçiciler kullanın (örneğin, `.p-dialog-mask .p-dialog .p-tabview` gibi)
2. `!important` kullanarak stil önceliğini artırın
3. Özel sınıflar ekleyerek (örneğin, `dashboard-edit-dialog`) daha kolay hedeflenebilir seçiciler oluşturun

## Sonuç

Bu ek düzeltmelerle, dialog ve tabview bileşenlerinin siyah arka plan sorunu tamamen çözülmüştür. Artık tüm bileşenler beyaz arka plana ve koyu metin rengine sahiptir, bu da içeriğin daha okunabilir olmasını sağlar.

PrimeNG'nin Lara temasına geçiş ve özel stil düzeltmeleri ile bileşenlerin arka plan ve metin rengi sorunları çözülmüştür. Bu değişiklikler, kullanıcı deneyimini iyileştirmiş ve uygulamanın görsel tutarlılığını artırmıştır. 