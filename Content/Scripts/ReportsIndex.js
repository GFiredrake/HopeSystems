$(document).ready(function () {
    $.ajax({
        type: "Get",
        url: "/Reporting/GenerateAvailableReportsSinglePage",
        data: {},
        dataType: "json"
    })
        .done(function (data) {
            var pause = 1;

            var length = data.length;
            var i = 0;

            while (i < length)
            {
                $('#Div' + data[i].DepartmentId.toString()).append('<h4><a href="' + data[i].ReportAction.toString() + '">' + data[i].ReportName.toString() + '</a></h4>');

                $('#DivContainer' + data[i].DepartmentId.toString()).removeClass("HiddenDiv");

                i++;
            }
        })

        .fail(function (jqXHR, textStatus, errorThrown) {

        });
});