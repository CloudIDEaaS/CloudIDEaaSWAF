import * as $ from "jquery";
require("jquery-ui");

const jQuery = $;

jQuery.fn.tagName = function () {
    return this.prop("tagName").toLowerCase();
};

jQuery.fn.getChildData = function (dataAttribute) {

    let foundData;

    this.find("*").each((i, e) => {

        let data = $(e).data(dataAttribute);

        if (data) {
            foundData = data;
        }
    });

    return foundData;
}

jQuery.fn.appendElement = function (tagName, content) {

    let element;

    if (typeof content == "string") {
        element = $(`<${tagName}>${content}</${tagName}>`);
    }
    else {

        element = $(`<${tagName}></${tagName}>`);

        if (typeof content == "function") {
            content(element);
        }
        else if (typeof content == "object") {

            let innerContent = content["innerHtml"] || content["innerText"] || "";
            let data = content["data"];

            element = $(`<${tagName}>${innerContent}</${tagName}>`);
            element.addProperties(content);

            if (data) {

                let key = Object.keys(data)[0];
                let elementData = data[key];

                element.data(key, elementData);
            }
        }
    }

    this.append(element);

    return element;
};

jQuery.fn.createElement = function (tagName, content) {

    let element;

    if (typeof content == "string") {
        element = $(`<${tagName}>${content}</${tagName}>`);
    }
    else {

        element = $(`<${tagName}></${tagName}>`);

        if (typeof content == "function") {
            content(element);
        }
        else if (typeof content == "object") {

            let innerContent = content["innerHtml"] || content["innerText"] || "";
            let data = content["data"];

            element = $(`<${tagName}>${innerContent}</${tagName}>`);
            element.addProperties(content);

            if (data) {

                let key = Object.keys(data)[0];
                let elementData = data[key];

                element.data(key, elementData);
            }
        }
    }

    return element;
}

jQuery.fn.createDiv = function (content) {
    return this.createElement("div", content);
}

jQuery.fn.createIFrame = function (content) {
    return this.createElement("iframe", content);
}

jQuery.fn.appendLink = function (content) {
    return this.appendElement("a", content);
}

jQuery.fn.appendParagraph = function (content) {
    return this.appendElement("p", content);
}

jQuery.fn.appendLabel = function (content) {
    return this.appendElement("label", content);
};

jQuery.fn.appendOption = function (key, value, properties) {

    properties = Object.assign({
        value: key,
        innerText: value,
    }, properties);

    return this.appendElement("option", properties);
}

jQuery.fn.appendSelect = function (items, keyName, valueName, properties) {

    let select;
    let selectedIndex = properties ? properties.selectedIndex : null;
    let x = 0;

    if (!valueName && !properties) {
        properties = keyName;
    }

    select = this.appendElement("select", properties);

    if (selectedIndex && selectedIndex == -1) {
        select.appendOption("", "", { selected: "selected" });
    }

    $.each(items, (key, value) => {

        if (keyName && (typeof value == "object")) {
            key = value[keyName];
        }

        if (valueName && (typeof value == "object")) {
            value = value[valueName];
        }

        if (selectedIndex && selectedIndex == x) {
            select.appendOption(key, value, { selected: "selected" });
        }
        else {
            select.appendOption(key, value);
        }

        x++;
    });

    return select;
}

jQuery.fn.selectText = function () {

    let range
    let selection;
    let elem = this[0];

    if (document.body.createTextRange) {

        range = document.body.createTextRange();
        range.moveToElementText(elem);
        range.select();
    }
    else if (window.getSelection) {

        selection = window.getSelection();

        range = document.createRange();
        range.selectNodeContents(elem);

        selection.removeAllRanges();
        selection.addRange(range);
    }
}

jQuery.fn.appendTextBox = function (content) {

    let textBox;

    if (typeof content == "string") {
        textBox = $(`<textarea type='text' value='${content}'></textarea>`);
    }
    else {

        if (typeof content == "object") {

            let innerContent = content["innerHtml"] || content["innerText"] || "";

            textBox = $(`<input type='text'>${innerContent}</input>`);
            textBox.addProperties(content);
        }
        else {

            textBox = $("<input type='text'></input>");

            if (typeof content == "function") {
                content(textBox);
            }
        }
    }

    this.append(textBox);

    return textBox;
};

