var timeoutcounter = 0;
var intervalID = 0;
var timeoutpassword = '';
var timeoutID = 0;
var totalSalesUpdate = 0;
var networkUpdate = 0;
var isData = false;
var isUsd = false;
var isDataUSD = false;

clearInterval(totalSalesUpdate);
totalSalesUpdate = setInterval(LoadSales, 240000);
setTimeout(LoadSales, 3000);
addLoadEvent(isPasswordRequired);
addLoadEvent(loadbundles);
addLoadEvent(loadbundlesUSD);

networkUpdate = setInterval(checkStatusOnlineDetails, 240000);
setTimeout(checkStatusOnlineDetails, 3000);

function resetpwdtimeout() {
    timeoutpassword = $('#txtPassword').val();
    timeoutcounter = 30;
    clearInterval(intervalID);
    intervalID = setInterval(countTimeout, 1000);
}
function countTimeout() {
    timeoutcounter -= 1;
    $('#txtTimeout').html(timeoutcounter);
    if (timeoutcounter == 0) {
        clearInterval(intervalID);
        showpasswordinput();
        LoadSales();
    }
}
function showpasswordinput() {
    $('#txtPassword').val("");
    $('#grpTextPassword').show();
    $('#grpUnlocked').hide();
}
function showtimeoutcounter() {

    $('#grpTextPassword').hide();
    $('#grpUnlocked').show();
    if (!($("#txtPasswordRequired")[0].checked)) {
        $('#txtPassword').val(siteCookie("Pwd"));
        rechargebtn();
    }
    clearTimeout(timeoutID);
    timeoutID = setTimeout(rechargeOptionsBind, 750);

}

function rechargeOptionsBind() {
    $('#txtPassword').val(timeoutpassword);
    $(window).bind('keypress', function (e) {
        $(this).unbind(e);
        if (e.keyCode == 13) {
            rechargebtn();

        }
    });
}
function rechargeOptionsBind_Clear() {
    $(window).unbind('keypress');
}

function clearform() {
    $('#txtAmount').removeAttr('readonly');
    $('#txtPhoneNumber').removeAttr('readonly');
    $('#txtPassword').val("");
    $('#txtPhoneNumber').val("");
    $('#txtAmount').val("");
    $("#grpConfirm").hide();
    $("#grpPassword").hide();
    $('#txtPhoneNumber').focus();
    rechargeOptionsBind_Clear();
}

function retry(Amount, TargetMobile, RowID) {
    clearform();
    $("#" + RowID).closest("tr").remove();
    $('#txtPhoneNumber').val(String(TargetMobile));
    $('#txtAmount').val(String(Amount));
    $('#txtPhoneNumber').keydown();
    $('#txtAmount').keydown();
    $('#txtAmount').focus();
    checkform();
}

function number_phone_pos(text) {
    var result = text.value.replace(/[^0-9]/g, "");
    checkMode();
    var splitchar = "-";
    rlen = result.length;
    if (rlen > 7) { result = result.substring(0, 4) + splitchar + result.substring(4, 7) + splitchar + result.substring(7); }
    if (rlen > 4 && rlen <= 7) { result = result.substring(0, 4) + splitchar + result.substring(4); }
    text.value = result;
    var notdecimal = false;
    try {
        notdecimal = (event.keyCode != 8);
    } catch (ex) { }

    if (isVoipNumber(result)) {
        $('#txtPhoneNumber').attr("maxlength", "13")
    } else {
        $('#txtPhoneNumber').attr("maxlength", "12")
    }

    if (rlen == 10 || (rlen == 11 && isVoipNumber(result))) {
        if (isPhoneNumberValid(result)) {
            $("#txtPhoneNumber").css("color", "black");
        } else {
            $("#txtPhoneNumber").css("color", "red");
        }
    }
    else {
        $("#txtPhoneNumber").css("color", "black");
    }
    if (notdecimal && (isPhoneNumberValid(result) && (rlen == 10 && !isVoipNumber(result) || rlen == 11 && isVoipNumber(result)))) {
        if (!isData) {
            $('#txtAmount').focus();
        } else {
            $('#txtBundle').focus();
        }
    }

    checkform();
}

function number_amount(text) {

    var result = text.value.replace(/[^0-9.]/g, "");

    try {
        console.log("Logged:" + event.keyCode);
        if (result == "" && event.keyCode == 8) $('#txtPhoneNumber').focus();
        checkMode();

    } catch (ex) { }

    if ((result.split(".").length) > 1) {
        spArr = result.split(".");
        decimals = spArr[1];
        if (decimals.length > 2) { decimals = decimals.substring(0, 2); }
        result = spArr[0] + '.' + decimals;

    }
    text.value = result;
    checkform();
}

