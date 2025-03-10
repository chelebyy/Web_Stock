Bu sayfayı dikkate alma

Amaç ve Kapsam:

Sadece local ağda çalışacak, dış dünyaya kapalı bir web projesi.
Uzun vadeli, otomatik çalışan ve yönetilebilir bir yapı hedefleniyor.
Platform Tercihi:

Düşük özellikli bilgisayarlarda daha verimli olan Linux tabanlı sunucu kullanımı.
Docker ve Konteyner Yönetimi:

Her servisi izole bir konteynerde çalıştırmak için Docker kullanılacak.
Tüm servislerin (PostgreSQL, .NET Core API, Angular frontend, Nginx) yönetimi için Docker Compose tercih edilecek.
Servis Dağılımı:

PostgreSQL: Veritabanı, veri kalıcılığı için volume kullanılarak çalışacak.
.NET Core API: Dockerfile ile yapılandırılarak API servisi olarak çalıştırılacak.
Angular Frontend: Uygulama build alındıktan sonra statik dosyalar haline gelecek.
Nginx:
Ters proxy görevi görecek; Angular’ın statik dosyalarını sunacak ve /api/ isteklerini .NET Core API’ye yönlendirecek.
Nginx konfigürasyonunda yalnızca belirlenen IP bloğundan (örneğin, 192.168.1.0/24) erişime izin verilecek.
Otomatik Başlatma ve Yönetim:

Docker Compose dosyasında restart: always ayarı ile servislerin otomatik yeniden başlatılması sağlanacak.
Sistem açılışında Docker Compose’un otomatik çalışması için systemd veya crontab kullanılacak.
Güvenlik ve Erişim Kontrolü:

Nginx ve Linux firewall (örneğin, ufw veya iptables) ayarlarıyla sadece local ağdan erişim kısıtlanacak.//