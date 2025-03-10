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
  @Input() visible: boolean = false;
  @Input() itemName: string = '';
  @Input() title: string = 'Silme Onayı';
  
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onHide() {
    this.visible = false;
    this.visibleChange.emit(false);
  }

  onConfirm() {
    this.confirm.emit();
    this.onHide();
  }

  onCancel() {
    this.cancel.emit();
    this.onHide();
  }
}
