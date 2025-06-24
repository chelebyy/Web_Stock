# Kullanıcı Dashboard Sayfası Knowledge Base

## Genel Bakış
Bu belge, kullanıcı dashboard sayfasının geliştirilmesi sırasında edinilen deneyimleri, karşılaşılan zorlukları ve çözümleri içermektedir.

## Yapılan İşlemler
1. Mevcut user-dashboard bileşeni incelendi.
2. Dashboard üzerindeki butonlar sadeleştirilerek yalnızca BİLGİ İŞLEM butonu eklendi.
3. Şifre değiştirme ve çıkış yapma fonksiyonları korundu.
4. Şifre değiştirme ve çıkış yapma tasarımı, admin dashboard'daki dropdown menü tasarımına uygun şekilde güncellendi.
5. Kullanılmayan metodlar (navigateToInventory, navigateToConsumables, navigateToTasks) kaldırıldı ve navigateToIT metodu eklendi.
6. DragDropModule eklenerek şifre değiştirme formunun sürüklenebilir olması sağlandı.
7. Yeni bir profileImageUrl değişkeni ve loadProfileImage() metodu eklendi.

## Tasarım
- Dashboard sayfası, animasyonlu bir arka plan ve cam efektli bir kart ile modern bir görünüme sahiptir.
- Üst kısımda kullanıcı adı ve kullanıcı avatarı içeren bir dropdown menü yer almaktadır.
- Dropdown menüden şifre değiştirme ve çıkış yapma işlemleri gerçekleştirilebilir.
- Ana kısımda büyük butonlar şeklinde modüller gösterilmektedir (şu an için sadece BİLGİ İŞLEM).
- Şifre değiştirme formu, kullanıcı dostu bir arayüz ile sunulmaktadır ve sürüklenebilirdir.
- Responsive tasarım sayesinde farklı ekran boyutlarına uyum sağlamaktadır.

## Şifre Değiştirme Fonksiyonu
- Kullanıcı mevcut şifresi ve yeni şifresini girerek şifresini değiştirebilir.
- Şifre değiştirme işlemi auth.service.ts içindeki changePassword metodu ile gerçekleştirilir.
- Şifre göster/gizle butonları ile kullanıcı şifreleri görüntüleyebilir.
- Şifre değiştirme başarılı olduğunda veya hata durumunda kullanıcıya toast mesajları ile bilgi verilir.
- Şifre değiştirme formu, kullanıcı tarafından sürüklenebilir ve istenen yere taşınabilir.

## Kullanıcı Profil Menüsü
- Kullanıcı profil resmi, kullanıcı adı ve rolü içeren bir dropdown menü eklenmiştir.
- Menü, kullanıcı avatarına tıkladığında açılır ve dışarı tıklandığında otomatik olarak kapanır.
- Menü içinden şifre değiştirme ve çıkış yapma işlemleri yapılabilir.
- Admin dashboard ile tutarlı bir tasarım sağlanmıştır.

## Karşılaşılan Zorluklar ve Çözümleri
- **Sorun**: Kullanıcı dashboard yapısı çok karmaşıktı.
- **Çözüm**: Yapı sadeleştirilerek yalnızca gerekli butonlar bırakıldı.

- **Sorun**: Şifre değiştirme formu sayfada sabit bir konumda yer alıyordu.
- **Çözüm**: DragDropModule kullanılarak form sürüklenebilir hale getirildi.

- **Sorun**: Şifre değiştirme ve çıkış butonları sayfada çok yer kaplıyordu.
- **Çözüm**: Butonlar dropdown menüye taşınarak daha temiz bir tasarım elde edildi.

## İleride Yapılabilecek İyileştirmeler
1. Daha fazla modül butonu eklenebilir (kullanıcı yetkilerine göre dinamik olarak).
2. Dashboard üzerine kullanıcının son aktivitelerini gösteren bir alan eklenebilir.
3. Kullanıcı profil ayarları eklenebilir.
4. Tema değiştirme seçeneği eklenebilir (açık/koyu tema).
5. Bildirim sistemi entegre edilebilir.
6. Profil resmi yükleme fonksiyonu eklenebilir.

## Kullanılan Bileşenler ve Servisler
- **Bileşenler**: CardModule, ButtonModule, InputTextModule, ToastModule, DragDropModule
- **Servisler**: AuthService, MessageService, Router

## Ekran Görüntüleri
[Ekran görüntüleri buraya eklenebilir] 