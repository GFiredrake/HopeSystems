function GetSkuVariationData() {
    $('#ErrorDiv').addClass('HiddenDiv')

    if ($('#SkuInput').val().length != 6)
    {
        $('#ErrorDiv').removeClass('HiddenDiv')
        return false;
    }

    $.ajax({
        type: "Get",
        url: "/Product/GenerateSkuData",
        data: { 'Sku': $('#SkuInput').val() },
        dataType: "json"
    })

    .done(function (data) {
        var pause = 1;
        $('#VariationDiv').empty();
        var length = parseInt(data.length);
        var i = 0
        while (i < length) {
            $('#VariationDiv').append('<div class="VariationBox" id="Variation' + i.toString() + '">' +
                'Variation Id:' + data[i].VariationId.toString() + '<br/>' +
                'Item Description:' + data[i].TvDescription.toString() + '<br/>' +
                'Variation description:' + data[i].VariationName.toString() + '<br/>' +
                'Stock Qty:' + data[i].FreeQty.toString() + '<br/><br/>' +
                '<input type="number" pattern="[0-9]*" id="ValueInput"><br/>' +
                '</input><button id="AddButton" onclick="AddStockLevel(' + i + ')">Add to Stock</button>' +
                '</input><button id="RemoveButton" onclick="RemoveStockLevel(' + i.toString + ')">Remove from Stock</button>' +
                '</div><br/><br/>')
            i++;
        }
    })

    .fail(function (jqXHR, textStatus, errorThrown) {

    });
}

function AddStockLevel(variant) {
    var pause = 1;
}

function RemoveStockLevel(variant) {
    var pause = 1;
}