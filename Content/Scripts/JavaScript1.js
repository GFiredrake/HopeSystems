(function () {
    $.ajax({
        type: "Get",
        url: "/Monitoring/GetMonitorInfo",
        dataType: "json"
    })
    .done(function (data) {

    })

    .fail(function () {
        alert("There has been a problem, please reload the page if the problem persists please contact support. The provlem was with /Monitoring/GetMonitorInfo")
    });


});