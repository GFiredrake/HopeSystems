$(document).ready(function () {
    $('#CurrencyType').val("1");
    var today = new Date();
    var maxdate = today.getFullYear() + "-" + (today.getMonth()+1) + "-" + today.getDate();
    $("#endDate").attr({ "max": maxdate })
    $("#startDate").attr({ "max": maxdate })

    if (getUrlVars()["quickreport"] == "yes")
    {
        var Days = 1;
        if (getUrlVars()["Days"] != null)
        {
            var Days = getUrlVars()["Days"];
            $("#DayAmmount").val(getUrlVars()["Days"]);
        }
        var Currency = 1;
        if (getUrlVars()["Currency"] != null) {
            var Currency = getUrlVars()["Currency"];
            $("#CurrencyType").val(getUrlVars()["Currency"]);
        }

        $("#itemsSoldTitle").removeClass("Hidden");
        $("#LoadingDiv").removeClass("InvisibleDiv");
        $('#DisplayTable1').empty();
        $('#DisplayTable2').empty();

        

        if (Currency == 1 || Currency == 2) {
            SingleCurrencyReport(Days, Currency)
        }
        ItemsSoldTotay(Currency);
    }
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


//pnpreport start
function GeneratePnPReport() {
    $.ajax({
        type: "Get",
        url: "/Reporting/GetPnpRecords",
        data: { 'NumberOfPeople': $("#HowManyPeople").val(), 'NumberOfMonths': $("#HowManyMonths").val() },
        dataType: "json"
    })
    .done(function (data) {
        $('#DisplayTable').empty();
        $('#CustomersToMakeSavingDiv').empty();
        $("#CustomersToMakeSavingDiv").append(data.length + " Customers could make a saving")
        if ($("#HowManyMonths").val() == 1)
        {
            
            $('#DisplayTable').append("<tr>" +
                                        "<td>Name</td>" +
                                        "<td>Email</td>" +
                                        "<td>Customer Id</td>" +
                                        "<td>Number of Orders</td>" +
                                        "<td>Last 30 Days</td>" +
                                        "<td>Total Saving</td>" +
                                    "</tr>");
        }
        if ($("#HowManyMonths").val() == 3) {
            $('#DisplayTable').append("<tr>" +
                                        "<td>Name</td>" +
                                        "<td>Email</td>" +
                                        "<td>Customer Id</td>" +
                                        "<td>Number of Orders</td>" +
                                        "<td>Last 60-90 Days</td>" +
                                        "<td>Last 30-60 Days</td>" +
                                        "<td>Last 30 Days</td>" +
                                        "<td>Total Saving</td>" +
                                    "</tr>");
        }

        
        $.each(data, function (index, item) {
            GenerateAdditionalLines(item)
        });
    })
    .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support.");
    });
}
//pnpreport end

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

//Orders by IP - start
function GenerateOrdersByIpReport(){
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateOrdersByIpReport",
        dataType: "json"
    })
        .done(function (data) {
            

        })
        .fail(function (xhr) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr);
            $("#LoadingDiv").addClass("InvisibleDiv");
        });
}
//Orders by IP - End


//PromotionalFreedomPnPSavings - Start
function GeneratePromotionalFreedomPnPSavings() {
    $('#LoadingDiv').removeClass("InvisibleDiv");
    $.ajax({
        type: "Get",
        url: "/Reporting/GeneratePromotionalFreedomPnPSavingsReport",
        data: { 'StartDate': $("#startDate").val(), 'EndDate': $("#endDate").val(), 'CurrencyId': $("#CurrencyType").val() },
        dataType: "json"
    })
    .done(function (data) {
        $('#DisplayTable').empty();
        $('#DisplayTable').append("<tr>" +
                                         "<td>Customer Id</td>" +
                                         "<td>Saving</td>" +
                                     "</tr>");
        $.each(data, function (index, item) {
            GenerateAdditionalLines(item)
        });
        $('#LoadingDiv').addClass("InvisibleDiv");
    })
    .fail(function (xhr) {
        
    });

}
//PromotionalFreedomPnPSavings - End

//SalesDataReport - Start
//function GenerateSalesDataReport() {

//    //break down into days 
//    var date1 = new Date($("#startDate").val().split('-')[0] + "/" + $("#startDate").val().split('-')[1] + "/" + $("#startDate").val().split('-')[2]);
//    var date2 = new Date($("#endDate").val().split('-')[0] + "/" + $("#endDate").val().split('-')[1] + "/" + $("#endDate").val().split('-')[2]);
//    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
//    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

//    var datesTorequest = [];

//    if (diffDays > 0) {
//        var counter = 0;
//        var days = parseInt($("#endDate").val().split('-')[2]);
//        var month = parseInt($("#endDate").val().split('-')[1]);
//        var year = parseInt($("#endDate").val().split('-')[0]);
//        datesTorequest.push(year.toString() + "-" + month.toString() + "-" + days.toString())

