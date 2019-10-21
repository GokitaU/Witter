import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(req: import("node_modules/@angular/common/http/http").HttpRequest<any>, next: import("@angular/common/http/http").HttpHandler): import("node_modules/rxjs/internal/Observable").Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse) {
          if (error.status === 401) {
            return throwError(error.statusText);
          }

          const applicationError = error.headers.get('Application-Error');

          if (applicationError) {
            console.error(applicationError);
            return throwError(applicationError);
          }

          let serverError = error.error.errors;
          let modalStateErrors = '';
          if (serverError && typeof serverError === 'object') {
            for (const key in serverError) {
              if (serverError[key]) {
                modalStateErrors += serverError[key] + '\n';
              }
            }
          }

          serverError = error.error;
          return throwError(modalStateErrors || serverError || 'Server Error');
        }
      })
    )
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
}
