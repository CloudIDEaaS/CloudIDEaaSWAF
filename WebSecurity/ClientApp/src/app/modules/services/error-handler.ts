import { HttpErrorResponse } from "@angular/common/http";
import { ErrorHandler, Injectable } from "@angular/core";
import { DebugUtils } from "../utils/DebugUtils";

@Injectable({
  providedIn: 'root'
})
export class AppErrorHandler {
  
  handleError(error: any): Promise<{ error: any, message: string }> {
    
    let promise = new Promise<{ error: any, message: string }>((resolve, reject) => {

      if (error instanceof Error) {
        console.error(error.message);
        resolve({ error: error, message: error.message });
      }
      else if (typeof error === 'string' || error instanceof String) {
        console.error(error);
        resolve({ error: error, message: <string>error });
      }
      else if (error instanceof HttpErrorResponse) {

        let message = `Error, message: ${error.message.substring(0, error.message.indexOf("for"))}, status: ${error.status}, statusText: ${error.statusText}, thumprint: ${btoa(error.url)}`
        console.error(message);
        
        resolve({ error: error, message: message });
      }
      else {
        DebugUtils.break();
      }
    });

    return promise;
  }
}

export class GlobalErrorHandler implements ErrorHandler {
  
  handleError(error: any): Promise<{ error: any, message: string }> {
    
    let appErrorHandler = new AppErrorHandler();
    let promise = appErrorHandler.handleError(error);

    return promise;
  }
}

