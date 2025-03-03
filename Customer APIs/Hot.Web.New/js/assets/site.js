
$(document).ready(function () {
    $('#quickrecharge').click(function (e) {
        quickrecharge();
    });
    $('#quickrecharge-retail').click(function (e) {
        quickrecharge_retail();
    });
    $('#btnRechargeBulk').click(function (e) {
        rechargeBulkwindow();
    });
    RefererFormatSite();
    CheckLogin();
    setBackground();
    document.onkeydown = KeyPress;

});
function KeyPress(e) {
    var evtobj = window.event ? event : e
    if (evtobj.keyCode == 90 && evtobj.ctrlKey) {
        alert("Site.js version 1.05");
    };
    
    
}
function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            if (oldonload) {
                oldonload();
            }
            func();
        }
    }
}

function formattedDate(mydate) {
    String.prototype.padLeft = function (length, character) {
        var test = '';
        try {
            test = new Array(length - this.length + 1).join(character || ' ') + this;
        } catch (ex) { console.log('Error'); test = this; }
        return test;
    };
    var monthNames = [
        "Jan", "Feb", "Mar",
        "Apr", "May", "Jun", "Jul",
        "Aug", "Sep", "Oct",
        "Nov", "Dec"
    ]

    var date = new Date(mydate.match(/\d+/)[0] * 1);
    var day = String(date.getDate()).padLeft(2, '0');
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var hours = String(date.getHours()).padLeft(2, '0');
    var minutes = String(date.getMinutes()).padLeft(2, '0');
    var seconds = String(date.getSeconds()).padLeft(2, '0');
    
    return (day + '-' + monthNames[monthIndex] + '-' + year.toString().substr(2, 2))+ ' ' + hours+':'+ minutes+':'+seconds;
}
function formattedDateNorm(mydate) {
    String.prototype.padLeft = function (length, character) {
        return new Array(length - this.length + 1).join(character || ' ') + this;
    };
    var monthNames = [
        "Jan", "Feb", "Mar",
        "Apr", "May", "Jun", "Jul",
        "Aug", "Sep", "Oct",
        "Nov", "Dec"
    ];

    var date = new Date(mydate);
    var day = String(date.getDate()).padLeft(2, '0'), monthIndex = date.getMonth(), year = date.getFullYear(), hours = String(date.getHours()).padLeft(2, '0'), minutes = String(date.getMinutes()).padLeft(2, '0'), seconds = String(date.getSeconds()).padLeft(2, '0');

    return (day + '-' + monthNames[monthIndex] + '-' + year.toString().substr(2, 2)) + ' ' + hours + ':' + minutes + ':' + seconds;
}

 function formatMoney(amount) {
     var c = 2, d = '.', t = ',';
     var n = amount,
    c = isNaN(c = Math.abs(c)) ? 2 : c,
    d = d == undefined ? "." : d,
    t = t == undefined ? "," : t,
    s = n < 0 ? "-" : "",
    i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
    j = (j = i.length) > 3 ? j % 3 : 0;
     return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
 };

// JSON to CSV Converter
function ConvertToCSV(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';

    for (var i = 0; i < array.length; i++) {
        var line = '';
        for (var index in array[i]) {
            if (line != '') line += ','
            line += array[i][index];
        }
        str += line + '\r\n';
    }

    return str;
}

