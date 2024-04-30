import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { HierarchyComponent } from './hierarchy';

@NgModule({
  declarations: [

    // Components, directives and pipes that belong to this module

    HierarchyComponent
  ],
  imports: [

    // Imported from other modules
    // These go in every component module that uses Ionic

    CommonModule

    // End
  ],
  providers: [
  ],
  exports: [

    // What is public to the outside

    HierarchyComponent
  ]
})
export class HierarchyModule {
  constructor() {
    console.debug('HierarchyModule');
  }
}
