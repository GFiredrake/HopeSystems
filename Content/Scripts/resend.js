﻿$(function () {
    $('form').submit(function () {
        $('input[type="submit"]', this).attr('disabled', 'disabled');
    });
});