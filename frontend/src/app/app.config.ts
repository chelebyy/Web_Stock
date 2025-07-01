import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { routes } from './app.routes';

import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { JwtModule } from '@auth0/angular-jwt';

// PrimeNG 19 tema import'ları
import { providePrimeNG } from 'primeng/config';
import Lara from '@primeng/themes/lara';

// Token getirme fonksiyonu (localStorage'dan token alır)
export function tokenGetter() {
  return localStorage.getItem('access_token');
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()), // Eski interceptor'ları DI'dan alacak şekilde ayarlandı
    
    // PrimeNG 19 tema konfigürasyonu
    providePrimeNG({
      theme: {
        preset: Lara
      }
    }),
    
    MessageService,
    ConfirmationService,

    importProvidersFrom(
      FormsModule,
      ReactiveFormsModule,
      ToastModule,
      ConfirmDialogModule,
      JwtModule.forRoot({
        config: {
          tokenGetter: tokenGetter,
        }
      })
    )
  ]
}; 