function TableToCSV($table) {
    var $rows = $table.find('thead>tr:has(td),tbody>tr:visible:has(td)'),

    // Temporary delimiter characters unlikely to be typed by keyboard
    // This is to avoid accidentally splitting the actual contents
    tmpColDelim = String.fromCharCode(11), // vertical tab character
    tmpRowDelim = String.fromCharCode(0), // null character

    // actual delimiter characters for CSV format
            colDelim = '","',
            rowDelim = '"\r\n"',

    // Grab text from table into CSV formatted string
            csv = '"' + $rows.map(function (i, row) {
                var $row = $(row),
                    $cols = $row.find('td');

                return $cols.map(function (j, col) {
                    var $col = $(col),
                        text = $col.text();

                    return text.replace(/"/g, '""'); // escape double quotes

                }).get().join(tmpColDelim);

            }).get().join(tmpRowDelim)
                .split(tmpRowDelim).join(rowDelim)
                .split(tmpColDelim).join(colDelim) + '"';
            return csv;
}

function download(filename, text) {
    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
    element.setAttribute('download', filename);

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}

function parseURLParams(url) {
    var queryStart = url.indexOf("?") + 1,
        queryEnd = url.indexOf("#") + 1 || url.length + 1,
        query = url.slice(queryStart, queryEnd - 1),
        pairs = query.replace(/\+/g, " ").split("&"),
        parms = {}, i, n, v, nv;

    if (query === url || query === "") {
        return;
    }

    for (i = 0; i < pairs.length; i++) {
        nv = pairs[i].split("=");
        n = decodeURIComponent(nv[0]);
        v = decodeURIComponent(nv[1]);

        if (!parms.hasOwnProperty(n)) {
            parms[n] = [];
        }

        parms[n].push(nv.length === 2 ? v : null);
    }
    return parms;
}
function RefererFormatSite() {
    var referer = (parseURLParams(document.URL)|| []);
    try {
        if (referer.retail[0] == 1) {
            $(".dealer").hide();
            $(".retail").show(250);
        }
        
    }
    catch (ex) { };
    try {
        if (referer.corporate[0] == 1) {
            $("#mnHelp").attr("href", "help.aspx/Corporate");
            $(".corporate").hide();
            $(".retail").hide(250);
        }
    }
    catch (ex) { };
    try {
        if (referer.info[0] == 1) {
            toggleAccinfo();
        }

    }
    catch (ex) { };
}
function CheckLogin() {

    if ($.cookie("HOTLogin")) {
        $(".loggedin").show(250);
        $(".loggedout").hide();
        
        if (siteCookie("Retail")) {
            $(".dealer").hide();
            $(".retail").show(250);
        } else {
            $(".retail").hide();
            $(".dealer").show();
        }
        if (siteCookie("NoRechargeUI")) {
            $(".recharge-ui").hide();
        }
         var expires = new Date();
         expires.setMinutes(expires.getMinutes() + 15); // Create a date 15 minutes from now  
         $.cookie('HOTLogin', $.cookie("HOTLogin"), { expires: expires });
    } else {
        $(".loggedin").hide(); 
        $(".loggedout").show(250);
        var pagename = document.URL.substr(document.URL.lastIndexOf("/") + 1);
        if (pagename == 'myaccount.aspx' || pagename == 'report.aspx' ||
            pagename == 'topup.aspx' || pagename == 'users.aspx' ||
            pagename == 'subscribers.aspx' || pagename == 'pos.aspx' ||
            pagename == 'zesa.aspx' || pagename == 'nyaradzo.aspx' ||
            pagename == 'Transactions.aspx'
           ) {
            openpage('./');
        }
    }


}
function setBackground(background) {
    if (background) {
        $("#hotback").css("background", background);
        $("#hotback").css("background-size", "100%");
    }
    else {
        if ($.cookie("HOTBack")) {
            $("#hotback").css("background",  $.cookie("HOTBack") );
            $("#hotback").css("background-size", "100%");
        }
    }
} 
function logout() {
    
    document.cookie = "HOTLogin" + '=; expires=Thu, 01 Jan 1990 00:00:01 GMT;';
    $.removeCookie("HOTLogin");
    openpage('./');
}
function back() {
    window.history.back();
    return false;
}
function openpage(page) {
     window.location.href = page;
}
function selectall() {
    $('tbody input:checkbox:visible').prop("checked", $("#maintick").prop("checked"));
    updatecount();
}
function shownotification(header, message) {
    $("#notification-header").html(header);
    $("#notification-message").html(message);
    $("#notification").show(150);
    var hidenotify = self.setInterval(function () {
        $("#notification").hide(250);
        window.clearInterval(hidenotify);
    }, 5000);
}
function showalert(header, message) {
    $("#alert-header").html(header);
    $("#alert-message").html(message);
    $("#alert").show(150);
    var hidenotify = self.setInterval(function () {
        $("#alert").hide(250);
        window.clearInterval(hidenotify);
    }, 5000);
}

function maintickstatus() {
    var numbervisible = $('tbody input:checkbox:visible').length;
    var numbervisiblechecked = $('tbody input:checkbox:visible:checked').length;
    if (numbervisible == numbervisiblechecked) {
        $("#maintick").prop("checked", true);
    } else {
        $("#maintick").prop("checked", false);
    }
}
function updatecount() {
    var numberchecked = $('tbody input:checkbox:checked').length;
    $("#txtSelected").html(numberchecked);
    maintickstatus();
}

/* Filter Table Stuff */
function filterTable(term, _id, startswith, additionalterms) {
    startswith = startswith || false;
    additionalterms = additionalterms || '';

    var table = document.getElementById(_id);
    var rows = $('#'+_id+'>tbody>tr');
    dehighlight(table);
    var terms = (term.value + ' ' + additionalterms).toLowerCase().split(" ");

    for (var r = 0; r < rows.length; r++) {
        var display = '';
        for (var i = 0; i < terms.length; i++) {
            if (rows[r].innerHTML.replace(/<[^>]+>/g, "").toLowerCase()
				.indexOf(terms[i]) < 0) {
                display = 'none';
            } else {
                if (startswith) {
                    var foundinstart = false;                    
                    for (var c = 0; c < rows[r].cells.length; c++) {
                       // console.log(table.rows[r].cells[c].innerHTML.replace(/<[^>]+>/g, "").toLowerCase().indexOf(terms[i]));
                        if (rows[r].cells[c].innerHTML.replace(/<[^>]+>/g, "").trim().toLowerCase().indexOf(terms[i]) == 0) {
                            foundinstart = true;
                        }
                    }
                    if (!foundinstart && terms[i] != '') { display = 'none'; }

                }
                if (terms[i].length && (additionalterms.toLowerCase().indexOf(terms[i]) < 0)) highlight(terms[i], rows[r]);
            }

            rows[r].style.display = display;
        }
    }
    maintickstatus();
}
function dehighlight(container) {
    for (var i = 0; i < container.childNodes.length; i++) {
        var node = container.childNodes[i];

        if (node.attributes && node.attributes['class']
			&& node.attributes['class'].value == 'highlighted') {
            node.parentNode.parentNode.replaceChild(
					document.createTextNode(
						node.parentNode.innerHTML.replace(/<[^>]+>/g, "")),
					node.parentNode);
            // Stop here and process next parent
            return;
        } else if (node.nodeType != 3) {
            // Keep going onto other elements
            dehighlight(node);
        }
    }
}
function highlight(term, container) {
    for (var i = 0; i < container.childNodes.length; i++) {
        var node = container.childNodes[i];

        if (node.nodeType == 3) {
            // Text node
            var data = node.data;
            var data_low = data.toLowerCase();
            if (data_low.indexOf(term) >= 0) {
                //term found!
                var new_node = document.createElement('span');

                node.parentNode.replaceChild(new_node, node);

                var result;
                while ((result = data_low.indexOf(term)) != -1) {
                    new_node.appendChild(document.createTextNode(
								data.substr(0, result)));
                    new_node.appendChild(create_node(
								document.createTextNode(data.substr(
										result, term.length))));
                    data = data.substr(result + term.length);
                    data_low = data_low.substr(result + term.length);
                }
                new_node.appendChild(document.createTextNode(data));
            }
        } else {
            // Keep going onto other elements
            highlight(term, node);
        }
    }
}
function create_node(child) {
    var node = document.createElement('span');
    node.setAttribute('class', 'highlighted');
    node.attributes['class'].value = 'highlighted';
    node.appendChild(child);
    return node;
}

function number_phone(text) {
    var result = text.replace(/[^0-9]/g, "");
    var splitchar = " ";
    rlen = result.length;
    if (rlen > 7) { result = result.substring(0, 4) + splitchar + result.substring(4, 7) + splitchar + result.substring(7); }
    if (rlen > 4 && rlen <= 7) { result = result.substring(0, 4) + splitchar + result.substring(4); }
    return result;
}

function number_amount(text) {
    var result = text.replace(/[^0-9.]/g, "");
    if ((result.split(".").length) > 1) {
        spArr = result.split(".");
        decimals = spArr[1];
        if (decimals.length > 2) { decimals = decimals.substring(0, 2); }
        result = spArr[0] + '.' + decimals;
    }
    return result;
}

function number_formatted(number) {
    return ($.isNumeric(number) ? parseFloat(Math.round(number * 100) / 100).toFixed(2) : number); 
}

/* Dialog Boxes*/
function adduser() {
    $.get('./addusers.html?version=1', function (addusersstr) {
        $.Dialog({
            'title': 'Add New Trusted User',
            'content': addusersstr,
            'draggable': true,
            'closeButton': true,
            'keepOpened': true,
            'buttonsAlign': 'right',
            'position': {
                'offsetY': 20
            },
            'buttons': {
                '<i class="icon-plus-2"></i>Add User': {
                    'action': function () {
                        if (addUserValidator.form()) {
                            newAccessCode = (($("#txtType").val() == 1) ? $("#txtMobile").val() : $("#txtEmail").val());
                            return addUserAccess($("#txtUsername").val(), $("#txtType").val(), newAccessCode, $("#txtPassword").val(), ($("#txtSalesPassword:checked").val() ? 1 : 0));
                        } else {
                            return false;
                        };
                    }
                }
            }

        });
        $()["Input"]({ initAll: true });
    });
}

function edituser(AccessID, UserName, AccessCode, ChannelID, Checked) {
    $.get('./edituser.html?version=1', function (editusersstr) {
        $.Dialog({
            'title': 'Edit Trusted User',
            'content': editusersstr,
            'draggable': true,
            'closeButton': true,
            'keepOpened': true,
            'buttonsAlign': 'right',
            'position': {
                'offsetY': 20
            },
            'buttons': {
                '<i class="icon-save"></i>Save Changes': {
                    'action': function () {
                        if (editUserValidator.form()) {
                            newAccessCode = (($("#txtType").val() == 1) ? $("#txtMobile").val() : $("#txtEmail").val());
                            SaveUserAccess($("#txtUsername").val(), $("#txtType").val(), newAccessCode, $("#txtPassword").val(), AccessID, ($("#txtSalesPassword:checked").val() ? 1 : 0));

                            return true;
                        } else {
                            return false;
                        };
                    }
                }
            }

        });
        $()["Input"]({ initAll: true });
        document.getElementById('txtType').value = ChannelID;
        setChannel(ChannelID);
        document.getElementById('txtEmail').value = AccessCode;
        document.getElementById('txtUsername').value = UserName;
        document.getElementById('txtSalesPassword').checked = Checked;


    });
}

function viewreport(startdate, enddate,filter) {
    newwindow = window.open('./report.aspx?startdate=\'' + startdate + '\'&enddate=\'' + enddate + '\'&filter=\'' + filter + '\'', "Hot Recharge Report", '');
    if (window.focus) { newwindow.focus(); }
    return false;
}

function quickrecharge() {
    $.get('./quickrecharge.html', function (quickrechargecontent) {
        $.Dialog({
            'title': 'Quick Recharge',
            'content': quickrechargecontent,
            'draggable': true,
            'closeButton': true,
            'buttons': {}
        });
        $()["Input"]({ initAll: true });
    });
}
function quickrecharge_retail() {
    $.get('./quickrecharge_retail.html', function (quickrechargecontent) {
        $.Dialog({
            'title': 'Quick Recharge',
            'content': quickrechargecontent,
            'draggable': true,
            'closeButton': true,
            'keepOpened': true,
            'buttons': {}
        });
        $()["Input"]({ initAll: true });
    });
} 

function addnewsubscriber() {
    $.get('./addsubscriber.html', function (newsubscribercontent) {
        $.Dialog({
            'title': 'Add Subscriber',
            'content': newsubscribercontent,
            'draggable': true,
            'closeButton': true,
            'keepOpened': true,
            'buttonsAlign': 'right',
            'position': {
                'offsetY': 20
            },
            'buttons': {
                '<i class="icon-plus-2"></i> Add Subscriber': {
                    'action': function () {
                        if (addsubvalidator.form()) {
                            addSubscriber($("#txtSubName").val(), $("#txtPhoneNumber").val().replace('+263','0'));
                            return true;
                        } else {
                            return false;
                        };
                    }
                }
            }
        });
        $()["Input"]({ initAll: true });
    });
}
function editSubscriber(SubscriberName, Mobile, SubscriberID) {
    $.get('./editsubscriber.html', function (editsubstr) {
        $.Dialog({
            'title': 'Edit Subscriber',
            'content': editsubstr,
            'draggable': true,
            'closeButton': true,
            'keepOpened': true,
            'buttonsAlign': 'right',
            'position': {
                'offsetY': 20
            },
            'buttons': {
                '<i class="icon-save"></i>Save Changes': {
                    'action': function () {
                        if (addsubvalidator.form()) {
                            saveSubscriber($("#txtSubName").val(), $("#txtPhoneNumber").val().replace('+263', '0'), SubscriberID);
                            return true;
                        } else {
                            return false;
                        };
                    }
                }
            }

        });
        $()["Input"]({ initAll: true });
        document.getElementById('txtSubName').value = SubscriberName;
        document.getElementById('txtPhoneNumber').value = Mobile;

    });
}

function addrecharges() {
    var AccessCode = siteCookie("UID");
    var Amount = $("#txtRechargeAmt").val().replace(/[^0-9.]/g, "");
    var Password = siteCookie("Pwd");
    var delay = 0;
    $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
        var Mobile = $('td', this)[2].innerHTML.replace(/[^0-9]/g, "");
        var SubscriberName = $('td', this)[1].innerHTML;
        
        // Delay no longer needed 
        //if (!(Mobile.indexOf('077') == 0 || Mobile.indexOf('+26377') == 0 || Mobile.indexOf('26377') == 0)) {
        //    delay = delay + 230;
          
        //} else {
           delay = delay + 75;
        //};
        setTimeout(function () {
            Recharge($("#rechargeslog"), SubscriberName, Mobile, Amount, AccessCode, Password);
        }, delay);
    });
    $('tbody input:checkbox').prop("checked", false);
}

