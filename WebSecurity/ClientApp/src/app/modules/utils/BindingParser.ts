import { AST, AstVisitor, ParseError, ParseSourceSpan, ASTWithSource, Call, SafeCall, SafeKeyedRead, ThisReceiver, Unary } from '@angular/compiler';
import { CompilePipeSummary, VariableAst } from './AngularCompilerCompat';

class RecursiveAstVisitor implements AstVisitor  {

  visitUnary?(ast: Unary, context: any) {
  }
  visitThisReceiver?(ast: ThisReceiver, context: any) {
  }
  visitSafeKeyedRead(ast: SafeKeyedRead, context: any) {
  }
  visitCall(ast: Call, context: any) {

  }
  visitSafeCall(ast: SafeCall, context: any) {

  }
  visitASTWithSource?(ast: ASTWithSource, context: any) {
    this.visitAll([ast.ast], null);
  }
  visit?(ast: AST, context?: any) {
    ast.visit(this);
  }

  visitBinary (ast, context) {
      ast.left.visit(this);
      ast.right.visit(this);
      return null;
  }

  visitChain (ast, context) {
    return this.visitAll(ast.expressions, context);
  }

  visitConditional (ast, context) {
      ast.condition.visit(this);
      ast.trueExp.visit(this);
      ast.falseExp.visit(this);
      return null;
  }

  visitPipe (ast, context) {
      ast.exp.visit(this);
      this.visitAll(ast.args, context);
      return null;
  };

  visitFunctionCall(ast, context) {
      ((ast.target)).visit(this);
      this.visitAll(ast.args, context);
      return null;
  }

  visitImplicitReceiver(ast, context) {
     return null;
  }

  visitInterpolation(ast, context) {
      return this.visitAll(ast.expressions, context);
  }

  visitKeyedRead(ast, context) {
      ast.obj.visit(this);
      ast.key.visit(this);
      return null;
  }

  visitKeyedWrite(ast, context) {
      ast.obj.visit(this);
      ast.key.visit(this);
      ast.value.visit(this);
      return null;
  }

  visitLiteralArray(ast, context) {
      return this.visitAll(ast.expressions, context);
  }

  visitLiteralMap(ast, context)
  {
     return this.visitAll(ast.values, context);
  }

  visitLiteralPrimitive(ast, context) {
    return null;
  }

  visitMethodCall (ast, context) {
      ast.receiver.visit(this);
      return this.visitAll(ast.args, context);
  }

  visitPrefixNot(ast, context) {
      ast.expression.visit(this);
      return null;
  }

  visitNonNullAssert(ast, context) {
      ast.expression.visit(this);
      return null;
  }

  visitPropertyRead(ast, context) {
      ast.receiver.visit(this);
      return null;
  }

  visitPropertyWrite(ast, context) {
      ast.receiver.visit(this);
      ast.value.visit(this);
      return null;
  }

  visitSafePropertyRead(ast, context) {
      ast.receiver.visit(this);
      return null;
  }

  visitSafeMethodCall(ast, context) {
      ast.receiver.visit(this);
      return this.visitAll(ast.args, context);
  }

  visitAll(asts, context) {
      let __this = this;
      asts.forEach(function (ast) { return ast.visit(__this, context); });
      return null;
  }

  visitQuote(ast, context) {
     return null;
  }
}

export class PipeCollector extends RecursiveAstVisitor {

  pipes: Map<any, any>;

  constructor() {
    super();
    this.pipes = new Map();
  }

  visitPipe(ast, context) {
      this.pipes.set(ast.name, ast);
      ast.exp.visit(this);
      this.visitAll(ast.args, context);
      return null;
  }
}

class EmptyExpr extends AST {

  constructor() {
      super(undefined, null);
  }

  visit(visitor, context) {
      if (context === void 0) { context = null; }
      // do nothing
  }
}

const _SELECTOR_REGEXP = new RegExp('(\\:not\\()|' + //":not("
    '([-\\w]+)|' + // "tag"
    '(?:\\.([-\\w]+))|' + // ".class"
    '(?:\\[([-.\\w*]+)(?:=([\"\']?)([^\\]\"\']*)\\5)?\\])|' + // "[name]", "[name=value]",
    '(\\))|' + // ")"
    '(\\s*,\\s*)', // ","
'g');

