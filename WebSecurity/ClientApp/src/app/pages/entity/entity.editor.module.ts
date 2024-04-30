import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { EntityEditorPage } from './entity.editor';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule
  ],
  exports: [
    EntityEditorPage
  ],
  declarations: [EntityEditorPage]
})
export class EntityEditorModule {}
