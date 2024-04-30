import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { ProjectPage } from './project.page';
import { DirectivesModule } from '../../directives.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    ReactiveFormsModule,
    DirectivesModule,
  ],
  exports: [
    ProjectPage
  ],
  declarations: [
    ProjectPage
  ]
})
export class ProjectModule {}
