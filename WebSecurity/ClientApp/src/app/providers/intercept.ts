import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { Provider } from '@angular/core';
import { Api } from '../providers/api/api';

@Injectable()
export class Interceptor implements HttpInterceptor {

  constructor(private api: Api) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.api.handle(req, next);
  }
}

export const InterceptorProvider: Provider =
  { provide: HTTP_INTERCEPTORS, useClass: Interceptor, multi: true };
