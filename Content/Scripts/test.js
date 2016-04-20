window.onload = function () {
    //red, yellow, purple, dark blue, light blue, light green, olive green, black, grey
    var colourList = ['#ff3333', '#ffcc00', '#d279a6', '#6666ff', '#80d4ff', '#40bf40', '#408000', '#000000', '#8c8c8c'];
    var HighLightList = ['#ff8080', '#ffdb4d', '#df9fbf', '#9999ff', '#b3e6ff', '#66cc66', '#59b300', '#262626', '#bfbfbf'];


   //create data dynamically - pie chart

    var PieCount = 9;
    var DynamicData = [];
    for (var j = 0; j < PieCount;) {
        DynamicData[j] = 
            {
                value: Math.round(Math.random() * 100),
                color: colourList[j],
                highlight: HighLightList[j],
                label: colourList[j]
            }
        j++;
    }

    var MyPie = document.getElementById("DynamicPie").getContext("2d");
    var DynamicPieNew = new Chart(MyPie).Pie(DynamicData);

    // create data dynamically - Line chart - should make colours see thru using rgba??

    var CountLine = 3;
    var DynamicLineData = [];
    for (var l = 0; l < CountLine;) {
        DynamicLineData[l] = 
            {
                label: "Dataset " + (l + 1).toString(),
                fillColor: colourList[l],
                strokeColor: 'black',
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: HighLightList[l],
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: [Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100), Math.round(Math.random() * 100)]
            }
        l++;
    }

    var mylinedata = {
        labels: ["January", "February", "March", "April", "May", "June", "July"],
        datasets: DynamicLineData
    }

    var MyLine = document.getElementById("DynamicLine").getContext("2d");
    var DynamicLineNew = new Chart(MyLine).Line(mylinedata);


};