function calcPossibleSecurityContexts(registry, selector, propName, isAttribute) {
  var /** @type {?} */ ctxs = [];
  CssSelector.parse(selector).forEach(function (selector) {
      var /** @type {?} */ elementNames = selector.element ? [selector.element] : registry.allKnownElementNames();
      var /** @type {?} */ notElementNames = new Set(selector.notSelectors.filter(function (selector) { return selector.isElementSelector(); })
          .map(function (selector) { return selector.element; }));
      var /** @type {?} */ possibleElementNames = elementNames.filter(function (elementName) { return !notElementNames.has(elementName); });
      ctxs.push.apply(ctxs, possibleElementNames.map(function (elementName) { return registry.securityContext(elementName, propName, isAttribute); }));
  });
  return ctxs.length === 0 ? [SecurityContext.NONE] : Array.from(new Set(ctxs)).sort();
}

function splitAtColon(input, defaultValues) {
  return _splitAt(input, ':', defaultValues);
}

function mergeNsAndName(prefix, localName) {
  return prefix ? ":" + prefix + ":" + localName : localName;
}

function _splitAt(input, character, defaultValues) {
  var /** @type {?} */ characterIndex = input.indexOf(character);
  if (characterIndex == -1)
      return defaultValues;
  return [input.slice(0, characterIndex).trim(), input.slice(characterIndex + 1).trim()];
}

function splitAtPeriod(input, defaultValues) {
  return _splitAt(input, '.', defaultValues);
}

enum SecurityContext {
  NONE = 0,
  HTML = 1,
  STYLE = 2,
  SCRIPT = 3,
  URL = 4,
  RESOURCE_URL = 5
}

enum PropertyBindingType {
  /**
     * A normal binding to a property (e.g. `[property]="expression"`).
     */
  Property =  0,
  /**
     * A binding to an element attribute (e.g. `[attr.name]="expression"`).
     */
  Attribute =  1,
  /**
     * A binding to a CSS class (e.g. `[class.name]="condition"`).
     */
  Class =  2,
  /**
     * A binding to a style rule (e.g. `[style.rule]="expression"`).
     */
  Style =  3,
  /**
     * A binding to an animation reference (e.g. `[animate.key]="expression"`).
     */
  Animation =  4,
}

/**
 * A binding for an element property (e.g. `[property]="expression"`) or an animation trigger (e.g.
 * `[\@trigger]="stateExp"`)
 */
class BoundElementPropertyAst {

  isAnimation: boolean;

  constructor (public name, public type, public securityContext, public value, public unit, public sourceSpan) {
      this.isAnimation = this.type === PropertyBindingType.Animation;
  }

  visit(visitor, context) {
      return visitor.visitElementProperty(this, context);
  }
}

const PROPERTY_PARTS_SEPARATOR = '.';
const ATTRIBUTE_PREFIX = 'attr';
const CLASS_PREFIX = 'class';
const STYLE_PREFIX = 'style';
const ANIMATE_PROP_PREFIX = 'animate-';

enum BoundPropertyType {
    DEFAULT = 0,
    LITERAL_ATTR = 1,
    ANIMATION = 2
}

/**
 * Represents a parsed property.
 */
export class BoundProperty {

  isAnimation: boolean;
  isLiteral: boolean;

  constructor(public name, public expression, public type, public sourceSpan) {
      this.isLiteral = this.type === BoundPropertyType.LITERAL_ATTR;
      this.isAnimation = this.type === BoundPropertyType.ANIMATION;
  }
}

enum ParseErrorLevel {
  WARNING = 0,
  ERROR = 1
}

enum TagContentType {
  RAW_TEXT = 0,
  ESCAPABLE_RAW_TEXT = 1,
  PARSABLE_DATA = 2
}

class HtmlTagDefinition {

  closedByChildren;
  closedByParent : boolean;
  canSelfClose : boolean;
  isVoid: boolean;
  requiredParents;
  parentToAdd;
  implicitNamespacePrefix;
  contentType;
  ignoreFirstLf;

