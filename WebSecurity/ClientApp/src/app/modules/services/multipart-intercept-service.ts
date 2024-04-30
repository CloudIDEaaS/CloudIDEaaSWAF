import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class MultipartInterceptService implements HttpInterceptor {
  private parserMap = {
    'application/json': JSON.parse,
  };

  private parseMultipart(multipart: string, boundary: string): any {
    const dataArray: string[] = multipart.split(`--${boundary}`);
    dataArray.shift();
    dataArray.forEach((dataBlock) => {
      const rows = dataBlock.split(/\r?\n/).splice(1, 4);
      if (rows.length < 1) {
        return;
      }
      const headers = rows.splice(0, 2);
      const body = rows.join('');
      if (headers.length > 1) {
        const pattern = /Content-Type: ([a-z\/+]+)/g;
        const match = pattern.exec(headers[0]);
        if (match === null) {
          throw Error('Unable to find Content-Type header value');
        }
        const contentType = match[1];
        if (this.parserMap.hasOwnProperty(contentType) === true) {
          return this.parserMap[contentType](body);
        }
      }
    });
    return false;
  }

  private parseResponse(response: HttpResponse<any>): HttpResponse<any> {
    
    const contentTypeHeaderValue = response.headers.get('Content-Type');
    const body = response.body;

    if (contentTypeHeaderValue) {
        
      const contentTypeArray = contentTypeHeaderValue.split(';');
      const contentType = contentTypeArray[0];

      switch (contentType) {
        case 'multipart/related':
        case 'multipart/mixed':
        case 'multipart/form-data':
          const boundary = contentTypeArray[1].split('boundary=')[1];
          const parsed = this.parseMultipart(body, boundary);
          if (parsed === false) {
            throw Error('Unable to parse multipart response');
          }
          return response.clone({ body: parsed });
        default:
          return response;
      }
    }
    else {
      return response;
    }
  }

  // intercept request and add parse custom response
  public intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      map((response: HttpResponse<any>) => {
        if (response instanceof HttpResponse) {
          return this.parseResponse(response);
        }
      })
    );
  }
}
