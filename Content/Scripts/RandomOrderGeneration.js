function GenerateRandomOrder() {
    $('#GenerateButton').addClass('HiddenDiv')
    var rates = document.getElementById('Condition1').checked;

    var condition = 0
    if (document.getElementById('Condition1').checked) {
        condition = 1;
    }
    if (document.getElementById('Condition2').checked) {
        condition = 2;
    }
    if (document.getElementById('Condition3').checked) {
        condition = 3;
    }

    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateRandomOrder",
        data: { 'StartDate': $('#StartDate').val(), 'StartTime': $('#StartTime').val(), 'EndDate': $('#EndDate').val(), 'EndTime': $('#EndTime').val(), 'Condition': condition , 'SKU': $('#SkuInput').val() },
        dataType: "json"
    })
    .done(function (data) {
        var pause = 1;

        $('#DisplayTable').append("<tr>" +
                                          "<td>Order Id</td>" +
                                          "<td>Customer Id</td>" +
                                          "<td>Title</td>" +
                                          "<td>First Name</td>" +
                                          "<td>Last Name</td>" +
                                          "<td>Email Address</td>" +
                                          "<td>Phone Number</td>" +
                                      "</tr>");

        GenerateAdditionalLinesSales(data)

        
    })
    .fail(function (xhr, status, error) {
        alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr.responseText);
    });
}

$("#Condition1").change(function () {
    $("#StartDateDiv").removeClass("HiddenDiv")
    $("#EndDateDiv").removeClass("HiddenDiv")
    $("#SkuDiv").addClass("HiddenDiv")
});

$("#Condition2").change(function () {
    $("#StartDateDiv").removeClass("HiddenDiv")
    $("#EndDateDiv").removeClass("HiddenDiv")
    $("#SkuDiv").removeClass("HiddenDiv")
});

$("#Condition3").change(function () {
    $("#StartDateDiv").removeClass("HiddenDiv")
    $("#EndDateDiv").addClass("HiddenDiv")
    $("#SkuDiv").addClass("HiddenDiv")
});

function GenerateAdditionalLinesSales(item, tableNumber) {
    var content = "<tr>"
    for (key in item) {
            var value = item[key];
            if(value != null )
            content += '<td>' + value + '</td>';
    }

    content += "</tr>"
    $('#DisplayTable').append(content);
}