  constructor(_a) {
      var _b = _a === void 0 ? {} : _a, closedByChildren = _b.closedByChildren, requiredParents = _b.requiredParents, implicitNamespacePrefix = _b.implicitNamespacePrefix, _c = _b.contentType, contentType = _c === void 0 ? TagContentType.PARSABLE_DATA : _c, _d = _b.closedByParent, closedByParent = _d === void 0 ? false : _d, _e = _b.isVoid, isVoid = _e === void 0 ? false : _e, _f = _b.ignoreFirstLf, ignoreFirstLf = _f === void 0 ? false : _f;
      let __this = this;
      this.closedByChildren = {};
      this.closedByParent = false;
      this.canSelfClose = false;
      if (closedByChildren && closedByChildren.length > 0) {
          closedByChildren.forEach(function (tagName) { return __this.closedByChildren[tagName] = true; });
      }
      this.isVoid = isVoid;
      this.closedByParent = closedByParent || isVoid;
      if (requiredParents && requiredParents.length > 0) {
          this.requiredParents = {};
          // The first parent is the list is automatically when none of the listed parents are present
          this.parentToAdd = requiredParents[0];
          requiredParents.forEach(function (tagName) { return __this.requiredParents[tagName] = true; });
      }
      this.implicitNamespacePrefix = implicitNamespacePrefix || null;
      this.contentType = contentType;
      this.ignoreFirstLf = ignoreFirstLf;
  }

  requireExtraParent(currentParent) {
      if (!this.requiredParents) {
          return false;
      }
      if (!currentParent) {
          return true;
      }
      var /** @type {?} */ lcParent = currentParent.toLowerCase();
      var /** @type {?} */ isParentTemplate = lcParent === 'template' || currentParent === 'ng-template';
      return !isParentTemplate && this.requiredParents[lcParent] != true;
  }

  isClosedByChild(name) {
      return this.isVoid || name.toLowerCase() in this.closedByChildren;
  }
}

// see http://www.w3.org/TR/html51/syntax.html#optional-tags
// This implementation does not fully conform to the HTML5 spec.
const TAG_DEFINITIONS = {
  'base': new HtmlTagDefinition({ isVoid: true }),
  'meta': new HtmlTagDefinition({ isVoid: true }),
  'area': new HtmlTagDefinition({ isVoid: true }),
  'embed': new HtmlTagDefinition({ isVoid: true }),
  'link': new HtmlTagDefinition({ isVoid: true }),
  'img': new HtmlTagDefinition({ isVoid: true }),
  'input': new HtmlTagDefinition({ isVoid: true }),
  'param': new HtmlTagDefinition({ isVoid: true }),
  'hr': new HtmlTagDefinition({ isVoid: true }),
  'br': new HtmlTagDefinition({ isVoid: true }),
  'source': new HtmlTagDefinition({ isVoid: true }),
  'track': new HtmlTagDefinition({ isVoid: true }),
  'wbr': new HtmlTagDefinition({ isVoid: true }),
  'p': new HtmlTagDefinition({
      closedByChildren: [
          'address', 'article', 'aside', 'blockquote', 'div', 'dl', 'fieldset', 'footer', 'form',
          'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'header', 'hgroup', 'hr',
          'main', 'nav', 'ol', 'p', 'pre', 'section', 'table', 'ul'
      ],
      closedByParent: true
  }),
  'thead': new HtmlTagDefinition({ closedByChildren: ['tbody', 'tfoot'] }),
  'tbody': new HtmlTagDefinition({ closedByChildren: ['tbody', 'tfoot'], closedByParent: true }),
  'tfoot': new HtmlTagDefinition({ closedByChildren: ['tbody'], closedByParent: true }),
  'tr': new HtmlTagDefinition({
      closedByChildren: ['tr'],
      requiredParents: ['tbody', 'tfoot', 'thead'],
      closedByParent: true
  }),
  'td': new HtmlTagDefinition({ closedByChildren: ['td', 'th'], closedByParent: true }),
  'th': new HtmlTagDefinition({ closedByChildren: ['td', 'th'], closedByParent: true }),
  'col': new HtmlTagDefinition({ requiredParents: ['colgroup'], isVoid: true }),
  'svg': new HtmlTagDefinition({ implicitNamespacePrefix: 'svg' }),
  'math': new HtmlTagDefinition({ implicitNamespacePrefix: 'math' }),
  'li': new HtmlTagDefinition({ closedByChildren: ['li'], closedByParent: true }),
  'dt': new HtmlTagDefinition({ closedByChildren: ['dt', 'dd'] }),
  'dd': new HtmlTagDefinition({ closedByChildren: ['dt', 'dd'], closedByParent: true }),
  'rb': new HtmlTagDefinition({ closedByChildren: ['rb', 'rt', 'rtc', 'rp'], closedByParent: true }),
  'rt': new HtmlTagDefinition({ closedByChildren: ['rb', 'rt', 'rtc', 'rp'], closedByParent: true }),
  'rtc': new HtmlTagDefinition({ closedByChildren: ['rb', 'rtc', 'rp'], closedByParent: true }),
  'rp': new HtmlTagDefinition({ closedByChildren: ['rb', 'rt', 'rtc', 'rp'], closedByParent: true }),
  'optgroup': new HtmlTagDefinition({ closedByChildren: ['optgroup'], closedByParent: true }),
  'option': new HtmlTagDefinition({ closedByChildren: ['option', 'optgroup'], closedByParent: true }),
  'pre': new HtmlTagDefinition({ ignoreFirstLf: true }),
  'listing': new HtmlTagDefinition({ ignoreFirstLf: true }),
  'style': new HtmlTagDefinition({ contentType: TagContentType.RAW_TEXT }),
  'script': new HtmlTagDefinition({ contentType: TagContentType.RAW_TEXT }),
  'title': new HtmlTagDefinition({ contentType: TagContentType.ESCAPABLE_RAW_TEXT }),
  'textarea': new HtmlTagDefinition({ contentType: TagContentType.ESCAPABLE_RAW_TEXT, ignoreFirstLf: true }),
}

