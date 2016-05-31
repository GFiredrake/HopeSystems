$("input[type='radio']").change(function () {

    if ($(this).hasClass("ButtonA") == true) {
        $("#SkuDiv").removeClass("InvisibleDiv");
        $("#SkuDiv").removeClass("HiddenDiv");
        $("#SupplierDiv").addClass("InvisibleDiv");
        $("#SuppliersAvailable").val("");
        $(".ButtonB").prop("checked", false);
    }

    if ($(this).hasClass("ButtonB") == true) {
        $("#SupplierDiv").removeClass("InvisibleDiv");
        $("#SupplierDiv").removeClass("HiddenDiv");
        $("#SkuDiv").addClass("InvisibleDiv");
        $("#SkuInput").val("");
        $(".ButtonA").prop("checked", false);
    }

});
$("#startDate").change(function () {
    $("#endDate").attr({ "min": $("#startDate").val() })
});


function GenerateBuyerGodReport() {
    $('#DisplayTable').empty();

    if($('input[name=Answer]:checked').val() == undefined)
    {
        alert("Please Select either a Sku number or a Supplier name")
    }
    if($('input[name=Answer]:checked').val() == "A")
    {
        GenerateReportBySku()
    }
    if ($('input[name=Answer]:checked').val() == "B")
    {
        GenerateReportBySupplier()
    }

}

function GenerateReportBySku() {
    $("#ErrorDiv").addClass("InvisibleDiv");
    $("#LoadingDiv").removeClass("InvisibleDiv");

    var Start = $('#startDate').val();
    var End = $('#endDate').val();

       $.ajax({
            type: "Get",
            url: "/Reporting/GenerateBuyerGodGetProductFromSku",
            data: { 'Variable': $('#SkuInput').val().toString(), 'Start': Start, 'End': End },
            dataType: "json"
        })
        .done(function (data) {
            $('#NumberLoading').empty();
            $('#CurrentLoading').empty();
            $('#CurrentLoading').append('0');
            $('#NumberLoading').append("/" + data.length);
            var Loading = 1;

            $('#DisplayTable').empty();
            $('#DisplayTable').append("<tr>" +
                                            "<td>SKU</td>" +
                                            "<td>Tv Description</td>" +
                                            "<td>Variation</td>" +
                                            "<td>Qty In Bins</td>" +
                                            "<td>Awaiting Dispatch</td>" +
                                            "<td>Free Qty</td>" +
                                            "<td>Qty Sold</td>" +
                                            "<td>Cost Price</td>" +
                                            "<td>Line Value</td>" +
                                            "<td>ExVat Selling Price</td>" +
                                            "<td>Line Retail Value</td>" +
                                            "<td>DD/SH Exp Date</td>" +
                                            "<td>Stock Age</td>" +
                                            "<td>Supplier</td>" +
                                            "<td>Buyer</td>" +
                                            "<td>Ex Vat Sales</td>" +
                                            "<td>Ex Vat Profit</td>" +
                                            "<td>Ex Vat Sales All Time</td>" +
                                            "<td>Ex Vat Profit All Time</td>" +
                                            "<td>% Margin</td>" +
                                            "<td>£/Min</td>" +
                                            "<td>PPM</td>" +
                                            "<td>% Returns</td>" +
                                            "<td>No.Returns</td>" +
                                        "</tr>");

            $.each(data, function (index, item) {
                GenerateAdditionalLines(item)
                $('#CurrentLoading').empty();
                $('#CurrentLoading').append(Loading);
                Loading++
                if (Loading == data.length) {
                    $("#LoadingDiv").addClass("InvisibleDiv");

                }
            });

        })
        .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
            $("#LoadingDiv").addClass("InvisibleDiv");
        });
}

function GenerateReportBySupplier() {
    $("#ErrorDiv").addClass("InvisibleDiv");
    $("#LoadingDiv").removeClass("InvisibleDiv");

    var Start = $('#startDate').val();
    var End = $('#endDate').val();

    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateBuyerGodGetProductFromSupplier",
        data: { 'Variable': $('#SuppliersAvailable').val().toString(), 'Start': Start, 'End': End },
        dataType: "json"
    })
     .done(function (data) {
         $('#NumberLoading').empty();
         $('#CurrentLoading').empty();
         $('#CurrentLoading').append('0');
         $('#NumberLoading').append("/" + data.length);
         var Loading = 1;

         $('#DisplayTable').empty();
         $('#DisplayTable').append("<tr>" +
                                         "<td>SKU</td>" +
                                         "<td>Tv Description</td>" +
                                         "<td>Variation</td>" +
                                         "<td>Qty In Bins</td>" +
                                         "<td>Awaiting Dispatch</td>" +
                                         "<td>Free Qty</td>" +
                                         "<td>Qty Sold</td>" +
                                         "<td>Cost Price</td>" +
                                         "<td>Line Value</td>" +
                                         "<td>ExVat Selling Price</td>" +
                                         "<td>Line Retail Value</td>" +
                                         "<td>DD/SH Exp Date</td>" +
                                         "<td>Stock Age</td>" +
                                         "<td>Supplier</td>" +
                                         "<td>Buyer</td>" +
                                         "<td>Ex Vat Sales</td>" +
                                         "<td>Ex Vat Profit</td>" +
                                         "<td>Ex Vat Sales All Time</td>" +
                                         "<td>Ex Vat Profit All Time</td>" +
                                         "<td>% Margin</td>" +
                                         "<td>£/Min</td>" +
                                         "<td>PPM</td>" +
                                         "<td>% Returns</td>" +
                                         "<td>No.Returns</td>" +
                                     "</tr>");

         $.each(data, function (index, item) {
             GenerateAdditionalLines(item)
             $('#CurrentLoading').empty();
             $('#CurrentLoading').append(Loading);
             Loading++
             if (Loading == data.length) {
                 $("#LoadingDiv").addClass("InvisibleDiv");

             }
         });

     })
     .fail(function (xhr) {
         alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
         $("#LoadingDiv").addClass("InvisibleDiv");
     });
}

function GenerateAdditionalLines(item) {
    var content = "<tr>"
    for (key in item) {

        var value = item[key];
        if (value == '.') {
            value = ''
        }
        if (value == '<span class="bold red">£</span> (<span class="bold green">£</span>)') {
            value = "No Orders"
        }
        content += '<td>' + value + '</td>';

    }

    content += "</tr>"
    $('#DisplayTable').append(content);
}