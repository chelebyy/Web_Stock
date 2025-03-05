# Uygulama Başlığı Güncelleme

## Yapılan Değişiklikler

Tarayıcı sekmesinde görünen uygulama başlığı "Frontend" yerine "Bilişim Sistemi" olarak değiştirildi.

**Değiştirilen Dosya**: `frontend/src/index.html`

**Değişiklik**:
```diff
<head>
  <meta charset="utf-8">
-  <title>Frontend</title>
+  <title>Bilişim Sistemi</title>
  <base href="/">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="icon" type="image/x-icon" href="favicon.ico">
</head>
```

## Teknik Detaylar

Angular uygulamalarında sayfa başlığı genellikle şu şekillerde belirlenir:

1. `index.html` dosyasında `<title>` etiketi içinde - Bu değişiklikte kullanılan yöntem
2. Angular router üzerinden `title` özelliği ile sayfa bazında 
3. Angular `Title` servisini kullanarak programatik olarak

Bu güncelleme sonrasında, kullanıcılar tarayıcı sekmesinde "Frontend" yerine "Bilişim Sistemi" yazısını göreceklerdir.

## Testler

- Tarayıcıda uygulama çalıştırıldığında sekme başlığının "Bilişim Sistemi" olarak göründüğünden emin olunmalıdır.
- Farklı sayfalara geçişlerde başlık değişikliğini koruduğu kontrol edilmelidir.

## Öğrenilen Dersler

- Angular uygulamalarında uygulama başlığı değişikliği için birden fazla yöntem vardır.
- Basit başlık değişiklikleri için `index.html` dosyasındaki `<title>` etiketini düzenlemek yeterlidir.
- Dinamik başlık değişiklikleri için Angular'ın `Title` servisi kullanılabilir.

## İlgili Dosyalar

- frontend/src/index.html 