const _DEFAULT_TAG_DEFINITION = new HtmlTagDefinition(undefined);

function getHtmlTagDefinition(tagName) {
  return TAG_DEFINITIONS[tagName.toLowerCase()] || _DEFAULT_TAG_DEFINITION;
}

/**
 * A binding for an element event (e.g. `(event)="handler()"`) or an animation trigger event (e.g.
 * `(\@trigger.phase)="callback($event)"`).
 */
class BoundEventAst {

  isAnimation: boolean;
  fullName: string;

  constructor(public name, public target, public phase, public handler, public sourceSpan) {
      this.fullName = BoundEventAst.prototype.calcFullName(this.name, this.target, this.phase);
      this.isAnimation = !!this.phase;
  }

  calcFullName(name, target, phase) {
      if (target) {
          return target + ":" + name;
      }
      else if (phase) {
          return "@" + name + "." + phase;
      }
      else {
          return name;
      }
  }

  visit(visitor, context) {
      return visitor.visitEvent(this, context);
  }

}

/**
 * A css selector contains an element name,
 * css classes and attribute/value pairs with the purpose
 * of selecting subsets out of them.
 */
class CssSelector {

  element;
  classNames: any[];
  attrs: any[];
  notSelectors: any[];

  constructor() {
      this.element = null;
      this.classNames = [];
      this.attrs = [];
      this.notSelectors = [];
  }

  static parse(selector) {
    var /** @type {?} */ results = [];
    var /** @type {?} */ _addResult = function (res, cssSel) {
        if (cssSel.notSelectors.length > 0 && !cssSel.element && cssSel.classNames.length == 0 &&
            cssSel.attrs.length == 0) {
            cssSel.element = '*';
        }
        res.push(cssSel);
    };

    var /** @type {?} */ cssSelector = new CssSelector();
    var /** @type {?} */ match;
    var /** @type {?} */ current = cssSelector;
    var /** @type {?} */ inNot = false;

    _SELECTOR_REGEXP.lastIndex = 0;

    while (match = _SELECTOR_REGEXP.exec(selector)) {
        if (match[1]) {
            if (inNot) {
                throw new Error('Nesting :not is not allowed in a selector');
            }
            inNot = true;
            current = new CssSelector();
            cssSelector.notSelectors.push(current);
        }
        if (match[2]) {
            current.setElement(match[2]);
        }
        if (match[3]) {
            current.addClassName(match[3]);
        }
        if (match[4]) {
            current.addAttribute(match[4], match[6]);
        }
        if (match[7]) {
            inNot = false;
            current = cssSelector;
        }
        if (match[8]) {
            if (inNot) {
                throw new Error('Multiple selectors in :not are not supported');
            }
            _addResult(results, cssSelector);
            cssSelector = current = new CssSelector();
        }
    }
    _addResult(results, cssSelector);
    return results;
  }

  isElementSelector() {
    return this.hasElementSelector() && this.classNames.length == 0 && this.attrs.length == 0 &&
        this.notSelectors.length === 0;
  }

  hasElementSelector() {
    return !!this.element;
  };

  setElement(element) {
    if (element === void 0) { element = null; }
    this.element = element;
  }

