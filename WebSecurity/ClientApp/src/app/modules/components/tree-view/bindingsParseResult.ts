import { BindingTransform } from './';

export class BindingsParseResult {

  bindingTransforms: BindingTransform[];
  propertyName: string;

  constructor() {
    this.bindingTransforms = [];
  }
}