function addrecharges_usd() {
    var AccessCode = siteCookie("UID");
    var Amount = $("#txtRechargeAmtUsd").val().replace(/[^0-9.]/g, "");
    var Password = siteCookie("Pwd");
    var delay = 0;
    $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
        var Mobile = $('td', this)[2].innerHTML.replace(/[^0-9]/g, "");
        var SubscriberName = $('td', this)[1].innerHTML;

        // Delay no longer needed 
        //if (!(Mobile.indexOf('077') == 0 || Mobile.indexOf('+26377') == 0 || Mobile.indexOf('26377') == 0)) {
        //    delay = delay + 230;

        //} else {
        delay = delay + 75;
        //};
        setTimeout(function () {
            Recharge_USD($("#rechargeslog"), SubscriberName, Mobile, Amount, AccessCode, Password);
        }, delay);
    });
    $('tbody input:checkbox').prop("checked", false);
}

function addrecharges_data() {
    var AccessCode = siteCookie("UID");
    var ProductCode = $("#txtBundle option:selected").val();
    var Password = siteCookie("Pwd");
    var delay = 0;
    $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
        var Mobile = $('td', this)[2].innerHTML.replace(/[^0-9]/g, "");
        var SubscriberName = $('td', this)[1].innerHTML;
        delay = delay + 75; 
        setTimeout(function () {
            Recharge_Data($("#rechargeslog"), SubscriberName, Mobile, ProductCode, AccessCode, Password);
        }, delay);
    });
    $('tbody input:checkbox').prop("checked", false);
}

