// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var fileExtension = ['csv', 'xlsx'];


var customElement = $("<div>", {
    "css": {
        "text-align": "center"
    },
    "class": "spinner-border text-danger spinner-border-page-load"
});

var singleFieldLoader = $("<div>", {
    "css": {
        "text-align": "center"
    },
    "class": "spinner-border spinner-border-sm text-danger"
});

var viewLoader_blank = $("<div>", {
    "css": {
        "text-align": "center"
    },
    "class": "spinner-grow spinner-grow-blank text-danger"
}); 

var viewLoader_4rem = $("<div>", {
    "css": {
        "text-align": "center"
    },
    "class": "spinner-border spinner-4rem text-danger"
}); 

$(document).ready(function () {

    viewLoginUserDetails();

    viewAdminAccessModules();
    viewGranularAccessModules();
    viewSoaAccessModules();
    viewPaymentAccessModules();
    viewReportsAccessModules();

    (function () {
        const idleDurationSecs = 900;    // X number of seconds //15 mins 
        const redirectUrl = window.location.origin; // Redirect idle users to this URL
        let idleTimeout; // variable to hold the timeout, do not modify

        const resetIdleTimeout = function () {

            // Clears the existing timeout
            if (idleTimeout) clearTimeout(idleTimeout);

            // Set a new idle timeout to load the redirectUrl after idleDurationSecs
            idleTimeout = setTimeout(() => location.href = redirectUrl, idleDurationSecs * 1000);
        };

        // Init on page load
        resetIdleTimeout();

        // Reset the idle timeout on any of the events listed below
        ['click', 'touchstart', 'mousemove'].forEach(evt =>
            document.addEventListener(evt, resetIdleTimeout, false)
        );

    })();

});

function viewLoginUserDetails(options = null) {
    $.ajax({
        url: '/Home/ViewLoginUserDetails'
        , type: 'POST'
        , contentType: 'application/json; charset=utf-8;'
        , dataType: 'html'
        , success: function (response) {
            $('#divResults-loginUserDetails').html(response);
        }
        , complete: function () {

        }
    });
}

function viewAdminAccessModules(options = null) {
    $.ajax({
        url: '/Home/ViewAdminAccessModules'
        , type: 'POST'
        , contentType: 'application/json; charset=utf-8;'
        , dataType: 'html'
        , success: function (response) {
            $('#divResults-adminAccessModules').html(response);
        }
        , complete: function () {

        }
    });
}

function viewGranularAccessModules(options = null) {
    $.ajax({
        url: '/Home/ViewGranularAccessModules'
        , type: 'POST'
        , contentType: 'application/json; charset=utf-8;'
        , dataType: 'html'
        , success: function (response) {
            $('#divResults-granularAccessModules').html(response);
        }
        , complete: function () {

        }
    });
}

function viewSoaAccessModules(options = null) {
    $.ajax({
        url: '/Home/ViewSoaAccessModules'
        , type: 'POST'
        , contentType: 'application/json; charset=utf-8;'
        , dataType: 'html'
        , success: function (response) {
            $('#divResults-soaAccessModules').html(response);
        }
        , complete: function () {

        }
    });
}

function viewPaymentAccessModules(options = null) {
    $.ajax({
        url: '/Home/ViewPaymentAccessModules'
        , type: 'POST'
        , contentType: 'application/json; charset=utf-8;'
        , dataType: 'html'
        , success: function (response) {
            $('#divResults-paymentAccessModules').html(response);
        }
        , complete: function () {

        }
    });
}

function viewReportsAccessModules(options = null) {
    $.ajax({
        url: '/Home/ViewReportsAccessModules'
        , type: 'POST'
        , contentType: 'application/json; charset=utf-8;'
        , dataType: 'html'
        , success: function (response) {
            $('#divResults-reportsAccessModules').html(response);
        }
        , complete: function () {

        }
    });
}

