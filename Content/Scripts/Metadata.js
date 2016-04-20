var myConditionOne = false;
var myConditionTwo = false;
var myConditionThree = false;

function getLanguages() {
        
        $.ajax({
            type: "Get",
            url: "/CMS/GetLanguages",
            dataType: "json"
        })
        .done(function (data) {
            $.each(data, function (key, value) {
                $('#languageSelector')
                    .append($("<option></option>")
                    .attr("value", this.Value)
                    .text(this.Text));
            });
        })
        $('.LanguageSelect').removeClass('HiddenDiv');
        $('#MetadataTypeButton').addClass('HiddenDiv');
        myConditionOne = true;
    
}
function GetPosibleMetadataToEdit() {
    if ($("#languageSelector").val() != 0) {
        $.ajax({
            type: "Get",
            url: "/CMS/GetPosibleMetadataToEdit",
            dataType: "json",
            data: { 'InputNumber': $('#MetadataType').val(), 'LanguageId': $("#languageSelector").val() },
        })
        .done(function (data) {
            $.each(data, function (key, value) {
                $('#MetadatorSelector')
                    .append($("<option></option>")
                    .attr("value", this.Value)
                    .text(this.Text));
            });
        })
        
        $('.MetadataSelect').removeClass('HiddenDiv');
        $('#languageSelectButton').addClass('HiddenDiv');
        myConditionTwo = true;
    }
    else {
        $("#LanguageError").append("Please select a language");
    }
}
function EnterMetadata() {
    if ($("#MetadatorSelector").val() != 0) {
        $.ajax({
            type: "Post",
            url: "/CMS/GetCurrentMetadata",
            dataType: "json",
            data: { 'InputNumber': $('#MetadataType').val(), 'LanguageId': $("#languageSelector").val(), 'MetaSelected': $("#MetadatorSelector").val() },
        })
        .done(function (data) {
            $("#MetadataInput").val(data);
        })
        
        $('.MetadataInput').removeClass('HiddenDiv');
        $('#MetadataSelectButton').addClass('HiddenDiv');
        myConditionThree = true;
        $('.Savebutton').removeClass('HiddenDiv');
    }
    else {
        $("#MetadateError").append("Please select an item");
    }
}
function ClearErrors()
{
    $("#MetadataTypeError").empty();
    $("#LanguageError").empty();
    $("#MetadateError").empty();
    $('.SavedDiv').addClass('HiddenDiv');
}
function ResetPage() {
    $('.MetadataInput').addClass('HiddenDiv');
    $('.Savebutton').addClass('HiddenDiv');
    $('.MetadataSelect').addClass('HiddenDiv');
    $('.LanguageSelect').addClass('HiddenDiv');
    $('.SavedDiv').removeClass('HiddenDiv');

    $('#MetadataTypeButton').removeClass('HiddenDiv');
    myConditionOne = false;
    $('#languageSelectButton').removeClass('HiddenDiv');
    myConditionTwo = false;;
    $('#MetadataSelectButton').removeClass('HiddenDiv');
    myConditionThree = false;

    $("#MetadataType").val("0");
    $('#languageSelector').find('option').remove().end().append('<option value="0">Please Select Language...</option>');
    $('#MetadatorSelector').find('option').remove().end().append('<option value="0">Please Select metadata...</option>');
}
function SaveMetadata()
{
    $.ajax({
        type: "Post",
        url: "/CMS/SaveMetaData",
        dataType: "json",
        data: { 'InputNumber': $('#MetadataType').val(), 'LanguageId': $("#languageSelector").val(), 'MetaSelected': $("#MetadatorSelector").val(), 'MetaInput': $("#MetadataInput").val() },
    })
    .done(function (data) {
        ResetPage();
    })
}


$("#MetadataTypeButton").click(function () {
    ClearErrors();
    if ($("#MetadataType").val() != 0) {
        if ($("#MetadataType").val() == 1)
        {
            getLanguages();
        }
        if ($("#MetadataType").val() == 2)
        {
            $('#languageSelector')
                    .append($("<option></option>")
                    .attr("value", 1)
                    .text("English"));
            $("#languageSelector").val("1");
            $('#MetadataTypeButton').addClass('HiddenDiv');
            myConditionOne = true;
            GetPosibleMetadataToEdit();
        }
    }
    else {
        $("#MetadataTypeError").append("Please select a type");
    }
});
$("#languageSelectButton").click(function () {
    ClearErrors();
    GetPosibleMetadataToEdit();
});
$("#MetadataSelectButton").click(function () {
    ClearErrors();
    EnterMetadata();
});
$("#SaveButton").click(function () {
    SaveMetadata();
});

$("#MetadataType").mousedown(function (e) {
    if (myConditionOne) {
        e.preventDefault();
    }
});

$("#languageSelector").mousedown(function (e) {
    if (myConditionTwo) {
        e.preventDefault();
    }
});

$("#MetadatorSelector").mousedown(function (e) {
    if (myConditionThree) {
        e.preventDefault();
    }
});