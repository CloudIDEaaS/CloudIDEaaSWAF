import { AsyncPipe, DatePipe } from '@angular/common';
import { PipeTransform } from '@angular/core';
// tslint:disable-next-line:max-line-length
import { PropertyRead, ASTWithSource, Parser, Lexer, DomElementSchemaRegistry, ParseError, ParseSourceSpan, ParseLocation, ParseSourceFile, BindingPipe, LiteralPrimitive } from '@angular/compiler';
import { PipeCollector, BindingParser, BoundProperty } from '../../utils/BindingParser';
import { DebugUtils } from '../../utils/DebugUtils';
import { BindingsParseResult, BindingTransform, TreeView, ExpandPipe } from './';
import { List } from 'linq-collections';
import { CompilePipeSummary, CompileSummaryKind } from '../../utils/AngularCompilerCompat';

export class BindingsParser {

  private registry: DomElementSchemaRegistry;
  private lexer: Lexer;
  private parser : Parser;
  private pipes: CompilePipeSummary[]

  constructor(private treeView: TreeView) {

    this.registry = new DomElementSchemaRegistry();
    this.lexer = new Lexer();
    this.parser = new Parser(this.lexer);
    this.pipes = [
      {
        name: 'async',
        pure: false,
        summaryKind: CompileSummaryKind.Pipe,
        type: { reference: AsyncPipe, diDeps: [], lifecycleHooks: [] },
      },
      {
        name: 'onexpand',
        pure: false,
        summaryKind: CompileSummaryKind.Pipe,
        type: { reference: ExpandPipe, diDeps: [], lifecycleHooks: [] },
      },
      {
        name: 'date',
        pure: false,
        summaryKind: CompileSummaryKind.Pipe,
        type: { reference: DatePipe, diDeps: [], lifecycleHooks: [] },
      },
     ];
  }

  parse(member: string, value: string) : BindingsParseResult {

    const errors: ParseError[] = [];
    const bindingParser = new BindingParser(this.parser, undefined, this.registry, this.pipes, errors);
    const source = member + '=' + value;
    const sourceFile = new ParseSourceFile(source, 'parse://BindingsParser');
    const targetProps : any[] = [];
    const targetMatchableAttributes : any[] = [];
    const span = new ParseSourceSpan(
      new ParseLocation(sourceFile, 0, 0, 0),
      new ParseLocation(sourceFile, source.length, 0, source.length));
    let propertyName: string;
    const bindingsParseResult = new BindingsParseResult();

    bindingParser.parsePropertyBinding(member, value, false, span, targetMatchableAttributes, targetProps);

    targetProps.forEach(p => {

      if (p instanceof BoundProperty) {

        const boundProperty = p as BoundProperty;
        const pipeCollector = new PipeCollector();
        const ast = boundProperty.expression as ASTWithSource;
        const list = new List<BindingPipe>();

        const captureProperty = (p: PropertyRead) => {
          bindingsParseResult.propertyName = p.name;
        };

        ast.visit(pipeCollector);

        if (pipeCollector.pipes.size === 0) {

          const property = p.expression.ast as PropertyRead;

          captureProperty(property);
        } else {

            pipeCollector.pipes.forEach((p: BindingPipe) => {

              const capturePipe = (p: BindingPipe) => {

                if (!list.contains(p)) {

                  const pipeName = p.name;
                  const pipeSummary = bindingParser.pipesByName.get(pipeName);
                  const pipeType = pipeSummary.type.reference;
                  const bindingTransform = new BindingTransform();
                  let pipeTransform: PipeTransform;

                  if (pipeType.name === 'AsyncPipe' || pipeType.name === 'ExpandPipe') {
                    pipeTransform = (new pipeType({ markForCheck: () => bindingTransform.markForCheck() }) as PipeTransform);
                  } else {
                    pipeTransform = (new pipeType() as PipeTransform);
                  }

                  bindingTransform.pipeName = pipeName;
                  bindingTransform.pipeTransform = pipeTransform;
                  bindingsParseResult.bindingTransforms.push(bindingTransform);

                  p.args.forEach(a => {

                    if (a instanceof LiteralPrimitive) {

                      const primitive = a as LiteralPrimitive;

                      bindingTransform.args.push(primitive.value);
                    } else {
                      DebugUtils.break();
                    }
                  })

                  if (p.exp instanceof PropertyRead) {

                    const property = p.exp as PropertyRead;

                    captureProperty(property);
                  } else if (p.exp instanceof BindingPipe) {

                    const pipe = p.exp as BindingPipe;

                    capturePipe(pipe);
                  } else {
                    DebugUtils.break();
                  }

                  list.push(p);
                }
              };

              capturePipe(p);
            });
          }
          } else {
        DebugUtils.break();
      }
    });

    return bindingsParseResult;
  }
}
