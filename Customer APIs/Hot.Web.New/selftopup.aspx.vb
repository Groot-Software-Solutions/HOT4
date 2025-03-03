Imports System.Net
Imports System.IO
Imports System.Text
Imports Newtonsoft.Json
Imports Common
Imports System.Data.SqlClient
Imports HOT5.Common

Partial Class selftopup
    Inherits System.Web.UI.Page

    Private Sub cmdSelfGo_ServerClick(sender As Object, e As EventArgs) Handles cmdSelfGo.ServerClick

        Dim TargetNumber As String = Sanitize(txtNumber.Value).Replace("-", "")
        Dim EcocashNumber As String = Sanitize(txtEcoNumber.Value).Replace("-", "")
        Dim Amount As Decimal = 0
        Try
            Amount = Sanitize(txtAmount.Value)
        Catch ex As Exception
            lblMessage.InnerHtml = "<span style='color:red'>Enter valid amount to recharge</span>"
        End Try
        ' If IsReCaptchValid() Then
        If True Then
            If Not IsValidNumber(EcocashNumber) And IsEcoCashNumber(EcocashNumber) Then
                lblMessage.InnerHtml = "<span style='color:red'>Enter valid Ecocash number</span>"
                Exit Sub
            End If
            If Not IsValidNumber(TargetNumber) Then
                lblMessage.InnerHtml = "<span style='color:red'>Enter valid number to recharge</span>"
                Exit Sub
            End If

            If Not IsAllowedtoSelfTopUp(EcocashNumber) Then
                txtContent.InnerHtml = "<div style = 'text-align: center;margin: 3em;' Class='span6'><h2><i class='icon-cancel red'></i> Invalid Account Type</h2><h3>Please use the SMS method for recharging</h3><div style='margin-bottom: 5em;'>Recharging with this portal is not allowed with your current acccount. If you require assistance on how to use your account please contact support on 0772929223</div> <div><h4>How to sell using SMS</h4><small>Send a SMS To <strong>180</strong> as shown below with your amount and mobile number replaced with your requirements. No data or airtime is used to send the SMS<br><br></small><strong>HOT <em>Amount MobileNumber Pin</em></strong></div></div>"

                Exit Sub
            End If

            If SendSelfTopUp(EcocashNumber, TargetNumber, Amount) Then
                txtContent.InnerHtml = "<div style = 'text-align: center;margin: 3em;' Class='span6'><h2 id='pageheading'><img src='images/preloader-w8-cycle-black.gif' height='20px'> Processing</h2><h3>Please confirm the transaction on your phone by putting your Ecocash Pin</h3><div style='margin-bottom: 5em;'>Recharge for&nbsp;<strong>" + TargetNumber + "</strong> of <strong>" + FormatNumber(txtAmount.Value, 2, True) + "</strong> has been requested</div>            <div><h4>Want to do more transactions</h4><small>Just send a SMS To <strong>180</strong> as shown below with your amount and mobile number replaced with your requirements. No data or airtime is used to send the SMS<br><br></small><strong>TOPUP <em>Amount MobileNumber</em></strong></div></div>"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CompleteTan", "setTimeout(function() {$(""#pageheading"")[0].innerHTML=""<i class='icon-checkmark green circle'></i> Recharge Sent"";}, 5000);", True)
            Else
                lblMessage.InnerHtml = "<span style='color:red'>Error processing transaction please try again</span>"
            End If

        Else
            lblMessage.InnerHtml = "<span style='color:red'>Please tick the checkmark</span>"
        End If


    End Sub

    Private Function SendSelfTopUp(ecocashNumber As String, targetNumber As String, amount As Decimal) As Boolean
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iSMS As xSMS = New xSMS() With {
                    .SmppID = 1,
                    .Direction = True,
                    .Mobile = ecocashNumber,
                    .SMSText = "TopUp " + amount.ToString() + " " + targetNumber
                }

                xSMSAdapter.Save(iSMS, sqlConn)
                Return True
            End Using
        Catch ex As Exception
            Console.WriteLine(ex)
        End Try
        Return False
    End Function

    Private Function IsAllowedtoSelfTopUp(ecocashNumber As String) As Boolean
        Try
            Using sqlConn As New SqlConnection(Conn)
                sqlConn.Open()
                Dim iAccess As xAccess = xAccessAdapter.SelectCode(ecocashNumber, sqlConn)
                If iAccess Is Nothing Then Return True
                Dim iAccount As xAccount = xAccountAdapter.SelectRow(iAccess.AccountID, sqlConn)
                If iAccount.Profile.ProfileID = 6 Then Return True
            End Using
        Catch ex As Exception
        End Try
        Return True ' allow everyone to self top for now 


    End Function

    Private Function IsEcoCashNumber(ecocashNumber As String) As Boolean
        If ecocashNumber.StartsWith("077") Then Return True
        If ecocashNumber.StartsWith("078") Then Return True
        Return False
    End Function



    Public Function IsReCaptchValid() As Boolean

        Dim captchaResponse As String = Request.Form("g-recaptcha-response")
        Dim secretKey As String = ConfigurationManager.AppSettings("CaptchaSecretKey")
        Dim apiUrl As String = "https://www.google.com/recaptcha/api/siteverify"
        Dim data As String = "secret=" & secretKey & "&response=" & captchaResponse

        Dim myrequest As HttpWebRequest, response As HttpWebResponse = Nothing, reader As StreamReader
        Dim byteData() As Byte, postStream As Stream = Nothing, ResponseData As String, result As New xGoogleCaptchaResponse

        myrequest = DirectCast(WebRequest.Create(apiUrl), HttpWebRequest)
        myrequest.Method = "POST"
        myrequest.ContentType = "application/x-www-form-urlencoded"

        byteData = Encoding.UTF8.GetBytes(data)
        myrequest.ContentLength = byteData.Length


        ' Write data  
        Try
            postStream = myrequest.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)
        Finally
            If Not postStream Is Nothing Then postStream.Close()
        End Try

        Try
            ' Get response  
            response = DirectCast(myrequest.GetResponse(), HttpWebResponse)

            ' Get the response stream into a reader  
            reader = New StreamReader(response.GetResponseStream())

            ' Console application output  
            ResponseData = (reader.ReadToEnd())

            result = JsonConvert.DeserializeObject(ResponseData, GetType(xGoogleCaptchaResponse))
        Catch wex As WebException
            If Not wex.Response Is Nothing Then
                Dim errorResponse As HttpWebResponse = Nothing
                Try
                    errorResponse = DirectCast(wex.Response, HttpWebResponse)

                Finally
                    If Not errorResponse Is Nothing Then errorResponse.Close()
                End Try
            End If

        Finally
            If Not response Is Nothing Then response.Close()
        End Try
        Return result.success
    End Function


End Class
Public Class xGoogleCaptchaResponse

    Public Property success As Boolean
    Public Property challenge_ts As String
    Public Property hostname As String

End Class