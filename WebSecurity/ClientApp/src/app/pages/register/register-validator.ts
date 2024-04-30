import {
  Validators,
  FormGroup,
  FormControl,
  AbstractControl,
  ValidatorFn,
  ValidationErrors
} from "@angular/forms";
import { Injectable, Component } from "@angular/core";
import { ValidationMap } from "../../../app/modules/utils/ValidationMap";
import { RegisterUser } from "../../models/registeruser.model";
import { h } from "ionicons/dist/types/stencil-public-runtime";

@Injectable()
export class RegistrationValidator {
  registrationForm: FormGroup;
  validationMap: ValidationMap;
  registerUser: RegisterUser;

  constructor() {
    // kn - todo
    // translateService.get(["NAME_IS_REQUIRED", "MINIMUM_OF_2_CHARACTERS", "MAXIMUM_OF_100_CHARACTERS", "DESCRIPTION_IS_REQUIRED", "MINIMUM_OF_20_CHARACTERS"]).subscribe(values => {

    let values = {
      NAME_IS_REQUIRED: "First/last name is required",
      MINIMUM_OF_2_CHARACTERS: "A minimum of 2 characters required",
      MAXIMUM_OF_100_CHARACTERS: "A maximum of 100 characters required",
      LOCATION_IS_REQUIRED: "Location is required",
      MINIMUM_OF_10_CHARACTERS: "A minimum of 10 characters required",
      PHONE_NUMBER_IS_REQUIRED: "Phone number is required",
      PHONE_NUMBER_FORMAT: "Phone number format should be (000) 000-0000",
      USER_NAME_IS_REQUIRED: "User name is required",
      USER_NAME_FORMAT: "User name should be valid email",
      PASSWORD_IS_REQUIRED: "Password is required",
      PASSWORD_FORMAT: "Password should contain a minimum of 1 uppercase, 1 lowercase, 1 number, and 1 special character (@$!%*#?&)",
      PASSWORD_MATCH: "Passwords must match",
    };

    this.validationMap = new ValidationMap([
      {
        firstName: [
          {
            required: {
              function: Validators.required,
              message: values["NAME_IS_REQUIRED"]
            }
          },
          {
            minLength: {
              function: Validators.minLength(2),
              message: values["MINIMUM_OF_2_CHARACTERS"]
            }
          },
          {
            maxLength: {
              function: Validators.maxLength(100),
              message: values["MAXIMUM_OF_100_CHARACTERS"]
            }
          },
          {
            // TODO - enable by returning non-null from function and setting message
            custom: {
              function: this.nameValidator(),
              message: null
            }
          }
        ]
      },
      {
        lastName: [
          {
            required: {
              function: Validators.required,
              message: values["NAME_IS_REQUIRED"]
            }
          },
          {
            minLength: {
              function: Validators.minLength(2),
              message: values["MINIMUM_OF_2_CHARACTERS"]
            }
          },
          {
            maxLength: {
              function: Validators.maxLength(100),
              message: values["MAXIMUM_OF_100_CHARACTERS"]
            }
          },
          {
            // TODO - enable by returning non-null from function and setting message
            custom: {
              function: this.nameValidator(),
              message: null
            }
          }
        ]
      },
      {
        organizationName: [
          {
            max: {
              function: Validators.maxLength(100),
              message: values["MAXIMUM_OF_100_CHARACTERS"]
            }
          },
          {
            // TODO - enable by returning non-null from function and setting message
            custom: {
              function: this.organizationNameValidator(),
              message: null
            }
          }
        ]
      },
      {
        location: [
          {
            required: {
              function: Validators.required,
              message: values["LOCATION_IS_REQUIRED"]
            }
          },
          {
            minLength: {
              function: Validators.minLength(10),
              message: values["MINIMUM_OF_10_CHARACTERS"]
            }
          },
          {
            maxLength: {
              function: Validators.maxLength(100),
              message: values["MAXIMUM_OF_100_CHARACTERS"]
            }
          },
          {
            // TODO - enable by returning non-null from function and setting message
            custom: {
              function: this.locationValidator(),
              message: null
            }
          }
        ]
      },
      {
        phoneNumber: [
          {
            required: {
              function: Validators.required,
              message: values["PHONE_NUMBER_IS_REQUIRED"]
            }
          },
          {
            minLength: {
              function: Validators.minLength(10),
              message: values["MINIMUM_OF_10_CHARACTERS"]
            }
          },
          {
            pattern: {
              function: Validators.pattern(/^\(\d\d\d\) \d\d\d\-\d\d\d\d$/),
              message: values["PHONE_NUMBER_FORMAT"]
            }
          },
          {
            // TODO - enable by returning non-null from function and setting message
            custom: {
              function: this.phoneNumberValidator(),
              message: null
            }
          }
        ]
      },
      {
        userName: [
          {
            required: {
              function: Validators.required,
              message: values["USER_NAME_IS_REQUIRED"]
            }
          },
          {
            email: {
              function: Validators.email,
              message: values["USER_NAME_FORMAT"]
            }
          },
          {
            // TODO - enable by returning non-null from function and setting message
            custom: {
              function: this.userNameValidator(),
              message: null
            }
          }
        ]
      },
      {
        password: [
          {
            required: {
              function: Validators.required,
              message: values["PASSWORD_IS_REQUIRED"]
            }
          },
          {
            pattern: {
              function: Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]/),
              message: values["PASSWORD_FORMAT"]
            }
          },
          {
            minLength: {
              function: Validators.minLength(10),
              message: values["MINIMUM_OF_10_CHARACTERS"]
            }
          },
          {
            // TODO - enable by returning non-null from function and setting message
            custom: {
              function: this.passwordValidator(),
              message: "null"
            }
          }
        ]
      },
      {
        passwordConfirm: [
          {
            pattern: {
              function: Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]/),
              message: values["PASSWORD_FORMAT"]
            }
          },
          {
            minLength: {
              function: Validators.minLength(10),
              message: values["MINIMUM_OF_10_CHARACTERS"]
            }
          },
          {
            passwordConfirmValidator: {
              function: this.passwordConfirmValidator(),
              message: values["PASSWORD_MATCH"]
            }
          }
        ]
      },
      {
        form: [
          {
            // TODO - enable by returning non-null from function and setting message
            registrationValidator: {
              function: this.registrationFormValidator(),
              message: null
            }
          }
        ]
      }
    ]);
  }

  createRegistrationForm(registerUser: RegisterUser): FormGroup {

    this.registerUser = registerUser;

    this.registrationForm = new FormGroup(
      {
        firstName: new FormControl(
          registerUser.firstName,
          this.validationMap.get("firstName").functions
        ),
        lastName: new FormControl(
          registerUser.lastName,
          this.validationMap.get("lastName").functions
        ),
        organizationName: new FormControl(
          registerUser.organizationName,
          this.validationMap.get("organizationName").functions
        ),
        location: new FormControl(
          registerUser.location,
          this.validationMap.get("location").functions
        ),
        phoneNumber: new FormControl(
          registerUser.phoneNumber,
          this.validationMap.get("phoneNumber").functions
        ),
        userName: new FormControl(
          registerUser.userName,
          this.validationMap.get("userName").functions
        ),
        password: new FormControl(
          registerUser.password,
          this.validationMap.get("password").functions
        ),
        passwordConfirm: new FormControl(
          registerUser.passwordConfirm,
          this.validationMap.get("passwordConfirm").functions
        )
      },
      this.validationMap.get("form").functions
    );

    return this.registrationForm;
  }

  getControl(fieldName: string): FormControl {
    return <FormControl>this.registrationForm.get(fieldName);
  }

  hasValidationErrors(
    fieldName: string,
    checkTouched: boolean = true
  ): boolean {
    let control = <FormControl>this.registrationForm.get(fieldName);
    let hasError = false;

    if (!control) {
      return hasError;
    }

    this.validationMap.map
      .get(fieldName)
      .entryMap.forEach((validatorEntry, name) => {
        let touched = checkTouched ? control.touched : true;
        let code = validatorEntry.function.name;
        let field = fieldName;

        if (!code) {
          code = name;
        }

        if (touched && (control.hasError(code) || control.hasError(code.toLowerCase()))) {
          hasError = true;
          return false;
        }
      });

    return hasError;
  }

  getValidationErrors(
    fieldName: string,
    checkTouched: boolean = true
  ): string[] {
    let errors: string[] = [];
    let control = <FormControl>this.registrationForm.get(fieldName);

    if (!control) {
      return errors;
    }

    this.validationMap.map
      .get(fieldName)
      .entryMap.forEach((validatorEntry, name) => {
        let touched = checkTouched ? control.touched : true;
        let code = validatorEntry.function.name;
        let field = fieldName;

        if (touched && (control.hasError(code) || control.hasError(code.toLowerCase()))) {
          errors.push(validatorEntry.message);
        }
      });

    return errors;
  }

  getValidationErrorsText(fieldName: string): string {
    let control = <FormControl>this.registrationForm.get(fieldName);
    let errors: string[] = [];
    let errorsObject : ValidationErrors;

    if (!control) {
      return "";
    }

    errorsObject = control.errors;

    Object.getOwnPropertyNames(errorsObject).forEach(p => {
      this.validationMap.map
        .get(fieldName)
        .entryMap.forEach((validatorEntry, name) => {
          if (name == p || name.toLowerCase() == p) {
            errors.push(validatorEntry.message);
          }
        });
    });

    return errors.join("\r\n");
  }

  nameValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      return null;
    };
  }

  locationValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      return null;
    };
  }

  organizationNameValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      return null;
    };
  }

  phoneNumberValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      return null;
    };
  }

  userNameValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      return null;
    };
  }

  passwordValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      return null;
    };
  }

  passwordConfirmValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {

      if (control && control.parent && control.parent.controls["password"]) {

        let confirmPassword = control.value;
        let password = control.parent.controls["password"].value;

        if (confirmPassword !== password) {
          return { "passwordConfirmValidator": true };
        }
      }

      return null;
    };
  }

  registrationFormValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {

      if (!this.registrationForm) {
        return;
      }

      if (this.registrationForm.get("firstName")) {
        this.registerUser.firstName = this.registrationForm.get("firstName").value;
      }

      if (this.registrationForm.get("lastName")) {
        this.registerUser.lastName = this.registrationForm.get("lastName").value;
      }

      if (this.registrationForm.get("organizationName")) {
        this.registerUser.organizationName = this.registrationForm.get("organizationName").value;
      }

      if (this.registrationForm.get("userName")) {
        this.registerUser.userName = this.registrationForm.get("userName").value;
        this.registerUser.emailAddress = this.registrationForm.get("userName").value;
      }

      if (this.registrationForm.get("phoneNumber")) {
        this.registerUser.phoneNumber = this.registrationForm.get("phoneNumber").value;
      }

      if (this.registrationForm.get("password") && this.registrationForm.get("passwordConfirm")) {

        let password = this.registrationForm.get("password").value;
        let passwordConfirm = this.registrationForm.get("passwordConfirm").value;

        if (password === passwordConfirm) {
          this.registerUser.password = this.registrationForm.get("password").value;
        }
      }

      return null;
    };
  }
}
