function GenerateFlexiPayReportReport() {
    if ($('#YearsAvailable').val() != 0 && $("#MonthsAvailable").val() != 0) {
        $("#ErrorDiv").addClass("InvisibleDiv");

        RequestReportData();

    }
    else {
        $('#ErrorDiv').removeClass("InvisibleDiv");
    }
}

function RequestReportData() {
    $("#LoadingDiv").removeClass("InvisibleDiv");
    $('#DisplayTable').empty();

    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateFlexiBuyReport",
        data: { 'Year': $('#YearsAvailable').val(), 'Month': $("#MonthsAvailable").val() },
        dataType: "json"
    })
        .done(function (data) {
            var list = data;
            $("#LoadingDiv").addClass("InvisibleDiv");

            $('#DisplayTable').append("<tr>" +
                                            "<td>OrderDate</td>" +
                                            "<td>OrderID</td>" +
                                            "<td>CustomerID</td>" +
                                            "<td>CustomerName</td>" +
                                            "<td>ItemId</td>" +
                                            "<td>TotalFlexiValue</td>" +
                                            "<td>TotalExVATValue</td>" +
                                            "<td>TotalVAT</td>" +
                                            "<td>TotalPaidToDate</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').text().substring(0, 3) + "</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').next().text().substring(0, 3) + "</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').next().next().text().substring(0, 3) + "</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').next().next().next().text().substring(0, 3) + "</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').next().next().next().next().text().substring(0, 3) + "</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').next().next().next().next().next().text().substring(0, 3) + "</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').next().next().next().next().next().next().text().substring(0, 3) + "</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').next().next().next().next().next().next().next().text().substring(0, 3) + "</td>" +
                                            "<td>" + $('#MonthsAvailable option:selected').next().next().next().next().next().next().next().next().text().substring(0, 3) + "</td>" +
                                            "<td>AmountComp</td>" +
                                            "<td>TermComp</td>" +
                                        "</tr>");
            $.each(list, function (index, item) {
                GenerateAdditionalLines(item)
            });

        })
        .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seek technical support");
            $("#LoadingDiv").addClass("InvisibleDiv");
        });
}

function GenerateAdditionalLines(item) {
    var content = "<tr>"
    for (key in item) {

        var value = item[key];

        if (key == "currency" || key == "idx" || key == "orderdate" || key == "orderid" || key == "customerid" || key == "customername" || key == "itemid" || key == "amountcomp" || key == "tempcomp") {
            content += '<td>' + value + '</td>';
        }
        else {
            content += '<td class="' + value.split('*')[1] + '">' + value.split('*')[0] + '</td>';
        }
    }
    content += "</tr>"
    $('#DisplayTable').append(content);
}

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