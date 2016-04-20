window.onload = function () {
    getDashboardInfo();
};

function getDashboardInfo() {
    $.ajax({
        type: "Get",
        url: "/Dashboard/GetDashboardInfo",//change
        dataType: "json"
    })
    .done(function (data) {
        var list = data;

        $.each(list, function (index, item) {
            $('#' + index).empty();
            $('#' + index).append(item);
        });

        $('#LastReloaded').empty();
        $('#LastReloaded').append(new Date().toString())

        if (parseInt($('#NewOrdersToday').text()) > parseInt($('#OrdersMade').val()) && document.getElementById('newOders').checked == false) {
            var audio = new Audio("http://hope.tools/Content/Sounds/cash-register-05.wav");
            audio.play();
        }
        $('#OrdersMade').val($('#NewOrdersToday').text())


        //getAllerts();
    })

    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("There has been a problem, please reload the page if the problem persists please contact support. The provlem was with /Monitoring/GetMonitorInfo" + textStatus + " " + errorThrown)
    });

    setTimeout(arguments.callee, 60000);
}