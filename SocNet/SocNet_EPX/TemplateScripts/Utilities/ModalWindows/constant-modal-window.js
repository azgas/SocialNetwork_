function showProcessingModal() {
    addProcessingModal();
}

function hideProcessingModal() {
    var modalDialogContainer = $('div#processing-modal.modal-dialog-container');

    if (modalDialogContainer.length > 0) {
        modalDialogContainer.removeClass('constant-modal');

        modalDialogContainer.modal('hide');
    }
}

function addProcessingModal() {
    var constantModal = $('.constant-modal');

    if (constantModal.length == 0) {
        var loadingIndicatorPath = $('#loading-indicator-path').data('url');

        addConstantModal('processing-modal', 'Processing',
            '<img src="' + loadingIndicatorPath + '" />');
    }
}

function addConstantModal(modalId, title, content) {
    var modalClasses = 'modal-dialog-container modal fade colored-header constant-modal';

    var otherDialog = $('.modal-dialog-container');

    $(document.body).append('<div id="' + modalId + '" '
        + 'class=" ' + modalClasses + '" '
        + '" >'
        + '<div class="modal-dialog">'
        + '<div class="modal-content">'
        + '<div class="modal-header">'
        + '<h4>' + title + '<h4>'
        + '</div>'
        + '<div class="modal-body">'
        + '<div class="text-center">'
        + content
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>');

    var modalDialogContainer = $('div#' + modalId + '.modal-dialog-container');

    if (otherDialog.length > 0)
        modalDialogContainer.modal({ backdrop: false });
    else
        modalDialogContainer.modal();

    modalDialogContainer.on("hidden.bs.modal", function () {
        $(this).trigger('modal-hidden');
        $(this).remove();
    })
}