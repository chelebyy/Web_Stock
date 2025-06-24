import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { TooltipModule } from 'primeng/tooltip';

export type ButtonVariant = 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light' | 'dark';
export type ButtonSize = 'small' | 'medium' | 'large';

/**
 * Uygulama genelinde tutarlı bir görünüm sağlayan button bileşeni.
 * PrimeNG Button bileşenini sarmalar ve ek özellikler ekler.
 */
@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule, ButtonModule, RippleModule, TooltipModule],
  template: `
    <button
      pButton
      pRipple
      [type]="submit ? 'submit' : 'button'"
      [label]="label"
      [icon]="icon"
      [iconPos]="iconPosition"
      [disabled]="disabled || loading"
      [loading]="loading"
      [class]="getButtonClass()"
      [pTooltip]="tooltip"
      [tooltipPosition]="tooltipPosition"
      (click)="onClick($event)"
    >
      <ng-content></ng-content>
    </button>
  `,
  styles: [`
    :host ::ng-deep {
      .p-button {
        border-radius: 0.375rem;
        transition: all 0.2s ease;
        
        &:not(:disabled):hover {
          transform: translateY(-2px);
          box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
        }
        
        &.p-button-sm {
          font-size: 0.75rem;
          padding: 0.25rem 0.5rem;
        }
        
        &.p-button-lg {
          font-size: 1.125rem;
          padding: 0.75rem 1.5rem;
        }
        
        &.p-button-primary {
          background-color: #3B82F6;
          border-color: #3B82F6;
        }
        
        &.p-button-secondary {
          background-color: #64748B;
          border-color: #64748B;
        }
        
        &.p-button-success {
          background-color: #10B981;
          border-color: #10B981;
        }
        
        &.p-button-danger {
          background-color: #EF4444;
          border-color: #EF4444;
        }
        
        &.p-button-warning {
          background-color: #F59E0B;
          border-color: #F59E0B;
          color: #ffffff;
        }
        
        &.p-button-info {
          background-color: #0EA5E9;
          border-color: #0EA5E9;
        }
        
        &.p-button-light {
          background-color: #F1F5F9;
          border-color: #E2E8F0;
          color: #334155;
        }
        
        &.p-button-dark {
          background-color: #1E293B;
          border-color: #1E293B;
        }
      }
    }
  `]
})
export class ButtonComponent {
  @Input() label = '';
  @Input() icon = '';
  @Input() iconPosition: 'left' | 'right' = 'left';
  @Input() disabled = false;
  @Input() loading = false;
  @Input() variant: ButtonVariant = 'primary';
  @Input() size: ButtonSize = 'medium';
  @Input() outlined = false;
  @Input() rounded = false;
  @Input() raised = false;
  @Input() text = false;
  @Input() submit = false;
  @Input() tooltip = '';
  @Input() tooltipPosition = 'top';
  
  @Output() buttonClick = new EventEmitter<Event>();
  
  /**
   * Button sınıflarını oluşturur
   */
  getButtonClass(): string {
    const classes = ['p-button'];
    
    // Variant
    classes.push(`p-button-${this.variant}`);
    
    // Size
    if (this.size === 'small') {
      classes.push('p-button-sm');
    } else if (this.size === 'large') {
      classes.push('p-button-lg');
    }
    
    // Style variants
    if (this.outlined) {
      classes.push('p-button-outlined');
    }
    
    if (this.rounded) {
      classes.push('p-button-rounded');
    }
    
    if (this.raised) {
      classes.push('p-button-raised');
    }
    
    if (this.text) {
      classes.push('p-button-text');
    }
    
    return classes.join(' ');
  }
  
  /**
   * Click olayını yönetir
   */
  onClick(event: Event): void {
    if (!this.disabled && !this.loading) {
      this.buttonClick.emit(event);
    }
  }
} 