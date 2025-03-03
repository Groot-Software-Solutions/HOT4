<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="subscribers.aspx.vb" Inherits="subscribers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<!-- Body File -->
<div class="page secondary with-sidebar" id="page-subs">
	<div class="page-header">
            <div class="page-header-content">
                <h1> Subscriber<small>Management</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
    </div>
    <div class="page-sidebar" data-offset-top="200">
    	<div class="padding10">
			<div>
				<div class="input-control text ">             
					<input id="txtfilter" type="text" placeholder="Filter Subscribers" onKeyup="javascript:filterTable(this,'subscriber1', $('#startswith').prop('checked'));" onKeydown="javascript:filterTable(this,'subscriber1', $('#startswith').prop('checked'));" />
					<button class="btn-clear"></button>
				</div>
				
				<div class="place-left"><label class="input-control checkbox"><input type="checkbox" id="startswith" onclick="javascript:filterTable($('#txtfilter')[0],'subscriber1', $('#startswith').prop('checked'));"><span class="helper"><small>Show rows that start with filter</small></span></label></div>
			</div> 
			
			<div class="place-right"><h4><span id="txtSelected">0</span> Selected Subscriber(s)</h4></div>
        	                
	        <button class="width-full bg-color-gray" id="btnRecharge"><i class="icon-power"></i>Airtime</button>
            <div class="clearfix"></div>
       		<div class="bg-color-gray option margin-top-none" style="display:none;" id="rechargebar">	
                <div class=" padding10"><span style="font-size: 12px;">Recharge Amount: </span><input id="txtRechargeAmt" type="text" style="width:60px;"  onkeydown="javascript: this.value =number_amount(this.value);" onkeyup="javascript: this.value =number_amount(this.value);"  /></div>
                <div class="clearfix"></div>
                <button id="btnRechargesubs" class="bg-color-gray" style="width:100%;margin-bottom:0px;"><i class="icon-checkmark"></i>Confirm</button>
            </div>

            <button class="width-full bg-color-gray" id="btnRechargeUsd"><i class="icon-power"></i>USD Airtime</button>
            <div class="clearfix"></div>
       		<div class="bg-color-gray option margin-top-none" style="display:none;" id="rechargebarusd">	
                <div class=" padding10"><span style="font-size: 12px;">Recharge Amount: </span><input id="txtRechargeAmtUsd" type="text" style="width:60px;"  onkeydown="javascript: this.value =number_amount(this.value);" onkeyup="javascript: this.value =number_amount(this.value);"  /></div>
                <div class="clearfix"></div>
                <button id="btnRechargesubsusd" class="bg-color-gray" style="width:100%;margin-bottom:0px;"><i class="icon-checkmark"></i>Confirm</button>
            </div>
            <button class="width-full bg-color-gray" id="btnRechargeNetDataUSD" ><i class="icon-earth"></i>NetOne $USD Airtime</button>
            <div class="clearfix"></div>
       	    <div class="bg-color-gray option margin-top-none" style="display:none;" id="rechargebarnetdatausd">	
                    <div class="input-control select" style="padding: 0px 5px; margin-bottom: 0px;" > 
		                <select id="txtBundleUSDNetone"></select>
	                </div>
                   <div class="clearfix"></div>
                <button id="btnRechargesubsdataUSDNetone" class="bg-color-gray" style="width:100%;margin-bottom:0px;"><i class="icon-checkmark"></i>Confirm</button>
            </div>
            <button class="width-full bg-color-gray" id="btnRechargeData" ><i class="icon-earth"></i>Econet Data</button>
            <div class="clearfix"></div>
       	    <div class="bg-color-gray option margin-top-none" style="display:none;" id="rechargebardata">	
                    <div class="input-control select" style="padding: 0px 5px; margin-bottom: 0px;" > 
		                <select id="txtBundle"></select>
	                </div>
                   <div class="clearfix"></div>
                <button id="btnRechargesubsdata" class="bg-color-gray" style="width:100%;margin-bottom:0px;"><i class="icon-checkmark"></i>Confirm</button>
            </div>
            <button class="width-full bg-color-gray" id="btnRechargeDataUSD" ><i class="icon-earth"></i>Econet $USD Data</button>
            <div class="clearfix"></div>
       	    <div class="bg-color-gray option margin-top-none" style="display:none;" id="rechargebardataUSD">	
                    <div class="input-control select" style="padding: 0px 5px; margin-bottom: 0px;" > 
		                <select id="txtBundleUSDEconet"></select>
	                </div>
                   <div class="clearfix"></div>
                <button id="btnRechargesubsdataUSDEconet" class="bg-color-gray" style="width:100%;margin-bottom:0px;"><i class="icon-checkmark"></i>Confirm</button>
            </div>

             <button class="width-full bg-color-gray hidden" id="btnRechargeNetData" ><i class="icon-earth"></i>NetOne Data</button>
            <div class="clearfix"></div>
       	    <div class="bg-color-gray option margin-top-none" style="display:none;" id="rechargebarnetdata">	
                    <div class="input-control select" style="padding: 0px 5px; margin-bottom: 0px;" > 
		                <select id="txtpins"></select>
	                </div>
                   <div class="clearfix"></div>
                <button id="btnRechargesubsnetdata" class="bg-color-gray" style="width:100%;margin-bottom:0px;"><i class="icon-checkmark"></i>Confirm</button>
            </div>
              
            

            <button class="width-full bg-color-gray allow-usd hidden" id="btnVoucher" ><b>$</b>USD Vouchers</button>
            <div class="clearfix"></div>
       	    <div class="bg-color-greenLight option margin-top-none" style="display:none;" id="voucherbar">	
                    <div class="input-control select" style="padding: 0px 5px; margin-bottom: 0px;" > 
		                <select id="txtUSDEVD"></select>
	                </div>
                   <div class="clearfix"></div>
                <button id="btnRechargeUsdVoucher" class="bg-color-gray" style="width:100%;margin-bottom:0px;"><i class="icon-checkmark"></i>Confirm</button>
            </div>

			<button class="zesa width-full bg-color-gray" id="btnZesa" style="display:none;"><i class=" icon-power-2"></i>Zesa Token</button>        
            <button class="width-full bg-color-gray" id="btnTelone" ><i class=" icon-phone"></i>Telone Products</button>        
           <button class="width-full bg-color-gray" id="btnNyaradzo" ><i class=" icon-book"></i>Nyaradzo Payment</button>   
            <div class="clearfix"></div>
       		
             <button class="width-full bg-color-gray" id="btnRechargeBulk"><i class="icon-clipboard-2"></i>Bulk Recharge</button>
             
            <div class="clearfix"></div>
       	
			<button class="width-full bg-color-gray" id="btnViewTrans"><i class="icon-stats"></i>View Transactions</button>        
            <div class="clearfix"></div>
            <div class="bg-color-gray option margin-top-none" style="display:none;" id="viewtransbar">	
                <div>
                    <div class=" padding10">   	Start Date: 
                        <div class="input-control text datepicker" data-role="datepicker" style="margin-bottom: -20px;">
                            <input type="text"  id="txtStartDate" />
                            <button class="btn-date"></button>
                        </div>
                    </div>
                    <div class=" padding10">     End Date: 
                        <div class="input-control text datepicker" data-role="datepicker">
                            <input type="text" id="txtEndDate" />
                            <button class="btn-date"></button>
                        </div>
                    </div>
                </div>
                <div class="clearfix"></div>
                <button id="btnViewTransactions" class="bg-color-gray" style="width:100%;margin-bottom:0px;"><i class="icon-checkmark"></i>Confirm</button>
            </div>
            
            <button class="width-full bg-color-gray" id="btnViewPins"><i class="icon-barcode"></i>View Sold Pins</button>
            <div class="clearfix"></div>

			<button class="width-full bg-color-gray" id="btnDelete"><i class="icon-remove"></i>Delete Selected</button>
            <div class="clearfix"></div>
            <div class="bg-color-gray option margin-top-none" style="display:none;" id="deletebar">				
                <div class=" padding10">Are you sure you want to remove the selected subscribers</div>
                <div class="clearfix"></div>
                <button class="bbg-color-gray margin-bottom-none" id="btnDeleteSubscribers"style="margin-right:0px;float:left;"><i class="icon-checkmark"></i>Confirm</button>
                <button class="bg-color-gray margin-bottom-none" id="btnCancelDelete" ><i class="icon-cancel-2"></i>Cancel</button>                
            </div>
			<button id="btnAddsub" class="width-full bg-color-gray" id="btnRecharge"><i class="icon-broadcast"></i>Add Subscriber</button>
			
          
            <button class="width-full bg-color-orange" id="btnViewSubs" style="display:none"><i class="icon-user-2"></i>View Subscribers</button>        
            <div class="clearfix"></div>
           
            <div class="bg-color-grayDark padding10" >
                <i class="icon-help place-left icon-3x" id="txtStatusIcon" ></i>
                <p class="place-right">
                    <h5 class="fg-color-white">System Status</h5>	                            
					<h5 class="fg-color-orange" id="txtStatusHead">Current Unavailable</h5>
					<p  id="txtStatusMessage" style="display:none">
									
					</p>
                </p> 
			</div>
           
            
        </div>

    </div>
  	<div class="page-region">
		<div class="page-region-content">
			<div>
				<table id="subscriber1" class="hovered">
				<thead>
					  <tr>
						<td width="25px"><label class="input-control checkbox" onclick='javascript:selectall();' style="margin-left: 5px;" ><input type="checkbox" id="maintick"><span class="helper"></span></label></td>
						<td>Name</td>
						<td>Mobile Number</td>
						<td>Brand</td>
						<td>Status</td>
						<td width="25px"></td>
						<td width="25px"><a href='javascript:loadsubscribers();'><i class='icon-loop'></i></a></td>
					  </tr>
				</thead>
				<tbody>
					  
				</tbody>
				</table>
                <table id="pins" class="hovered" style="display:none;">
				<thead>
					  <tr>
						<td width="20%" >Date</td>
                        <td width="10%" >Brand</td>
                        <td width="15%" >Mobile</td>
                        <td width="8%" >Value </td>
                        <td  >Pin</td>
                        <td  >Ref</td>
                      
					  </tr>
				</thead>
				<tbody>
					  
				</tbody>
				</table>
			</div>
			
		</div>
    </div>