//        while (counter < diffDays) {
//            if (days > 1) {
//                days = days - 1;
//            }
//            else {
//                month = month - 1;
//                if (month == 1) {
//                    days = 31;
//                    year = year - 1;
//                }
//                if (month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) {
//                    days = 31;
//                }
//                if (month == 4 || month == 6 || month == 9 || month == 11) {
//                    days = 30;
//                }
//                if (month == 2)
//                {
//                    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0) == true) {
//                        days = 29;
//                    }
//                    else {
//                        days = 28;
//                    }
//                }
//            }
//            datesTorequest.push(year.toString() + "-" + month.toString() + "-" + days.toString())
//            counter ++;
//        }
//    }

    
//    $('#DisplayTable').empty();
//    $('#DisplayTable').append("<tr>" +
//                                        "<td>customerid</td>" +
//                                        "<td>title</td>" +
//                                        "<td>firstname</td>" +
//                                        "<td>lastname</td>" +
//                                        "<td>phonenumber1</td>" +
//                                        "<td>emailaddress</td>" +
//                                        "<td>Addressline1</td>" +
//                                        "<td>Addressline2</td>" +
//                                        "<td>Town</td>" +
//                                        "<td>County</td>" +
//                                        "<td>Postcode</td>" +
//                                        "<td>IsFreedomMember</td>" +
//                                        "<td>ReceiveEmail</td>" +
//                                        "<td>ReceivePost</td>" +
//                                        "<td>ReceiveSms</td>" +
//                                        "<td>OrderMethod</td>" +
//                                        "<td>Source</td>" +
//                                        "<td>Sku</td>" +
//                                        "<td>VariationSku</td>" +
//                                        "<td>ItemName</td>" +
//                                        "<td>quantity</td>" +
//                                        "<td>ItemLineValue</td>" +
//                                        "<td>Catagory1</td>" +
//                                        "<td>Catagory2</td>" +
//                                        "<td>Catagory3</td>" +
//                                        "<td>Catagory4</td>" +
//                                        "<td>Catagory5</td>" +
//                                        "<td>Brand</td>" +
//                                 "</tr>");
//    //call for each day
//    var numberOfCalls = datesTorequest.length;
//    var seacondCount = 0
//    while (seacondCount < numberOfCalls) {
//        var date = datesTorequest[seacondCount]
//        $('#LoadingDiv').removeClass("InvisibleDiv");
//        $.ajax({
//            type: "Get",
//            url: "/Reporting/GenerateSalesDataReportByDate",
//            data: { 'StartDate': date, 'EndDate': date },
//            dataType: "json"
//        })
//        .done(function (data) {

//            $.each(data, function (index, item) {
//                GenerateAdditionalLines(item)
//            });
//            $('#LoadingDiv').addClass("InvisibleDiv");
//        })
//        .fail(function (xhr) {
//            alert("Error in record numbers");
//        });
//        seacondCount++;
//    }
//}


function alternativeGeneration() {
    var numberOfReacordsToReturn = 0;
    $('#LoadingDiv').removeClass("InvisibleDiv");
    $.ajax({
        type: "Get",
        url: "/Reporting/GetRecordCount",
        data: { 'StartDate': $("#startDate").val(), 'EndDate': $("#endDate").val() },
        dataType: "json"
    })
        .done(function (data) {
            numberOfReacordsToReturn = data;

            var numberOfReacordsLeftToReturn = numberOfReacordsToReturn;

            var numberOfReacordsAlreadyReturned = 0;

            while (numberOfReacordsLeftToReturn > 0) {
                var numberOfReacordsToReturnInOneTime = 0;

                if (numberOfReacordsLeftToReturn > 2500) {
                    numberOfReacordsToReturnInOneTime = 2500;
                }
                else {
                    numberOfReacordsToReturnInOneTime = numberOfReacordsLeftToReturn;
                }
                $.ajax({
                    type: "Get",
                    url: "/Reporting/GenerateSalesDataReportByDate2",
                    data: { 'StartDate': $("#startDate").val(), 'EndDate': $("#endDate").val(), 'NumberOfReacordsToGet': numberOfReacordsToReturnInOneTime, 'NumberOfRecordsDone': numberOfReacordsAlreadyReturned },
                    dataType: "json"
                })
                .done(function (data2) {
                    
                    $.each(data2, function (index, item) {
                        GenerateAdditionalLines(item)
                    });
                    $('#LoadingDiv').addClass("InvisibleDiv");
                })
                .fail(function (xhr) {
                    alert("Error in record numbers");
                });
                numberOfReacordsAlreadyReturned = numberOfReacordsAlreadyReturned + numberOfReacordsToReturnInOneTime
                numberOfReacordsLeftToReturn = numberOfReacordsLeftToReturn - numberOfReacordsToReturnInOneTime
            }

            $('#DisplayTable').empty();
            $('#DisplayTable').append("<tr>" +
                                                "<td>Customer Id</td>" +
                                                "<td>Title</td>" +
                                                "<td>First Name</td>" +
                                                "<td>Last Name</td>" +
                                                "<td>Phone Number</td>" +
                                                "<td>E-Mail Address</td>" +
                                                "<td>Address Line 1</td>" +
                                                "<td>Address Line 2</td>" +
                                                "<td>Town</td>" +
                                                "<td>County</td>" +
                                                "<td>Postcode</td>" +
                                                "<td>Is Freedom Member</td>" +
                                                "<td>Receive Email</td>" +
                                                "<td>Receive Post</td>" +
                                                "<td>Receive Sms</td>" +
                                                "<td>Order Method</td>" +
                                                "<td>Source</td>" +
                                                "<td>Order Date</td>" +
                                                "<td>Sku</td>" +
                                                "<td>Variation Sku</td>" +
                                                "<td>Item Name</td>" +
                                                "<td>Quantity</td>" +
                                                "<td>Item Line Value</td>" +
                                                "<td>Catagory 1</td>" +
                                                "<td>Catagory 2</td>" +
                                                "<td>Catagory 3</td>" +
                                                "<td>Catagory 4</td>" +
                                                "<td>Catagory 5</td>" +
                                                "<td>Brand</td>" +
                                         "</tr>");
        })
        .fail(function (xhr) {
            alert("Error in record numbers retrival");
        });
}

//SalesDataReport - End


//funky test area - start
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}
//funky test area - end


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

function tableToExcel2() {
    $("#DisplayTable").table2excel({
    // exclude CSS class
    exclude: ".noExl",
    name: "Sales Data",
    filename: "ItemsOrderedBetweenDatesReport" //do not include extension
});

}