function addrecharges_dataUSD() {
    var AccessCode = siteCookie("UID");
    var ProductCode = $("#txtBundleUSD option:selected").val();
    var Password = siteCookie("Pwd");
    var delay = 0;
    $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
        var Mobile = $('td', this)[2].innerHTML.replace(/[^0-9]/g, "");
        var SubscriberName = $('td', this)[1].innerHTML;
        delay = delay + 75;
        setTimeout(function () {
            Recharge_DataUSD($("#rechargeslog"), SubscriberName, Mobile, ProductCode, AccessCode, Password);
        }, delay);
    });
    $('tbody input:checkbox').prop("checked", false);
}

function addrecharges_dataUSD_WithCode(ProductCode) {
    var AccessCode = siteCookie("UID"); 
    var Password = siteCookie("Pwd");
    var delay = 0;
    $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
        var Mobile = $('td', this)[2].innerHTML.replace(/[^0-9]/g, "");
        var SubscriberName = $('td', this)[1].innerHTML;
        delay = delay + 75;
        setTimeout(function () {
            Recharge_DataUSD($("#rechargeslog"), SubscriberName, Mobile, ProductCode, AccessCode, Password);
        }, delay);
    });
    $('tbody input:checkbox').prop("checked", false);
}

function addrecharges_usdevd() {
    var AccessCode = siteCookie("UID");
    var selected = $("#txtUSDEVD option:selected").val()
    var Amount = selected.split('-')[0];
    var brandid = selected.split('-')[1];
    var Password = siteCookie("Pwd");
    var delay = 0;
    $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
        var Mobile = $('td', this)[2].innerHTML.replace(/[^0-9]/g, "");
        var SubscriberName = $('td', this)[1].innerHTML;

        // Delay no longer needed 
        //if (!(Mobile.indexOf('077') == 0 || Mobile.indexOf('+26377') == 0 || Mobile.indexOf('26377') == 0)) {
        //    delay = delay + 230;

        //} else {
        delay = delay + 75;
        //};
        setTimeout(function () {
            Recharge_USDEVD($("#rechargeslog"), SubscriberName, Mobile, Amount, brandid , AccessCode, Password);
        }, delay);
    });
    $('tbody input:checkbox').prop("checked", false);
}