  getMatchingElementTemplate() {
      var /** @type {?} */ tagName = this.element || 'div';
      var /** @type {?} */ classAttr = this.classNames.length > 0 ? " class=\"" + this.classNames.join(' ') + "\"" : '';
      var /** @type {?} */ attrs = '';
      for (var /** @type {?} */ i = 0; i < this.attrs.length; i += 2) {
          var /** @type {?} */ attrName = this.attrs[i];
          var /** @type {?} */ attrValue = this.attrs[i + 1] !== '' ? "=\"" + this.attrs[i + 1] + "\"" : '';
          attrs += " " + attrName + attrValue;
      }
      return getHtmlTagDefinition(tagName).isVoid ? "<" + tagName + classAttr + attrs + "/>" :
          "<" + tagName + classAttr + attrs + "></" + tagName + ">";
  }

  addAttribute(name, value) {
      if (value === void 0) { value = ''; }
      this.attrs.push(name, value && value.toLowerCase() || '');
  }

  addClassName(name) {
    this.classNames.push(name.toLowerCase());
  }

  toString() {
      var /** @type {?} */ res = this.element || '';
      if (this.classNames) {
          this.classNames.forEach(function (klass) { return res += "." + klass; });
      }
      if (this.attrs) {
          for (var /** @type {?} */ i = 0; i < this.attrs.length; i += 2) {
              var /** @type {?} */ name_1 = this.attrs[i];
              var /** @type {?} */ value = this.attrs[i + 1];
              res += "[" + name_1 + (value ? '=' + value : '') + "]";
          }
      }
      this.notSelectors.forEach(function (notSelector) { return res += ":not(" + notSelector + ")"; });
      return res;
  }

  calcPossibleSecurityContexts(registry, selector, propName, isAttribute) {

    var /** @type {?} */ ctxs = [];

    CssSelector.parse(selector).forEach(function (selector) {
        var /** @type {?} */ elementNames = selector.element ? [selector.element] : registry.allKnownElementNames();
        var /** @type {?} */ notElementNames = new Set(selector.notSelectors.filter(function (selector) { return selector.isElementSelector(); })
            .map(function (selector) { return selector.element; }));
        var /** @type {?} */ possibleElementNames = elementNames.filter(function (elementName) { return !notElementNames.has(elementName); });
        ctxs.push.apply(ctxs, possibleElementNames.map(function (elementName) { return registry.securityContext(elementName, propName, isAttribute); }));
    });

    return ctxs.length === 0 ? [SecurityContext.NONE] : Array.from(new Set(ctxs)).sort();
  }
}

function _isAnimationLabel(name) {
  return name[0] == '@';
}

export class BindingParser {

  _exprParser;
  _interpolationConfig;
  _schemaRegistry;
  _targetErrors : any[];
  pipesByName: Map<string, CompilePipeSummary>;
  _usedPipes: Map<string, CompilePipeSummary>;

  constructor(_exprParser, _interpolationConfig, _schemaRegistry, pipes, _targetErrors) {

      this._exprParser = _exprParser;
      this._interpolationConfig = _interpolationConfig;
      this._schemaRegistry = _schemaRegistry;
      this._targetErrors = _targetErrors;
      this.pipesByName = new Map<string, CompilePipeSummary>();
      this._usedPipes = new Map<string, CompilePipeSummary>();

      pipes.forEach((pipe) => { return this.pipesByName.set(pipe.name, pipe); });
  }

  getUsedPipes() {
     return Array.from(this._usedPipes.values());
  }

  createDirectiveHostPropertyAsts(dirMeta, elementSelector, sourceSpan) {

      if (dirMeta.hostProperties) {
          var /** @type {?} */ boundProps_1 = [];
          Object.keys(dirMeta.hostProperties).forEach(function (propName) {
              var /** @type {?} */ expression = dirMeta.hostProperties[propName];
              if (typeof expression === 'string') {
                  this.parsePropertyBinding(propName, expression, true, sourceSpan, [], boundProps_1);
              }
              else {
                  this._reportError("Value of the host property binding \"" + propName + "\" needs to be a string representing an expression but got \"" + expression + "\" (" + typeof expression + ")", sourceSpan);
              }
          });

          return boundProps_1.map(function (prop) { return this.createElementPropertyAst(elementSelector, prop); });
      }
      return null;
  }

