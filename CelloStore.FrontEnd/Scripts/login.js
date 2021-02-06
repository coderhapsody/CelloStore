var login = {
    doLogin: function() {
        var username = $("#WindowUserName").val();
        var password = $("#WindowPassword").val();
        var loginButton = $("#LoginButton").kendoButton().data("kendoButton");
        loginButton.enable(false);
        $("#loginProgress").show();
        $.ajax({
            type: "POST",
            url: '@Url.Action("Login", "Home")',
            data: "UserName=" + username + "&Password=" + password,
            success: function(result) {
                if (result) {
                    document.location.href = "@Url.Action("Index","Home")";
                } else {
                    loginButton.enable(true);
                    $("#loginProgress").hide();
                    $("#WindowUserName").val("");
                    $("#WindowPassword").val("");
                    alert("Invalid username or password");
                    $("#WindowUserName").focus();
                    return false;
                }
            },
            error: function(data) {
                alert("Error when trying to log in");
                return false;
            }
        });
        }
};