import * as $ from "jquery";
require("./itemCarouselForm.js");
require("./jquery.jcarousel.js");
require("jquery-ui");
const { PropertiesNode } = require("./propertiesNode");
const jQuery = $;

jQuery.fn.assignCarousel = function (carouselContainer, carouselForm) {

    let carouselList = this.getCarouselList();

    this.data("carouselAssignments", {
        carouselContainer: carouselContainer,
        carouselForm: carouselForm
    });

    carouselList.push(carouselForm);
}

jQuery.fn.getCarouselList = function () {

    let carouselList;
    let window$ = $(window);

    carouselList = window$.data("carouselList");

    if (!carouselList) {
        carouselList = [];

        window$.data("carouselList", carouselList);
    }

    return carouselList;
}

jQuery.fn.getCarouselAssignments = function () {

    let assignments = this.data("carouselAssignments");

    return assignments;
}

jQuery.fn.appendCheckboxListCarouselForm = function (config) {

    let items = config.items;

    let itemCarouselForm;
    let itemTemplate = (p, d) => {

        let checkBox = p.appendCheckbox(d.text, d.value);

        checkBox.focusin((e) => {

            let elem = e.target;
            let checked = elem.checked;
            let itemCarouselFormInstance = itemCarouselForm.itemCarouselForm("instance");

            itemCarouselFormInstance.raiseOnItemSelected(d.name, d, elem, checked);
        });

        checkBox.change((e) => {

            let elem = e.target;
            let checked = elem.checked;
            let itemCarouselFormInstance = itemCarouselForm.itemCarouselForm("instance");

            itemCarouselFormInstance.raiseOnItemSelected(d.name, d, elem, checked);
        });
    };

    itemCarouselForm = this.itemCarouselForm({
        items: config.items,
        slides: config.slides,
        state: config.state,
        itemTemplate: itemTemplate,
    });

    return itemCarouselForm;
};

jQuery.fn.checkCarouselFocusOut = function (callback) {

    let carouselFormList = this.find(".carouselForm ul");
    let dropPanel = this.getChildData("inlineElementDropPanel");
    let thisEvent = event;
    let eventTarget = thisEvent.target;

    if (carouselFormList) {

        setTimeout(() => {

            let focused = $(":focus");
            let listHasFocused = carouselFormList.has(focused).length > 0;
            let dropHasFocused = false;

            if (dropPanel) {

                if (focused.length == 0 && thisEvent.constructor.name == "FocusEvent") {
                    listHasFocused = carouselFormList.has(eventTarget).length > 0;
                }

                dropHasFocused = dropPanel && dropPanel.has(focused).length > 0;

                if (!dropHasFocused) {
                    dropHasFocused = dropPanel && dropPanel.has(eventTarget).length > 0;
                }
            }

            if (!listHasFocused && !dropHasFocused) {

                let td = carouselFormList.closest("td");
                let assignments = td.getCarouselAssignments();
                let carouselContainer = assignments.carouselContainer;

                if (carouselContainer) {
                    carouselContainer.remove();
                }

                if (dropPanel) {
                    dropPanel.fadeOut();
                }

                callback();
            }
        }, 100);
    }
}

jQuery.fn.checkDropPanelFocusOut = function () {

    let dropPanel = $(".inlineElementDropPanel");
    let thisEvent = event;
    let eventTarget = thisEvent.target;

    if (dropPanel) {

        setTimeout(() => {

            let dropHasFocused = false;
            let focused = $(":focus");

            if (dropPanel) {

                if (dropPanel) {

                    if (focused.length == 0 && thisEvent.constructor.name == "FocusEvent") {
                        listHasFocused = carouselFormList.has(eventTarget).length > 0;
                    }

                    dropHasFocused = dropPanel && dropPanel.has(focused).length > 0;

                    if (!dropHasFocused) {
                        dropHasFocused = dropPanel && dropPanel.has(eventTarget).length > 0;
                    }
                }
            }

            if (!dropHasFocused) {
                dropPanel.fadeOut();
            }
        }, 100);
    }
}

