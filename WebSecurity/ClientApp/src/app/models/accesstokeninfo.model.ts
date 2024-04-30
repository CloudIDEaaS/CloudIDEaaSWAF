export class AccessTokenInfo {
  public accessToken: string;
  public tokenType: string;
  public expiresIn: string;
  public refreshToken: string;
  public error: string;

  constructor(c: any)
  {
    if (c.access_token !== undefined || c.error !== undefined) {
      this.accessToken = c.access_token;
      this.tokenType = c.token_type;
      this.expiresIn = c.expires_in;
      this.refreshToken = c.refreshToken;
      this.error = c.error;
    }
    else if (c !== undefined) {
      (<any>Object).assign(this, c);
    } else {
      throw new TypeError("Unexpected arguments to Token constructor")
    }
  }
}
