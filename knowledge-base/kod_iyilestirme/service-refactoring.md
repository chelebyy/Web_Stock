# Frontend Servis Refaktörü

## Genel Bakış

Bu belge, frontend servislerinin BaseHttpService'i kullanacak şekilde refaktör edilmesi sürecini belgelemektedir. Bu refaktör çalışması, kod tekrarını azaltmak, hata yönetimini merkezileştirmek ve bakımı kolaylaştırmak amacıyla yapılmıştır.

## Yapılan Değişiklikler

### 1. BaseHttpService Kullanımı

Aşağıdaki servisler, BaseHttpService'i kullanacak şekilde güncellenmiştir:

- RoleService
- UserService
- PermissionService
- UserPermissionService
- PasswordService

### 2. Değişiklik Detayları

Her servis için yapılan değişiklikler:

#### RoleService

- `private apiUrl` yerine `private endpoint` kullanıldı
- HTTP işlemleri için BaseHttpService metodları kullanıldı
- Tekrarlanan hata yönetimi ve yanıt normalizasyonu kodu kaldırıldı

#### UserService

- `private apiUrl` yerine `private usersEndpoint` ve `private authEndpoint` kullanıldı
- HTTP işlemleri için BaseHttpService metodları kullanıldı
- Profil resmi işlemleri için `uploadFile` ve `downloadFile` metodları kullanıldı
- HttpStatusCodes ve ErrorCodes sabitleri korundu

#### PermissionService

- `private apiUrl` yerine `private endpoint` kullanıldı
- HTTP işlemleri için BaseHttpService metodları kullanıldı
- PreserveFormat interface'i kaldırıldı (BaseHttpService'de zaten var)

#### UserPermissionService

- `private apiUrl` yerine `private endpoint` kullanıldı
- HTTP işlemleri için BaseHttpService metodları kullanıldı
- Tekrarlanan yanıt normalizasyonu kodu kaldırıldı

#### PasswordService

- `private apiUrl` yerine `private endpoint` ve `private fixPasswordEndpoint` kullanıldı
- HTTP işlemleri için BaseHttpService metodları kullanıldı
- Şifre sıfırlama işlemleri için özel header kullanımı korundu

## Faydalar

1. **Kod Tekrarının Azaltılması**: Her serviste tekrarlanan HTTP işlemleri, hata yönetimi ve yanıt normalizasyonu kodu kaldırıldı.
2. **Merkezi Hata Yönetimi**: Tüm HTTP isteklerinde aynı hata yönetimi mantığı kullanılıyor.
3. **Standartlaştırma**: Tüm servisler aynı yapıyı kullanıyor, bu da kodun okunabilirliğini artırıyor.
4. **Bakım Kolaylığı**: HTTP isteklerinde yapılacak değişiklikler tek bir yerde yapılabilir.
5. **Dosya İşlemleri Desteği**: Dosya yükleme ve indirme işlemleri için özel metodlar kullanılıyor.

## Dikkat Edilmesi Gereken Noktalar

1. **Constructor Parametreleri**: BaseHttpService'i extend eden servislerde, constructor parametrelerinde `protected override` anahtar kelimesi kullanılmalıdır.
2. **Endpoint Tanımları**: Her servis kendi endpoint'ini tanımlamalıdır.
3. **Özel İşlemler**: Şifre sıfırlama gibi özel işlemler için, BaseHttpService'in metodları yerine özel HTTP istekleri kullanılabilir.

## Örnek Kullanım

```typescript
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

  getItems(): Observable<Item[]> {
    return this.get<Item[]>(this.endpoint);
  }

  getItemById(id: number): Observable<Item> {
    return this.get<Item>(`${this.endpoint}/${id}`);
  }

  createItem(item: Omit<Item, 'id'>): Observable<Item> {
    return this.post<Item>(this.endpoint, item);
  }

  updateItem(id: number, item: Partial<Item>): Observable<Item> {
    return this.put<Item>(`${this.endpoint}/${id}`, item);
  }

  deleteItem(id: number): Observable<void> {
    return this.delete<void>(`${this.endpoint}/${id}`);
  }
}
```

## Sonraki Adımlar

1. **Birim Testleri**: BaseHttpService ve türetilmiş servisler için birim testleri yazılmalıdır.
2. **Önbelleğe Alma**: Sık kullanılan istekler için önbelleğe alma mekanizması eklenebilir.
3. **İstek Yeniden Deneme**: Başarısız istekler için otomatik yeniden deneme mekanizması eklenebilir.
4. **İstek İptal Etme**: Uzun süren istekleri iptal etme desteği eklenebilir.
5. **İstek İlerleme Takibi**: Dosya yükleme gibi işlemler için ilerleme takibi eklenebilir. 