import { Component, OnInit, Input, ViewChild } from "@angular/core";
import { NgForm, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { UserProvider } from "../../providers/user.provider";
import { RegistrationValidator } from './register-validator';
import { RegisterUser } from '../../models/registeruser.model';
import { ToastController, LoadingController, IonInput } from '@ionic/angular';

@Component({
  selector: "page-register",
  templateUrl: "register.html",
  styleUrls: ["./register.scss"],
})
export class RegisterPage implements OnInit {
  registerUser: RegisterUser;
  submitted = false;
  registrationForm: FormGroup;
  @ViewChild("#passwordInput", { static: true }) passwordInput: IonInput;
  barLabel: "Password strength:";
  myColors = ['#DD2C00', '#FF6D00', '#FFD600', '#AEEA00', '#00C853'];

  get formPassword() {
    let password = this.registrationForm.get("password");
    return password.value;
  }

  get formPasswordConfirm() {
    let password = this.registrationForm.get("passwordConfirm");
    return password.value;
  }

  constructor(
    public router: Router,
    public toastCtrl: ToastController,
    public loadingController: LoadingController,
    public userProvider: UserProvider,
    public registrationValidator: RegistrationValidator
  )
  {
    this.registerUser = new RegisterUser({ firstName: "", lastName: "", userName: "", organizationName: "", emailAddress: "", phoneNumber: "", location: "" });
    this.registrationForm = this.registrationValidator.createRegistrationForm(this.registerUser);
  }

  ngOnInit() {
  }

  submit(form: NgForm)
  {
    this.submitted = true;

    if (form.valid) {

      let loadingController = this.loadingController.create({message : "Registering you, please wait..."}).then(loading => {

        loading.present();

        this.userProvider.register(this.registerUser).subscribe((resp) => {

          setTimeout(() => {

            this.router.navigateByUrl("emailverification");
            loading.dismiss();

          }, 100);

        }, (err) => {

          let message = err.message;

          if (err.error) {
            if (typeof err.error === "string") {
              message = err.error;
            }
          }

          let toastController = this.toastCtrl.create({message: message, duration: 3000,position: 'top'}).then(toast => {
            toast.present();
          });

          loading.dismiss();
        });
      });
    }
  }
}
