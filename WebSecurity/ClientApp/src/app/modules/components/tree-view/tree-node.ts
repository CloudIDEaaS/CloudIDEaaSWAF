import { TreeView } from './';
import { DomSanitizer, SafeResourceUrl, SafeUrl, SafeStyle } from '@angular/platform-browser';

export class TreeNode {

    private _childNodes : any[];
    public title : string;
    public icon : string;
    public image : string;
    public style : SafeStyle;
    public isExpanded : boolean;
    public isSelected : boolean;
    public parentNode : any;
    public previousNode : any;
    public nextNode : any;
    public object : any;
    public element : HTMLLabelElement;
    public parentTreeView: TreeView;
    public onTitleChange: (node: TreeNode, title: string) => Promise<any>;

    constructor(public rootTreeView: TreeView) {
        this.isExpanded = false;
        this.isSelected = false;
    }

    public get hasVisibleChildren() : boolean {

        if (this.isExpanded && this.childNodes && this.childNodes.length) {
          return true;
        }
        else {
          return false;
        }
    }

    public get previousVisibleNode() : any
    {
        let node : any;

        if (this.previousNode) {

            let previousNode = this.previousNode;
            let previousTreeNode = <TreeNode> previousNode.__treeNode__;

            if (previousTreeNode.hasVisibleChildren) {

              while (previousTreeNode && previousTreeNode.hasVisibleChildren) {

                previousNode = previousTreeNode.lastChildNode;

                if (previousNode) {
                  previousTreeNode = previousNode.__treeNode__;
                }
              }

              node = previousNode;
            }
            else {
                node = previousNode;
            }
        }
        else if (this.parentNode) {

            let previousNode = this.parentNode;

            node = previousNode;
        }

        return node;
    }

    public raiseChangeTitle(title: string) : Promise<any> {
      return this.onTitleChange(this.object, title);
    }

    public get firstChildNode() : any {

      if (this.childNodes && this.childNodes.length) {
        let firstChildNode = this.childNodes[0];

        return firstChildNode;
      }
      else {
        return null;
      }
    }

    public get lastChildNode() : any {

      if (this.childNodes && this.childNodes.length) {
        let firstChildNode = this.childNodes[this.childNodes.length - 1];

        return firstChildNode;
      }
      else {
        return null;
      }
    }

    public get nextVisibleNode() : any {
        let node : any;

        if (this.hasVisibleChildren) {

            let nextNode = this.childNodes[0];

            node = nextNode;
        }
        else if (this.nextNode) {

            let nextNode = this.nextNode;

            node = nextNode;
        }
        else if (this.parentNode) {

            let parentNode = this.parentNode;

            while (parentNode != null) {

              if (parentNode.__treeNode__.nextNode) {

                let nextNode = parentNode.__treeNode__.nextNode;

                node = nextNode;
                break;
              }

              parentNode = parentNode.__treeNode__.parentNode;
            }
        }

        return node;
    }

    public get childNodes(){
        return this._childNodes;
    }

    public set childNodes(nodes : any[]){
        this._childNodes = nodes;
    }

    public forEachChild(fn : (n : any) => void) {

        if (this.childNodes)
        {
            this.childNodes.forEach(n => {
                fn(n);
            });
        }
    }

    public forEachDescendant(fn : (n : any) => void) {

        this.forEachChild(n => {
            fn(n);

            n.__treeNode__.forEachDescendant(n2 => {
                fn(n2);
            });
        });
    }
}
