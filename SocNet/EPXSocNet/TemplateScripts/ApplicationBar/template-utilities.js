var TemplateUtilities = (function () {
    function TemplateUtilities() { }

    var getErrorPageUrl = function () {
        var errorPageUrl = $("span[data-error-url]").data("error-url");

        if (!errorPageUrl)
            throw "Error page url element not defined";

        return errorPageUrl;
    };

    TemplateUtilities.prototype.showErrorPage = function () {
        var errorPageUrl = getErrorPageUrl();
        window.location = errorPageUrl;
    };

    return TemplateUtilities;
}());

var templateUtilities = new TemplateUtilities();