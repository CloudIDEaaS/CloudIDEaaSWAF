import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TreeView, ForNodesDirective, OnCreateDirective, ExpandPipe } from './';
import { EditLabelModule } from '../edit-label';
import { IonicModule } from '@ionic/angular';
import { DynamicHooksModule } from 'ngx-dynamic-hooks';

@NgModule({
  declarations: [

    // Components, directives and pipes that belong to this module

    ExpandPipe,
    TreeView,
    ForNodesDirective,
    OnCreateDirective,
  ],
  imports: [

    // Imported from other modules

    EditLabelModule,
    DynamicHooksModule,

    // These go in every component module that uses Ionic

    IonicModule,
    CommonModule

    // End
  ],
  providers: [
  ],
  exports: [

    // What is public to the outside

    TreeView
  ]
})
export class TreeViewModule
{
}
