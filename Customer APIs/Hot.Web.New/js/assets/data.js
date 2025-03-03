var currentSales = 0.00;
var xTellerUsers = [];
var xBrandIDs = [];
var xStatusIDs = [];
var xTransactions;
var xSubscribers = [];
var xBundles = {} ;
var xBundlesUSD = {} ;
var xTeloneBundles = {};
var xPins = [];
var xNetoneAirtimeCodes = {
    0.5: "UA05",
    1: "UA1",
    2: "UA2",
    5: "UA5",
    10: "UA10", 
    20: "UA20",
    50: "UA50",
};
var Token = "";


function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}
function siteCookie(item) {
    var cookie = $.cookie('HOTLogin');
    try {
        var subcookiesarray = cookie.split("&");
        switch (item) {
            case "UID":
                return subcookiesarray[0].split("=")[1];
                break;
            case "AccountID":
                return subcookiesarray[1].split("=")[1];
                break;
            case "HOTAccessID":
                return subcookiesarray[2].split("=")[1];
                break;
            case "AccessID":
                return subcookiesarray[2].split("=")[1];
                break;
            case "Pwd":
                return subcookiesarray[3].split("=")[1];
                break;
            case "Retail":
                return cookie.includes("Retail");
                break;
            case "NoRechargeUI":
                return cookie.includes("NoRechargeUI");
                break;
            default:
                return ""
        };
    }
    catch (ex) {
        if (!(item == "Retail"  || item == "NoRechargeUI")) CheckLogin();
    };
}
function isPasswordRequired() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnPasswordRequired";
    var parameters = "{'AccessID':'" + siteCookie("AccessID") + "'}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d || '';
            data = (data == "true");
            $(".txtPasswordRequired")[0].checked = (data);

        },
        error: function (emsg) {
            console.log(parameters);
            console.log(emsg);

        }
    });

};
function LoadBalance() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnGetBalance";
    var parameters = "{'AccountID':'" + siteCookie("AccountID") + "' , 'AccessID':'" + siteCookie("AccessID") + "'}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            data = ($.isNumeric(data) ? "$ " + parseFloat(Math.round(data * 100) / 100).toFixed(2) : data);
            $("#txtBalance").html(data);
        },
        error: function (emsg) {
            console.log(parameters);
            console.log(emsg);

        }
    });

};
function updateSales() {
    data = ($.isNumeric(currentSales) ? parseFloat(Math.round(currentSales * 100) / 100).toFixed(2) : 0);
    $("#txtTotalSales").html(data);
};
function LoadSales() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnGetTotalSales";
    var parameters = "{'AccessID':'" + siteCookie("AccessID") + "'}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            // console.log(msg);
            try {
                var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            } catch (ex) { data = msg };
            console.log(data);
            data = ($.isNumeric(data) ? parseFloat(Math.round(data * 100) / 100).toFixed(2) : data);
            currentSales = parseFloat($.isNumeric(data) ? parseFloat(data) : 0);
            $("#txtTotalSales").html(data);
        },
        error: function (emsg) {
            console.log(parameters);
            console.log(emsg);

        }
    });

};

function Recharge(Rechargetable, SubscriberName, TargetMobile, rAmount, AccessCode, Password) {
    Recharge_New(Rechargetable, SubscriberName, TargetMobile, rAmount, AccessCode, Password);

    /*
    // Switcher for System While Transitioning to new Webservice
  
    if (TargetMobile.substring(0, 5) == '08644') {
        Recharge_Old(Rechargetable, SubscriberName, TargetMobile, rAmount, AccessCode, Password);
    } else {
        Recharge_New(Rechargetable, SubscriberName, TargetMobile, rAmount, AccessCode, Password);
    };
    */
}

