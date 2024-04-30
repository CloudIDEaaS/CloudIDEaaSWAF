import { PipeTransform } from '@angular/core';
import { Observable } from 'rxjs';
import { Observer } from 'rxjs-compat/Observer';
import { AnonymousSubscription, Subscription } from 'rxjs-compat/Subscription';
import { Subscribable } from 'rxjs-compat/Observable';

export class BindingTransform implements Subscribable<any> {

  pipeName: string;
  pipeTransform: PipeTransform;
  args: any[];
  observers : Observer<any>[];
  private observable: Observable<any>;

  constructor() {
    this.observers = [];
    this.args = [];
  }
  subscribe(observer: Partial<Observer<any>>): AnonymousSubscription {

    let subscription = new Subscription(() => this.unsubscribe());

    this.observers.push(<Observer<any>> observer);

    return subscription;
  }

  markForCheck() {

    if (!this.observable) {

      this.observable = <Observable<any>> this.pipeTransform["_obj"];

      this.observers.forEach(o => {
        o.next(this.pipeTransform["_latestValue"]);
      });

      this.observable.subscribe({
        next : (v) => {
          this.observers.forEach(o => {
            o.next(v);
          });
        },
        error : (e) => {
          this.observers.forEach(o => {
            o.error(e);
          });
        },
        complete : () => {
          this.observers.forEach(o => {
            o.complete();
          });
        }
      });
    }
  }

  unsubscribe() {
  }
}
