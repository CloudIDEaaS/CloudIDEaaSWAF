import * as $ from "jquery";
const jQuery = $;

export function PropertiesNode(name, data) {
    this.name = name;
    this.data = data;
    this.childProperties = [];

    this.lookupSelected = (name) => {

        let selected = false;

        $.each(this.childProperties, (i) => {

            let property = this.childProperties[i];

            if (property.name == name) {

                if (property.data) {
                    selected = property.data.selected;
                }
            }
        });

        return selected;
    }
}
