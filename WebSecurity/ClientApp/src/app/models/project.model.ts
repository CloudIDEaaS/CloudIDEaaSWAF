import { ProjectState } from './projectstate';

export class Project {
    public projectId: string;
    public applicationName: string;
    public projectName: string;
    public lastSaveTime: Date;
    public projectState: number;
    public saveAsRevision: boolean;
    public saveEntitiesOnly: boolean;
    public rootModel: any;
    public entities: any;
    public entityConfig: string;

    constructor(options: { projectName: string, applicationName: string})
    constructor(c: any)
    {
      if (c.name !== undefined && c.model !== undefined) {
        this.projectId = c.id;
        this.applicationName = this.applicationName;
        this.projectName = c.projectName;
        this.lastSaveTime = c.lastSaveTime;
        this.projectState = c.state;
        this.rootModel = c.rootModel;
      }
      else if (c !== undefined) {
        (<any>Object).assign(this, c);
      } else {
        throw new TypeError("Unxpected arguments to Project constructor");
      }
    }

    public get projectData() {
      return { projectId: this.projectId, projectName: this.projectName, rootModel: this.rootModel };
    }
  }
