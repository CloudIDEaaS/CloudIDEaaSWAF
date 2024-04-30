import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TabsPage } from './tabs-page';
import { ProjectPage } from '../project/project.page';
import { DevopsPage } from '../devops/devops.page';

const routes: Routes = [
  {
    path: 'tabs',
    component: TabsPage,
    children: [
      {
        path: 'project',
        children: [
          {
            path: '',
            component: ProjectPage,
          },
          {
            path: 'session/:sessionId',
            loadChildren: () => import('../project/project.module').then(m => m.ProjectModule)
          }
        ]
      },
      {
        path: 'about',
        children: [
          {
            path: '',
            loadChildren: () => import('../about/about.module').then(m => m.AboutModule)
          }
        ]
      },
      {
        path: 'devops',
        children: [
          {
            path: '',
            loadChildren: () => import('../devops/devops.module').then(m => m.DevopsModule)
          }
        ]
      },
      {
        path: '',
        redirectTo: '/app/tabs/project',
        pathMatch: 'full'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TabsPageRoutingModule { }

