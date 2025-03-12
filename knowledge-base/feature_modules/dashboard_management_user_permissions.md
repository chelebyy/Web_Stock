# Dashboard Sayfası Kullanıcı İzinleri Yönetimi

Bu belge, dashboard sayfalarına erişim izni verilen kullanıcıların yönetimi için eklenen özelliği açıklamaktadır.

## Genel Bakış

Dashboard yönetimi sayfasında, her bir dashboard sayfası için erişim izni verilen kullanıcıları belirlemek amacıyla bir kullanıcı seçim alanı eklenmiştir. Bu özellik sayesinde, belirli bir sayfaya erişim izni olan kullanıcıları doğrudan seçebilir ve yönetebilirsiniz.

## Teknik Detaylar

### Veri Modeli Değişiklikleri

Dashboard sayfası modeline aşağıdaki alanlar eklenmiştir:

```typescript
interface DashboardPage {
  // Mevcut alanlar
  id: number;
  name: string;
  description: string;
  route: string;
  isActive: boolean;
  requiredPermission: string;
  
  // Yeni eklenen alanlar
  allowedUsers?: User[]; // İzin verilen kullanıcılar
  allowedUserIds?: number[]; // İzin verilen kullanıcı ID'leri
  createdAt: Date;
  updatedAt: Date;
}
```

### Kullanıcı Seçimi İçin Arayüz Bileşenleri

Kullanıcı seçimi için PrimeNG MultiSelect bileşeni kullanılmıştır:

```html
<div class="field">
  <label for="allowedUsers">İzin Verilen Kullanıcılar</label>
  <p-multiSelect
    id="allowedUsers"
    [options]="userOptions"
    [(ngModel)]="page.allowedUserIds"
    placeholder="Kullanıcı seçin"
    optionLabel="label"
    optionValue="value"
    [style]="{'width':'100%', 'background-color': 'white'}"
    appendTo="body"
    [autoZIndex]="true"
    [baseZIndex]="10000"
    styleClass="dashboard-users-multiselect"
    [panelStyleClass]="'dashboard-users-multiselect-panel'"
    [filter]="true"
    filterPlaceHolder="Kullanıcı ara..."
    [showToggleAll]="true"
    [virtualScroll]="true"
    [itemSize]="38"
    [maxSelectedLabels]="3"
    selectedItemsLabel="{0} kullanıcı seçildi"
    defaultLabel="Kullanıcı seçin"
    display="chip">
    <!-- Özel şablonlar -->
  </p-multiSelect>
  <small class="p-help-text">Bu sayfaya erişim izni verilecek kullanıcıları seçin. Boş bırakırsanız, gerekli izne sahip tüm kullanıcılar erişebilir.</small>
</div>
```

### Kullanıcı Verilerinin Alınması

Kullanıcı verileri, UserService aracılığıyla API'den alınmaktadır:

```typescript
// Kullanıcıları yükle
loadUsers(): void {
  this.userService.getUsers().subscribe({
    next: (users) => {
      this.users = users;
      // Kullanıcı seçeneklerini oluştur
      this.userOptions = users.map(user => ({
        label: `${user.firstName || ''} ${user.lastName || ''} (${user.sicil})`,
        value: user.id || 0,
        sicil: user.sicil
      }));
      console.log('Kullanıcılar yüklendi:', this.userOptions);
    },
    error: (error) => {
      console.error('Kullanıcıları yüklerken hata oluştu:', error);
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Kullanıcılar yüklenirken bir hata oluştu.'
      });
    }
  });
}
```

### Kullanıcı Seçimi Görünümü

Kullanıcı seçimi için özel görünüm şablonları eklenmiştir:

1. **Öğe Şablonu**: Her bir kullanıcı öğesinin nasıl görüntüleneceğini belirler.
   ```html
   <ng-template let-user pTemplate="item">
     <div class="user-item">
       <span class="user-name">{{ user.label }}</span>
       <span class="user-sicil">({{ user.sicil }})</span>
     </div>
   </ng-template>
   ```

2. **Seçilen Öğeler Şablonu**: Seçilen kullanıcıların nasıl görüntüleneceğini belirler.
   ```html
   <ng-template let-value pTemplate="selectedItems">
     <div *ngIf="value" class="selected-users-container">
       <div *ngFor="let user of value | slice:0:3" class="selected-user-chip">
         {{ getUserLabelById(user) }}
       </div>
       <div *ngIf="value.length > 3" class="more-users-chip">
         +{{ value.length - 3 }} daha
       </div>
     </div>
   </ng-template>
   ```

### Stil Tanımlamaları

MultiSelect bileşeni için özel stiller eklenmiştir:

```scss
/* MultiSelect Stilleri */
.dashboard-users-multiselect {
  width: 100%;
  
  .p-multiselect-label {
    color: #333333 !important;
    font-weight: 500 !important;
    padding: 0.75rem 0.75rem !important;
    background-color: white !important;
  }
  
  // Diğer stil tanımlamaları...
}

.user-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  
  .user-name {
    font-weight: 500;
    color: #333333;
  }
  
  .user-sicil {
    color: #64748b;
    font-size: 0.9rem;
  }
}

// Diğer stil tanımlamaları...
```

## Kullanım Senaryoları

### Senaryo 1: Tüm Kullanıcılara İzin Verme

Eğer bir dashboard sayfası için kullanıcı seçimi yapılmazsa (boş bırakılırsa), o sayfaya erişim izni olan tüm kullanıcılar (requiredPermission alanında belirtilen izne sahip olanlar) sayfaya erişebilir.

### Senaryo 2: Belirli Kullanıcılara İzin Verme

Eğer bir dashboard sayfası için belirli kullanıcılar seçilirse, sadece bu kullanıcılar sayfaya erişebilir. Bu durumda, requiredPermission alanında belirtilen izne sahip olsalar bile, seçilmeyen kullanıcılar sayfaya erişemez.

## Güvenlik Hususları

1. Kullanıcı izinleri hem istemci tarafında hem de sunucu tarafında kontrol edilmelidir.
2. Kullanıcı listesi, yalnızca yönetici yetkisine sahip kullanıcılar tarafından görüntülenebilir ve düzenlenebilir olmalıdır.
3. API isteklerinde, kullanıcı izinleri doğrulanmalıdır.

## Gelecek Geliştirmeler

1. Kullanıcı grupları oluşturarak, birden fazla kullanıcıya aynı anda izin verme özelliği eklenebilir.
2. İzin verilen kullanıcıların geçmişi (kim tarafından, ne zaman izin verildiği) takip edilebilir.
3. Kullanıcı izinlerinin geçerlilik süresi eklenebilir.

## Sonuç

Bu özellik sayesinde, dashboard sayfalarına erişim izinleri daha granüler bir şekilde yönetilebilir hale gelmiştir. Artık belirli bir sayfaya sadece belirli kullanıcıların erişmesini sağlayabilir, böylece sistem güvenliğini ve kullanıcı deneyimini iyileştirebilirsiniz. 