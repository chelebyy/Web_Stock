# Kod İyileştirme Planı

## Genel Yaklaşım İlkeleri

1. **Kademeli Değişiklikler**: Sistemi bir anda değiştirmek yerine, küçük ve kontrollü adımlarla ilerleyeceğiz.
2. **Sürekli Test**: Her değişiklikten sonra kapsamlı testler yaparak sistemin çalışır durumda olduğunu doğrulayacağız.
3. **Feature Branch Kullanımı**: Her iyileştirme için ayrı bir feature branch oluşturacak ve değişiklikleri ana koda entegre etmeden önce gözden geçireceğiz.
4. **Geri Alma Stratejisi**: Her değişiklik için bir geri alma planı oluşturacak ve gerekirse hızlıca eski duruma dönebileceğiz.

## Aşama 1: Düşük Riskli İyileştirmeler

Bu aşamada, sistemin davranışını değiştirmeden yapılabilecek basit iyileştirmelere odaklanacağız.

### Aşama 1.1. Frontend Temizliği

- [X] Gereksiz `console.log` ifadelerinin kaldırılması
- [X] Sihirli string ve sayıların sabitlerle değiştirilmesi
- [X] Kullanılmayan import ifadelerinin temizlenmesi
- [X] Kod formatının düzenlenmesi (boşluklar, girintiler)
- [X] Yorum satırlarının iyileştirilmesi

### Aşama 1.2. Backend Temizliği

- [X] Gereksiz log ifadelerinin kaldırılması veya log seviyelerinin düzenlenmesi
- [X] Kullanılmayan using ifadelerinin temizlenmesi
- [X] Kod formatının düzenlenmesi
- [X] Yorum satırlarının iyileştirilmesi

### Aşama 1.3. Belgelendirme İyileştirmeleri

- [X] README.md dosyasının güncellenmesi
- [X] API dokümantasyonunun iyileştirilmesi
- [X] Kurulum ve çalıştırma talimatlarının güncellenmesi

### Aşama 1 Risk Değerlendirmesi

**Risk Seviyesi**: Düşük
**Potansiyel Etki**: Minimal, sistemin davranışını değiştirmeyecek
**Geri Alma Stratejisi**: Değişiklikler küçük ve izole olduğu için, gerekirse kolayca geri alınabilir

## Aşama 2: Orta Riskli İyileştirmeler

Bu aşamada, sistemin davranışını kısmen değiştirebilecek ancak büyük ölçüde iyileştirme sağlayacak değişikliklere odaklanacağız.

### Aşama 2.1. Global Exception Handling (Backend)

- [X] Merkezi bir hata yakalama mekanizması oluşturma
- [X] Hata loglamanın iyileştirilmesi
- [X] Kullanıcı dostu hata mesajlarının standartlaştırılması

### Aşama 2.2. Shared Components (Frontend)

- [ ] Sık kullanılan UI bileşenlerinin shared klasörüne taşınması
- [ ] Dialog ve confirmation dialog bileşenlerinin standartlaştırılması
- [ ] Form bileşenlerinin iyileştirilmesi

### Aşama 2.3. Base Service Oluşturma (Frontend)

- [X] Ortak HTTP istekleri için BaseHttpService oluşturma
- [X] Hata yönetiminin merkezileştirilmesi
- [X] Token yönetiminin iyileştirilmesi

### Aşama 2 Risk Değerlendirmesi

**Risk Seviyesi**: Orta
**Potansiyel Etki**: Sistemin bazı bölümlerinin davranışını değiştirebilir
**Geri Alma Stratejisi**: Her bir değişiklik için ayrı bir feature branch kullanılacak ve değişiklikler kademeli olarak uygulanacak

## Teknik Detaylar

### Frontend İyileştirmeleri Örneği

```typescript
// Önceki hali
function getUserData() {
  console.log("Getting user data...");
  http.get("http://localhost:5037/api/users").subscribe(
    data => {
      console.log("User data:", data);
      this.users = data;
    },
    error => {
      console.log("Error:", error);
    }
  );
}

// İyileştirilmiş hali
function getUserData() {
  this.userService.getUsers().subscribe({
    next: (data) => {
      this.users = data;
      this.loading = false;
    },
    error: (error) => {
      this.errorService.handleError(error);
      this.loading = false;
    }
  });
}
```

### Backend İyileştirmeleri Örneği

```csharp
// Önceki hali
[HttpGet]
public IActionResult GetUsers()
{
    try
    {
        var users = _userRepository.GetAll();
        return Ok(users);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return StatusCode(500, "Internal server error");
    }
}

// İyileştirilmiş hali
[HttpGet]
public async Task<IActionResult> GetUsers()
{
    var users = await _userService.GetAllUsersAsync();
    return Ok(users);
    // Global exception handler middleware tarafından hatalar yakalanacak
}
```

## İlerleme Takibi

Her bir aşama tamamlandığında, bu belge güncellenecek ve ilerleme durumu kaydedilecektir. Ayrıca, her bir değişiklik için ayrı bir commit oluşturulacak ve değişikliklerin açıklaması commit mesajında belirtilecektir.

## Sonraki Adımlar

Aşama 1 ve Aşama 2 tamamlandıktan sonra, daha yüksek riskli iyileştirmeler için yeni bir plan oluşturulacaktır. Bu plan, sistemin performansını, güvenliğini ve ölçeklenebilirliğini artırmaya odaklanacaktır. 