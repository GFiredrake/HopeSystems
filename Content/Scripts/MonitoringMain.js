window.onload = function () {
    getMonitorInfo();
};

function getMonitorInfo() {
    $.ajax({
        type: "Get",
        url: "/Monitoring/GetMonitorInfo",
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

        if (parseInt($('#NewOrdersToday').text()) > parseInt($('#OrdersMade').val()) && document.getElementById('newOders').checked == false)
        {
            var audio = new Audio("http://hope.tools/Content/Sounds/cash-register-05.wav");
            audio.play();
        }
        $('#OrdersMade').val($('#NewOrdersToday').text())


        getAllerts();
    })

    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("There has been a problem, please reload the page if the problem persists please contact support. The provlem was with /Monitoring/GetMonitorInfo" + textStatus + " " + errorThrown)
    });

    setTimeout(arguments.callee, 600000);
}

function getAllerts() {
    var isMainMonitor = false;

    if ($('#isMainMonitor').is(":checked") == false) {
        isMainMonitor = true;
    }

    $.ajax({
        type: "Get",
        url: "/Monitoring/GetMonitorAlerts",
        data: {
            'FlexiUnder': $('#FlexibuyHasEnoughEntrys').text(),
            'LablerRun': $('#LabelerLastRun').text(),
            'PaymentAppRun': $('#PaymentAppLastRun').text(),
            'PaymentAsRunRun': $('#PaymentsAsPaidLastRun').text(),
            'flexiErrorCount': $('#FlexiInput').val(),
            'labelerErrorCount': $('#LabelerInput').val(),
            'paymentAppCount': $('#PaymentInput').val(),
            'paymentAsPaidErrorCount': $('#PaymentAsPaidInput').val(),
            'isMainMonitor': isMainMonitor,
            'potentialDuplicateOrderCount': $('#PotentialDuplicateOrders').text(),
            'potentialDuplicateOrder': $('#PotentialDuplicateOrdersInput').val()
        },
        dataType: "json"
    })
    .done(function (newdata) {

        $('#FlexiInput').val(newdata.FlexiErrorCount.toString())
        $('#LabelerInput').val(newdata.LabelerErrorCount.toString())
        $('#PaymentInput').val(newdata.PaymentAppCount.toString())
        $('#PaymentAsPaidInput').val(newdata.PaymentAsPaidErrorCount.toString())
        $('#PotentialDuplicateOrdersInput').val(newdata.NewDuplicateOrdersCount.toString());

        $(".MonitoringWarningDiv div").parent().find('div').removeClass("RedBoxDiv");

        if (newdata.DidFlexiError == 1) {
            $.ajax({
                type: "POST",
                url: "/Monitoring/RecordAndFixBrokenFlexiBuyRecords",
                data: {},
                dataType: "json"
            })
            .done(function (data) {
                //$('#ErrorModal').append("</br>There are flexibuy records in Order_Items that have hat no Payment Schedule entrys created. the effected reacords have been recorded and fixed</br>");
            })
            .fail(function () {
                //$('#ErrorModal').append("</br>There are flexibuy records in Order_Items that have hat no Payment Schedule entrys created.Though there was an error in recording and fixing them.</br>");
            });


            $('#FlexiDiv').addClass("RedBoxDiv");
            var audio = new Audio("http://hope.tools/Content/Sounds/gameover.wav");
            audio.play();
        }

        $.ajax({
            type: "GET",
            url: "/Monitoring/GetFlexiErrorsNumber",
            data: {},
            dataType: "json"
        })
        .done(function (data) {
            $('#FlexiErrorsToday').empty();
            $('#FlexiErrorsToday').append(data);
        })
        .fail(function () {
        });

        if (newdata.DidLabelerError == 1) {
            $('#ErrorModal').append("</br>The Labeler has not run in over an hour and twenty Minutes.</br>");
            $('#LabelerDiv').addClass("RedBoxDiv");
            var audio = new Audio("http://hope.tools/Content/Sounds/greatscott3.wav");
            audio.play();
        }

        if (newdata.DidPaymentApp == 1) {
            $('#ErrorModal').append("</br>The Payment App has not run in over an hour and a half.</br>");
            $('#PaymentAppDiv').addClass("RedBoxDiv");
            var audio = new Audio("http://hope.tools/Content/Sounds/busy.wav");
            audio.play();
        }

        if (newdata.DidPaymentAsPaidError == 1) {
            $('#ErrorModal').append("</br>The Payment as paid stored proc has not run in over a minuet.</br>");
            $('#PaymentAppDiv').addClass("RedBoxDiv");
            var audio = new Audio("http://hope.tools/Content/Sounds/wegotone.wav");
            audio.play();
        }

        if (newdata.DidDuplicateOrdersError == 1)
        {
        //    if (newdata.WasError == 1)
        //    {
        //        $('#ErrorModal').append("</br>There have been potential duplicate orders created.</br>");
        //        var audio = new Audio("http://hope.tools/Content/Sounds/problem.wav");
        //        audio.play();
        //    }
            $('#DuplicateOrdersDiv').addClass("AmberBoxDiv");
        }

        if (newdata.WasError == 1) {
            $('#myAnchor')[0].click();
        }

        checkIdPotentialDuplicateOrdersNeedsZeroing()
    })

    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("There has been a problem, please reload the page if the problem persists please contact support. The provlem was with /Monitoring/GetMonitorAlerts" + textStatus + " " + errorThrown)
    });
}

function checkIdPotentialDuplicateOrdersNeedsZeroing() {
    var startTime = '11:57 PM';
    var endTime = '11:59 PM';

    var curr_time = getval();

    if (get24Hr(curr_time) > get24Hr(startTime) && get24Hr(curr_time) < get24Hr(endTime)) {
        $('#PotentialDuplicateOrdersInput').val("0");
    } 
}

function get24Hr(time){
    var hours = Number(time.match(/^(\d+)/)[1]);
    var AMPM = time.match(/\s(.*)$/)[1];
    if(AMPM == "PM" && hours<12) hours = hours+12;
    if(AMPM == "AM" && hours==12) hours = hours-12;

    var minutes = Number(time.match(/:(\d+)/)[1]);
    hours = hours*100+minutes;
    console.log(time +" - "+hours);
    return hours;
}

function getval() {
    var currentTime = new Date()
    var hours = currentTime.getHours()
    var minutes = currentTime.getMinutes()

    if (minutes < 10) minutes = "0" + minutes;

    var suffix = "AM";
    if (hours >= 12) {
        suffix = "PM";
        hours = hours - 12;
    }
    if (hours == 0) {
        hours = 12;
    }
    var current_time = hours + ":" + minutes + " " + suffix;

    return current_time;

}

function CloseModal()
{
    $('#ErrorModal').empty();
}