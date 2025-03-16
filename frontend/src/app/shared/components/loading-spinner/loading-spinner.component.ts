import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="spinner-container" [ngClass]="{'overlay': overlay}">
      <div class="spinner" [ngClass]="size">
        <div class="bounce1" [ngStyle]="{'background-color': color}"></div>
        <div class="bounce2" [ngStyle]="{'background-color': color}"></div>
        <div class="bounce3" [ngStyle]="{'background-color': color}"></div>
      </div>
      <div *ngIf="message" class="spinner-message">{{ message }}</div>
    </div>
  `,
  styles: [`
    .spinner-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
    }
    
    .overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(255, 255, 255, 0.7);
      z-index: 9999;
    }
    
    .spinner {
      margin: 0 auto;
      width: 70px;
      text-align: center;
    }
    
    .spinner > div {
      width: 18px;
      height: 18px;
      background-color: #3498db;
      border-radius: 100%;
      display: inline-block;
      animation: sk-bouncedelay 1.4s infinite ease-in-out both;
    }
    
    .spinner .bounce1 {
      animation-delay: -0.32s;
    }
    
    .spinner .bounce2 {
      animation-delay: -0.16s;
    }
    
    .small > div {
      width: 10px;
      height: 10px;
    }
    
    .large > div {
      width: 24px;
      height: 24px;
    }
    
    .spinner-message {
      margin-top: 10px;
      font-size: 14px;
      color: #333;
    }
    
    @keyframes sk-bouncedelay {
      0%, 80%, 100% { 
        transform: scale(0);
      } 40% { 
        transform: scale(1.0);
      }
    }
  `]
})
export class LoadingSpinnerComponent {
  @Input() color: string = '#3498db';
  @Input() size: 'small' | 'medium' | 'large' = 'medium';
  @Input() overlay: boolean = false;
  @Input() message: string = '';
} 