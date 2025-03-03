<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="resetemail.aspx.vb" Inherits="resetemail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <!-- Body File -->
<div class="page secondary ">
        <div class="page-header">
            <div class="page-header-content">
                <h1>Reset<small>Password</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
        </div>
        
        <div class="page-region">
            <div class="page-region-content">
            	<div class="span9">                	
                    <form runat="server" class="span3 offset3 grpReset" id="frmReset">
						<h3 style="text-align:center;" >User Login </h3>
						<div class="input-control text"> 
							<input  runat="server" type="text" name="txtEmail"  id="txtEmail" placeholder="hot@hot.co.zw" style="text-align:center;" /> 
							<button class="btn-clear" tabindex="-1" type="button"/>
						</div> 
					    <asp:button id="cmdGo" class=" button place-right" runat="server" text="Send Reset Email" style="margin-right:20%;"/>
                       
					</form >
                    
                </div>
				<div class="span9">
                   <div class="span3" id="grpError" style="border: 1px solid; margin: 0 auto;display:none;">
                        <h4 style="width: 98%; background-color: red;padding: 5px; text-align: center; margin: 2px;">Error</h4>
                        <p style="text-align: center; margin: 10px;">
                            Invalid Email<br>Please check email and try again. <br />
                            <a href="resetemail.aspx">Retry</a>
                        </p>
                    </div>
                </div>
                <div class="span9">
                   <div class="span3" id="grpSuccess" style="border: 1px solid; margin: 0 auto;display:none;">
                        <h4 style="width: 98%; background-color: green;padding: 5px; text-align: center; margin: 2px;color:White;">Email Sent</h4>
                        <p style="text-align: center; margin: 10px;">
                            Reset Email Sent<br>Please check your mailbox for reset email.
                        </p>
                    </div>
                </div>
            </div>
        </div>
		
</div>
<script type="text/javascript">
    
    

var form = $("#<%= frmReset.ClientID %>");
        form.validate({
        
		rules: {
			<%=txtEmail.UniqueID %>: {
				required: true,
				minlength: 6
			}
		},
		messages: {
			
			<%=txtEmail.UniqueID %>: {
				required: "Enter your email address",
				minlength: "Enter a valid email address",	
                remote:"This email is already registered"			
			}
		},
		errorPlacement: function(error, element) {			
				error.appendTo ( element.parent() );			
		}
		
	});
	

</script>

</asp:Content>

