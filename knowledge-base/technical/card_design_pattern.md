# Kart Tasarım Deseni Teknik Belgesi

Versiyon: 1.0
Son Güncelleme: 12.03.2025

## Genel Bakış

Kart tasarım deseni, bilgileri gruplamak ve görsel olarak organize etmek için kullanılan modern bir UI yaklaşımıdır. Bu belge, projemizde kart tasarım deseninin nasıl uygulandığını ve en iyi uygulamaları açıklar.

## Kart Bileşeni Anatomisi

### Temel Yapı

```html
<div class="card">
  <div class="card-header">
    <h3 class="card-title">Başlık</h3>
    <div class="card-actions">
      <!-- Eylem butonları -->
    </div>
  </div>
  <div class="card-content">
    <!-- Ana içerik -->
  </div>
  <div class="card-footer">
    <!-- Alt bilgiler veya ek eylemler -->
  </div>
</div>
```

### SCSS Yapısı

```scss
.card {
  background-color: #ffffff;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05), 0 1px 3px rgba(0, 0, 0, 0.1);
  transition: all 0.2s ease;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  border: 2px solid #93c5fd;
  
  &:hover {
    transform: translateY(-2px);
    box-shadow: 0 10px 15px rgba(0, 0, 0, 0.05), 0 4px 6px rgba(0, 0, 0, 0.05);
    border-color: #3B82F6;
  }
  
  .card-header {
    padding: 1rem 1.5rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid #e2e8f0;
    
    .card-title {
      margin: 0;
      font-size: 1.25rem;
      font-weight: 600;
      color: #1e293b;
    }
    
    .card-actions {
      display: flex;
      gap: 0.5rem;
    }
  }
  
  .card-content {
    padding: 1.5rem;
    flex: 1;
  }
  
  .card-footer {
    padding: 1rem 1.5rem;
    border-top: 1px solid #e2e8f0;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
```

## Kart Düzeni

### Grid Düzeni

Kartları düzenlemek için CSS Grid kullanıyoruz:

```scss
.card-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1.5rem;
  
  @media (max-width: 768px) {
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
  }
  
  @media (max-width: 480px) {
    grid-template-columns: 1fr;
  }
}
```

### Esnek Düzen

Bazı durumlarda, Flexbox kullanarak kartları düzenliyoruz:

```scss
.card-flex {
  display: flex;
  flex-wrap: wrap;
  gap: 1.5rem;
  
  .card {
    flex: 1 1 250px;
    min-width: 250px;
    max-width: 100%;
    
    @media (max-width: 480px) {
      flex: 1 1 100%;
    }
  }
}
```

## Kart Varyasyonları

### Kullanıcı Kartı

```html
<div class="card user-card">
  <div class="card-header">
    <div class="user-avatar">
      <img src="avatar.jpg" alt="Kullanıcı Avatarı">
    </div>
    <h3 class="card-title">Kullanıcı Adı</h3>
  </div>
  <div class="card-content">
    <div class="user-info">
      <p><strong>E-posta:</strong> kullanici@ornek.com</p>
      <p><strong>Rol:</strong> Yönetici</p>
    </div>
  </div>
  <div class="card-footer">
    <span class="user-status active">Aktif</span>
    <button class="btn btn-primary">Düzenle</button>
  </div>
</div>
```

### İzin Kartı

```html
<div class="card permission-card">
  <div class="card-header">
    <h3 class="card-title">Sayfa İzni</h3>
    <div class="card-actions">
      <button class="btn btn-icon" pTooltip="Bilgi" tooltipPosition="top">
        <i class="pi pi-info-circle"></i>
      </button>
    </div>
  </div>
  <div class="card-content">
    <p class="permission-name">Stok Yönetimi</p>
    <p class="permission-description">Stok ekleme, düzenleme ve silme izni</p>
    <p class="permission-path">/stock-management</p>
  </div>
  <div class="card-footer">
    <span class="permission-status" [ngClass]="{'assigned': isAssigned}">
      {{ isAssigned ? 'Atanmış' : 'Atanmamış' }}
    </span>
    <button class="btn" [ngClass]="isAssigned ? 'btn-danger' : 'btn-success'">
      {{ isAssigned ? 'İzni Kaldır' : 'İzin Ver' }}
    </button>
  </div>
</div>
```

## Erişilebilirlik Hususları

Kart tasarımlarında erişilebilirliği sağlamak için aşağıdaki hususlara dikkat edilmelidir:

1. **Semantik HTML**: Uygun HTML etiketleri kullanın (h1-h6, button, a, vb.)
2. **Klavye Erişimi**: Tüm etkileşimli öğeler klavye ile erişilebilir olmalıdır
3. **ARIA Etiketleri**: Gerektiğinde ARIA etiketleri ekleyin
4. **Renk Kontrastı**: Metin ve arka plan arasında yeterli kontrast sağlayın
5. **Odak Göstergeleri**: Klavye odağı için görsel göstergeler ekleyin

```html
<div class="card" tabindex="0" role="region" aria-labelledby="card-title-1">
  <div class="card-header">
    <h3 id="card-title-1" class="card-title">Erişilebilir Kart Başlığı</h3>
  </div>
  <div class="card-content">
    <!-- İçerik -->
  </div>
  <div class="card-footer">
    <button class="btn btn-primary" aria-label="Düzenle">Düzenle</button>
  </div>
</div>
```

## Performans Optimizasyonu

Kart tabanlı arayüzlerde performansı optimize etmek için:

1. **Sanal Kaydırma**: Büyük kart listeleri için sanal kaydırma kullanın
2. **Tembel Yükleme**: Kartları görünür hale geldikçe yükleyin
3. **Resim Optimizasyonu**: Kart içindeki resimleri optimize edin
4. **CSS Optimizasyonu**: Gereksiz CSS kurallarından kaçının
5. **Animasyon Performansı**: `transform` ve `opacity` gibi performanslı özellikleri kullanın

## En İyi Uygulamalar

1. **Tutarlılık**: Tüm uygulama genelinde tutarlı kart tasarımları kullanın
2. **Responsive Tasarım**: Farklı ekran boyutları için uyarlanabilir kartlar tasarlayın
3. **İçerik Hiyerarşisi**: Önemli bilgileri kartın üst kısmında gösterin
4. **Görsel Geri Bildirim**: Hover, focus ve aktif durumlar için görsel geri bildirim sağlayın
5. **Yükleme Durumları**: Kart içeriği yüklenirken iskelet ekranlar (skeleton screens) kullanın

## Örnek Kullanım Senaryoları

1. **Kullanıcı Listesi**: Kullanıcı bilgilerini kartlar halinde gösterme
2. **İzin Yönetimi**: Sayfa izinlerini kartlar halinde gösterme
3. **Ürün Kataloğu**: Ürünleri kartlar halinde listeleme
4. **Dashboard Widgetları**: İstatistikleri kartlar halinde gösterme
5. **Form Bölümleri**: Form alanlarını mantıksal gruplara ayırma

## Değişiklik Kaydı
- 12.03.2025: İlk versiyon oluşturuldu 