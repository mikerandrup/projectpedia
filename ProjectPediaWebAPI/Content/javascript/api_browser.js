(function (exports) {

    'use strict';

    var apiBrowserInterface = {
        init: function () {
            console.log("API Init!");

            methods.attachElements();
            methods.attachEvents();
            methods.configureJqAjax();
        },

        retrieveDataFromRelativeUri: function (relativeUri) {
            var hostPrefix = document.location.origin,
                apiUri = hostPrefix + relativeUri;

            console.log("Making call to " + apiUri);

            $.ajax({
                url: apiUri,
                success: callbacks.handleAjaxSuccess,
                failure: callbacks.handleAjaxError
            });
        }
    },

    elements = {
        responseHolderJq: null
    },

    callbacks = {
        handleAjaxSuccess: function (data) {
            var dataAsMarkup = methods.renderResponseData(data);
            elements.responseHolderJq.html(dataAsMarkup);
        },

        handleAjaxError: function (a, b, c) {
            console.log("some kinda error:", a, b, c);
        }
    },

    methods = {
        attachElements: function () {
            elements.responseHolderJq = $("#responseHolder");
        },

        attachEvents: function () {

        },

        configureJqAjax: function () {
            $.ajaxSetup({
                cache: false,
                dataType: "json",
                type: "GET"
            });
        },

        renderResponseData: function (data) {
            var htmlString = render.parse(data);

            return htmlString;
        },

        createResponseContainer: function () {
            var responseEl = document.createElement("div");
            responseEl.className = "";

            return responseEl;
        },

        getType: function (value) {
            // thanks Doug! http://javascript.crockford.com
            var s = typeof value;
            if (s === 'object') {
                if (value) {
                    if (value instanceof Array) {
                        s = 'array';
                    }
                } else {
                    s = 'null';
                }
            }
            return s;
        }

    },

    render = {
        parse: function (currentNode) {
            var nodeMarkup = "",
                nodeType = methods.getType(currentNode);

            switch (nodeType) {
                case "object":
                    nodeMarkup = render.objectNode(currentNode);
                    break;
                case "array":
                    nodeMarkup = render.arrayNode(currentNode);
                    break;
                default:
                    nodeMarkup = render.valueNode(currentNode);
            }

            return nodeMarkup;
        },

        objectNode: function (objectPrimitive) {
            var markup = "<div class='renderedObject'>",
                key;

            for (key in objectPrimitive) {
                markup += "<p>";

                if (key === "detailUri") {
                    markup += render.apiLink(objectPrimitive[key])
                }
                else {
                    markup +=
                    "<label>" + key + "</label>" +
                    render.parse(objectPrimitive[key]);
                }
                markup += "</p>";
            }

            return markup + "</div>";
        },

        arrayNode: function (arrayPrimitive) {
            var markup = "<div class='renderedArray'>",
                i,
                length = arrayPrimitive.length;

            for (i = 0; i < length; i++) {
                markup += 
                    render.parse(arrayPrimitive[i]);
            }

            return markup + "</div>";
        },

        valueNode: function (valuePrimitive, keyName) {

            var markup =
                "<span>" +
                valuePrimitive +
                "</span>";

            return markup;
        },

        apiLink: function (apiUrl) {
            var markup = "<span " + 
                "class='apiLink imClickable' " + 
                "data-apiuri='" + apiUrl + "'>" +
                "&raquo;" +
                "</span>";

            return markup; 
        }
    };

    exports.APIBrowser = apiBrowserInterface;
    exports.Render = render;

} (this));