<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Transactions.aspx.vb" Inherits="Transactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/assets/list.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!-- Body File -->
<div class="page secondary" id="page-subs">
	<div class="page-header">
            <div class="page-header-content">
                <h1>View<small> Transactions</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
    </div>
    
  	<div class="page-region">
		<div class="page-region-content" id="tranList">
            <div id="page-toolbar">
			      
                 <div class=" place-left span2">Start Date:
                <div class="input-control text datepicker " data-role="datepicker" style="margin-top:10px;">
                    <input type="text"  id="txtStartDate" />
                    <button class="btn-date"></button>
                </div>
            </div>
                 <div class=" place-left span2">End Date:
                <div class="input-control text datepicker " data-role="datepicker" style="margin-top:10px;">
                    <input type="text" id="txtEndDate" />
                    <button class="btn-date"></button>
                </div>
            </div> 
                 <div class="place-left span2">
                    <button id="btnViewTransactions" class="width-full bg-color-redog" style="margin-bottom: 5px; color: white"><i class="icon-checkmark"></i>Get Report</button>
                    <button id="btnExport" class="width-full bg-color-redog" style="margin-bottom: 5px; color: white"><i class="icon-cloud-2"></i>Export Report</button>
                 </div>
                 <div class="place-left span2">
				    <div class="input-control text ">             
					    <input id="txtfilter" type="text" placeholder="Filter Transactions" class="search"/>
					    <button class="btn-clear"></button>
				    </div>
				    <div class="input-control select" id="txtSubs">
                        <select name="txtTellers" id="txtSubscribers" onchange="javascript:filter_trans_table();">
                            <option value="" selected>All Subscribers</option>
                            <option value="">Laoding ...</option>
                           
                        </select>
                     </div>
				   
			    </div>
                 <div class="place-left span2">
                     <div class="input-control select">
                        <select name="txtTellers" id="txtTellers" onchange="javascript:filter_trans_table();">
                            <option value="" selected>All Tellers</option>
                            <option value="">Laoding ...</option>
                           
                        </select>
                     </div>
                     <div class="input-control select">
                        <select name="txtBrands" id="txtBrands" onchange="javascript:filter_trans_table();">
                            <option value="" selected>All Networks</option>
                            <option value="Econet">Econet</option>
                            <option value="NetOne">NetOne</option>
                            <option value="Telecel">Telecel</option>
                            <option value="Africom">Africom</option>
                        </select>
                    </div>
                 </div>
                <div id="toolbar-bottom" class="width-full place-left"></div>
                <div class="clearfix"></div>
            </div>
			<div>
                <div id="csv-key" style="display:none;">
                    <div class="place-left">
                        <h3>Brand ID Codes</h3>
                        <table id="brand-key" class="hovered bordered">
				            <thead>
					              <tr>
						            <td>Brand ID</td>
                                    <td>Brand</td>                      
					              </tr>
				            </thead>
				            <tbody>

                            </tbody>
				        </table>
                        <h3>Status Codes</h3>
                        <table id="status-key" class="hovered  bordered">
				            <thead>
					              <tr>
						            <td>Status ID</td>
                                    <td>Status</td>                      
					              </tr>
				            </thead>
				            <tbody>

                            </tbody>
				        </table>
                    </div>
                  
                    <div class="place-left">
                        <h3>Teller ID Codes</h3>
                        <table id="teller-key" class="hovered bordered">
				            <thead>
					              <tr>
						            <td>Teller ID</td>
                                    <td>Teller Name</td> 
                                    <td>Login Address</td>   
                                    <td>Login Type</td>                  
					              </tr>
				            </thead>
				            <tbody>

                            </tbody>
				        </table>
                    </div>
                      <div class="clearfix"></div>
                </div>
                <div id="transactions-report" class="width-full">
                    <h3>Transactions Report</h3>
				    <table id="trans" class="hovered bordered tablesorter">
				        <thead>
					          <tr>
                                <th class="sort" data-sort="rd">Date</th>
						        <th class="sort" data-sort="rid">Recharge ID</th>
						        <th class="sort" data-sort="rtl">Teller</th>
						        <th class="sort" data-sort="rmo">Mobile</th>
						        <th class="sort" data-sort="rbn">Brand</th>
						        <th class="sort" data-sort="ramt">Amount</th>
                                <th>Discount</th>
						        <th class="sort" data-sort="rst">Status</th>
                                <th>StatusID</th>
                                <th class="sort" data-sort="rtid">TellerID</th>
                                <th>Teller Login</th>
					          </tr>
				        </thead>
				        <tbody class="list">
					  
				        </tbody>
                        <tfoot>
                            <tr>
						        <td></td>
						        <td colspan="3"># of Transations: <span id="txtNumberTrans">0</span></td>
						        <td colspan="2" style="padding-right:10px;">Total: <span id="txtTotalAmount">0.00</span></td>
                                <td></td>
						        <td></td>
					         </tr>
                        </tfoot>
				    </table>
                </div>   
                
					  
				
			</div>
			
		</div>
    </div>
</div>
<script type="text/javascript" >

    addLoadEvent(function () {

        loadstatusids();
        loadbrandids();
        loaduserskey();
        if (!(siteCookie("Retail"))) {
            loadsubscribers_transactions($("#txtSubscribers"));
            $("#txtSubs").show();
        }
        checkStatusOnline();
        setInterval(function () { checkStatusOnline(); }, 30000);

    });
    /* Sidebar menu */

    $('#btnViewTransactions').click(function (e) {
         getTransactionData($("#txtStartDate").val(), $("#txtEndDate").val());
         
    });

    $('#btnExport').click(function (e) {
        download('tranreport.csv', TableToCSV($('#trans')));
        $('#csv-key').show(500);
    });

    var transList;
    
    if (!String.prototype.startsWith) {
        String.prototype.startsWith = function (searchString, position) {
            position = position || 0;
            return this.indexOf(searchString, position) === position;
        };
    }

    function listtable() {
        transList = new List('tranList', options);
    }

    function filter_trans_table() {

        transList.filter(function (item) {
            var result = true;
            var fbrand = document.getElementById("txtBrands").options[document.getElementById("txtBrands").selectedIndex].value;
            var fteller = document.getElementById("txtTellers").options[document.getElementById("txtTellers").selectedIndex].value;
            var fsubs = document.getElementById("txtSubscribers").options[document.getElementById("txtSubscribers").selectedIndex].value;
            if (!(item.values().rbn.startsWith(fbrand) || fbrand == "")) result = false;
            if (!(item.values().rtid == (fteller) || fteller == "")) result = false;
            if (!(item.values().rmo == (fsubs) || fsubs == ""|| fsubs === undefined )) result = false;
            return result;
        });
        TranSummary();
    }

</script>

<style>
#trans .highlighted { background: rgba(255, 255, 0, 0.1); }
#txtSubs {display:none;}
#trans tr>* { text-align: center; }
#trans tr>*:nth-child(9), #trans tr>*:nth-child(10), #trans tr>*:nth-child(11) { display:none; }
#trans>tfoot td { text-align: right;}
#trans>tfoot tr { height: 30px; }
 
#trans>tbody>tr>td:nth-child(6) { text-align: right; }
#trans>tbody>tr>td:nth-child(1) { text-align: left; }
#trans>tbody>tr>td:nth-child(3) { text-align: left; }

#page-toolbar { border-top: 1px solid #eee; margin-top: -20px; padding-top: 10px;}
#toolbar-bottom { border-top: 1px solid #eee; margin-bottom: 10px; margin-top: -5px;}

</style>
</asp:Content>