$.widget("custom.carouselForm", {

    currentState: {},
    slides: null,
    name: null,
    lostFocus: null,
    getProperties: null,
    propertiesNode: null,
    rootProperty: null,
    _create: function () {

        let parentElement = $(this.element);

        this.slides = this.options.slides;
        this.currentState = this.options.state;
        this.lostFocus = this.options.lostFocus;
        this.getProperties = this.options.getProperties;
        this.propertiesNode = this.currentState.properties;
        this.name = this.propertiesNode.name;

        this.appendSlides(parentElement, this.slides, this.currentState);
    },
    getCarouselProperties: function (inlineElement, dropPanel, state) {

        let entry = this.slides.cases[state.case];
        let uiElements = entry.uiElements;
        let properties = [];

        $.each(uiElements, (i, e) => {

            if (this.getProperties) {
                let innerProperties = this.getProperties(e.getProperties, inlineElement, dropPanel, state);

                properties.push(innerProperties);
            }
            else {
                let innerProperties = e.getProperties(inlineElement, dropPanel, state);

                if (innerProperties) {
                    properties.push(innerProperties);
                }
            }
        });

        return properties;
    },
    appendSlides: function (parentElement, slides, state) {

        let currentCase = state.case;
        let cases = slides.cases;
        let selectedCaseObject;
        let uiElements = [];
        let carouselFormList;
        let customData = state.customData;
        let propertiesNode = this.propertiesNode;

        for (var caseProperty in cases) {

            if (caseProperty == currentCase) {
                selectedCaseObject = cases[caseProperty];
                this.rootProperty = caseProperty;

                break;
            }
        }

        if (selectedCaseObject) {

            let carousel;
            let wrapper;

            carouselFormList = this.createCarouselFormList(parentElement, customData);
            uiElements = selectedCaseObject.uiElements;

            wrapper = carouselFormList.parents('.jcarousel-wrapper');
            carousel = wrapper.find('.jcarousel');

            uiElements.forEach((e) => {

                let name = e.name;
                let createInline = e.createInline;
                let createDropPanel = e.createDropPanel;
                let inlineElement;
                let dropPanel;
                let dropPanelPanel;
                let targetProperty;
                let newTarget;
                let elementProperties = propertiesNode;
                let currentState = Object.assign(this.currentState, {

                    parentElement: this.createPropertyPanel(carouselFormList, customData),
                    showDropPanel: () => {

                    },
                    showNext: (name) => {

                        let target = carousel.jcarousel('target').first();
                        let last = carousel.jcarousel('last');

                        if (dropPanelPanel) {
                            dropPanelPanel.fadeOut(100);
                        }

                        if (target[0] == last[0]) {
                            currentState.tabNext(target);
                        }
                        else {

                            carousel.on('jcarousel:animateend', function (e, c) {

                                newTarget = carousel.jcarousel('target');
                                targetProperty = newTarget.innerMost();

                                console.debug("targetProperty.focus");
                                targetProperty.focus();
                            });

                            carousel.jcarousel('scroll', '+=1');
                        }
                    },
                    showPrev: (name) => {

                        let target = carousel.jcarousel('target').first();
                        let first = carousel.jcarousel('first');

                        if (dropPanelPanel) {
                            dropPanelPanel.fadeOut(100);
                        }

                        if (target[0] == first[0]) {
                            currentState.tabPrev(target);
                        }
                        else {

                            carousel.on('jcarousel:animateend', function (event, c) {

                                newTarget = carousel.jcarousel('target');
                                targetProperty = newTarget.innerMost();

                                console.debug("targetProperty.focus");
                                targetProperty.focus();
                            });

                            carousel.jcarousel('scroll', '-=1');
                        }
                    },
                    end: () => {

                    },
                    setCaseValue: (caseValue) => {

                    }
                });

                if (createInline) {

                    inlineElement = createInline(elementProperties, currentState);

                    inlineElement.focusout(() => {

                        parentElement.checkCarouselFocusOut(() => {

                            let properties = this.getCarouselProperties(inlineElement, dropPanel, this.currentState);

                            [this.propertiesNode.data] = properties;

                            this.lostFocus(properties);
                        });
                    });

                    inlineElement.on("keydown", () => {

                        let key = event.key;
                        let inlinePlaceholder = inlineElement[0].placeholder;

                        switch (key) {

                            case "Tab": {

                                if (event.shiftKey) {
                                    currentState.showPrev();
                                }
                                else {
                                    currentState.showNext();
                                }

                                event.cancelBubble = true;
                                event.preventDefault();

                                break;
                            }
                            case "ArrowDown": {

                                dropPanel = inlineElement.data("inlineElementDropPanel");

                                event.cancelBubble = true;
                                event.preventDefault();

                                if (dropPanel) {

                                    let firstChild = dropPanel.find(":input").first();

                                    console.debug("firstChild.focus");
                                    firstChild.focus();
                                }

                                break;
                            }
                        }
                    });
                }

                if (inlineElement && createDropPanel) {

                    inlineElement.createDropPanel((p) => {

                        let state = currentState;
                        let showNext = state.showNext;
                        let showPrev = state.showPrev;
                        let dropState = Object.assign(currentState, {
                            parentElement: p
                        });

                        p.addClass("inlineElementDropPanel");
                        dropPanelPanel = p;

                        inlineElement.data("tabNext", (d) => {

                            showNext();

                            event.cancelBubble = true;
                            event.preventDefault();
                        });

                        inlineElement.data("tabPrev", (d) => {

                            let dropPanelOwner = p.data("dropPanelOwner");

                            console.debug("dropPanelOwner.focus");
                            dropPanelOwner.focus();

                            event.cancelBubble = true;
                            event.preventDefault();
                        });

                        dropPanel = createDropPanel(elementProperties, dropState);
                        inlineElement.data("inlineElementDropPanel", dropPanel);

                        dropPanel.find(":input").focusout(() => {

                            parentElement.checkCarouselFocusOut(() => {

                                let properties = this.getCarouselProperties(inlineElement, dropPanel, this.currentState);

                                this.lostFocus(properties);
                            });
                        });
                    });
                }
            });

            carousel.jcarousel();
        }
    },
    createCarouselFormList(parent, customData) {

        let list = parent.appendDiv({
            class: "carouselForm"
        })
            .appendDiv({
                class: "jcarousel-wrapper"
            })
            .appendDiv({
                class: "jcarousel"
            })
            .appendListUnordered();

        return list;
    },
    createPropertyPanel(list, customData) {

        let listItem = list.appendListItem();
        let propertyItem = listItem.appendDiv({
            class: "propertyItem",
            style: {
                width: customData.cellWidth - 2 + "px",
                height: customData.cellHeight - 2 + "px"
            }
        });

        return propertyItem;
    },
    _generateHtml: function () {

        let parentElement = $(this.element);
    },
    _setOption: function (key, value) {

        if (key === "value") {
            value = this._constrain(value);
        }

        this._super(key, value);
    },
    _setOptions: function (options) {

        this._super(options);
    }
});
