<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="fbcom.aspx.vb" Inherits="fbcom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">

    window.fbAsyncInit = function () {
        FB.init({ appId: '538648886250768', status: true, cookie: true, xfbml: true });
        FB.Event.subscribe('edge.create', function (href, widget) {
            console.log('FB Liked');
            savestatus(1);
        });
        FB.Event.subscribe('edge.remove', function (href, widget) {
            console.log('FB Liked');
            savestatus(1);
        });

    };
    (function () {
        var e = document.createElement('script');
        e.type = 'text/javascript';
        e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
        e.async = true;
        document.getElementById('fb-root').appendChild(e);

    } ());

  /* (function(d, s, id) {
  var js, fjs = d.getElementsByTagName(s)[0];
  if (d.getElementById(id)) return;
  js = d.createElement(s); js.id = id;
  js.src = "//connect.facebook.net/en_US/all.js#xfbml=1&appId=538648886250768";
  fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
    // callback that logs arguments
    var page_like_callback = function (url, html_element) {
        console.log('FB Liked');
        savestatus(1);
    }	
    */				   
</script>
<div class="page secondary ">
        <div class="page-header">
            <div class="page-header-content">
                <h1>Facebook<small>Promotion</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
        </div>
        
        <div class="page-region">
            <div class="page-region-content">
                <div class="span11" >
                	<h2>How <small>the promotion works</small></h2>
                    <ol style="margin-right:25px;">
                        <li>Like our facebook page</li>
                        <li>Enter your email address and allow us to contact you once a month. You will get a verification email from us</li>
                        <li>Enter your contact mobile number, which we will topup</li> 
                        <li>Hot will top-up that mobile number with 10c</li>
					</ol>
                    <p>Terms and conditions:<br />
                    <small>Econet or netone numbers only for 10c free airtime.
                    Offers are only a once only per account, user and number registered<br/>
                    Promotion ends 30 April 2014 </small></p>
                    <br />
                </div>
            	<div class ="span11">
					<div id="grpFB" class="span2" style="float: left;">
						
						

						<h3><i id="txtfbicon" class="icon-info fg-color-blue"></i>  Step 1: <small>Like Our Facebook Page</small></h3>	
                         <!-- <div style="z-index:3000;" class="fb-like" data-href="https://www.facebook.com/HOTRecharge" data-layout="box_count" data-action="like" data-show-faces="true" data-share="false"></div>
                         -->
                         <div style="z-index:3000;"><fb:like href="https://www.facebook.com/HOTRecharge" layout="box_count" action="like" show_faces="false" share="false"></fb:like></div>
                         
					</div>
					<div id="grpEmail" class="span4" style="float: left;">
						<h3><i id="txtvficon" class="icon-info fg-color-blue"></i>  Step 2: <small>Verify Your Email address</small></h3>
						<form id="frmFBLike" class="span4" onsubmit="javascript:return false;" > 
							Email Address <br /> 
							<div class="input-control text ">
								<input name = "txtSubName" id ="txtSubName" type="email" placeholder="hot@hot.co.zw"/>
								<button class="btn-clear" tabindex="-1" type="button"/>
							</div>
							<div>
							<label class="input-control checkbox">
								<input name="terms" type="checkbox" id="terms" checked />
								<span class="helper">
									<small>Send me promotional offers from HOT Recharge.</small>
								</span>
							</label> 
                            </div>
								<button id="btnSubmit" class="place-right" onclick="javascript:verifyemail();" ><i class="icon-mail"></i>Send Verification Email</button>
						</form>
					</div>
					<div id="grpContact" class="span4" style="float: left;" onsubmit="javascript:return false;">
						<h3><i id="txtcnicon" class="icon-info fg-color-blue"></i>  Step 3: <small>Confirm Contact Number</small></h3>
						<form id="frmNumberVerify" class="span4"> 
							Contact Number <br /> 
							<div class="input-control text"> 
								<input name="txtPhoneNumber" id="txtPhoneNumber" type="text" placeholder="Phone Number" disabled/>
								<button class="btn-clear" tabindex="-1" type="button" />
							</div>
							<button id="btnSendAirtime" class="place-right" onclick="javscript:contactnumb();" disabled><i class="icon-power"></i>Send Airtime</button>
						</form>
					</div>
<script>

    var addfbvalidator = $("#frmFBLike").validate({

        rules: {
            txtSubName: {
                required: true,
                minlength: 4
            },
            terms: "required"
        },
        messages: {

            txtSubName: {
                required: "Enter a Email Address",
                rangelength: jQuery.format("Enter at least {0} characters")
            },
            terms: "To receive the 10c airtime you must subscribe to the promotional offers"
        },
        errorPlacement: function (error, element) {
            error.appendTo(element.parent());
        }
    });
    var addnumbervalidator = $("#frmNumberVerify").validate({

        rules: {
            txtPhoneNumber: {
                required: true,
                phonesZim: true
            }
        },
        messages: {

            txtPhoneNumber: {
                required: "Enter a Phone Number",
                phoneZim: "Enter valid Phone Number"
            }
       },
        errorPlacement: function (error, element) {
            error.appendTo(element.parent());
        }
    });



</script>
				</div>
            </div>
        </div>
		
