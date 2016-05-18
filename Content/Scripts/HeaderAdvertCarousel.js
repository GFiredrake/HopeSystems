$(document).ready(function () {
    $.ajax({
        type: "Get",
        url: "/Reporting/RetrieveHeaderAdvertCarouselData",
        data: {},
        dataType: "json"
    })
        .done(function (data) {
         
        })

        .fail(function (jqXHR, textStatus, errorThrown) {

        });
});