function Recharge_Old(Rechargetable, SubscriberName, TargetMobile, rAmount, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnRecharge";
    var parameters = "{'AccessCode':'" + AccessCode + "','AccessPassword':'" + Password + "','TargetMobile':'" + TargetMobile + "','Amount':" + rAmount + "}";
    var rechargeid = String(Math.floor(Math.random() * (20001)) + 10000);
    rAmount = parseFloat(Math.round(rAmount * 100) / 100).toFixed(2);
    console.log(webMethod);
    console.log(parameters);
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            $(Rechargetable).after('<tr><th class="place-left">' + SubscriberName + '</th><th>' + number_phone(TargetMobile) + '</th><th>' + rAmount + '</th><th id="' + rechargeid + '"><img src="images/preloader-w8-cycle-black.gif" width="16" /></th></tr> ');
        },
        success: function (msg) {
            var result = msg.d.replace(/"/g, '');
            result = result.replace(/;/g, '<br/>');

            if (result.substring(0, 3).replace(/[^0-9]/g, "").length > 0) {

                var retrylink = ' <a href="javascript:retry(\'' + rAmount + '\',\'' + TargetMobile + '\',\'' + rechargeid + '\');"><i class="icon-loop" title="Retry"/></i></a>';
                var messagebox = "<a href=\"javascript: showalert('Recharge Error Information','" + result.substring(4) + "');\"><i class='icon-warning fg-color-orange'></i></a>";
                result = messagebox + retrylink;
            } else {
                result = '<i class="icon-checkmark fg-color-greenDark"></i>';
                currentSales += parseFloat(rAmount);
                updateSales();
            };

            $("#" + rechargeid).html(result);
        },
        error: function (e) {
            var messagebox = "<a href=\"javascript: showalert('Recharge Status Failure','Status Unavailable for " + TargetMobile + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
            $("#" + rechargeid).html(messagebox);
        }
    });
}
function Recharge_New(Rechargetable, SubscriberName, TargetMobile, rAmount, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-pinless";

    rAmount = parseFloat(Math.round(rAmount * 100) / 100).toFixed(2);
    var parameters = "{'targetMobile':'" + TargetMobile + "','amount':" + rAmount + "}";
    var rechargeid = 'Web' + guid();


    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $(Rechargetable).after('<tr><th class="place-left">' + SubscriberName + '</th><th>' + number_phone(TargetMobile) + '</th><th>' + rAmount + '</th><th id="' + rechargeid + '"><img src="images/preloader-w8-cycle-black.gif" width="16" /></th></tr> ');
        },
        success: function (msg) {
            //      console.log(msg);
            var result = '<i class="icon-checkmark fg-color-greenDark"></i>';
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                currentSales += parseFloat(rAmount);
                updateSales();
            } catch (ex) { console.log(ex); };

            $("#" + rechargeid).html(result);
        },
        error: function (e) {
            //    console.log(e);
            var msg = e.responseText;
            var messagebox = "";
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                var retrylink = ' <a href="javascript:retry(\'' + rAmount + '\',\'' + TargetMobile + '\',\'' + rechargeid + '\');"><i class="icon-loop" title="Retry"/></i></a>';
                messagebox = "<a href=\"javascript: showalert('Recharge Error Information','" + data.ReplyMessage + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
                messagebox += retrylink;
            } catch (ex) {
                messagebox = "<a href=\"javascript: showalert('Recharge Status Failure','Status Unavailable for " + TargetMobile + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
            };
            $("#" + rechargeid).html(messagebox);
        }
    });
}
function Recharge_USD(Rechargetable, SubscriberName, TargetMobile, rAmount, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-pinless-usd";

    rAmount = parseFloat(Math.round(rAmount * 100) / 100).toFixed(2);
    var parameters = "{'targetMobile':'" + TargetMobile + "','amount':" + rAmount + "}";
    var rechargeid = 'Web' + guid();


    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $(Rechargetable).after('<tr><th class="place-left">' + SubscriberName + '</th><th>' + number_phone(TargetMobile) + '</th><th>' + rAmount + '</th><th id="' + rechargeid + '"><img src="images/preloader-w8-cycle-black.gif" width="16" /></th></tr> ');
        },
        success: function (msg) {
            //      console.log(msg);
            var result = '<i class="icon-checkmark fg-color-greenDark"></i>';
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                currentSales += parseFloat(rAmount);
                updateSales();
            } catch (ex) { console.log(ex); };

            $("#" + rechargeid).html(result);
        },
        error: function (e) {
            //    console.log(e);
            var msg = e.responseText;
            var messagebox = "";
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                var retrylink = ' <a href="javascript:retry(\'' + rAmount + '\',\'' + TargetMobile + '\',\'' + rechargeid + '\');"><i class="icon-loop" title="Retry"/></i></a>';
                messagebox = "<a href=\"javascript: showalert('Recharge Error Information','" + data.ReplyMessage + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
                messagebox += retrylink;
            } catch (ex) {
                messagebox = "<a href=\"javascript: showalert('Recharge Status Failure','Status Unavailable for " + TargetMobile + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
            };
            $("#" + rechargeid).html(messagebox);
        }
    });
}
function Recharge_Data(Rechargetable, SubscriberName, TargetMobile, Productcode, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-data";

    var parameters = "{'targetMobile':'" + TargetMobile + "','productcode':'" + Productcode + "'}";
    var rechargeid = 'Web' + guid();

    //console.log(webMethod);
    //console.log(parameters);

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $(Rechargetable).after('<tr><th class="place-left">' + SubscriberName + '</th><th>' + number_phone(TargetMobile) + '</th><th>' + Productcode + '</th><th id="' + rechargeid + '"><img src="images/preloader-w8-cycle-black.gif" width="16" /></th></tr> ');
        },
        success: function (msg) {
            //      console.log(msg);
            var result = '<i class="icon-checkmark fg-color-greenDark"></i>';
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                currentSales += parseFloat(rAmount);
                updateSales();
            } catch (ex) { console.log(ex); };

            $("#" + rechargeid).html(result);
        },
        error: function (e) {
            //    console.log(e);
            var msg = e.responseText;
            var messagebox = "";
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                var retrylink = ' <a href="javascript:retrydata(\'' + Productcode + '\',\'' + TargetMobile + '\',\'' + rechargeid + '\');"><i class="icon-loop" title="Retry"/></i></a>';
                messagebox = "<a href=\"javascript: showalert('Recharge Error Information','" + data.ReplyMessage + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
                messagebox += retrylink;
            } catch (ex) {
                messagebox = "<a href=\"javascript: showalert('Recharge Status Failure','Status Unavailable for " + TargetMobile + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
            };
            $("#" + rechargeid).html(messagebox);
        }
    });
}
function Recharge_DataUSD(Rechargetable, SubscriberName, TargetMobile, Productcode, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-data-usd";

    var parameters = "{'targetMobile':'" + TargetMobile + "','productcode':'" + Productcode + "'}";
    var rechargeid = 'Web' + guid();

    //console.log(webMethod);
    //console.log(parameters);

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $(Rechargetable).after('<tr><th class="place-left">' + SubscriberName + '</th><th>' + number_phone(TargetMobile) + '</th><th>' + Productcode + '</th><th id="' + rechargeid + '"><img src="images/preloader-w8-cycle-black.gif" width="16" /></th></tr> ');
        },
        success: function (msg) {
            //      console.log(msg);
            var result = '<i class="icon-checkmark fg-color-greenDark"></i>';
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                currentSales += parseFloat(rAmount);
                updateSales();
            } catch (ex) { console.log(ex); };

            $("#" + rechargeid).html(result);
        },
        error: function (e) {
            //    console.log(e);
            var msg = e.responseText;
            var messagebox = "";
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                var retrylink = ' <a href="javascript:retrydata(\'' + Productcode + '\',\'' + TargetMobile + '\',\'' + rechargeid + '\');"><i class="icon-loop" title="Retry"/></i></a>';
                messagebox = "<a href=\"javascript: showalert('Recharge Error Information','" + data.ReplyMessage + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
                messagebox += retrylink;
            } catch (ex) {
                messagebox = "<a href=\"javascript: showalert('Recharge Status Failure','Status Unavailable for " + TargetMobile + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
            };
            $("#" + rechargeid).html(messagebox);
        }
    });
}
function Recharge_Pins(Rechargetable, SubscriberName, TargetMobile, brandid, rAmount, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-evd";
    rAmount = parseFloat(Math.round(rAmount * 100) / 100).toFixed(2);
    //"{'BrandID':" + brandid + ", 'Denomination':" + ramount + ", 'Quantity':1,'TargetNumber':'" + TargetMobile + "'}"
    var parameters = "{'BrandID':" + parseInt(brandid) + ", 'Denomination':" + rAmount + ", 'Quantity':1,'TargetNumber':'" + TargetMobile + "'}";
    var rechargeid = 'Web' + guid();

    //console.log(webMethod);
    //console.log(parameters);

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $(Rechargetable).after('<tr><th class="place-left">' + SubscriberName + '</th><th>' + number_phone(TargetMobile) + '</th><th>' + rAmount + '</th><th id="' + rechargeid + '"><img src="images/preloader-w8-cycle-black.gif" width="16" /></th></tr> ');
        },
        success: function (msg) {
            //      console.log(msg);
            var result = '<i class="icon-checkmark fg-color-greenDark"></i>';
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                currentSales += parseFloat(rAmount);
                updateSales();
            } catch (ex) { console.log(ex); };

            $("#" + rechargeid).html(result);
        },
        error: function (e) {
            //    console.log(e);
            var msg = e.responseText;
            var messagebox = "";
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                var retrylink = ' <a href="javascript:retrydata(\'' + Productcode + '\',\'' + TargetMobile + '\',\'' + rechargeid + '\');"><i class="icon-loop" title="Retry"/></i></a>';
                messagebox = "<a href=\"javascript: showalert('Recharge Error Information','" + data.ReplyMessage + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
                messagebox += retrylink;
            } catch (ex) {
                messagebox = "<a href=\"javascript: showalert('Recharge Status Failure','Status Unavailable for " + TargetMobile + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
            };
            $("#" + rechargeid).html(messagebox);
        }
    });
}
function Recharge_USDEVD(Rechargetable, SubscriberName, TargetMobile, brandid, rAmount, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-evd-usd";
    rAmount = parseFloat(Math.round(rAmount * 100) / 100).toFixed(2);
    var parameters = "{'BrandID':" + parseInt(brandid) + ", 'Denomination':" + parseFloat(rAmount) + ", 'Quantity':1,'TargetNumber':'" + TargetMobile + "'}";
    var rechargeid = 'Web' + guid();

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $(Rechargetable).after('<tr><th class="place-left">' + SubscriberName + '</th><th>' + number_phone(TargetMobile) + '</th><th>' + rAmount + '</th><th id="' + rechargeid + '"><img src="images/preloader-w8-cycle-black.gif" width="16" /></th></tr> ');
        },
        success: function (msg) {
            //      console.log(msg);
            var result = '<i class="icon-checkmark fg-color-greenDark"></i>';
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                currentSales += parseFloat(rAmount);
                updateSales();
            } catch (ex) { console.log(ex); };

            $("#" + rechargeid).html(result);
        },
        error: function (e) {
            //    console.log(e);
            var msg = e.responseText;
            var messagebox = "";
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg)
                var retrylink = ' <a href="javascript:retrydata(\'' + ramount + '\',\'' + TargetMobile + '\',\'' + rechargeid + '\');"><i class="icon-loop" title="Retry"/></i></a>';
                messagebox = "<a href=\"javascript: showalert('Recharge Error Information','" + data.ReplyMessage + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
                messagebox += retrylink;
            } catch (ex) {
                messagebox = "<a href=\"javascript: showalert('Recharge Status Failure','Status Unavailable for " + TargetMobile + "');\" ><i class='icon-warning fg-color-orange'></i></a>"
            };
            $("#" + rechargeid).html(messagebox);
        }
    });
}


