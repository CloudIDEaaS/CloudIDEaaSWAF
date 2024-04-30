import { Validators, ValidatorFn } from "@angular/forms";

export interface ValidatorEntry
{
  function : ValidatorFn,
  message : string
}

export class Validator
{
  constructor(public name: string, public entryMap : Map<string, ValidatorEntry>)  {
  }

  get functions() : ValidatorFn[]
  {
    var array : ValidatorFn[] = [];

    this.entryMap.forEach(entry =>{
      array.push(entry.function);
    });

    return array;
  }
}

export class ValidationMap {
  map: Map<string, Validator>;

  constructor(config) {
    this.process(config);
  }

  get(key : string) : Validator
  {
    return this.map.get(key);
  }

  process(config : any[]) {

    this.map = new Map<string, Validator>();

    config.forEach((controls : any[]) => {

      Object.keys(controls).forEach((property : string) => {

        let validators = controls[property];
        let entryMap = new Map<string, ValidatorEntry>();
        let validator: Validator;

        validators.forEach((validatorInfo) => {

          Object.keys(validatorInfo).forEach((property2 : string) => {

            let validatorEntry : ValidatorEntry = validatorInfo[property2];
            let validatorFunction = validatorEntry.function;
            let message = validatorEntry.message;

            entryMap.set(property2, validatorEntry);
 
          });

        });

        validator = new Validator(property, entryMap);
        this.map.set(property, validator);

      });
  
    });
  }
}
