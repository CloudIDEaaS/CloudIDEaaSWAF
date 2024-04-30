import { Component, ViewChild, ElementRef, ViewChildren, QueryList, OnInit, Output, EventEmitter, ApplicationRef } from "@angular/core";
import { HierarchyComponent } from "../../modules/components/hierarchy/hierarchy";
import * as $ from "jquery";
import { IonChip, AlertController } from "@ionic/angular";
import { ValidationContext } from "../validation/validationcontext";
import { RulePolicy} from "@angularlicious/rules-engine";
import { AddNodeRule } from "../validation/addnoderule";
import { BusinessModelUIState } from "../../models/businessmodeluistate";
import { getLevel, getLevelEnumFieldName, removeSpaces } from "../../businessModelLevel";
import { ViewBasePage } from '../viewer/viewbase.page';
import { DevOpsProvider } from '../../providers/devops.provider';
import { Router } from '@angular/router';
import { BreadcrumbService } from '@cloudideaas/ngx-breadcrumb';
import { environment } from "../../../environments/environment";
declare const require: any;

@Component({
  selector: "business-model-editor",
  templateUrl: "./model.editor.html",
  styleUrls: ["./model.editor.scss"],
})
export class ModelEditorPage extends ViewBasePage implements OnInit {

  chartContainer: any;
  childCheckedValue: boolean = true;
  devMode = !environment.production;
  siblingCheckedValue: boolean;
  selectedLevelTypeValue: string;
  nodeNameTextValue: string;
  selectedNodeTextValue: string;
  addButtonToolTipValue: string;
  deleteButtonToolTipValue: string;
  statusBarTextValue: string;
  @ViewChild("businessModel", { static: true} ) businessModel: HierarchyComponent;
  @ViewChild("addNodeButton", { static: true} ) addNodeButton: any;
  @ViewChild("editPanel", { static: true} ) editPanel: any;
  @ViewChild("deleteNodeButton", { static: true} ) deleteNodeButton: any;
  @ViewChild("addButtonTooltip", { static: true} ) addButtonTooltipSpan: any;
  @ViewChild("deleteButtonTooltip", { static: true} ) deleteButtonTooltipSpan: any;
  @ViewChildren(IonChip) levelChips: QueryList<IonChip>;
  @Output() childCheckedChange = new EventEmitter();
  @Output() siblingCheckedChange = new EventEmitter();
  @Output() selectedLevelTypeChange = new EventEmitter();
  @Output() nodeNameTextChange = new EventEmitter();
  @Output() selectedNodeTextChange = new EventEmitter();
  @Output() addButtonToolTipChange = new EventEmitter();
  @Output() deleteButtonToolTipChange = new EventEmitter();
  @Output() statusBarTextChange = new EventEmitter();
  selectedLevelChip: string;
  selectedNode: any;

  constructor(private elementRef: ElementRef,
    private alertController: AlertController,
    devOpsProvider: DevOpsProvider,
    router: Router,
    breadcrumbService: BreadcrumbService,
    applicationRef: ApplicationRef) {
      super(devOpsProvider, router, breadcrumbService, applicationRef);
  }

  ngOnInit(): void {
  }

  ngDoCheck(): void {
    this.applyRules();
  }

  refresh() {
    this.init();
  }

