window.onload = function () {
    $('#IsActive').addClass("Hidden");
    $('#IsActive').addClass("PermisionCheckbox");
};

//Done
function GetSkuData() {
    $('#hiddensku').val($('#SkuInput').val())
    $('#ErrorDiv').addClass('HiddenDiv')
    $('#SkuErrorDiv').addClass('HiddenDiv')
    if ($('#SkuInput').val().length == 6)
    {
        $.ajax({
            type: "Get",
            url: "/Tv/GetSkuDataToEdit",
            data: { 'Sku': $('#SkuInput').val() },
            dataType: "json"
        })
        .done(function (data) {
            var pause = 1;
            if(data.Variable1 == null)
            {
                $('#SkuErrorDiv').removeClass('HiddenDiv')
            }
            else
            {

                var permision = parseInt(data.Variable7.toString())
                $('#ProductId').val(data.Variable1.toString());
                $('#SkuInfoDiv').removeClass('HiddenDiv');
                $('#Description').val(data.Variable2.toString());
                $('#WebText').val(data.Variable3.toString());
                $('#ProducerNotesSku').val(data.Variable4.toString());
                $('#BuyerNotes').val(data.Variable5.toString());
                $('#IsActive').removeClass();
                $('#IsActive').empty();
                $('#VariationDiv').empty();
                $('#LastModified').empty();
                if (data.Variable6.toString() == 1)
                {
                    $('#IsActive').append('Product Active.');
                    //$("#IsActiveCheckBox").prop("checked", true);
                    //$("#IsActiveCheckBoxValue").val("1");
                    $('#IsActive').addClass('green')
                }
                else {
                    $('#IsActive').append('Product Not Active');
                    //$("#IsActiveCheckBox").prop("checked", false);
                    //$("#IsActiveCheckBoxValue").val("0");
                    $('#IsActive').addClass('red')
                }

                $('#LastModified').append("Last Modified By: " + data.Variable9.toString() + " On the " + data.Variable8.toString())


                GetSkuVariationData(permision, data.Variable1.toString());

            }
        })

        .fail(function (xhr, status, error) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr.responseText);
        });
    }
    else {
        $('#ErrorDiv').removeClass('HiddenDiv')
    }
    
}
function GetSkuVariationData(permision, Parentproductid) {
    $.ajax({
        type: "Get",
        url: "/Tv/GetSkuVariationDataToEdit",
        data: { 'Sku': $('#SkuInput').val(), 'PPId': Parentproductid },
        dataType: "json"
    })
        .done(function (data) {
            var pause = 1;
            var dataLength = data.length;
            var base = 0;
            while (base < dataLength)
            {
                var active = "";
                if (data[base].Variable4.toString() == "1")
                {
                    active = "Product Active."
                }
                else {
                    active = "Product Not Active."
                }
                $('#VariationDiv').append('<div>Variation SKU: ' + data[base].Variable1.toString() + '<br /> Variation Description: <input type="text" name="VariationDescription' + base + '" value="' + data[base].Variable2.toString() + '"> <br /> Free Quantity: ' + data[base].Variable3.toString() + '<br /> <div id="' + base + 'Active">' + active + '</div></div><br /><br /><input type="text" class="Hidden" name="VariationSku' + base + '" value="' + data[base].Variable1.toString() + '">'); //<input type="checkbox" name="Variation' + base + 'IsActiveCheckBox" id="Variation' + base + 'IsActiveCheckBox" class="Hidden PermisionCheckbox" value="Active">
                if (active == "Product Active.")
                {
                    $('#' + base + 'Active').addClass('green');
                    $('#Variation' + base + 'IsActiveCheckBox').prop("checked", true)
                }
                else{
                    $('#' + base + 'Active').addClass('red');
                }
                base++;
            }
            //if (permision => 65) {
            //    $('.PermisionCheckbox').removeClass('Hidden');
            //}
                
        })

        .fail(function (xhr, status, error) {
            alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr.responseText);
        });
}

$('#toggle').click(function () {
    //check if checkbox is checked
    if ($(this).is(':checked')) {

        $('#SaveSkuForm').removeAttr('disabled'); //enable input

    } else {
        $('#SaveSkuForm').attr('disabled', true); //disable input
    }
});



//Incompleate
//function SaveSkuData() {
//    var mainActive = 0;
//    if (document.getElementById('IsActive').checked) {
//        mainActive = 1;
//    }

//    $.ajax({
//        type: "Get",
//        url: "/Tv/SaveDataForSku",
//        data: { 'Description': $('#Description').val(), 'WebText': $('#WebText').val(), 'ProducerNotesSku': $('#ProducerNotesSku').val(), 'BuyerNotes': $('#BuyerNotes').val(), 'IsActive': mainActive },
//        dataType: "json"
//    })
//       .done(function (data) {
          
//       })

//       .fail(function (xhr, status, error) {
//           alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr.responseText);
//       });
//}