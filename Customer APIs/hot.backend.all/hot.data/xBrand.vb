Imports System.Collections.Concurrent
Imports System.Data.SqlClient
Imports System.Linq

Public Class xBrand
    Public Enum Brands As Integer
        EconetUSD = 1
        Text = 2
        EasyCall = 3
        Juice = 4
        EconetPlatform = 5
        Prepaid = 5
        Econet078 = 6
        EconetBB = 7
        EconetTXT = 8
        TelecelUSD = 10
        TelecelTXT = 11
        TelecelBB = 12
        Africom = 16
        Nyaradzo = 17
        EconetData = 19
        EconetFacebook = 20
        EconetWhatsapp = 21
        EconetInstagram = 22
        EconetTwitter = 23
        EasyCallEVD = 24
        NetoneOneFusion = 25
        NetoneOneFi = 26
        ZETDC = 27
        ZOL = 28
        MunicipalHarare = 29
        MunicipalGweru = 30
        MunicipalBulawayo = 31
        ZETDCFees = 32
        TeloneVoice = 33
        TeloneBroadband = 34
        TeloneLTE = 35
        TeloneVoip = 36
        TeloneBillPayment = 37
        NetoneUSD = 38
        NetoneData = 39
        NetoneSocial = 40
        NetoneWhatsApp = 41
        NetoneSMS = 42
        TeloneUSD = 43
        ZETDCUSD = 44
        NyaradzoUSD = 45
    End Enum

    Private _BrandID As Integer
    Public Property BrandID() As Integer
        Get
            Return _BrandID
        End Get
        Set(ByVal value As Integer)
            _BrandID = value
        End Set
    End Property

    Private _Network As xNetwork
    Public Property Network() As xNetwork
        Get
            Return _Network
        End Get
        Set(ByVal value As xNetwork)
            _Network = value
        End Set
    End Property

    Private _BrandName As String
    Public Property BrandName() As String
        Get
            Return _BrandName
        End Get
        Set(ByVal value As String)
            _BrandName = value
        End Set
    End Property

    Private _BrandSuffix As String
    Public Property BrandSuffix() As String
        Get
            Return _BrandSuffix
        End Get
        Set(ByVal value As String)
            _BrandSuffix = value
        End Set
    End Property

    Private _WalletTypeId As Int32
    Public Property WalletTypeId() As Int32
        Get
            Return _WalletTypeId
        End Get
        Set(ByVal value As Int32)
            _WalletTypeId = value
        End Set
    End Property

    Public ReadOnly Property Key As String
        Get
            Return GetKey(Network, BrandSuffix)
        End Get
    End Property

    Sub New()
        _Network = New xNetwork
    End Sub
    Sub New(ByVal sqlRdr As SqlDataReader)
        _BrandID = sqlRdr("BrandID")

        _BrandName = sqlRdr("BrandName")
        _BrandSuffix = sqlRdr("BrandSuffix")
        Try
            _WalletTypeId = sqlRdr(6)
        Catch
        End Try
        _Network = New xNetwork(sqlRdr)
    End Sub
    Public Overrides Function ToString() As String
        Return _BrandName
    End Function

    Public Shared Function GetKey(iNetwork As xNetwork, suffix As String) As String
        Return Trim(iNetwork.NetworkID & suffix)
    End Function
    Private Shared Function HasColumn(dr As SqlDataReader, columnName As String) As Boolean
        For i As Integer = 0 To dr.FieldCount
            If dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase) Then Return True
        Next
        Return False
    End Function
End Class

''' <summary>
''' TODO: Add comments to describe the lazy loading of the dictionary
''' </summary>
Public Class xBrandAdapter
    Public Shared Property BrandsByKey As ConcurrentDictionary(Of String, xBrand) = New ConcurrentDictionary(Of String, xBrand)

    Public Shared ReadOnly Property BrandsById As Dictionary(Of Integer, xBrand) = New Dictionary(Of Integer, xBrand)


    Public Shared Function GetBrand(iNetwork As xNetwork, suffix As String, sqlConn As SqlConnection) As xBrand
        Dim key = xBrand.GetKey(iNetwork, suffix)
        If BrandsByKey.ContainsKey(key) Then
            Return BrandsByKey(key)
        End If
        List(sqlConn)
        If BrandsByKey.ContainsKey(key) Then
            Return BrandsByKey(key)
        Else
            Throw New Exception(String.Format("The brand with key '{0}' is not registered in HOT db", key))
        End If
    End Function

    Public Shared Function GetBrand(brandId As Integer, sqlConn As SqlConnection) As xBrand
        If BrandsById.ContainsKey(brandId) Then
            Return BrandsById(brandId)
        End If
        List(sqlConn)
        Return BrandsById(brandId)
    End Function


    'all under here added by KMR 16 dec 2012 from office web server
    Public Shared Function List(sqlConn As SqlConnection) As List(Of xBrand)
        BrandsByKey = New ConcurrentDictionary(Of String, xBrand)
        Using sqlCmd As New SqlCommand("xBrand_List", sqlConn)
            sqlCmd.CommandType = CommandType.StoredProcedure
            Using sqlRdr As SqlDataReader = sqlCmd.ExecuteReader
                While sqlRdr.Read
                    Dim brand As xBrand = New xBrand(sqlRdr)
                    BrandsByKey(Trim(brand.Key)) = brand
                    BrandsById(brand.BrandID) = brand
                End While
                sqlRdr.Close()
            End Using
        End Using
        Return BrandsByKey.Select(Function(x) x.Value).ToList()
    End Function
End Class