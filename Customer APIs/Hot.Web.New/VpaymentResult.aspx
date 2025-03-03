<%@ Page Language="VB" AutoEventWireup="false"  MasterPageFile="~/Main.master" CodeFile="VpaymentResult.aspx.vb" Inherits="VPayments_Result" StyleSheetTheme="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<title>Vpayment Transaction Result</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="page secondary ">
        <div class="page-header">
            <div class="page-header-content">
                <h1>Vpayment <small>Transaction Result</small></h1>
                <a href="javascript:back();" class="back-button big page-back"></a>
            </div>
        </div>
        
        <div class="page-region">
            <div class="page-region-content">
            	<div>
    <h1></h1>
    <p>
        The results of your recent Vpayment transaction are below. If you have any further inquiries, please contact support with your payment reference. You can view this payment in your Transactions list once it has been approved.
    </p>

    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>

    <table class="span8 offset1">
        <tr>
            <td align="right" style="font-weight: bold">Date</td>
            <td>
                <asp:Label ID="lblDate" runat="server" Text="(result)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="font-weight: bold">Result</td>
            <td>
                <asp:Label ID="lblResult" runat="server" Text="(result)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="font-weight: bold">Amount</td>
            <td>
                <asp:Label ID="lblAmount" runat="server" Text="(amount)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="font-weight: bold">Reference</td>
            <td>
                <asp:Label ID="lblvRef" runat="server" Text="(reference)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right" style="font-weight: bold">Additional Details</td>
            <td>
                <asp:Label ID="lblDetails" runat="server" Text="(none)"></asp:Label>
            </td>
        </tr>
        </table>
        </div>
    </div>
</div>
</div>
</asp:Content>
