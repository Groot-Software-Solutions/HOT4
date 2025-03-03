Imports Common
Imports Softwarehouse.Messaging
Imports Microsoft.VisualBasic
Imports HOT5.Common
Imports System.Data.SqlClient
Imports log4net
Imports log4net.Config

Public Class Vpayments
    Public Shared Function GetLogger() As ILog
        Dim logger As ILog = LogManager.GetLogger(GetType(Vpayments))
        XmlConfigurator.Configure()
        Return logger
    End Function

    Public Shared Sub UpdateStatus(ByVal guid As String)
        Try
            Dim connection As New SqlConnection(Conn)
            connection.Open()

            Dim vPaymentID As Guid = New Guid(guid)
            Dim logger = GetLogger()

            'get the transaction from the db
            Dim transaction = xBankTrxAdapter.GetFromvPayment(vPaymentID, connection)

            'get the vpayment from the db
            Dim vpayment = xBankvPaymentAdapter.SelectRow(New xvPayment() With {.vPaymentID = vPaymentID}, connection)

            If (Not transaction.BankTrxState.BankTrxStateID = xBankTrxState.BankTrxStates.Pending) Then
                Throw New Exception("Cannot update transaction state. The transaction state ID is " + CType(transaction.BankTrxState.BankTrxStateID, xBankTrxState.BankTrxStates).ToString() + ". It cannot be updated when in this state.")
            End If

            Dim merchantKey As Guid = System.Guid.Parse(ConfigurationManager.AppSettings("merchant_key"))

            Dim smm As SimplifiedMessageManager = New SimplifiedMessageManager(logger)

            Dim result As String = smm.ProcessTransaction(New Dictionary(Of String, String), vpayment.CheckURL, merchantKey)

            Dim msg As Dictionary(Of String, String) = smm.ParseResultString(result, merchantKey)

            'used to save the bank reference in the transaction but no longer a facility available to do this within HOT5.Common
            'so, if a bank ref exists and isnt empty, save that in the vpayment, otherwise save vpayments ref
            vpayment.vPaymentRef = If(msg.Keys.Contains("bankreference") And Not String.IsNullOrEmpty(msg("bankreference")), "BANK: " + msg("bankreference"), "VPAYMENT: " + msg("reference"))

            logger.Info("Going to update transaction status of " + transaction.RefName + " to " + msg("status"))

            Select Case msg("status")
                Case "Failed"
                Case "Created but not Paid"
                Case "Awaiting Redirect"
                    vpayment.ErrorMsg = msg("status")
                    xBankvPaymentAdapter.UpdateRefAndError(vpayment, connection)

                Case "Paid"
                    transaction.BankTrxState = New xBankTrxState() With {
                        .BankTrxState = CType(xBankTrxState.BankTrxStates.Success, String),
                        .BankTrxStateID = xBankTrxState.BankTrxStates.Success
                    }
                    transaction.BankTrxType = New xBankTrxType() With {
                        .BankTrxType = CType(xBankTrxType.BankTrxTypes.vPaymentsConfirmed, String),
                        .BankTrxTypeID = xBankTrxType.BankTrxTypes.vPaymentsConfirmed
                    }
                    vpayment.vPaymentRef = If(msg.Keys.Contains("bankreference"), msg("bankreference"), "")

                    xBankvPaymentAdapter.UpdateRefAndError(vpayment, connection)
                    xBankTrxAdapter.UpdateState(transaction, connection)

                    'get the user's account
                    Dim access As xAccess = xAccessAdapter.SelectCode(transaction.Identifier, connection)

                    'create a payment for their account
                    Dim payment As New xPayment() With {
                        .AccountID = access.AccountID,
                        .Amount = transaction.Amount,
                        .PaymentDate = DateTime.Now,
                        .PaymentSource = New xPaymentSource() With {
                            .PaymentSource = CType(xPaymentSource.PaymentSources.vPayment, String),
                            .PaymentSourceID = xPaymentSource.PaymentSources.vPayment
                        },
                        .PaymentType = New xPaymentType() With {
                            .PaymentType = CType(xPaymentType.PaymentTypes.BankAuto, String),
                            .PaymentTypeID = xPaymentType.PaymentTypes.BankAuto
                        },
                        .Reference = vpayment.vPaymentRef,
                        .LastUser = "Vpayments"
                    }

                    'save the payment
                    xPaymentAdapter.Save(payment, connection)

                    'link the payment to the bank transaction
                    transaction.PaymentID = payment.PaymentID
                    xBankTrxAdapter.UpdatePaymentID(transaction, connection)

                Case "Cancelled"
                    transaction.BankTrxState = New xBankTrxState() With {
                        .BankTrxState = CType(xBankTrxState.BankTrxStates.Suspended, String),
                        .BankTrxStateID = xBankTrxState.BankTrxStates.Suspended
                    }
                    vpayment.ErrorMsg = "Cancelled by the user"
                    vpayment.vPaymentRef = If(msg.Keys.Contains("reference"), msg("reference"), "")

                    xBankvPaymentAdapter.UpdateRefAndError(vpayment, connection)
                    xBankTrxAdapter.UpdateState(transaction, connection)

            End Select
        Catch err As Exception
            GetLogger().Error(err)
            Throw New Exception("Failed to update the status of the Vpayment transaction - " + err.Message)
        End Try
    End Sub
End Class
