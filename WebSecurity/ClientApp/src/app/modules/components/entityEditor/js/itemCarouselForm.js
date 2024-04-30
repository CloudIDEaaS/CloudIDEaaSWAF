import * as $ from "jquery";
require("./itemCarouselForm.js");
require("jquery-ui");
const { PropertiesNode } = require("./propertiesNode");
const jQuery = $;

jQuery.fn.canTabNextForm = function () {

    let td = this.closest("td");
    let next = td.next(":parent");
    let current = td;

    while (next.length == 0) {

        current = current.parent();
        next = current.next();

        if (next.length) {

            if (next[0].tagName.toLowerCase() == "tr") {

                next = next.children("td");
                next = next.find(":input")
            }
            else {
                break;
            }
        }
        else {
            break;
        }
    }

    return next.length;
}

jQuery.fn.canTabPrevForm = function () {

    let td = this.closest("td");
    let prev = td.prev(":parent");
    let current = td;

    console.debug("tabPrevForm");

    while (prev.length == 0) {

        current = current.parent();
        prev = current.prev();

        if (prev.length) {

            if (prev[0].tagName.toLowerCase() == "tr") {

                prev = prev.children("td");
                prev = prev.find(":input")
            }
            else {
                break;
            }
        }
        else {
            break;
        }
    }

    return prev.length;
}

jQuery.fn.tabNextForm = function () {

    let td = this.closest("td");
    let next = td.next();
    let current = td;

    while (next.length == 0) {

        current = current.parent();
        next = current.next();

        if (next.length) {

            if (next[0].tagName.toLowerCase() == "tr") {

                next = next.children("td");
                next = next.find(":input")
            }
            else {
                break;
            }
        }
        else {
            break;
        }
    }

    event.cancelBubble = true;
    event.preventDefault();

    if (next.length) {

        if (next[0].tagName.toLowerCase() == "td") {
            next = next.find(":input")
        }

        next.data("focusTimestamp", Date.now());
        console.debug("next.focus");
        next.focus();

        return true;
    }
    else {
        return false;
    }
}

jQuery.fn.tabPrevForm = function () {

    let td = this.closest("td");
    let prev = td.prev();
    let current = td;

    console.debug("tabPrevForm");

    while (prev.length == 0) {

        current = current.parent();
        prev = current.prev();

        if (prev.length) {

            if (prev[0].tagName.toLowerCase() == "tr") {
                prev = prev.children("td");
                prev = prev.find(":input")
            }
            else {
                break;
            }
        }
        else {
            break;
        }
    }

    event.cancelBubble = true;
    event.preventDefault();

    if (prev.length) {

        if (prev[0].tagName.toLowerCase() == "td") {
            prev = prev.find(":input")
        }

        prev.data("focusTimestamp", Date.now());
        prev.focus();

        return true;
    }
    else {
        return false;
    }
}

