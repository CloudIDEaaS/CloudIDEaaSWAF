export enum BusinessModelLevel {
  businessModel,
  stakeholder,
  department,
  role,
  system,
  responsibility_feature,
  task,
  dataitem
}

export function getValidation() {

  let validation: any = {
    names: {
      businessModel: "Business Model",
      stakeholder: "Stakeholder",
      department: "Department",
      role: "Role",
      system: "System",
      responsibility_feature: "Responsibility / Feature",
      task: "Task",
      dataitem: "Data Item"
    },
    businessModel: {
      validChildren: [
        BusinessModelLevel.stakeholder,
        BusinessModelLevel.role,
        BusinessModelLevel.system
      ]
    },
    stakeholder: {
      validChildren: [
        BusinessModelLevel.stakeholder,
        BusinessModelLevel.department,
        BusinessModelLevel.role,
        BusinessModelLevel.system,
        BusinessModelLevel.responsibility_feature,
        BusinessModelLevel.task
      ]
    },
    department: {
      validChildren: [
        BusinessModelLevel.department,
        BusinessModelLevel.role,
        BusinessModelLevel.system,
        BusinessModelLevel.responsibility_feature,
        BusinessModelLevel.task
      ]
    },
    role: {
      validChildren: [
        BusinessModelLevel.system,
        BusinessModelLevel.responsibility_feature,
        BusinessModelLevel.task
      ]
    },
    system: {
      validChildren: [
        BusinessModelLevel.system,
        BusinessModelLevel.responsibility_feature,
        BusinessModelLevel.task
      ]
    },
    responsibility_feature: {
      validChildren: [
        BusinessModelLevel.responsibility_feature,
        BusinessModelLevel.task,
        BusinessModelLevel.dataitem
      ]
    },
    task: {
      validChildren: [BusinessModelLevel.task, BusinessModelLevel.dataitem]
    },
    dataitem: {
      validChildren: [BusinessModelLevel.task]
    }
  };

  return validation;
}

export function getLevelEnumFieldName(level: BusinessModelLevel): string {
  
  let name: string;

  for (var enumMember in BusinessModelLevel) {
    var enumMemberValue = parseInt(enumMember, 10);

    if (enumMemberValue === level) {
      name = BusinessModelLevel[enumMember];
    }
  }

  return name;
}

export function getLevel(levelName: string): BusinessModelLevel {
  
    let level: BusinessModelLevel;
    let validation = getValidation();

    if (levelName) {

        Object.keys(validation.names).forEach(p => {
        
            let value = validation.names[p].toLowerCase();
    
            if (value === levelName.toLowerCase() || removeSpaces(value) === levelName.toLowerCase()  || p === levelName.toLowerCase()) {
                level = BusinessModelLevel[p];
            }
        });
    };

    return level;
}

export function removeSpaces(str : string) {
    return str.replace(new RegExp(" ", 'g'), "");
  }

