<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="users.aspx.vb" Inherits="users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!-- Body File -->
<div class="page secondary with-sidebar" id="page-subs">
	<div class="page-header">
            <div class="page-header-content">
                <h1>Teller<small>Management</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
    </div>
    <div class="page-sidebar" data-offset-top="200">
    	<div class="padding10">
			<div>
				<div class="input-control text ">             
					<input id="txtfilter" type="text" placeholder="Filter Users" onKeyup="javascript:filterTable(this,'users1', $('#startswith').prop('checked'));console.log($('#startswith').prop('checked'));" onKeydown="javascript:filterTable(this,'users1', $('#startswith').prop('checked'));" />
					<button class="btn-clear"></button>
				</div>
				
				<div class="place-left"><label class="input-control checkbox"><input type="checkbox" id="startswith" onclick="javascript:filterTable($('#txtfilter')[0],'users1', $('#startswith').prop('checked'));"><span class="helper"><small>Show rows that start with filter</small></span></label></div>
			</div> 
			
			<div style="text-align: right;"><h4><span id="txtSelected">0</span> Selected Teller(s)</h4></div>
        	                
	        <button class="width-full bg-color-gray" id="btnAddUser" ><i class="icon-plus-2"></i>Add New Teller</button>
           
			<button class="width-full bg-color-gray" id="btnViewDeleted"><i class="icon-user"></i>View Removed Tellers</button>        
            
            <button class="width-full bg-color-gray" id="btnViewActive" style="display:none;"><i class="icon-user"></i>View Active Tellers</button>        
            
			<button class="width-full bg-color-gray" id="btnDelete"><i class="icon-remove"></i>DeActivate Teller(s)</button>
            <button class="width-full bg-color-gray" id="btnActivateUsers" style="display:none;"><i class="icon-history" ></i>ReActivate Teller(s)</button>
            <div class="clearfix"></div>
            <div class="bg-color-redLight option margin-top-none" style="display:none;" id="deletebar">				
                <div class=" padding10">Are you sure you want to remove the selected users</div>
                <div class="clearfix"></div>
                <button class="bg-color-redLight margin-bottom-none" style="margin-right: 0px;float:left;" id="btnDeleteUsers"><i class="icon-checkmark"></i>Confirm</button>
                <button class="bg-color-redLight margin-bottom-none" id="btnCancelDeleteUsers" ><i class="icon-cancel-2"></i>Cancel</button>                
            </div>
			
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
            <table id="users1" class="hovered">
            <thead>
				<tr>
					<td width="25px">
                        <label class="input-control checkbox " onclick="javascript:selectall();" style="margin-left:5px;" >
                        <input type="checkbox" id="maintick"><span class="helper"></span></label>
                    </td>
					<td>Teller Name</td>
                    <td>Email/Mobile</td>
                    <td>Access</td>
                    <td>Status</td>
                    <td  style= "text-align:center; width:25px;"><i  title="Sales Password lock/unlock - Locked requires a password be entered every time a sale is made from the POS page" class='icon-locked'></i></td>
                    <td width="25px"></td>
                    <td width="25px"><a href='javascript:loadusers();'><i class='icon-loop'></i></a></td>
				</tr>
			</thead>
			<tbody>
                  
            </tbody>
            </table>
		</div>
        
    </div>
    </div>
</div>
<script type="text/javascript" >
    addLoadEvent(function () {
       
        loadusers();
        checkStatusOnline();
        setInterval(function () { checkStatusOnline(); }, 30000);
        ;
    });
    $('#btnAddUser').click(function (e) {
        adduser();
    });
    $("#btnDeleteUsers").click(function () {
        $(".option").hide(50);
        $('tbody tr').filter(':has(:checkbox:checked)').each(function () {          
            ChangeUserAccessStatus($('td', this)[1].innerHTML, $('input', $('td', this)).attr("id"), true);
        });

    });
    $("#btnActivateUsers").click(function () {
        $(".option").hide(50);
        $('tbody tr').filter(':has(:checkbox:checked)').each(function () {
            ChangeUserAccessStatus($('td', this)[1].innerHTML, $('input', $('td', this)).attr("id"), false);
        });
        viewActiveUsers();
        updatecount();
    });   
    $("#btnCancelDeleteUsers").click(function () {
        $(".option").hide(50);
    });    
    $("#btnViewDeleted").click(function () {
        $(".option").hide(50);
        viewDeletedUsers();
    });

    $("#btnViewActive").click(function () {
        $(".option").hide(50);
        viewActiveUsers();
    });   
    
    $("#btnDelete").click(function () {
        $(".option").hide(50);
        $("#deletebar").show(150);
    });

    function viewDeletedUsers() {
        loadusers_Deleted();
        $("#btnViewActive").show();
        $("#btnViewDeleted").hide();
        $("#btnActivateUsers").show();
        $("#btnDelete").hide();
    };
    function viewActiveUsers() {
        loadusers();
        $("#btnViewDeleted").show();
        $("#btnViewActive").hide();
        $("#btnActivateUsers").hide();
        $("#btnDelete").show();
    };
</script>
<!-- Body File end -->
</asp:Content>

