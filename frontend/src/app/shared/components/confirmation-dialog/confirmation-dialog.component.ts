import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-confirmation-dialog',
  standalone: true,
  imports: [CommonModule, ButtonModule, DialogModule],
  template: `
    <p-dialog 
      [(visible)]="visible" 
      [header]="title" 
      [modal]="true" 
      [closable]="closable"
      [style]="{ width: '450px' }"
      [baseZIndex]="10000"
      [draggable]="false"
      [resizable]="false">
      
      <div class="confirmation-content">
        <i [class]="getIconClass()" [ngStyle]="{ color: getIconColor() }"></i>
        <span>{{ message }}</span>
      </div>
      
      <ng-template pTemplate="footer">
        <button 
          pButton 
          [label]="cancelLabel" 
          icon="pi pi-times" 
          (click)="onCancel()"
          [ngClass]="{ 'p-button-text': cancelButtonStyle === 'text', 'p-button-outlined': cancelButtonStyle === 'outlined' }"
          class="p-button-secondary"></button>
        <button 
          pButton 
          [label]="confirmLabel" 
          icon="pi pi-check" 
          (click)="onConfirm()"
          [ngClass]="{ 'p-button-text': confirmButtonStyle === 'text', 'p-button-outlined': confirmButtonStyle === 'outlined' }"
          [class]="'p-button-' + confirmButtonSeverity"></button>
      </ng-template>
    </p-dialog>
  `,
  styles: [`
    .confirmation-content {
      display: flex;
      align-items: center;
      justify-content: flex-start;
      margin: 0 0 20px 0;
    }
    
    .confirmation-content i {
      font-size: 2rem;
      margin-right: 16px;
    }
    
    :host ::ng-deep .p-dialog-footer {
      text-align: right;
      padding: 1.5rem;
      border-top: 1px solid #e9ecef;
    }
    
    :host ::ng-deep .p-dialog-footer button {
      margin-left: 8px;
    }
  `]
})
export class ConfirmationDialogComponent {
  @Input() visible: boolean = false;
  @Input() title: string = 'Onay';
  @Input() message: string = 'Bu işlemi gerçekleştirmek istediğinizden emin misiniz?';
  @Input() confirmLabel: string = 'Evet';
  @Input() cancelLabel: string = 'Hayır';
  @Input() closable: boolean = true;
  @Input() type: 'info' | 'warning' | 'error' | 'success' | 'question' = 'question';
  @Input() confirmButtonSeverity: 'primary' | 'secondary' | 'success' | 'info' | 'warning' | 'danger' = 'primary';
  @Input() confirmButtonStyle: 'filled' | 'outlined' | 'text' = 'filled';
  @Input() cancelButtonStyle: 'filled' | 'outlined' | 'text' = 'outlined';
  
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  @Output() visibleChange = new EventEmitter<boolean>();

  getIconClass(): string {
    switch (this.type) {
      case 'info':
        return 'pi pi-info-circle';
      case 'warning':
        return 'pi pi-exclamation-triangle';
      case 'error':
        return 'pi pi-times-circle';
      case 'success':
        return 'pi pi-check-circle';
      case 'question':
        return 'pi pi-question-circle';
      default:
        return 'pi pi-question-circle';
    }
  }

  getIconColor(): string {
    switch (this.type) {
      case 'info':
        return '#2196F3';
      case 'warning':
        return '#FFC107';
      case 'error':
        return '#F44336';
      case 'success':
        return '#4CAF50';
      case 'question':
        return '#607D8B';
      default:
        return '#607D8B';
    }
  }

  onConfirm(): void {
    this.confirm.emit();
    this.visible = false;
    this.visibleChange.emit(false);
  }

  onCancel(): void {
    this.cancel.emit();
    this.visible = false;
    this.visibleChange.emit(false);
  }
} 