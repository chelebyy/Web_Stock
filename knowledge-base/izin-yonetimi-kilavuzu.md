# Kullanıcı İzinleri Yönetim Kılavuzu

Bu kılavuz, sistemdeki kullanıcıların sayfa bazlı yetkilerini nasıl yöneteceğinizi açıklamaktadır.

## İçindekiler

1. [İzin Sistemi Genel Bakış](#izin-sistemi-genel-bakış)
2. [Rol Bazlı İzin Atama](#rol-bazlı-izin-atama)
3. [Kullanıcı Bazlı Özel İzin Atama](#kullanıcı-bazlı-özel-izin-atama)
4. [Örnek Senaryo: Test ve Test2 Kullanıcıları](#örnek-senaryo-test-ve-test2-kullanıcıları)
5. [Sorun Giderme](#sorun-giderme)

## İzin Sistemi Genel Bakış

Sistemdeki izin yapısı şu şekilde çalışır:

- **Roller**: Kullanıcılar belirli rollere atanır (örn. Admin, User, Manager)
- **İzinler**: Sayfalara ve işlevlere erişimi kontrol eden izinler (örn. IT.Access, Pages.UserDashboard)
- **Rol İzinleri**: Rollere atanan izinler, o role sahip tüm kullanıcılar için geçerlidir
- **Kullanıcı İzinleri**: Kullanıcılara özel olarak atanan izinler, rol izinlerini geçersiz kılabilir

### İzin Hiyerarşisi

1. Admin kullanıcılar her zaman tüm izinlere sahiptir
2. Kullanıcıya özel atanan izinler, rol izinlerinden daha önceliklidir
3. Kullanıcının rolünden gelen izinler

## Rol Bazlı İzin Atama

Rol bazlı izin atama, kullanıcıları belirli rollere atayarak ve bu rollere izinler tanımlayarak yapılır:

### Adım 1: Rol İzinlerini Yönetme

1. Admin panelinde "Roller" menüsüne gidin
2. İzinlerini düzenlemek istediğiniz rolün yanındaki "İzinleri Yönet" butonuna tıklayın
3. Açılan sayfada, izinler gruplar halinde listelenecektir
4. İstediğiniz izinleri işaretleyin ve "Kaydet" butonuna tıklayın

### Adım 2: Kullanıcıya Rol Atama

1. Admin panelinde "Kullanıcılar" menüsüne gidin
2. Düzenlemek istediğiniz kullanıcının yanındaki "Düzenle" butonuna tıklayın
3. Açılan formda "Rol" alanından kullanıcıya atamak istediğiniz rolü seçin
4. "Kaydet" butonuna tıklayın

## Kullanıcı Bazlı Özel İzin Atama

Bazı kullanıcılara rollerinden bağımsız olarak özel izinler atamak isteyebilirsiniz:

### Adım 1: Kullanıcı İzinlerini Yönetme

1. Admin panelinde "Kullanıcılar" menüsüne gidin
2. İzinlerini düzenlemek istediğiniz kullanıcının yanındaki "İzinleri Yönet" butonuna tıklayın
3. Açılan sayfada, kullanıcının rolünden gelen izinler ve özel izinleri gösterilecektir
4. İstediğiniz izinleri işaretleyin ve "Kaydet" butonuna tıklayın

### Adım 2: Rol İzinlerine Sıfırlama

Kullanıcının özel izinlerini kaldırıp, sadece rolünden gelen izinlere sahip olmasını istiyorsanız:

1. Kullanıcı izinleri sayfasında "Rol İzinlerine Sıfırla" butonuna tıklayın
2. Onay mesajını kabul edin

## Örnek Senaryo: Test ve Test2 Kullanıcıları

Test kullanıcısının Bilgi İşlem sayfasına erişebilmesi, Test2 kullanıcısının erişememesi için:

### Test Kullanıcısına İzin Atama

1. Admin panelinde "Kullanıcılar" menüsüne gidin
2. Test kullanıcısının yanındaki "İzinleri Yönet" butonuna tıklayın
3. "Pages" grubunda "IT.Access" iznini işaretleyin
4. "Kaydet" butonuna tıklayın

### Test2 Kullanıcısından İzin Kaldırma

1. Admin panelinde "Kullanıcılar" menüsüne gidin
2. Test2 kullanıcısının yanındaki "İzinleri Yönet" butonuna tıklayın
3. "Pages" grubunda "IT.Access" izninin işaretli olmadığından emin olun
4. "Kaydet" butonuna tıklayın

## Sorun Giderme

### İzin Değişiklikleri Uygulanmıyor

1. **Oturumu Yenileme**: Kullanıcının oturumunu kapatıp yeniden açması gerekebilir
2. **Önbellek Temizleme**: Tarayıcı önbelleğini temizleyin
3. **Token Kontrolü**: JWT token'ın süresi dolmuş olabilir

### Kullanıcı İzinleri Görünmüyor

1. **Veritabanı Kontrolü**: Veritabanında ilgili izinlerin tanımlı olduğundan emin olun
2. **API Kontrolü**: API'nin doğru çalıştığından emin olun
3. **Rol Kontrolü**: Kullanıcının rolünün doğru atandığından emin olun 