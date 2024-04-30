// import { TranslateService } from "@ngx-translate/core";
// import { Authorize } from "../modules/utils/AuthorizeDecorator";
import { Component, NgZone, ViewChild } from "@angular/core";
import { List } from "linq-javascript";

export class User {
  public userId: string;
  public userFirstName: string;
  public userLastName: string;
  public userName: string;
  public userEmailAddress: string;

  constructor(options: { userId: string, firstName: string, lastName: string, userName: string, emailAddress: string })
  constructor(u: any)
  {
    if (u.id !== undefined && u.firstName !== undefined && u.lastName !== undefined && u.userName !== undefined && u.emailAddress !== undefined) {
      this.userId = u.UserId;
      this.userFirstName = u.FirstName;
      this.userLastName = u.LastName;
      this.userName = u.UserName;
      this.userEmailAddress = u.EmailAddress;
    }
    else if (u !== undefined) {
      (<any>Object).assign(this, u);
    } else {
      throw new TypeError("Unexpected arguments to Course constructor")
    }
  }

  public get userData() {
    return { userId: this.userId, userFirstName: this.userFirstName, userLastName: this.userLastName, userName: this.userName, userEmailAddress: this.userEmailAddress };
  }
}
