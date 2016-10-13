$(function () {
    var loadUserApplications = function () {
        var applicationsElement = $("li[data-appliactions-url]").first();

        if (!applicationsElement)
            throw "Applications element is not defined";

        var url = $(applicationsElement).data("appliactions-url");

        $.ajax({
            method: "GET",
            url: url,
            success: function (data) {
                $(applicationsElement).html(data);
            },
            error: function () {
                templateUtilities.showErrorPage();
            }
        });
    };

    loadUserApplications();
});