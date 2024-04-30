import { NgModule } from '@angular/core';
import { SetFocusDirective } from './modules/utils/setfocus.directive';
import { SetDefaultCommandDirective } from './modules/utils/setdefaultcommand.directive';

@NgModule({
  declarations: [SetFocusDirective, SetDefaultCommandDirective],
  exports: [SetFocusDirective, SetDefaultCommandDirective]
})
export class DirectivesModule {}
