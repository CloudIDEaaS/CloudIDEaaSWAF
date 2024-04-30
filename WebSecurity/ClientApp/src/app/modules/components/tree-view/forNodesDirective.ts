import { Directive, IterableDiffer, IterableDiffers, ChangeDetectorRef, DoCheck, TemplateRef, ViewContainerRef, ViewRef, Input, OnChanges, SimpleChanges, Output, EventEmitter } from "@angular/core";

@Directive({
  selector: '[forNodes]',
  inputs: ['forNodes']
})
export class ForNodesDirective implements DoCheck {

  private collection: any;
  private differ: IterableDiffer<any>;
  private viewMap: Map<any,ViewRef> = new Map<any,ViewRef>();
  private trackByFn: Function

  constructor(private changeDetector: ChangeDetectorRef,
    private differs: IterableDiffers,
    private template: TemplateRef<any>,
    private viewContainer: ViewContainerRef) {
  }

  @Input()
  public set forNodesOf(coll: any) {

    this.collection = coll;

    if (coll && !this.differ) {
      this.differ = this.differs.find(coll).create((value) => coll);
    }
  }

  ngDoCheck() {

    if (this.differ) {

      const changes = this.differ.diff(this.collection);

      if (changes) {

        changes.forEachAddedItem((change) => {

          const view = this.viewContainer.createEmbeddedView(this.template);
          view.context.$implicit = change.item;
          this.viewMap.set(change.item, view);
        });

        changes.forEachRemovedItem((change) => {
          const view = this.viewMap.get(change.item);
          const viewIndex = this.viewContainer.indexOf(view);
          this.viewContainer.remove(viewIndex);
          this.viewMap.delete(change.item);
        });
      }
    }
  }}
