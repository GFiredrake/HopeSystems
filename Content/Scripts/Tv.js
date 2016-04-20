$("#ActiveChannels").change(function () {
    //var d = new Date(),
    //    month = '' + (d.getMonth() + 1),
    //    day = '' + d.getDate(),
    //    year = d.getFullYear();

    //if (month.length < 2) month = '0' + month;
    //if (day.length < 2) day = '0' + day;

    //var today = [year, month, day].join('-');

    //$('#datePicker').val(today);
    $("#DateDiv").removeClass("HiddenDiv");
});

$('#datePicker').change(function () {

    $.ajax({
        type: "POST",
        url: "/Tv/HasChannelAndDateBeenCompleated",
        data: { 'Channel': $('#ActiveChannels').val(), 'Date': $("#datePicker").val() },
        dataType: "json"
    })
    .done(function (data) {
        if (data == 0) {
            $("#HourDiv").removeClass("HiddenDiv");
            $("#ErrorArea").addClass("HiddenDiv");
        }
        else {
            $("#ErrorArea").removeClass("HiddenDiv");
            $("#ErrorArea").append("The Minutes for this date have already been compleated and sent, please try another date");
        }
    })
    .fail(function (xhr) {

    });

    
});

$('#HourSelect').change(function () {
    $.ajax({
        type: "GET",
        url: "/Tv/RetriveInfoForDateAndHour",
        data: { 'Channel': $('#ActiveChannels').val(), 'Date': $("#datePicker").val(), 'Hour': $("#HourSelect").val() },
        dataType: "json"
    })
    .done(function (data) {
        $("#NotesArea").text(data.Notes);
        $("#ShowNameDisplay").text(data.ShowName);
        $("#ShowSales").text("£" + data.Sales);
        $("#Director").val(data.Director);
        $("#Floor").val(data.Floor);
        $("#Guest").val(data.Guest);
        $("#ProducerNotes").val(data.Notes);
        $("#DirectorNotes").val(data.DirectorNotes);
        $("#FloorNotes").val(data.FloorNotes);
        $("#ForwardNotes").val(data.GoingForward);

        //hidden data
        $("#Date").val($("#datePicker").val());
        $("#Hour").val($("#HourSelect").val());
        $("#Total").val(data.Sales);
        $("#ShowName").val(data.ShowName);
        $("#ActivePresenters").val(data.Presenter);
        $("#ActiveProducers").val(data.Producer);
        $("#Gui").val($("#Gui").val());

    })
    .fail(function (xhr) {

    });


    $("#EditArea").removeClass("HiddenDiv");
});

function clicked(e) {
    if (!confirm('Are you sure you wish to finalise the day, if you continue no additional notes will be able to be entered for today. would you like to close today and send?')) e.preventDefault();
}