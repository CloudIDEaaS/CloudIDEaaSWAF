import { NGXLogger } from "ngx-logger";

export interface ILogEvent {
  type: keyof NGXLogger;
  message: string;
  file?: string;
}