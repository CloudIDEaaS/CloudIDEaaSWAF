import { Component, ElementRef } from '@angular/core';
import { NavController, NavParams } from '@ionic/angular';
import { ICellRendererComp } from "ag-grid-community";

/**
 * Generated class for the EditDeletePage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@Component({
  selector: 'edit-delete',
  templateUrl: 'edit-delete.html',
})
export class EditDeleteButtons implements ICellRendererComp {

  data: any;
  onEditClick: (data: any) => void;
  onDeleteClick: (data: any) => void;

  constructor(public navCtrl: NavController, public navParams: NavParams, private elementRef: ElementRef) {
  }

  getGui(): HTMLElement {
    return this.elementRef.nativeElement;
  }

  destroy?(): void {
  }


  refresh(params: any): boolean {
    return true;
  }

  edit() {
    this.onEditClick(this.data);
  }

  delete() {
    this.onDeleteClick(this.data);
  }

  init(params: any): void {
    this.data = params.data;
    this.onEditClick = params.onEditClick;
    this.onDeleteClick = params.onDeleteClick;
  }

  ionViewDidLoad() {
  }

}
