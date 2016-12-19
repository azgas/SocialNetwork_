$(function () {
    var loadUserAllowedProjects = function () {
        var projectsElement = $("li[data-projects-url]").first();

        if (!projectsElement)
            throw "Projects element is not defined";

        var url = $(projectsElement).data("projects-url");

        $.ajax({
            method: "GET",
            url: url,
            success: function (data) {
                $(projectsElement).html(data);
            },
            error: function () {
                templateUtilities.showErrorPage();
            }
        });
    };

    loadUserAllowedProjects();

    $(document).on("click", "a[data-select-project-url]", function (event) {
        var url = $(this).data("select-project-url");
        var projectUri = $(this).data("select-project-projecturi");

        if (!url)
            throw "Select project url is not specified"

        if (!projectUri)
            throw "Project uri is not specified";

        $.ajax({
            method: "POST",
            url: url,
            data: {
                projectUri: projectUri
            },
            success: function (data) {
                window.location = data;
            },
            error: function () {
                templateUtilities.showErrorPage();
            }
        });

        event.preventDefault();
    });
});