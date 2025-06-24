import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptor } from './core/http/auth.interceptor';
import { ErrorHandlingInterceptor } from './core/http/error-handling.interceptor';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { JwtModule } from '@auth0/angular-jwt';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';

// Token getirme fonksiyonu (localStorage'dan token alır)
export function tokenGetter() {
  return localStorage.getItem('token');
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([AuthInterceptor, ErrorHandlingInterceptor])),
    MessageService,
    importProvidersFrom(
      ToastModule,
      JwtModule.forRoot({    // JwtModule.forRoot ile JwtHelperService sağlandı
        config: {
          tokenGetter: tokenGetter,
          // İzin verilen domain'leri ve izin verilmeyen route'ları buraya ekleyebilirsiniz
          // allowedDomains: ["example.com"],
          // disallowedRoutes: ["http://example.com/examplebadroute/"],
        }
      })
    ),
    providePrimeNG({
      theme: {
        preset: Aura
      }
    })
  ]
};
