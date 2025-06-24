# BaseHttpService Kullanım Kılavuzu

## Genel Bakış

`BaseHttpService`, HTTP isteklerini yönetmek için merkezi bir servis sağlar. Bu servis, tüm HTTP isteklerini standartlaştırır, hata yönetimini merkezileştirir ve kod tekrarını azaltır. Ayrıca, API yanıtlarını normalize ederek farklı API yanıt formatlarıyla uyumlu çalışmayı sağlar.

## Özellikler

- Standart HTTP metodları (GET, POST, PUT, DELETE)
- Merkezi hata yönetimi
- Otomatik token yönetimi
- API yanıtı normalizasyonu
- Dosya yükleme ve indirme desteği

## Kurulum

`BaseHttpService`, `CoreModule` içinde tanımlanmıştır ve uygulama genelinde kullanılabilir. Servisinizin `BaseHttpService`'i kullanabilmesi için aşağıdaki adımları izleyin:

1. Servisinizi `BaseHttpService`'den türetin
2. Constructor'da `http` ve `authService` parametrelerini `protected override` olarak tanımlayın
3. `super(http, authService)` çağrısı yapın

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../../../core/services/base-http.service';
import { AuthService } from '../../../core/authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class MyService extends BaseHttpService {
  private endpoint = '/api/my-endpoint';

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }
}
```

## Kullanım

### GET İsteği

```typescript
getItems(): Observable<Item[]> {
  return this.get<Item[]>(this.endpoint);
}

getItemById(id: number): Observable<Item> {
  return this.get<Item>(`${this.endpoint}/${id}`);
}
```

### POST İsteği

```typescript
createItem(item: Omit<Item, 'id'>): Observable<Item> {
  return this.post<Item>(this.endpoint, item);
}
```

### PUT İsteği

```typescript
updateItem(id: number, item: Partial<Item>): Observable<Item> {
  return this.put<Item>(`${this.endpoint}/${id}`, item);
}
```

### DELETE İsteği

```typescript
deleteItem(id: number): Observable<void> {
  return this.delete<void>(`${this.endpoint}/${id}`);
}
```

### Dosya Yükleme

```typescript
uploadItemFile(id: number, file: File): Observable<any> {
  return this.uploadFile<any>(`${this.endpoint}/${id}/upload`, file);
}
```

### Dosya İndirme

```typescript
downloadItemFile(id: number): Observable<Blob> {
  return this.downloadFile(`${this.endpoint}/${id}/download`);
}
```

## Hata Yönetimi

`BaseHttpService`, tüm HTTP isteklerinde otomatik olarak hata yönetimi sağlar. Hatalar konsola loglanır ve kullanıcı dostu hata mesajları oluşturulur. Özel hata işleme için `catchError` operatörünü kullanabilirsiniz:

```typescript
getItems(): Observable<Item[]> {
  return this.get<Item[]>(this.endpoint)
    .pipe(
      catchError(error => {
        console.error('Özel hata işleme:', error);
        return of([] as Item[]); // Boş dizi döndür
      })
    );
}
```

## API Yanıtı Normalizasyonu

`BaseHttpService`, farklı API yanıt formatlarını otomatik olarak normalize eder. Desteklenen formatlar:

- `$values` dizisi (ReferenceHandler.Preserve)
- Normal dizi yanıtı
- `value` veya `Value` içindeki dizi
- Tek nesne yanıtı

## Örnekler

### Örnek Servis

```typescript
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseHttpService } from '../../../core/services/base-http.service';
import { AuthService } from '../../../core/authentication/auth.service';

export interface ExampleItem {
  id: number;
  name: string;
  description: string;
  createdAt: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ExampleService extends BaseHttpService {
  private endpoint = '/api/example';

  constructor(
    protected override http: HttpClient,
    protected override authService: AuthService
  ) {
    super(http, authService);
  }

  getItems(): Observable<ExampleItem[]> {
    return this.get<ExampleItem[]>(this.endpoint);
  }

  getItemById(id: number): Observable<ExampleItem> {
    return this.get<ExampleItem>(`${this.endpoint}/${id}`);
  }

  createItem(item: Omit<ExampleItem, 'id' | 'createdAt'>): Observable<ExampleItem> {
    return this.post<ExampleItem>(this.endpoint, item);
  }

  updateItem(id: number, item: Partial<ExampleItem>): Observable<ExampleItem> {
    return this.put<ExampleItem>(`${this.endpoint}/${id}`, item);
  }

  deleteItem(id: number): Observable<void> {
    return this.delete<void>(`${this.endpoint}/${id}`);
  }
}
```

## Faydaları

- **Kod Tekrarını Azaltır**: Tüm servislerde aynı HTTP işlemlerini tekrar tekrar yazmak yerine, ortak işlevselliği `BaseHttpService`'de toplar.
- **Tutarlı Hata Yönetimi**: Tüm HTTP isteklerinde aynı hata yönetimi mantığı kullanılır.
- **Merkezi Token Yönetimi**: Tüm isteklerde otomatik olarak token eklenir.
- **Bakım Kolaylığı**: HTTP isteklerinde yapılacak değişiklikler tek bir yerde yapılabilir.
- **Standartlaştırma**: Tüm servisler aynı yapıyı kullanır, bu da kodun okunabilirliğini artırır.

## Gelecek Geliştirmeler

- İstek önbelleğe alma (caching) mekanizması
- İstek yeniden deneme (retry) mekanizması
- İstek iptal etme (cancellation) desteği
- İstek ilerleme (progress) takibi
- İstek önceliği (priority) belirleme 