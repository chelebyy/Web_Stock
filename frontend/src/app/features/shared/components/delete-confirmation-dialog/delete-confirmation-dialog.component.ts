import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-delete-confirmation-dialog',
  standalone: true,
  imports: [CommonModule, DialogModule, ButtonModule],
  templateUrl: './delete-confirmation-dialog.component.html',
  styleUrls: ['./delete-confirmation-dialog.component.scss']
})
export class DeleteConfirmationDialogComponent {
  @Input() visible = false;
  @Input() itemName = '';
  @Input() title = 'Silme Onayı';
  
  // Geriye dönük uyumluluk için ekstra özellikler
  @Input() headerText = ''; // headerText'i ekle
  @Input() message = ''; // message'ı ekle
  
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  
  // Alternatif olay emitterleri (geriye dönük uyumluluk için)
  @Output() onConfirm = new EventEmitter<void>();
  @Output() onCancel = new EventEmitter<void>();

  get dialogTitle(): string {
    return this.headerText || this.title;
  }
  
  get dialogMessage(): string {
    return this.message || '';
  }

  onHide() {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  onConfirmClick() {
    this.confirm.emit();
    this.onConfirm.emit(); // Geriye dönük uyumluluk için eski eventi de çağır
    this.onHide();
  }

  onCancelClick() {
    this.cancel.emit();
    this.onCancel.emit(); // Geriye dönük uyumluluk için eski eventi de çağır
    this.onHide();
  }
}
