Imports System.ServiceModel
Imports Hot.Core.EconetPay

Namespace ServiceWrappers
    Public Class ChannelFactoryWrapper (Of T)
        Implements INetworkClient(Of T)

        Private ReadOnly _factory As ChannelFactory(Of T)

        Public Sub New(factory As ChannelFactory(Of T))
            _factory = factory
        End Sub

        Public Function GetNetwork() As T Implements INetworkClient(Of T).GetNetwork
            Return _factory.CreateChannel()
        End Function

        Public Sub Close() Implements INetworkClient(Of T).Close
            _factory.Close()
        End Sub

        Public Sub Abort() Implements INetworkClient(Of T).Abort
            _factory.Abort()
        End Sub     
    End Class

End NameSpace