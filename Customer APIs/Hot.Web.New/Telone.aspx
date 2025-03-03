<%@ Page Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Telone.aspx.vb" Inherits="Telone" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page">
        <div class="page secondary ">
            <div class="page-header">
                <div class="page-header-content">
                    <h1>Telone Bundle<small>Purchase</small></h1>
                    <a href="javascript:back();" class="back-button big page-back"></a>
                </div>
            </div>

            <div class="page-region">
                 <div class="col-lg-8 col-md-12">
                    <div class="row " style="display: flex; align-items: center; margin-bottom: 30px">
                        <h3 style="margin-right: 60px">Select Currency</h3>
                        <div>

                            <input type="radio" id="TeloneZiG" name="Telone" value="TeloneZiG" onclick="LoadBundlesOnchange()" checked />
                            <label for="TeloneZiG">ZiG</label>
                        </div>
                        <div style="margin-right: 130px">
                            <input type="radio" id="TeloneUSD" name="Telone" onclick="LoadBundlesOnchange()" value="TeloneUSD" />
                            <label for="TeloneUSD">USD</label>
                        </div>
                    </div>
                    <div id="telone-purchase">
                        <div class="row ">
                             <div class="col-lg-8 col-md-12">
                                <h3 style="text-align: center;">Account Number</h3>
                                <div class="form-box input-control text">
                                    <input type="text" name="txtaccountNumber" placeholder="Account Number" id="txtaccountNumber" maxlength="11" style="text-align: center;" />
                                    <button class="btn-clear" tabindex="-1" type="button" />
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-lg-8 col-md-12">
                                <h3 style="text-align: center;">Bundle</h3>

                                <div class="form-box input-control select">
                                    <select id="txtTeloneBundle"></select>

                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-lg-8 col-md-12">
                                <h3 style="text-align: center;">Notification Mobile Number</h3>
                                <div class="form-box input-control text">
                                    <input type="text" name="txtMoibleNumber" placeholder="Mobile Number" id="txtMoibleNumber" maxlength="10" style="text-align: center;" />
                                    <button class="btn-clear" tabindex="-1" type="button" />
                                </div>
                            </div>
                        </div>
                        <div class="row" id="teloneConfirm">
                             <div class="col-lg-8 col-md-12">
                                <button id="btnVerify" class="Hot-btn" style="background: #ce2127; font-size: 18px; height: 72px !important;
                                padding: 26px 27px !important; color: #ffffff; position: relative;"">Purchase Bundle</button>
                            </div>
                        </div>
                    </div>
                    <div id="telone-confirm">
                        <div class="row " style="text-align: center">
                            <div class="span8">
                                <h2>
                                    <br />
                                    Confirm Account Details</h2>
                                <h3 id="customerName"></h3>
                                <h2>
                                    <br />
                                </h2>
                            </div>
                        </div>
                        <div class="row">
                            <table class="table bordered offset1 span6" style="text-align: center">
                                <tbody>
                                    <tr>
                                        <td>Currency</td>
                                        <td id="dtcurrency">{selectionValue}</td>

                                    </tr>
                                    <tr>
                                        <td>Account Number</td>
                                        <td id="dtaccountnumber">11001100101</td>

                                    </tr>
                                    <tr>
                                        <td>Bundle</td>
                                        <td id="dtamount">2000.00</td>

                                    </tr>
                                    <tr>
                                        <td>Confirmation to be sent to </td>
                                        <td id="dtmobile">0772 397 464</td>

                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class=" row">
                            <div class="span8">
                                <p>
                                    <br />
                                    <i class=" icon icon-warning fg-color-orangeDark"></i>Please note that TelOne can not be refunded or transfered, please ensure that the details are correct before confirming the transaction.
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="span8">
                                <button id="btnConfirm" class="button place-right" style="margin-right: 29%;">Confirm Bundle Purchase</button>
                            </div>
                        </div>
                    </div>
                    <div id="telone-success">
                        <div class=" row">
                            <div class="span8">
                                <h2 style="text-align: center;"><i class="icon icon-checkmark fg-color-green"></i>Transaction Successful</h2>
                                <p>
                                    <i class=" icon icon-info fg-color-blue"></i>Telone confirmation has been sent by SMS to the number provided. If you do not receive the voucher on you phone you can check the sold pin page find the most recent vouchers generated on the account.
                                </p>
                            </div>
                        </div>
                    </div>
                    <div id="telone-pending">
                        <div class=" row">
                            <div class="span8">
                                <h2 style="text-align: center;"><i class=" icon icon-info fg-color-orangeDark"></i>Transaction Submitted for Processing</h2>
                                <p>
                                    Telone purchase has been submitted to Telone processing. We will send you a SMS with the Telone to the mobile number you provided once the transaction has been processed by Telone. You can check the pins page to see the progress of the transaction if you dont receive a SMS in the next 15 minutes.
                                    <br />
                                    <br />
                                    <i class="icon icon-warning fg-color-red"></i>Please do not attempt to redo this transaction before it is confirmed that the transaction failed.
                                </p> 
                            </div>
                        </div>
                    </div>
                    <div id="telone-loading">
                        <div class="row">
                            <div class="span2 offset4">
                                <img src="images/preloader-w8-cycle-black.gif" width="32" />
                            </div>
                        </div>
                    </div>
                    <div id="telone-failed">
                        <div class=" row">
                            <div class="span8">
                                <h2 style="text-align: center;"><i class=" icon icon-info fg-color-red"></i>Transaction Failed</h2>
                                <p id="telone-error-details"></p>
                                <p>
                                    Telone purchase has failed. Please check the transaction page if the issue was a Telone Provider error to see if the recharge was not proccessed.
                                    <br />
                                    <br />
                                    <i class="icon icon-warning fg-color-red"></i>Please do not attempt to redo this transaction before you are sure that the transaction failed.
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var AccessCode = siteCookie("UID");
        var Password = siteCookie("Pwd");

        addLoadEvent(function () {

            LoadBundlesOnchange();
            checkStatusOnline();
            setInterval(function () { checkStatusOnline(); }, 30000);
            showPurchase();
        });

        function hideAll() {
            $("#telone-purchase").hide();
            $("#telone-confirm").hide();
            $("#telone-success").hide();
            $("#telone-failed").hide();
            $("#telone-loading").hide();
            $("#telone-pending").hide();
        }
        function showTeloneLoading() {
            hideAll();
            $("#telone-loading").show(300);
        }
        function showPurchase() {
            hideAll();
            $("#telone-purchase").show(300);
        }
        function showConfirmDetails() {
            hideAll();
            $("#telone-confirm").show(300);
        }
        function showTeloneSuccess() {
            hideAll();
            $("#telone-success").show(300);
        }
        function showTeloneFailure() {
            hideAll();
            $("#telone-failed").show(300);
        }

        function showTelonePending() {
            hideAll();
            $("#telone-pending").show(300);
        }

        function PurchaseToken() {
            var AccountNumber = $("#txtaccountNumber").val();
            var ProductId = $("#txtTeloneBundle").val();
            var TargetNumber = $("#txtMoibleNumber").val();
            var selectionValue = document.querySelector('input[name="Telone"]:checked').value;
            if (selectionValue == "TeloneZiG") {
                Recharge_TelOne_Bundle(AccountNumber, TargetNumber, ProductId, AccessCode, Password);
            }
            else {
                Recharge_TelOne_USD_Bundle(AccountNumber, TargetNumber, ProductId, AccessCode, Password);
            }
        }
        $("#btnVerify").click(function () {
            ConfirmDetails();
        })
        $("#btnConfirm").click(function () {
            PurchaseToken();
        })

        //Loading ZiG and USD bundles 
        function LoadBundlesOnchange() {
            var selectionValue = document.querySelector('input[name="Telone"]:checked').value;
            if (selectionValue == "TeloneZiG") {
                loadtelonebundles();
            }
            else {
                loadtelonebundlesUSD();
            }
        }

        //Confirming account details 
        function ConfirmDetails() {
            // VerifyForm
            var AccountNumber = $("#txtaccountNumber").val();
            var BundleProduct = $("#txtTeloneBundle option:selected").text();
            var TargetNumber = $("#txtMoibleNumber").val();
            $("#dtaccountnumber").html(AccountNumber);
            $("#dtamount").html(BundleProduct);
            $("#dtmobile").html(number_phone(TargetNumber));
            Confirm_Telone_Customer(AccountNumber, AccessCode, Password);

        }


    </script>

    	<!-- jquery meanmenu js -->
<script src="js/assets/asset/js/jquery.meanmenu.js"></script>


<script src="js/assets/asset/js/jquery.scrollUp.js"></script>

</asp:Content>
