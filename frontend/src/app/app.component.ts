import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { PrimeNG } from 'primeng/config';
import { definePreset } from '@primeng/themes';
import Lara from '@primeng/themes/lara';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [
        CommonModule, 
        RouterOutlet,
        ToastModule,
        ConfirmDialogModule
    ],
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  private primeng = inject(PrimeNG);
  title = 'Stock Management';

  ngOnInit() {
    // PrimeNG 19 tema konfigürasyonu
    const MyPreset = definePreset(Lara, {
      semantic: {
        primary: {
          50: '#f0f9ff',
          100: '#e0f2fe',
          200: '#bae6fd',
          300: '#7dd3fc',
          400: '#38bdf8',
          500: '#0ea5e9',
          600: '#0284c7',
          700: '#0369a1',
          800: '#075985',
          900: '#0c4a6e',
          950: '#082f49'
        }
      }
    });
    
    this.primeng.theme.set({
      preset: MyPreset
    });
  }
}