  createDirectiveHostEventAsts(dirMeta, sourceSpan) {

    if (dirMeta.hostListeners) {
        var /** @type {?} */ targetEventAsts_1 = [];

        Object.keys(dirMeta.hostListeners).forEach(function (propName) {
            var /** @type {?} */ expression = dirMeta.hostListeners[propName];
            if (typeof expression === 'string') {
                this.parseEvent(propName, expression, sourceSpan, [], targetEventAsts_1);
            }
            else {
                this._reportError("Value of the host listener \"" + propName + "\" needs to be a string representing an expression but got \"" + expression + "\" (" + typeof expression + ")", sourceSpan);
            }
        });
        return targetEventAsts_1;
    }

    return null;
  }

  parseInterpolation(value, sourceSpan) {
      var /** @type {?} */ sourceInfo = sourceSpan.start.toString();
      try {
          var /** @type {?} */ ast = /** @type {?} */ ((this._exprParser.parseInterpolation(value, sourceInfo, this._interpolationConfig)));
          if (ast)
              this._reportExpressionParserErrors(ast.errors, sourceSpan);
          this._checkPipes(ast, sourceSpan);
          return ast;
      }
      catch (/** @type {?} */ e) {
          this._reportError("" + e, sourceSpan);
          return this._exprParser.wrapLiteralPrimitive('ERROR', sourceInfo);
      }
  }

  parseInlineTemplateBinding(prefixToken, value, sourceSpan, targetMatchableAttrs, targetProps, targetVars) {

    var /** @type {?} */ bindings = this._parseTemplateBindings(prefixToken, value, sourceSpan);

    for (var /** @type {?} */ i = 0; i < bindings.length; i++) {
        var /** @type {?} */ binding = bindings[i];
        if (binding.keyIsVar) {
            targetVars.push(new VariableAst(binding.key, binding.name, sourceSpan));
        }
        else if (binding.expression) {
            this._parsePropertyAst(binding.key, binding.expression, sourceSpan, targetMatchableAttrs, targetProps);
        }
        else {
            targetMatchableAttrs.push([binding.key, '']);
            this.parseLiteralAttr(binding.key, null, sourceSpan, targetMatchableAttrs, targetProps);
        }
    }
  }

  _parseTemplateBindings(prefixToken, value, sourceSpan) {
      let __this = this;
      var /** @type {?} */ sourceInfo = sourceSpan.start.toString();
      try {
          var /** @type {?} */ bindingsResult = this._exprParser.parseTemplateBindings(prefixToken, value, sourceInfo);
          this._reportExpressionParserErrors(bindingsResult.errors, sourceSpan);
          bindingsResult.templateBindings.forEach(function (binding) {
              if (binding.expression) {
                  __this._checkPipes(binding.expression, sourceSpan);
              }
          });
          bindingsResult.warnings.forEach(function (warning) { __this._reportError(warning, sourceSpan, ParseErrorLevel.WARNING); });
          return bindingsResult.templateBindings;
      }
      catch (/** @type {?} */ e) {
          this._reportError("" + e, sourceSpan);
          return [];
      }
  }

  parseLiteralAttr(name, value, sourceSpan, targetMatchableAttrs, targetProps) {
      if (_isAnimationLabel(name)) {
          name = name.substring(1);
          if (value) {
              this._reportError("Assigning animation triggers via @prop=\"exp\" attributes with an expression is invalid." +
                  " Use property bindings (e.g. [@prop]=\"exp\") or use an attribute without a value (e.g. @prop) instead.", sourceSpan, ParseErrorLevel.ERROR);
          }
          this._parseAnimation(name, value, sourceSpan, targetMatchableAttrs, targetProps);
      }
      else {
          targetProps.push(new BoundProperty(name, this._exprParser.wrapLiteralPrimitive(value, ''), BoundPropertyType.LITERAL_ATTR, sourceSpan));
      }
  }

  parsePropertyBinding(name, expression, isHost, sourceSpan, targetMatchableAttrs, targetProps) {

      var /** @type {?} */ isAnimationProp = false;

      if (name.startsWith(ANIMATE_PROP_PREFIX)) {
          isAnimationProp = true;
          name = name.substring(ANIMATE_PROP_PREFIX.length);
      }
      else if (_isAnimationLabel(name)) {
          isAnimationProp = true;
          name = name.substring(1);
      }
      if (isAnimationProp) {
          this._parseAnimation(name, expression, sourceSpan, targetMatchableAttrs, targetProps);
      }
      else {
          this._parsePropertyAst(name, this._parseBinding(expression, isHost, sourceSpan), sourceSpan, targetMatchableAttrs, targetProps);
      }
  }

