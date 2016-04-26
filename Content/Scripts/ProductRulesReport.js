function getInfo() {
    $('#DisplayTable1').empty();
    $('#DisplayTable2').empty();

    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateProductRulesReport",
        data: { 'Days': $('#DaysSelect').val() },
        dataType: "json"
    })
    .done(function (data) {
        $('#DisplayTable1').append("<tr>" +
                                        "<td>SKU</td>" +
                                        "<td>TV Description</td>" +
                                        "<td>Item Rule (From > To)</td>" +
                                        "<td>Date of Action</td>" +
                                   "</tr>");
        $('#DisplayTable2').append("<tr>" +
                                        "<td>SKU</td>" +
                                        "<td>Tv Description</td>" +
                                        "<td>Item Rule (From > To)</td>" +
                                        "<td>Date of Action</td>" +
                                   "</tr>");
        $.each(data, function (index, item) {
            GenerateAdditionalLines(item)
        });
    })

    .fail(function (jqXHR, textStatus, errorThrown) {
       
    });
}

function GenerateAdditionalLines(item) {
    var whattable = '0';
    var content = "<tr>"
    for (key in item) {
        if (item[key] == '1') {
            whattable = '1'
        }
        else if (item[key] == '2') {
            whattable = '2'
        }
        else {
            var value = item[key];
            content += '<td>' + value + '</td>';
        }
    }
    content += "</tr>"
    $('#DisplayTable' + whattable).append(content);
}