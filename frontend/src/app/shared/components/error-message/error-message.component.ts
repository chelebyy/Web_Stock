import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="message" class="error-container" [ngClass]="type">
      <div class="error-icon">
        <i class="pi" [ngClass]="getIconClass()"></i>
      </div>
      <div class="error-content">
        <div class="error-title" *ngIf="title">{{ title }}</div>
        <div class="error-message">{{ message }}</div>
      </div>
      <div class="error-close" *ngIf="dismissible" (click)="onDismiss()">
        <i class="pi pi-times"></i>
      </div>
    </div>
  `,
  styles: [`
    .error-container {
      display: flex;
      align-items: flex-start;
      padding: 12px 16px;
      border-radius: 4px;
      margin-bottom: 16px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }
    
    .error {
      background-color: #ffebee;
      border-left: 4px solid #f44336;
    }
    
    .warning {
      background-color: #fff8e1;
      border-left: 4px solid #ffc107;
    }
    
    .info {
      background-color: #e3f2fd;
      border-left: 4px solid #2196f3;
    }
    
    .success {
      background-color: #e8f5e9;
      border-left: 4px solid #4caf50;
    }
    
    .error-icon {
      margin-right: 12px;
      font-size: 20px;
      display: flex;
      align-items: center;
      justify-content: center;
    }
    
    .error .error-icon {
      color: #f44336;
    }
    
    .warning .error-icon {
      color: #ffc107;
    }
    
    .info .error-icon {
      color: #2196f3;
    }
    
    .success .error-icon {
      color: #4caf50;
    }
    
    .error-content {
      flex: 1;
    }
    
    .error-title {
      font-weight: bold;
      margin-bottom: 4px;
    }
    
    .error-message {
      font-size: 14px;
    }
    
    .error-close {
      cursor: pointer;
      font-size: 16px;
      opacity: 0.7;
      transition: opacity 0.2s;
    }
    
    .error-close:hover {
      opacity: 1;
    }
  `]
})
export class ErrorMessageComponent {
  @Input() message: string = '';
  @Input() title: string = '';
  @Input() type: 'error' | 'warning' | 'info' | 'success' = 'error';
  @Input() dismissible: boolean = true;
  @Output() dismiss = new EventEmitter<void>();

  getIconClass(): string {
    switch (this.type) {
      case 'error':
        return 'pi-exclamation-circle';
      case 'warning':
        return 'pi-exclamation-triangle';
      case 'info':
        return 'pi-info-circle';
      case 'success':
        return 'pi-check-circle';
      default:
        return 'pi-exclamation-circle';
    }
  }

  onDismiss(): void {
    this.dismiss.emit();
  }
} 