  parsePropertyInterpolation(name, value, sourceSpan, targetMatchableAttrs, targetProps) {

      var /** @type {?} */ expr = this.parseInterpolation(value, sourceSpan);

      if (expr) {
          this._parsePropertyAst(name, expr, sourceSpan, targetMatchableAttrs, targetProps);
          return true;
      }
      return false;
  }

  _parsePropertyAst(name, ast, sourceSpan, targetMatchableAttrs, targetProps) {
      targetMatchableAttrs.push([name, /** @type {?} */ ((ast.source))]);
      targetProps.push(new BoundProperty(name, ast, BoundPropertyType.DEFAULT, sourceSpan));
  }

  _parseAnimation(name, expression, sourceSpan, targetMatchableAttrs, targetProps) {

      // This will occur when a @trigger is not paired with an expression.
      // For animations it is valid to not have an expression since */void
      // states will be applied by angular when the element is attached/detached

      var /** @type {?} */ ast = this._parseBinding(expression || 'undefined', false, sourceSpan);
      targetMatchableAttrs.push([name, /** @type {?} */ ((ast.source))]);
      targetProps.push(new BoundProperty(name, ast, BoundPropertyType.ANIMATION, sourceSpan));
  }

  _parseBinding(value, isHostBinding, sourceSpan) {

      var /** @type {?} */ sourceInfo = sourceSpan.start.toString();

      try {
          var /** @type {?} */ ast = isHostBinding ?
              this._exprParser.parseSimpleBinding(value, sourceInfo, this._interpolationConfig) :
              this._exprParser.parseBinding(value, sourceInfo, this._interpolationConfig);
          if (ast)
              this._reportExpressionParserErrors(ast.errors, sourceSpan);
          this._checkPipes(ast, sourceSpan);
          return ast;
      }
      catch (/** @type {?} */ e) {
          this._reportError("" + e, sourceSpan);
          return this._exprParser.wrapLiteralPrimitive('ERROR', sourceInfo);
      }
  };

  createElementPropertyAst(elementSelector, boundProp) {

      if (boundProp.isAnimation) {
          return new BoundElementPropertyAst(boundProp.name, PropertyBindingType.Animation, SecurityContext.NONE, boundProp.expression, null, boundProp.sourceSpan);
      }

      var /** @type {?} */ unit = null;
      var /** @type {?} */ bindingType = /** @type {?} */ ((undefined));
      var /** @type {?} */ boundPropertyName = null;
      var /** @type {?} */ parts = boundProp.name.split(PROPERTY_PARTS_SEPARATOR);
      var /** @type {?} */ securityContexts = /** @type {?} */ ((undefined));

      // Check check for special cases (prefix style, attr, class)

      if (parts.length > 1) {
          if (parts[0] == ATTRIBUTE_PREFIX) {
              boundPropertyName = parts[1];
              this._validatePropertyOrAttributeName(boundPropertyName, boundProp.sourceSpan, true);
              securityContexts = calcPossibleSecurityContexts(this._schemaRegistry, elementSelector, boundPropertyName, true);
              var /** @type {?} */ nsSeparatorIdx = boundPropertyName.indexOf(':');
              if (nsSeparatorIdx > -1) {
                  var /** @type {?} */ ns = boundPropertyName.substring(0, nsSeparatorIdx);
                  var /** @type {?} */ name_1 = boundPropertyName.substring(nsSeparatorIdx + 1);
                  boundPropertyName = mergeNsAndName(ns, name_1);
              }
              bindingType = PropertyBindingType.Attribute;
          }
          else if (parts[0] == CLASS_PREFIX) {
              boundPropertyName = parts[1];
              bindingType = PropertyBindingType.Class;
              securityContexts = [SecurityContext.NONE];
          }
          else if (parts[0] == STYLE_PREFIX) {
              unit = parts.length > 2 ? parts[2] : null;
              boundPropertyName = parts[1];
              bindingType = PropertyBindingType.Style;
              securityContexts = [SecurityContext.STYLE];
          }
      }

      // If not a special case, use the full property name

      if (boundPropertyName === null) {
          boundPropertyName = this._schemaRegistry.getMappedPropName(boundProp.name);
          securityContexts = calcPossibleSecurityContexts(this._schemaRegistry, elementSelector, boundPropertyName, false);
          bindingType = PropertyBindingType.Property;
          this._validatePropertyOrAttributeName(boundPropertyName, boundProp.sourceSpan, false);
      }
      return new BoundElementPropertyAst(boundPropertyName, bindingType, securityContexts[0], boundProp.expression, unit, boundProp.sourceSpan);
  }

