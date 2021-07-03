function loadServerPartialView(container, baseUrl) {
    displayBusyIndicator();
    return $.get(baseUrl, function (responseData) {
        $(container).html(responseData);
        hideBusyIndicator();
    });
}

function displayBusyIndicator() {
    $('.loading').show();
}

function hideBusyIndicator() {
    $('.loading').hide();
}