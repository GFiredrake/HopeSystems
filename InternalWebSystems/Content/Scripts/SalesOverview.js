$(document).ready(function () {
    //$('#CurrencyType>option:eq(1)').prop('selected', true);
    $('#CurrencyType').val("1");
});

function GenerateSalesOverviewReport() {
    $("#LoadingDiv").removeClass("InvisibleDiv");
    $('#DisplayTable1').empty();
    $('#DisplayTable2').empty();
    var Days = $("#DayAmmount").val();
    var Currency = $("#CurrencyType").val()

    if (Currency == 1 || Currency == 2) {
        SingleCurrencyReport(Days, Currency)
    }
    ItemsSoldTotay(Currency)
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
                                          "<td>Quantity</td>" +
                                          "<td>IncVAT Turnover (" + symbol + ")</td>" +
                                      "</tr>");
          $.each(list, function (index, item) {
              GenerateAdditionalLines(item, 2)
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

            var symbol = $("#CurrencyType").find(":selected").text().split('(')[1].charAt(0)
            
            $('#DisplayTable1').append("<tr>" +
                                            "<td>LineRetailValue</td>" +
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
                                            "<td># Orders</td>" +
                                        "</tr>");
            $.each(list, function (index, item) {
                GenerateAdditionalLines(item, 1)
            });

            revert()
            
        })
        .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
            $("#LoadingDiv").addClass("InvisibleDiv");
        });
}

function GenerateAdditionalLines(item, tableNumber) {
    var content = "<tr>"
    for (key in item) {

        var value = item[key];

        content += '<td>' + value + '</td>';

    }

    content += "</tr>"
    $('#DisplayTable' + tableNumber).append(content);
}

function revert() {
    $("#LoadingDiv").addClass("InvisibleDiv");
    $("#GenerateButton").removeClass("Hidden");
    $("#GenerateButtonAll").removeClass("Hidden");
}
