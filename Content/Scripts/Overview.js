window.onload = function () {
    var ColourList = ['#ED137D', '#314b9f', '#872990 '];
    var HighLightList = ['#f259a4', '#6e81bb', '#ab69b1'];
    var RGBColourList = ["rgba(237,19,125,0.5)", "rgba(49,75,159,0.5)", "rgba(135,41,144,0.5)"];

    //PieChartDataList
    $.ajax({
        type: "Get",
        url: "/Overview/GetPieChartData",
        data: { },
        dataType: "json"
    })
    .done(function (data) {
        var pause = 1;
        //Web orders Vs Phone Orders
        var WebVsPhoneDynamicData = [];
        WebVsPhoneDynamicData[0] = { value: data[0], color: ColourList[0], highlight: HighLightList[0], label: "Web Orders" }
        WebVsPhoneDynamicData[1] = { value: data[1], color: ColourList[1], highlight: HighLightList[1], label: "Phone Orders" }

        var WebVsPhoneOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false
        }

        var WebVsPhonePie = document.getElementById("PieWebVsPhone").getContext("2d");
        var WebVsPhoneDynamicPieNew = new Chart(WebVsPhonePie).Pie(WebVsPhoneDynamicData, WebVsPhoneOptions);
        document.getElementById('PieWebVsPhoneLegend').innerHTML = WebVsPhoneDynamicPieNew.generateLegend();
        //Customers Normal Vs Freedom
        var CustomerNormalVsFreedomDynamicData = [];
        CustomerNormalVsFreedomDynamicData[0] = { value: data[4], color: ColourList[0], highlight: HighLightList[0], label: "Freedom Customers" }
        CustomerNormalVsFreedomDynamicData[1] = { value: data[5], color: ColourList[1], highlight: HighLightList[1], label: "Non Freedom Customer" }

        var CustomerNormalVsFreedomOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false,
            percentageInnerCutout: 50,
        }

        var CustomerNormalVsFreedomPie = document.getElementById("PieCustomerNormalVsFreedom").getContext("2d");
        var CustomerNormalVsFreedomDynamicPieNew = new Chart(CustomerNormalVsFreedomPie).Pie(CustomerNormalVsFreedomDynamicData, CustomerNormalVsFreedomOptions);
        document.getElementById('PieCustomerNormalVsFreedomLegend').innerHTML = CustomerNormalVsFreedomDynamicPieNew.generateLegend();
        //Need Data - Customers Existing Vs New
        var CustomersNewVsExistingDynamicData = [];
        CustomersNewVsExistingDynamicData[0] = { value: data[2], color: ColourList[0], highlight: HighLightList[0], label: "New Customers" }
        CustomersNewVsExistingDynamicData[1] = { value: data[3], color: ColourList[1], highlight: HighLightList[1], label: "Existing Customers" }

        var CustomersNewVsExistingOptions = {
            segmentShowStroke: false,
            animateRotate: true,
            animateScale: false,
        }

        var CustomersNewVsExistingPie = document.getElementById("PieCustomersNewVsExisting").getContext("2d");
        var CustomersNewVsExistingDynamicPieNew = new Chart(CustomersNewVsExistingPie).Pie(CustomersNewVsExistingDynamicData, CustomersNewVsExistingOptions);
        document.getElementById('PieCustomersNewVsExistingLegend').innerHTML = CustomersNewVsExistingDynamicPieNew.generateLegend();
    })
    .fail(function (xhr, status, error) {
        alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr.responseText);
    });


   

    //Need Data - Web orders Vs Phone Orders Over 7 Days
    var WebVsPhone7DaysCount = 2;
    var WebVsPhone7DaysDynamicData = [];
    for (var l = 0; l < WebVsPhone7DaysCount;) {
        WebVsPhone7DaysDynamicData[l] =
            {
                label: "Dataset " + (l + 1).toString(),
                fillColor: RGBColourList[l],
                strokeColor: ColourList[l],
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: ColourList[l],
                pointHighlightFill: ColourList[l],
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: [Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100)]
            }
        l++;
    }


    var WebVsPhone7DaysDynamicDataWithLabels = {
        labels: ["Day 1", "Day 2", "Day 3", "Day 4", "Day 5", "Day 6", "Day 7"],
        datasets: WebVsPhone7DaysDynamicData
    }

    var WebVsPhone7DaysLine = document.getElementById("LineWebVsPhone7Days").getContext("2d");
    var WebVsPhone7DaysDynamicLineNew = new Chart(WebVsPhone7DaysLine).Line(WebVsPhone7DaysDynamicDataWithLabels);


    

    

    //Need Data - TurnOver vs Margin
    var TurnoverVsMarginExVat7DaysCount = 2;
    var TurnoverVsMarginExVat7DaysDynamicData = [];
    for (var l = 0; l < TurnoverVsMarginExVat7DaysCount;) {
        TurnoverVsMarginExVat7DaysDynamicData[l] =
            {
                label: "Dataset " + (l + 1).toString(),
                fillColor: RGBColourList[l],
                strokeColor: ColourList[l],
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: ColourList[l],
                pointHighlightFill: ColourList[l],
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: [Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100)]
            }
        l++;
    }


    var TurnoverVsMarginExVat7DaysDynamicDataWithLabels = {
        labels: ["Day 1", "Day 2", "Day 3", "Day 4", "Day 5", "Day 6", "Day 7"],
        datasets: TurnoverVsMarginExVat7DaysDynamicData
    }

    var TurnoverVsMarginExVat7DaysLine = document.getElementById("BarTurnoverVsMarginExVat7Days").getContext("2d");
    varTurnoverVsMarginExVat7DaysDynamicLineNew = new Chart(TurnoverVsMarginExVat7DaysLine).Bar(TurnoverVsMarginExVat7DaysDynamicDataWithLabels);
};

function GeneratePieChartData() {
    
};