function checkStatusOnline() {
    $.ajax({
        type: "POST",
        url: document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnOnlineCheck",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);

            switch (data.Status) {
                case "System Offline":
                    $("#txtStatusHead").addClass("fg-color-redLight").removeClass("fg-color-orange fg-color-greenLight");
                    $("#txtStatusIcon").addClass("icon-cancel ").removeClass("icon-info icon-help");
                    break;
                case "Currently Online":
                    $("#txtStatusHead").html(data.Status).addClass("fg-color-greenLight").removeClass("fg-color-orange fg-color-redLight");
                    $("#txtStatusIcon").addClass("icon-info ").removeClass("icon-cancel icon-help");
                    break;
                default:
                    $("#txtStatusHead").html("Currently Unavailable").addClass("fg-color-orange").removeClass("fg-color-redLight fg-color-greenLight");
                    $("#txtStatusIcon").addClass("icon-help ").removeClass("icon-info icon-cancel");
            }
            $("#txtStatusHead").html(data.Status)
            $("#txtStatusMessage").html(data.Message);

        },
        error: function (emsg) {

        }
    });

};
function checkStatusOnlineDetails() {
    $.ajax({
        type: "POST",
        url: document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnOnlineCheckDetailed",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = '', status = '', errors = 0;
            try {

                jQuery.each(data, function (rec) {
                    switch (this.StatusID) {
                        case 0:
                            status = 'class="icon-checkmark"';
                            break;
                        case 1:
                            status = 'class="icon-warning"';
                            errors += 1;
                            break;

                        default:
                            status = 'class="icon-help"';
                    }
                    t = t + '<li>' + this.Name + '<i ' + status + '></i></li>';
                });
                $("#networkStatus").html(t);
                switch (errors) {
                    case 0:
                        $("#networkStatusBox").removeClass("bg-color-orangeDark").addClass("bg-color-green");
                        break;
                    case 1:
                        $("#networkStatusBox").removeClass("bg-color-orangeDark").addClass("bg-color-orange");
                        break;
                    case 2:
                        $("#networkStatusBox").removeClass("bg-color-orangeDark").addClass("bg-color-orange");
                        break;
                    default:
                        $("#networkStatusBox").removeClass("bg-color-orangeDark").addClass("bg-color-red");
                        break;
                }
                //$("#networkStatus").removeClass(s);
            } catch (ex) {
                $("#txtStatusHead").html("System Offline")
                $("#txtStatusHead").addClass("fg-color-redLight").removeClass("fg-color-orange fg-color-greenLight");
                $("#txtStatusIcon").addClass("icon-cancel ").removeClass("icon-info icon-help");
                $("#networkStatusBox").removeClass("bg-color-orangeDark").addClass("bg-color-red");
            }
        },
        error: function (emsg) {

        }
    });

};
function isRegistered(email) {

    console.log("Started");
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnIsRegistered";
    var parameters = "{'AccessCode':'" + $(email).val() + "'}";

    return $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        timeout: 2000,
        tryCount: 0,
        retryLimit: 2,
        beforeSend: function (xhr) {
            $("#imgLoading").css("display", "block");
        },
        success: function (msg) {
            $(email).removeClass("checked-email").removeClass("registered").removeClass("unregistered");
            console.log(msg);
            $(email).addClass("checked-email");
            if (msg.d == '"registered"') {
                $(email).addClass("registered");
            } else {
                $(email).addClass("unregistered");
            };

        },
        error: function (xhr, textStatus, errorThrown) {
            if (textStatus == 'timeout') {
                this.tryCount++;
                console.log(this.tryCount);
                if (this.tryCount <= this.retryLimit) {
                    //try again
                    $.ajax(this);
                    return;
                }

            }
            $(email).removeClass("checked-email").removeClass("registered").removeClass("unregistered");
            $(email).addClass("checked-email").addClass("check-failed");
            console.log(emsg);
        },
        complete: function (msg) {
            $("#imgLoading").css("display", "none");
            form.valid();
        }
    });
};

/* Account Functions */
function loadaccountinfo() {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSelectAccount";
    var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d)[0] || $.parseJSON(msg.d)[0];

            $("#txtAccountName").attr("value", data.Account.AccountName);
            $("#txtAddress1").attr("value", data.Address.Address1);
            $("#txtAddress2").attr("value", data.Address.Address2);
            $("#txtCity").attr("value", data.Address.City);
            $("#txtNationalID").attr("value", data.Account.NationalID);
            $("#txtVatNumber").attr("value", data.Address.VatNumber);
            $("#txtContactName").attr("value", data.Address.ContactName);
            $("#txtContactNumber").attr("value", data.Address.ContactNumber);
            $("#txtProfileName").html(data.Account.Profile.ProfileName);
            $("#txtEmail").attr("value", data.Account.Email);
            $("#txtBalance").html(($.isNumeric(data.Account.Balance) ? "$ " + parseFloat(Math.round(data.Account.Balance * 100) / 100).toFixed(2) : data.Account.Balance));

        },
        error: function (emsg) {
            console.log(emsg);
        }
    });

};
function loadUpdateAccountInfo() {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSelectAccount";
    var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d)[0] || $.parseJSON(msg.d)[0];

            $("#atxtAccountName").html(data.Account.AccountName);
            $("#atxtAddress1").html(data.Address.Address1);
            $("#atxtAddress2").html(data.Address.Address2);
            $("#atxtCity").html(data.Address.City);
            $("#atxtNationalID").html(data.Account.NationalID);
            $("#atxtVatNumber").html(data.Address.VatNumber);
            $("#atxtContactName").html(data.Address.ContactName);
            $("#atxtContactNumber").html(data.Address.ContactNumber);
            $("#atxtProfileName").html(data.Account.Profile.ProfileName);
            $("#atxtEmail").html(data.Account.Email);

        },
        error: function (emsg) {
            console.log(emsg);
        }
    });

};
function saveaccountinfo() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSaveAccount";
    var AccountName = $("#txtAccountName").val();
    var Address1 = $("#txtAddress1").val();
    var Address2 = $("#txtAddress2").val();
    var City = $("#txtCity").val();
    var NationalID = $("#txtNationalID").val();
    var VatNumber = $("#txtVatNumber").val();
    var ContactName = $("#txtContactName").val();
    var ContactNumber = $("#txtContactNumber").val();
    var Email = $("#txtEmail").val();
    var parameters = "{'AccountID':'" + siteCookie("AccountID") + "', 'AccountName':'" + AccountName +
        "','Address1':'" + Address1 + "','Address2':'" + Address2 + "','City':'" + City + "','NationalID':'" + NationalID +
        "','VatNumber':'" + VatNumber + "','ContactName':'" + ContactName + "','ContactNumber':'" + ContactNumber + "','Email':'" + Email + "'}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log(msg);
            lockaccountinfo();
            shownotification('Account Information', 'Account information saved successfully');
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });
}

