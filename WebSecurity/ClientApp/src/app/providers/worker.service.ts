import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';
import { promise } from 'protractor';

export class WorkerInfo {
  public id: Guid;
  public promise: Promise<any>;
  public worker: Worker;

  constructor(id: Guid, promise:Promise<any>, worker: Worker) {
    this.id = id;
    this.promise = promise;
    this.worker = worker;
  }
}

@Injectable({
  providedIn: 'root'
})
export class WebworkerService {
  private workerFunctionToUrlMap = new WeakMap<Function, string>();
  private promiseToWorkerMap = new WeakMap<WorkerInfo, Worker>();

  public run<T>(workerFunction: (input: any) => T, data?: any): WorkerInfo {
    const url = <string> this. getOrCreateWorkerUrl(workerFunction);
    return this.runUrl(url, data);
  }

  public runUrl(url: string, data?: any): WorkerInfo {
    const worker = new Worker(url);
    const promise = this.createPromiseForWorker(worker, data);
    const workerInfo = new WorkerInfo(Guid.create(), promise, worker);
    const promiseCleaner = this.createPromiseCleaner(workerInfo);

    this.promiseToWorkerMap.set(workerInfo, worker);

    promise.then(promiseCleaner).catch(promiseCleaner);

    return workerInfo;
  }

  public terminate<T>(workerInfo: WorkerInfo) {
    this.removeWorker(workerInfo);
  }

  public getWorker(workerInfo: WorkerInfo): Worker {
    return <Worker> this.promiseToWorkerMap.get(workerInfo);
  }

  private createPromiseForWorker<T>(worker: Worker, data: any) {
    return new Promise<T>((resolve, reject) => {

      try
      {
        worker.addEventListener('message', (event) => resolve(event.data));
        worker.addEventListener('error', reject);

        worker.postMessage(data);
      }
      catch (e)
      {
        reject(e);
      }
    });
  }

  public getOrCreateWorkerUrl(fn: Function): string | undefined {
    if (!this.workerFunctionToUrlMap.has(fn)) {
      const url = this.createWorkerUrl(fn);
      this.workerFunctionToUrlMap.set(fn, url);
      return url;
    }
    return this.workerFunctionToUrlMap.get(fn);
  }

  private createWorkerUrl(resolve: Function): string {

    const resolveString = resolve.toString();

    const webWorkerTemplate = `
            self.addEventListener('message', function(e) {
                ((${resolveString})(e.data));
            });
        `;
    const blob = new Blob([webWorkerTemplate], { type: 'text/javascript' });
    return URL.createObjectURL(blob);
  }

  private createPromiseCleaner<T>(workerInfo: WorkerInfo): (input: any) => T {
    return (event) => {
      this.removeWorker(workerInfo);
      return event;
    };
  }

  private removeWorker(workerInfo: WorkerInfo) {
    const worker = this.promiseToWorkerMap.get(workerInfo);
    if (worker) {
      worker.terminate();
    }
    this.promiseToWorkerMap.delete(workerInfo);
  }
}