</div>
  <script type="text/javascript" >
      disableall();
      addLoadEvent(function () {
          
          loadstatus();
          fillDetails();

      }
);
      function verifyemail() {
          if (addfbvalidator.form()) {
              console.log('Start Sending Email');
              sendverification();
              console.log('Sent Email');

          };
          return true;

      }
      function contactnumb() {
          if (addnumbervalidator.form()) {
              completepromo();
          };
          return false;
      }
      function fillDetails() {
          var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSelectAccount";
          var parameters = "{'AccountID':" + siteCookie("AccountID") + "}";

          $.ajax({
              type: "POST",
              url: webMethod,
              data: parameters,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (msg) {
                  var data = JSON && JSON.parse(msg.d)[0] || $.parseJSON(msg.d)[0];

                  $("#txtSubName").attr("value", data.Account.Email);

                  $("#txtPhoneNumber").attr("value", data.Address.ContactNumber);
              },
              error: function (emsg) {
                  console.log(emsg);
              }
          });
      }

      function disableall() {
          $("#terms").prop('disabled', true);
          $("#txtSubName").prop('disabled', true);
          $("#btnSubmit").prop('disabled', true);
          $("#txtPhoneNumber").prop('disabled', true);
          $("#btnSendAirtime").prop('disabled', true);
      }
      function sendverification() {
          var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSendVerificationEmail";
          var parameters = "{'AccountID':'" + siteCookie("AccountID") + "','Email':'" + $("#txtSubName").val() + "'}";
          $.ajax({ type: "POST",
              url: webMethod,
              data: parameters,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (msg) {
                  console.log(msg.d);
                  try {
                      var data = msg.d;
                      shownotification('Information', data);
                      loadstatus();
                      console.log(data);
                  }
                  catch (err) {
                      console.log(err)
                  }
              },
              error: function (emsg) {
                  console.log(emsg);
              }
          });

      };
      function completepromo() {
          var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnCompletePromo";
          var parameters = "{'AccountID':'" + siteCookie("AccountID") + "','Target':'" + $("#txtPhoneNumber").val() + "'}";
          $.ajax({ type: "POST",
              url: webMethod,
              data: parameters,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (msg) {

                  try {
                      var data = msg.d;
                      shownotification('Information', data);
                      loadstatus();
                      console.log(data);
                  }
                  catch (err) {
                      console.log(err)
                  }
              },
              error: function (emsg) {
                  console.log(emsg);
              }
          });

      };
      function loadstatus() {
          var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSelectAccount";
          var parameters = "{'AccountID':'" + siteCookie("AccountID") + "'}";
          console.log('start');
          $.ajax({ type: "POST",
              url: webMethod,
              data: parameters,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (msg) {
                  console.log('Logging');
                  try {
                      var data = JSON && JSON.parse(msg.d)[0] || $.parseJSON(msg.d)[0];
                      console.log('Status:' + data.Address.Latitude)
                      disableall();
                      if (data.Address.Latitude == 1) {

                          $("#txtfbicon").removeClass("icon-info fg-color-blue").addClass("icon-checkmark fg-color-green");
                          $("#terms").prop('disabled', false);
                          $("#txtSubName").prop('disabled', false);
                          $("#btnSubmit").prop('disabled', false);
                      };
                      if (data.Address.Latitude == 2) {
                          $("#terms").prop('disabled', true);
                          $("#txtSubName").prop('disabled', true);
                          $("#btnSubmit").prop('disabled', true);
                          $("#txtfbicon").removeClass("icon-info fg-color-blue").addClass("icon-checkmark fg-color-green");
                          $("#txtvficon").removeClass("icon-info fg-color-blue").addClass("icon-checkmark fg-color-green");
                          $("#txtPhoneNumber").prop('disabled', false);
                          $("#btnSendAirtime").prop('disabled', false);
                          fillemail();
                          //console.log('i was here');
                      };
                      if (data.Address.Latitude == 3) {
                          $("#terms").prop('disabled', true);
                          $("#txtSubName").prop('disabled', true);
                          $("#btnSubmit").prop('disabled', true);
                          $("#txtPhoneNumber").prop('disabled', true);
                          $("#btnSendAirtime").prop('disabled', true);
                          $("#txtfbicon").removeClass("icon-info fg-color-blue").addClass("icon-checkmark fg-color-green");
                          $("#txtvficon").removeClass("icon-info fg-color-blue").addClass("icon-checkmark fg-color-green");

                          $("#txtcnicon").removeClass("icon-info fg-color-blue").addClass("icon-checkmark fg-color-green");
                      };



                  }
                  catch (err) {
                      console.log(err)
                  }
              },
              error: function (emsg) {
                  console.log(emsg);
              }
          });

      };

      function savestatus(status) {
          var webMethod = document.URL.substr(0, document.URL.lastIndexOf("/") + 1) + "data.asmx/fnSaveStatus";
          var parameters = "{'AccountID':'" + siteCookie("AccountID") + "','status':'" + status + "'}";
          console.log(parameters);
          $.ajax({
              type: "POST",
              url: webMethod,
              data: parameters,
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (msg) {
                  loadstatus();
                  console.log('Saved Status');
              },
              error: function (emsg) {
                  console.log(emsg);
              }
          });
      }
</script>
</asp:Content>

