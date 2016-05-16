function GenerateOdsReport() {
    $('#ErrorDiv').addClass('Hidden');
    $('#TopInfoDiv').addClass('HiddenDiv');
    if ($('#DateSelector').val() != "" && $('#ActiveChannels').val() != "0") {
        
        $('#DisplayTable').empty();
        $.ajax({
            type: "Get",
            url: "/Reporting/GenerateOdsReport",
            data: { 'Date': $('#DateSelector').val(), 'Channell': $('#ActiveChannels').val() },
            dataType: "json"
        })
        .done(function (data) {
            $('#DisplayTable').append("<tr>" +
                                            "<td>Time</td>" +
                                            "<td>IncVat</td>" +
                                            "<td>ExVat</td>" +
                                            "<td>IncVatMargin</td>" +
                                            "<td>ExVatMargin</td>" +
                                            "<td>SoldQty</td>" +
                                       "</tr>");

            $.each(data, function (index, item) {
                GenerateAdditionalLines(item)
            });
            $('#TableArea').removeClass('HiddenDiv');
            GenerateTopInfo();
            GenerateVariationInfo();
        })

        .fail(function (jqXHR, textStatus, errorThrown) {

        });
    }
    else {
        $('#ErrorDiv').removeClass('Hidden');
    }
}

function GenerateAdditionalLines(item) {

    var content = "<tr>"
    for (key in item) {
            var value = item[key];
            content += '<td>' + value + '</td>';
    }
    content += "</tr>"
    $('#DisplayTable').append(content);
}

function GenerateTopInfo() {
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateTopInfo",
        data: { 'Date': $('#DateSelector').val(), 'Channell': $('#ActiveChannels').val() },
        dataType: "json"
    })
        .done(function (data) {
            $('#TopInfoDiv').removeClass('HiddenDiv');
            $('#SkuH').empty();
            $('#DescriptionH').empty();
            $('#SkuH').append(data[0])
            $('#DescriptionH').append(data[1])
        })

        .fail(function (jqXHR, textStatus, errorThrown) {

        });
}
function GenerateVariationInfo() {
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateVariationInfo",
        data: { 'Date': $('#DateSelector').val(), 'Channell': $('#ActiveChannels').val() },
        dataType: "json"
    })
        .done(function (data) {
            $('#variationDiv').empty();
            var pause = 1;
            var length = parseInt(data.length);
            var i = 0
            while (i < length)
            {
                $('#variationDiv').append('Variation Sku: ' + data[i].Variable1.toString() + ' - Variation Description: ' + data[i].Variable2.toString() + ' - Variation in Stock: ' + data[i].Variable3.toString() + '<br />')
                i++;
            }

        })

        .fail(function (jqXHR, textStatus, errorThrown) {

        });
}