/* Access User Functions */
function addUserAccess(Username, ChannelID, AccessCode, AccessPassword, SalesPassword) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnAddAccessUser";
    var AccountID = siteCookie("AccountID");
    var CurrentAccessCode = siteCookie("UID");
    var Password = siteCookie("Pwd");

    var parameters = "{'AccountID':'" + AccountID + "', 'MainAccessCode':'" + CurrentAccessCode + "','Password':'" + Password +
        "','Username':'" + Username + "','ChannelID':'" + ChannelID + "','AccessCode':'" + AccessCode + "','AccessPassword':'" + AccessPassword + "','SalesPassword':'" + SalesPassword + "'}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == '"Success"') {
                loadusers();
                shownotification('Add New User', Username + ' added successfully');
                return true;
            } else {
                showalert('Add New User', msg.d);
                return false;
            };
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });
};
function SaveUserAccess(Username, ChannelID, AccessCode, AccessPassword, AccessID, SalesPassword) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSaveAccessUser";
    var AccountID = siteCookie("AccountID");
    var CurrentAccessCode = siteCookie("UID");
    var Password = siteCookie("Pwd");

    var parameters = "{'AccountID':'" + AccountID + "', 'MainAccessCode':'" + CurrentAccessCode + "','Password':'" + Password +
        "','Username':'" + Username + "','ChannelID':'" + ChannelID + "','AccessCode':'" + AccessCode + "','AccessPassword':'" + AccessPassword + "','AccessID':'" + AccessID + "','SalesPassword':'" + SalesPassword + "'}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == '"Success"') {
                loadusers();
                shownotification('Edit User', Username + '\'s edited information successfully');
                return true;
            } else {
                showalert('Edit User Error', msg.d);
                return false;
            };
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });
}
function ChangeUserAccessStatus(Username, AccessID, Status) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnChangeAccessUserStatus";
    var AccountID = siteCookie("AccountID");
    var CurrentAccessCode = siteCookie("UID");
    var Password = siteCookie("Pwd");

    var parameters = "{'AccountID':'" + AccountID + "', 'MainAccessCode':'" + CurrentAccessCode + "','Password':'" + Password +
        "','Status':'" + Status + "','AccessID':'" + AccessID + "'}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == '"Success"') {
                loadusers();
                shownotification('User Account Status', Username + '\'s account status changed successfully');
                return true;
            } else {
                showalert('User Account Status', msg.d);
                return false;
            };
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });
}
function loadusers() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnListAccessUsers";
    var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = '';
            jQuery.each(data, function (rec) {
                t = t + '<tr> <td><label class="input-control checkbox" onclick="javascript:updatecount();"  ><input type="checkbox" id="' + this.Access.AccessID + '"><span class="helper"></span></label></td>' +
                    '<td>' + this.AccessWeb.AccessName + '</td><td>' + this.Access.AccessCode + '</td><td>' + this.Access.Channel['Channel'] + '</td><td>' + (this.Access.Deleted ? "Disabled" : "Active") +
                    '</td><td><i title=" Sales Password ' + (this.AccessWeb.SalesPassword ? "" : "Not") + ' Required" class="icon-' + (this.AccessWeb.SalesPassword ? "locked" : "unlocked") + '"></i></td>' +
                    '<td><a href="javascript:edituser(\'' + this.Access.AccessID + '\', \'' + this.AccessWeb.AccessName + '\',\'' + this.Access.AccessCode + '\', \'' + this.Access.Channel['ChannelID'] +
                    '\',' + this.AccessWeb.SalesPassword + ');"><i class="icon-pencil"></i></a></td><td><a href="javascript:ChangeUserAccessStatus(\'' + this.AccessWeb.AccessName + '\',' + this.Access.AccessID + ', true);"><i class="icon-cancel"></i></a></td></tr>';
            });

            $("#users1>tbody:first").html(t);
        },
        error: function (emsg) {
            $("#users1>tbody:first").html('');
            console.log(emsg);
        }
    });

};
function loadusers_Deleted() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnListAccessUsers_Deleted";
    var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = '';
            jQuery.each(data, function (rec) {
                t = t + '<tr> <td><label class="input-control checkbox" onclick="javascript:updatecount();"  ><input type="checkbox" id="' + this.Access.AccessID + '"><span class="helper"></span></label></td>' +
                    '<td>' + this.AccessWeb.AccessName + '</td><td>' + this.Access.AccessCode + '</td><td>' + this.Access.Channel['Channel'] + '</td><td>' + (this.Access.Deleted ? "Disabled" : "Active") + '</td>' +
                    '<td><a href="javascript:edituser(\'' + this.Access.AccessID + '\', \'' + this.AccessWeb.AccessName + '\',\'' + this.Access.AccessCode + '\', \'' + this.Access.Channel['ChannelID'] +
                    '\');"><i class="icon-pencil"></i></a></td><td><a href="javascript:javascript:ChangeUserAccessStatus(\'' + this.AccessWeb.AccessName + '\',' + this.Access.AccessID + ', false);viewActiveUsers();"><i class="icon-history"></i></a></td></tr>';
            });

            $("#users1>tbody:first").html(t);
        },
        error: function (emsg) {
            $("#users1>tbody:first").html('');
            console.log(emsg);
        }
    });

};

/* Subscriber Functions  */
function addSubscriber(SubscriberName, MobileNumber) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnAddSubscribers";
    var AccountID = siteCookie("AccountID");
    var CurrentAccessCode = siteCookie("UID");
    var Password = siteCookie("Pwd");

    var parameters = "{'AccountID':'" + AccountID + "', 'AccessCode':'" + CurrentAccessCode + "','Password':'" + Password +
        "','Name':'" + SubscriberName + "','Mobile':'" + MobileNumber + "'}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == '"Success"') {
                loadsubscribers();
                shownotification('Add Subscriber', SubscriberName + ' added successfully');
                return true;
            } else {
                showalert('Add Subscriber Error', msg.d);
                return false;
            };
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });
};
function saveSubscriber(SubscriberName, MobileNumber, SubscriberID) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSaveSubscribers";
    var AccountID = siteCookie("AccountID");
    var CurrentAccessCode = siteCookie("UID");
    var Password = siteCookie("Pwd");

    var parameters = "{'AccountID':'" + AccountID + "', 'AccessCode':'" + CurrentAccessCode + "','Password':'" + Password +
        "','Name':'" + SubscriberName + "','Mobile':'" + MobileNumber + "','SubscriberID':'" + SubscriberID + "'}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == '"Success"') {
                loadsubscribers();
                shownotification('Edit Subscriber', SubscriberName + '\'s information edited successfully');
                return true;
            } else {
                showalert('Edit Subscriber Error', msg.d);
                return false;
            };
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });
};
function deleteSubscriber(SubscriberName, SubscriberID) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnDeleteSubscribers";
    var AccountID = siteCookie("AccountID");
    var CurrentAccessCode = siteCookie("UID");
    var Password = siteCookie("Pwd");

    var parameters = "{'AccountID':'" + AccountID + "', 'AccessCode':'" + CurrentAccessCode + "','Password':'" + Password +
        "','SubscriberID':'" + SubscriberID + "'}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == '"Success"') {
                loadsubscribers();
                shownotification('Delete Subscriber', SubscriberName + ' deleted successfully');
                return true;
            } else {
                showalert('Delete Subscriber Error', msg.d);
                return false;
            };
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });
};
function loadsubscribers() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnListSubscribers";
    var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = "";
            jQuery.each(data, function (rec) {
                t = t + " <tr><td><label class='input-control checkbox' onclick='javascript:updatecount();'  ><input type='checkbox' id=" + this.SubscriberID + "><span class='helper'></span></label></td> <td> " +
                    this.SubscriberName + "</td> <td> " + this.SubscriberMobile +
                    "</td> <td> " + this.Brand['Network']['Network'] +
                    "</td> <td> " + (this.Active ? "Active" : "Disabled") + "</td> " +
                    "<td><a href='javascript:editSubscriber(\"" + this.SubscriberName + "\", \"" + this.SubscriberMobile + "\", " + this.SubscriberID + ");'><i class='icon-pencil'></i></a></td>" +
                    "<td><a href='javascript:deleteSubscriber(\"" + this.SubscriberName + "\", " + this.SubscriberID + ");'><i class='icon-cancel'></i></a></td> </tr> ";
            });

            $("#subscriber1>tbody:first").html(t);
        },
        error: function (emsg) {
            $("#subscriber1>tbody:first").html('');
            console.log(emsg);
        }
    });

};
function loadsubscribers_select(subCombo) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnListSubscribers";
    var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = "";
            jQuery.each(data, function (rec) {
                t = t + "<option value='" + this.SubscriberMobile + "'>" + this.SubscriberName + "</option>";
                xSubscribers[this.SubscriberMobile] = this;
            });

            subCombo.html(t);
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });

};
function loadsubscribers_transactions(subCombo) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnListSubscribers";
    var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = "<option value=''>All Subscribers</option>";
            jQuery.each(data, function (rec) {
                t = t + "<option value='" + this.SubscriberName + "'>" + this.SubscriberName + "</option>";
                xSubscribers[this.SubscriberMobile] = this;
            });

            subCombo.html(t);
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });

};
//Background Functions 

