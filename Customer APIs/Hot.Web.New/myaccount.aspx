<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="myaccount.aspx.vb" Inherits="myaccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Body File -->

    <!--==================================================-->
    <!-- Start myaccount Area -->
    <!--==================================================-->
    <div style="padding-top: 30px !important;">
        <div class="container">
            <div class="row">


                <div style="background-color: #ce2128; color: white; padding-top: 20px; margin-right: 10px;" class="col-lg-4 col-md-12">


                    <h2 style="color: white"><i class="icon-bars"></i><small>Account</small> Balances</h2>
                    <br />
                    <table id="tBalance" class="no-borders no-padding">
                        <tr>
                            <td style="width: 20%">Airtime ZiG</td>
                            <td style="width: 30%"><span id="txtBalance">0.00</span></td>
                            <td style="width: 10%">USD</td>
                            <td><span id="USDBalance">0.00</span></td>
                        </tr>
                        <tr>
                            <td>Utility ZiG</td>
                            <td><span id="ZesaBalance">0.00</span></td>
                            <td>USD</td>
                            <td><span id="UtilityUSDBalance">0.00</span></td>
                        </tr>

                    </table>

                </div>
            </div>
        </div>
    </div>

    <div style="margin-bottom: 0px !important;" class="service-details-area wow fadeInUp">

        <div class="container">

            <div class="row">


                <div class="col-lg-4 col-md-12 bblock recharge-ui">
                    <button style="background-color: transparent; color: white" onclick='javascript:window.location.href = "subscribers.aspx";'>
                        <h2><i class="icon-user-3"></i><small><small>Recharge</small> Subscribers</small></h2>
                    </button>
                </div>

                <div class="col-lg-3 col-md-12 sblock recharge-ui">

                    <button style="background-color: transparent; color: white" onclick='javascript:window.location.href = "pos.aspx";'>
                        <h2><i class="icon-bars"></i><small>Point</small> of Sales</h2>
                    </button>

                </div>

                <div class="col-lg-4 col-md-12 bblock">

                    <button style="background-color: transparent; color: white" onclick='javascript:window.location.href = "Transactions.aspx";'>
                        <h2>
                            <i class="icon-pie"></i><small>Transaction</small> Report
                        </h2>

                    </button>
                </div>

                <div class="col-lg-4 col-md-12 bblock recharge-ui">
                    <button style="background-color: transparent; color: white" onclick='javascript:window.location.href = "zesa.aspx";'>
                        <h2><i class="icon-power-2"></i><small><small>Zesa</small> Token</small></h2>
                    </button>
                </div>
                <div class="col-lg-3 col-md-12 sblock recharge-ui">
                    <button style="background-color: transparent; color: white" onclick='javascript:window.location.href = "Telone.aspx";'>
                        <h2><i class="icon-phone"></i><small><small>Telone</small> Products</small></h2>
                    </button>
                </div>
                <div class="col-lg-4 col-md-12 bblock recharge-ui">
                    <button style="background-color: transparent; color: white" onclick='javascript:window.location.href = "Nyaradzo.aspx";'>
                        <h2><i class="icon-book"></i><small><small>Nyaradzo</small> Payment</small></h2>
                    </button>
                </div>

                <div class="col-lg-4 col-md-12 bblock">
                    <button style="background-color: transparent;" onclick='javascript:window.location.href = "topup.aspx";'>
                        <h2><i class="icon-cart"></i><small>Credit</small> Your Account</h2>
                    </button>
                </div>
                <div id="btnManageUsers" class="col-lg-3 col-md-12 sblock">
                    <button style="background-color: transparent; color: white" onclick='javascript:window.location.href = "users.aspx";'>
                        <h2><i class="icon-locked"></i><small><small>Manage</small>Trusted Users </small></h2>
                    </button>
                </div>

            </div>
        </div>
    </div>
    </div>
	<!--==================================================-->
    <!-- End myaccount Area -->
    <!--==================================================-->





    <div style="padding-top: 30px !important;" class="about-area">
        <div class="container">
            <div class="row">




                <div style="background-color: #ce2128; padding-top: 20px; margin-right: 10px;" class="col-lg-4 col-md-12">
                    <h2 class="fg-color-whiter"><i class="icon-key-2"></i><small>Change</small> Password</h2>
                    <form runat="server" id="frmChangePassword">
                        <h6 style="margin-bottom: 0px;" class="fg-color-whiter">Current Password</h6>
                        <div class="form-box input-control password">
                            <input id="txtOldPwd" runat="server" type="password" placeholder="Current Password" />
                            <button class="btn-reveal"></button>
                        </div>
                        <h6 style="margin-bottom: 0px;" class="fg-color-whiter">New Password</h6>
                        <div class="form-box input-control password">
                            <input id="txtNewPwd" runat="server" type="password" placeholder="New Password" />
                            <button class="btn-reveal"></button>
                        </div>
                        <h6 style="margin-bottom: 0px;" class="fg-color-whiter">Confirm Password</h6>
                        <div class="form-box input-control password">
                            <input id="txtCfmPwd" runat="server" type="password" placeholder="Confirm Password" />
                            <button class="btn-reveal"></button>
                        </div>
                        <div class="clearfix"></div>
                        <button id="btnChangePwd" class="form-box " style="width: 100%; margin-bottom: 0px;" runat="server">
                            <i class="icon-key"></i>Change Password
                        </button>
                        <div class="padding10" style="text-align: center" id="txtAlert" runat="server">
                        </div>
                        <div style="text-align: center; width: 100%;" class="retail">
                            <div class="input-control checkbox">
                                <label>
                                    <input type="checkbox" class="txtPasswordRequired" checked id="txtPasswordRequired" name="txtPasswordRequired" runat="server" onmouseup="javascript:$('.saverequired').show();" />
                                    <span class="check"></span>
                                    Password Required
                                </label>
                            </div>
                            <button class="button bg-color-green fg-color-whiter saverequired no-bottom-margin" id="btnSaveRequred" style="display: none;" runat="server">
                                <i class="icon-save"></i><span>Save</span>
                            </button>
                        </div>
                        <div class="saverequired" style="font-size: 12px; display: none;">Enter your password above to save change.</div>
                    </form>
                </div>


                <div style="background-color: #f1f1f1; padding-top: 20px;" class="col-lg-7 col-md-12">
                    <h2><i class="icon-newspaper"></i><small>Account</small> Information</h2>
                    <table id="accountinfo" class="no-borders no-bottom-margin">
                        <tr>
                            <td style="width: 18%"></td>
                            <td></td>
                            <td style="width: 18%"></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Account Name</h5>
                            </td>
                            <td colspan="3">
                                <div class="form-box input-control text disabled no-bottom-margin">
                                    <input id="txtAccountName" type="text" disabled="" placeholder="John Doe">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Address</h5>
                            </td>
                            <td colspan="3">
                                <div class="form-box input-control text disabled  no-bottom-margin">
                                    <input id="txtAddress1" type="text" disabled="" placeholder="Address">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Surburb</h5>
                            </td>
                            <td>
                                <div class="form-box input-control text disabled  no-bottom-margin">
                                    <input id="txtAddress2" type="text" disabled="" placeholder="Surburb">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                            <td>
                                <h5>City</h5>
                            </td>
                            <td>
                                <div class="form-box input-control text disabled  no-bottom-margin">
                                    <input id="txtCity" type="text" disabled="" placeholder="City">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>ID Number</h5>
                            </td>
                            <td>
                                <div class="form-box input-control text disabled  no-bottom-margin">
                                    <input id="txtNationalID" type="text" disabled="" placeholder="00-000000X00">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                            <td>
                                <h5>VAT Number</h5>
                            </td>
                            <td>
                                <div class="form-box input-control text disabled no-bottom-margin">
                                    <input id="txtVatNumber" type="text" disabled="" placeholder="0000/0000" maxlength="9">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <h5>Contact Name</h5>
                            </td>
                            <td>
                                <div class="form-box input-control text disabled no-bottom-margin">
                                    <input id="txtContactName" type="text" disabled="" placeholder="Contact Name">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                            <td>
                                <h5>Phone Number</h5>
                            </td>
                            <td>
                                <div class="form-box input-control text disabled no-bottom-margin">
                                    <input id="txtContactNumber" type="text" disabled="" placeholder="+263 000 0000">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Email</h5>
                            </td>
                            <td colspan="2">
                                <div class="form-box input-control text disabled no-bottom-margin">
                                    <input id="txtEmail" type="email" disabled="" placeholder="hot@hot.co.zw">
                                    <button class="btn-clear" tabindex="-1" type="button"></button>
                                </div>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <button class="place-right no-bottom-margin bg-color-redLight locked" onclick="javascript:toggleAccinfo();" id="btnLock" style="display: none;">
                                    <i class="icon-unlocked"></i><span>Edit</span>
                                </button>
                                <button id="btnSaveAccount" class="place-right no-bottom-margin bg-color-redLight" style="display: none;" onclick="javascript:saveaccountinfo();">
                                    <i class="icon-save"></i><span>Save</span>
                                </button>
                            </td>
                        </tr>
                    </table>
                </div>




            </div>

        </div>
    </div>
    <script type="text/javascript">
        addLoadEvent(loadaccountinfo);
        addLoadEvent(isPasswordRequired);
        addLoadEvent(loadZESABalance);
        addLoadEvent(loadUSDBalance);
        addLoadEvent(LoadBalanceNew);


        function loadUSDBalance() {
            Get_USD_Balance(siteCookie("UID"), siteCookie("Pwd"));
        }
        function loadZESABalance() {
            Get_Zesa_Balance(siteCookie("UID"), siteCookie("Pwd"));
            Get_Zesa_USD_Balance(siteCookie("UID"), siteCookie("Pwd"));
        }

        function toggleAccinfo() {
            if ($("#btnLock").hasClass("locked")) {
                unlockaccountinfo();
            }
            else {
                lockaccountinfo();
            }
        }

        function unlockaccountinfo() {
            var status = $("#btnLock");
            $("#accountinfo Input").removeAttr("disabled");
            $(status).removeClass("locked");
            $(status).children("i").removeClass("icon-unlocked");
            $(status).children("i").addClass("icon-locked");
            $(status).children("span").html("Lock");
            $("#btnSaveAccount").show();
        }
        function lockaccountinfo() {
            var status = $("#btnLock");
            $("#accountinfo Input").attr("disabled", "disabled");
            $(status).addClass("locked");
            $(status).children("i").removeClass("icon-locked");
            $(status).children("i").addClass("icon-unlocked");
            $(status).children("span").html("Edit");
            $("#btnSaveAccount").hide();
        }
        $('#btnViewTransactions').click(function (e) {
            var filterlist = '';

            viewreport($("#txtStartDate").val(), $("#txtEndDate").val(), filterlist);
        });


        function UpdateAccNotify() {
            var AccUpdated = ($.cookie("DetailsUpdated") === undefined ? false : true);
            //var AccUpdated = true;

            if (!(AccUpdated)) {

                $.get('./AddressUpdateNotification.html?version=1', function (addusersstr) {
                    $.Dialog({
                        'title': 'Confirm Contact Details',
                        'content': addusersstr,
                        'draggable': true,
                        'closeButton': true,
                        'keepOpened': true,
                        'buttonsAlign': 'right',
                        'position': {
                            'offsetY': 20
                        },
                        'buttons': {
                            '<i class="icon-pencil"></i>Update Details': {
                                'action': function () {
                                    unlockaccountinfo();
                                    return true;
                                }
                            },
                            '<i class="icon-thumbs-up"></i>These details are correct': {
                                'action': function () {
                                    $.cookie("DetailsUpdated", 1);
                                    return true;
                                }
                            }
                        }

                    });


                    $()["Input"]({ initAll: true });
                });

            }


        }
    </script>

    </div>
 <!-- Body File end -->

    <!-- jquery meanmenu js -->
    <script src="asset/js/jquery.meanmenu.js"></script>


    <script src="asset/js/jquery.scrollUp.js"></script>
</asp:Content>

