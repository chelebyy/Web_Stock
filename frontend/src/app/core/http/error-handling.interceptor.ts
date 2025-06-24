import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, throwError, finalize, tap } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorService } from '../services/error.service';
import { LoadingService } from '../services/loading.service';

export const ErrorHandlingInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> => {
  const errorService = inject(ErrorService);
  const loadingService = inject(LoadingService);

  loadingService.show();

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      console.error('ErrorHandlingInterceptor yakaladı:', error);

      errorService.handleError(error);

      return throwError(() => error);
    }),
    finalize(() => {
      loadingService.hide();
    })
  );
}; 