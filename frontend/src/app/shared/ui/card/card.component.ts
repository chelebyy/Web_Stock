import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';

/**
 * Uygulama genelinde tutarlı bir görünüm sağlayan kart bileşeni.
 * PrimeNG Card bileşenini sarmalar ve ek özellikler ekler.
 */
@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule, CardModule],
  template: `
    <p-card 
      [header]="header" 
      [subheader]="subheader"
      [style]="getCardStyle()"
      [styleClass]="getCardClass()"
    >
      <ng-template pTemplate="header" *ngIf="headerTemplate">
        <ng-content select="[card-header]"></ng-content>
      </ng-template>
      
      <ng-content></ng-content>
      
      <ng-template pTemplate="footer" *ngIf="footerTemplate">
        <ng-content select="[card-footer]"></ng-content>
      </ng-template>
    </p-card>
  `,
  styles: [`
    :host ::ng-deep {
      .p-card {
        border-radius: 0.5rem;
        box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
        transition: all 0.3s ease;
        
        &:hover {
          box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
        }
        
        .p-card-header {
          padding: 1.25rem 1.5rem;
          border-bottom: 1px solid #e2e8f0;
          font-weight: 600;
        }
        
        .p-card-body {
          padding: 1.5rem;
        }
        
        .p-card-content {
          padding: 0;
        }
        
        .p-card-footer {
          padding: 1.25rem 1.5rem;
          border-top: 1px solid #e2e8f0;
        }
        
        &.card-flat {
          box-shadow: none;
          border: 1px solid #e2e8f0;
          
          &:hover {
            box-shadow: none;
          }
        }
        
        &.card-hoverable:hover {
          transform: translateY(-5px);
        }
        
        &.card-primary {
          border-top: 3px solid #3B82F6;
        }
        
        &.card-success {
          border-top: 3px solid #10B981;
        }
        
        &.card-warning {
          border-top: 3px solid #F59E0B;
        }
        
        &.card-danger {
          border-top: 3px solid #EF4444;
        }
        
        &.card-info {
          border-top: 3px solid #0EA5E9;
        }
      }
    }
  `]
})
export class CardComponent {
  @Input() header = '';
  @Input() subheader = '';
  @Input() headerTemplate = false;
  @Input() footerTemplate = false;
  @Input() flat = false;
  @Input() hoverable = false;
  @Input() width = '';
  @Input() height = '';
  @Input() variant: 'primary' | 'success' | 'warning' | 'danger' | 'info' | 'none' = 'none';
  
  /**
   * Kart sınıflarını oluşturur
   */
  getCardClass(): string {
    const classes = [];
    
    if (this.flat) {
      classes.push('card-flat');
    }
    
    if (this.hoverable) {
      classes.push('card-hoverable');
    }
    
    if (this.variant !== 'none') {
      classes.push(`card-${this.variant}`);
    }
    
    return classes.join(' ');
  }
  
  /**
   * Kart stillerini oluşturur
   */
  getCardStyle(): any {
    const style: any = {};
    
    if (this.width) {
      style.width = this.width;
    }
    
    if (this.height) {
      style.height = this.height;
    }
    
    return style;
  }
} 