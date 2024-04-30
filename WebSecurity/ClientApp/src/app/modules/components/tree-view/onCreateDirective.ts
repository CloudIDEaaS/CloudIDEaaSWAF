import { Directive, Output, EventEmitter, Input, SimpleChange} from '@angular/core';

@Directive({
  selector: '[onCreate]'
})
export class OnCreateDirective {

  @Output() onCreate: EventEmitter<any> = new EventEmitter<any>();

  ngOnInit() {
     this.onCreate.emit('dummy');
  }
}