function saveBackground(Background, Name) {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSaveBackground";
    var CurrentAccessCode = siteCookie("UID");
    var Password = siteCookie("Pwd");

    var parameters = "{'AccessCode':'" + CurrentAccessCode + "','AccessPassword':'" + Password +
        "','Background':\"" + Background + "\"}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            if (msg.d == '"Success"') {
                setBackground(Background);
                $.cookie("HOTBack", Background);
                shownotification('Background Saved', Name + ' successfully set as new background');
                return true;
            } else {
                showalert('Background Save Error', msg.d);
                return false;
            };
        },
        error: function (emsg) {
            console.log(emsg);
        }
    });
};

function loadsoldpins() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnGetSoldPinsAccount";
    var parameters = "{'accesscode':'" + siteCookie("UID") + "','accesspassword':'" + siteCookie("Pwd") + "'}";
    console.log(parameters);
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log(msg);
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = '';
            jQuery.each(data, function (rec) {
                t = t + '<tr> <td>' + this.RechargeDate + '</td><td>' + this.BrandName + '</td><td>' + this.Mobile + '</td><td>' + this.PinValue + '</td><td>' + this.Pin + '</td>' + '<td>' + this.PinRef + '</td></tr>';
            });

            $("#pins>tbody:first").html(t);
        },
        error: function (emsg) {
            $("#pins>tbody:first").html('');
            console.log(emsg);
        }
    });

};

// Transactions 
function getTransactionData(StartDate, EndDate, Filter) {
    var AccessCode = siteCookie("UID");
    var Password = siteCookie("Pwd");
    var AccountID = siteCookie("AccountID");
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnAccountTransactions_Retail";
    var parameters = "{'AccessCode':'" + AccessCode + "','AccessPassword':'" + Password + "','AccountID':'" + AccountID +
        "','StartDate':'" + StartDate + "','EndDate':'" + EndDate + "'}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#trans>tbody").html('<tr><td colspan="12" style="text-align:center;"><img src="images/preloader-w8-cycle-black.gif" width="16"/> Large queries can take up 2 to 3 minutes</td></tr>');
            TranSummary();
        },
        success: function (msg) {
            var data = [];
            var rows = '';
            var str_array = msg.d.split('\\r');
            console.log('done loading');

            for (var i = 0; i < str_array.length; i++) {
                // Trim the excess whitespace.
                //str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
                var line = str_array[i].split(','), cline = [];
                if (line.length >= 8) {
                    cline['RechargeDate'] = new Date(line[0]);
                    cline['Mobile'] = line[1];
                    cline['Amount'] = parseFloat(line[2]);
                    cline['StateID'] = Number(line[3]);
                    cline['RechargeID'] = Number(line[4]);
                    cline['Discount'] = parseFloat(line[5]);
                    cline['BrandID'] = Number(line[6]);
                    cline['AccessID'] = Number(line[7]);

                    data.push(cline);
                }

            }
            xTransactions = data;
            displayTransactions(xTransactions);

        },
        error: function (e) {
            console.log(e);
            console.log(parameters);

        }
    });
}
function displayTransactions(data) {
    var options = {
        valueNames: ['rd', 'rid', 'rtl', 'rmo', 'rbn', 'ramt', 'rdis', 'rst', 'rsid', 'rtid', 'rlid'],
        page: 50000
    };

    $("#trans>tbody").html('<tr><td class="rd"></td><td class="rid"></td><td class="rtl"></td><td class="rmo">' +
        '</td><td class="rbn"></td><td class="ramt"></td><td class="rdis"></td><td class="rst"></td><td class="rsid">' +
        '</td><td class="rtid"></td><td class="rlid"></td></tr>');

    transList = new List('tranList', options);
    jQuery.each(data, function () {
        transList.add({
            rd: formattedDateNorm(this.RechargeDate), rid: this.RechargeID, rtl: getTellerName(this.AccessID),
            rmo: (siteCookie("Retail") ? this.Mobile : getSubscriberName(this.Mobile)), rbn: getBrandName(this.BrandID), ramt: formatMoney(this.Amount), rdis: this.Discount.toFixed(1) + '%',
            rst: getStatusName(this.StateID), rsid: this.StateID, rtid: this.AccessID, rlid: getTellerLogin(this.AccessID)
        });
    });

    transList.remove("rid", '');
    console.log('done rendering');

    TranSummary();
    console.log('done summary');

    var fbrand = document.getElementById("txtBrands").options[document.getElementById("txtBrands").selectedIndex].value;
    var fteller = document.getElementById("txtTellers").options[document.getElementById("txtTellers").selectedIndex].value;
    var fsubs = document.getElementById("txtSubscribers").options[document.getElementById("txtSubscribers").selectedIndex].value;
    var needstobefiltered = false;
    if (!(fbrand == "")) needstobefiltered = true;
    if (!(fteller == "")) needstobefiltered = true;
    if (!(fsubs == "" || fsubs === undefined)) needstobefiltered = true;
    if (needstobefiltered == true) filter_trans_table();
}
function loadstatusids() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnListStatus";

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = "";
            jQuery.each(data, function (rec) {
                t = t + " <tr><td>" + this.StateID + "</td><td>" + this.State + "</td></tr> ";
                xStatusIDs[this.StateID] = this;
            });

            $("#status-key>tbody:first").html(t);
        },
        error: function (emsg) {
            $("#status-key>tbody:first").html('');
            console.log(emsg);
        }
    });

};
function loadbrandids() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnListBrands";

    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = "";
            jQuery.each(data, function (rec) {
                t = t + "<tr><td>" + this.BrandID + "</td><td>" + this.BrandName + "</td></tr> ";
                xBrandIDs[this.BrandID] = this;
            });
            //xBrandIDs = data;
            $("#brand-key>tbody:first").html(t);
        },
        error: function (emsg) {
            $("#brand-key>tbody:first").html('');
            console.log(emsg);
        }
    });

};
function loaduserskey() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnListAccessUsers";
    var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = JSON && JSON.parse(msg.d) || $.parseJSON(msg.d);
            var t = '', txtTeller = $("#txtTellers");
            txtTeller.empty();
            txtTeller.append('<option value="" defualt selected>All Tellers</option>');

            jQuery.each(data, function (rec) {
                t = t + '<tr><td>' + this.Access.AccessID + '</td><td>' + this.AccessWeb.AccessName + '</td><td>' + this.Access.AccessCode + '</td><td>' + this.Access.Channel.Channel + '</td></tr>';
                txtTeller.append('<option value="' + this.Access.AccessID + '" defualt selected>' + this.AccessWeb.AccessName + '</option>');
                xTellerUsers[this.Access.AccessID] = this;
            });
            //xTellerUsers = data;
            $("#teller-key>tbody:first").html(t);
            $("#txtTellers").val("");
        },
        error: function (emsg) {
            $("#teller-key>tbody:first").html('');
            console.log(emsg);
        }
    });

};

