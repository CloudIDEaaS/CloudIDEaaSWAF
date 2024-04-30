import { Injectable } from "@angular/core";
import { Api } from "./api/api";
import { Project } from "../models/project.model";
import { map, filter, mergeMap, tap } from "rxjs/operators";
import { Observable, from, } from "rxjs";
import { Storage } from "@capacitor/storage";
import { Guid } from "guid-typescript";
import { reject } from 'q';
import { WebworkerService, WorkerInfo } from './worker.service';
import * as $ from "jquery";
import { HttpHeaders } from '@angular/common/http';
import { promise } from 'protractor';
import { resolve } from "dns";
declare const require: any;

@Injectable()
export class DevOpsProvider {

  project: Project;
  webSocketClientRunning = false;

  constructor(private api: Api, private workerService: WebworkerService) {
  }

  clearCache() {
    Storage.remove({ key: "devOpsProject" });
  }

  backgroundSaveProjectToServer(project: Project, baseServiceUrl: string) {

    let worker: WorkerInfo;
    let promise: Promise<any>;

    this.saveCache(project);

    worker = this.workerService.run(this.jquerySave, { project: JSON.stringify(project), baseServiceUrl: baseServiceUrl });
    promise = worker.promise;

    promise.then(() => {
      console.debug("got backgroundSaveProjectToServer start message");
    }, (e) => {
      console.error(e);
    });
  }

  startSessionWebSocketClient(serverSessionId: Guid, clientCookie: Guid, webSocketUrl: string): WorkerInfo {

    let worker: WorkerInfo;
    let promise: Promise<any>;

    worker = this.workerService.run(this.sessionWebSocketClient, { serverSessionId: serverSessionId.toString(), clientCookie: clientCookie.toString(), webSocketUrl: webSocketUrl });
    promise = worker.promise;

    promise.then(() => {
      console.debug("got startSessionWebSocketClient start message");
    }, (e) => {
      console.error(e);
    });

    return worker;
  }

  stopSessionWebSocketClient(worker: WorkerInfo) {

    this.webSocketClientRunning = false;

    this.workerService.terminate(worker);
  }

  sessionWebSocketClient = (input: { serverSessionId: string, clientCookie: string, webSocketUrl: string }) => {

    let webSocketUrl = input.webSocketUrl;
    let serverSessionId = input.serverSessionId;
    let clientCookie = input.clientCookie;
    let webSocketResponseMilliseconds = 0;
    let received = false;
    let startTime: number;
    let opened = false;
    let running = true;
    let run: () => void;
    let socket: WebSocket = null;

    run = () => {

      let promise = new Promise<boolean>((callback) => {

        socket = new WebSocket(webSocketUrl);

        this.webSocketClientRunning = true;

        console.debug("sessionWebSocketClient running, connecting to..." + webSocketUrl);

        socket.onopen = (event) => {
          console.debug("opened connection to " + webSocketUrl);
          callback(true);
        };

        socket.onclose = (event) => {
          console.debug("closed connection from " + webSocketUrl);
          running = false;
        };

        socket.onmessage = (event) => {

          webSocketResponseMilliseconds = Date.now() - startTime;
          received = true;

          console.debug(event.data);
          socket.close();

          setTimeout(() => {
            run();
          }, 1000);
        };

        socket.onerror = (event) => {
          console.debug("error: " + event);
        };
      });

      promise.then((running) => {

        if (running) {

          let message = `Ping {${serverSessionId},${clientCookie},${webSocketResponseMilliseconds}}`;

          console.debug(message);

          startTime = Date.now();

          received = false;
          socket.send(message);
        }
      });
    };

    run();
  }

  jquerySave = (input: { project: string, baseServiceUrl: string }) => {

    let project = input.project;
    let baseServiceUrl = input.baseServiceUrl;
    let url = baseServiceUrl + "/api/devops/updateProject";
    let xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
      if (this.readyState === 4 && this.status === 200) {
      }
    };

    xhttp.open("PUT", url, true);

    $.each(this.api.getJsonHeaders(), (header: any) => {
      xhttp.setRequestHeader(header.key, header.value);
    });

    xhttp.send(project);

    console.debug(project);
  }

  saveCache(project: Project) {
    Storage.set({ key: "devOpsProject", value: JSON.stringify(project) });
  }

  getProjectFromCache(): Promise<Project> {

    let promise = new Promise<Project>(resolve => {

      Storage.get({ key: "devOpsProject" }).then(r => {
        let json = r.value;
        let project = <Project>JSON.parse(json);

        resolve(project);

      });
    });

    return promise;
  }

  getProjectForCurrentUser(): Observable<Project> {

    let observable = from(Storage.get({ key: "devOpsProject" })).pipe(map(r => {
      let json = r.value;
      let project = <Project>JSON.parse(json);

      return project;
    }));

    return observable.pipe(mergeMap((p: Project) => {

      if (p) {

        return new Promise<Project>((resolve, reject) => {
          resolve(p);
        });
      }
      else {

        let observableMerge = this.api.get<Project>("devops/getProject");

        return observableMerge.pipe(map(p2 => {

          if (p2) {
            Storage.set({ key: "devOpsProject", value: JSON.stringify(p2) });
            return new Project(<any>p2);
          }
          else {
            return null;
          }
        }));
      }
    }));
  }

  updateProject(project: Project) {

    let observable = this.api.put("devops/project", project);

    observable.subscribe(() => {
      this.saveCache(project);
    });

    return observable;
  }

  createProject(project: Project) {

    let observable = this.api.post("devops/createProject", project);

    observable.subscribe(() => {
      this.saveCache(project);
    });

    return observable;
  }

  deleteProject(id: string): any {

    let observable = this.api.delete("devops/project?id=" + id);

    observable.subscribe(() => {
      this.clearCache();
    });

    return observable;
  }

  createCommandSession(sessionClientCookie: Guid): Observable<Guid> {
    return this.api.get<Guid>("devops/creatAutomationCommandSession", { sessionClientCookie: sessionClientCookie });
  }

  sendCommand(command: string, clientCookie: Guid, serverSessionId: Guid, baseServiceUrl: string) {

    let observable = new Observable<string>(subscriber => {

      let url = baseServiceUrl + `/api/devops/sendAutomationCommand?command=${command}&sessionClientCookie=${clientCookie}&serverSessionId=${serverSessionId}`;
      let xhttp = new XMLHttpRequest();
      let headers = this.api.getJsonHeaders();

      this.api.initializeHeaders(headers);

      xhttp.onreadystatechange = (ev: Event) => {
        if (xhttp.readyState === 4) {

          if (xhttp.status === 200) {
            subscriber.complete();
          }
          else {
            subscriber.error(xhttp.responseText);
          }
        }
      };

      xhttp.onprogress = (ev: ProgressEvent) => {
        subscriber.next(xhttp.responseText);
      };

      xhttp.open("GET", url, true);

      $.each(headers.keys(), (index: number, key: string) => {
        xhttp.setRequestHeader(key, <string>headers.get(key));
      });

      xhttp.send();

      console.debug();
    });

    return observable;
  }
}
