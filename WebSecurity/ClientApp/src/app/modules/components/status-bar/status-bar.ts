import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { StatusBarService } from '../../services/status-bar-service';

@Component({
  selector: 'status-bar',
  templateUrl: './status-bar.html',
  styleUrls: ['./status-bar.scss'],
})
export class StatusBar implements OnInit {
  
  @ViewChild('statusLabel') statusLabel: ElementRef<HTMLLabelElement>;

  constructor(private statusBarService: StatusBarService) { 
    statusBarService.onStatus.subscribe((s) => {
      this.statusLabel.nativeElement.innerText = s;
    });
  }

  ngOnInit() {
    
  }

}
