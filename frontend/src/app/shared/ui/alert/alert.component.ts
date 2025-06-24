import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';

interface Message {
  severity: string;
  summary?: string;
  detail?: string;
  id?: any;
  key?: string;
  life?: number;
  sticky?: boolean;
  closable?: boolean;
  data?: any;
}

export type AlertSeverity = 'success' | 'info' | 'warn' | 'error';

/**
 * Uygulama genelinde tutarlı bir görünüm sağlayan uyarı bileşeni.
 * PrimeNG Message bileşenini sarmalar ve ek özellikler ekler.
 */
@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [CommonModule, MessagesModule, MessageModule],
  template: `
    <div class="alert-container" [ngClass]="{'alert-closable': closable}">
      <p-messages 
        [(value)]="messages" 
        [closable]="closable"
        [styleClass]="getAlertClass()"
      ></p-messages>
    </div>
  `,
  styles: [`
    :host {
      display: block;
      margin-bottom: 1rem;
    }
    
    .alert-container {
      width: 100%;
    }
    
    :host ::ng-deep {
      .p-message {
        margin: 0;
        border-radius: 0.375rem;
        
        &.alert-outline {
          background-color: transparent;
          
          &.p-message-success {
            color: #10B981;
            border: 1px solid #10B981;
          }
          
          &.p-message-info {
            color: #0EA5E9;
            border: 1px solid #0EA5E9;
          }
          
          &.p-message-warn {
            color: #F59E0B;
            border: 1px solid #F59E0B;
          }
          
          &.p-message-error {
            color: #EF4444;
            border: 1px solid #EF4444;
          }
        }
        
        &.alert-filled {
          &.p-message-success {
            background-color: #10B981;
            border-color: #10B981;
            color: #ffffff;
          }
          
          &.p-message-info {
            background-color: #0EA5E9;
            border-color: #0EA5E9;
            color: #ffffff;
          }
          
          &.p-message-warn {
            background-color: #F59E0B;
            border-color: #F59E0B;
            color: #ffffff;
          }
          
          &.p-message-error {
            background-color: #EF4444;
            border-color: #EF4444;
            color: #ffffff;
          }
        }
      }
      
      .p-message-wrapper {
        padding: 1rem;
      }
      
      .p-message-icon {
        font-size: 1.25rem;
      }
      
      .p-message-summary {
        font-weight: 600;
      }
      
      .p-message-detail {
        margin-top: 0.25rem;
      }
    }
  `]
})
export class AlertComponent implements OnInit {
  @Input() severity: AlertSeverity = 'info';
  @Input() summary = '';
  @Input() detail = '';
  @Input() closable = false;
  @Input() outline = false;
  @Input() filled = false;
  
  messages: Message[] = [];
  
  ngOnInit() {
    this.messages = [
      {
        severity: this.severity,
        summary: this.summary,
        detail: this.detail
      }
    ];
  }
  
  /**
   * Alert sınıflarını oluşturur
   */
  getAlertClass(): string {
    const classes = [];
    
    if (this.outline) {
      classes.push('alert-outline');
    }
    
    if (this.filled) {
      classes.push('alert-filled');
    }
    
    return classes.join(' ');
  }
} 