  parseEvent(name, expression, sourceSpan, targetMatchableAttrs, targetEvents) {

      if (_isAnimationLabel(name)) {
          name = name.substr(1);
          this._parseAnimationEvent(name, expression, sourceSpan, targetEvents);
      }
      else {
          this._parseEvent(name, expression, sourceSpan, targetMatchableAttrs, targetEvents);
      }
  }

  _parseAnimationEvent(name, expression, sourceSpan, targetEvents) {

      var /** @type {?} */ matches = splitAtPeriod(name, [name, '']);
      var /** @type {?} */ eventName = matches[0];
      var /** @type {?} */ phase = matches[1].toLowerCase();

      if (phase) {
          switch (phase) {
              case 'start':
              case 'done':
                  var /** @type {?} */ ast = this._parseAction(expression, sourceSpan);
                  targetEvents.push(new BoundEventAst(eventName, null, phase, ast, sourceSpan));
                  break;
              default:
                  this._reportError("The provided animation output phase value \"" + phase + "\" for \"@" + eventName + "\" is not supported (use start or done)", sourceSpan);
                  break;
          }
      }
      else {
          this._reportError("The animation trigger output event (@" + eventName + ") is missing its phase value name (start or done are currently supported)", sourceSpan);
      }
  }

  _parseEvent(name, expression, sourceSpan, targetMatchableAttrs, targetEvents) {

      // long format: 'target: eventName'
      var _a = splitAtColon(name, [/** @type {?} */ ((null)), name]), target = _a[0], eventName = _a[1];
      var /** @type {?} */ ast = this._parseAction(expression, sourceSpan);

      targetMatchableAttrs.push([/** @type {?} */ ((name)), /** @type {?} */ ((ast.source))]);
      targetEvents.push(new BoundEventAst(eventName, target, null, ast, sourceSpan));
      // Don't detect directives for event names for now,
      // so don't add the event name to the matchableAttrs
  }

  _parseAction(value, sourceSpan) {

      var /** @type {?} */ sourceInfo = sourceSpan.start.toString();

      try {
          var /** @type {?} */ ast = this._exprParser.parseAction(value, sourceInfo, this._interpolationConfig);

          if (ast) {
              this._reportExpressionParserErrors(ast.errors, sourceSpan);
          }

          if (!ast || ast.ast instanceof EmptyExpr) {
              this._reportError("Empty expressions are not allowed", sourceSpan);
              return this._exprParser.wrapLiteralPrimitive('ERROR', sourceInfo);
          }
          this._checkPipes(ast, sourceSpan);
          return ast;
      }
      catch (/** @type {?} */ e) {
          this._reportError("" + e, sourceSpan);
          return this._exprParser.wrapLiteralPrimitive('ERROR', sourceInfo);
      }
  }

  _reportError(message, sourceSpan, level = undefined) {
      if (level === void 0) { level = ParseErrorLevel.ERROR; }
      this._targetErrors.push(new ParseError(sourceSpan, message, level));
  }

  _reportExpressionParserErrors(errors, sourceSpan) {
      for (var _i = 0, errors_1 = errors; _i < errors_1.length; _i++) {
          var error = errors_1[_i];
          this._reportError(error.message, sourceSpan);
      }
  }

  _checkPipes(ast, sourceSpan) {
      let __this = this;
      if (ast) {
          var /** @type {?} */ collector = new PipeCollector();
          ast.visit(collector);
          collector.pipes.forEach(function (ast, pipeName) {
              var /** @type {?} */ pipeMeta = __this.pipesByName.get(pipeName);
              if (!pipeMeta) {
                  __this._reportError("The pipe '" + pipeName + "' could not be found", new ParseSourceSpan(sourceSpan.start.moveBy(ast.span.start), sourceSpan.start.moveBy(ast.span.end)));
              }
              else {
                  __this._usedPipes.set(pipeName, pipeMeta);
              }
          });
      }
  }

  _validatePropertyOrAttributeName(propName, sourceSpan, isAttr) {
      var /** @type {?} */ report = isAttr ? this._schemaRegistry.validateAttribute(propName) :
          this._schemaRegistry.validateProperty(propName);
      if (report.error) {
          this._reportError(/** @type {?} */ ((report.msg)), sourceSpan, ParseErrorLevel.ERROR);
      }
  }
}
