import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditDeleteButtons } from './edit-delete';
import { IonicModule } from '@ionic/angular';

@NgModule({
  declarations: [

    // Components, directives and pipes that belong to this module

    EditDeleteButtons
  ],
  imports: [

    // Imported from other modules
    // These go in every component module that uses Ionic

    CommonModule,
    // End
  ],
  providers: [
  ],
  exports: [

    // What is public to the outside

    EditDeleteButtons
  ]
})
export class EditDeleteButtonsModule {
  constructor() {
  }
}
