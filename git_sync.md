# GitHub'dan Güncel Verileri Çekme ve Senkronizasyon Rehberi

Bu rehber, başka bir GitHub deposundan güncel verileri çekme ve mevcut projenizi güncellemek için kullanabileceğiniz adımları içermektedir.

## İçindekiler
1. [Gereksinimler](#gereksinimler)
2. [Uzak Repo Ekleme](#uzak-repo-ekleme)
3. [Güncel Verileri Çekme](#güncel-verileri-çekme)
4. [Çakışmaları Çözme](#çakışmaları-çözme)
5. [Değişiklikleri Push Etme](#değişiklikleri-push-etme)
6. [Otomatik Senkronizasyon](#otomatik-senkronizasyon)
7. [Sık Karşılaşılan Sorunlar](#sık-karşılaşılan-sorunlar)

## Gereksinimler
- Git terminal komutlarına erişim
- GitHub hesabı
- Uzak repoya erişim izinleri
- Git'in yerel makinenize kurulu olması

## Uzak Repo Ekleme

### 1. Mevcut Reponuzu Kontrol Edin
```powershell
git remote -v
```

Bu komut, mevcut uzak repoları listeler. Genellikle `origin` adında bir uzak repo görürsünüz.

### 2. Senkronize Etmek İstediğiniz Repoyu Ekleyin
```powershell
git remote add upstream https://github.com/kullaniciadi/repo-adi.git
```

Burada `upstream` eklediğiniz yeni uzak reponun takma adıdır. Dilerseniz farklı bir isim kullanabilirsiniz.

### 3. Uzak Repoları Tekrar Kontrol Edin
```powershell
git remote -v
```

Artık hem `origin` hem de `upstream` repoları listelenmelidir.

## Güncel Verileri Çekme

### 1. Yerel Değişiklikleri Commit Edin
```powershell
git add .
git commit -m "Yerel değişiklikler"
```

### 2. Upstream Repodan Güncel Veriyi Çekin
```powershell
git fetch upstream
```

Bu komut, upstream repodaki tüm değişiklikleri yerel reponuza indirir ancak henüz birleştirmez.

### 3. Ana Branch'i Seçin
```powershell
git checkout main
```

Burada `main` ana branch adıdır. Reponuzda farklı bir isim (örneğin `master`) kullanılıyorsa o ismi kullanın.

### 4. Upstream Değişikliklerini Birleştirin
```powershell
git merge upstream/main
```

Bu komut, upstream'den çektiğiniz değişiklikleri yerel main branch'inize birleştirir.

## Çakışmaları Çözme

### 1. Çakışmaları Tespit Edin
Merge işlemi sırasında çakışmalar olabilir. Git, bu çakışmaları dosyalarda işaretleyecektir.

### 2. Çakışmaları Düzenleyin
Çakışma olan dosyaları bir metin editöründe açın. Çakışmalar şu şekilde işaretlenir:

```
<<<<<<< HEAD
Yerel değişiklikler
=======
Upstream değişiklikler
>>>>>>> upstream/main
```

Bu işaretler arasındaki metni düzenleyerek çakışmaları çözün.

### 3. Çözülen Çakışmaları Commit Edin
```powershell
git add .
git commit -m "Çakışmalar çözüldü"
```

## Değişiklikleri Push Etme

### 1. Değişiklikleri Origin Reponuza Push Edin
```powershell
git push origin main
```

## Otomatik Senkronizasyon

Projenizi düzenli olarak güncellemek için bir PowerShell scripti oluşturabilirsiniz:

```powershell
# sync_repo.ps1
cd $PSScriptRoot
git fetch upstream
git checkout main
git merge upstream/main
git push origin main
Write-Host "Senkronizasyon tamamlandı!" -ForegroundColor Green
```

Bu scripti çalıştırmak için:
```powershell
.\sync_repo.ps1
```

## Sık Karşılaşılan Sorunlar

### "fatal: 'upstream' does not appear to be a git repository"
Bu hata, upstream repo yanlış eklendiğinde oluşur. Şu komutu kullanarak düzeltebilirsiniz:
```powershell
git remote remove upstream
git remote add upstream https://github.com/kullaniciadi/repo-adi.git
```

### "error: failed to push some refs to 'origin'"
Bu hata, yerel reponuzda bulunmayan değişiklikler origin repoda varsa oluşur. Önce pull işlemi yapın:
```powershell
git pull origin main
```

### "error: Your local changes would be overwritten by merge"
Bu hata, değişikliklerinizi commit etmeden merge yapmaya çalıştığınızda oluşur. Önce değişikliklerinizi commit edin veya stash yapın:
```powershell
git stash
git merge upstream/main
git stash pop
```

### "Authentication failed"
Bu hata, GitHub kimlik doğrulaması başarısız olduğunda oluşur. GitHub'da kişisel erişim token'ı oluşturun ve bunu kullanın veya SSH anahtarı ayarlayın.

### "You have divergent branches"
Bu hata, hem yerel hem de uzak repoda farklı değişiklikler olduğunda oluşur. Rebase kullanabilirsiniz:
```powershell
git pull --rebase upstream main
```

## Sonuç

Bu adımları izleyerek, başka bir GitHub reposundan güncel verileri çekebilir ve projenizi güncel tutabilirsiniz. Düzenli olarak senkronizasyon yapmak, projenizin güncel kalmasını ve diğer geliştiricilerle sorunsuz işbirliği yapmanızı sağlar. 