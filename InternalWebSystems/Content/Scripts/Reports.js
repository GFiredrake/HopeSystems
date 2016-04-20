$(document).ready(function () {
    var today = new Date();
    var maxdate = today.getFullYear() + "-" + (today.getMonth()+1) + "-" + today.getDate();
    $("#endDate").attr({ "max": maxdate })
    $("#startDate").attr({ "max": maxdate })
});

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


//Buyer God Report - START
function GenerateBuyerGodReport() {
    $('#DisplayTable').empty();
    var isSku;
    var variableToSend;
    if ($("#SkuInput").val() != "") {
        if ($("#SkuInput").val().length == 6)
        {
            isSku = 1;
            variableToSend = $('#SkuInput').val();
            $("#SkuErrorDiv").addClass("InvisibleDiv");
        }
        else {
            $("#SkuErrorDiv").removeClass("InvisibleDiv");
            $("#ErrorDiv").addClass("InvisibleDiv");
        }
    }
    else {
        if ($('#SuppliersAvailable').val() != "0") {
            isSku = 2;
            variableToSend = $('#SuppliersAvailable').val();
        }
    }
    if (isSku != undefined && $('#startDate').val() != "" && $('#endDate').val() != "") {
        $("#ErrorDiv").addClass("InvisibleDiv");
        $("#LoadingDiv").removeClass("InvisibleDiv");
        
        var Start = $('#startDate').val();
        var End = $('#endDate').val();
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
        RequestAndGenerate(variableToSend, isSku, Start, End, 0);
    }
    else {
        $("#ErrorDiv").removeClass("InvisibleDiv");
    }

    
};

function GenerateAdditionalLines(item) {
    var content = "<tr>"
    for(key in item) {

        var value = item[key];
        if (value == '.') {
            value = ''
        }
            content += '<td>' + value + '</td>';
        
    }

    content += "</tr>"
    $('#DisplayTable').append(content);
}

function RequestAndGenerate(variableToSend, isSku, Start, End, IsFull) {
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateBuyerGodGetSkus",
        data: { 'Variable': variableToSend, 'IsSku': isSku, 'Start': Start, 'End': End },
        dataType: "json"
        })
        .done(function (data) {
            var list = data;
            if (IsFull == 1) {
                GenerateItemsTwo(list, Start, End);
            }
            else {
                GenerateItems(list, Start, End);
            }
            
        })
        .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
            $("#LoadingDiv").addClass("InvisibleDiv");
        });
    
}

function GenerateItems(list, Start, End) {

    $('#NumberLoading').empty();
    $('#CurrentLoading').empty();
    $('#CurrentLoading').append('0');
    $('#NumberLoading').append("/" + list.length);
    var whentorevert = list.length;
    var Loading = 1;
    $.each(list, function (index, item) {
        $.ajax({
            type: "Get",
            url: "/Reporting/GenerateBuyerGodGetProductFromSku",
            data: { 'Variable': item, 'Start': Start, 'End': End },
            dataType: "json"
        })
        .done(function (data) {
            var list2 = data;
            
            $.each(list2, function (index, item) {
                GenerateAdditionalLines(item)
                whentorevert--
                $('#CurrentLoading').empty();
                $('#CurrentLoading').append(Loading);
                Loading ++
                if (whentorevert == 0)
                { revert(); }
            });
            
        })
        .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
            $("#LoadingDiv").addClass("InvisibleDiv");
        });
    });
}

function revert() {
    $("#LoadingDiv").addClass("InvisibleDiv");
    $("#GenerateButton").removeClass("Hidden");
    $("#GenerateButtonAll").removeClass("Hidden");
}






function RequestAndGenerateFullreport() {
    $("#LoadingDiv").removeClass("InvisibleDiv");
    var Start = $('#startDate').val();
    var End = $('#endDate').val();

    $("#GenerateButton").addClass("Hidden");
    $("#GenerateButtonAll").addClass("Hidden");

    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateBuyerGodFullReportSuplierList",
        dataType: "json"
    })
        .done(function (data) {
            var list = data;

           
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
            $.each(list, function (index, item) {
                RequestAndGenerate(item, 2, Start, End, 1)
            });

        })
        .fail(function (xhr) {
            alert("(30338) an error has occured, Please try again. if this problem persists please seak technical support");
            $("#LoadingDiv").addClass("InvisibleDiv");
        });

    revert();
}

function GenerateItemsTwo(list, Start, End, IsFull) {

    $.each(list, function (index, item) {
        $.ajax({
            type: "Get",
            url: "/Reporting/GenerateBuyerGodGetProductFromSku",
            data: { 'Variable': item, 'Start': Start, 'End': End },
            dataType: "json"
        })
        .done(function (data) {
            var list2 = data;
            
            $.each(list2, function (index, item) {
                GenerateAdditionalLines(item)
            });
            
        })
        .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
            $("#LoadingDiv").addClass("InvisibleDiv");
        });
    });

}








//Buyer God Report - END

var tableToExcel = (function () {
    var uri = 'data:application/vnd.ms-excel;base64,'
    , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
    , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
    , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    return function (table, name, filename) {
        if (!table.nodeType) table = document.getElementById(table)
        //removes the £ from the currency
        var tableData = table.innerHTML.replace(/£/g, "");
        var ctx = { worksheet: name || 'Worksheet', table: tableData }

        var date = new Date();
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var formatedDatewithTime = (date.getDate() + ' ' + monthNames[date.getMonth()] + '  ' + date.getFullYear()) + ' ' + date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();

        var fileName = name.replace(/ /g, "") + formatedDatewithTime.replace(/ /g, "");

        document.getElementById("dlink").href = uri + base64(format(template, ctx));
        document.getElementById("dlink").download = fileName + filename;
        document.getElementById("dlink").click();
    }
})()