function getTellerName(searchVal) {
    return (xTellerUsers[searchVal] === undefined ? 'Unknown' : xTellerUsers[searchVal].AccessWeb.AccessName);
};
function getTellerLogin(searchVal) {
    return (xTellerUsers[searchVal] === undefined ? 'Unknown' : xTellerUsers[searchVal].Access.AccessCode);
};
function getBrandName(searchVal) {
    return (xBrandIDs[searchVal] === undefined ? 'Unknown' : xBrandIDs[searchVal].BrandName);
};
function getStatusName(searchVal) {
    return (xStatusIDs[searchVal] === undefined ? 'Unknown' : xStatusIDs[searchVal].State);
};
function getSubscriberName(searchVal) {
    return (xSubscribers[searchVal] === undefined ? searchVal : xSubscribers[searchVal].SubscriberName);
};

function TranSummary() {
    var total = 0;
    var array = $('.list tr:visible td:nth-child(6)');
    for (var i = 0, n = array.length; i < n; ++i) {
        total += parseFloat(array[i].innerHTML.replaceAll(',', ''));
    }
    $('#txtNumberTrans').html($('#trans>tbody>tr:visible').length);
    $('#txtTotalAmount').html(formatMoney(total));
}

//Data Bundles
function loadbundles() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/get-data-bundles";
    var rechargeid = 'Web' + guid();

    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': siteCookie("UID"),
            'x-access-password': siteCookie("Pwd"),
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $("#txtBundle").html('<option>Bundles Unavailable</option>');
        },
        success: function (msg) {

            try {
                $("#txtBundle").html("");
                $("#txtNetOneBundle").html("");
                xBundles = {};
                $.each(msg.Bundles, function (i, val) {
                    if (val.BrandId == 25 || val.BrandId == 26) {
                        $("#txtNetOneBundle").append($('<option />', { value: val.ProductCode, text: `${val.Name} - ZiG$ ${formatMoney(val.Amount / 100)}` }));
                    } else {
                        $("#txtBundle").append($('<option />', { value: val.ProductCode, text: `${val.Name} - ZiG$ ${formatMoney(val.Amount / 100)}` }));
                    }
                    xBundles[this.ProductCode]= this;
                });
            } catch (ex) {
                console.log(ex);
                $("#txtBundle").html('<option>Bundles Unavailable</option>');
            };
        },
        error: function (e) {

            console.log(e);
            $("#txtBundle").html('<option>Bundles Unavailable</option>');
        }
    });

}
function getBundleCost(ProductCode) {
    return (xBundles[ProductCode].Amount / 100);
}
function loadbundlesUSD() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/get-data-bundles-usd";
    var rechargeid = 'Web' + guid();

    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': siteCookie("UID"),
            'x-access-password': siteCookie("Pwd"),
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $("#txtBundleUSD").html('<option>Bundles Unavailable</option>');
            $("#txtBundleUSDNetone").html('<option>Bundles Unavailable</option>');
            $("#txtBundleUSDEconet").html('<option>Bundles Unavailable</option>');
        },
        success: function (msg) {

            try {
                $("#txtBundleUSD").html("");
                $("#txtBundleUSDNetone").html("");
                $("#txtBundleUSDEconet").html("");
                xBundlesUSD = {};
                $.each(msg.Bundles, function (i, val) {
                    $("#txtBundleUSD").append($('<option />', { value: val.ProductCode, text: `${val.Name} - USD$ ${formatMoney(val.Amount / 100)}` }));
                    if (val.BrandId == 38) {
                        $("#txtBundleUSDNetone").append($('<option />', { value: val.ProductCode, text: `${val.Name} - USD$ ${formatMoney(val.Amount / 100)}` }));
                    }
                    if (val.BrandId == 1) { 
                        $("#txtBundleUSDEconet").append($('<option />', { value: val.ProductCode, text: `${val.Name} - USD$ ${formatMoney(val.Amount / 100)}` }));
                    }
                    xBundlesUSD[this.ProductCode]=this;
                });
            } catch (ex) {
                console.log(ex);
                $("#txtBundleUSD").html('<option>Bundles Unavailable</option>');
            };
        },
        error: function (e) {

            console.log(e);
            $("#txtBundleUSD").html('<option>Bundles Unavailable</option>');
            $("#txtBundleUSDNetone").html('<option>Bundles Unavailable</option>');
            $("#txtBundleUSDEconet").html('<option>Bundles Unavailable</option>');
        }
    });

}
function getBundleCostUSD(ProductCode) {
    return (xBundlesUSD[ProductCode].Amount / 100);
}


// Data Pins 
function loadpinbundles() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/query-evd";
    var rechargeid = 'Web' + guid();

    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': siteCookie("UID"),
            'x-access-password': siteCookie("Pwd"),
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            $("#txtpins").html('<option>Loading available Pins</option>');
        },
        success: function (msg) {

            try {
                $("#txtpins").html("");
                $.each(msg.InStock, function (i, val) {
                    if (val.BrandId !== 24) {
                        $("#txtpins").append($('<option />', { value: `${val.BrandId}-${formatMoney(val.PinValue)}`, text: `${val.BrandName} - ZiG$ ${formatMoney(val.PinValue)}` }));
                        xBundles[`${val.BrandId}-${formatMoney(val.PinValue)}`] = this;
                    }

                });
            } catch (ex) {
                console.log(ex);
                $("#txtpins").html('<option>Pins Unavailable</option>');
            };
        },
        error: function (e) {

            console.log(e);
            $("#txtpins").html('<option>Pins Unavailable</option>');
        }
    });

}


