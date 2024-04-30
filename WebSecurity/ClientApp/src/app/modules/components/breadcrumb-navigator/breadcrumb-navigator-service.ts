import { EventEmitter, Injectable, Output } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BreadcrumbNavigatorService {
  @Output() onShowNext: EventEmitter<[any, boolean]> = new EventEmitter<[any, boolean]>();
  @Output() onSendData: EventEmitter<[any, string, any]> = new EventEmitter<[any, string, any]>();

  public showNext(sender: any, show: boolean) {
    this.onShowNext.emit([sender, show]);
  }

  public sendData(sender: any, name: string, data: any) {
    this.onSendData.emit([sender, name, data]);
  }
}
