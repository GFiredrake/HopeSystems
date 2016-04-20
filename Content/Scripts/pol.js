function RequestReportData() {

    var something = false;

    if ($('#prepNotes').is(":checked") == true)
    {
        something = true;
    }


    $.ajax({
        type: "Post",
        url: "/PolSheet/cmdmakepolsheet_Click",
            data: { 'UserInputShowDate': $('#startDate').val(), 'UserInputShowTime': $('#startTime').val(), 'UserInputCheckPrepNotes': something },
        dataType: "json"
    })

}

// UserInputShowDate,  UserInputShowTime,  UserInputCheckPrepNotes - $('#prepNotes').val()