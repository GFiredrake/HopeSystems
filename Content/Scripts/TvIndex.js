window.onload = function () {
    var thisint = parseInt($('#PermissionLevel').val());
    if (thisint >= 40) {
        $('#SkuEditButton').removeClass('Hidden');
    }
    parseInt($('#PermissionLevel').val())
};

