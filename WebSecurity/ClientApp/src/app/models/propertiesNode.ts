export class PropertiesNode {
    name = "";
    data = {};
    childProperties = [];

    constructor(name, data) {
        this.name = name;
        this.data = data;
    }
}
