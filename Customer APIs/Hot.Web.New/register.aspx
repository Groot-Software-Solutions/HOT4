<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="register.aspx.vb" Inherits="register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>	    (function () {
            var _fbq = window._fbq || (window._fbq = []);
            if (!_fbq.loaded) {
                var fbds = document.createElement('script');
                fbds.async = true;
                fbds.src = '//connect.facebook.net/en_US/fbds.js';
                var s = document.getElementsByTagName('script')[0];
                s.parentNode.insertBefore(fbds, s);
                _fbq.loaded = true;
            }
        })();
        window._fbq = window._fbq || [];
        window._fbq.push(['track', '6021198579248', { 'value': '0.01', 'currency': 'USD' }]);
    </script>
    <noscript>
        <img height="1" width="1" alt="" style="display: none" src="https://www.facebook.com/tr?ev=6021198579248&amp;cd[value]=0.01&amp;cd[currency]=USD&amp;noscript=1" /></noscript>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Body File -->
    <div class="page secondary">
        <div class="page-header">
            <div class="page-header-content">
                <h1>Sign<small>Up</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
        </div>


        <div class="page-region">
            <div class="page-region-content">
                <div class="span10">

                    <div class="place-left span7">
                        <form id="frmRegister" runat="server">

                            <table class="no-borders no-bottom-margins">
                                <tr>
                                    <td style="width: 105px;">
                                        <strong><small>Account Type</small></strong>
                                    </td>
                                    <td>

                                        <div class="input-control radio margin10" data-role="input-control">
                                            <label>
                                                Corporate 
                                            <input type="radio" name="txtAcctype" value="1" onchange="$('#<%= txtAccType2.UniqueID %>').text('1');" />
                                                <span class="check"></span>
                                                </br><small>for known regular recharges </br>e.g. employees & subscribers</small>
                                            </label>
                                        </div>
                                        <div class="input-control radio margin10" data-role="inputs-control">
                                            <label>
                                                Retail 
                                            <input type="radio" name="txtAcctype" value="2" onchange="$('#<%= txtAccType2.UniqueID %>').text('2');" />
                                                <span class="check"></span>
                                                </br><small>for selling to many </br>unknown customers</small>
                                            </label>
                                        </div>
                                        <div class="input-control radio margin10" data-role="input-control">
                                            <label>
                                                Individuals 
                                            <input type="radio" name="txtAcctype" value="1" onchange="$('#<%= txtAccType2.UniqueID %>').text('3');" />
                                                <span class="check"></span>
                                                </br><small>for personal use and recharging
                                                    <br />
                                                    friends and family</small>
                                            </label>
                                        </div>

                                    </td>
                                    <td style="width: 5px;">
                                        <input name="txtAccType2" id="txtAccType2" class="txtAccType2" type="text" placeholder="1" runat="server" style="display: none;" value="1" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 105px;">
                                        <strong><small>Account Name</small></strong>
                                    </td>
                                    <td>
                                        <div class="input-control text no-bottom-margin">
                                            <input name="txtAccName" id="txtAccName" type="text" placeholder="Your Organisation (Pvt) Ltd" runat="server" />
                                            <button class="btn-clear" tabindex="-1" type="button">
                                            </button>
                                        </div>
                                    </td>
                                    <td style="width: 5px;"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong><small>National ID</small></strong>
                                    </td>
                                    <td>
                                        <div class="input-control text no-bottom-margin">
                                            <input name="txtID" id="txtID" type="text" placeholder="00-000000X00 ID of Account controller" runat="server" />
                                            <button class="btn-clear" tabindex="-1" type="button">
                                            </button>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong><small>Email Address</small></strong>
                                    </td>
                                    <td>
                                        <div class="input-control text no-bottom-margin">
                                            <input class="fnIsRegistered" name="txtEmail" id="txtEmail" type="text" placeholder="accountant@your_organisation.co.zw" runat="server">
                                            <button class="btn-clear" tabindex="-1" type="button"></button>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <td>
                                        <strong><small>Password</small></strong>
                                    </td>
                                    <td>
                                        <div class="input-control password no-bottom-margin">
                                            <input name="txtPassword" id="txtPassword" type="password" placeholder="Enter password" runat="server">
                                            <button class="btn-reveal" tabindex="-1" type="button"></button>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong><small>Confirm</small></strong>
                                    </td>
                                    <td>
                                        <div class="input-control password no-bottom-margin">
                                            <input name="txtConfirm" id="txtConfirm" type="password" placeholder="Confirm" runat="server">
                                            <button class="btn-reveal" tabindex="-1" type="button"></button>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong><small>Referred By</small></strong>
                                    </td>
                                    <td>
                                        <div class="input-control select">
                                            <select name="txtReferredBy" id="txtReferredBy" runat="server" onchange="javascript:isOther($(this).val());">
                                                <option value="Online Advert" selected>Online Advert</option>
                                                <option value="Office Staff">Office Staff</option>
                                                <option value="A Friend">A Friend</option>
                                                <option value="Facebook">Facebook</option>
                                                <option value="Other">Other</option>
                                                <option value="None">None</option>
                                            </select>
                                        </div>

                                    </td>
                                    <td></td>
                                </tr>
                                <tr id="txtOtherRef">
                                    <td>
                                        <strong><small>Ref by Other</small></strong>
                                    </td>
                                    <td>
                                        <div class="input-control text no-bottom-margin">
                                            <input name="txtReferredByOther" id="txtReferredByOther" type="text" placeholder="Online Advert" runat="server" />
                                            <button class="btn-clear" tabindex="-1" type="button"></button>
                                        </div>

                                    </td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <td colspan="2">
                                        <br />
                                        <label class="input-control checkbox">
                                            <input name="terms" type="checkbox" id="terms">
                                            <span class="helper">
                                                <small>I accept and have read the HOTRecharge <a href="./">Terms & Conditions</a><br />
                                                </small>
                                            </span>
                                        </label>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btnSubmit" class=" button place-right" runat="server" Text="Sign Up" />
                                         <div class="g-recaptcha place-left" data-sitekey="6LedkuYUAAAAAIfH7j6l200bs46DPVYXnjYKbsX9"></div>
                                    </td>
                                </tr>
                            </table>

                        </form>
                        <script src="https://www.google.com/recaptcha/api.js" async defer></script>
                    </div>

                    <div class=" span3 place-left">
                        <pre style="line-height: 0pt; padding-bottom: 0px;">							
							<h4>Trouble registering?</h4>
						</pre>
                        <h2 id="contactus"><strong><small>Contact</small> Us</strong></h2>
                        <address>
                            <strong>The Cottage</strong><br />
                            No 17 Arundel Road<br />
                            Alexandra Park
                               Harare<br />
                            <abbr title="Telephone"><i class="icon-phone"></i></abbr>
                            (242) 700 007<br />
                            <abbr title="Fax Machine"><i class="icon-printer"></i></abbr>
                            (242) 700 413<br />
                            <abbr title="Mobile"><i class="icon-mobile"></i></abbr>
                            (+263) 772 929 223<br />
                            <abbr title="Email"><i class="icon-mail"></i></abbr>
                            <a class="fg-color-darken" href="mailto:register@hot.co.zw">register@hot.co.zw</a>
                        </address>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $("#txtOtherRef").hide();
        function isOther(referred) {
            console.log(referred);
            if (referred == "Other") {
                $("#txtOtherRef").show();
            } else {
                $("#txtOtherRef").hide();
            }
        }

        var checkWebMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnIsRegistered";
        jQuery.validator.addClassRules("fnIsRegistered", {
            required: true,
            email: true,
            remote: {
                url: checkWebMethod,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                cache: false,
                tryCount: 0,
                retryLimit: 2,
                timeout: 1500,
                dataFilter: function (msg) {
                    var data = JSON && JSON.parse(msg) || $.parseJSON(msg);
                    console.log(data);
                    return ((data.d == '"True"') ? true : false);
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (textStatus == 'timeout') {
                        this.tryCount++;
                        if (this.tryCount <= this.retryLimit) {
                            //try again
                            $.ajax(this);
                            return true;
                        }
                        return true;
                    }

                    submitted = validator.formSubmitted;
                    validator.prepareElement(element);
                    validator.formSubmitted = submitted;
                    validator.successList.push(element);
                    delete validator.invalid[element.name];
                    validator.showErrors();

                }
            },
            minlength: 6
        });

        var form = $("#<%= frmRegister.ClientID %>");
        form.validate({

            rules: {
			<%= txtAccName.UniqueID %>: {
            required: true,
            minlength: 6
        },
			<%= txtID.UniqueID %>: {
                required: true,
                minlength: 4
            },

			<%=txtPassword.UniqueID %>: {
                required: true,
                minlength: 6
            },
			<%=txtConfirm.UniqueID %>: {
                required: true,
                minlength: 6,
                equalTo: "#<%=txtPassword.ClientID %>"
            },
            terms: "required"
		},
            messages: {
			<%=txtID.UniqueID %>: {
            required: "Enter your ID Number",
            rangelength: jQuery.format("Enter at least {0} characters")
        },
			<%=txtAccName.UniqueID %>: {
                required: "Enter a Account Name",
                rangelength: jQuery.format("Enter at least {0} characters")
            },
			<%=txtPassword.UniqueID %>: {
                required: "Provide a password",
                rangelength: jQuery.format("Enter at least {0} characters")
            },
			<%=txtConfirm.UniqueID %>: {
                required: "Repeat your password",
                minlength: jQuery.format("Enter at least {0} characters"),
                equalTo: "Enter the same password as above"
            },
			<%=txtEmail.UniqueID %>: {
                required: "Enter your email address",
                minlength: "Enter a valid email address",
                remote: "This email is already registered"
            },
            terms: "Please Agree to the Terms"
		},
            errorPlacement: function (error, element) {
                error.appendTo(element.parent());
            }
		
	});

    </script>


</asp:Content>

