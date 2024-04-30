import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { IonicModule } from '@ionic/angular';
import { ViewerPage } from './viewer.page';
import { ModelEditorModule } from '../businessModel/model.editor.module';
import { EntityEditorModule } from '../entity/entity.editor.module';
import { ProjectModule } from '../project/project.module';

const routes: Routes = [
  {
    path: '',
    component: ViewerPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ModelEditorModule,
    EntityEditorModule,
    ProjectModule,
    RouterModule.forChild(routes)
  ],
  declarations: [ViewerPage]
})
export class ViewerPageModule {}
