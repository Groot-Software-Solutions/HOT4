<%@ Page Title="" Language="VB" MasterPageFile="~/Main.master" AutoEventWireup="false" CodeFile="Topup.aspx.vb" Inherits="Topup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Body File -->
<div class="page secondary ">
        <div class="page-header">
            <div class="page-header-content">
                <h1>Credit<small>Your Account</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
        </div>
        
        <div class="page-region">
            <div class="page-region-content">
            	<div class="col-lg-12 col-md-12">                	
                    <p>
						This will initiate an online card payment topup using Vpayments, an online payment service for ZimSwitch bank cards. More information is available here <a href="https://secure.zss.co.zw/vpayments/" target="_blank">https://secure.zss.co.zw/vpayments/</a>
					</p>
					<form runat="server" id="frmTopup" class="span3 offset3">
						<h3 style="text-align:center;" >TopUp Amount</h3>
						<div class="form-box input-control text"> 
							<input  runat="server" type="text" name="txtAmount" placeholder="Topup Amount" id="txtAmount" style="text-align:center;" /> 
							<button class="btn-clear" tabindex="-1" type="button"/>
						</div> 
						<input  runat="server" type="submit" id="cmdGo" value="Go Topup" class="Hot-btn place-left"  /> 
					</form >
                </div>
				
            </div>
        </div>
		
</div>
<script>
    $("#<%= frmTopup.ClientID %>").ready(function () {
        $("#<%= frmTopup.ClientID %>").validate({
            ignore: ".ignore",
            rules: {
                <%= txtAmount.UniqueID %>: {
                    required: true,
                    number: true,
                    min:1,
                    max:10000000
                }
            },
            messages: {

                <%= txtAmount.UniqueID %>: {
                    required: "Enter a amount to Topup",
                    min: "Please enter an amount between $1 and 10000000",
                    max: "Please enter an amount between $1 and 10000000"
                }
            },
            errorPlacement: function (error, element) {
                error.appendTo(element.parent());
            }

        });
    });
</script>
  
<!-- Page end  -->
    	<!-- jquery meanmenu js -->
<script src="js/assets/asset/js/jquery.meanmenu.js"></script>


<script src="js/assets/asset/js/jquery.scrollUp.js"></script>

</asp:Content>

