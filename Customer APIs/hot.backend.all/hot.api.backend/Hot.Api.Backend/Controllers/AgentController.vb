Imports System.Web.Http
Imports Hot.Api.Backend.Models

Namespace Controllers
    <RoutePrefix("api/v1/agents")>
    Public Class AgentController
        Inherits ApiController

        ReadOnly _service As AgentService = New AgentService(Config.IsTestMode())

        <Route("wallet-balance")>
        <HttpGet>
        Function WalletBalance() As WalletBalanceResponse
            Return _service.WalletBalance(Context.Get(Request.Headers))
        End Function

        <Route("wallet-balance-usd")>
        <HttpGet>
        Function WalletBalanceUSD() As WalletBalanceResponse
            Return _service.WalletBalanceUSD(Context.Get(Request.Headers))
        End Function


        ' GET: agents/enduser-balance
        <Route("enduser-balance")>
        <HttpGet>
        Function EndUserBalance(<FromUri> req As EnduserBalanceRequest) As EndUserBalanceResponse
            Return _service.EndUserBalance(req, Context.Get(Request.Headers))
        End Function

        'POST: agents/recharge
        <Route("recharge-pinless")>
        <HttpPost>
        Function RechargePinless(<FromBody> req As RechargePinlessRequest) As RechargeResponseModel
            Return _service.Recharge(req, Context.Get(Request.Headers))
        End Function

        'POST: agents/recharge
        <Route("recharge-pinless-usd")>
        <HttpPost>
        Function RechargePinlessUSD(<FromBody> req As RechargePinlessRequest) As RechargeResponseModel
            Return _service.RechargeUSD(req, Context.Get(Request.Headers))
        End Function

        <Route("query-transaction")>
        <HttpGet>
        Function QueryTransaction(<FromUri> req As QueryTransactionRequest) As QueryTransactionsResponse
            Return _service.QueryTransaction(req, Context.Get(Request.Headers))
        End Function


        'POST: agents/recharge
        <Route("recharge-data")>
        <HttpPost>
        Function RechargeData(<FromBody> req As RechargeDataRequest) As RechargeResponseModel
            Return _service.RechargeData(req, Context.Get(Request.Headers))
        End Function

        'POST: agents/recharge
        <Route("recharge-data-usd")>
        <HttpPost>
        Function RechargeDataUSD(<FromBody> req As RechargeDataRequest) As RechargeResponseModel
            Return _service.RechargeData(req, Context.Get(Request.Headers), True)
        End Function

        <Route("get-data-bundles")>
        <HttpGet>
        Function GetDataBundles() As GetBundleResponse
            Return _service.GetEconetDataBundles(Context.Get(Request.Headers))
        End Function

        <Route("get-data-bundles-all")>
        <HttpGet>
        Function GetDataBundlesAll() As GetBundleResponse
            Return _service.GetAllDataBundles(Context.Get(Request.Headers))
        End Function

        <Route("get-data-bundles-netone")>
        <HttpGet>
        Function GetDataBundles_Netone() As GetBundleResponse
            Return _service.GetNetoneDataBundles(Context.Get(Request.Headers))
        End Function

        <Route("get-data-bundles-usd")>
        <HttpGet>
        Function GetDataBundlesUSD() As GetBundleResponse
            Return _service.GetAllDataBundles(Context.Get(Request.Headers), True)
        End Function


        <Route("wallet-balance-zesa")>
        <HttpGet>
        Function WalletBalanceZesa() As WalletBalanceResponse
            Return _service.WalletBalanceZESA(Context.Get(Request.Headers))
        End Function
        <Route("wallet-balance-zesa-usd")>
        <HttpGet>
        Function WalletBalanceZesaUSD() As WalletBalanceResponse
            Return _service.WalletBalanceZESAUSD(Context.Get(Request.Headers))
        End Function

        <Route("check-customer-zesa")>
        <HttpPost>
        Function QueryTransaction(<FromBody> req As ZesaCustomerInfoRequest) As ZesaCustomerResponseModel
            Return _service.GetCustomerDetails(Context.Get(Request.Headers), req)
        End Function

        <Route("recharge-zesa")>
        <HttpPost>
        Function RechargeZesa(<FromBody> req As RechargeZesaRequest) As RechargeZesaResponseModel
            Return _service.RechargeZesa(req, Context.Get(Request.Headers))
        End Function

        <Route("recharge-zesa-usd")>
        <HttpPost>
        Function RechargeZesaUSD(<FromBody> req As RechargeZesaRequest) As RechargeZesaResponseModel
            Return _service.RechargeZesa(req, Context.Get(Request.Headers), True)
        End Function

        <Route("query-zesa-transaction")>
        <HttpPost>
        Function QueryZesaTransaction(<FromBody> req As QueryZesaRequest) As RechargeZesaResponseModel
            Return _service.QueryZesa(req, Context.Get(Request.Headers))
        End Function

        <Route("complete-zesa")>
        <HttpPost>
        Function CompleteZesa(<FromBody> req As CompleteZesaRequest) As RechargeZesaResponseModel
            Return _service.CompleteRechargeZesa(req, Context.Get(Request.Headers))
        End Function



        <Route("ecocash-request")>
        <HttpPost>
        Function EcoCashRequest(<FromBody> req As EcocashFundingRequest) As EcocashAPI.Transaction
            Return _service.EcocashFundAccount(req, Context.Get(Request.Headers))
        End Function

        <Route("ecocash-request-usd")>
        <HttpPost>
        Function EcoCashRequestUSD(<FromBody> req As EcocashFundingRequest) As EcocashAPI.Transaction
            req.Account = 3
            Return _service.EcocashFundAccount(req, Context.Get(Request.Headers))
        End Function

        <Route("onemoney-request")>
        <HttpPost>
        Function OneMoneyRequest(<FromBody> req As EcocashFundingRequest) As EcocashAPI.Transaction
            Return _service.OneMoneyFundAccount(req, Context.Get(Request.Headers))
        End Function

        <Route("onemoney-request-usd")>
        <HttpPost>
        Function OneMoneyRequestUSD(<FromBody> req As EcocashFundingRequest) As EcocashAPI.Transaction
            req.Account = 3
            Return _service.OneMoneyFundAccount(req, Context.Get(Request.Headers))
        End Function

        <Route("query-evd")>
        <HttpGet>
        Function GetEVDStock() As QueryEVDResponse
            Return _service.QueryEvdStock(Context.Get(Request.Headers))
        End Function


        <Route("bulk-evd")>
        <HttpPost>
        Function BulkEVD(<FromBody> req As BulkEvdRequest) As BulkEvdResponse
            Return _service.BulkEvd(req, Context.Get(Request.Headers))
        End Function

        <Route("recharge-evd")>
        <HttpPost>
        Function RechargeEVD(<FromBody> req As RechargeEvdRequest) As BulkEvdResponse
            Return _service.RechargeEvd(req, Context.Get(Request.Headers))
        End Function


        <Route("query-telone-bundles")>
        <HttpGet>
        Function GetADSLStock() As QueryADSLResponse
            Return _service.QueryAdslStock(Context.Get(Request.Headers))
        End Function

        <Route("query-telone-bundles-usd")>
        <HttpGet>
        Function GetADSLStockUSD() As QueryADSLResponse
            Return _service.QueryAdslStock(Context.Get(Request.Headers), True)
        End Function


        <Route("bulk-telone-evd")>
        <HttpPost>
        Function BulkTeloneEVD(<FromBody> req As BulkADSLRequest) As BulkADSLResponse
            Return _service.BulkAdsl(req, Context.Get(Request.Headers))
        End Function

        <Route("recharge-telone-adsl")>
        <HttpPost>
        Function RechargeTeoneAdsl(<FromBody> req As RechargeADSLRequest) As BulkADSLResponse
            Return _service.RechargeAdsl(req, Context.Get(Request.Headers))
        End Function
        <Route("recharge-telone-adsl-usd")>
        <HttpPost>
        Function RechargeTeoneAdslUsd(<FromBody> req As RechargeADSLRequest) As BulkADSLResponse
            Return _service.RechargeAdsl(req, Context.Get(Request.Headers), True)
        End Function

        <Route("verify-telone-account")>
        <HttpGet>
        Function VerifyTeloneAccount(<FromUri> req As VerifyAccountADSLRequest) As VerifyAccountADSLResponse
            Return _service.VerifyAdslAccount(req, Context.Get(Request.Headers))
        End Function

        <Route("query-telone-balance")>
        <HttpGet>
        Function GetADSLBalance(<FromUri> req As EndUserAdslBalanceRequest) As EndUserAdslBalanceResponse
            Return _service.GetADSLBalance(req, Context.Get(Request.Headers))
        End Function

        '<Route("pay-telone-bill")>
        '<HttpPost>
        'Function PayTeloneBill(<FromBody> req As PayTeloneBillRequest) As PayTeloneBillResponse
        '    Return _service.PayTeloneBill(req, Context.Get(Request.Headers))
        'End Function

        <Route("recharge-telone-voip")>
        <HttpPost>
        Function RechargeTeloneVoip(<FromBody> req As RechargeTeloneVoipRequest) As RechargeTeloneVoipResponse
            Return _service.RechargeTeloneVoip(req, Context.Get(Request.Headers))
        End Function


        <Route("query-evd-usd")>
        <HttpGet>
        Function GetUSDEVDStock() As QueryEVDResponse
            Return _service.QueryUSDEvdStock(Context.Get(Request.Headers))
        End Function

        <Route("recharge-evd-usd")>
        <HttpPost>
        Function RechargeUSDEVD(<FromBody> req As RechargeEvdRequest) As BulkEvdResponse
            Return _service.RechargeUSDEvd(req, Context.Get(Request.Headers))
        End Function


        <Route("query-nyaradzo-account")>
        <HttpGet>
        Function GetNyaradzoAccount(<FromUri> PolicyNumber As String) As NyaradzoCustomerResponseModel
            Return _service.QueryNyaradzoAccount(PolicyNumber, Context.Get(Request.Headers))
        End Function

        <Route("nyaradzo-payment")>
        <HttpPost>
        Function NyaradzoPayment(<FromBody> req As NyaradzoPaymentRequest) As RechargeResponseModel
            Return _service.MakeNyaradzoPayment(req, Context.Get(Request.Headers))
        End Function



    End Class


End Namespace