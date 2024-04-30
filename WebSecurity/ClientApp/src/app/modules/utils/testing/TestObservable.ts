import { Observable, Observer, Subscription, Subscriber } from "rxjs";
import { Subscribable } from "rxjs-compat/Observable";
import { PartialObserver } from "rxjs-compat/Observer";
import { AnonymousSubscription } from "rxjs-compat/Subscription";
import { List, IEnumerable } from "linq-collections";

export class TestObservable<T> implements Subscribable<T> {

  observers: Observer<T>[];
  values: List<T>;
  timer?: NodeJS.Timeout;

  constructor(values: T[] = []) {
    this.observers = [];
    this.values = new List<T>(values);
  }

  subscribe(observer: Partial<Observer<T>>): AnonymousSubscription {

    let subscription = new Subscription(() => this.unsubscribe());

    if (this.timer == null) {

      this.timer = setInterval(() => {

        if (this.values.count()) {

          let value = <T> this.values.removeAt(0);

          this.observers.forEach(o => {
            o.next(value);
          });

        }
        else {

          clearTimeout(this.timer);

          this.observers.forEach(o => {

            if (o.complete) {
              o.complete();
            }
          });
        }
      }, 1000);
    }

    this.observers.push(<Observer<T>>observer);

    return subscription;
  }

  unsubscribe() {
  }

  static from<T>(values: T[]): TestObservable<T> {
    return new TestObservable<T>(values);
  }
}
