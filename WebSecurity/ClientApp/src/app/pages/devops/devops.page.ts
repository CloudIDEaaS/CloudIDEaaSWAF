import { Component, OnInit, ViewChild, AfterViewInit, ApplicationRef, ElementRef, OnDestroy } from '@angular/core';
import { NgTerminal } from 'ng-terminal/public-api';
import { ViewBasePage } from '../viewer/viewbase.page';
import { DevOpsProvider } from '../../providers/devops.provider';
import { Guid } from "guid-typescript";
import * as $ from "jquery";
import { timeout } from 'rxjs/operators';
import { ToastController } from '@ionic/angular';
import { environment } from '../../../environments/environment';
import { List } from 'linq-collections';
import { Storage } from '@capacitor/storage';
import { KeyCode } from '../../modules/utils/keyCodes';
import { WorkerInfo } from '../../providers/worker.service';

const ANSI_RESET = "\u001B[0m";
const ANSI_BLACK = "\u001B[30m";
const ANSI_RED = "\u001B[31m";
const ANSI_GREEN = "\u001B[32m";
const ANSI_YELLOW = "\u001B[33m";
const ANSI_BLUE = "\u001B[34m";
const ANSI_PURPLE = "\u001B[35m";
const ANSI_CYAN = "\u001B[36m";
const ANSI_WHITE = "\u001B[37m";

@Component({
  selector: 'app-devops',
  templateUrl: './devops.page.html',
  styleUrls: ['./devops.page.scss'],
})
export class DevopsPage extends ViewBasePage implements AfterViewInit {

  private sessionClientCookie: Guid;
  private serverSessionId: Guid;
  private commandMode: boolean;
  private commandIndex: number = -1;
  private commandHistory: List<string>;
  @ViewChild('terminal', { static: true }) private terminal: NgTerminal;
  fromHistory: boolean;
  webSocketWorker: WorkerInfo;

  constructor(protected devOpsProvider: DevOpsProvider) {
    super(devOpsProvider);

    this.commandHistory = new List<string>();
  }

  ionViewDidLeave() {
    this.devOpsProvider.stopSessionWebSocketClient(this.webSocketWorker);
  }

  init() {

    if (this.project) {

      this.sessionClientCookie = Guid.create();

      this.terminal.underlying.focus();
      this.terminal.underlying.textarea.focus();
      this.terminal.underlying.options.cursorBlink = true;

      this.terminal.write("Welcome to the Hydra DevOps Terminal. Connecting to server...\r\n");

      this.devOpsProvider.createCommandSession(this.sessionClientCookie).subscribe((guid) => {

        this.serverSessionId = guid;
        this.terminal.write(ANSI_YELLOW + `Connected with server session: ${ this.serverSessionId.toString() }.` + ANSI_RESET + "\r\n");

        this.commandMode = true;
        this.loadCommandHistory();

        this.terminal.write("\r\n$ ");

        this.webSocketWorker = this.devOpsProvider.startSessionWebSocketClient(this.serverSessionId, this.sessionClientCookie, environment.webSocketUrl);

      }, (err) => {

        let message = err.message;

        if (err.error) {
          if (typeof err.error === "string") {
            message = err.error;
          }
        }

        this.terminal.write(ANSI_RED + message + ANSI_RESET + "\r\n");
      });
    }
    else {
      this.terminal.write("Welcome to the Hydra DevOps Terminal.\r\n");
      this.terminal.write(ANSI_RED + "No project found. Please create from the Project Page." + ANSI_RESET + "\r\n");
    }
  }

  refresh() {
    this.init();
  }

  ngAfterViewInit(): void {

    setTimeout(() => {

      let terminalDiv = <ElementRef> this.terminal["terminalDiv"];
      let cursorLayer = $(terminalDiv.nativeElement);

      cursorLayer.trigger("click");

    }, 1000);

    this.terminal.keyEventInput.subscribe(e => {

      const ev = e.domEvent;
      const printable = !ev.altKey && !ev.ctrlKey && !ev.metaKey;

      if (ev.keyCode === KeyCode.RETURN) {

        let terminal = this.terminal.underlying;
        let buffer = terminal.buffer.active;
        let lineNumber = buffer.cursorY + buffer.viewportY;
        let command = buffer.getLine(lineNumber).translateToString(true);

        if (this.commandMode) {

          if (command)
          {
            if (command.startsWith("$ ")) {
              command = command.replace(/\$ /, "");
            }

            if (command && command.length) {

              let observable = this.devOpsProvider.sendCommand(command, this.sessionClientCookie, this.serverSessionId, environment.baseServiceUrl);

              this.terminal.write("\r\n");

              observable.subscribe((o) => {
                this.terminal.write(o);
              }, (e) => {
                this.terminal.write(ANSI_RED + e + ANSI_RESET);
              }, () => {
                this.terminal.write("\r\n$ ");
              });

              if (!this.fromHistory) {
                this.commandHistory.insert(0, command);
                this.saveCommandHistory();
              }
            }
          }
        }
        else {
          this.commandMode = true;
          this.terminal.write("\r\n$ ");
        }
      }
      else if (ev.keyCode === KeyCode.BACK_SPACE) {

        if (this.terminal.underlying.buffer.active.cursorX > 2) {
          this.terminal.write('\b \b');
        }
      }
      else if (ev.keyCode === KeyCode.UP) {

        let command: string;
        let terminal = this.terminal.underlying;
        let buffer = terminal.buffer.active;
        let lineNumber = buffer.cursorY + buffer.viewportY;

        if (this.commandIndex < this.commandHistory.count()) {
          this.commandIndex++;
        }

        command = this.commandHistory.get(this.commandIndex);
        terminal.clear();

        this.terminal.write(command);
        this.fromHistory = true;
      }
      else if (ev.keyCode === KeyCode.DOWN) {

        let command: string;
        let terminal = this.terminal.underlying;
        let buffer = terminal.buffer.active;
        let lineNumber = buffer.cursorY + buffer.viewportY;

        if (this.commandIndex > -1) {
          this.commandIndex--;
        }

        if (this.commandIndex === -1) {
          command = this.commandHistory.get(0);
        }
        else {
          command = this.commandHistory.get(this.commandIndex);
        }

        terminal.selectLines(lineNumber, lineNumber + 1);
        terminal.clear();

        this.terminal.write(command);
        this.fromHistory = true;
      }
      else if (this.commandMode && printable) {
        this.fromHistory = false;
        this.terminal.write(e.key);
      }
    });
  }

  loadCommandHistory() {

  Storage.get({ key: "devops.commandHistory"}).then(result => {

      let commandHistoryText = result.value;

      if (commandHistoryText !== null) {
        this.commandHistory.pushRange(commandHistoryText.split("\n"));
      }
    });
  }

  saveCommandHistory() {

    let commandHistoryText = this.commandHistory.toArray().join("\n");

    Storage.set({ key: "devops.commandHistory", value: commandHistoryText});
  }
}
