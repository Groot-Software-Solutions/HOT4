<%@ Page Language="VB" AutoEventWireup="false" CodeFile="netonepins.aspx.vb" Inherits="netonepins" MasterPageFile="~/main.master" %>



<asp:Content ID="head1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="page secondary ">
        <div class="page-header">
            <div class="page-header-content">
                <h1>Self Top Up</h1>
                <a href="javascript:back();" class="back-button big page-back"></a> 
            </div>
           
        </div>
        
        <div class="page-region">
            <div class="page-region-content">
                <div class="span9" style="min-height:450px;" runat="server" id="txtContent">                	
                    <p>
						This will initiate an online merchant payment on the number you provide. 
					</p>
					<form runat="server" id="frmSelfTopup" class="span4 offset2" >
                        <h3 style="text-align:center;" >Econet number to bill</h3>
						<div class="input-control text"> 
							<input autocomplete="off" runat="server" type="text" name="txtEcoNumber" placeholder="Ecocash Number" id="txtEcoNumber" style="text-align:center;" onkeyup="javascript: number_phone_pos(this);" /> 
							<button class="btn-clear" tabindex="-1" type="button"/>
						</div> 
                        <h3 style="text-align:center;" >Number to Recharge</h3>
						<div class="input-control text"> 
							<input autocomplete="off" runat="server" type="text" name="txtNumber" placeholder="Topup Number" id="txtNumber" style="text-align:center;" onkeyup="javascript: number_phone_pos(this);" /> 
							<button class="btn-clear" tabindex="-1" type="button"/>
						</div>  
						<h3 style="text-align:center;" >Bundle to Recharge</h3>
						<div class="input-control select"> 
							<select  runat="server" type="text" name="txtpins"  id="txtpins" style="text-align:center;">
                            </select>
							
						</div>  
               
                        <%--<div class="g-recaptcha" data-sitekey="6LedkuYUAAAAAIfH7j6l200bs46DPVYXnjYKbsX9"></div>--%>
                        <label  runat="server" id="lblMessage" style="text-align: center; display: block;"></label> 
						<input  runat="server" type="submit" id="cmdSelfGo" value="Go Topup" class="button place-right" style="margin-right:29%;" /> 
                        
					</form >
                    <%--<script src="https://www.google.com/recaptcha/api.js" async defer></script>--%>
                </div>
            </div>
        </div>
		
</div>
    <script>

         

        function number_phone_pos(text) {
            var result = text.value.replace(/[^0-9]/g, "");

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
                $(text).attr("maxlength", "12")
            }

            if (rlen == 10 || (rlen == 11 && isVoipNumber(result))) {
                if (isPhoneNumberValid(result)) {
                    $(text).css("color", "black");
                } else {
                    $(text).css("color", "red");
                }
            }
            else {
                $(text).css("color", "black");
            }
            if (notdecimal && (isPhoneNumberValid(result) && (rlen == 10 && !isVoipNumber(result) || rlen == 11 && isVoipNumber(result)))) {
                $('#ctl00_ContentPlaceHolder1_txtAmount').focus();
            }

            checkform();
        }

        function number_amount(text) {

            var result = text.value.replace(/[^0-9.]/g, "");

            if ((result.split(".").length) > 1) {
                spArr = result.split(".");
                decimals = spArr[1];
                if (decimals.length > 2) { decimals = decimals.substring(0, 2); }
                result = spArr[0] + '.' + decimals;

            }
            text.value = result;
            checkform();
        }

        function isVoipNumber(phonenumber) {
            return (phonenumber.replace(/[^0-9]/g, "").substring(0, 3) == '086');
        }

        function checkform() {
            phonenumber = $('#ctl00_ContentPlaceHolder1_txtNumber').val().replace(/[^0-9]/g, "");
            econumber = $('#ctl00_ContentPlaceHolder1_txtEcoNumber').val().replace(/[^0-9,.]/g, "");

            var isValidEcoCash = (econumber.startsWith("077") || econumber.startsWith("078")) && isPhoneNumberValid(econumber) && econumber.length == 10;
            phonenumbervalid = isPhoneNumberValid(phonenumber);
            var voip = isVoipNumber(phonenumber);


            if ((phonenumber.length == 10 || (phonenumber.length == 11 && voip)) && (amount.length >= 1) && (phonenumbervalid == true) && (isValidEcoCash)) {
                //$('#ctl00_ContentPlaceHolder1_cmdSelfGo').removeAttr("").show();
                $('#ctl00_ContentPlaceHolder1_cmdSelfGo').removeAttr("disabled", "");
                try { if (event.keyCode == 13) { confirmed(); } } catch (e) { };

            } else {
                $('#ctl00_ContentPlaceHolder1_cmdSelfGo').attr("disabled", "");
                //$('#ctl00_ContentPlaceHolder1_cmdSelfGo').hide();
            }
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

    </script>
</asp:Content>


