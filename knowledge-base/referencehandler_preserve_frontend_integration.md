# ReferenceHandler.Preserve ve Frontend Entegrasyonu

## Sorun Tanımı

.NET Core 9 ile geliştirilen API projesi, JSON serileştirme sırasında döngüsel referansları yönetmek için `ReferenceHandler.Preserve` ayarını kullanıyor. Bu ayar, JSON yanıtlarının formatını özel bir yapıya dönüştürür ve bu durum Angular/PrimeNG tarafından doğrudan işlenmeyen bir formata neden olur.

## ReferenceHandler.Preserve Nedir?

`ReferenceHandler.Preserve`, .NET'in System.Text.Json serileştirme kütüphanesinin bir özelliğidir. Bu ayar etkinleştirildiğinde, döngüsel referansları (örn. `Role -> Users -> Role -> ...`) yönetmek için serileştirilmiş JSON'da referansları korumak için özel bir format kullanır:

```json
{
  "$id": "1",
  "$values": [
    { 
      "$id": "2",
      "id": 1, 
      "name": "Admin",
      "users": {
        "$id": "3",
        "$values": [
          {
            "$id": "4",
            "id": 1,
            "username": "admin",
            "role": {
              "$ref": "2"
            }
          }
        ]
      }
    },
    { 
      "$id": "5",
      "id": 2, 
      "name": "User",
      "users": { 
        "$id": "6",
        "$values": [] 
      }
    }
  ]
}
```

Normal bir JSON dizisi yerine, `$id` ve `$values` özelliklerine sahip bir nesne alırız. Bu format, döngüsel referansları önlemek için tasarlanmıştır ancak Angular ve PrimeNG bileşenleri için zorluklar yaratır.

## Angular ve PrimeNG ile Yaşanan Sorunlar

1. **ngFor Sorunu**: Angular'ın `*ngFor` direktifi yalnızca Iterable nesnelerle çalışır. `{ $id: "1", $values: [...] }` formatı bir dizi değil, bir nesnedir.

   ```typescript
   // Hata
   ERROR: Cannot find a differ supporting object '[object Object]' of type 'object'.
   NgFor only supports binding to Iterables, such as Arrays.
   ```

2. **PrimeNG Table Sorunu**: PrimeNG Table bileşeni, verileri düz bir dizi olarak bekler ve `_data.slice()` metodunu çağırır, ancak bu metot bir nesnede bulunmaz.

   ```typescript
   // Hata
   ERROR TypeError: _data.slice is not a function
   ```

## Çözüm: Frontend'de Veri Normalizasyonu

Aşağıdaki çözümler, frontend uygulamasının `ReferenceHandler.Preserve` formatıyla çalışmasını sağlar:

### 1. Tip Tanımı Ekleme

```typescript
// ReferenceHandler.Preserve formatı için tip tanımı
interface PreserveFormat<T> {
  $id?: string;
  $values?: T[];
}
```

### 2. Servis Seviyesinde Normalizasyon

```typescript
// Servis sınıfı içinde
private normalizeResponse<T>(response: any): T {
  if (!response) return [] as unknown as T;
  
  // $values dizisi varsa
  if (response && response.$values) {
    console.log('$values dizisi bulundu:', response.$values.length, 'öğe var');
    return response.$values as T;
  }
  // Dizi ise doğrudan döndür
  else if (Array.isArray(response)) {
    console.log('Dizi yanıtı bulundu:', response.length, 'öğe var');
    return response as T;
  }
  // Başka özelliklerde veri varsa
  else if (response && typeof response === 'object') {
    // value veya Value özelliğini kontrol et
    if (response.value && Array.isArray(response.value)) {
      return response.value as T;
    }
    
    if (response.Value && Array.isArray(response.Value)) {
      return response.Value as T;
    }
    
    // ID özelliğine sahip tek bir nesne ise
    if ('id' in response || 'Id' in response) {
      return response as T;
    }
  }
  
  // Son çare: orijinal yanıtı döndür
  return response as T;
}

// Kullanımı
getRoles(): Observable<Role[]> {
  return this.http.get<any>(`${this.apiUrl}`)
    .pipe(map(response => this.normalizeResponse<Role[]>(response)));
}
```

