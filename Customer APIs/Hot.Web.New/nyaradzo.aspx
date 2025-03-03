<%@ Page Language="VB" AutoEventWireup="false" CodeFile="nyaradzo.aspx.vb" Inherits="nyaradzo" MasterPageFile="~/main.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page">
        <div class ="page secondary ">
  <div class="page-header">
                <div class="page-header-content">
                    <h1>Nyaradzo <small>Payment</small></h1>
                    <a href="javascript:back();" class="back-button big page-back"></a>
                </div>
            </div>
        
        <div class="page-region">
                <div class="page-region-content">
            
                    <div id="nyaradzo-purchase">
                        <div class="row ">
                        <div class="col-lg-8 col-md-12">
                                <h3 style="text-align:center;" >Policy Number</h3>
				                <div class="form-box input-control text"> 
					                <input type="text" name="txtMeterNumber" placeholder="Policy Number" id="txtMeterNumber" maxlength="11" style="text-align:center;" /> 
					                <button class="btn-clear" tabindex="-1" type="button"/>
				                </div> 
                            </div>
                            
                        </div>
                        <div class="row">
                           <div class="col-lg-8 col-md-12">
                                <h3 style="text-align:center;" >Payment Amount</h3>
				                <div class="form-box input-control text"> 
					                <input type="number" name="txtAmount" placeholder="Payment Amount" id="txtAmount" style="text-align:center;" min="50" max="50000" /> 
					                <button class="btn-clear" tabindex="-1" type="button"/>
				                </div> 
                            </div>
                            
                        </div>
                        <div class="row">
                            <div class="col-lg-8 col-md-12">
                                <h3 style="text-align:center;" >Notification Mobile Number</h3>
				                <div class="form-box input-control text"> 
					                <input type="text" name="txtMoibleNumber" placeholder="Mobile Number" id="txtMoibleNumber" maxlength="10" style="text-align:center;" /> 
					                <button class="btn-clear" tabindex="-1" type="button"/>
				                </div> 
                            </div>
                        </div>
                        <div class="row" id="nyaradzoConfirm">
                            <div class="col-lg-8 col-md-12">
                                 <button id="btnVerify" class="form-box" style="background: #ce2127; font-size: 18px; height: 72px !important;
                                padding: 26px 27px !important; color: #ffffff; position: relative;">Make Payment</button> 
					        </div>
                        </div>
                    </div>
                    <div id="nyaradzo-confirm">
                        <div class="row " style="text-align:center">
                            <div class="span8">
 <h2><br />Confirm Policy Details</h2>
                            <h3 id="customerName"></h3>
                                <h2><br /> </h2>
                            </div> 
                        </div>
                        <div class="row">
                            <table class="table bordered offset1 span6" style="text-align:center">
                                <tbody>
                                    <tr>
                                        <td>Policy Number</td><td id="dtmeternumber">11001100101</td>
                                        
                                    </tr>
                                    <tr>
                                        <td>Account Status</td><td id="accountStatus">0.00</td>
                                        
                                    </tr>
                                    <tr>
                                        <td>Monthly Premium</td><td id="monthlyPremium">0.00</td>
                                        
                                    </tr>
                                    <tr>
                                        <td>Amount Due</td><td id="amountToBePaid">0.00</td>
                                        
                                    </tr>
                                    <tr>
                                        <td>Payment Amount</td><td id="dtamount">0.00</td>
                                        
                                    </tr>
                                     <tr>
                                        <td>Notification to be sent to </td><td id="dtmobile">0772 929 223</td>
                                        
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class=" row"> 
                            <div class="span8">
                                <p><br />
                                    <i class=" icon icon-warning fg-color-orangeDark"></i> Please note that Nyaradzo Payments can not be refunded or transfered, please ensure that the details are correct before confirming the transaction.
				                </p>
                            </div> 
                        </div>
                        <div class="row" >
                            <div class="span8">
                                 <button id="btnConfirm" class="button place-right" style="margin-right:29%;" >Confirm Payment</button> 
					        </div>
                        </div>
                    </div>
                    <div id="nyaradzo-success">
                          <div class=" row"> 
                            <div class="span8">
                                <h2 style="text-align: center;"><i class="icon icon-checkmark fg-color-green"></i> Transaction Successful</h2>
                                <p>
                                    <i class=" icon icon-info fg-color-blue"></i> Nyaradzo Payment was successful a confirmation has been sent by SMS to the number provided.
				                </p>
                            </div> 
                        </div>
                    </div>
                    <div id="nyaradzo-pending">
                          <div class=" row"> 
                            <div class="span8">
                                <h2 style="text-align: center;"><i class=" icon icon-info fg-color-orangeDark"></i> Transaction Submitted for Processing</h2>
                                <p>
                                     Nyaradzo payment has been submitted to Nyaradzo processing. We will send you a SMS with the confirmation to the mobile number you provided once the transaction has been processed by Nyaradzo. <br /><br /><i class="icon icon-warning fg-color-red"></i> Please do not attempt to redo this transaction before it is confirmed that the transaction failed.
				                </p>
                            </div> 
                        </div>
                    </div>
                    <div id="nyaradzo-loading">
                        <div class="row">
                            <div class="span2 offset4">
                                <img src="images/preloader-w8-cycle-black.gif" width="32" />
                            </div>
                        </div>
                    </div>
                    <div id="nyaradzo-failed">
                          <div class=" row"> 
                            <div class="span8">
                                <h2 style="text-align: center;"><i class=" icon icon-info fg-color-red"></i> Transaction Failed</h2>
                                <p id="nyaradzo-error-details"></p>
                                <p> Nyaradzo Payment has failed. Please check the transaction page if the issue was a Nyaradzo Provider error to see if the payment was not proccessed.
                                    <br /><br /><i class="icon icon-warning fg-color-red"></i> Please do not attempt to redo this transaction before you are sure that the transaction failed.
				                </p>
                            </div> 
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
              $("#nyaradzo-purchase").hide();
              $("#nyaradzo-confirm").hide();
              $("#nyaradzo-success").hide();
              $("#nyaradzo-failed").hide();
              $("#nyaradzo-loading").hide();
              $("#nyaradzo-pending").hide();
          }
          function showNyaradzoLoading() {
              hideAll();
              $("#nyaradzo-loading").show(300);
          }
          function showPurchase(){
              hideAll();
              $("#nyaradzo-purchase").show(300);
          }
          function showConfirmDetails() {
              hideAll();
              $("#nyaradzo-confirm").show(300); 
          }
          function showNyaradzoSuccess() {
              hideAll();
              $("#nyaradzo-success").show(300);
          }
          function showNyaradzoFailure() {
              hideAll();
              $("#nyaradzo-failed").show(300);
          }

          function showNyaradzoPending() {
              hideAll();
              $("#nyaradzo-pending").show(300);
          }

          function ConfirmDetails() {
              // VerifyForm
              var MeterNumber = $("#txtMeterNumber").val();
              var Amount = $("#txtAmount").val();
              var TargetNumber = $("#txtMoibleNumber").val();


              $("#dtmeternumber").html(MeterNumber);
              $("#dtamount").html(number_formatted(Amount));
              $("#dtmobile").html(number_phone(TargetNumber));
               
              Confirm_Nyaradzo_Customer(MeterNumber, AccessCode, Password);
               
          }

          function MakePayment() {
              var MeterNumber = $("#txtMeterNumber").val();
              var Amount = $("#txtAmount").val();
              var TargetNumber = $("#txtMoibleNumber").val();

              Recharge_Nyaradzo(MeterNumber, TargetNumber, Amount, AccessCode,Password);  
          }
          $("#btnVerify").click(function () {
              ConfirmDetails();
          })
          $("#btnConfirm").click(function () {
              MakePayment();
          })

          
         

      </script>
    </div>  
    	<!-- jquery meanmenu js -->
<script src="js/assets/asset/js/jquery.meanmenu.js"></script>


<script src="js/assets/asset/js/jquery.scrollUp.js"></script>
</asp:Content>