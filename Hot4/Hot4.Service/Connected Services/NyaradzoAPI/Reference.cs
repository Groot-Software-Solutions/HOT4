﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NyaradzoAPI
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="NyaradzoResultModel", Namespace="http://tempuri.org/")]
    public partial class NyaradzoResultModel : object
    {
        
        private bool ValidResponseField;
        
        private string ResponseDataField;
        
        private string ErrorDataField;
        
        private NyaradzoAPI.AccountSummary ItemField;
        
        private NyaradzoAPI.TransactionResult ResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public bool ValidResponse
        {
            get
            {
                return this.ValidResponseField;
            }
            set
            {
                this.ValidResponseField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string ResponseData
        {
            get
            {
                return this.ResponseDataField;
            }
            set
            {
                this.ResponseDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string ErrorData
        {
            get
            {
                return this.ErrorDataField;
            }
            set
            {
                this.ErrorDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public NyaradzoAPI.AccountSummary Item
        {
            get
            {
                return this.ItemField;
            }
            set
            {
                this.ItemField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public NyaradzoAPI.TransactionResult Result
        {
            get
            {
                return this.ResultField;
            }
            set
            {
                this.ResultField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AccountSummary", Namespace="http://tempuri.org/")]
    public partial class AccountSummary : object
    {
        
        private string IdField;
        
        private NyaradzoAPI.DateTimeOffset DateCreatedField;
        
        private string PolicyNumberField;
        
        private string SourceReferenceField;
        
        private string StatusField;
        
        private string ResponseCodeField;
        
        private string MonthlyPremiumField;
        
        private string AmountToBePaidField;
        
        private string NumberOfMonthsField;
        
        private string ResponseDescriptionField;
        
        private string BalanceField;
        
        private string PolicyHolderField;
        
        private string BankCodeField;
        
        private string TxnRefField;
        
        private string CurrencyCodeField;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, EmitDefaultValue=false, Order=1)]
        public NyaradzoAPI.DateTimeOffset DateCreated
        {
            get
            {
                return this.DateCreatedField;
            }
            set
            {
                this.DateCreatedField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string PolicyNumber
        {
            get
            {
                return this.PolicyNumberField;
            }
            set
            {
                this.PolicyNumberField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string SourceReference
        {
            get
            {
                return this.SourceReferenceField;
            }
            set
            {
                this.SourceReferenceField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string Status
        {
            get
            {
                return this.StatusField;
            }
            set
            {
                this.StatusField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string ResponseCode
        {
            get
            {
                return this.ResponseCodeField;
            }
            set
            {
                this.ResponseCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string MonthlyPremium
        {
            get
            {
                return this.MonthlyPremiumField;
            }
            set
            {
                this.MonthlyPremiumField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string AmountToBePaid
        {
            get
            {
                return this.AmountToBePaidField;
            }
            set
            {
                this.AmountToBePaidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string NumberOfMonths
        {
            get
            {
                return this.NumberOfMonthsField;
            }
            set
            {
                this.NumberOfMonthsField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string ResponseDescription
        {
            get
            {
                return this.ResponseDescriptionField;
            }
            set
            {
                this.ResponseDescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public string Balance
        {
            get
            {
                return this.BalanceField;
            }
            set
            {
                this.BalanceField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=11)]
        public string PolicyHolder
        {
            get
            {
                return this.PolicyHolderField;
            }
            set
            {
                this.PolicyHolderField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=12)]
        public string BankCode
        {
            get
            {
                return this.BankCodeField;
            }
            set
            {
                this.BankCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=13)]
        public string TxnRef
        {
            get
            {
                return this.TxnRefField;
            }
            set
            {
                this.TxnRefField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=14)]
        public string CurrencyCode
        {
            get
            {
                return this.CurrencyCodeField;
            }
            set
            {
                this.CurrencyCodeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TransactionResult", Namespace="http://tempuri.org/")]
    public partial class TransactionResult : object
    {
        
        private string MessageField;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Message
        {
            get
            {
                return this.MessageField;
            }
            set
            {
                this.MessageField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DateTimeOffset", Namespace="http://tempuri.org/")]
    public partial class DateTimeOffset : object
    {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="NyaradzoAPI.NyaradzoSoap")]
    public interface NyaradzoSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ProcessPayment", ReplyAction="*")]
        System.Threading.Tasks.Task<NyaradzoAPI.ProcessPaymentResponse> ProcessPaymentAsync(NyaradzoAPI.ProcessPaymentRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Refund", ReplyAction="*")]
        System.Threading.Tasks.Task<NyaradzoAPI.RefundResponse> RefundAsync(NyaradzoAPI.RefundRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AccountQuery", ReplyAction="*")]
        System.Threading.Tasks.Task<NyaradzoAPI.AccountQueryResponse> AccountQueryAsync(NyaradzoAPI.AccountQueryRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ProcessPaymentRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ProcessPayment", Namespace="http://tempuri.org/", Order=0)]
        public NyaradzoAPI.ProcessPaymentRequestBody Body;
        
        public ProcessPaymentRequest()
        {
        }
        
        public ProcessPaymentRequest(NyaradzoAPI.ProcessPaymentRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ProcessPaymentRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string AppKey;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string PolicyNumber;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string reference;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public decimal amount;
        
        public ProcessPaymentRequestBody()
        {
        }
        
        public ProcessPaymentRequestBody(string AppKey, string PolicyNumber, string reference, decimal amount)
        {
            this.AppKey = AppKey;
            this.PolicyNumber = PolicyNumber;
            this.reference = reference;
            this.amount = amount;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ProcessPaymentResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ProcessPaymentResponse", Namespace="http://tempuri.org/", Order=0)]
        public NyaradzoAPI.ProcessPaymentResponseBody Body;
        
        public ProcessPaymentResponse()
        {
        }
        
        public ProcessPaymentResponse(NyaradzoAPI.ProcessPaymentResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ProcessPaymentResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public NyaradzoAPI.NyaradzoResultModel ProcessPaymentResult;
        
        public ProcessPaymentResponseBody()
        {
        }
        
        public ProcessPaymentResponseBody(NyaradzoAPI.NyaradzoResultModel ProcessPaymentResult)
        {
            this.ProcessPaymentResult = ProcessPaymentResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RefundRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Refund", Namespace="http://tempuri.org/", Order=0)]
        public NyaradzoAPI.RefundRequestBody Body;
        
        public RefundRequest()
        {
        }
        
        public RefundRequest(NyaradzoAPI.RefundRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RefundRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string AppKey;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string reference;
        
        public RefundRequestBody()
        {
        }
        
        public RefundRequestBody(string AppKey, string reference)
        {
            this.AppKey = AppKey;
            this.reference = reference;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RefundResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RefundResponse", Namespace="http://tempuri.org/", Order=0)]
        public NyaradzoAPI.RefundResponseBody Body;
        
        public RefundResponse()
        {
        }
        
        public RefundResponse(NyaradzoAPI.RefundResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class RefundResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public NyaradzoAPI.NyaradzoResultModel RefundResult;
        
        public RefundResponseBody()
        {
        }
        
        public RefundResponseBody(NyaradzoAPI.NyaradzoResultModel RefundResult)
        {
            this.RefundResult = RefundResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AccountQueryRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AccountQuery", Namespace="http://tempuri.org/", Order=0)]
        public NyaradzoAPI.AccountQueryRequestBody Body;
        
        public AccountQueryRequest()
        {
        }
        
        public AccountQueryRequest(NyaradzoAPI.AccountQueryRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AccountQueryRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string AppKey;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string PolicyNumber;
        
        public AccountQueryRequestBody()
        {
        }
        
        public AccountQueryRequestBody(string AppKey, string PolicyNumber)
        {
            this.AppKey = AppKey;
            this.PolicyNumber = PolicyNumber;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AccountQueryResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AccountQueryResponse", Namespace="http://tempuri.org/", Order=0)]
        public NyaradzoAPI.AccountQueryResponseBody Body;
        
        public AccountQueryResponse()
        {
        }
        
        public AccountQueryResponse(NyaradzoAPI.AccountQueryResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AccountQueryResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public NyaradzoAPI.NyaradzoResultModel AccountQueryResult;
        
        public AccountQueryResponseBody()
        {
        }
        
        public AccountQueryResponseBody(NyaradzoAPI.NyaradzoResultModel AccountQueryResult)
        {
            this.AccountQueryResult = AccountQueryResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public interface NyaradzoSoapChannel : NyaradzoAPI.NyaradzoSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.2.0-preview1.23462.5")]
    public partial class NyaradzoSoapClient : System.ServiceModel.ClientBase<NyaradzoAPI.NyaradzoSoap>, NyaradzoAPI.NyaradzoSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public NyaradzoSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(NyaradzoSoapClient.GetBindingForEndpoint(endpointConfiguration), NyaradzoSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NyaradzoSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(NyaradzoSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NyaradzoSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(NyaradzoSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public NyaradzoSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NyaradzoAPI.ProcessPaymentResponse> NyaradzoAPI.NyaradzoSoap.ProcessPaymentAsync(NyaradzoAPI.ProcessPaymentRequest request)
        {
            return base.Channel.ProcessPaymentAsync(request);
        }
        
        public System.Threading.Tasks.Task<NyaradzoAPI.ProcessPaymentResponse> ProcessPaymentAsync(string AppKey, string PolicyNumber, string reference, decimal amount)
        {
            NyaradzoAPI.ProcessPaymentRequest inValue = new NyaradzoAPI.ProcessPaymentRequest();
            inValue.Body = new NyaradzoAPI.ProcessPaymentRequestBody();
            inValue.Body.AppKey = AppKey;
            inValue.Body.PolicyNumber = PolicyNumber;
            inValue.Body.reference = reference;
            inValue.Body.amount = amount;
            return ((NyaradzoAPI.NyaradzoSoap)(this)).ProcessPaymentAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NyaradzoAPI.RefundResponse> NyaradzoAPI.NyaradzoSoap.RefundAsync(NyaradzoAPI.RefundRequest request)
        {
            return base.Channel.RefundAsync(request);
        }
        
        public System.Threading.Tasks.Task<NyaradzoAPI.RefundResponse> RefundAsync(string AppKey, string reference)
        {
            NyaradzoAPI.RefundRequest inValue = new NyaradzoAPI.RefundRequest();
            inValue.Body = new NyaradzoAPI.RefundRequestBody();
            inValue.Body.AppKey = AppKey;
            inValue.Body.reference = reference;
            return ((NyaradzoAPI.NyaradzoSoap)(this)).RefundAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NyaradzoAPI.AccountQueryResponse> NyaradzoAPI.NyaradzoSoap.AccountQueryAsync(NyaradzoAPI.AccountQueryRequest request)
        {
            return base.Channel.AccountQueryAsync(request);
        }
        
        public System.Threading.Tasks.Task<NyaradzoAPI.AccountQueryResponse> AccountQueryAsync(string AppKey, string PolicyNumber)
        {
            NyaradzoAPI.AccountQueryRequest inValue = new NyaradzoAPI.AccountQueryRequest();
            inValue.Body = new NyaradzoAPI.AccountQueryRequestBody();
            inValue.Body.AppKey = AppKey;
            inValue.Body.PolicyNumber = PolicyNumber;
            return ((NyaradzoAPI.NyaradzoSoap)(this)).AccountQueryAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.NyaradzoSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.NyaradzoSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.NyaradzoSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://10.10.31.60:8017/Nyaradzo.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.NyaradzoSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://10.10.31.60:8017/Nyaradzo.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            NyaradzoSoap,
            
            NyaradzoSoap12,
        }
    }
}