  init() {

    let _this = this;
    let $this = $(this.elementRef.nativeElement);

    this.chartContainer = $(this.businessModel.chartContainer);

    this.selectedLevelType = "Undefined";
    this.nodeNameText = "Test";
    this.statusBarText = "Ready";

    this.chartContainer.on("click", ".node", (e) => {

      let target = $(e.target);
      let node = target.closest(".node");
      let offsetX = e.offsetX;
      let offsetY = e.offsetY;
      let width = target.width();
      let leftOffset = width - offsetX;
      let nodeData = node.data().source;
      let levelType = nodeData.title;
      let $selectedTextBox = $("#selectedTextInput");

      if (levelType === "dataitem") {
        if (leftOffset > 4 && leftOffset < 20) {
          if (offsetY > 1 && offsetY < 18) {

            let hierarchy = this.businessModel.getHierarchy("businessModel");
            this.navigateToNode(hierarchy, nodeData, true);

            return;
          }
        }
      }

      this.selectedNodeText = node.find(".title").text();
      this.selectedNode = nodeData;

      this.applyRules();
    });

    this.chartContainer.on("dblclick", ".node", (e) => {

      let target = $(e.target);
      let node = target.closest(".node");
      let offsetX = e.offsetX;
      let offsetY = e.offsetY;
      let width = target.width();
      let leftOffset = width - offsetX;
      let nodeData = node.data().source;
      let hierarchy = this.businessModel.getHierarchy("businessModel");
      let levelType = nodeData.title;
      let edit = false;

      if (levelType === "dataitem") {
        if (leftOffset > 4 && leftOffset < 20) {
          if (offsetY > 1 && offsetY < 18) {
            edit = true;
          }
        }
      }

      this.navigateToNode(hierarchy, nodeData, edit);
    });

    this.chartContainer.on("click", ".orgchart", (e) => {
      if (!$(e.target).closest(".node").length) {
        this.selectedNodeText = null;
        this.applyRules();
      }
    });

    $("input[name='node-type']").on("click", () => {
      if ($this.val() === "parent") {
        $("#edit-panel").addClass("edit-parent-node");
        $("#new-nodelist").children(":gt(0)").remove();
      } else {
        $("#edit-panel").removeClass("edit-parent-node");
      }
    });

    $("#addNodeButton").on("click", () => {

      let nodeVals = [];
      let levelType = this.selectedLevelType;
      let level = getLevel(levelType);
      let levelName = getLevelEnumFieldName(level);
      let hierarchy;

      $("#new-nodelist").find(".new-node").each((index : any, item : any) => {

        let validVal = item.value.trim();
        if (validVal.length) {
          nodeVals.push(validVal);
        }
      });

      let $node = _this.chartContainer.find(".focused");
      let nodeType = $("input[name='node-type']:checked");

      if (!nodeVals.length) {
        alert("Please input value for new node");
        return;
      }

      if (!nodeType.length) {
        alert("Please select a node type");
        return;
      }

      if (nodeType.val() !== "parent" && !$node) {
        alert("Please select one node in orgchart");
        return;
      }

      if (nodeType.val() === "parent") {

        this.businessModel.addParent(this.chartContainer.find(".node:first"), { "name": nodeVals[0], "id": this.getId() });

      }
      else if (nodeType.val() === "siblings") {

        if ($node[0].id === this.chartContainer.find(".node:first")[0].id) {
          alert("You are not allowed to directly add sibling nodes to root node");
          return;
        }

        this.businessModel.addSiblings($node, nodeVals.map(function (item) {
            return { "name": item, "relationship": "110", "id": this.getId() };
          }));
      }
      else {

        let hasChild = $node.parent().attr("colspan") > 0 ? true : false;

        if (!hasChild) {

          var rel = nodeVals.length > 1 ? "110" : "100";

          this.businessModel.addChildren($node[0], {
              children: nodeVals.map((item) => {
              return { name: item, title: levelName, className: levelName, relationship: rel, id: this.getId() };
            })
          });

          try {
          this.businessModel.showChildren($node[0]);
          }
          catch {
          }
        }
        else {

          this.businessModel.addSiblings($node.closest("tr").siblings(".nodes").find(".node:first-child")[0], {
            children: nodeVals.map((item) => {
              return { name: item, title: levelName, className: levelName, relationship: "110", id: this.getId() };
            })
          });
        }
      }

      this.save();

    });

    $("#deleteNodeButton").on("click", () => {

      var $node = $("#selected-node").data("node");

      if (!$node) {

        alert("Please select one node in orgchart");
        return;

      }
      else if ($node[0] === $(".orgchart").find(".node:first")[0]) {

        if (!window.confirm("Are you sure you want to delete the whole chart?")) {
          return;
        }

      }

      this.businessModel.removeNodes($node[0]);

      $("#selected-node").val("").data("node", null);
    });
  }

  get addButtonToolTip(): string {
    return this.addButtonToolTipValue;
  }

  set addButtonToolTip(value: string) {
    this.addButtonToolTipValue = value;
    this.addButtonToolTipChange.emit(this.addButtonToolTipValue);
  }

  get deleteButtonToolTip(): string {
    return this.deleteButtonToolTipValue;
  }

  set deleteButtonToolTip(value: string) {
    this.deleteButtonToolTipValue = value;
    this.deleteButtonToolTipChange.emit(this.deleteButtonToolTipValue);
  }

  get selectedNodeText(): string {
    return this.selectedNodeTextValue;
  }

  set selectedNodeText(value: string) {
    this.selectedNodeTextValue = value;
    this.selectedNodeTextChange.emit(this.selectedNodeTextValue);
  }

  get statusBarText(): string {
    return this.statusBarTextValue;
  }

  set statusBarText(value: string) {
    this.statusBarTextValue = value;
    this.statusBarTextChange.emit(this.statusBarTextValue);
  }

  get nodeNameText(): string {
    return this.nodeNameTextValue;
  }

  set nodeNameText(value: string) {
    this.nodeNameTextValue = value;
    this.nodeNameTextChange.emit(this.nodeNameTextValue);
  }

