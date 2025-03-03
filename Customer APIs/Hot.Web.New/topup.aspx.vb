Imports HOT5.Common
Imports System.Data.SqlClient
Imports Common
Imports vPayments

Partial Class Topup
    Inherits System.Web.UI.Page

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.ServerClick
        Dim iAccess As HOT5.Common.xAccess = Session.Item("HOTAccess")
        Dim connection As New SqlConnection(Conn)
        connection.Open()

        Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, connection)

        ' Dim transactionBatches As List(Of xBankTrxBatch) = xBankTrxBatchAdapter.List(xBank.Banks.vPayments, connection)

        'initialize a new batch using vpayments bank and todays date in case none exists in DB
        Dim transactionBatch As xBankTrxBatch = xBankTrxBatchAdapter.GetCurrentBatch(New xBankTrxBatch With {
            .BankID = xBank.Banks.vPayments,
            .BatchDate = DateTime.Now,
            .LastUser = "VpaymentsUser",
            .BatchReference = "Vpayments " + DateTime.Now.ToString("d MMM yyyy")
        }, connection)

        'parse the topup amount and validate it
        Dim amount As Decimal = 0
        Try
            amount = Decimal.Parse(txtAmount.Value)
            If (amount > 1500000 Or amount < 1) Then Throw New Exception()
        Catch ex As Exception
            showex("The topup amount must be a valid amount between 1 and 1500000")
            Exit Sub
        End Try

        Dim guid As Guid = New Guid().NewGuid()
        Dim smm As New Softwarehouse.Messaging.SimplifiedMessageManager(Vpayments.GetLogger())

        ' create local transaction
        Dim transaction = New xBankTrx() With {
            .Amount = amount,
            .TrxDate = Date.Now,
            .Identifier = iAccess.AccessCode,
            .BankTrxState = New xBankTrxState With {.BankTrxStateID = xBankTrxState.BankTrxStates.Pending, .BankTrxState = CType(xBankTrxState.BankTrxStates.Pending, String)},
            .BankTrxType = New xBankTrxType With {.BankTrxType = CType(xBankTrxType.BankTrxTypes.vPaymentsPending, String), .BankTrxTypeID = xBankTrxType.BankTrxTypes.vPaymentsPending},
            .BankTrxBatchID = transactionBatch.BankTrxBatchID,
            .RefName = iAccount.AccountName,
            .Branch = Request.Url.OriginalString,
            .BankRef = "na"
        }

        'save the transaction
        xBankTrxAdapter.Insert(transaction, connection)

        'communicate with vpayments to get a url to redirect to for the user to make his payment
        Try
            Dim merchantKey As Guid = guid.Parse(ConfigurationManager.AppSettings("merchant_key"))

            'create a vpayment record
            Dim vpayment = New xvPayment() With {
                .vPaymentID = guid
            }

            Dim result As String = smm.ProcessTransaction(New Dictionary(Of String, String) From {
                                                          {"confirmurl", ConfigurationManager.AppSettings("public_url") + "/VpaymentUpdateStatus.aspx?guid=" + guid.ToString()},
                                                          {"returnurl", ConfigurationManager.AppSettings("public_url") + "/VpaymentResult.aspx?guid=" + guid.ToString()},
                                                          {"reference", "HOT Recharge - " + transaction.BankTrxID.ToString("#000000")},
                                                          {"amount", transaction.Amount.ToString("F02")},
                                                          {"storefrontid", ConfigurationManager.AppSettings("merchant_id")},
                                                          {"additionalinfo", "HOT Recharge Account Topup"},
                                                          {"Status", "Message"}}, ConfigurationManager.AppSettings("vpayments_url"), merchantKey)

            Dim msg = smm.ParseString(result, merchantKey)

            If (msg("status").ToLower() = "ok") Then
                vpayment.CheckURL = msg("checkurl")
                vpayment.ProcessURL = msg("processurl")
                vpayment.BankTrxID = CType(transaction.BankTrxID, Integer)
                vpayment.ErrorMsg = "na"
                vpayment.vPaymentRef = "na"

                'save the vpayment
                xBankvPaymentAdapter.Insert(vpayment, connection)

                ' redirect to vpayments
                Response.Redirect(vpayment.ProcessURL)
            Else
                vpayment.ErrorMsg = msg("error")
                xBankvPaymentAdapter.Insert(vpayment, connection)
                Throw New Exception("Response message status was not OK")
            End If


        Catch err As Exception
            showex("Sorry, we failed to initiate a payment with vPayments. Please call us directly for assistance and quote the following error - " + err.Message)

        End Try
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            Dim iAccess As xAccess = Session.Item("HOTAccess")
            If iAccess Is Nothing Then
                Response.Redirect("index.aspx")
            End If
        Catch ex As Exception
            showex(ex.Message)
        End Try

    End Sub
    Sub showex(ByVal message As String)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "showmsg", "showalert('VPayment Error','" + message.Replace("'", """") + "');", True)
    End Sub
End Class
