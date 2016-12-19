var modalCount = 0;

function setPopupReloadUrl(modalId, url) {
    $('div#' + modalId + '.modal-dialog-container').attr('data-ajax-modal-reload-url', url);
}

function addPopup(modalId, modalTitle, loadingIndicatorPah, modalWindowClasses, isInner) {
    var modalClasses = 'modal-dialog-container modal fade colored-header ';

    if (modalWindowClasses !== undefined)
        modalClasses += modalWindowClasses;

    $(document.body).append('<div id="' + modalId + '" '
        + 'class=" ' + modalClasses + '" '
        + 'data-ajax-modal-title="' + modalTitle + '" '
        + 'data-ajax-modal-loading-indicator-path="' + loadingIndicatorPah + '" >'
        + '<div class="modal-dialog">'
        + '<div class="modal-content">'
        + '</div>'
        + '</div>'
        + '</div>');

    var modalDialogContainer = $('div#' + modalId + '.modal-dialog-container');

    if (isInner) {
        modalDialogContainer.modal({ backdrop: false });

        var margin = 30 + 20 * modalCount;
        modalDialogContainer.find('.modal-content').attr('style', 'margin-top:' + margin + 'px');
    }
    else
        modalDialogContainer.modal();

    modalDialogContainer.on("hidden.bs.modal", function () {
        $(this).trigger('modal-hidden');
        $(this).remove();

        modalCount = modalCount - 1;
    })

    modalCount = modalCount + 1;
}

function setLoadingContentForPopup(modalId) {
    var modalDialogContainer = $('div#' + modalId + '.modal-dialog-container');
    var modalTitle = modalDialogContainer.data('ajax-modal-title');
    var loadingIndicatorPath = modalDialogContainer.data('ajax-modal-loading-indicator-path');

    modalDialogContainer.find('div.modal-dialog > div.modal-content').html(
        '<div class="modal-header">'
        + '<h3>' + modalTitle + '<h3>'
        + '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>'
        + '</div>'
        + '<div class="modal-body">'
        + '<div class="text-center">'
        + '<img src="' + loadingIndicatorPath + '" />'
        + '</div>'
        + '</div>');
}

function checkAjaxModalLinksForPopup(element) {
    element.find("a[data-ajax-modal-link='true']").click(function (event) {
        event.preventDefault();

        var modalId = $(this).data("ajax-modal-id");
        var methodUrl = $(this).attr("href");

        var modalTitle = $(this).data('ajax-modal-title');
        var loadingIndicatorPah = $(this).data('ajax-modal-loading-indicator-path');
        var modalWindowClasses = $(this).data('ajax-modal-classes');

        var otherDialog = $('.modal-dialog-container');
        if (otherDialog.length > 0)
            addPopup(modalId, modalTitle, loadingIndicatorPah, modalWindowClasses, true);
        else
            addPopup(modalId, modalTitle, loadingIndicatorPah, modalWindowClasses, false);

        setPopupReloadUrl(modalId, methodUrl);

        setLoadingContentForPopup(modalId);

        $.ajax({
            type: "GET",
            url: methodUrl,
            dataType: "html",
            success: function (data) { replacePopupContent(modalId, data); },
            error: function () { window.location.replace("/Home/Error/"); }
        });
    })
}

function replacePopupContent(modalId, data) {
    var modalDialogContainer = $('div#' + modalId + '.modal-dialog-container');

    modalDialogContainer.find('div.modal-dialog > div.modal-content').html(data);

    modalDialogContainer.find("form[data-ajax-inner-modal-form='true']").submit(function () {
        var methodUrl = $(this).attr("action");

        $.ajax({
            type: "POST",
            url: methodUrl,
            dataType: "html",
            data: $(this).serialize(),
            success: function (data) { replacePopupContent(modalId, data); },
            error: function () { window.location.replace("/Home/Error"); }
        });

        setLoadingContentForPopup(modalId);

        return false;
    })

    modalDialogContainer.find("a[data-ajax-inner-modal-link='true']").click(function (event) {
        event.preventDefault();

        var methodUrl = $(this).attr("href");

        $.ajax({
            type: "GET",
            url: methodUrl,
            dataType: "html",
            success: function (data) { replacePopupContent(modalId, data); },
            error: function () { window.location.replace("/Home/Error"); }
        });

        setPopupReloadUrl(modalId, methodUrl);

        setLoadingContentForPopup(modalId);
    })

    checkAjaxModalLinksForPopup(modalDialogContainer);

    modalDialogContainer.trigger('modal-content-loaded');
}

