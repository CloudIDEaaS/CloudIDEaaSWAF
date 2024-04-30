'use strict';
import * as $ from "jquery";
require("./carouselForm.js");
require("./itemCarouselForm");
require("./jqueryUIHelpers");
require("jquery-ui");
const { PropertiesNode } = require("./propertiesNode");
const jQuery = $;

jQuery.fn.addColumn = function (data) {

    let name = data[0];
    let dataType = data[1];
    let properties = data[2];
    let row = this.insertTableRowAt(-1);
    let rowSelector = row.appendTableCell({
        class: "rowselector",
        placeholder: "Enter mask"
    });

    if (properties) {

        let node = new PropertiesNode();

        Object.assign(node, properties);
        properties = node;
    }
    else {
        properties = new PropertiesNode("properties");
        data[2] = properties;
    }

    row.appendTableCell({
        class: "datagridcell editable",
        contenteditable: "true",
        innerText: name,
        spellcheck: false
    });

    row.appendTableCell({
        class: "datagridcell editable",
        contenteditable: "true",
        innerText: dataType
    });

    row.appendTableCell({
        class: "datagridcell",
        data: {
            cellData: {
                data: data,
                properties: properties
            },
        }
    });

    rowSelector.mousedown(function () {
        $(this).parent().addClass('selected').siblings().removeClass('selected');
    });

    row.find(".datagridcell").focusin(function () {

        let td = $(this);
        let focusTimestamp = td.data("focusTimestamp");

        td.handleCellState();

        if (focusTimestamp) {

            setTimeout(() => {
                td.data("focusTimestamp", null);
            }, 500);
        }
    });

    return row;
}

jQuery.fn.addNew = function (data) {

    let row = this.appendTableRow();
    let rowSelector = row.appendTableCell({
        class: "rowselector"
    });

    row.appendTableCell({
        class: "datagridcell editable",
        contenteditable: "true"
    });

    row.appendTableCell({
        class: "datagridcell editable",
        contenteditable: "true"
    });

    row.appendTableCell({
        class: "datagridcell"
    });

    rowSelector.mousedown(function () {
        $(this).parent().addClass('selected').siblings().removeClass('selected');
    });

    row.find(".datagridcell").focusin(function () {

        let td = $(this);
        let focusTimestamp = td.data("focusTimestamp");

        td.handleCellState();

        if (focusTimestamp) {

            setTimeout(() => {
                td.data("focusTimestamp", null);
            }, 500);
        }
    });

    return row;
}

jQuery.fn.tabNextTable = function () {

    let td = this.closest("td");
    let next = td.next();
    let current = td;

    while (next.length == 0) {

        current = current.parent();
        next = current.next();

        if (next.length) {
            if (next.tagName() == "tr") {
                next = next.children("td.datagridcell:first");
            }
        }
    }

    event.cancelBubble = true;
    event.preventDefault();

    next.data("focusTimestamp", Date.now());

    console.debug("next.focus");
    next.focus();

    return next;
}

jQuery.fn.tabPrevTable = function () {

    let td = this.closest("td");
    let prev = td.prev()
    let current = td;
    let target = event.target;

    while (prev.length == 0) {

        current = current.parent();
        prev = current.next();

        if (prev.length) {
            if (prev[0].tagName.toLowerCase() == "tr") {
                prev = next.children("td.datagridcell:first");
            }
        }
    }

    event.cancelBubble = true;
    event.preventDefault();

    prev.data("focusTimestamp", Date.now());
    console.debug("prev.focus");

    prev.focus();

    return prev;
}

jQuery.fn.getVariables = function () {

    let dataGridData = this.closest(".datagriddata")
    let dataGridHeader = this.closest(".datagridcontainer").find(".datagridheader");
    let dataTypesDropdown = $(window).data("dataTypesDropdown");
    let slides = $(window).data("slides");
    let entityChanged = $(window).data("entityChanged");

    return [dataGridData, dataGridHeader, dataTypesDropdown, slides, entityChanged];
}

