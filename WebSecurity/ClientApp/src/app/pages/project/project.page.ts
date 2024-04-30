import { Component, OnInit, ElementRef, ApplicationRef, ViewChild } from '@angular/core';
import { ViewBasePage } from '../viewer/viewbase.page';
import { DevOpsProvider } from '../../providers/devops.provider';
import { Router } from '@angular/router';
import { BreadcrumbService } from '@cloudideaas/ngx-breadcrumb';
import { NgForm, FormGroup } from '@angular/forms';
import { ToastController, LoadingController, IonInput } from '@ionic/angular';
import { ProjectValidator } from './project-validator';
import { Project } from '../../models/project.model';
import { ProjectState } from '../../models/projectstate';

@Component({
  selector: 'project',
  templateUrl: './project.page.html',
  styleUrls: ['./project.page.scss'],
})
export class ProjectPage extends ViewBasePage implements OnInit {
  submitted: boolean;
  projectForm: FormGroup;
  @ViewChild("projectName", { static: true }) projectName: IonInput;
  @ViewChild("applicationName", { static: true }) applicationName: IonInput;

  constructor(private elementRef: ElementRef,
    public toastCtrl: ToastController,
    public loadingController: LoadingController,
    public projectValidator: ProjectValidator,
    devOpsProvider: DevOpsProvider,
    router: Router,
    breadcrumbService: BreadcrumbService,
    applicationRef: ApplicationRef) {

      super(devOpsProvider, router, breadcrumbService, applicationRef);

      this.project = new Project({ projectName: "", applicationName: "" });
      this.projectForm = this.projectValidator.createProjectForm(this.project);
  }

  ngOnInit() {
  }

  init() {

  }

  refresh() {
    this.init();
  }

  onProjectNameFocusOut(event: Event) {

    if (!this.projectValidator.hasValidationErrors("projectName"))
    {
      this.applicationName.value = this.projectName.value;
    }
  }

  submit(form: NgForm)
  {
    this.submitted = true;

    if (form.valid) {

      let loadingController = this.loadingController.create({message : "Creating project, please wait..."}).then(loading => {

        loading.present();

        this.devOpsProvider.createProject(this.project).subscribe((resp) => {

          setTimeout(() => {

            this.router.navigateByUrl("devops");
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