</div>
<style>
#subscriber1 .highlighted { background: yellow; }
</style>
<script type="text/javascript" >
    addLoadEvent(function () {
        loadsubscribers();
        loadbundles(); 
        loadbundlesUSD();
        loadusdbundles();
        loadpinbundles();
        checkStatusOnline();
        setInterval(function () { checkStatusOnline(); }, 30000);
        Get_Zesa_Balance(siteCookie("UID"), siteCookie("Pwd"));
    });
   
    /* Sidebar menu */
    $("#btnViewTrans").click(function () {
        $(".option").hide(50);
        // $("#viewtransbar").show(150);
        openpage('Transactions.aspx');
    })
    $("#btnZesa").click(function () { 
        openpage('Zesa.aspx');
    })
    $("#btnTelone").click(function () {
        openpage('Telone.aspx');
    })
    $("#btnNyaradzo").click(function () {
        openpage('Nyaradzo.aspx');
    })
    

    $("#btnViewPins").click(function () {
        $(".option").hide(50);

        $("#btnViewPins").hide(150);
        $("#btnViewSubs").show(50);

        $("#subscriber1").hide(50);
        $("#pins").show(150);

        loadsoldpins();
    });
    $("#btnViewSubs").click(function () {
        $(".option").hide(50);

        $("#btnViewSubs").hide(50);
        $("#btnViewPins").show(150);

        $("#pins").hide(150);
        $("#subscriber1").show(50);
    });
    
    $("#btnRecharge").click(function () {
        $(".option").hide(50);
        $("#rechargebar").show(150);
    });
    $("#btnRechargeUsd").click(function () {
        $(".option").hide(50);
        $("#rechargebarusd").show(150);
    });
    $("#btnRechargeData").click(function () {
        $(".option").hide(50);
        $("#rechargebardata").show(150);
    });
    $("#btnRechargeDataUSD").click(function () {
        $(".option").hide(50);
        $("#rechargebardataUSD").show(150);
    });
    $("#btnRechargeNetDataUSD").click(function () {
        $(".option").hide(50);
        $("#rechargebarnetdatausd").show(150);
    });
    $("#btnVoucher").click(function () {
        $(".option").hide(50);
        $("#voucherbar").show(150);
    });
    $("#btnDelete").click(function () {
        $(".option").hide(50);
        $("#deletebar").show(150);
    });
    $("#btnDeleteSubscribers").click(function () {
        $(".option").hide(50);
        $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
            deleteSubscriber($('td', this)[1].innerHTML, $('input', $('td', this)).attr("id"));
        });
        loadsubscribers();
        updatecount();
    });
    $("#btnCancelDelete").click(function () {
        $(".option").hide(50);
    });
    $('#btnRechargesubs').click(function (e) {
        rechargewindow();
    });
    $('#btnRechargesubsusd').click(function (e) {
        rechargewindowusd();
    });
    $('#btnRechargesubsdata').click(function (e) {
        rechargedatawindow();
    });
    $('#btnRechargesubsdataUSDEconet').click(function (e) {
        rechargedataUSDwindowEconet();
    });
    $('#btnRechargesubsdataUSDNetone').click(function (e) {
        rechargedataUSDwindowNetone();
    });

    $('#btnViewTransactions').click(function (e) {
        var filterlist = '';
        var comma = '';
        $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
            filterlist = filterlist + comma + $('td', this)[2].innerHTML;
            comma = ',';
        });	
        viewreport($("#txtStartDate").val(), $("#txtEndDate").val(),filterlist);
    });
    $('#btnAddsub').click(function (e) {
        addnewsubscriber();
    });    

    $('#btnRechargeUsdVoucher').click(function (e) {
        rechargeUSDEvdwindow();
    });

    $("#btnRechargeNetData").click(function () {
        $(".option").hide(50);
        $("#rechargebarnetdata").show(150);
    });
    
    $('#btnRechargesubsnetdata').click(function (e) {
        rechargeEvdwindow();
    });
    
  
    function rechargewindow() {
        $.get('./recharge.html', function (rechargecontent) {

            $.Dialog({
                'title': 'Recharges',
                'content': rechargecontent,
                'draggable': true,
                'closeButton': true,
                'buttons': {}
            });
            addrecharges();
            $()["Input"]({ initAll: true });
        });
    }

    function rechargewindowusd() {
        $.get('./recharge.html', function (rechargecontent) {

            $.Dialog({
                'title': 'Recharges',
                'content': rechargecontent,
                'draggable': true,
                'closeButton': true,
                'buttons': {}
            });
            addrecharges_usd();
            $()["Input"]({ initAll: true });
        });
    }

    function rechargedatawindow() {
        $.get('./recharge.html', function (rechargecontent) {

            $.Dialog({
                'title': 'Data Recharges',
                'content': rechargecontent,
                'draggable': true,
                'closeButton': true,
                'buttons': {}
            });
            addrecharges_data();
            $($("#rechargelog thead th")[2]).html("Product Code");
            $()["Input"]({ initAll: true });
        });
    }

    function rechargedataUSDwindowEconet() {
        var productCode = $("#txtBundleUSDEconet option:selected").val();
        $.get('./recharge.html', function (rechargecontent) {

            $.Dialog({
                'title': 'Data Recharges',
                'content': rechargecontent,
                'draggable': true,
                'closeButton': true,
                'buttons': {}
            });
            addrecharges_dataUSD_WithCode(productCode);
            $($("#rechargelog thead th")[2]).html("Product Code");
            $()["Input"]({ initAll: true });
        });
    }

    function rechargedataUSDwindowNetone() {
        var productCode = $("#txtBundleUSDNetone option:selected").val();
        $.get('./recharge.html', function (rechargecontent) {

            $.Dialog({
                'title': 'Data Recharges',
                'content': rechargecontent,
                'draggable': true,
                'closeButton': true,
                'buttons': {}
            });
            addrecharges_dataUSD_WithCode(productCode);
            $($("#rechargelog thead th")[2]).html("Product Code");
            $()["Input"]({ initAll: true });
        });
    }

    function rechargeUSDEvdwindow() {
        $.get('./recharge.html', function (rechargecontent) {

            $.Dialog({
                'title': 'Recharges',
                'content': rechargecontent,
                'draggable': true,
                'closeButton': true,
                'buttons': {}
            });
            addrecharges_usdevd();
            $()["Input"]({ initAll: true });
        });
    }

    function rechargeEvdwindow() {
        $.get('./recharge.html', function (rechargecontent) {

            $.Dialog({
                'title': 'Recharges',
                'content': rechargecontent,
                'draggable': true,
                'closeButton': true,
                'buttons': {}
            });
            addrecharges_evd();
            $()["Input"]({ initAll: true });
        });
    }
    
    
</script>
<!-- Body File end -->
</asp:Content>