function reloadAjaxModalWindow(modalId) {
    var methodUrl = $('div#' + modalId + '.modal-dialog-container').data("ajax-modal-reload-url");

    $.ajax({
        type: "GET",
        url: methodUrl,
        dataType: "html",
        success: function (data) { replacePopupContent(modalId, data); },
        error: function () { window.location.replace("/Home/Error"); }
    });

    setLoadingContentForPopup(modalId);
}

function showModal(modalId, modalTitle, methodUrl, loadingIndicatorPah, modalWindowClasses) {
    var otherDialog = $('.modal-dialog-container');
    if (otherDialog.length > 0)
        addPopup(modalId, modalTitle, loadingIndicatorPah, modalWindowClasses, true);
    else
        addPopup(modalId, modalTitle, loadingIndicatorPah, modalWindowClasses, false);

    setPopupReloadUrl(modalId, methodUrl);

    setLoadingContentForPopup(modalId);

    $.ajax({
        type: "GET",
        url: methodUrl,
        dataType: "html",
        success: function (data) { replacePopupContent(modalId, data); },
        error: function () { window.location.replace("/Home/Error/"); }
    });
}

function showStaticModalWithFooterContent(modalId, modalTitle, modalContent, modalFooterContent, closeButtonText, modalWindowClasses) {
    var otherDialog = $('.modal-dialog-container');
    var isInner = otherDialog.length > 0;

    var modalClasses = 'modal-dialog-container modal fade colored-header ';

    if (modalWindowClasses !== undefined)
        modalClasses += modalWindowClasses;

    var headerCloseModalButton = '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';

    var footerCloseModalButton = '<button class="btn btn-default btn-flat" data-dismiss="modal">' + closeButtonText + '</button>';

    $(document.body).append('<div id="' + modalId + '" '
        + 'class=" ' + modalClasses + '" >'
        + '<div class="modal-dialog">'
        + '<div class="modal-content">'
        + '<div class="modal-header">'
        + '<h3>' + modalTitle + '</h3>' + headerCloseModalButton
        + '</div>'
        + '<div class="modal-body">' + modalContent + '</div>'
        + '<div class="modal-footer">' + modalFooterContent + footerCloseModalButton + '</div>'
        + '</div>'
        + '</div>'
        + '</div>');

    var modalDialogContainer = $('div#' + modalId + '.modal-dialog-container');

    if (isInner) {
        modalDialogContainer.modal({ backdrop: false });

        var margin = 30 + 20 * modalCount;
        modalDialogContainer.find('.modal-content').attr('style', 'margin-top:' + margin + 'px');
    }
    else
        modalDialogContainer.modal();

    modalDialogContainer.on("hidden.bs.modal", function () {
        $(this).trigger('modal-hidden');
        $(this).remove();

        modalCount = modalCount - 1;
    })

    modalCount = modalCount + 1;
}

function showStaticModal(modalId, modalTitle, modalContent, closeButtonText, modalWindowClasses) {
    showStaticModalWithFooterContent(modalId, modalTitle, modalContent, '', closeButtonText, modalWindowClasses);
}

function hideModal(modalId) {
    var modalDialogContainer = $('#' + modalId);

    if (modalDialogContainer.length > 0) {
        modalDialogContainer.modal('hide');
    }
}

function isModalOpen(modalId) {
    return $('#' + modalId).length > 0;
}

$(function () {
    checkAjaxModalLinksForPopup($(document.body));
})