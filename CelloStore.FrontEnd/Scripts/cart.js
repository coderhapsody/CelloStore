var cart = (function (cart) {

    cart.countItemsInCart = function (countAjaxUrl) {
        $.ajax({
            url: countAjaxUrl,
            type: "GET",
            success: function (result) {
                if (result == undefined)
                    result = 0;
                $("#itemCount").text(result);
            }
        });
    };

    cart.addToCart = function(productCode, qty, areaName, ajaxUrl, countAjaxUrl) {
        var parameters = {
            productCode: productCode,
            qty: qty,
            areaName : areaName
        };
        $.ajax({
            type: "POST",
            url: ajaxUrl,
            data: parameters,
            success: function(result) {
                cart.countItemsInCart(countAjaxUrl);
                appAlert.showSuccess("Product " + productCode + " has been added to cart");
            }
        });
    };

    cart.removeFromCart = function(lineNo, ajaxUrl, countAjaxUrl) {
        var parameters = {
            lineNo: lineNo            
        };
        $.ajax({
            type: "POST",
            url: ajaxUrl,
            data: parameters,
            async: false,
            success: function (result) {
                cart.countItemsInCart(countAjaxUrl);                
            }
        });
    };

    return cart;
})(cart || {});


var appAlert = (function(appAlert) {
    appAlert.showSuccess = function(message) {
        var notification = $("#notification").kendoNotification({
            stacking: "up",
            button: true,
            position: {
                top: 20,
                right: 20
            }
        }).data("kendoNotification");

        notification.show(message, "success");
    };

    appAlert.showWarning = function(message) {
        var notification = $("#notification").kendoNotification({
            stacking: "up",
            button: true,
            position: {
                bottom: 20,
                right: 20
            }
        }).data("kendoNotification");

        notification.show(message, "warning");
    };

    appAlert.showInfo = function(message) {
        var notification = $("#notification").kendoNotification({
            stacking: "up",
            button: true,
            position: {
                bottom: 20,
                right: 20
            }
        }).data("kendoNotification");

        notification.show(message, "info");
    };

    return appAlert;
})(appAlert || {});

$(function() {
    cart.countItemsInCart(appVars.cartUrl);
});