$.widget("custom.itemCarouselForm", {
    // default options
    currentState: {},
    propertiesRootNode: null,
    container: null,
    items: null,
    onItemSelected: null,
    raiseOnItemSelected: function (item, data, elem, checked) {
        this.onItemSelected(item, data, elem, checked);
    },
    _create: function () {

        let itemTemplate = this.options.itemTemplate;
        let data = this.options.items;
        let parentElement = $(this.element);
        let slides = this.options.slides;

        this.items = data;
        this.currentState = this.options.state;
        this.propertiesRootNode = this.currentState.properties;

        this.container = parentElement.appendTable({
            columns: [
                {
                    itemTemplate: (td, data) => {

                        let item = itemTemplate(td, data);
                        let inner = td.find(":input");

                        inner.on("keydown", () => {

                            let key = event.key;
                            let target = $(event.target);

                            switch (key) {

                                case "Tab": {

                                    let dropPanelOwner = parentElement.data("dropPanelOwner");

                                    if (event.shiftKey) {

                                        if (!target.canTabPrevForm()) {

                                            let tabPrev = dropPanelOwner.data("tabPrev");

                                            tabPrev();
                                        }
                                    }
                                    else {

                                        if (!target.canTabNextForm()) {

                                            let tabNext = dropPanelOwner.data("tabNext");

                                            tabNext();
                                            target.checkDropPanelFocusOut();
                                        }
                                    }

                                    break;
                                }
                            }
                        });

                        return item;
                    }
                },
                {

                    itemTemplate: (td, data) => {

                        let propertiesCell = td;
                        let propertiesRootNode = data.state.properties;
                        let carouselContainer;
                        let initialState;
                        let cellWidth;
                        let cellHeight;
                        let selectedCellHeight;
                        let state = data.state;
                        let originalCellWidth = 1;
                        let carouselForm;
                        let itemPropertiesNode;
                        let propertiesCellWidth;
                        let currentState = this.currentState;

                        $.each(propertiesRootNode.childProperties, (i) => {

                            let property = propertiesRootNode.childProperties[i];

                            if (property.name == data.name) {
                                itemPropertiesNode = property;
                            }
                        });

                        if (!itemPropertiesNode) {
                            itemPropertiesNode = new PropertiesNode(data.name);
                            propertiesRootNode.childProperties.push(itemPropertiesNode);
                        }

                        propertiesCell.addClass("overlay propertiesCell");
                        propertiesCell.text("");

                        propertiesCell.css({
                            "vertical-align": "top"
                        });

                        cellWidth = 275;
                        selectedCellHeight = 25;

                        state.addItemCaseActivatedHandler(data.name, (selectedCase, item, data, element, selected) => {

                            let propertiesCellWidth = propertiesCell.width();
                            let addCarousel;
                            let getProperties;

                            carouselContainer = propertiesCell.data("carouselContainer");
                            cellHeight = propertiesCell.height();

                            let carouselContainerLostFocus = (element) => {
                                propertiesCell.width(originalCellWidth);
                                propertiesCell.height(selectedCellHeight);
                            }

                            initialState = {
                                id: new Date().getTime(),
                                columnData: data.state.columnData,
                                case: selectedCase,
                                properties: itemPropertiesNode,
                                tabNext: (e) => {

                                    if (!e.tabNextForm()) {
                                        carouselContainer.remove();
                                    }
                                },
                                tabPrev: (e) => {

                                    if (!e.tabPrevForm()) {
                                        carouselContainer.remove();
                                    }
                                },
                                customData: {
                                    cellWidth: cellWidth,
                                    cellHeight: selectedCellHeight
                                }
                            };

                            if (carouselContainer) {
                                carouselContainer.remove();
                            }

                            getProperties = (getProperties, inlineElement, dropPanel, state) => {

                                let name = data.name;
                                let checked = ($(element).prop("checked"));
                                let properties = { selected: checked };

                                Object.assign(properties, getProperties(inlineElement, dropPanel, state));

                                return properties;
                            }

                            addCarousel = () => {

                                propertiesCell.width(cellWidth);

                                carouselContainer = propertiesCell.appendDiv();

                                carouselContainer.width(cellWidth - 1);
                                carouselContainer.height(cellHeight - 1);
                                propertiesCell.data("carouselContainer", carouselContainer);

                                propertiesCell.height(selectedCellHeight);

                                carouselForm = carouselContainer.carouselForm({
                                    slides: slides,
                                    lostFocus: () => carouselContainerLostFocus(element),
                                    getProperties: getProperties,
                                    state: initialState
                                }).carouselForm("instance");

                                propertiesCell.assignCarousel(carouselContainer, carouselForm);
                            };

                            $(element).focusin(() => {

                                // assumes is checkbox, better handling?

                                if ($(element).prop("checked")) {

                                    if (propertiesCell.has(carouselContainer).length == 0) {
                                        addCarousel();
                                    }
                                }
                            });

                            $(element).focusout(() => {
                                $(this).checkDropPanelFocusOut();
                            });

                            if (selected) {
                                addCarousel();
                            }
                            else {
                                propertiesCell.width(originalCellWidth);
                            }
                        });
                    }
                }
            ],
            data: data
        });

        parentElement.find(":input").keydown((e) => {

            let key = event.key;
            let target = e.target;
            let inputs = parentElement.find(":input");
            let currentIndex = inputs.index(target);
            let newIndex = 0;

            switch (key) {

                case "ArrowDown": {

                    newIndex = currentIndex + 1;

                    if (newIndex < inputs.length) {

                        console.debug("inputs[newIndex].focus");
                        inputs[newIndex].focus();
                    }

                    event.cancelBubble = true;
                    event.preventDefault();

                    break;
                }
                case "ArrowUp": {

                    newIndex = currentIndex - 1;

                    if (newIndex >= 0) {

                        console.debug("inputs[newIndex].focus");
                        inputs[newIndex].focus();
                    }
                    else {

                        console.debug("parentElement.focus");
                        parentElement.focus();
                    }

                    event.cancelBubble = true;
                    event.preventDefault();

                    break;
                }
            }
        });
    }
});