  get selectedLevelType(): string {
    return this.selectedLevelTypeValue;
  }

  set selectedLevelType(value: string) {

    this.selectedLevelTypeValue = value;
    this.selectLegendLevelChip(this.selectedLevelTypeValue);

    this.selectedLevelTypeChange.emit(this.selectedLevelTypeValue);
  }

  get childChecked(): boolean {
    return this.childCheckedValue;
  }

  set childChecked(value: boolean) {
    this.childCheckedValue = value;
    this.childCheckedChange.emit(this.childCheckedValue);
  }

  get siblingChecked(): boolean {
    return this.siblingCheckedValue;
  }

  set siblingChecked(value: boolean) {
    this.siblingCheckedValue = value;
    this.siblingCheckedChange.emit(this.siblingCheckedValue);
  }

  getJsonFromHierarchy(hierarchy: any)  {

    let children = hierarchy.children || [];
    let jsonChildren = [];
    let json : any  = {
        id: hierarchy.id,
        name: hierarchy.name,
        title: hierarchy.title,
        className: hierarchy.className
      };

    children.forEach(c => {
        let child = this.getJsonFromHierarchy(c);
        jsonChildren.push(child);
    });

    if (jsonChildren.length) {
      json.children = jsonChildren;
    }

    return json;
  }

  save(newRevision: boolean = false) {

    this.getProjectFromCache().then((p) => {

      let hierarchy = <any> this.businessModel.getHierarchy("businessModel");
      let displayedRootNode = this.getDisplayedRootNode(p.rootModel);
      let json = this.getJsonFromHierarchy(hierarchy.businessModel.rootNode);

      for (let p in json) {
        displayedRootNode[p] = json[p];
      }

      if (newRevision) {
        p.saveAsRevision = true;
      }

      this.backgroundSaveProjectToServer(p);

    });
  }

  saveToCache() {

    this.getProjectFromCache().then((p) => {

      let hierarchy = <any> this.businessModel.getHierarchy("businessModel");
      let displayedRootNode = this.getDisplayedRootNode(p.rootModel);
      let json = this.getJsonFromHierarchy(hierarchy.businessModel.rootNode);

      for (let p in json) {
        displayedRootNode[p] = json[p];
      }

      this.cacheProject(p);
    });
  }

  saveToServer() {

    this.getProjectFromCache().then((p) => {

      let hierarchy = <any> this.businessModel.getHierarchy("businessModel");
      let displayedRootNode = this.getDisplayedRootNode(p.rootModel);
      let json = this.getJsonFromHierarchy(hierarchy.businessModel.rootNode);

      for (let p in json) {
        displayedRootNode[p] = json[p];
      }

      this.backgroundSaveProjectToServer(p);

    });
  }

  saveRevisionToServer() {

    this.getProjectFromCache().then((p) => {

      let hierarchy = <any> this.businessModel.getHierarchy("businessModel");
      let displayedRootNode = this.getDisplayedRootNode(p.rootModel);
      let json = this.getJsonFromHierarchy(hierarchy.businessModel.rootNode);

      for (let p in json) {
        displayedRootNode[p] = json[p];
      }

      p.saveAsRevision = true;

      this.updateProject(p);

    });
  }

  applyRules() {

    let $addNodeButton = $(this.addNodeButton.nativeElement);
    let $deleteNodeButton = $(this.deleteNodeButton.nativeElement);
    let $addButtonTooltipSpan = $(this.addButtonTooltipSpan.nativeElement);
    let $deleteButtonTooltipSpan = $(this.deleteButtonTooltipSpan.nativeElement);
    let businessModelUIState = this.createBusinessModelUIState();
    let validationContext = new ValidationContext()
          .addRule(new AddNodeRule(businessModelUIState));

    validationContext.renderRules();

    validationContext.results.forEach((e) => {

      if (e.isValid) {

        switch (e.rulePolicy.name) {

          case "AddNodeIsEnabled":

              $addNodeButton.prop("disabled", false);
              $addNodeButton.removeClass("disabled");
              $addButtonTooltipSpan.css("visibility", "hidden");

              this.addButtonToolTip = null;

              break;
        }

      }
      else if (e.rulePolicy.isDisplayable) {

        let message = e.message;
        let rules = <Array<RulePolicy>> e.rulePolicy["rules"];

        if (rules) {

          message += ":\r\n\r\n";

          rules.forEach((r) => {

            if (!r.isValid && r.isDisplayable) {
              message += "\r\n" + r.message;
            }
          });
        }

        switch (e.rulePolicy.name) {

          case "AddNodeIsEnabled":

            $addNodeButton.prop("disabled", true);
            $addNodeButton.addClass("disabled");
            $addButtonTooltipSpan.css("visibility", "visible");

            this.addButtonToolTip = message;
        }
     }
    });
  }

