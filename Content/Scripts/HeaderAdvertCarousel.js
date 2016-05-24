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
            var dropdownNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
            var amountOfUsedNumbers = $('#dropdownNumbers').text().split(',').length
            var i = 0;

            while (i < amountOfUsedNumbers) {
                    var index = dropdownNumbers.indexOf(parseInt($('#dropdownNumbers').text().split(',')[i]))
                    if (index > -1) {
                        dropdownNumbers.splice(index, 1);
                    }
                    i++;
            }
            var select2 = $('<select id="NewViewOrderSelect" />');

            $('<option />', { value: 0, text: "Please Select One" }).appendTo(select2);

            for (var val in dropdownNumbers) {
                $('<option />', { value: dropdownNumbers[val], text: dropdownNumbers[val] }).appendTo(select2);
            }

            select2.appendTo('#NewViewOrderDiv');

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


//Modal area

function SaveNewCarouselItem() {
    var pause = 1;

    if ($('#NewAltText').val() == "")
    {
        return false;
    }
    if ($('#NewViewOrderSelect').val() == "0") {
        return false;
    }
    if ($('#NewStartDate').val() == "") {
        return false;
    }
    if ($('#NewEndDate').val() == "") {
        return false;
    }
    if ($('#NewStartTime').val() == "") {
        return false;
    }
    if ($('#NewEndTime').val() == "") {
        return false;
    }

    var fullStartDate = $('#NewStartDate').val() + " " + $('#NewStartTime').val() + ":00";
    var fullEndDate = $('#NewEndDate').val() + " " + $('#NewEndTime').val() + ":00";

    $.ajax({
        type: "Post",
        url: "/CMS/SaveNewAdvertCarouselData",
        data: { 'altText': $('#NewAltText').val(), 'displayOrder': $('#NewViewOrderSelect').val(), 'startDate': $('#NewStartDate').val(), 'endDate': $('#NewEndDate').val() },
        dataType: "json"
    })
    .done(function (data) {
        if (data = "success") {
            document.location.reload()
        }

    })
}

// Original JavaScript code by Chirp Internet: www.chirp.com.au
// Please acknowledge use of this code by including this header.

var checkForm = function (e) {
    var form = (e.target) ? e.target : e.srcElement;
};

var modal_init = function () {

    var modalWrapper = document.getElementById("modal_wrapper");
    var modalWindow = document.getElementById("modal_window");

    var openModal = function (e) {
        modalWrapper.className = "overlay";
        var overflow = modalWindow.offsetHeight - document.documentElement.clientHeight;
        if (overflow > 0) {
            modalWindow.style.maxHeight = (parseInt(window.getComputedStyle(modalWindow).height) - overflow) + "px";
        }
        modalWindow.style.marginTop = (-modalWindow.offsetHeight) / 2 + "px";
        modalWindow.style.marginLeft = (-modalWindow.offsetWidth) / 2 + "px";
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
    };

    var closeModal = function (e) {
        modalWrapper.className = "";
        e.preventDefault ? e.preventDefault() : e.returnValue = false;
    };

    var clickHandler = function (e) {
        if (!e.target) e.target = e.srcElement;
        if (e.target.tagName == "DIV") {
            if (e.target.id != "modal_window") closeModal(e);
        }
    };

    var keyHandler = function (e) {
        if (e.keyCode == 27) closeModal(e);
    };

    if (document.addEventListener) {
        document.getElementById("modal_open").addEventListener("click", openModal, false);
        document.getElementById("modal_close").addEventListener("click", closeModal, false);
        document.addEventListener("click", clickHandler, false);
        document.addEventListener("keydown", keyHandler, false);
    } else {
        document.getElementById("modal_open").attachEvent("onclick", openModal);
        document.getElementById("modal_close").attachEvent("onclick", closeModal);
        document.attachEvent("onclick", clickHandler);
        document.attachEvent("onkeydown", keyHandler);
    }

};

if (document.addEventListener) {
    document.getElementById("modal_feedback").addEventListener("submit", checkForm, false);
    document.addEventListener("DOMContentLoaded", modal_init, false);
} else {
    document.getElementById("modal_feedback").attachEvent("onsubmit", checkForm);
    window.attachEvent("onload", modal_init);
}