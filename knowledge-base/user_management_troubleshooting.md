# Kullanıcı Yönetimi Sayfası Sorun Giderme

## İzinler ve Ad Soyad Görüntüleme Sorunu

### Sorun
Kullanıcı yönetimi sayfasında kullanıcıların ad-soyad bilgileri ve izinleri doğru şekilde görüntülenmiyordu. Ayrıca, Angular reactive form'da disabled özelliği ile ilgili bir uyarı mesajı vardı.

### Hata Mesajı
```
It looks like you're using the disabled attribute with a reactive form directive. If you set disabled to true
when you set up this control in your component class, the disabled attribute will actually be set in the DOM for
you. We recommend using this approach to avoid 'changed after checked' errors.
```

### Nedeni
1. Kullanıcı verilerinin backend'den alınması ve frontend'de gösterilmesi sırasında veri dönüşümü eksikti:
   - Backend'den gelen `username` bilgisi, `fullName` alanına atanmamıştı
   - `permissions` alanı doğru şekilde doldurulmamıştı
   
2. ReactiveForm içindeki disabled özelliği HTML üzerinden `[disabled]` niteliği ile değil, component sınıfı içinden kontrol edilmesi gerekiyordu.

### Çözüm

1. **Veri dönüşümü düzeltmesi:**
   - `loadUsers()` metodu güncellendi:
   ```typescript
   loadUsers() {
     this.userService.getUsers().subscribe({
       next: (data) => {
         this.users = data.map(user => {
           return {
             id: user.id,
             fullName: user.username, // Kullanıcı adını fullName alanına at
             sicil: user.sicil || `N/A-${user.id}`,
             permissions: user.isAdmin ? 'Admin' : (user.roleName || 'Kullanıcı'),
             roleId: user.roleId || (user.isAdmin ? 1 : 2),
             isAdmin: user.isAdmin,
             isActive: true,
             createdAt: user.createdAt || new Date()
           };
         });
         
         this.filteredUsers = [...this.users];
         this.updatePagination();
       },
       // ...
     });
   }
   ```

2. **ReactiveForm disabled özelliği düzeltmesi:**
   - HTML'den `[disabled]` kaldırıldı:
   ```html
   <p-dropdown [options]="roles" formControlName="roleId" placeholder="Rol seçin" 
     styleClass="w-full" optionLabel="label" optionValue="value">
   </p-dropdown>
   ```

   - Component sınıfında formun oluşturulması değiştirildi:
   ```typescript
   initForm() {
     this.userForm = this.formBuilder.group({
       // ...
       roleId: [{value: 2, disabled: false}] // Varsayılan Kullanıcı rol ID'si
     });
     
     // isAdmin değiştiğinde roleId alanını etkileme
     this.userForm.get('isAdmin')?.valueChanges.subscribe(isAdmin => {
       const roleIdControl = this.userForm.get('roleId');
       
       if (isAdmin) {
         roleIdControl?.patchValue(1, {emitEvent: false});
         roleIdControl?.disable({emitEvent: false});
       } else {
         roleIdControl?.enable({emitEvent: false});
         if (roleIdControl?.value === 1) {
           roleIdControl?.patchValue(2, {emitEvent: false});
         }
       }
     });
   }
   ```

   - Form gönderiminde `getRawValue()` kullanımı eklendi:
   ```typescript
   saveUser() {
     this.submitted = true;

     if (this.userForm.invalid) {
       return;
     }

     const userData = {...this.userForm.getRawValue()}; // getRawValue kullanarak disabled alanları da al
     
     // ...
   }
   ```

## Öğrenilen Dersler

1. **Angular ReactiveForm'da disabled durumu yönetimi:**
   - HTML'de `[disabled]` niteliği yerine component sınıfında `FormControl` oluşturulurken `{value: value, disabled: true/false}` şeklinde yapılandırılmalı veya `disable()/enable()` metodları kullanılmalı.
   - Eğer değerler arasında bağımlılık varsa, `valueChanges` ile dinlemek ve `disable()/enable()` metotlarını kullanmak doğru yaklaşımdır.
   - Form gönderilirken, disabled alanları da almak için `form.value` yerine `form.getRawValue()` kullanılmalı.

2. **Backend/Frontend veri dönüşümü:**
   - API'den alınan verilerin doğru şekilde haritalanması ve dönüştürülmesi önemlidir.
   - Kullanıcı verilerinin işlenmesi sırasında field isimlerinin doğru eşleştirilmesi gerekir.
   - Olmayan veriler için varsayılan değerler atanması, frontend'de hataları önlemeye yardımcı olur.

3. **Verilerin görüntülenmesi için kontrol:**
   - API'den alınan verilerin doğru şekilde işlenip işlenmediği konsola geçici log'lar eklenerek test edilmeli.
   - Veri akışının her adımının doğruluğu kontrol edilmeli ve her adım için uygun hata yakalama eklenmelidir. 