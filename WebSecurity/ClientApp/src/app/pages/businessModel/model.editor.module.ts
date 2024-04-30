import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { Routes, RouterModule } from "@angular/router";
import { IonicModule } from "@ionic/angular";
import { ModelEditorPage } from "./model.editor";
import { HierarchyModule } from "../../modules/components/hierarchy/hierarchy.module";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    HierarchyModule
  ],
  exports: [
    ModelEditorPage
  ],
  declarations: [ModelEditorPage]
})
export class ModelEditorModule {}
