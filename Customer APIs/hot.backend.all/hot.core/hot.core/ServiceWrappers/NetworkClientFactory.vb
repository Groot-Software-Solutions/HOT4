Imports System.ServiceModel

Namespace ServiceWrappers

    Public Class NetworkClientFactory
        Public Shared Function Create(Of T)(endpointAddress As String, Optional timeout As Integer = 30000) As INetworkClient(Of T)
            Dim binding = New BasicHttpBinding()
            binding.SendTimeout = New TimeSpan(0, 0, timeout / 1000)
            Dim channelFactory As ChannelFactory = New ChannelFactory(Of T)(binding, New EndpointAddress(endpointAddress))
            Dim wrapper = New ChannelFactoryWrapper(Of T)(channelFactory)
            Return wrapper
        End Function
    End Class
End NameSpace