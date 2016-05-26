window.onload = function () {
    var thisint = parseInt($('#PermissionLevel').val());
    if (thisint >= 65) {
        $('#SkuStockLevelEditButton').removeClass('Hidden');
    }

};