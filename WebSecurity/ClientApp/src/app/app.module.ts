import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { IonicModule } from '@ionic/angular';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { FormsModule } from '@angular/forms';
import { DevOpsProvider } from './providers/devops.provider';
import { WebworkerService } from './providers/worker.service';
import { Api } from './providers/api/api';
import { BreadcrumbService } from '@cloudideaas/ngx-breadcrumb';
import { UserProvider } from './providers/user.provider';
import { RouteGuardProvider } from './providers/routeGuard.provider';
import { RegistrationValidator } from './pages/register/register-validator';
import { NgxMaskIonicModule } from '@cloudideaas/ngx-mask-ionic';
import { LoadOrToastProvider } from './providers/loadOrToast';
import { AccordionModule } from "@cloudideaas/ngx-accordion";
import { ProjectValidator } from './pages/project/project-validator';
import { InterceptorProvider } from './providers/intercept';
import { IndexPageLoader } from './providers/index-page-loader';
import { LoggerModule, NgxLoggerLevel } from "ngx-logger";
import { LoggerProxy } from './modules/utils/loggerProxy';

const loader = new IndexPageLoader();
const logLevel = loader.GetLogLevel();
const disableConsoleLogging = logLevel == NgxLoggerLevel.OFF;

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    LoggerModule.forRoot({
      serverLoggingUrl: environment.baseServiceUrl + "/cloudideaas/hydra/services/api/log",
      level: logLevel,
      serverLogLevel: NgxLoggerLevel.TRACE,
      disableConsoleLogging: disableConsoleLogging
    }),
    FormsModule,
    AccordionModule,
    NgxMaskIonicModule.forRoot(),
    IonicModule.forRoot(),
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: environment.production
    })
  ],
  declarations: [AppComponent],
  providers: [
    LoggerProxy,
    DevOpsProvider,
    WebworkerService,
    Api,
    BreadcrumbService,
    UserProvider,
    RouteGuardProvider,
    LoadOrToastProvider,
    RegistrationValidator,
    ProjectValidator,
    InterceptorProvider,
    { provide: IndexPageLoader, useFactory: () => loader }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  constructor(private loggerProxy: LoggerProxy) {

    let logQueue = window["logQueue"];

    this.loggerProxy.initialize(logQueue);
  }
}
