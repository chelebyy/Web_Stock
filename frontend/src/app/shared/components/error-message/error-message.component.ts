import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [CommonModule, MessageModule],
  template: `
    <div class="error-message-container">
      <p-message
        *ngFor="let msg of messages"
        [severity]="msg.severity"
        [text]="msg.text"
        [closable]="msg.closable ?? true"
        [styleClass]="'mb-2'"
      ></p-message>
    </div>
  `,
  styles: [`
    .error-message-container {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
    }
    
    :host ::ng-deep {
      .p-message {
        width: 100%;
        margin-bottom: 0.5rem;
      }
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ErrorMessageComponent {
  /**
   * Gösterilecek hata mesajı veya mesajları.
   * Tek bir string veya PrimeNG Message[] formatında olabilir.
   */
  @Input() error: string | any[] | null = null;

  /**
   * PrimeNG Message[] formatına dönüştürülmüş mesajlar.
   */
  get messages(): any[] {
    if (!this.error) {
      return [];
    }
    
    if (typeof this.error === 'string') {
      return [{ 
        severity: 'error', 
        text: this.error 
      }];
    }
    
    return this.error.map(msg => {
      let text = '';
      
      // PrimeNG 19'da text özelliği kullanıldığı için
      // eski summary ve detail yapısını text'e dönüştürüyoruz
      if (msg.summary && msg.detail) {
        text = `${msg.summary}: ${msg.detail}`;
      } else if (msg.summary) {
        text = msg.summary;
      } else if (msg.detail) {
        text = msg.detail;
      }
      
      return {
        severity: msg.severity || 'error',
        text: text,
        closable: msg.closable ?? true,
        id: msg.id,
        key: msg.key,
        life: msg.life,
        sticky: msg.sticky,
        data: msg.data,
        icon: msg.icon
      };
    }).filter(m => m.text);
  }
} 