import { ChangeDetectorRef, Pipe, PipeTransform } from '@angular/core';
import { TreeView, TreeNode } from './';
import { List, IEnumerable } from 'linq-collections';
import { AsyncPipe } from '@angular/common';
import { DebugUtils } from '../../utils/DebugUtils';
import * as $ from 'jquery';

@Pipe({ name: "onexpand"})
export class ExpandPipe extends AsyncPipe {

  constructor(_ref: ChangeDetectorRef) {

    super(_ref);

    ExpandPipe.prototype.transform = <any> this._transform;
  }

  _transform<T>(value: any, args: any[]) {

    let list = new List<any>(args);
    let parentNode = list.single(a => a.__treeNode__);
    let parentTreeNode = <TreeNode> parentNode.__treeNode__;

    parentTreeNode.childNodes = [];
    parentTreeNode.childNodes.push({ title: 'Loading...', icon: 'time' });

    parentTreeNode.rootTreeView.onexpand.subscribe({next: (n : TreeNode) => {

        if (parentTreeNode == n) {

          let children = new List<any>(n.childNodes);
          let spinster = children.first();
          let spinsterNode = <TreeNode> spinster.__treeNode__;
          let label = $(spinsterNode.element);

          let parentCol = label.parent().parent();                        // parent column

          if (!parentCol[0] || parentCol.prop("tagName").toLowerCase() != "ion-col") {
            DebugUtils.break();
          }

          let lastCol = parentCol.prev()             // column containing icon

          if (!lastCol[0] || lastCol.prop("tagName").toLowerCase() != "ion-col") {
            DebugUtils.break();
          }

          let icon = <HTMLLabelElement> lastCol.children()[0];   // icon itself

          if (!icon || icon.tagName.toLowerCase() != "ion-icon") {
            DebugUtils.break();
          }

          super.transform(value);
        }
      },
      error : (e) => {
        throw e;
      }});
  }
}

