import { ApplicationRef, Component, EventEmitter, OnInit, Output, ViewContainerRef } from "@angular/core";
import { ActivatedRoute, LoadChildrenCallback, NavigationEnd, Router, Route, } from "@angular/router";
import { BreadcrumbService } from "@cloudideaas/ngx-breadcrumb"
import { filter, tap } from "rxjs/operators";
import { List } from "linq-collections";
import { Subscription } from "rxjs";
import { BreadcrumbNavigatorService } from "./breadcrumb-navigator-service";
const queryString = require('query-string');

@Component({
  selector: "breadcrumb-navigator",
  templateUrl: "./breadcrumb-navigator.html",
  styleUrls: ["./breadcrumb-navigator.scss"],
})
export class BreadcrumbNavigator implements OnInit {
  public routePaths: List<{component: any; path: string; friendlyName: string; parms: any; outlet: string; url?: string }>;
  currentRoute: string;
  @Output() onNavigate: EventEmitter<{ component: any; path: string; friendlyName: string; parms: any; outlet: string; url?: string }> = new EventEmitter();
  @Output() onBacktrack: EventEmitter<{ routePath: { component: any; path: string; friendlyName: string; parms: any; outlet: string; url?: string }, reduceCount: number }> = new EventEmitter();
  @Output() onLeave: EventEmitter<any> = new EventEmitter();

  constructor(private router: Router,
    private activatedRoute: ActivatedRoute,
    private breadcrumbService: BreadcrumbService,
    private applicationRef: ApplicationRef,
    private viewContainerRef: ViewContainerRef,
    private breadcrumbNavigatorService: BreadcrumbNavigatorService) {

    let subscription: Subscription = null;

    this.routePaths = new List<{ component: any; path: string; friendlyName: string; parms: any; outlet: string; url?: string }>();

    subscription = router.events
      .pipe(filter((event) => event instanceof NavigationEnd)) 
      .subscribe((e: NavigationEnd) => {

        if (this.currentRoute && e.url.length < this.currentRoute.length) {
          if (this.currentRoute.startsWith(e.url)) {

            this.currentRoute = e.url;
            this.backtrack(this.currentRoute);
            return;
          }
        }

        if (!this.currentRoute || e.url == this.currentRoute || e.url.startsWith(this.currentRoute)) {
          this.currentRoute = e.url;
          this.buildRoute(true);
        }
        else {

          this.routePaths.clear();

          subscription.unsubscribe();
          this.onLeave.emit();
        }
      });
  }

  public push(component: any, path: string, friendlyName: string, parms: any = null) {

    this.routePaths.push({ component: component, path: path, friendlyName: friendlyName, parms: parms, outlet: "breadcrumb-router-outlet" });

    if (this.currentRoute) {
      this.buildRoute(true);
    }
  }

  public pop() {
    if (this.routePaths.count) {
      this.routePaths.pop();
    }

    if (this.currentRoute) {
      this.buildRoute();
    }
  }

  getActiveRoute(routes: Route[]) : Route {
    let parts = this.removeQueryString(this.currentRoute).split("/");
    let lastRoute: Route;

    parts.forEach(p => {

      if (p.length) {
        let routeList = new List<Route>(routes);
        let route = routeList.singleOrDefault((r) => r.path == p);

        if (!route) {

          // on feature routes, there is typically a blank-named default route, so go to its children

          route = routeList.first();
          routes = route.children;
          routeList = new List<Route>(routes);
          route = routeList.single((r) => r.path == p);
        }

        if (route["_loadedRoutes"]) {
          routes = route["_loadedRoutes"];
        }
        else {
          routes = route.children;
        }

        lastRoute = route;
      }
    });

    return lastRoute;
  }

  getActivePath() : string {

    let path : string = this.currentRoute

    if (path.indexOf("?") != -1) {
      path = this.removeQueryString(path);
    }

    path = path.substring(path.lastIndexOf("/") + 1);

    return path;
  }

  removeQueryString(url: string) {

    if (url.indexOf("?") != -1) {
      url = url.substring(0, url.indexOf("?"));
    }

    return url;
  }

  buildRoute(adding: boolean = false) {
    let activatedRoute = this.activatedRoute;
    let activePath = this.getActivePath();
    let config = this.router.config;
    let activeRoute = this.getActiveRoute(config);
    let count = this.routePaths.count();
    let targetRoutePath: { component: any; path: string; friendlyName: string; parms: any; outlet: string; url?: string };
    
    if (count) {
      targetRoutePath = this.routePaths.last();
    }
    else {
      return;
    }

    if (activeRoute.path != targetRoutePath.path) {

      let lastPath: { component: any; path: string; friendlyName: string; parms: any; outlet: string; url?: string } = null;
      let targetRoute: string = this.removeQueryString(this.currentRoute);
      let childrenAddedToActiveRoute = false;
      let lastRoute: Route;

      this.routePaths.skip(Math.max(count - 2, 0)).forEach((p) => {

        let newRoute = <Route>{ path: p.path, component: p.component };
        let children = [newRoute];

        if (!childrenAddedToActiveRoute) {

          if (activeRoute["_loadedRoutes"]) {
            let loadedRoutes = <Array<Route>>activeRoute["_loadedRoutes"];
            let loadedRoute = loadedRoutes[0];

            loadedRoute.children = children;
          }
          else {
            lastRoute = activeRoute;
            childrenAddedToActiveRoute = true;

            return;
          }

          childrenAddedToActiveRoute = true;

          lastRoute = newRoute;
        }
        else {
          lastRoute.children = children;
        }

        if (!targetRoute.endsWith(p.path)) {

          if (p.parms) {
            targetRoute += "/" + p.path + "?" + queryString.stringify(p.parms);
          }
          else {
            targetRoute += "/" + p.path;
          }
        }

        lastPath = p;
      });

      if (adding) {
        lastPath.url = targetRoute;

        this.breadcrumbService.addFriendlyNameForRoute(targetRoute, lastPath.friendlyName);
      }

      this.router.navigateByUrl(targetRoute);
      this.onNavigate.emit(lastPath);
    }
  }

  backtrack(oldRoute: string) {

    let activePath = this.currentRoute.substring(this.currentRoute.lastIndexOf("/") + 1);
    let routePath = this.routePaths.single(r => r.path === activePath);
    let index = this.routePaths.indexOf(routePath);
    let count = this.routePaths.count();

    for (let x = index + 1; x < count; x++) {
      this.routePaths.pop();
    }

    this.onBacktrack.emit({ routePath: routePath, reduceCount: count - index - 1 });
  }

  ngOnInit() {
  }
}
