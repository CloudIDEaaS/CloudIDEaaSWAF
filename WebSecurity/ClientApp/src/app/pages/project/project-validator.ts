import {
  Validators,
  FormGroup,
  FormControl,
  AbstractControl,
  ValidatorFn
} from "@angular/forms";
import { Injectable, Component, ViewChild } from "@angular/core";
import { ValidationMap } from "../../modules/utils/ValidationMap";
import { Project } from '../../models/project.model';
import { IonInput } from '@ionic/angular';

@Injectable()
export class ProjectValidator {
  projectForm: FormGroup;
  validationMap: ValidationMap;
  project: Project;

  constructor() {
    // kn - todo
    // translateService.get(["NAME_IS_REQUIRED", "MINIMUM_OF_2_CHARACTERS", "MAXIMUM_OF_100_CHARACTERS", "DESCRIPTION_IS_REQUIRED", "MINIMUM_OF_20_CHARACTERS"]).subscribe(values => {

    let values = {
      PROJECT_NAME_IS_REQUIRED: "Project name is required",
      APPLICATION_NAME_IS_REQUIRED: "Application name is required",
      NAME_FORMAT: "Name must start with a letter and only contain letters, numbers, and spaces",
      MAXIMUM_OF_100_CHARACTERS: "A maximum of 100 characters required",
      MINIMUM_OF_2_CHARACTERS: "A minimum of 2 characters required",
    };

    this.validationMap = new ValidationMap([
      {
        projectName: [
          {
            required: {
              function: Validators.required,
              message: values["PROJECT_NAME_IS_REQUIRED"]
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
            pattern: {
              function: Validators.pattern(/^[a-zA-Z_ ][a-zA-Z_0-9 ]*$/),
              message: values["NAME_FORMAT"]
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
        applicationName: [
          {
            required: {
              function: Validators.required,
              message: values["APPLICATION_NAME_IS_REQUIRED"]
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
            pattern: {
              function: Validators.pattern(/^[a-zA-Z_ ][a-zA-Z_0-9 ]*$/),
              message: values["NAME_FORMAT"]
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
        form: [
          {
            // TODO - enable by returning non-null from function and setting message
            projectValidator: {
              function: this.projectFormValidator(),
              message: null
            }
          }
        ]
      }
    ]);
  }

  createProjectForm(project: Project): FormGroup {

    this.project = project;

    this.projectForm = new FormGroup(
      {
        projectName: new FormControl(
          project.projectName,
          this.validationMap.get("projectName").functions
        ),
        applicationName: new FormControl(
          project.applicationName,
          this.validationMap.get("applicationName").functions
        )
      },
      this.validationMap.get("form").functions
    );

    return this.projectForm;
  }

  getControl(fieldName: string): FormControl {
    return <FormControl>this.projectForm.get(fieldName);
  }

  hasValidationErrors(
    fieldName: string,
    checkTouched: boolean = true
  ): boolean {

    let hasError = false;

    if (this.projectForm) {
      let control = <FormControl>this.projectForm.get(fieldName);

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
    }

    return hasError;
  }

  getValidationErrors(
    fieldName: string,
    checkTouched: boolean = true
  ): string[] {
    let errors: string[] = [];
    let control = <FormControl>this.projectForm.get(fieldName);

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
          errors.push(validatorEntry.message);
        }
      });

    return errors;
  }

  getValidationErrorsText(fieldName: string): string {

    let control = <FormControl>this.projectForm.get(fieldName);
    let errors: string[] = [];
    let errorsObject = control.errors;

    Object.getOwnPropertyNames(errorsObject).forEach(p => {
      this.validationMap.map
        .get(fieldName)
        .entryMap.forEach((validatorEntry, name) => {
          if (name === p || name.toLowerCase() === p) {
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

  projectFormValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {

      if (!this.projectForm) {
        return;
      }

      if (this.projectForm.get("projectName")) {
        this.project.projectName = this.projectForm.get("projectName").value;
      }

      if (this.projectForm.get("applicationName")) {
        this.project.applicationName = this.projectForm.get("applicationName").value;
      }

      return null;
    };
  }
}
