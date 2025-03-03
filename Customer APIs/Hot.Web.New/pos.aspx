
<%@ Page Title="" Language="VB" MasterPageFile="~/main.master" AutoEventWireup="false" CodeFile="pos.aspx.vb" Inherits="pos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" src="js/assets/effect.js"></script>
<script type="text/javascript" src="js/assets/effect-shake.js"></script>
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 

	    <div class="page" id="page-index">
        <div id="options" style="display: none;">
            <input type="checkbox" class="txtPasswordRequired" id="txtPasswordRequired" name="txtPasswordRequired" />
        </div>
        <div class="page-region">
            <div class="page-region-content">
                <h1 style="">Point of Sale</h1>
                <div class="row-content">       
                    <div class="pos-form">
                      
                        <form class="shadow p-3 mb-5 bg-white rounded" id="payment-form">
                            <div class="input-radio-container">
                                <div class="label_input">
                                    <label for="txtPhoneNumber">
                                        Enter Mobile Number                                 
                                    </label>
                                    <input id="txtPhoneNumber" type="text" placeholder="Enter Phone Number" maxlength="13" onkeydown="javascript: number_phone_pos(this);" onkeyup="javascript: number_phone_pos(this);" />
                                </div>

                                <div class="radio-buttons" id="recharge-type-buttons">
                                    <label class="custom-radio">
                                        <input type="radio" name="rechargeType" onclick="javascript: checkMode();" value="Airtime" checked />
                                        <span class="radio-btn"><i class="las la-check" "></i>
                                            <h3 class="radio-text">Airtime</h3>
                                        </span>
                                    </label>
                                    <label class="custom-radio">
                                        <input type="radio" name="rechargeType" onclick="javascript: checkMode();" value="Data" />
                                        <span class="radio-btn"><i class="las la-check"></i>
                                            <h3 class="radio-text">Data</h3>
                                        </span>
                                    </label>

                                </div>

                            </div>
                            <div class="input-radio-container">
                                <div class="input-control text disabled  no-bottom-margin">
                                    <label for="txtAmount">
                                        Enter Amount
                                    </label>
                                    <input id="txtAmount" type="text" placeholder="Enter Amount" maxlength="13" onkeydown="javascript:number_amount(this)" onkeyup="javascript:number_amount(this)" />
                                  <%--  <button class="btn-clear" tabindex="-1" type="button"></button>--%>
                                     <div id="txtBundleDiv" class="input-control select" style="display: none; margin-top:19px;">
     <select id="txtBundle" onkeydown="javascript: checkMode();"></select>
 </div>
 <div id="txtBundleUsdDiv" class="input-control select" style="display: none; margin-top:19px">
     <select id="txtBundleUSD" onkeydown="javascript: checkMode();"></select>
 </div>
                                </div>
                               

                                <div class="radio-buttons" id="recharge-type-buttons">
                                    <label class="custom-radio">
                                        <input type="radio" name="currency" checked onclick="javascript: checkMode();" value="USD" />
                                        <span class="radio-btn"><i class="las la-check"></i>
                                            <h3 class="radio-text">USD</h3>
                                        </span>
                                    </label>
                                    <label class="custom-radio">
                                        <input type="radio" name="currency" onclick="javascript: checkMode();" value="ZiG" />
                                        <span class="radio-btn"><i class="las la-check" ></i>
                                            <h3 class="radio-text">ZiG</h3>
                                        </span>
                                    </label>

                                </div>

                            </div>


                            <button type="button" id="confirm-button" onclick="javascript:rechargebtn(true);">Recharge</button>
                        </form>
                    </div>

                    <div class="sales-card shadow p-3  mb-5 bg-white rounded border-radius: 10px; box-shadow: 0 0 20px #c3c3c367;">
                        <h3><b>Sales for the day</b></h3>
                        <div class="" id="card">
                            <table class="sales-table">
                                <thead>
                                    <tr>
                                        <th>Wallet Type</th>
                                        <th>Amount $</th>
                                    </tr>
                                    <tr>
                                        <td>USD</td>
                                        <td id ="USDSales"></td>
                                    </tr>
                                     <tr>
                                         <td>ZiG</td>
                                         <td id="ZiGSales"></td>
                                     </tr>
                                        <tr>
                                        <td>ZiG ZESA</td>
                                        <td id="ZiGZesaSales"></td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right:10px;">USD ZESA</td>
                                        <td id="USDZesaSales"></td>
                                    </tr>
                                    
                                   
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">

                <div class="padding10 bg-color-blueLight" style="min-height: 310px; margin-top: 10px; border-radius: 10px; box-shadow: 0 0 20px #c3c3c367;">
                    <h2><i class="icon-clipboard-2"></i>Recharge Log</h2>
                    <table class="hovered" id="rechargelog">
                        <thead>
                            <tr>
                                <th scope="col" style="width:1px;"></th>
                                <th scope="col" style="width:160px;">Phone Number</th>
                                <th scope="col" style="width:150px;">Amount</th>
                                <th scope="col" style="width:100px;">Status</th> 
                            </tr>
                        </thead>
                        <tbody id="rechargelogbody">
                                   
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

    </div>
    </div>
	
	
    <script type="text/javascript" src="js/assets/pos.js?1.031"></script>
     
