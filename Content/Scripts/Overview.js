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
        //Need Data - Web orders Vs Phone Orders Over 7 Days
        var WebVsPhone7DaysCount = 2;
        var WebVsPhone7DaysDynamicData = [];
        WebVsPhone7DaysDynamicData[0] = { value: data[6], color: ColourList[0], highlight: HighLightList[0], label: "Web Orders" }
        WebVsPhone7DaysDynamicData[1] = { value: data[7], color: ColourList[1], highlight: HighLightList[1], label: "Phone Orders" }

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
        var NumberOfEachPostageTypeCount = one.length;
        var NumberOfEachPostageTypeDynamicData = [];


        for (var j = 0; j < NumberOfEachPostageTypeCount;) {
            NumberOfEachPostageTypeDynamicData[j] =
                {
                    value: one[j].Variable1.toString(),
                    color: ColourList[j],
                    highlight: HighLightList[j],
                    label: one[j].Variable2.toString()
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
        var NumberOfEachPostageType7DaysCount = two.length;
        var NumberOfEachPostageType7DaysDynamicData = [];

        for (var j = 0; j < NumberOfEachPostageType7DaysCount;) {
            NumberOfEachPostageType7DaysDynamicData[j] =
                {
                    value: two[j].Variable1.toString(),
                    color: ColourList[j],
                    label: two[j].Variable2.toString()
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
   

    
    


    

    

    //Need Data - TurnOver vs Margin
    //var TurnoverVsMarginExVat7DaysCount = 2;
    //var TurnoverVsMarginExVat7DaysDynamicData = [];
    //for (var l = 0; l < TurnoverVsMarginExVat7DaysCount;) {
    //    TurnoverVsMarginExVat7DaysDynamicData[l] =
    //        {
    //            label: "Dataset " + (l + 1).toString(),
    //            fillColor: RGBColourList[l],
    //            strokeColor: ColourList[l],
    //            pointColor: "rgba(220,220,220,1)",
    //            pointStrokeColor: ColourList[l],
    //            pointHighlightFill: ColourList[l],
    //            pointHighlightStroke: "rgba(220,220,220,1)",
    //            data: [Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100)]
    //        }
    //    l++;
    //}


    //var TurnoverVsMarginExVat7DaysDynamicDataWithLabels = {
    //    labels: ["Day 1", "Day 2", "Day 3", "Day 4", "Day 5", "Day 6", "Day 7"],
    //    datasets: TurnoverVsMarginExVat7DaysDynamicData
    //}

    //var TurnoverVsMarginExVat7DaysLine = document.getElementById("BarTurnoverVsMarginExVat7Days").getContext("2d");
    //varTurnoverVsMarginExVat7DaysDynamicLineNew = new Chart(TurnoverVsMarginExVat7DaysLine).Bar(TurnoverVsMarginExVat7DaysDynamicDataWithLabels);
};

