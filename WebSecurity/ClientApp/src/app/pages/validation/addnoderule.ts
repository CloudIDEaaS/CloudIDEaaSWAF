import { RulePolicy, RuleResult, IsNotNullOrUndefined } from '@angularlicious/rules-engine';
import { BusinessModelUIState } from '../../models/businessmodeluistate';
import {ValidationContextState, AreEqual, IsTrue, StringIsNotNullEmptyRange} from '@angularlicious/rules-engine';
import { ChildLevelRule } from './childlevelrule';
 
export class AddNodeRule extends RulePolicy {
    hasErrors: boolean = false;
    results: Array<RuleResult> = new Array<RuleResult>();
    rules: Array<RulePolicy> = new Array<RulePolicy>();
 
    constructor(private businessModelUIState: BusinessModelUIState) {
        super("AddNodeIsEnabled", "Unable to add node at this time", true);

        this.configureRules();
    }

    configureRules() {
        this.rules.push(new IsNotNullOrUndefined("SelectedNodeTextIsNotNull", "Selected node name cannot be null", this.businessModelUIState.selectedNodeText, true));
        this.rules.push(new IsNotNullOrUndefined("NodeNameTextIsNotNull", "Node name cannot be null", this.businessModelUIState.nodeNameText, true));
        this.rules.push(new IsNotNullOrUndefined("NodeTypeIsNotNull", "Node type must by child or sibling", this.businessModelUIState.selectedNodeType, true));
        this.rules.push(new ChildLevelRule(this.businessModelUIState, this.businessModelUIState.selectedLevelType));
        this.rules.push(new ChildLevelRule(this.businessModelUIState, this.businessModelUIState.selectedLevelChip));
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