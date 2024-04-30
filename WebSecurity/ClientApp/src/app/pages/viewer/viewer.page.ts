import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
declare const require: any;

@Component({
  selector: 'app-viewer',
  templateUrl: './viewer.page.html',
  styleUrls: ['./viewer.page.scss'],
})
export class ViewerPage implements OnInit {

  public viewType: string;

  constructor(private router: Router) { 

    let url = this.router.url;
    let parts = url.split("/");
    let queryParms = this.router["browserUrlTree"].queryParams;

    switch (parts[1]) {
      case "home": {

        switch (parts[2]) {
          case "project": {
            this.viewType = "project";
            break;
          }
          case "model": {

            if (queryParms.edit === "true") {
              this.viewType = "entity";
            }
            else {
              this.viewType = "model";
            }
        
            break;
          }
        }
      }
    }
  }

  ngOnInit() {
  }
}
