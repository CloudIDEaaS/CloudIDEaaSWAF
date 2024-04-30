import { RulePolicy, RuleResult } from '@angularlicious/rules-engine';
import { BusinessModelUIState } from '../../models/businessmodeluistate';
import {ValidationContextState, AreEqual, IsTrue, StringIsNotNullEmptyRange} from '@angularlicious/rules-engine';
import { BusinessModelLevel, getLevel, getLevelEnumFieldName, getValidation } from '../../businessModelLevel';

export class ChildLevelRule extends RulePolicy {
    hasErrors: boolean = false;
    results: Array<RuleResult> = new Array<RuleResult>();
    rules: Array<RulePolicy> = new Array<RulePolicy>();
    levelName: string;
    level: BusinessModelLevel;
    parentLevel: BusinessModelLevel;

    constructor(private businessModelUIState: BusinessModelUIState, levelName: string) {
        super("ChildLevelIsValid", "Level is not valid for new child of this node", true);

        if (businessModelUIState.selectedNode) {

            let parentLevelName = businessModelUIState.selectedNode.title;

            this.parentLevel = getLevel(parentLevelName);
        }

        this.levelName = levelName;

        if (this.levelName) {
            this.level = getLevel(this.levelName);
        }

        this.configureRules();
    }

    configureRules() {
        this.rules.push(new IsTrue("IsValidLevel", `Business model level '${ this.levelName }' is not a valid level name.`, this.isValidLevelName(this.levelName), true));
        this.rules.push(new IsTrue("IsValidLevel", `Business model level '${ this.levelName }' is not a valid level for a child of node type '${ this.businessModelUIState.selectedLevelType }'.`, this.isValidChildLevel(this.parentLevel, this.level), true));
    }

    isValidChildLevel(parentLevel: BusinessModelLevel, childLevel: BusinessModelLevel): boolean {

        if (childLevel && parentLevel) {

            let validation = getValidation();
            let name = getLevelEnumFieldName(parentLevel);
            let validChildren = <Array<BusinessModelLevel>> validation[name].validChildren;

            return validChildren.includes(childLevel);
        }
        else {
            return false;
        }
    }

    isValidLevelName(levelName: string): boolean {
        let level = getLevel(levelName);
        return level !== undefined;
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