jQuery.fn.appendTextArea = function (content) {

    let textArea;

    if (typeof content == "string") {
        textArea = $(`<textarea>'${content}'></textarea>`);
    }
    else {

        if (typeof content == "object") {

            let innerContent = content["innerHtml"] || content["innerText"] || "";

            textArea = $(`<textarea>${innerContent}</textarea>`);
            textArea.addProperties(content);
        }
        else {

            textArea = $("<textarea></textarea>");

            if (typeof content == "function") {
                content(textArea);
            }
        }
    }

    this.append(textArea);

    return textArea;
};

jQuery.fn.createUniqueId = function () {

    let id = (new Date().getTime()) * 1000 + Math.floor(Math.random() * 1001);

    return id;
}

jQuery.fn.appendCheckbox = function (text, checked) {

    let id = this.createUniqueId();
    let checkBox;
    let label;

    if (checked) {
        checkBox = $(`<input id='${id}' checked class='checkbox' type='checkbox'></input>`);
    }
    else {
        checkBox = $(`<input id='${id}' class='checkbox' type='checkbox'></input>`);
    }

    label = $(`<label class='checkboxLabel' for='${id}'>${text}</label>`);

    this.append(checkBox);
    this.append(label);

    label.mousedown(() => {

        label.css({
            "background": "#318efb",
            "color": "white"
        });

        console.debug("checkBox.focus");
        checkBox.focus();
    });

    checkBox.focusin(() => {

        label.css({
            "background": "#318efb",
            "color": "white"
        });
    });

    checkBox.focusout(() => {

        label.css({
            "background": "white",
            "color": "black"
        });
    });

    return checkBox;
};

jQuery.fn.appendListItem = function (content) {

    let listItem;

    if (typeof content == "string") {
        listItem = $(`<li>${content}</li>`);
    }
    else {

        if (typeof content == "object") {

            let innerContent = content["innerHtml"] || content["innerText"] || "";

            listItem = $(`<li>${innerContent}</li>`);
            listItem.addProperties(content);
        }
        else {
            listItem = $("<li></li>");

            if (typeof content == "function") {
                content(div);
            }
            else if (Array.isArray(content)) {
                listItem.appendFromArrayContent(content, "li");
            }
        }
    }

    this.append(listItem);

    return listItem;
}

jQuery.fn.addProperties = function (obj) {

    let keys = Object.keys(obj);

    keys.forEach((k) => {

        if (k === "style") {
            this.css(obj[k]);
        }
        else if (k != "innerHtml" && k !== "innerText") {
            this.prop(k, obj[k]);
        }
    });
}

jQuery.fn.appendFromArrayContent = function (array, childTag) {

    array.forEach((i) => {

        let itemElement = $(`<${childTag}>${i}</${childTag}>`);

        list.append(itemElement);
    });
}

jQuery.fn.appendListUnordered = function (content) {

    let list;

    if (typeof content == "string") {
        list = $(`<ul>${content}</ul>`);
    }
    else {

        list = $("<ul></ul>");

        if (typeof content == "function") {
            content(div);
        }
        else if (typeof content == "object") {
            list.addProperties(content);
        }
        else if (Array.isArray(content)) {
            list.appendFromArrayContent(content, "li");
        }
    }

    this.append(list);

    return list;
}

jQuery.fn.innerMost = function () {

    let inner = $.merge(this, this.find("*")).last();

    return inner;
};

jQuery.fn.then = function () {

    let then = this.parent();

    return then;
};

jQuery.fn.above = function () {

    let above = this.parent();

    return above;
};

jQuery.fn.appendDiv = function (content) {

    let div;

    if (typeof content == "string") {
        div = $(`<div>${content}</div>`);
    }
    else {

        div = $("<div></div>");

        if (typeof content == "function") {
            content(div);
        }
        else if (typeof content == "object") {
            div.addProperties(content);
        }
    }

    this.append(div);

    return div;
};

