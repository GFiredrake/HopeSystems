$(document).ready(function () {
    $('#CurrencyType').val("1");

    if (getUrlVars()["Date"].length == 10) {
        var pause = 1;
        var symbol = "£";
        if (getUrlVars()["Symbol"].length != null) {
            var symbol = decodeURI(getUrlVars()["Symbol"]);
        }
        GenerateTheSalesOverviewReportDaily(getUrlVars()["Date"], getUrlVars()["Currency"], symbol)
    }
});

function GenerateSalesOverviewReport() {
    $("#itemsSoldTitle").removeClass("Hidden");
    $("#LoadingDiv").removeClass("InvisibleDiv");
    $('#DisplayTable1').empty();
    $('#DisplayTable2').empty();
    var Days = $("#DayAmmount").val();
    var Currency = $("#CurrencyType").val();

    if (Currency == 1 || Currency == 2) {
        SingleCurrencyReport(Days, Currency)
    }
    ItemsSoldTotay(Currency);
}

function ItemsSoldTotay(Currency) {
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateItemsSoldToday",
        data: { 'Currency': Currency },
        dataType: "json"
    })
      .done(function (data) {
          var list = data

          var symbol = $("#CurrencyType").find(":selected").text().split('(')[1].charAt(0)

          $('#DisplayTable2').append("<tr>" +
                                          "<td>SKU</td>" +
                                          "<td>Description</td>" +
                                          "<td>Free Quantity</td>" +
                                          "<td>Quantity Sold</td>" +
                                          "<td>IncVAT Turnover (" + symbol + ")</td>" +
                                      "</tr>");
          $.each(list, function (index, item) {
              GenerateAdditionalLinesSales(item, 2)
          });



      })
      .fail(function (xhr) {
          alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
          $("#LoadingDiv").addClass("InvisibleDiv");
      });
}

function SingleCurrencyReport(Days, Currency) {
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateSalesOverview",
        data: { 'Days': Days, 'Currency': Currency },
        dataType: "json"
    })
        .done(function (data) {
            var list = data
            if (getUrlVars()["Currency"] != null) {
                $("#CurrencyType").val(getUrlVars()["Currency"]);
            }
            var symbol = $("#CurrencyType").find(":selected").text().split('(')[1].charAt(0)
            
            $('#DisplayTable1').append("<tr>" +
                                            "<td>Date</td>" +
                                            "<td>IncVAT Turnover (" + symbol + ")</td>" +
                                            "<td>ExVAT Turnover (" + symbol + ") Products</td>" +
                                            "<td>ExVAT Margin (" + symbol + ") Products</td>" +
                                            "<td>ExVAT Turnover (" + symbol + ") Products (Billed)</td>" +
                                            "<td>ExVAT Margin (" + symbol + ") Products (Billed)</td>" +
                                            "<td>ExVAT Turnover (" + symbol + ") P&P</td>" +
                                            "<td>ExVAT Margin (" + symbol + ") P&P</td>" +
                                            "<td>New Freedom Members</td>" +
                                            "<td>Freedom Renewals</td>" +
                                            "<td>New Customers</td>" +
                                            "<td>Existing Customers</td>" +
                                            "<td># Product Orders</td>" +
                                        "</tr>");
            $.each(list, function (index, item) {
                GenerateAdditionalLinesSales(item, 1)
            });

            revert()
            
        })
        .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
            $("#LoadingDiv").addClass("InvisibleDiv");
        });
}

function GenerateAdditionalLinesSales(item, tableNumber) {
    var content = "<tr>"
    for (key in item) {
        //remove table number to make work
        if (key == "Date" && tableNumber == 1 && tableNumber == 99) {
            var value = item[key];
            content += '<td><a href="SalesOverviewReportDaily?Date=' + value + '&Currency=' + $("#CurrencyType").val() + '&Days=' + $("#DayAmmount").val() + '&Symbol=' + $("#CurrencyType").find(":selected").text().split('(')[1].charAt(0) + '"><span>' + value + '</span></a></td>';
        }
        else if(key == "Time" && tableNumber == 1) {
            var value = item[key];
            //content += '<td><a href="SalesOverviewReportHourley?Time=' + value + '"><span>' + value + '</span></a></td>';
            //content += '<td><span onclick="DisplaySalesModal(\'' + value.toString() + '\')">' + value + '</span></td>';
            content += '<td><button onclick="DisplaySalesModal(\'' + value.toString() + '\')">' + value + '</button></td>';
        }
        else {
            var value = item[key];
 
            content += '<td>' + value + '</td>';
        }
        

    }

    content += "</tr>"
    $('#DisplayTable' + tableNumber).append(content);
}

function revert() {
    $("#LoadingDiv").addClass("InvisibleDiv");
    $("#GenerateButton").removeClass("Hidden");
    $("#GenerateButtonAll").removeClass("Hidden");
}

function GenerateTheSalesOverviewReportDaily(Date, Currency, symbol) {
    $('#DisplayTable1').empty();
    $("#LoadingDiv").removeClass("InvisibleDiv");
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateSalesOverviewReportDaily",
        data: { 'Date': Date, 'Currency': Currency },
        dataType: "json"
        })
        .done(function (data) {

            $('#DisplayTable1').append("<tr>" +
                                           "<td>Time</td>" +
                                           "<td>IncVAT Turnover (" + symbol + ")</td>" +
                                           "<td>ExVAT Turnover (" + symbol + ") Products</td>" +
                                           "<td>ExVAT Margin (" + symbol + ") Products</td>" +
                                           "<td>ExVAT Turnover (" + symbol + ") Products (Billed)</td>" +
                                           "<td>ExVAT Margin (" + symbol + ") Products (Billed)</td>" +
                                           "<td>ExVAT Turnover (" + symbol + ") P&P</td>" +
                                           "<td>ExVAT Margin (" + symbol + ") P&P</td>" +
                                           "<td>New Freedom Members</td>" +
                                           "<td>Freedom Renewals</td>" +
                                           "<td>New Customers</td>" +
                                           "<td>Existing Customers</td>" +
                                           "<td># Product Orders</td>" +
                                       "</tr>");
            $.each(data, function (index, item) {
                GenerateAdditionalLinesSales(item, 1)
            });
            $("#LoadingDiv").addClass("InvisibleDiv");
        })
        .fail(function (xhr) {
            alert("Error in generating daily report");
        });
}

function DisplaySalesModal(time) {
    $('#DisplayTable3').empty();
    $('#myAnchor')[0].click();
    $("#LoadingDiv2").removeClass("InvisibleDiv");
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateSalesOverviewReportHourly",
        data: { 'Time': time, 'Currency': getUrlVars()["Currency"] },
        dataType: "json"
    })
        .done(function (data) {
            var symbol = decodeURI(getUrlVars()["Symbol"].split('#')[0]);
            $('#DisplayTable3').append("<tr>" +
                                          "<td>SKU</td>" +
                                          "<td>Description</td>" +
                                          "<td>Quantity</td>" +
                                          "<td>IncVAT Turnover (" + symbol + ")</td>" +
                                      "</tr>");
            $.each(data, function (index, item) {
                GenerateAdditionalLinesSales(item, 3)
            });
            $("#LoadingDiv2").addClass("InvisibleDiv");
        })

        .fail(function (xhr) {
            alert("Error in generating daily report");
        });
}

function CloseModal() {
    $('#DisplayTable3').empty();
}