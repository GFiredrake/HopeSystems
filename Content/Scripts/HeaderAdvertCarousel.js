$(document).ready(function () {
    $.ajax({
        type: "Get",
        url: "/CMS/RetrieveHeaderAdvertCarouselData",
        data: {},
        dataType: "json"
    })
        .done(function (data) {
            var pause = 1;

            var length = parseInt(data.length);
            var i = 0
            while (i < length) {
                $('#dropdownNumbers').append(data[i].carouselid.toString() + ',');
                $('#MainInfoDiv').append(
                                         '<div id="Carousel' + data[i].carouselid.toString() + 'Div" class="CarouselItem">' +
                                            '<h3>' + data[i].carouselid.toString() + '</h3>' +
                                            '<h5>Image URL:</h5> <p id="imageurl' + data[i].carouselid.toString() + '">' + data[i].imageurl.toString() + '</p>' +
                                            '<h5>Alternative Text:</h5> <p id="alttext' + data[i].carouselid.toString() + '">' + data[i].alttext.toString() + '</p>' +
                                            '<h5>Image Link URL:</h5> <p id="imagelinkurl' + data[i].carouselid.toString() + '">' + data[i].imagelinkurl.toString() + '</p>' +
                                            '<h5>Display Order:</h5> <p id="displayorder' + data[i].carouselid.toString() + '">' + data[i].displayorder.toString() + '</p>' +
                                            '<h5>Active:</h5> <p id="active' + data[i].carouselid.toString() + '">' + data[i].active.toString() + '</p>' +
                                            '<h5>Start Date:</h5> <p id="displaydate_start' + data[i].carouselid.toString() + '">' + data[i].displaydate_start.toString() + '</p>' +
                                            '<h5>End Date:</h5> <p id="displaydate_end' + data[i].carouselid.toString() + '">' + data[i].displaydate_end.toString() + '<br/><br/>' + '</p>' +
                                            '<input type="button" value="Edit" onclick="EditSpecificDiv(' + data[i].carouselid.toString() + ');" />' +
                                         '</div>')
                i++;
            }
        })

        .fail(function (jqXHR, textStatus, errorThrown) {

        });
});

function EditSpecificDiv(carouselid) {
    var dropdownNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
    var amountOfUsedNumbers = $('#dropdownNumbers').text().split(',').length
    var i = 0;
    while (i < amountOfUsedNumbers) {
        if ($('#dropdownNumbers').text().split(',')[i] != carouselid) {
            var index = dropdownNumbers.indexOf(parseInt($('#dropdownNumbers').text().split(',')[i]))
            if (index > -1) {
                dropdownNumbers.splice(index, 1);
            }
            i++;
        }
        else {
            i++;
        }
    }

    var imageurl = $("#imageurl" + carouselid).text();
    var alttext = $("#alttext" + carouselid).text();
    var imagelinkurl = $("#imagelinkurl" + carouselid).text();
    var displayorder = $("#displayorder" + carouselid).text();
    var active = $("#active" + carouselid).text();
    var displaydate_start = $("#displaydate_start" + carouselid).text();
    var displaydate_end = $("#displaydate_end" + carouselid).text();
    var pause = 1;

    var select = $('<select id="ViewOrderSelect" />');

    for (var val in dropdownNumbers) {
        $('<option />', { value: dropdownNumbers[val], text: dropdownNumbers[val] }).appendTo(select);
    }

    $('#ViewOrderSelect')

    $('#MainInfoDiv').empty();
    $('#MainInfoDiv').append('<div id="Carousel' + carouselid + 'Div" class="CarouselItem">' +
                                '<h3>' + carouselid + ' - Edit Mode</h3>' +
                                '<h5>Image URL:</h5> <p id="imageurl' + carouselid + '">' + imageurl + '</p>' +
                                '<h5>Alternative Text:</h5> <input class="carouseleditbox" id="alttext' + carouselid + '" name="alttext' + carouselid + '" type="text" value="' + alttext + '">' +
                                '<h5>Image Link URL:</h5> <p id="imagelinkurl' + carouselid + '">' + imagelinkurl + '</p>' +
                                '<h5>Display Order:</h5> <div id="ViewOrderDiv"></div>' +
                                '<h5>Active:</h5> <p id="active' + carouselid + '">' + active + '</p>' +
                                '<h5>Start Date:</h5> <input class="carouseleditbox" id="displaydate_start' + carouselid + '" name="displaydate_start' + carouselid + '" type="text" value="' + displaydate_start + '">' +
                                '<h5>End Date:</h5> <input class="carouseleditbox" id="displaydate_end' + carouselid + '" name="displaydate_end' + carouselid + '" type="text" value="' + displaydate_end + '"><br/><br/>' +
                                '<input type="button" value="Save" onclick="SaveSpecificDiv(' + carouselid + ')" />' +
                             '</div>')

    select.appendTo('#ViewOrderDiv');
    $('#ViewOrderSelect option[value="' + displayorder + '"]').attr("selected", true);
}

function SaveSpecificDiv(carouselid) {
    $.ajax({
        type: "Post",
        url: "/CMS/SaveHeaderAdvertCarouselData",
        data: { 'Id': carouselid, 'AltText': $('#alttext' + carouselid).val(), 'DisplayOrder': $('#ViewOrderSelect').val(), 'StartDate': $('#displaydate_start' + carouselid).val(), 'EndDate': $('#displaydate_end' + carouselid).val() },
        dataType: "json"
    })
    .done(function (data) {
        if(data = "success")
        {
            document.location.reload()
        }
        
    })
}


//<input id="Director" name="Director" type="text" value="">