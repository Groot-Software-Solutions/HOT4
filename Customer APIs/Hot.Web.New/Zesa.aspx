<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Zesa.aspx.vb" Inherits="Zesa" MasterPageFile="~/main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page">
        <div class="page secondary ">
            <div class="page-header">
                <div class="page-header-content">
                    <h1>Zesa Token<small>Purchase</small></h1>
                    <a href="javascript:back();" class="back-button big page-back"></a>
                </div>

            </div>


            <div class="col-lg-12 col-md-12">


                <div id="zesa-purchase">
                    <div class="row " style="display: flex; align-items: center; margin-bottom: 30px">
                        <h3 style="margin-right: 60px">Select Currency</h3>
                        <div>

                            <input type="radio" id="ZESAZiG" name="ZESA" value="ZesaZiG" checked />
                            <label for="ZESAZiG">ZiG</label>
                        </div>
                        <div style="margin-right: 130px">
                            <input type="radio" id="ZESAUSD" name="ZESA" value="ZesaUSD" />
                            <label for="ZESAUSD">USD</label>
                        </div>



                    </div>
                    <div class="row ">
                        <div class="col-lg-8 col-md-12">
                            <h3 style="text-align: center;">Meter Number</h3>
                            <div class="form-box input-control text">
                                <input type="text" name="txtMeterNumber" placeholder="Meter Number" id="txtMeterNumber" maxlength="11" style="text-align: center;" />
                                <button class="btn-clear" tabindex="-1" type="button" />
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-lg-8 col-md-12">
                            <h3 style="text-align: center;">Token Amount</h3>
                            <div class="form-box input-control text">
                                <input type="number" name="txtAmount" placeholder="Token Amount" id="txtAmount" style="text-align: center;" min="50" max="50000" />
                                <button class="btn-clear" tabindex="-1" type="button" />
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-lg-8 col-md-12">
                            <h3 style="text-align: center;">Mobile Number</h3>
                            <div class="form-box input-control text">
                                <input type="text" name="txtMoibleNumber" placeholder="Mobile Number" id="txtMoibleNumber" maxlength="10" style="text-align: center;" />
                                <button class="btn-clear" tabindex="-1" type="button" />
                            </div>
                        </div>
                    </div>
                    <div class="row" id="zesaConfirm">
                        <div class="col-lg-8 col-md-12">
                            <button id="btnVerify" class="Hot-btn form-box" style="background: #ce2127; font-size: 18px; height: 72px !important; padding: 26px 27px !important; color: #ffffff; position: relative;">
                                Purchase Token</button>
                        </div>
                    </div>
                </div>
            </div>
            <p>
                <br />
            </p>
            <div id="zesa-confirm">
                <div class="row " style="text-align: center">
                    <div class="span8">
                        <h2>
                            <br />
                            Confirm Meter Details</h2>
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
                                <td>Meter Number</td>
                                <td id="dtmeternumber">11001100101</td>

                            </tr>
                            <tr>
                                <td>Currency</td>
                                <td id="dtcurrency">ZiG</td>

                            </tr>
                            <tr>
                                <td>Amount</td>
                                <td id="dtamount">2000.00</td>

                            </tr>
                            <tr>
                                <td>Token to be sent to </td>
                                <td id="dtmobile">0772 929 223</td>

                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class=" row">
                    <div class="span8">
                        <p>
                            <br />
                            <i class=" icon icon-warning fg-color-orangeDark"></i>Please note that Zesa Tokens can not be refunded or transfered, please ensure that the details are correct before confirming the transaction.
                        </p>
                    </div>
                </div>
                <div class="row">
                    <div class="span8">
                        <button id="btnConfirm" class="button place-right" style="margin-right: 29%;">Confirm Purchase Token</button>
                    </div>
                </div>
            </div>
            <div id="zesa-success">
                <div class=" row">
                    <div class="span8">
                        <h2 style="text-align: center;"><i class="icon icon-checkmark fg-color-green"></i>Transaction Successful</h2>
                        <p>
                            <i class=" icon icon-info fg-color-blue"></i>Zesa Tokens has been sent by SMS to the number provided. If you do not receive the token on you phone you can check the tokens page find the most recent tokens generated on the account.
                        </p>
                    </div>
                </div>
            </div>
            <div id="zesa-pending">
                <div class=" row">
                    <div class="span8">
                        <h2 style="text-align: center;"><i class=" icon icon-info fg-color-orangeDark"></i>Transaction Submitted for Processing</h2>
                        <p>
                            ZESA Token purchase has been submitted to ZESA processing. We will send you a SMS with the ZESA Token to the mobile number you provided once the transaction has been processed by ZESA. You can check the tokens page to see the progress of the transaction if you dont receive a SMS in the next 15 minutes.
                                    <br />
                            <br />
                            <i class="icon icon-warning fg-color-red"></i>Please do not attempt to redo this transaction before it is confirmed that the transaction failed.
                        </p>
                    </div>
                </div>
            </div>
            <div id="zesa-loading">
                <div class="row">
                    <div class="span2 offset4">
                        <img src="images/preloader-w8-cycle-black.gif" width="32" />
                    </div>
                </div>
            </div>
            <div id="zesa-failed">
                <div class=" row">
                    <div class="span8">
                        <h2 style="text-align: center;"><i class=" icon icon-info fg-color-red"></i>Transaction Failed</h2>
                        <p id="zesa-error-details"></p>
                        <p>
                            ZESA Token purchase has failed. Please check the transaction page if the issue was a Zesa Provider error to see if the token was not proccessed.
                                    <br />
                            <br />
                            <i class="icon icon-warning fg-color-red"></i>Please do not attempt to redo this transaction before you are sure that the transaction failed.
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        var AccessCode = siteCookie("UID");
        var Password = siteCookie("Pwd");

        addLoadEvent(function () {
            showPurchase();
            setInterval(function () { checkStatusOnline(); }, 30000);
        });
        function hideAll() {
            $("#zesa-purchase").hide();
            $("#zesa-confirm").hide();
            $("#zesa-success").hide();
            $("#zesa-failed").hide();
            $("#zesa-loading").hide();
            $("#zesa-pending").hide();
        }
        function showZesaLoading() {
            hideAll();
            $("#zesa-loading").show(300);
        }
        function showPurchase() {
            hideAll();
            $("#zesa-purchase").show(300);
        }
        function showConfirmDetails() {
            hideAll();
            $("#zesa-confirm").show(300);
        }
        function showZesaSuccess() {
            hideAll();
            $("#zesa-success").show(300);
        }
        function showZesaFailure() {
            hideAll();
            $("#zesa-failed").show(300);
        }

        function showZesaPending() {
            hideAll();
            $("#zesa-pending").show(300);
        }

        function ConfirmDetails() {
            // VerifyForm
            var MeterNumber = $("#txtMeterNumber").val();
            var Amount = $("#txtAmount").val();
            var TargetNumber = $("#txtMoibleNumber").val();
            var selectedCurrency = document.querySelector('input[name="ZESA"]:checked').value;
            var Currency = (selectedCurrency == "ZesaZiG") ? "ZiG" : "USD";

            $("#dtmeternumber").html(MeterNumber);
            $("#dtamount").html(number_formatted(Amount));
            $("#dtmobile").html(number_phone(TargetNumber));
            $("#dtcurrency").html(Currency);
            Confirm_Zesa_Customer(MeterNumber, AccessCode, Password,);
        }

        function PurchaseToken() {
            var MeterNumber = $("#txtMeterNumber").val();
            var Amount = $("#txtAmount").val();
            var TargetNumber = $("#txtMoibleNumber").val();

            var selectionValue = document.querySelector('input[name="ZESA"]:checked').value;

            if (selectionValue == "ZesaZiG") {
                Recharge_Zesa(MeterNumber, TargetNumber, Amount, AccessCode, Password);
            }
            else {
                Recharge_Zesa_USD(MeterNumber, TargetNumber, Amount, AccessCode, Password);
            }
        }

        $("#btnVerify").click(function () {
            ConfirmDetails();
        })
        $("#btnConfirm").click(function () {
            PurchaseToken();
        })




    </script>
    </div>

</asp:Content>
