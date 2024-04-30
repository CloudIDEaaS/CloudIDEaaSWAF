import { RulePolicy, RuleResult } from '@angularlicious/rules-engine';
 
export class DeleteNodeRule extends RulePolicy {
    hasErrors: boolean = false;
    results: Array<RuleResult> = new Array<RuleResult>();
    rules: Array<RulePolicy> = new Array<RulePolicy>();
 
    constructor(name: string, message: string, isDisplayable: boolean) {
        super(name, message, isDisplayable);
    }
 
    render(): RuleResult {
        this.rules.sort(s => s.priority).forEach(r => this.results.push(r.execute()));
        return this.processResults();
    }
 
    public hasRules(): boolean {
        if (this.rules && this.rules.length > 0) {
            return true;
        }
        return false;
    }
 
    processResults(): RuleResult {
        if (this.results.filter(r => (r.isValid === false)).length > 0) {
            this.isValid = false;
            this.hasErrors = true;
        }
        return new RuleResult(this);
    }
}