// Zesa Tokens
function Confirm_Zesa_Customer(MeterNumber, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/check-customer-zesa";

    var parameters = "{'MeterNumber':'" + MeterNumber + "'}";
    var rechargeid = 'Web' + guid();


    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            showZesaLoading();
        },
        success: function (msg) {
            try {
                console.log(msg);
                var data = msg;
                console.log(data);
                if (data.ReplyCode == 2) {
                    $("#customerName").html(data.CustomerInfo.CustomerName);
                    showConfirmDetails();
                } else {
                    $("#zesa-error-details").html(data.ReplyMsg);
                    showZesaFailure();
                }
            } catch (ex) {
                console.log(ex);
                var msg = e.responseText;
                try {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    msg = data.ReplyMessage;
                } catch (ex) { }
                showZesaFailure();
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                msg = data.ReplyMessage;
            } catch (ex) { }
            $("#zesa-error-details").html(msg);
            showZesaFailure();
        }
    });
}
function Recharge_Zesa(MeterNumber, TargetMobile, rAmount, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-zesa";

    rAmount = parseFloat(Math.round(rAmount * 100) / 100).toFixed(2);
    var parameters = "{'MeterNumber':'" + MeterNumber + "','TargetNumber':'" + TargetMobile + "','amount':" + rAmount + "}";
    var rechargeid = 'Web' + guid();
    console.log(parameters);

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            showZesaLoading();
        },
        success: function (msg) {
            try {
                data = msg;
                console.log(data);
                if (data.ReplyCode == 2) {
                    showZesaSuccess();
                } else if (data.ReplyCode == 4) {
                    showZesaPending();
                } else {
                    $("#zesa-error-details").html(data.ReplyMsg);
                    showZesaFailure();
                }
            } catch (ex) {
                console.log(ex);
                var msg = e.responseText;
                try {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    msg = data.ReplyMessage;
                } catch (ex) { }
                $("#zesa-error-details").html(e.responseText);
                showZesaFailure();
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                msg = data.ReplyMessage;
            } catch (ex) { }
            $("#zesa-error-details").html(msg);
            showZesaFailure();
        }
    });
}
function Recharge_Zesa_USD(MeterNumber, TargetMobile, rAmount, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-zesa-usd";

    rAmount = parseFloat(Math.round(rAmount * 100) / 100).toFixed(2);
    var parameters = "{'MeterNumber':'" + MeterNumber + "','TargetNumber':'" + TargetMobile + "','amount':" + rAmount + "}";
    var rechargeid = 'Web' + guid();
    console.log(parameters);

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            showZesaLoading();
        },
        success: function (msg) {
            try {
                data = msg;
                console.log(data);
                if (data.ReplyCode == 2) {
                    showZesaSuccess();
                } else if (data.ReplyCode == 4) {
                    showZesaPending();
                } else {
                    $("#zesa-error-details").html(data.ReplyMsg);
                    showZesaFailure();
                }
            } catch (ex) {
                console.log(ex);
                var msg = e.responseText;
                try {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    msg = data.ReplyMessage;
                } catch (ex) { }
                $("#zesa-error-details").html(e.responseText);
                showZesaFailure();
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                msg = data.ReplyMessage;
            } catch (ex) { }
            $("#zesa-error-details").html(msg);
            showZesaFailure();
        }
    });
}

function Get_Zesa_Balance(AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/wallet-balance-zesa";


    var rechargeid = 'Web' + guid();


    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (msg) {
            try {
                //console.log(msg);
                var data = msg;
                //console.log(data);
                if (data.ReplyCode == 2) {
                    $("#ZesaBalance").html("$ " + parseFloat(Math.round(data.WalletBalance * 100) / 100).toFixed(2));
                    //if (data.WalletBalance != 0) {
                    $(".zesa").show();
                    $(".no-zesa").hide();
                    // }
                }
            } catch (ex) {
                console.log(ex);
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
        }
    });
}
function Get_Zesa_USD_Balance(AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/wallet-balance-zesa-usd";


    var rechargeid = 'Web' + guid();


    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (msg) {
            try {
                //console.log(msg);
                var data = msg;
                //console.log(data);
                if (data.ReplyCode == 2) {
                    $("#UtilityUSDBalance").html("$ " + parseFloat(Math.round(data.WalletBalance * 100) / 100).toFixed(2));
                    //if (data.WalletBalance != 0) {
                    $(".zesa").show();
                    $(".no-zesa").hide();
                    // }
                }
            } catch (ex) {
                console.log(ex);
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
        }
    });
}

// Telone 
function loadtelonebundles() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/query-telone-bundles";
    var rechargeid = 'Web' + guid();

    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': siteCookie("UID"),
            'x-access-password': siteCookie("Pwd"),
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $("#txtTeloneBundle").html('<option>Loading</option>');
        },
        success: function (msg) {
            try {
                $("#txtTeloneBundle").html("");
                xTeloneBundles = {};
                $.each(msg.Products, function (i, val) {
                    console.log(val);
                    $("#txtTeloneBundle").append($('<option />', { value: val.productIdField, text: `${val.nameField} - $ ${formatMoney(val.priceField)}` }));
                    xTeloneBundles[this.ProductIdField]=this;
                });
            } catch (ex) {
                console.log(ex);
                $("#txtTeloneBundle").html('<option>Bundles Unavailable</option>');
            };
        },
        error: function (e) {

            console.log(e);
            $("#txtTeloneBundle").html('<option>Bundles Unavailable</option>');
        }
    });

}
function loadtelonebundlesUSD() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/query-telone-bundles-USD";

    var rechargeid = 'Web' + guid();

    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': siteCookie("UID"),
            'x-access-password': siteCookie("Pwd"),
            'x-agent-reference': rechargeid,
            'Access-Control-Allow-Origin': '*'
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $("#txtTeloneBundle").html('<option>Loading</option>');
        },
        success: function (msg) {
            try {
                $("#txtTeloneBundle").html("");
                $.each(msg.Products, function (i, val) {
                    $("#txtTeloneBundle").append($('<option />', { value: val.productIdField, text: `${val.nameField} - $ ${formatMoney(val.priceField)}` }));
                    xTeloneBundles[this.ProductIdField] = this;
                });
            } catch (ex) {
                console.log(ex);
                $("#txtTeloneBundle").html('<option>Bundles Unavailable</option>');
            };
        },
        error: function (e) {

            console.log(e);
            $("#txtTeloneBundle").html('<option>Bundles Unavailable</option>');
        }
    });

}
function Confirm_Telone_Customer(AccountNumber, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/verify-telone-account";

    var rechargeid = 'Web' + guid();

    $.ajax({
        type: "GET",
        url: webMethod,
        data:
        {
            'AccountNumber': AccountNumber
        },
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            showTeloneLoading();
        },
        success: function (msg) {
            try {
                console.log(msg);
                var data = msg;
                console.log(data);
                if (data.ReplyCode == 2) {
                    $("#customerName").html(data.AccountName);
                    showConfirmDetails();
                } else {
                    $("#telone-error-details").html(data.ReplyMsg);
                    showTeloneFailure();
                }
            } catch (ex) {
                console.log(ex);
                var msg = e.responseText;
                try {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    msg = data.ReplyMessage;
                } catch (ex) { }
                showTeloneFailure();
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                msg = data.ReplyMessage;
            } catch (ex) { }
            $("#zesa-error-details").html(msg);
            showTeloneFailure();
        }
    });
}
function Recharge_TelOne_Bundle(AccountNumber, TargetMobile, ProductId, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-telone-adsl";

    var parameters = "{'AccountNumber':'" + AccountNumber + "','TargetNumber':'" + TargetMobile + "','ProductId':" + ProductId + "}";
    var rechargeid = 'Web' + guid();
    console.log(parameters);

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            showTeloneLoading();
        },
        success: function (msg) {
            try {
                data = msg;
                console.log(data);
                if (data.ReplyCode == 2) {
                    showTeloneSuccess();
                } else if (data.ReplyCode == 4) {
                    showTelonePending();
                } else {
                    $("#telone-error-details").html(data.ReplyMsg);
                    showTeloneFailure();
                }
            } catch (ex) {
                console.log(ex);
                var msg = ex.responseText;
                try {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    msg = data.ReplyMessage;
                } catch (ex) { }
                $("#telone-error-details").html(e.responseText);
                showTeloneFailure();
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                msg = data.ReplyMessage;
            } catch (ex) { }
            $("#telone-error-details").html(msg);
            showTeloneFailure();
        }
    });
}
function Recharge_TelOne_USD_Bundle(AccountNumber, TargetMobile, ProductId, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/recharge-telone-adsl-USD";

    var parameters = "{'AccountNumber':'" + AccountNumber + "','TargetNumber':'" + TargetMobile + "','ProductId':" + ProductId + "}";
    var rechargeid = 'Web' + guid();
    console.log(parameters);

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            showTeloneLoading();
        },
        success: function (msg) {
            try {
                data = msg;
                console.log(data);
                if (data.ReplyCode == 2) {
                    showTeloneSuccess();
                } else if (data.ReplyCode == 4) {
                    showTelonePending();
                } else {
                    $("#telone-error-details").html(data.ReplyMsg);
                    showTeloneFailure();
                }
            } catch (ex) {
                console.log(ex);
                var msg = ex.responseText;
                try {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    msg = data.ReplyMessage;
                } catch (ex) { }
                $("#telone-error-details").html(e.responseText);
                showTeloneFailure();
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                msg = data.ReplyMessage;
            } catch (ex) { }
            $("#telone-error-details").html(msg);
            showTeloneFailure();
        }
    });
}

