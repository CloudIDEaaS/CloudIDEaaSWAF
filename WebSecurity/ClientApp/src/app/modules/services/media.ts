import { EventEmitter, Injectable, Output } from '@angular/core';
import { AlertController } from '@ionic/angular';
import { VoiceRecorder } from 'capacitor-voice-recorder';
import { Observable, Subscriber } from 'rxjs';
import { CurrentPlatformService } from './current-platform';

@Injectable({
  providedIn: 'root'
})
export class MediaService {
  private mediaRecorder: MediaRecorder;
  @Output() onSignal: EventEmitter<number> = new EventEmitter<number>();
  audioContext: AudioContext;

  constructor(private currentPlatformService: CurrentPlatformService,
    private alertController: AlertController) {
  }

  public stop() {
    this.mediaRecorder.stop();
  }

  public playAudio(arrayBuffer: ArrayBuffer) : Promise<void> {

    this.onSignal.emit(0);

    let promise = new Promise<void>(async(resolve, reject) => {

      this.onSignal.emit(1);

      if (window.AudioContext) {

        let source: AudioBufferSourceNode;

        this.onSignal.emit(2);
        this.audioContext = new AudioContext();

        if (this.audioContext.state == "suspended") {

          let alert = await this.alertController.create({
            header: 'Allow audio to continue',
            message: 'Do you allow audio?',
            buttons: [
              {
                text: 'No',
                role: 'cancel',
                cssClass: 'secondary',
                id: 'cancel-button'
              }, {
                text: 'Okay',
                id: 'confirm-button',
                handler: () => {

                  this.playAudio(arrayBuffer).then(() => {
                    resolve();
                  }, e => {
                    reject(e);
                  });
      
                }
              }
            ]
          });
      
          await alert.present();          
          return;
        }

        source = this.audioContext.createBufferSource();

        this.onSignal.emit(3);

        this.audioContext.decodeAudioData(arrayBuffer.slice(0), (b) => {

          this.onSignal.emit(4);
          
          source.buffer = b;
          source.connect(this.audioContext.destination);
          source.start(0);

          this.onSignal.emit(5);

          source.addEventListener("ended", () => {
            this.onSignal.emit(6);
            resolve();
          });

        }, (e) => {

          this.onSignal.emit(-2);
          reject(e);

        });
      }
      else {
        this.onSignal.emit(-1);
        reject("No browser capability to play audio");
      }
    });

    return promise;
  }

  public recordAudio(intervalMilliseconds: number): Observable<Blob> {

    let observable = new Observable<Blob>((subscriber: Subscriber<Blob>) => {

      if (this.currentPlatformService.isBrowser) {

        let handleSuccess = (stream: MediaStream) => {

          let options = { mimeType: 'audio/webm' };
          let audioContext = new AudioContext();
          let analyser = audioContext.createAnalyser();
          let microphone = audioContext.createMediaStreamSource(stream);
          let javascriptNode = audioContext.createScriptProcessor(2048, 1, 1);

          analyser.smoothingTimeConstant = 0.8;
          analyser.fftSize = 1024;

          microphone.connect(analyser);
          analyser.connect(javascriptNode);
          javascriptNode.connect(audioContext.destination);

          javascriptNode.onaudioprocess = () => {
            let array = new Uint8Array(analyser.frequencyBinCount);
            let values = 0;
            let length: number;
            let average: number;

            analyser.getByteFrequencyData(array);

            length = array.length;

            for (let i = 0; i < length; i++) {
              values += (array[i]);
            }

            average = values / length;
            this.onSignal.emit(average);
          };

          this.mediaRecorder = new MediaRecorder(stream, options);

          this.mediaRecorder.addEventListener('dataavailable', (e) => {
            if (e.data.size > 0) {
              subscriber.next(e.data);
            }
          });

          this.mediaRecorder.addEventListener('stop', () => {
            subscriber.complete();
          });

          this.mediaRecorder.start(intervalMilliseconds);
        };

        (<any>navigator).permissions.query({ name: 'microphone' }).then((result) => {

          if (result.state == 'granted') {
            navigator.mediaDevices.getUserMedia({ audio: true, video: false }).then(handleSuccess);
          }
          else if (result.state == 'prompt') {
            navigator.mediaDevices.getUserMedia({ audio: true, video: false }).then(handleSuccess);
          }
          else if (result.state == 'denied') {
            subscriber.error(new Error("User media denied"));
          }
          result.onchange = () => { };
        });
      }
      else {
        VoiceRecorder.requestAudioRecordingPermission();
      }
    });

    return observable;
  }
}