### 3. Bileşen Seviyesinde Normalizasyon

```typescript
loadRoles() {
  this.roleService.getRoles().subscribe({
    next: (data: any) => {
      // Döngüsel referans düzeltmesi - ReferenceHandler.Preserve formatını düz diziye dönüştür
      if (data && data.$values) {
        this.roles = data.$values as Role[];
      } else if (Array.isArray(data)) {
        this.roles = data as Role[];
      } else {
        this.roles = []; // Varsayılan boş dizi
      }
    }
  });
}
```

### 4. İç İçe Özellikler için Normalizasyon

```typescript
// İzin gruplarına erişim için normalizasyon
getPermissionsByGroup(groupName: string): Permission[] {
  if (!this.permissionGroups || !Array.isArray(this.permissionGroups)) {
    console.warn('permissionGroups dizi değil veya tanımsız');
    return [];
  }
  
  const group = this.permissionGroups.find(g => g.group === groupName);
  
  if (!group) {
    return [];
  }
  
  // İzinler dizisi olabilir veya $values içeren bir nesne olabilir
  if (Array.isArray(group.permissions)) {
    return group.permissions;
  } else if (group.permissions && typeof group.permissions === 'object') {
    // PreserveFormat kontrolü
    const preserveData = group.permissions as unknown as PreserveFormat<Permission>;
    return preserveData && preserveData.$values ? preserveData.$values : [];
  }
  
  return [];
}
```

## İyileştirme Önerileri

Tüm frontend uygulamasında tutarlı bir veri normalizasyon yaklaşımı için aşağıdaki adımları öneririz:

1. **Merkezi Yardımcı Servis**: Tüm uygulamada tutarlı normalizasyon için bir `DataNormalizationService` oluşturun.

   ```typescript
   @Injectable({
     providedIn: 'root'
   })
   export class DataNormalizationService {
     normalizeArray<T>(data: any): T[] {
       // Normalizasyon mantığı burada
     }
   }
   ```

2. **HTTP Interceptor**: Tüm API yanıtlarını otomatik olarak normalleştirmek için bir HTTP interceptor kullanın.

3. **API Yanıt Wrapper'ları**: API yanıtları için tip güvenliği sağlayan wrapper sınıfları oluşturun.

   ```typescript
   export interface ApiResponse<T> {
     data: T;
     error?: string;
     success: boolean;
   }
   ```

4. **Geliştirici Araçları**: Geliştirme sırasında API yanıtlarını izlemek için geliştirici araçları kullanın.

## Alternatif Çözüm: Backend'de Ayarları Değiştirme

Frontend'i değiştirmek yerine, backend'de JSON serileştirme ayarlarını değiştirebiliriz:

```csharp
// Program.cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Döngüsel referansları tamamen yoksay
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        
        // VEYA
        
        // DTO kullanarak manuel dönüşüm yap
        // options.JsonSerializerOptions.ReferenceHandler = null;
    });
```

Bu yaklaşım daha temiz olabilir, ancak mevcut API'yi kullanan diğer istemciler için geri uyumluluğu bozabilir.

## Öğrenilen Dersler

1. API yanıt formatlarının frontend ile uyumluluğunu erken aşamalarda test edin
2. Değişen serileştirme ayarlarını projede belgelendirin
3. Serileştirme sorunları için hem frontend hem de backend çözümleri değerlendirin
4. Veri dönüşümleri için tutarlı ve merkezi bir yaklaşım benimseyin
5. Hata mesajlarını dikkatlice analiz edin; örneğin "Cannot find a differ" hatası genellikle bir nesnenin dizi olarak işlenmeye çalışıldığını gösterir
6. API yanıtlarının her zaman varyasyonlarını ele almak için güçlü kod yazın
7. Tüm veri dönüşümleri için type guard'ları kullanın 