// USD PINS
function loadusdbundles() {
    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/query-evd-usd";
    var rechargeid = 'Web' + guid();

    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': siteCookie("UID"),
            'x-access-password': siteCookie("Pwd"),
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {

            $("#txtUSDEVD").html('<option>Loading...</option>');
        },
        success: function (msg) {

            try {
                $("#txtUSDEVD").html("");
                $.each(msg.InStock, function (i, val) {
                    $("#txtUSDEVD").append($('<option />', { value: `${val.BrandId}-${formatMoney(val.PinValue)}`, text: `${val.BrandName} - USD$ ${formatMoney(val.PinValue)}` }));
                    xBundles[this.ProductCode] = this;
                });
            } catch (ex) {
                console.log(ex);
                $("#txtUSDEVD").html('<option>Bundles Unavailable</option>');
            };
        },
        error: function (e) {

            console.log(e);
            $("#txtUSDEVD").html('<option>Bundles Unavailable</option>');
        }
    });

}
function Get_USD_Balance(AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/wallet-balance-usd";


    var rechargeid = 'Web' + guid();


    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (msg) {
            try {
                //console.log(msg);
                var data = msg;
                //console.log(data);
                if (data.ReplyCode == 2) {
                    $("#USDBalance").html("$ " + parseFloat(Math.round(data.WalletBalance * 100) / 100).toFixed(2));
                    //if (data.WalletBalance != 0) {
                    $(".allow-usd").show();

                    // }
                }
            } catch (ex) {
                console.log(ex);
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
        }
    });
}

// Nyaradzo Payments
function Confirm_Nyaradzo_Customer(PolicyNumber, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/query-nyaradzo-account?PolicyNumber="+ PolicyNumber;
     
    var rechargeid = 'Web' + guid();


    $.ajax({
        type: "GET",
        url: webMethod, 
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            showNyaradzoLoading();
        },
        success: function (msg) {
            try {
                console.log(msg);
                var data = msg;
                console.log(data);
                if (data.ReplyCode == 2) {
                    $("#customerName").html(data.PolicyHolderName);
                    $("#accountStatus").html(data.Status);
                    $("#amountToBePaid").html(number_formatted(data.Balance));
                    $("#monthlyPremium").html(number_formatted(data.MOnthlyPremium));

                    showConfirmDetails();
                } else {
                    $("#nyaradzo-error-details").html(data.ReplyMsg);
                    showNyaradzoFailure();
                }
            } catch (ex) {
                console.log(ex);
                var msg = e.responseText;
                try {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    msg = data.ReplyMessage;
                } catch (ex) { }
                showNyaradzoFailure();
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                msg = data.ReplyMessage;
            } catch (ex) { }
            $("#nyaradzo-error-details").html(msg);
            showNyaradzoFailure();
        }
    });
}
function Recharge_Nyaradzo(PolicyNumber, TargetMobile, rAmount, AccessCode, Password) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v1/agents/nyaradzo-payment";

    rAmount = parseFloat(Math.round(rAmount * 100) / 100).toFixed(2);
    var parameters = "{'PolicyNumber':'" + PolicyNumber + "','MobileNumber':'" + TargetMobile + "','Amount':" + rAmount + "}";
    var rechargeid = 'Web' + guid();
    console.log(parameters);

    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        headers: {
            'x-access-code': AccessCode,
            'x-access-password': Password,
            'x-agent-reference': rechargeid
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (e) {
            showNyaradzoLoading();
        },
        success: function (msg) {
            try {
                data = msg;
                console.log(data);
                if (data.ReplyCode == 2) {
                    showNyaradzoSuccess();
                } else if (data.ReplyCode == 4) {
                    showNyaradzoPending();
                } else {
                    $("#nyaradzo-error-details").html(data.ReplyMsg);
                    showNyaradzoFailure();
                }
            } catch (ex) {
                console.log(ex);
                var msg = e.responseText;
                try {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    msg = data.ReplyMessage;
                } catch (ex) { }
                $("#nyaradzo-error-details").html(e.responseText);
                showNyaradzoFailure();
            };

        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            try {
                var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                msg = data.ReplyMessage;
            } catch (ex) { }
            $("#nyaradzo-error-details").html(msg);
            showNyaradzoFailure();
        }
    });
}

// New API V3
function GetToken(AccessCode, Pwd) {


    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v3/identity/login";
    var Data = '{"AccessCode": "' + AccessCode + '", "Password": "' + Pwd + '"}';

    $.ajax({
        type: "POST",
        url: webMethod,
        dataType: "json",
        contentType: "application/json;",
        data: Data,
        async: false,
        success: function (msg) {
            try {
                Token = msg;
            } catch (ex) {
                console.log(ex);
            };
            return Token;
        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
            return "";
        }
    });
}

// New API Balance Functions
function Get_All_Balances(JwtToken) {

    var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "api/v3/account/balance/";

    $.ajax({
        type: "GET",
        url: webMethod,
        headers: {
            'authorization': ('Bearer ' + JwtToken),
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (msg) {
            try {

                data = msg;
                console.log(data);
                data.forEach((item) => {
                    switch (item.accountTypeId) {
                        case 1:
                            $("#txtBalance").html("$ " + parseFloat(Math.round(item.balance * 100) / 100).toFixed(2));
                            break;
                        case 2:
                            $("#ZesaBalance").html("$ " + parseFloat(Math.round(item.balance * 100) / 100).toFixed(2));
                            $(".zesa").show();
                            $(".no-zesa").hide();
                            break;
                        case 3:
                            $("#USDBalance").html("$ " + parseFloat(Math.round(item.balance * 100) / 100).toFixed(2));
                            $(".allow-usd").show();
                            break;
                        case 4:
                            $("#UtilityUSDBalance").html("$ " + parseFloat(Math.round(item.balance * 100) / 100).toFixed(2));
                            $(".zesa").show();
                            $(".no-zesa").hide();
                            break;
                        default:
                    };
                });
            } catch (ex) {
                console.log(ex);
            };
        },
        error: function (e) {
            console.log(e);
            var msg = e.responseText;
        }
    });
}
function LoadBalanceNew() {
    GetToken(siteCookie("UID"), siteCookie("Pwd"));
    Get_All_Balances(Token.token);
}
