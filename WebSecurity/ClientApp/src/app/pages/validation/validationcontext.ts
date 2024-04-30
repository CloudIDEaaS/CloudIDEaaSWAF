import { IValidationContext, ValidationContextState, RuleResult, RulePolicy } from "@angularlicious/rules-engine";

export class ValidationContext implements IValidationContext {
    state: ValidationContextState = ValidationContextState.NotEvaluated;
    results: Array<RuleResult> = new Array<RuleResult>();
    rules: Array<RulePolicy> = new Array<RulePolicy>();
    source: string;
 
    /**
    * Use this method to add a new rule to the ValidationContext. 
    */
    addRule(rule: RulePolicy) {
        if (this.source) {
            rule.source = this.source;
        }
        this.rules.push(rule);
        return this;
    }
  
    /**
    * Use this method to execute the rules added to the [ValidationContext].
    */
    renderRules(): IValidationContext {
        this.results = new Array<RuleResult>();
        if (this.rules && this.rules.length < 1) {
            return this;
        }
        this.rules.sort(r => r.priority).forEach(r => this.results.push(r.execute()));
        return this;
    }
 
    /**
    * Use to determin if the validation context has any rule violations.
    */
    hasRuleViolations(): boolean {
        var hasViolations = false;
        if (this.rules && this.rules.filter(r => r.isValid === false)) {
            hasViolations = true;
        }
        return hasViolations;
    }
 
    /**
        * *Use to indicate if the validation context is valid - no rule violations.
        * @returns {}: returns a boolean.
        */
    get isValid(): boolean {
        var isRuleValid: boolean = true;
        if (this.rules) {
            var invalidRulesCount = this.rules.filter(r => r.isValid === false).length;
            if (invalidRulesCount > 0) {
                isRuleValid = false;
            }
        }
        return isRuleValid;
    }
}