<style>
    #txtTotalSales {
        text-align: right;
        padding-top: 30px;
        font-family: monospace;
        font-size: 60px;
    }

    #networkStatus {
        list-style: none;
    }

        #networkStatus li {
            font-size: 25px;
            padding: 10px;
        }

        #networkStatus i {
            float: right;
        }

    #txtAmount {
        font-size: 25px;
        border-radius: 10px;
        text-align: center;
        margin-top: 20px;
    }

    #txtPhoneNumber {
        font-size: 25px;
        border-radius: 10px;
        text-align: center;
        margin-top: 20px;
    }

    #recharge-helper {
        list-style: none;
        height: 90px;
        width: 100%;
    }

        #recharge-helper li {
            width: 100%;
            height: 90px;
        }

    #btnConfirm {
        width: 100%;
        height: 90px;
        font-size: 30px;
    }

    #grpConfirm {
        display: none;
    }

    #grpPassword {
        display: none;
        font-size: 30px;
    }

        #grpPassword i {
            font-size: 25px;
        }

    #txtPassword {
        font-size: 30px;
        width: 200px;
        margin-top: 5px;
        margin-bottom: 5px;
    }

    a#btnRecharge {
        color: black;
        font-size: 16px;
    }

    a#btnCancelTran {
        color: black;
        font-size: 16px;
    }

    #grpUnlocked {
        font-size: 35px;
        padding-bottom: 20px;
        padding-top: 20px;
        display: none;
    }

    table, th, td {
        border: none;
        border-collapse: collapse;
    }

    .row-content {
        display: grid;
        grid-template-columns: 65% auto auto auto;
        grid-gap: 10px;
        grid-column-gap: 44px;
        padding: 10px;
        justify-items: center;
        align-items: stretch;
    }

    .sales-card {
        grid-column: 2 / span 3;
        padding: 26px;
    }


    #confirm-button {
        background: red;
        border-radius: 10px;
        color: white;
        width: 30%;
        height: 46px;
    }

    #payment-form {
        display: flex;
        gap: 24px;
        flex-direction: column;
        box-shadow: 0 0 20px #c3c3c367;
        width: 100%;
        padding: 29px;
        border-radius: 19px;
    }


    #currency-radio-buttons {
        display: flex;
        flex-direction: column;
    }

    #recharge-type-buttons {
        margin-top: 40px;
    }

    .radio-buttons {
        width: 100%;
        margin: 0 auto;
        text-align: center;
    }

    .custom-radio input {
        display: none;
    }

    .custom-radio {
        width: 20px;
        margin-right:75px !important
    }

    .radio-btn {
        margin: 10px;
        margin-left: 1px;
        width: 82px;
        height: 32px;
        border: 3px solid transparent;
        display: inline-block;
        border-radius: 10px;
        position: relative;
        text-align: center;
        box-shadow: 0 0 20px #c3c3c367;
        cursor: pointer;
    }

        .radio-btn > i {
            color: #ffffff;
            background-color: #ff0000;
            font-size: 20px;
            position: absolute;
            top: -15px;
            left: 50%;
            transform: translateX(-50%) scale(2);
            border-radius: 50px;
            padding: 3px;
            transition: 0.5s;
            pointer-events: none;
            opacity: 0;
        }

        .radio-btn .hobbies-icon {
            width: 150px;
            height: 150px;
            position: absolute;
            top: 40%;
            left: 50%;
            transform: translate(-50%, -50%);
        }

            .radio-btn .hobbies-icon img {
                display: block;
                width: 100%;
                margin-bottom: 20px;
            }

            .radio-btn .hobbies-icon i {
                color: #FFDAE9;
                line-height: 80px;
                font-size: 60px;
            }

            .radio-btn .hobbies-icon h3 {
                color: #555;
                font-size: 18px;
                font-weight: 300;
                text-transform: uppercase;
                letter-spacing: 1px;
            }

    .custom-radio input:checked + .radio-btn {
        border: 2px solid #ff0000;
    }

        .custom-radio input:checked + .radio-btn > i {
            opacity: 1;
            transform: translateX(-50%) scale(1);
        }

    .input-radio-container {
        display: flex;
    }

    #txtPhoneNumber {
        width: 300px;
        padding: 10px 0 10px 0;
        font-size: 15px;
        border-radius: 10px;
        text-align: center;
        border: 1px solid #0000003b;
        border: 3px solid transparent;
        box-shadow: 0 0 9px #cccccca1;
    }

    #txtAmount, #txtBundleUSD, #txtBundle {
        width: 300px;
        padding: 10px 0 10px 0;
        font-size: 15px;
        border-radius: 10px;
        text-align: center;
        border: 1px solid #0000003b;
        border: 3px solid transparent;
        box-shadow: 0 0 9px #cccccca1;
        margin-right: 5px;
    }

    #amount {
        width: 300px;
        padding: 10px 0 10px 0;
        font-size: 15px;
        border-radius: 10px;
        text-align: center;
        border: 1px solid #0000003b;
        border: 3px solid transparent;
        box-shadow: 0 0 9px #cccccca1;
    }

    .sales-card {
        border-radius: 10px;
        box-shadow: 0 0 20px #c3c3c367;
        padding: 43px;
        display: grid;
        align-items: end;
        width: 100%;
    }

    table.sales-table > td {
        padding: 20px;
        margin: auto;
        border: none !important;
        border-spacing: 0 !important;
    }

    /* END
</style>
</asp:Content>

