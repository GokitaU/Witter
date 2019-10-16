import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(req: import("C:/Dev/Witter/Witter/Witter/Witter-SPA/node_modules/@angular/common/http/http").HttpRequest<any>, next: import("C:/Dev/Witter/Witter/Witter/Witter-SPA/node_modules/@angular/common/http/http").HttpHandler): import("C:/Dev/Witter/Witter/Witter/Witter-SPA/node_modules/rxjs/internal/Observable").Observable<import('@angular/common/http').HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(error => {
        if (error.status === 401) {
          return throwError(error.statusText);
        }

        if (error instanceof HttpErrorResponse) {
          const appError = error.headers.get('Application-Error');

          if (appError) {
            return throwError(appError);
          }
        }

        const serverErrors = error.error.errors;
        let modelStateErrors = '';

        if (serverErrors && typeof serverErrors === 'object') {
          for (const key in serverErrors) {
            if (serverErrors[key]) {
              modelStateErrors += serverErrors[key] + '\n';
            }
          }
        }
        return throwError(modelStateErrors || serverErrors || 'Server Error');
      })
    );
    }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
}
