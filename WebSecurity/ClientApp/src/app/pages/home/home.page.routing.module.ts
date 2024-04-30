import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomePage } from "./home.page";

const routes: Routes = [
  {
    path: "",
    component: HomePage,
    children: [
      {
        path: "model",
        children: [
          {
            path: "",
            loadChildren: () => import("../viewer/viewer.module").then(m => m.ViewerPageModule)
          }
        ]
      },
      { 
        path: "**", 
        loadChildren: () => import("../viewer/viewer.module").then(m => m.ViewerPageModule)
      }    
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class HomePageRoutingModule { }