jQuery.fn.handleCellState = function () {

    let th = this.getCurrentTableHeader();
    let colName = th.text();
    let carouselForm;
    let [dataGridData, dataGridHeader, dataTypesDropdown, slides, entityChanged] = this.getVariables();

    let carouselContainerLostFocus = (propertiesCell) => {

        let assignments = propertiesCell.getCarouselAssignments();
        let carouselForm = assignments.carouselForm;
        let propertiesNode = carouselForm.currentState.properties;
        let cellData = propertiesCell.data("cellData");

        Object.assign(cellData.properties, propertiesNode);

        entityChanged(propertiesNode);
    }

    if (!this.hasClass("rowselector")) {
        $(".selected").removeClass("selected");
    }

    switch (colName) {
        case "Data Type": {

            if (!this.has(dataTypesDropdown).length) {

                let width;
                let height;
                let value = this.text();

                this.addClass("overlay");
                this.text("");

                width = this.width();
                height = this.height();

                dataTypesDropdown.width(width - 1);
                dataTypesDropdown.height(height - 1);

                dataTypesDropdown.val(value);

                try {
                    this.append(dataTypesDropdown);
                }
                catch (e) {

                    console.error(e);

                    dataTypesDropdown.remove();
                    this.append(dataTypesDropdown);
                }

                dataTypesDropdown.show();
                dataTypesDropdown.focus();

                dataTypesDropdown.focusout(() => {

                    let td = dataTypesDropdown.parent();
                    let selected = dataTypesDropdown.getSelectedText();
                    let focusTimestamp = td.data("focusTimestamp");
                    let propertiesCell = td.next();

                    if (focusTimestamp) {

                        // stupid hack

                        let now = Date.now();

                        if (now - focusTimestamp < 100) {

                            console.debug("dataTypesDropdown.focus");
                            dataTypesDropdown.focus();
                            return;
                        }
                    }

                    dataTypesDropdown.remove();
                    dataTypesDropdown.hide();

                    td.removeClass("overlay");
                    td.html(selected);

                    propertiesCell.checkCarouselFocusOut(() => {

                        let propertiesCell = td.next("propertiesCell");

                        carouselContainerLostFocus(propertiesCell);
                    });
                });

                dataTypesDropdown.keydown(() => {

                    let key = event.key;

                    if (key == "Tab") {

                        if (event.shiftKey) {
                            dataTypesDropdown.tabPrevTable();
                        }
                    }
                });

                let dataTypeChange = () => {

                    let selected = dataTypesDropdown.getSelectedText();
                    let propertiesCell = this.next();
                    let carouselContainer;
                    let initialState;
                    let cellWidth;
                    let cellHeight;
                    let rowIndex = $(this).parent().index("table tbody tr");
                    let assignments = propertiesCell.getCarouselAssignments();
                    let columnData = propertiesCell.data("customData");
                    let cellData = propertiesCell.data("cellData");
                    let propertiesNode;

                    if (cellData.properties) {
                        propertiesNode = cellData.properties;
                    }
                    else {
                        propertiesNode = new PropertiesNode(`rowProperties[${rowIndex}]`)
                    }

                    propertiesCell.addClass("overlay propertiesCell");
                    propertiesCell.text("");

                    cellWidth = propertiesCell.width();
                    cellHeight = propertiesCell.height();

                    carouselContainer = propertiesCell.data("carouselContainer");

                    initialState = {
                        id: new Date().getTime(),
                        case: selected,
                        columnData: columnData,
                        tabNext: (e) => {

                            let focus = e.tabNextTable();

                            if (focus.tagName() != "td") {

                                let tr = dataGridData.addNew();
                                let tdFirst = tr.children("td.datagridcell:first");

                                tdFirst.focus();
                            }
                        },
                        tabPrev: (e) => {

                            let focus = e.tabPrevTable();
                        },
                        addItemCaseActivatedHandler: null,
                        properties: propertiesNode,
                        customData: {
                            cellWidth: cellWidth,
                            cellHeight: cellHeight
                        }
                    };

                    if (carouselContainer) {

                        let inlineElementDropPanel = $(".inlineElementDropPanel");

                        if (inlineElementDropPanel) {
                            inlineElementDropPanel.remove();
                        }

                        carouselContainer.remove();
                    }

                    carouselContainer = propertiesCell.appendDiv();

                    carouselContainer.width(cellWidth - 1);
                    carouselContainer.height(cellHeight - 1);

                    carouselForm = carouselContainer.carouselForm({
                        slides: slides,
                        lostFocus: () => carouselContainerLostFocus(propertiesCell),
                        state: initialState
                    }).carouselForm("instance");

                    propertiesCell.assignCarousel(carouselContainer, carouselForm);
                };
                dataTypesDropdown.change(() => {
                    dataTypeChange();
                });

                dataTypeChange();
            }

            break;
        }
        case "Field Name": {

            this.selectText();
            break;
        }
        case "Properties": {
            break;
        }
        default: {
            break;
        }
    }
}

$.widget("custom.entityEditor", {
    // default options
    _create: function () {

        let entity = this.options.entity;
        let slides = this.options.slides;
        let dataTypes = this.options.dataTypes;
        let entityChanged = this.options.entityChanged;
        let parentElement = $(this.element);
        let dataTypesDropdown = $("<select class='typesDropdown'></select>");
        let dataGridData = parentElement.find(".datagriddata")
        let dataGridHeader = parentElement.find(".datagridheader");
        let $window = $(window);

        $window.data("dataTypesDropdown", dataTypesDropdown);
        $window.data("slides", slides);
        $window.data("entityChanged", entityChanged);

        setTimeout(($) => {

            dataGridHeader.html(entity.name)
            dataGridData.addNew();

            $.each(entity.columns, (key, value) => {
                let row = dataGridData.addColumn(value);
            });

        }, 1, $);

        $.each(dataTypes, (key, value) => {
            dataTypesDropdown
                .append($("<option></option>")
                    .attr("value", value)
                    .text(value));
        });

        parentElement.find(".datagridcontainer").click(function () {

            let selectedRow = $(".selected")
            let target = $(event.target);

            if (target[0] == dataTypesDropdown[0]) {
                return;
            }

            if (selectedRow.find(target).length == 0) {
                $(".selected").removeClass('selected');
            }

            if (target.tagName().toLowerCase() == "td") {
                let td = $(target);
                td.handleCellState();
            }
        });
    }
});
