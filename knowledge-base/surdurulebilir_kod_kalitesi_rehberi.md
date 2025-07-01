---
description: Proje genelinde sürdürülebilir kod kalitesi ve yeni özellik geliştirme standartlarını tanımlar.
globs: ["src/**/*.{cs,ts,html}", "knowledge-base/**/*.md"]
alwaysApply: true
---

# Sürdürülebilir Kod Kalitesi ve Geliştirme Rehberi

Bu belge, Stock projesinde kod kalitesini sürekli olarak yüksek tutmak ve yeni eklenecek özelliklerin mevcut mimari standartlara uygun olmasını sağlamak için bir yol haritası sunar. Kalite, üç temel prensip üzerine inşa edilmiştir: Yerleşik Mimari Desenler, Otomatikleştirilmiş Araçlar ve Geliştirme Süreçleri.

---

## 1. Yerleşik Mimari Desenler (Takip Edilecek "Blueprint")

Projemiz, yeni özelliklerin üzerine inşa edileceği sağlam ve kanıtlanmış mimari desenlere sahiptir. Yeni eklenecek her özellik, bu mevcut "blueprint"leri (taslakları) takip etmelidir. Bu, kaliteyi ve tutarlılığı doğal olarak getirecektir.

### Yeni Bir Backend Özelliği Ekleme (Örn: "Bilgi İşlem")

Yeni bir backend özelliği eklenirken izlenecek yol haritası tam olarak şudur:

1.  **Controller Oluşturma:** `Stock.API` projesi içine `BilgiIslemController.cs` adında yeni bir controller eklenir. Bu controller, dış dünyadan gelen istekleri karşılar ve ilgili komut/sorguları tetikler.
2.  **CQRS Yapısını Kurma:** `Stock.Application/Features` altına `BilgiIslem` adında yeni bir klasör oluşturulur.
3.  **Komut ve Sorguları Tanımlama:** Bu klasörün içine, yapılacak işlere göre `Commands` ve `Queries` alt klasörleri açılır. Örneğin:
    *   `Features/BilgiIslem/Commands/CreateTicket/CreateTicketCommand.cs`
    *   `Features/BilgiIslem/Queries/GetTicketById/GetTicketByIdQuery.cs`
4.  **İş Mantığını Handler'da Yazma:** Her komut (Command) ve sorgunun (Query) kendi `Handler` sınıfı olur. Bu Handler'lar, iş mantığını uygulamak için `IUnitOfWork` ve `IMapper` gibi soyutlanmış servisleri kullanır. **Veritabanına asla doğrudan `DbContext` üzerinden erişilmemelidir.** Repository ve Unit of Work desenleri zorunludur.
5.  **Domain Mantığını Koruma:** Eğer "Ticket" (servis talebi) gibi yeni bir domain varlığı (Entity) oluşturulacaksa, bu `Stock.Domain/Entities` altında tanımlanır. Bu varlık, kendi iş kurallarını (örneğin bir ticket'ın başlığı boş olamaz gibi) ve durumlarını kendi içinde barındırmalıdır (`Result` deseni ve `ValueObject`'ler kullanılarak).

### Yeni Bir Frontend Özelliği Ekleme

1.  **Modül ve Rota Yapısı:** Yeni özellik `frontend/src/app/features/` altında kendi modülü olarak oluşturulur (`bilgi-islem` gibi). Bu modül, `app.routes.ts` içinde `lazy-loading` (tembel yükleme) ile yüklenmelidir.
2.  **Bileşen (Component) Mimarisi:**
    *   **Akıllı (Smart) Bileşenler:** Sayfa seviyesindeki bileşenler, servislerden veri çekme ve state yönetimi gibi işlerden sorumludur.
    *   **Sunumsal (Dumb) Bileşenler:** Tekrar kullanılabilir, sadece `@Input()` ile veri alıp `@Output()` ile olay yayan daha küçük UI bileşenleridir. Bu bileşenler `shared/components` altında yer alabilir.
3.  **Servisler:** API ile iletişime geçecek servisler, özelliğin kendi klasörü içinde veya `core/services` altında oluşturulur ve `BaseHttpService`'i miras almalıdır.
4.  **State Yönetimi:** Basit durumlar için `signal` tabanlı servisler, daha karmaşık senaryolar için ise standart state management kütüphaneleri (NgRx vb.) desenleri takip edilebilir.

---

## 2. Otomatikleştirilmiş Araçlar (Kaliteyi Zorunlu Kılan "Bekçiler")

İnsan hatasını en aza indirmek ve standartları otomatik olarak zorunlu kılmak için aşağıdaki araçlardan yararlanılır:

*   **.NET Analyzers (Roslyn):**
    *   **StyleCop.Analyzers:** Kod formatlama ve isimlendirme standartlarını zorunlu kılar.
    *   **SonarAnalyzer.CSharp:** Güvenlik açıkları, bug'lar ve "code smell" (kötü kod kokuları) için kodu sürekli tarar.
*   **ESLint (Frontend):** TypeScript kod standartlarını, formatlamayı ve potansiyel hataları yakalar.
*   **Build Sürecine Entegrasyon:** Bu analiz araçları, projenin build sürecine entegre edilmelidir. Eğer kod, belirlenen kalite kurallarını geçemezse, projenin build olması engellenmelidir. Bu, kalitesiz kodun ana kod tabanına karışmasını en başından önler.

---

## 3. Süreç ve Disiplin (Geliştirme "Kültürü")

Araçlar ve desenler, ancak doğru bir geliştirme kültürüyle birleştiğinde anlam kazanır.

*   **Kod Gözden Geçirme (Code Review):**
    *   Her yeni özellik veya değişiklik, bir **Pull Request (PR)** ile ana dala (`main` veya `develop`) birleştirilmeden önce takımın en az bir başka üyesi tarafından gözden geçirilmelidir.
    *   Gözden geçirme sırasında sorulacak temel sorular:
        1.  "Bu kod, yerleşik mimari desenlerimize (CQRS, Repository vb.) uyuyor mu?"
        2.  "Yeni 'magic string'ler veya konfigüre edilmemiş sayılar eklenmiş mi?"
        3.  "Hata yönetimi ve loglama doğru ve yeterli bir şekilde yapılmış mı?"
        4.  "Yazılan kod için birim (unit) veya entegrasyon testleri eklenmiş mi?"
*   **Dokümantasyon:**
    *   Yeni bir özellik eklendiğinde, `knowledge-base/feature_modules` altına özelliğin mimarisini, kararlarını ve kullanımını anlatan kısa bir belge (`bilgi_islem_module.md` gibi) eklenmelidir.
    *   Bu, projenin bilgi birikimini artırır ve gelecekteki geliştiricilere yol gösterir.
*   **Yapay Zeka Asistanı (Benim Rolüm):**
    *   Benden yeni bir özellik istendiğinde, bu belgede tanımlanan mimari desenlere ve kalite standartlarına harfiyen uyarak kod üreteceğim. Bu kuralların projedeki en önemli takipçilerinden biri olacağım.