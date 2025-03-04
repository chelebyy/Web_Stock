# Web_Stock Repo Komutları

## Temel Git Komutları

```powershell
# Repoyu klonla
git clone https://github.com/chelebyy/Web_Stock.git

# Değişiklikleri çek
git pull

# Değişiklikleri gönder
git add .
git commit -m "Mesaj"
git push
```

## Ev ve İşyeri Arasında Senkronizasyon

Farklı lokasyonlarda çalışırken projenin güncel kalması için:

### Her Çalışmaya Başlarken
```powershell
# Önce güncel değişiklikleri çek
git pull origin master
```

### Her Çalışma Bitiminde
```powershell
# Değişiklikleri kaydet ve gönder
git add .
git commit -m "Lokasyon: [Ev/İş] - Yapılan değişiklikler"
git push origin master
```

### Çakışma Olursa
```powershell
# Çakışmaları çöz ve devam et
git pull --rebase origin master
# Çakışmaları düzelt
git add .
git rebase --continue
git push origin master --force
```

> Not: `--force` parametresini dikkatli kullanın, sadece kendi değişikliklerinizi ezerken kullanın.

Bu değişikliği GitHub reponuza göndermek için aşağıdaki komutları kullanabilirsiniz:

```powershell
git add git_sync.md
git commit -m "git_sync.md dosyası basitleştirildi"
git push origin master 