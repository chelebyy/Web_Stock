# PowerShell'de && Operatörü Hatası

## Hata Mesajı

```
Token '&&' is not a valid statement separator in this version.
```

## Nedeni

PowerShell'de, Linux/Unix kabuklarında veya Windows Command Prompt'ta (cmd.exe) kullanılan `&&` operatörü desteklenmemektedir. Bu operatör, bir komut başarılı olduğunda ikinci bir komutu çalıştırmak için kullanılır.

Örneğin, aşağıdaki komut PowerShell'de çalışmayacaktır:

```powershell
cd frontend && ng serve
```

## Çözüm

PowerShell'de komutları ayrı ayrı çalıştırmak gerekiyor:

```powershell
cd frontend
ng serve
```

Alternatif olarak, PowerShell'de komutları birleştirmek için aşağıdaki yöntemler kullanılabilir:

1. Noktalı virgül (;) kullanarak:
```powershell
cd frontend; ng serve
```

2. PowerShell'in pipeline operatörünü kullanarak:
```powershell
cd frontend | Out-Null; ng serve
```

3. Bir PowerShell fonksiyonu veya script bloğu kullanarak:
```powershell
& { cd frontend; ng serve }
```

## Öğrenilen Ders

1. Farklı kabuk ortamlarında (bash, cmd, PowerShell) komut sözdizimi farklılık gösterebilir.
2. PowerShell'de komutları birleştirmek için `&&` yerine `;` kullanılmalıdır.
3. Platformlar arası uyumluluk için, komut dosyalarında platform özelliklerini dikkate almak önemlidir.

## Uygulama

Bu hata özellikle Angular uygulamasını çalıştırırken karşımıza çıktı. Frontend klasörüne gidip ng serve komutunu çalıştırmak için genellikle `cd frontend && ng serve` komutu kullanılır, ancak PowerShell'de bu komut çalışmaz.

Bunun yerine:

```powershell
cd frontend
ng serve
```

şeklinde iki ayrı komut olarak çalıştırmak gerekir. 