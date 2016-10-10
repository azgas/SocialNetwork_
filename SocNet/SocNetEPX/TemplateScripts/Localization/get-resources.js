var resources = {};

$.getJSON("Resources/GetResources", function (data) {
    resources = data;
});