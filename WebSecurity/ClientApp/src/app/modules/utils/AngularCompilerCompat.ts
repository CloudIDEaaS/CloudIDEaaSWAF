function _classCallCheck2(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }
function _createClass2(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); Object.defineProperty(Constructor, "prototype", { writable: false }); return Constructor; }
function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

export var VariableAst = /*#__PURE__*/function () {
  function VariableAst(name, value, sourceSpan) {
    _classCallCheck2(this, VariableAst);

    this.name = name;
    this.value = value;
    this.sourceSpan = sourceSpan;
  }

  _createClass2(VariableAst, [{
    key: "visit",
    value: function visit(visitor, context) {
      return visitor.visitVariable(this, context);
    }
  }], [{
    key: "fromParsedVariable",
    value: function fromParsedVariable(v) {
      return new VariableAst(v.name, v.value, v.sourceSpan);
    }
  }]);

  return VariableAst;
}();

export interface CompileIdentifierMetadata {
  reference: any;
}

export interface CompileTokenMetadata {
  value?: any;
  identifier?: CompileIdentifierMetadata | CompileTypeMetadata;
}

export interface CompileDiDependencyMetadata {
  isAttribute?: boolean;
  isSelf?: boolean;
  isHost?: boolean;
  isSkipSelf?: boolean;
  isOptional?: boolean;
  isValue?: boolean;
  token?: CompileTokenMetadata;
  value?: any;
}

export declare enum LifecycleHooks {
  OnInit = 0,
  OnDestroy = 1,
  DoCheck = 2,
  OnChanges = 3,
  AfterContentInit = 4,
  AfterContentChecked = 5,
  AfterViewInit = 6,
  AfterViewChecked = 7
}

export interface CompileTypeMetadata extends CompileIdentifierMetadata {
  diDeps: CompileDiDependencyMetadata[];
  lifecycleHooks: LifecycleHooks[];
  reference: any;
}

export declare class CompilePipeMetadata {
  type: CompileTypeMetadata;
  name: string;
  pure: boolean;
  constructor({ type, name, pure }: {
    type: CompileTypeMetadata;
    name: string;
    pure: boolean;
  });
  toSummary(): CompilePipeSummary;
}

export enum CompileSummaryKind {
  Pipe = 0,
  Directive = 1,
  NgModule = 2,
  Injectable = 3
}

export interface CompileTypeSummary {
  summaryKind: CompileSummaryKind | null;
  type: CompileTypeMetadata;
}

export interface CompilePipeSummary extends CompileTypeSummary {
  name: string;
  pure: boolean;
}
