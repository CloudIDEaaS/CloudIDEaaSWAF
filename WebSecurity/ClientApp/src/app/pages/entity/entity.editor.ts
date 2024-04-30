import { Component, OnInit, ElementRef, ApplicationRef, Output, EventEmitter } from '@angular/core';
import { ViewBasePage } from '../viewer/viewbase.page';
import { DevOpsProvider } from '../../providers/devops.provider';
import { Router } from '@angular/router';
import { BreadcrumbService } from '@cloudideaas/ngx-breadcrumb';
import * as $ from "jquery";
declare const require: any;
const entityEditor = require("../../modules/components/entityEditor/js/entityEditor.js");

@Component({
  selector: 'entity-editor',
  templateUrl: './entity.editor.html',
  styleUrls: ['./entity.editor.scss'],
})
export class EntityEditorPage extends ViewBasePage implements OnInit {

  entityEditor: any;

  constructor(private elementRef: ElementRef,
    devOpsProvider: DevOpsProvider,
    router: Router,
    breadcrumbService: BreadcrumbService,
    applicationRef: ApplicationRef) {
      super(devOpsProvider, router, breadcrumbService, applicationRef);
  }

  ngOnInit() {
  }

  refresh() {
    this.init();
  }

  save(newRevision: boolean = false) {

    let project = this.project;

    if (newRevision) {
        project.saveAsRevision = true;
    }

    project.saveEntitiesOnly = true;

    this.backgroundSaveProjectToServer(this.project);
  }

  entityChanged(properties) {
    this.save();
  }

  init() {
    this.editEntityTest();
  }

  editEntity() {

    const jsonpath = require("JSONPath");
    let entities = this.project.entities;
    let dataItem = this.project.rootModel;
    let expression = `$..*[?(@.parentDataItem == ${ dataItem.id })]`;
    let result = jsonpath.query(entities, expression);
    let entity = result[0];
    let createEntityConfig = new Function(this.project.entityConfig);
    let config = createEntityConfig();

    this.entityEditor = (<any>$(this.elementRef.nativeElement)).entityEditor({
      entity: entity,
      slides: config.slides,
      entityChanged: (properties) => this.entityChanged(properties),
      dataTypes: config.dataTypes
    }).entityEditor("instance");
  }

  editEntityTest() {

    const jsonpath = require("JSONPath");
    let entities = this.project.entities;
    let dataItem = this.project.rootModel;
    let expression = `$..*[?(@.parentDataItem == ${ dataItem.id })]`;
    let result = jsonpath.query(entities, expression);
    let entity = result[0];

    let dataTypes = {
        1: "string",
        2: "int",
        3: "datetime"
    };

    let maskedInput = {
        name: "mask",
        createInline: (properties, state) => {

            let parentElement = state.parentElement;
            let customData = state.customData;
            let textBox;

            textBox = parentElement.appendTextBox({ "style": {
                    "valign": "baseline",
                    "padding": "0px",
                    "font-size": "14px",
                    "height": customData.cellHeight - 4 + "px"
                },
                placeholder: "Enter mask"
            });

            if (properties.data.mask) {
                textBox.val(properties.data.mask);
            }

            return textBox;
        },
        getProperties: (inlineElement, dropPanel, state) => {

            let textBox = inlineElement;
            let value = textBox.val();

            return {
                mask: value
            };
        }
    };

    let slides = {
        cases: {
            "datetime": {
                uiElements: [{
                        name: "validations",
                        createInline: (properties, state) => {

                            let parentElement = state.parentElement;
                            let customData = state.customData;
                            let textBox;

                            textBox = parentElement.appendTextBox({ "style": {
                                    "valign": "baseline",
                                    "padding": "0px",
                                    "font-size": "14px",
                                    "height": customData.cellHeight - 4 + "px"
                                },
                                readonly: "readonly",
                                placeholder: "Select validations"
                            });

                            return textBox;
                        },
                        createDropPanel: (properties, state) => {

                            let parentElement = state.parentElement;
                            let checkBoxList;
                            let checkBoxListInstance;
                            let itemCaseCallbacks = [];
                            let addItemCaseActivatedHandler = (itemCase, callback) => {

                                itemCaseCallbacks[itemCase] = callback;
                            }

                            state.addItemCaseActivatedHandler = addItemCaseActivatedHandler;

                            checkBoxList = parentElement.appendCheckboxListCarouselForm({
                                items: [{
                                        name: "required",
                                        text: "Required",
                                        value: properties.lookupSelected("required"),
                                        state: state
                                    },
                                    {
                                        name: "range",
                                        text: "Range",
                                        value: properties.lookupSelected("range"),
                                        state: state
                                    },
                                ],
                                slides: {
                                    cases: {
                                        "required": {
                                            uiElements: [{
                                                name: "errorMessage",
                                                createInline: function (properties, state) {

                                                    let parentElement = state.parentElement;
                                                    let customData = state.customData;
                                                    let textBox;

                                                    textBox = parentElement.appendTextBox({ "style": {
                                                            "valign": "top",
                                                            "margin": "0px -2px 0px 5px",
                                                            "font-size": "14px",
                                                            "height": customData.cellHeight - 2 + "px",
                                                            "width": "250px"
                                                        },
                                                        placeholder: "Enter error message"
                                                    });

                                                    if (properties.data.errorMessage) {
                                                        textBox.val(properties.data.errorMessage);
                                                    }

                                                    return textBox;
                                                },
                                                getProperties: (inlineElement, dropPanel, state) => {

                                                    let textBox = inlineElement;
                                                    let value = textBox.val();

                                                    return { errorMessage: value };
                                                }
                                            }]
                                        },
                                        "range": {

                                        }
                                    }
                                },
                                state: state
                            })

                            checkBoxListInstance = checkBoxList.itemCarouselForm("instance");
                            checkBoxListInstance.onItemSelected = (itemSelected, data, element, checked) => {

                                let items = checkBoxListInstance.items;
                                let item = items.filter(i => i.name == itemSelected);
                                let callback = itemCaseCallbacks[item[0].name];

                                callback(itemSelected, item, data, element, checked);
                            }

                            return checkBoxList;
                        },
                        getProperties: (inlineElement, dropPanel, state) => {
                            return null;
                        }
                    },
                    maskedInput
                ]
            }
        }
    };

    this.entityEditor = (<any>$(this.elementRef.nativeElement)).entityEditor({
      entity: entity,
      entityChanged: (properties) => this.entityChanged(properties),
      slides: slides,
      dataTypes: dataTypes
    }).entityEditor("instance");
  }
}
