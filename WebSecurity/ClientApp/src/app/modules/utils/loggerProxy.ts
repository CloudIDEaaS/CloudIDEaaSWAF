import { NGXLogger } from "ngx-logger";
import { Injectable, Injector } from '@angular/core';
import { ILogEvent } from "./logEvent";

@Injectable({
  providedIn: 'root'
})
export class LoggerProxy {

  constructor(private logger: NGXLogger) {

    if (!this.logger) {
      debugger;
    }

    console.log();
  }

  initialize(logQueue: Array<ILogEvent>) {

    logQueue.forEach((e) => {
      this.log(e.type, e.message, e.file);
    });
  }

  public log(logMethod: keyof NGXLogger, message: string, file?: string): void {

    let method = logMethod.toString();

    switch (method) {
      case "trace":
        this.logger.trace(message, { fileName: '${file}' });
        break;
      case "debug":
        this.logger.debug(message, { fileName: '${file}' });
        break;
      case "info":
        this.logger.info(message, { fileName: '${file}' });
        break;
      case "log":
        this.logger.log(message, { fileName: '${file}' });
        break;
      case "warn":
        this.logger.warn(message, { fileName: '${file}' });
        break;
      case "error":
        this.logger.error(message, { fileName: '${file}' });
        break;
      case "fatal":
        this.logger.fatal(message, { fileName: '${file}' });
        break;
      default:
        debugger;
        throw new Error(`Invalid log method ${logMethod}`);
    }
  }
}
