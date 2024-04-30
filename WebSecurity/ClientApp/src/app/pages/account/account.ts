import { AfterViewInit, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AlertController } from '@ionic/angular';
import { UserProvider } from '../../providers/user.provider';

@Component({
  selector: 'page-account',
  templateUrl: 'account.html',
  styleUrls: ['./account.scss'],
})
export class AccountPage implements AfterViewInit {
  username: string;

  constructor(
    public alertCtrl: AlertController,
    public router: Router,
    public userProvider: UserProvider
  ) {

  }

  ngAfterViewInit() {
    this.getUsername();
  }

  updatePicture() {
    console.debug('Clicked to update picture');
  }

  // Present an alert with the current username populated
  // clicking OK will update the username and display it
  // clicking Cancel will close the alert and do nothing
  async changeUsername() {
    const alert = await this.alertCtrl.create({
      header: 'Change Username',
      buttons: [
        'Cancel',
        {
          text: 'Ok',
          handler: (data: any) => {
            // this.userProvider.setUsername(data.username);
            // this.getUsername();
          }
        }
      ],
      inputs: [
        {
          type: 'text',
          name: 'username',
          value: this.username,
          placeholder: 'username'
        }
      ]
    });
    await alert.present();
  }

  getUsername() {
    // this.userProvider.getUsername().then((username) => {
    //   this.username = username;
    // });
  }

  changePassword() {
    console.debug('Clicked to change password');
  }

  logout() {
    this.userProvider.logout();
    this.router.navigateByUrl('/login');
  }

  support() {
    this.router.navigateByUrl('/support');
  }
}
