import { Component, Output, EventEmitter, AfterViewInit, OnInit, ViewChild, ViewRef } from '@angular/core';
import { BreadcrumbService, BreadcrumbComponent } from '@cloudideaas/ngx-breadcrumb';
import { Router, NavigationStart, NavigationEnd, NavigationCancel, ResolveStart, ResolveEnd, RouteConfigLoadStart, RouteConfigLoadEnd, GuardsCheckStart, GuardsCheckEnd, RoutesRecognized, ChildActivationStart, ChildActivationEnd } from '@angular/router';
import { ModelEditorPage } from '../businessModel/model.editor';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements AfterViewInit, OnInit {

  pageTitleValue: string;
  @Output() pageTitleChange = new EventEmitter();
  @ViewChild(BreadcrumbComponent, { static: true} ) breadcrumb: BreadcrumbComponent;

  get pageTitle(): string {
    return this.pageTitleValue;
  }

  set pageTitle(value: string) {
    this.pageTitleValue = value;
    this.pageTitleChange.emit(this.pageTitleValue);
  }

  constructor(private router: Router, private breadcrumbService: BreadcrumbService) {
  }

  ngOnInit(): void {
  }
  
  ngAfterViewInit(): void {
    this.pageTitle = "[No Title]";
  }
}