function addrecharges_evd() {
    var AccessCode = siteCookie("UID");
    var selected = $("#txtpins option:selected").val()
    var Amount = selected.split('-')[0].replace(",","");
    var brandid = selected.split('-')[1].replace(",", "");
    var Password = siteCookie("Pwd");
    var delay = 0;
    $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
        var Mobile = $('td', this)[2].innerHTML.replace(/[^0-9]/g, "");
        var SubscriberName = $('td', this)[1].innerHTML;

        // Delay no longer needed 
        //if (!(Mobile.indexOf('077') == 0 || Mobile.indexOf('+26377') == 0 || Mobile.indexOf('26377') == 0)) {
        //    delay = delay + 230;

        //} else {
        delay = delay + 75;
        //};
        setTimeout(function () {
            Recharge_Pins($("#rechargeslog"), SubscriberName, Mobile, Amount, brandid, AccessCode, Password);
        }, delay);
    });
    $('tbody input:checkbox').prop("checked", false);
}

function rechargeBulkwindow() {
    $.get('./rechargebulk.html?v=1.26', function (rechargecontent) {

        $.Dialog({
            'title': 'Bulk Recharges',
            'content': rechargecontent,
            'draggable': true,
            'closeButton': true,
            'buttons': {}
        });
        $()["Input"]({ initAll: true });
    });
}

function isPhoneNumberValid(phonenumber) {
    phonenumber = phonenumber.replace(/[^0-9]/g, "");
    phonenumbervalid = true;
    switch (phonenumber.substring(0, 3)) {
        case '077': //Econet
            break;
        case '078': //Econet
            break;
        case '071': //NetOne
            break;
        case '073': //Telecel
            break;
        case '086':
            return (phonenumber.substring(0, 5) == '08644'); //Africom

            break;
        default:
            phonenumbervalid = false;
    }
    return phonenumbervalid;
}