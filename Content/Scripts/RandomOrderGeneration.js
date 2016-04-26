function GenerateRandomOrder() {

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
        
    })
    .fail(function (xhr, status, error) {
        alert("an error has occured, Please try again. if this problem persists please seak technical support : " + xhr.responseText);
    });
}