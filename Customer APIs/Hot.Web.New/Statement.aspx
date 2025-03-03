<%@ Page Language="VB"  MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="Statement.aspx.vb" Inherits="Statement" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script src="js/assets/list.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!-- Body File -->
<div class="page secondary" id="page-subs">
	 <div id="transactions-report" class="width-full">
                    <h3>Statement Report</h3>
				    <asp:placeholder ID="stmt" runat="server" />
                </div> 
  	
</div>
<script type="text/javascript" >

    addLoadEvent(function () {
         
    });
    /* Sidebar menu */

    var transList;

    function listtable() {
        transList = new List('tranList', options);
    }

   

</script>

<style>
#trans .highlighted { background: rgba(255, 255, 0, 0.1); }
#txtSubs {display:none;}
#trans tr>* { text-align: center; }
#trans tr>*:nth-child(9), #trans tr>*:nth-child(10), #trans tr>*:nth-child(11) { display:none; }
#trans>tfoot td { text-align: right;}
#trans>tfoot tr { height: 30px; }

#trans>tbody>tr.berror{background: rgba(255, 0, -1, .24);}
#trans>tbody>tr.payment{background: rgba(160, 246, 80, .3);}
#trans>tbody>tr>td:nth-child(6) { text-align: right; }
#trans>tbody>tr>td:nth-child(1) { text-align: left; }
#trans>tbody>tr>td:nth-child(3) { text-align: left; }

#page-toolbar { border-top: 1px solid #eee; margin-top: -20px; padding-top: 10px;}
#toolbar-bottom { border-top: 1px solid #eee; margin-bottom: 10px; margin-top: -5px;}

</style>

    	<!-- jquery meanmenu js -->
<script src="js/assets/asset/js/jquery.meanmenu.js"></script>


<script src="js/assets/asset/js/jquery.scrollUp.js"></script>
</asp:Content>


