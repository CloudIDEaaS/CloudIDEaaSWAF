export function RecordExpression(expression : string) {
    return (target: Object) => {
        target["__recordExpression__"] = expression;
    };    
}

export function PageName(name : string) {
    return (target: Object) => {
        target["__pageName__"] = name;
    };    
}