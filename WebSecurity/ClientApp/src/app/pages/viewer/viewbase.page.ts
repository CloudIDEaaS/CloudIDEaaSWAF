import { Router } from '@angular/router';
import { DevOpsProvider } from '../../providers/devops.provider';
import { BreadcrumbService } from '@cloudideaas/ngx-breadcrumb';
import { Project } from '../../models/project.model';
import { List } from 'linq-collections';
import { removeSpaces } from "../../businessModelLevel";
import { HomePage } from '../home/home.page';
import { ApplicationRef } from '@angular/core';
import { environment } from '../../../environments/environment';
declare const require: any;

export abstract class ViewBasePage {

  nodes: any;
  model: any;
  project: Project;
  homePageComponent: HomePage;
  businesModelName: string;
  abstract init();
  abstract refresh();

  constructor(protected devOpsProvider: DevOpsProvider,
    protected router: Router = null,
    protected breadcrumbService: BreadcrumbService = null,
    private applicationRef: ApplicationRef = null) {

      this.refreshNodes();
  }

  cacheProject(project: Project) {
    this.devOpsProvider.saveCache(project);
  }

  updateProject(project: Project) {
    this.devOpsProvider.updateProject(project);
  }

  backgroundSaveProjectToServer(project: Project) {
    this.devOpsProvider.backgroundSaveProjectToServer(project, environment.baseServiceUrl);
  }

  navigateToNode(hierarchy, nodeData, edit) {

    const jsonpath = require("JSONPath");
    let expression = `$..*[?(@.id == ${ nodeData.id })]`;
    let result = jsonpath.query(hierarchy, expression);
    let url = this.router.url;
    let segments = [];
    let lastFragment = new List<string>(this.router.url.split("/")).last();
    let name = nodeData.name;
    let levelType = nodeData.title;

    if (!edit) {
      if (name === lastFragment || name === this.model.name) {
        return;
      }
    }

    while (result.length) {

      nodeData = result[0];
      segments.push(nodeData.name);

      expression = `$..*[?(@.id == ${ nodeData.parent })]`;
      result = jsonpath.query(hierarchy, expression);
    }

    segments.pop();

    if (segments.length) {
      url += "/";
    }

    url += segments.reverse().join("/");

    if (edit) {
      url += "?edit=true";
    }

    this.router.navigateByUrl(url);
  }

  refreshNodes() {

    this.getCurrentModel().then(p => {

      // let appComponent = <AppComponent> this.applicationRef.components[0].instance;
      // this.homePageComponent = null; // <HomePage> appComponent.rootInstance;

      if (p) {
        this.businesModelName = `${ p.projectName } Business Model`;
        this.nodes = p.rootModel;
        this.project = p;
      }

      // this.homePageComponent.pageTitle = this.businesModelName;

      setTimeout(() => {
          this.init();
      }, 1);

      }, (e) => {
        throw new Error(e);
    });
  }

  getCurrentModel(): Promise<Project> {

    let model: any;
    let urlFragments: List<string>;
    let url: string;

    let promise = new Promise<Project>((resolve, reject) =>
    {
      let count: number;

      if (this.breadcrumbService && this.router) {

        urlFragments = new List<string>(this.router.url.split("/").map(f => decodeURI(f)));
        url = "";
        count = urlFragments.count();

        if (count <= 4) {
          this.devOpsProvider.clearCache();
        }
      }

      this.devOpsProvider.getProjectForCurrentUser().subscribe(p => {

        if (p) {
          model = p.rootModel;
          this.model = model;
        }

        if (this.breadcrumbService && this.router) {

            urlFragments.skip(4).forEach(f => {

              let children = new List<any>(model.children);
              let node;
              let lastIndex = f.lastIndexOf("?");
              let name: string;

              if (lastIndex > 0) {
                name = f.substr(0, lastIndex);
              }
              else {
                name = f;
              }

              node = children.singleOrDefault(c => removeSpaces(<string>c.name) === name || (<string>c.name) === name);

              if (node != null) {
                model = node;
              }
          });
        }

        if (p) {
          p.rootModel = model;
        }

        resolve(p);

      }, (e) => {
        reject(e);
      });
    });

    if (this.breadcrumbService && this.router) {

        urlFragments.skip(2).forEach(f => {

        let lastIndex = f.lastIndexOf("?");
        let name: string;

        if (lastIndex > 0) {
          name = f.substr(0, lastIndex);
        }
        else {
          name = f;
        }

        url += "/" + encodeURI(f);
        this.breadcrumbService.addFriendlyNameForRoute(url, name);
      });
    }

    return promise;
  }

  getProjectFromCache(): Promise<Project> {
    return this.devOpsProvider.getProjectFromCache();
  }

  getDisplayedRootNode(model) : any {

    let urlFragments = new List<string>(this.router.url.split("/").map(f => decodeURI(f)));
    let displayedRoot = model;

    urlFragments.skip(3).forEach(f => {

      let children = new List<any>(displayedRoot.children);
      let node;
      let lastIndex = f.lastIndexOf("?");
      let name: string;

      if (lastIndex > 0) {
        name = f.substr(0, lastIndex);
      }
      else {
        name = f;
      }

      node = children.singleOrDefault(c => removeSpaces(<string>c.name) === name || (<string>c.name) === name);

      if (node != null) {
        displayedRoot = node;
      }
    });

    return displayedRoot;
  }
}
