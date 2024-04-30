export class RegisterUser {
  public firstName: string;
  public lastName: string;
  public organizationName: string;
  public userName: string;
  public emailAddress: string;
  public phoneNumber: string;
  public location: string;
  public password: string;
  public passwordConfirm: string;

  constructor(options: { firstName: string, lastName: string, organizationName: string, userName: string, emailAddress: string, phoneNumber: string, location: string })
  constructor(u: any)
  {
    if (u.firstName !== undefined && u.lastName !== undefined && u.userName !== undefined && u.emailAddress !== undefined) {
      this.firstName = u.FirstName;
      this.lastName = u.LastName;
      this.userName = u.UserName;
      this.emailAddress = u.EmailAddress;
      this.location = u.location;
      this.password = "";
      this.passwordConfirm = "";
    }
    else if (u !== undefined) {
      (<any>Object).assign(this, u);
    } else {
      throw new TypeError("Unexpected arguments to RegisterUser constructor");
    }
  }

  public get userData() {
    return { firstName: this.firstName, lastName: this.lastName, organizationName: this.organizationName, userName: this.userName, emailAddress: this.userName, phoneNumber: this.phoneNumber, location: this.location, password: this.password };
  }
}
