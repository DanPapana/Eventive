function followEvent(id) {
    $.ajax({
        url: "Event/Follow?id=" + id,
        success: function () {
            $('#follow-' + id).toggle();
            $('#following-' + id).toggle();
        }
    });
}

function applyToEvent(id) {
    $('#apply-' + id).toggle();
    $('#applied-' + id).toggle();
    if ($('follow-' + id).is(":visible")) {
        $('#follow-' + id).hide();
        $('#following-' + id).show();
    }
}

$(document).ready(function () {
    $("#searchTitleBar").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#toSearch .card").filter(function () {
            $(this).toggle($(this).find('.title').text().toLowerCase().indexOf(value) > -1)
        });
    });
});

$(document).ready(function () {
    $("#searchLocationBar").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#toSearch .card").filter(function () {
            $(this).toggle($(this).find('.location').text().toLowerCase().indexOf(value) > -1)
        });
    });
});

