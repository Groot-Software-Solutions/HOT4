Namespace ServiceWrappers
    Public Interface INetworkClient (Of T)
        Function GetNetwork() As T
        Sub Close()
        Sub Abort()
    End Interface
End NameSpace