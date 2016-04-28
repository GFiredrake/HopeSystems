window.onload = function () {
    var ColourList =    ['#ED137D', '#314b9f', '#872990', '#25AAE1', '#8DC63F', '#CE3234'];
    var HighLightList = ['#f259a4', '#6e81bb', '#ab69b1', '#7CCCED', '#BADC8B', '#E18485'];
    var RGBColourList = ["rgba(237,19,125,0.5)", "rgba(49,75,159,0.5)", "rgba(135,41,144,0.5)"];

    //SimplePieChartDataList
    $.ajax({
        type: "Get",
        url: "/Overview/GetPieChartData",
        data: { },
        dataType: "json"
    })
    .done(function (data) {
        var pause = 1;
        //Web orders Vs Phone Orders
        var total1 = parseInt(data[0]) + parseInt(data[1]); 
        var WebVsPhoneDynamicData = [];
        WebVsPhoneDynamicData[0] = { value: data[0], color: ColourList[0], highlight: HighLightList[0], label: "Web Orders - " + Math.round(((parseInt(data[0]) / total1) * 100) * 10) / 10 + '%' }
        WebVsPhoneDynamicData[1] = { value: data[1], color: ColourList[1], highlight: HighLightList[1], label: "Phone Orders - " + Math.round(((parseInt(data[1]) / total1) * 100) * 10) / 10 + '%' }

        var WebVsPhoneOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false
        }

        var WebVsPhonePie = document.getElementById("PieWebVsPhone").getContext("2d");
        var WebVsPhoneDynamicPieNew = new Chart(WebVsPhonePie).Pie(WebVsPhoneDynamicData, WebVsPhoneOptions);
        document.getElementById('PieWebVsPhoneLegend').innerHTML = WebVsPhoneDynamicPieNew.generateLegend();
        //Customers Normal Vs Freedom
        var total2 = parseInt(data[4]) + parseInt(data[5]);
        var CustomerNormalVsFreedomDynamicData = [];
        CustomerNormalVsFreedomDynamicData[0] = { value: data[4], color: ColourList[1], highlight: HighLightList[0], label: "Freedom Customers - " + Math.round(((parseInt(data[4]) / total2) * 100) * 10) / 10 + '%' }
        CustomerNormalVsFreedomDynamicData[1] = { value: data[5], color: ColourList[0], highlight: HighLightList[1], label: "Non Freedom Customer - " + Math.round(((parseInt(data[5]) / total2) * 100) * 10) / 10 + '%' }

        var CustomerNormalVsFreedomOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false,
        }

        var CustomerNormalVsFreedomPie = document.getElementById("PieCustomerNormalVsFreedom").getContext("2d");
        var CustomerNormalVsFreedomDynamicPieNew = new Chart(CustomerNormalVsFreedomPie).Pie(CustomerNormalVsFreedomDynamicData, CustomerNormalVsFreedomOptions);
        document.getElementById('PieCustomerNormalVsFreedomLegend').innerHTML = CustomerNormalVsFreedomDynamicPieNew.generateLegend();
        //Need Data - Customers Existing Vs New
        var total3 = parseInt(data[2]) + parseInt(data[3]);
        var CustomersNewVsExistingDynamicData = [];
        CustomersNewVsExistingDynamicData[0] = { value: data[3], color: ColourList[1], highlight: HighLightList[1], label: "Existing - " + Math.round(((parseInt(data[3]) / total3) * 100) * 10) / 10 + '%' }
        CustomersNewVsExistingDynamicData[1] = { value: data[2], color: ColourList[0], highlight: HighLightList[0], label: "New - " + Math.round(((parseInt(data[2]) / total3) * 100) * 10) / 10 + '%' }

        var CustomersNewVsExistingOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false,
        }

        var CustomersNewVsExistingPie = document.getElementById("PieCustomersNewVsExisting").getContext("2d");
        var CustomersNewVsExistingDynamicPieNew = new Chart(CustomersNewVsExistingPie).Pie(CustomersNewVsExistingDynamicData, CustomersNewVsExistingOptions);
        document.getElementById('PieCustomersNewVsExistingLegend').innerHTML = CustomersNewVsExistingDynamicPieNew.generateLegend();
        //Need Data - Web orders Vs Phone Orders Over 7 Days
        var total4 = parseInt(data[6]) + parseInt(data[7]);
        var WebVsPhone7DaysCount = 2;
        var WebVsPhone7DaysDynamicData = [];
        WebVsPhone7DaysDynamicData[0] = { value: data[6], color: ColourList[0], highlight: HighLightList[0], label: "Web Orders - " + Math.round(((parseInt(data[6]) / total4) * 100) * 10) / 10 + '%' }
        WebVsPhone7DaysDynamicData[1] = { value: data[7], color: ColourList[1], highlight: HighLightList[1], label: "Phone Orders - " + Math.round(((parseInt(data[7]) / total4) * 100) * 10) / 10 + '%' }

        var WebVsPhone7DaysOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false,
        }

        var WebVsPhone7DaysPie = document.getElementById("PieWebVsPhone7Days").getContext("2d");
        var WebVsPhone7DaysDynamicPieNew = new Chart(WebVsPhone7DaysPie).Pie(WebVsPhone7DaysDynamicData, WebVsPhone7DaysOptions);
        document.getElementById('PieWebVsPhone7DaysLegend').innerHTML = WebVsPhone7DaysDynamicPieNew.generateLegend();
    })
    .fail(function (xhr, status, error) {
        alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr.responseText);
    });

    //ComplexPieChart
    $.ajax({
        type: "Get",
        url: "/Overview/GetPieChartDataComplex",
        data: {},
        dataType: "json"
    })
    .done(function (data) {
        var pause = 1;
        var one = data[0];
        var two = data[1];
        //Number of each postage type
        var total5 =0;
        var NumberOfEachPostageTypeCount = one.length;
        var NumberOfEachPostageTypeDynamicData = [];
        for (var n = 0; n < NumberOfEachPostageTypeCount;) {
            total5 = total5 + parseInt(one[n].Variable1)
            n++;
        }


        for (var j = 0; j < NumberOfEachPostageTypeCount;) {
            NumberOfEachPostageTypeDynamicData[j] =
                {
                    value: one[j].Variable1.toString(),
                    color: ColourList[j],
                    highlight: HighLightList[j],
                    label: one[j].Variable2.toString() + ' -' + Math.round(((parseInt(one[j].Variable1) / total5) * 100) * 10) / 10 + '%'
                }
            j++;
        }

        var NumberOfEachPostageTypeOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false,
        }

        var NumberOfEachPostageTypePie = document.getElementById("PieNumberOfEachPostageType").getContext("2d");
        var NumberOfEachPostageTypeDynamicPieNew = new Chart(NumberOfEachPostageTypePie).Pie(NumberOfEachPostageTypeDynamicData, NumberOfEachPostageTypeOptions);
        document.getElementById('NumberOfEachPostageTypelegend').innerHTML = NumberOfEachPostageTypeDynamicPieNew.generateLegend();

        //Number of each postage type 7 days
        var total6 = 0;
        var NumberOfEachPostageType7DaysCount = two.length;
        var NumberOfEachPostageType7DaysDynamicData = [];
        for (var n = 0; n < NumberOfEachPostageType7DaysCount;) {
            total6 = total6 + parseInt(two[n].Variable1)
            n++;
        }

        for (var j = 0; j < NumberOfEachPostageType7DaysCount;) {
            NumberOfEachPostageType7DaysDynamicData[j] =
                {
                    value: two[j].Variable1.toString(),
                    color: ColourList[j],
                    label: two[j].Variable2.toString() + ' -' + Math.round(((parseInt(two[j].Variable1) / total6) * 100) * 10) / 10 + '%'
                }
            j++;
        }

        var NumberOfEachPostageType7DaysOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false,
        }

        var NumberOfEachPostageType7DaysPie = document.getElementById("PieNumberOfEachPostageType7Days").getContext("2d");
        var NumberOfEachPostageType7DaysDynamicPieNew = new Chart(NumberOfEachPostageType7DaysPie).Pie(NumberOfEachPostageType7DaysDynamicData, NumberOfEachPostageType7DaysOptions);
        document.getElementById('NumberOfEachPostageType7DaysLegend').innerHTML = NumberOfEachPostageType7DaysDynamicPieNew.generateLegend();
       
    })
    .fail(function (xhr, status, error) {

    });
   
    $.ajax({
        type: "Get",
        url: "/Overview/GetBarChartData",
        data: {},
        dataType: "json"
    })
    .done(function (data) {
        var pause = 1;
        //Need Data - TurnOver vs Margin
        var TurnoverVsMarginExVat7DaysDynamicData = [];
        TurnoverVsMarginExVat7DaysDynamicData[0] =
                {
                    label: "turnoverexvat",
                    fillColor: RGBColourList[0],
                    strokeColor: ColourList[0],
                    pointColor: "rgba(220,220,220,1)",
                    pointStrokeColor: ColourList[0],
                    pointHighlightFill: ColourList[0],
                    pointHighlightStroke: "rgba(220,220,220,1)",
                    data: [data[0].Variable2.toString(), data[1].Variable2.toString(), data[2].Variable2.toString(), data[3].Variable2.toString(), data[4].Variable2.toString(), data[5].Variable2.toString(), data[6].Variable2.toString()]
                }
        TurnoverVsMarginExVat7DaysDynamicData[1] =
                {
                    label: "marginexvat",
                    fillColor: RGBColourList[1],
                    strokeColor: ColourList[1],
                    pointColor: "rgba(220,220,220,1)",
                    pointStrokeColor: ColourList[1],
                    pointHighlightFill: ColourList[1],
                    pointHighlightStroke: "rgba(220,220,220,1)",
                    data: [data[0].Variable3.toString(), data[1].Variable3.toString(), data[2].Variable3.toString(), data[3].Variable3.toString(), data[4].Variable3.toString(), data[5].Variable3.toString(), data[6].Variable3.toString()]
                }


        var TurnoverVsMarginExVat7DaysDynamicDataWithLabels = {
            labels: [data[0].Variable1.toString().split(' ')[0], data[1].Variable1.toString().split(' ')[0], data[2].Variable1.toString().split(' ')[0], data[3].Variable1.toString().split(' ')[0], data[4].Variable1.toString().split(' ')[0], data[5].Variable1.toString().split(' ')[0], data[6].Variable1.toString().split(' ')[0]],
            datasets: TurnoverVsMarginExVat7DaysDynamicData
        }

        var TurnoverVsMarginExVat7DaysLine = document.getElementById("BarTurnoverVsMarginExVat7Days").getContext("2d");
        var TurnoverVsMarginExVat7DaysDynamicLineNew = new Chart(TurnoverVsMarginExVat7DaysLine).Bar(TurnoverVsMarginExVat7DaysDynamicDataWithLabels);

    })
    .fail(function (xhr, status, error) {

    });
    
    $.ajax({
        type: "Get",
        url: "/Overview/GetBarChartData6month",
        data: {},
        dataType: "json"
    })
    .done(function (data) {
        var pause = 1;
        //Need Data - TurnOver vs Margin
        var TurnoverVsMarginExVat6MonthsDynamicData = [];
        TurnoverVsMarginExVat6MonthsDynamicData[0] =
                {
                    label: "turnoverexvat",
                    fillColor: RGBColourList[0],
                    strokeColor: ColourList[0],
                    pointColor: "rgba(220,220,220,1)",
                    pointStrokeColor: ColourList[0],
                    pointHighlightFill: ColourList[0],
                    pointHighlightStroke: "rgba(220,220,220,1)",
                    data: [data[0].Variable2.toString(), data[1].Variable2.toString(), data[2].Variable2.toString(), data[3].Variable2.toString(), data[4].Variable2.toString(), data[5].Variable2.toString(), data[6].Variable2.toString()]
                }
        TurnoverVsMarginExVat6MonthsDynamicData[1] =
                {
                    label: "marginexvat",
                    fillColor: RGBColourList[1],
                    strokeColor: ColourList[1],
                    pointColor: "rgba(220,220,220,1)",
                    pointStrokeColor: ColourList[1],
                    pointHighlightFill: ColourList[1],
                    pointHighlightStroke: "rgba(220,220,220,1)",
                    data: [data[0].Variable3.toString(), data[1].Variable3.toString(), data[2].Variable3.toString(), data[3].Variable3.toString(), data[4].Variable3.toString(), data[5].Variable3.toString(), data[6].Variable3.toString()]
                }


        var TurnoverVsMarginExVat6MonthsDataWithLabels = {
            labels: [CreateMonth(data[0].Variable1.toString().split(' ')[0]),
                     CreateMonth(data[1].Variable1.toString().split(' ')[0]),
                     CreateMonth(data[2].Variable1.toString().split(' ')[0]),
                     CreateMonth(data[3].Variable1.toString().split(' ')[0]),
                     CreateMonth(data[4].Variable1.toString().split(' ')[0]),
                     CreateMonth(data[5].Variable1.toString().split(' ')[0]), 
                     CreateMonth(data[6].Variable1.toString().split(' ')[0])],
            datasets: TurnoverVsMarginExVat6MonthsDynamicData
        }

        var TurnoverVsMarginExVat6MonthsLine = document.getElementById("BarTurnoverVsMarginExVat6Months").getContext("2d");
        var TurnoverVsMarginExVat6MonthsDynamicLineNew = new Chart(TurnoverVsMarginExVat6MonthsLine).Bar(TurnoverVsMarginExVat6MonthsDataWithLabels);

    })
    .fail(function (xhr, status, error) {

    });


    

    

    
};

function CreateMonth(Date) {
    var wMonths = ['nothing', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
    var dateNumber = Date.split('/')[1];
    if (dateNumber.charAt(0) == 0) {
        dateNumber = dateNumber.substring(1);
    }
    var month = wMonths[dateNumber];
   

    
    return month + ' ' + Date.split('/')[2];
}
