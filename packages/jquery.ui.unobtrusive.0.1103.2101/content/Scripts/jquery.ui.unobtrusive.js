/// <reference path="jquery-1.4.4.js" />
/// <reference path="jquery-ui.js" />
/// <reference path="jquery.ui.requireconfirmation.js" />

(function ($, undefined) {

    $.fn.setupUnobtrusiveChildren = function () {
        setupAllUnobtrusiveChildrenOf($(this));
    }

    // Helpers for handling attributes casing

    function getAllAttributes(jQueryElement, prefix, mapAttrNameFunc) {
        var domElement = jQueryElement.get(0);
        var result = {};
        var attributes = domElement.attributes;
        for (var i = 0; i < attributes.length; i++) {
            var attrName = attributes.item(i).name;

            if (attrName.indexOf(prefix) == 0) {
                var attrValue = jQueryElement.attr(attrName);
                var attrNameWithoutPrefix = attrName.substring(prefix.length);
                var attrValue = jQueryElement.attr(attrName);
                var resultAttrName = attrNameWithoutPrefix;
                if (mapAttrNameFunc) {
                    resultAttrName = mapAttrNameFunc(resultAttrName);
                }
                result[resultAttrName] = attrValue;
            }
        }
        return result;
    }

    function fixKnownAttrCasing(attrNameInLowerCase, knownAttributes) {
        for (var i = 0; i < knownAttributes.length; i++) {
            if (knownAttributes[i].toLowerCase() == attrNameInLowerCase) {
                return knownAttributes[i];
            }
        }
        return attrNameInLowerCase;
    }

    // Datepicker
    function fixKnownDatepickerAttrCasing(attrName) {
        return fixKnownAttrCasing(attrName, datepickerKnownAttributes);
    }

    function setupUnobtrusiveDatepicker(jQueryElement) {
        jQueryElement.datepicker(getAllAttributes(jQueryElement, "data-ui-datepicker-", fixKnownDatepickerAttrCasing));
    }

    function setupAllUnobtrusiveDatepickerChildrenOf(jQueryElement) {
        jQueryElement.find("[data-ui-datepicker=true]").each(function () {
            setupUnobtrusiveDatepicker($(this));
        });
    }

    var datepickerKnownAttributes = [
        "buttonImage", "buttonImageOnly", "altField", "altFormat", "appendText", "autoSize", "buttonText",
        "calculateWeek", "changeMonth", "changeYear", "closeText", "constrainInput", "currentText", "dateFormat", "dayNames",
        "dayNamesMin", "dayNamesShort", "defaultDate", "firstDay", "gotoCurrent", "hideIfNoPrevNext", "isRTL", "maxDate",
        "minDate", "monthNames", "monthNamesShort", "navigationAsDateFormat", "nextText", "numberOfMonths", "prevText",
        "selectOtherMonths", "shortYearCutoff", "showAnim", "showOn", "showOptions", "showOtherMonths", "showWeek",
        "stepMonths", "weekHeader", "yearRange", "yearSuffix"
        ];

    // Confirm action
    function setupUnobtrusiveConfirmAction(jQueryElement) {
        var message = jQueryElement.attr("data-ui-require-confirmation");
        if (message == null || message == 'undefined') {
            message = "Are you sure?";
        }
        jQueryElement.requireConfirmation(message);
    }

    function setupAllUnobtrusiveConfirmActionsChildrenOf(jQueryElement) {
        jQueryElement.find("[data-ui-require-confirmation]").each(function () {
            setupUnobtrusiveConfirmAction($(this));
        });
    }

    // Global plugin functions
    function setupAllUnobtrusiveChildrenOf(jQueryElement) {
        setupAllUnobtrusiveDatepickerChildrenOf(jQueryElement);
        setupAllUnobtrusiveConfirmActionsChildrenOf(jQueryElement);
    }

    $(function () {
        $(document).setupUnobtrusiveChildren();
    });
})(jQuery);