  createBusinessModelUIState(): BusinessModelUIState {

    let businessModelUIState = new BusinessModelUIState();

    businessModelUIState.selectedNode = this.selectedNode;
    businessModelUIState.selectedLevelType = this.selectedLevelType;
    businessModelUIState.selectedLevelChip = this.selectedLevelChip;
    businessModelUIState.selectedNodeText = this.selectedNodeText;
    businessModelUIState.nodeNameText = this.nodeNameText;
    businessModelUIState.addNodeEnabled = !this.addNodeButton.nativeElement.disabled;
    businessModelUIState.deleteNodeEnabled = !this.deleteNodeButton.nativeElement.disabled;

    if (this.childChecked) {
      businessModelUIState.selectedNodeType = "child";
    }
    else if (this.siblingChecked) {
      businessModelUIState.selectedNodeType = "sibling";
    }
    else {
      businessModelUIState.selectedNodeType = null;
    }

    return businessModelUIState;
  }

  onLegendChipClick(event: Event){

    let target = <HTMLElement> event.target;
    let allChips = this.levelChips;
    let targetChip = target;

    while (targetChip.nodeName.toLowerCase() !== "ion-chip") {
      targetChip = targetChip.parentElement;
    }

    if (targetChip.innerText.includes("/")) {
      this.selectedLevelChip = removeSpaces(targetChip.innerText);
    }
    else {
      this.selectedLevelChip = targetChip.innerText;
    }

    this.selectedLevelType = this.selectedLevelChip;

    allChips.forEach((c : any) =>
    {
      $(c.el).removeClass("levelPressed");
    });

    $(targetChip).addClass("levelPressed");

    this.applyRules();
  }

  selectLegendLevelChip(level : string){

    let allChips = this.levelChips;

    allChips.forEach((c : any) =>
    {
      let levelChip = c.el.innerText;
      let levelChipNoSpaces = removeSpaces(levelChip);

      if (levelChip === level || levelChipNoSpaces === level) {
        $(c.el).addClass("levelPressed");
        this.selectedLevelChip = levelChip;
      }
      else {
        $(c.el).removeClass("levelPressed");
      }
    });

    this.applyRules();
  }

  getId() {
    return (new Date().getTime()) * 1000 + Math.floor(Math.random() * 1001);
  }


  // filter(id: number): void {

  //   var $chart = $(".orgchart");

  //   // disable the expand/collapse feature

  //   $chart.addClass("noncollapsable");

  //   // distinguish the matched nodes and the unmatched nodes according to id

  //   $chart
  //     .find(".node")
  //     .filter(function(index, node) {
  //       return (
  //         $(node)
  //           .text()
  //           .toLowerCase()
  //           .indexOf(keyWord) > -1
  //       );
  //     })
  //     .addClass("matched")
  //     .closest("table")
  //     .parents("table")
  //     .find("tr:first")
  //     .find(".node")
  //     .addClass("retained");
  //   // hide the unmatched nodes
  //   $chart.find(".matched,.retained").each(function(index, node) {
  //     $(node)
  //       .removeClass("slide-up")
  //       .closest(".nodes")
  //       .removeClass("hidden")
  //       .siblings(".lines")
  //       .removeClass("hidden");
  //     var $unmatched = $(node)
  //       .closest("table")
  //       .parent()
  //       .siblings()
  //       .find(".node:first:not(.matched,.retained)")
  //       .closest("table")
  //       .parent()
  //       .addClass("hidden");
  //     $unmatched
  //       .parent()
  //       .prev()
  //       .children()
  //       .slice(1, $unmatched.length * 2 + 1)
  //       .addClass("hidden");
  //   });
  //   // hide the redundant descendant nodes of the matched nodes
  //   $chart.find(".matched").each(function(index, node) {
  //     if (
  //       !$(node)
  //         .closest("tr")
  //         .siblings(":last")
  //         .find(".matched").length
  //     ) {
  //       $(node)
  //         .closest("tr")
  //         .siblings()
  //         .addClass("hidden");
  //     }
  //   });
  // }

  // function clearFilterResult() {
  //   $(".orgchart")
  //     .removeClass("noncollapsable")
  //     .find(".node")
  //     .removeClass("matched retained")
  //     .end()
  //     .find(".hidden")
  //     .removeClass("hidden")
  //     .end()
  //     .find(".slide-up, .slide-left, .slide-right")
  //     .removeClass("slide-up slide-right slide-left");
  // }
}
