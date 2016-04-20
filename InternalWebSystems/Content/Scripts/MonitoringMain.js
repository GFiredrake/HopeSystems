﻿(function () {
    $.ajax({
        type: "Get",
        url: "/Monitoring/GetMonitorInfo",
        data: {},
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
                'paymentAsPaidErrorCount': $('#PaymentAsPaidInput').val()
                // 'FreedomErrorCount': $('#FreedomInput').val()
                // 'FreedomUnder': $('#FreedomWithoutRenwals').text()
            },
            dataType: "json"
        })

        .done(function (newdata) {
            

            $('#FlexiInput').val(newdata.FlexiErrorCount.toString())
            $('#LabelerInput').val(newdata.LabelerErrorCount.toString())
            $('#PaymentInput').val(newdata.PaymentAppCount.toString())
            //$('#FreedomInput').val(newdata.FreedomErrorCount.toString())
            $('#PaymentAsPaidInput').val(newdata.PaymentAsPaidErrorCount.toString())

            $(".MonitoringWarningDiv div").parent().find('div').removeClass("RedBoxDiv");

            if (newdata.DidFlexiError == 1) {
                
                
                $.ajax({
                    type: "POST",
                    url: "/Monitoring/RecordAndFixBrokenFlexiBuyRecords",
                    data: {},
                    dataType: "json"
                })
                .done(function (data) {
                    $('#ErrorModal').append("</br>There are flexibuy records in Order_Items that have hat no Payment Schedule entrys created. the effected reacords have been recorded and fixed</br>");
                })
                .fail(function () {
                    $('#ErrorModal').append("</br>There are flexibuy records in Order_Items that have hat no Payment Schedule entrys created.Though there was an error in recording and fixing them.</br>");
                });


                $('#FlexiDiv').addClass("RedBoxDiv");
            }

            if (newdata.DidLabelerError == 1) {
                $('#ErrorModal').append("</br>The Labeler has not run in over an hour and twenty minuets.</br>");
                $('#LabelerDiv').addClass("RedBoxDiv");
            }

            if (newdata.DidPaymentApp == 1) {
                $('#ErrorModal').append("</br>The Payment App has not run in over an hour and a half.</br>");
                $('#PaymentAppDiv').addClass("RedBoxDiv");
            }

            //if (newdata.DidFreedomError == 1) {
            //    $('#ErrorModal').append("</br>There are Freedom Members who dont have any payments schedules.</br>");
            //    $('#FreedomDiv').addClass("RedBoxDiv");
            //}
            if (newdata.DidPaymentAsPaidError == 1) {
                $('#ErrorModal').append("</br>The Payment as paid stored proc has not run in over a minuet.</br>");
                $('#PaymentAppDiv').addClass("RedBoxDiv");
            }

            if (newdata.WasError == 1) {
                $('#myAnchor')[0].click();

                //setTimeout(function () {
                //    //document.getElementById('myTune').play()
                //}, 5000);
                
            }
        })

        .fail(function () {
            alert("There has been a problem, please reload the page if the problem persists please contact support. A001")
        });
    })

    .fail(function () {
        alert("There has been a problem, please reload the page if the problem persists please contact support. I000")
    });

    setTimeout(arguments.callee, 60000);
})();


function CloseModal() {
    //document.getElementById('myTune').pause();
    //document.getElementById('myTune').currentTime = 0;

    $('#FlexiInput').val(0)
    $('#LabelerInput').val(0)
    $('#PaymentInput').val(0)
    //$('#FreedomInput').val(newdata.FreedomErrorCount.toString())
    $('#PaymentAsPaidInput').val(0)
    $('#ErrorModal').empty();
};