<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="background.aspx.vb" Inherits="background" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!-- Body File -->
<div class="page secondary ">
        <div class="page-header">
            <div class="page-header-content">
                <h1>Change<small>Background</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
        </div>
        
        <div class="page-region">
            <div class="page-region-content">
            	<div class="span10"> 
					<h2><strong>Images</strong></h2>
					<div class="image-collection">
							<div>
								<img src="images/backgrounds/vicfalls.jpg" />
								<div class="overlay">
									<span>The Victoria Falls</span>
									<div class ="clearfix"></div>
									<a href="javascript:setBackground('url('images/backgrounds/vicfalls.jpg')');" class="place-left">
										<i class="icon-glasses"></i>Preview
									</a>
									<a href="javascript:saveBackground(&#34;url('images/backgrounds/vicfalls.jpg')&#34;,'The Victoria Falls');" class="place-right">
										<i class="icon-download" ></i>Change
									</a>
								</div>
							</div>
							<div>
								<img src="images/backgrounds/greatzim.jpg" />
								<div class="overlay">
									<span>Black and white image of Great Zimababwe</span>
									<div class ="clearfix"></div>
									<a href="javascript:setBackground('url(\'images/backgrounds/greatzim.jpg\')');" class="place-left">
										<i class="icon-glasses"></i>Preview
									</a>
									<a href="javascript:saveBackground(&#34;url('images/backgrounds/greatzim.jpg')&#34;,'The Great Zimababwe');"class="place-right">
										<i class="icon-download" ></i>Change
									</a>
								</div>
							</div>							
							<div>
								<img src="images/backgrounds/Baboon Lagoon.jpg" />
								<div class="overlay">
									<span>Baboons on rocks near clear blue water</span>
									<div class ="clearfix"></div>
									<a href="javascript:setBackground('url(\'images/backgrounds/Baboon Lagoon.jpg\')');" class="place-left">
										<i class="icon-glasses"></i>Preview
									</a>
									<a href="javascript:saveBackground(&#34;url('images/backgrounds/Baboon Lagoon.jpg')&#34;,'Baboon Lagoon');"class="place-right">
										<i class="icon-download" ></i>Change
									</a>
								</div>
							</div>
					</div> 
					<h2><strong>Colors</strong></h2>
					<div class="image-collection">
							<div>
								<img class="bg-color-white" />
								<div class="overlay">
									<span>Original Site Background</span>
									<div class ="clearfix"></div>
									<a href="javascript:setBackground('white');" class="place-left">
										<i class="icon-glasses"></i>Preview
									</a>
									<a href="javascript:saveBackground('white','The Color White');"class="place-right">
										<i class="icon-download" ></i>Change
									</a>
								</div>
							</div>					
							<div>
								<img class="bg-color-teal" />
								<div class="overlay">
									<span>Teal Background</span>
									<div class ="clearfix"></div>
									<a href="javascript:setBackground('#00ABA9');" class="place-left">
										<i class="icon-glasses"></i>Preview
									</a>
									<a href="javascript:saveBackground('#00ABA9','The Color Teal');"class="place-right">
										<i class="icon-download" ></i>Change
									</a>
								</div>
							</div>
							<div>
								<img class="bg-color-orange" />
								<div class="overlay">
									<span>Orange Background</span>
									<div class ="clearfix"></div>
									<a href="javascript:setBackground('#E3A21A');" class="place-left">
										<i class="icon-glasses"></i>Preview
									</a>
									<a href="javascript:saveBackground('#E3A21A','The Color Orange');"class="place-right">
										<i class="icon-download" ></i>Change
									</a>
								</div>
							</div>									
							<div>
								<img class="bg-color-pink" />
								<div class="overlay">
									<span>Pink Background</span>
									<div class ="clearfix"></div>
									<a href="javascript:setBackground('#9F00A7');" class="place-left">
										<i class="icon-glasses"></i>Preview
									</a>
									<a href="javascript:saveBackground('#9F00A7','The Color Pink');"class="place-right">
										<i class="icon-download" ></i>Change
									</a>
								</div>
							</div>	
							
							<div>
								<img class="bg-color-purple" />
								<div class="overlay">
									<span>Purple Background</span>
									<div class ="clearfix"></div>
									<a href="javascript:setBackground('#603CBA');" class="place-left">
										<i class="icon-glasses"></i>Preview
									</a>
									<a href="javascript:saveBackground('#603CBA','The Color Purple');"class="place-right">
										<i class="icon-download" ></i>Change
									</a>
								</div>
							</div>		
					</div>       
                </div>
				
            </div>
        </div>
		
</div>
  
<!-- Body File   -->
</asp:Content>

