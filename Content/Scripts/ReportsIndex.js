$(document).ready(function () {
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateSkuStockReportReport",
        data: {},
        dataType: "json"
    })
        .done(function (data) {
            var pause = 1;

            $('#DisplayTable').append("<tr>" +
                                            "<td>Sku</td>" +
                                            "<td>Description</td>" +
                                            "<td>Variation Name</td>" +
                                            "<td>Free Qty</td>" +
                                            "<td>Supplier Name</td>" +
                                            "<td>Buyer Name</td>" +
                                       "</tr>");

            $.each(data, function (index, item) {
                GenerateAdditionalLines(item)
            });
            $('#TableArea').removeClass('HiddenDiv');
        })

        .fail(function (jqXHR, textStatus, errorThrown) {

        });
});