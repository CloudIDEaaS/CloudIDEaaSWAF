import { EventEmitter, Injectable, Output } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class StatusBarService {
  @Output() onStatus: EventEmitter<string> = new EventEmitter<string>();

  public show() {
  }

  public setStatus(status: string) {
    this.onStatus.emit(status);
  }
}