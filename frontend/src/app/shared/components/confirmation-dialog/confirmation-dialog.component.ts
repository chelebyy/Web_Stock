import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast'; // Onay/iptal mesajları için

@Component({
  selector: 'app-confirmation-dialog',
  standalone: true,
  imports: [CommonModule, ConfirmDialogModule, ToastModule],
  templateUrl: './confirmation-dialog.component.html',
  // Stil dosyası genellikle gerekmez, ConfirmDialog'un kendi stilleri yeterlidir.
  // styleUrls: ['./confirmation-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  // providers: [ConfirmationService, MessageService] // Servisleri buradan kaldırıyoruz.
})
export class ConfirmationDialogComponent {
  // Servisleri inject ile alıyoruz (Artık üst seviyeden sağlanacaklar)
  private readonly confirmationService = inject(ConfirmationService);
  private readonly messageService = inject(MessageService);

  // Bu bileşenin kendisi genellikle doğrudan bir işlev yapmaz.
  // ConfirmationService aracılığıyla diğer bileşenlerden tetiklenir.
  // Örnek Kullanım (Başka bir bileşende):
  // constructor(private confirmationService: ConfirmationService, private messageService: MessageService) {}
  //
  // confirmDelete(itemId: number) {
  //   this.confirmationService.confirm({
  //     message: 'Bu öğeyi silmek istediğinizden emin misiniz?',
  //     header: 'Silme Onayı',
  //     icon: 'pi pi-exclamation-triangle',
  //     acceptLabel: 'Evet, Sil',
  //     rejectLabel: 'Hayır',
  //     acceptButtonStyleClass: 'p-button-danger',
  //     rejectButtonStyleClass: 'p-button-text',
  //     accept: () => {
  //       // Silme işlemini gerçekleştir
  //       this.deleteItem(itemId);
  //       this.messageService.add({ severity: 'info', summary: 'Onaylandı', detail: 'Öğe başarıyla silindi.' });
  //     },
  //     reject: () => {
  //       this.messageService.add({ severity: 'error', summary: 'İptal Edildi', detail: 'Silme işlemi iptal edildi.' });
  //     }
  //   });
  // }
} 