<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="reset.aspx.vb" Inherits="reset" %>

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
                    <form runat="server" class="span3 offset3 grpReset" id="frmReset" style="display:none;">
						<h3 style="text-align:center;" >User Login </h3>
						<div class="input-control text"> 
							<input  runat="server" type="text" name="txtUser"  id="txtUser" style="text-align:center;" disabled /> 
							<button class="btn-clear" tabindex="-1" type="button"/>
						</div> 
					    <span>Password</span><br />
                        <div class="input-control password"> 
							<input  runat="server" type="password" name="txtPassword" placeholder="Password" id="txtPassword" /> 
							<button class="btn-clear" tabindex="-1" type="button"/>
						</div>
                        <span>Confirm</span><br />
                        <div class="input-control password"> 
							<input  runat="server" type="password" name="txtConfirm" placeholder="Confirm" id="txtConfirm" /> 
							<button class="btn-clear" tabindex="-1" type="button"/>
						</div>
                        <asp:button id="cmdGo" class=" button place-right" runat="server" text="Reset Password" style="margin-right:29%;"/>
                       
					</form >
                    
                </div>
				<div class="span9">
                   <div class="span3" id="grpError" style="border: 1px solid; margin: 0 auto;display:none;">
                        <h4 style="width: 98%; background-color: red;padding: 5px; text-align: center; margin: 2px;">Error</h4>
                        <p style="text-align: center; margin: 10px;">
                            Invalid Request<br>Please click or copy the link from the email again.
                        </p>
                    </div>
                </div>
            </div>
        </div>
		
</div>
<script type="text/javascript">

   
    function showerror() { 
        $('.grpReset').hide();
        $('#grpError').show();
    }
     function showreset() { 
        $('.grpReset').show();
        $('#grpError').hide();
    }
    

var form = $("#<%= frmReset.ClientID %>");
        form.validate({
        
		rules: {
		
			<%=txtPassword.UniqueID %>: {
				required: true,
				minlength: 6
			},
			<%=txtConfirm.UniqueID %>: {
				required: true,
				minlength: 6,
				equalTo: "#<%=txtPassword.ClientID %>"
			}
		},
		messages: {
			
			<%=txtPassword.UniqueID %>: {
				required: "Provide a password",
				rangelength: jQuery.format("Enter at least {0} characters")
			},
			<%=txtConfirm.UniqueID %>: {
				required: "Repeat your password",
				minlength: jQuery.format("Enter at least {0} characters"),
				equalTo: "Enter the same password as above"
			}
		},
		errorPlacement: function(error, element) {			
				error.appendTo ( element.parent() );			
		}
		
	});
	

</script>
</asp:Content>

