<%@ Page Title="HOT Recharge - Sign In" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="index.aspx.vb" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Body File -->


    <!--==================================================-->
<!-- Start hot-video Area -->
<!--==================================================-->

<section class="banner-section">
	<div class="banner-carousel owl-carousel">
		<div class="slide-item one">
			<div class="image-layer" style="background-image: url('asset/images/slider/banner.jpg');"></div>
			<div class="container">
				<video autoplay muted loop id="myVideo">
					<source src="asset/video/hot.mp4" type="video/mp4">
				  </video>
				</div>
			</div>
		</div>
		
	</div>
</section>
<!--==================================================-->
<!-- End hot-video Area -->
<!--==================================================-->

    <!--==================================================-->
<!-- Start Service Details Area -->
<!--==================================================-->
<div id="login" style="padding-top: 50px;" class="col-lg-12 col-md-12">
	<div class="section-title wow fadeInUp" data-wow-delay="0.2s" data-wow-duration="1s">
		<div class="section-sub-title">
<h2 style="text-align: center; font-family:'Korataki';"><br>Get your airtime here! <span style="color: #ce2128"></span> </h2>
</div>
</div>
</div>


<div style="margin-bottom: 40px !important;" class="service-details-area wow fadeInUp" data-wow-delay="0.3s" data-wow-duration="1s">
	
	<div class="container">
		
		<div class="row">
			

			<div class="col-lg-4 col-md-12">
				<div class="widget-categories-box">
					<!-- widget categories menu -->
					<div class="widget-categories-menu">
						<ul>
							<li><a href="register.aspx"> Not Registered? Register now! <span><i
									class="bi bi-chevron-double-right"></i></span></a></li>
							<li><a href="selftopup.aspx">Econet Airtime <span><i
								class="bi bi-chevron-double-right"></i></span></a></li>
							<li><a href="selftopup.aspx"> Netone Airtime <span><i
											class="bi bi-chevron-double-right"></i></span></a></li>
							
							<li><a href="selftopup.aspx"> Telecel Airtime <span><i
											class="bi bi-chevron-double-right"></i></span></a></li>
							
							<li><a href="faq.aspx"> FAQs <span><i
										class="bi bi-chevron-double-right"></i></span></a></li>

							<li><a href="downloads.aspx"> Downloads <span><i
									class="bi bi-chevron-double-right"></i></span></a></li>
							
						</ul>
					</div>
				</div>
				
				
			</div>
			<div class="col-lg-4 col-md-12">
				
					<img src="asset/images/resource/hot_recharge_register.jpg" alt="Hot Recharge Register">
				</div>
				
				<div style="background-color: #ce2128;" class="col-lg-4 col-md-12">
					
					
						<div id="hmLogin" >
    <div >
        <form id="LoginForm" runat="server" style="padding: 30px; padding-bottom: 20px;">
            <h2 class=" fg-color-white"><strong>Account Login</strong></h2>
            <h6 style="margin-bottom: 0px;color:white;" class="">Email Address/Mobile Number</h6>
            <div class="form-box input-control text">
                <input id="txtUID" type="text" placeholder="Email Address" runat="server" />
                <button class="btn-clear"></button>
            </div>
            <h6 style="margin-bottom: 0px;color:white;" class="">Password/Pin</h6>
            <div class="form-box input-control password">
                <input id="txtPWD" type="password" placeholder="Password" runat="server" />
                <button class="btn-reveal"></button>
            </div>
            <div class="clearfix"></div>
            <button id="cmdSignIn" style="background:#ce2127" class="button big fg-color-white margin-bottom-none"  runat="server"><i class="icon-arrow-right-3"></i>sign in</button>
            <h6 class="fg-color-white margin-bottom-none" runat="server" id="txtError" style="margin-left: 10px; height: 30px;"></h6>
            <a href="register.aspx" style="background:#ce2127" class="button mini fg-color-white margin-bottom-none"><i class="icon-pencil"></i>not registered? sign up here</a>
            <a href="resetemail.aspx" style="background:#ce2127" class="button mini  fg-color-white margin-bottom-none"><i class="icon-help-2"></i>i forgot my password.</a>


        </form>
    </div>
</div>	
							
							
					</div>	
				
			
			
			</div>
		</div>
	</div>
</div>
<!--==================================================-->
<!-- End Service Details Area -->
<!--==================================================-->

    <!--==================================================-->
<!-- Start Choose Us Area -->
<!--==================================================-->
<div style="background-color: #f1f1f1 " class="choose-us-area">
	<div class="container">
		<div class="row align-items-center">
			<div class="col-lg-6 col-md-12">
				<div class="section-title wow fadeInUp" data-wow-delay="0.2s" data-wow-duration="1s">
					<div class="section-sub-title">
						<h4>Why Choose Us</h4>
					</div>
					<div class="section-main-title">
						<h2>HOT gets it done!</h2>
					</div>
					<div class="choose-discription">
						<p>HOT recharge has a solution for your business, from airtime anytime, to zesa and other services</p>
					</div>
				</div>
				<div class="row choose">
					<div class="col-lg-6 col-md-12">
						<div class="choose-list wow fadeInUp" data-wow-delay="0.4s" data-wow-duration="1s">
							<span><i class="bi bi-chevron-double-right"></i> 18 Years Experience</span>
							<span><i class="bi bi-chevron-double-right"></i> Service Quality and commitment</span>
							<span><i class="bi bi-chevron-double-right"></i> One payment across networks</span>
						</div>
					</div>
					<div class="col-lg-6 col-md-12">
						<div class="choose-list wow fadeInUp" data-wow-delay="0.6s" data-wow-duration="1s">
							<span><i class="bi bi-chevron-double-right"></i> 24/7 Service Availability</span>
							<span><i class="bi bi-chevron-double-right"></i> 100% VAT tax Compliance</span>
							<span><i class="bi bi-chevron-double-right"></i> Realtime Reports</span>
						</div>
					</div>
				</div>
		
				<div style="padding-left: 5px;" class="Hot-btn choose wow fadeInUp" data-wow-delay="0.8s" data-wow-duration="1s">
					<a href="asset/pdf/brochure.pdf">Download Brochure</a>
				</div>
			</div>
			<div class="col-lg-6 col-md-12">
				<div class="choose-thumb-items">
					<div class="choose-thumb wow fadeInLeft" data-wow-delay="0.4s" data-wow-duration="1s">
						<img src="asset/images/resource/choose_hot3.jpg" alt="Hot Recharge Deals">
					</div>
					<div class="choose-thumb wow fadeInUp" data-wow-delay="0.4s" data-wow-duration="1s">
						<img src="asset/images/resource/choose_hot2.jpg" alt="Hot Recharge Savings">
					</div>
					<div class="choose-thumb three">
						<img src="asset/images/resource/choose_hot1.jpg" alt="Hot Recharge Airtime">
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
<!--==================================================-->
<!-- End Choose Us Area -->
<!--==================================================-->

   

                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        addLoadEvent(function () {
            checkStatusOnline();
            setInterval(function () { checkStatusOnline(); }, 30000);

            checkStatusOnlineDetails();
            setInterval(function () { checkStatusOnlineDetails(); }, 30000);
        }
        );

    </script>
    <!-- Body File end -->

		<!-- jquery meanmenu js -->
<script src="asset/js/jquery.meanmenu.js"></script>


<script src="asset/js/jquery.scrollUp.js"></script>
</asp:Content>