function checkMode() {
    if (event.keyCode == 68) showdata(); // D for data
    if (event.keyCode == 65) showAirtime(); // A for airtime
    if (event.keyCode == 85) showAirtimeUsd(); // U for USDBundles

    var recharge = document.querySelector('input[name="rechargeType"]:checked').value;
    var currency = document.querySelector('input[name="currency"]:checked').value;;
    if (currency == "USD" & recharge == "Airtime") showAirtimeUsd();
    if (currency == "ZiG" & recharge == "Airtime") showAirtime();
    if (currency == "USD" & recharge == "Data") showDataUsd();
    if (currency == "ZiG" & recharge == "Data") showdata();


}



function showdata() {
    hideFields();
    $('#txtBundleDiv').show();
    $("#rechargeinfo tbody tr:eq(2) td h3").html("Bundle Name");
    //$('#txtBundle').focus();
    isData = true;
    isDataUSD = false;
    isUsd = false;
    checkform();

}

function showAirtime() {
    hideFields();
    $('#txtAmount').show();
    $("#rechargeinfo tbody tr:eq(2) td h3").html("Amount");
    //$('#txtAmount').focus();
    isData = false;
    isDataUSD = false;
    isUsd = false;
    checkform();
}

function showAirtimeUsd() {
    hideFields();
    $('#txtAmount').show();
    $("#rechargeinfo tbody tr:eq(2) td h3").html("Amount");
    //$('#txtAmount').focus();
    isData = false;
    isDataUSD = false;
    isUsd = true;
    checkform();
}

function showDataUsd() {
    hideFields();
    $('#txtBundleUsdDiv').show();
    $("#rechargeinfo tbody tr:eq(2) td h3").html("USD Bundles");
    //$('#txtBundleUSD').focus();
    isData = true;
    isDataUSD = true;

    isUsd = false;
    checkform();
    console.log("dataUSD")
}

function hideFields() {
    $('#txtAmount').hide();
    $('#txtBundleDiv').hide();
    $('#txtBundleUsdDiv').hide();

}

function isVoipNumber(phonenumber) {
    return (phonenumber.replace(/[^0-9]/g, "").substring(0, 3) == '086');
}

function checkform() {
    amount = $('#txtAmount').val().replace(/[^0-9,.]/g, "");
    phonenumber = $('#txtPhoneNumber').val().replace(/[^0-9]/g, "");
    phonenumbervalid = isPhoneNumberValid(phonenumber);
    var voip = isVoipNumber(phonenumber);

    if ((phonenumber.length == 10 || (phonenumber.length == 11 && voip)) && ((amount.length >= 1) || isData) && (phonenumbervalid == true)) {
        $('#grpConfirm').show();
        try { if (event.keyCode == 13) { confirmed(); } } catch (e) { };

    } else {
        $('#grpConfirm').hide();
    }
}

function confirmed() {
    $('#grpConfirm').hide();
    $('#grpPassword').show();

    $('#txtAmount').attr('readonly', 'true');
    $('#txtPhoneNumber').attr('readonly', 'true');

    $('#txtPassword').focus();

    if (timeoutcounter > 0 || !($("#txtPasswordRequired")[0].checked)) {
        showtimeoutcounter();
    }
}

function rechargebtn(button) {
    $('#txtPassword').css("color", "black");
    button = button || false;
    var enterPressed = false;
    try {
        enterPressed = (event.keyCode == 13)
    }
    catch (ex) {
        enterPressed = false;
    };

    if (enterPressed || button) {

        //if ($('#txtPassword').val() != siteCookie("Pwd")) {
        //    $('#txtPassword').effect("shake");
        //    $('#txtPassword').css("color", "red");
        //} else {
        rechargeClient();
        //}
    }
}
function rechargeClient() {
    var AccessCode = siteCookie("UID");
    var Amount = $("#txtAmount").val().replace(/[^0-9.]/g, "");
    var ProductCode = $("#txtBundle option:selected").val();
    var ProductCodeUsd = $("#txtBundleUSD option:selected").val();
    var Password = siteCookie("Pwd");
    var Subscriber = $("#txtPhoneNumber").val().replace(/[^0-9]/g, "");
    console.log(Password);
    if (isData) {
        if (isDataUSD) {
            Recharge_DataUSD($("#rechargelogbody"), "", Subscriber, ProductCodeUsd, AccessCode, Password);
        } else {
            Recharge_Data($("#rechargelogbody"), "", Subscriber, ProductCode, AccessCode, Password);
        }

    } else {
        if (isUsd) {
            Recharge_USD($("#rechargelogbody"), "", Subscriber, Amount, AccessCode, Password);
        } else {
            Recharge($("#rechargelogbody"), "", Subscriber, Amount, AccessCode, Password);
        }
    }
    resetpwdtimeout();
    clearform();
}

function cancelTran() {
    clearform();
}