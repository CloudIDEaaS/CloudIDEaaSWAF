export class LoginUser {
  public userName: string;
  public password: string;

  constructor(options: { userName: string })
  constructor(u: any) {
    if (u.userName !== undefined) {
      this.userName = u.userName;
      this.password = "";
    }
    else if (u !== undefined) {
      (<any>Object).assign(this, u);
    } else {
      throw new TypeError("Unexpected arguments to LoginUser constructor");
    }
  }

  public get userData() {
    return { userName: this.userName, password: this.password };
  }
}
