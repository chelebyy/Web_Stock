## SpecStory

## Cursor Rules CLI Kullanım Rehberi (3/9/2025, 1:01:37 AM)

_**User**_

@https://github.com/sanjeed5/awesome-cursor-rules-mdc/blob/main/cursor-rules-cli/README.md  Linki incelle ve nasıl kullanabilirim beni yönlendir. Windows cihaz kullanıyorum

---

_**Assistant**_

# Cursor Rules CLI Kullanım Kılavuzu

Cursor Rules CLI, projeniz için uygun Cursor kurallarını otomatik olarak bulup yüklemenize yardımcı olan kullanışlı bir araçtır. Projenizi tarayarak kullandığınız kütüphaneleri ve çerçeveleri tespit eder ve bunlara uygun kuralları önerir.

## Cursor Rules CLI Nedir?

Bu araç şunları yapabilir:
- 🔍 Projenizdeki kütüphaneleri otomatik olarak tespit eder
- 📝 Doğrudan kütüphane belirtmeyi destekler
- 📥 Kuralları indirip Cursor'a yükler
- 🎨 Renkli, kullanıcı dostu bir arayüz sunar
- 🔀 Özel kural depolarıyla çalışır
- 🔒 %100 gizlilik odaklı (tüm tarama işlemleri yerel olarak gerçekleşir)

## Windows'ta Kurulum

Windows cihazınızda Cursor Rules CLI'yi kurmak için şu adımları izleyin:

1. Python'un yüklü olduğundan emin olun. Yüklü değilse [Python'un resmi sitesinden](https://www.python.org/downloads/) indirebilirsiniz.

2. PowerShell veya Komut İstemi açın ve şu komutu çalıştırın:
   ```
   pip install cursor-rules
   ```

## Temel Kullanım

PowerShell veya Komut İstemi'nde şu komutları kullanabilirsiniz:

1. Mevcut projeyi tarayıp eşleşen kuralları yüklemek için:
   ```
   cursor-rules
   ```

2. Kütüphaneleri doğrudan belirtmek için (proje taramasını atlar):
   ```
   cursor-rules --libraries "react,tailwind,typescript"
   ```

3. Belirli bir proje dizinini taramak için:
   ```
   cursor-rules -d C:\path\to\your\project
   ```

## Yaygın Seçenekler

- `--dry-run`: Herhangi bir şey yüklemeden önizleme yapın
- `--force`: Mevcut kuralları değiştirin
- `-v, --verbose`: Ayrıntılı çıktı gösterin
- `--quick-scan`: Daha hızlı tarama (yalnızca paket dosyalarını kontrol eder)
- `--max-results N`: En iyi N sonucu gösterin (varsayılan: 20)

## Özel Depolar

Kendi kural deponuzu kullanmak için:
```
cursor-rules --custom-repo kullanici-adiniz/depo-adiniz
```

Depo ayarını gelecekteki kullanımlar için kaydetmek için:
```
cursor-rules --custom-repo kullanici-adiniz/depo-adiniz --save-config
```

## Yapılandırma

Mevcut ayarları görüntülemek için:
```
cursor-rules --show-config
```

Ayarları global olarak kaydetmek için:
```
cursor-rules --save-config
```

Ayarları yalnızca mevcut proje için kaydetmek için:
```
cursor-rules --save-project-config
```

## Tüm Seçenekler

Tüm mevcut seçenekleri görmek için:
```
cursor-rules --help
```

Bu araç, Cursor editörünüzü daha verimli kullanmanıza yardımcı olacak kuralları otomatik olarak bulup yükleyerek geliştirme sürecinizi iyileştirebilir. Projenizin ihtiyaçlarına göre en uygun kuralları bulmanıza yardımcı olur.

---



---

