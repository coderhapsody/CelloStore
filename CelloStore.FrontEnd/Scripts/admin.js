var popUpWindow = (function () {
    return {
        show: function (url) {
            var newWindow = window.open(url, null, 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no,resizable=yes,width=930,height=600,scrollbars=yes,toolbar=no,status=no,left=50,top=50');
            if (window.focus) {
                newWindow.focus();
            }
        }
    }
})();