jQuery.fn.appendTableRow = function () {

    let row = $("<tr>");
    let tbody = this.find("tbody");

    if (tbody.length) {
        tbody.append(row);
    }
    else {
        this.append(row);
    }

    return row;
}

jQuery.fn.insertTableRowAt = function (index) {

    let insertBefore = this.find("tbody").children().eq(index);
    let row = $("<tr>");
    let tbody = this.find("tbody");

    if (tbody.length) {
        insertBefore.before(row);
    }
    else {
        insertBefore.before(row);
    }

    return row;
}

jQuery.fn.appendTableCell = function (content) {
    return this.appendElement("td", content);
}

jQuery.fn.appendTableHeader = function (content) {

    let row = $("<th>");

    row.append(content);
    this.append(row);

    return row;
}

jQuery.fn.appendTable = function (template) {

    let table = $("<table>");
    let headerRow = null;
    let data = template.data;

    template.columns.forEach((c) => {

        if (c.headerTitle) {

            if (!headerRow) {
                headerRow = table.appendTableRow();
            }

            headerRow.appendTableHeader(c.headerTitle);
        }
    });

    data.forEach((d) => {

        let row = table.appendTableRow();
        let index = 0;
        let keys = Object.keys(d);
        let data = d;

        template.columns.forEach((c) => {

            if (c.itemTemplate) {

                let itemTemplate = c.itemTemplate;

                if (typeof itemTemplate === "string") {
                    row.appendTableCell(data[itemTemplate]);
                }
                else if (typeof itemTemplate === "function") {

                    let td = row.appendTableCell();
                    itemTemplate(td, data);
                }
            }
            else {
                row.appendTableCell(data[keys[index]]);
            }

            index++;
        });
    });

    this.append(table);

    return table;
}

jQuery.fn.createDropPanel = function (content) {

    let input_position = this.offset();
    let targetHeight = this.height() + 6;
    let targetWidth = this.width();
    let body = $("body");
    let panel = body.appendDiv(content);
    let panelElement = panel[0];
    let element = this[0];
    let focusedElement;

    body.mousedown((e) => {

        if (e.target != panelElement && e.target != element && !panel.contains(e.target)) {

            focusedElement = null;
            panel.fadeOut();
        }
    });

    body.keydown((e) => {

        let key = event.key;

        if (key == "Escape") {
            panel.fadeOut();
        }
    });

    panel.blur(() => {
        focusedElement = null;
    });

    panel.mousedown(() => {
        focusedElement = panelElement;
    });

    window.setTimeout(() => {

        panel.find("*").mousedown(() => {
            focusedElement = panelElement;
        });

    }, 1);

    panel.css({
        'position': 'absolute',
        'padding': '3px',
        'display': 'none',
        'z-index': 2,
        'background-color': 'white',
        'top': (input_position.top + targetHeight) + 'px',
        'left': (input_position.left) + 'px',
        'border': '1px solid gray',
        'box-shadow': '2px 2px 2px #aaaaaa'
    });

    this.focusin(() => {

        console.debug("fadeIn");

        panel.fadeIn();
    });

    this.click(() => {

        let target = event.target;

        if (panel && !focusedElement) {
            panel.fadeToggle();
        }

        focusedElement = null;
    });

    this.blur(() => {

        window.setTimeout(() => {

            let focused = $(":focus");
            let panelHasFocused = panel.has(focused).length > 0;

            if (focusedElement == panelElement) {

                panel.focusout(() => {
                    if (focusedElement != panelElement) {
                        panel.fadeOut();
                    }
                });
            }
            else if (panel && !panelHasFocused) {
                panel.fadeOut();
            }

        }, 1);
    });

    panel.data("dropPanelOwner", this);
}

jQuery.fn.getSelectedText = function () {
    return this.find("option:selected").text();
}

jQuery.fn.getCurrentTableHeader = function () {
    return this.closest('table').find('th').eq(this.index());
}

jQuery.fn.contains = function (element) {
    return $.contains(this[0], element);
}
