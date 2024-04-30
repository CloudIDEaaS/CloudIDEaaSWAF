import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { WINDOW_PROVIDERS } from "./services/window.service";

@NgModule({
  providers: [
    WINDOW_PROVIDERS
  ]
})
export class CoreModule {

  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule is already loaded. Import it in the AppModule only');
    }
  }

  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: []
    };
  }

}