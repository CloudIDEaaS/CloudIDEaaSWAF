import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'hyper-link',
  templateUrl: './hyper-link.html',
  styleUrls: ['./hyper-link.scss'],
})
export class HyperLink implements OnInit {

  @Input('content') content: string;
  @Input('route') route: string;
  @Input('class') class: string;

  constructor() {
    console.debug("HyperLink");
  }

  get routeArray(): string[] {
    return [this.route];
  }

  ngOnInit() { }

}
