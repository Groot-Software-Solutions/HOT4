//using CoreWCF;  // CoreWCF namespace
//using CoreWCF.Channels;  // For messaging
//using System.Threading.Tasks;


//namespace Hot4.Service.Abstract
//{

//    [ServiceContract]
//    public class IServiceSoap
//    {

//        [OperationContract(Action = "http://tempuri.org/AfriBalanceEnquiry", ReplyAction = "*")]
//        AfriBalanceEnquiryResponse AfriBalanceEnquiry(AfriBalanceEnquiryRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriBalanceEnquiry", ReplyAction = "*")]
//        Task<AfriBalanceEnquiryResponse> AfriBalanceEnquiryAsync(AfriBalanceEnquiryRequest request);

//        [OperationContract(Action = "http://tempuri.org/Login", ReplyAction = "*")]
//        LoginResponse Login(LoginRequest request);

//        [OperationContract(Action = "http://tempuri.org/Login", ReplyAction = "*")]
//        Task<LoginResponse> LoginAsync(LoginRequest request);

//        [OperationContract(Action = "http://tempuri.org/AaaProvision", ReplyAction = "*")]
//        AaaProvisionResponse AaaProvision(AaaProvisionRequest request);

//        [OperationContract(Action = "http://tempuri.org/AaaProvision", ReplyAction = "*")]
//        Task<AaaProvisionResponse> AaaProvisionAsync(AaaProvisionRequest request);

//        [OperationContract(Action = "http://tempuri.org/AaaEdit", ReplyAction = "*")]
//        AaaEditResponse AaaEdit(AaaEditRequest request);

//        [OperationContract(Action = "http://tempuri.org/AaaEdit", ReplyAction = "*")]
//        Task<AaaEditResponse> AaaEditAsync(AaaEditRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriRecharge", ReplyAction = "*")]
//        AfriRechargeResponse AfriRecharge(AfriRechargeRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriRecharge", ReplyAction = "*")]
//        Task<AfriRechargeResponse> AfriRechargeAsync(AfriRechargeRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriTransfer", ReplyAction = "*")]
//        AfriTransferResponse AfriTransfer(AfriTransferRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriTransfer", ReplyAction = "*")]
//        Task<AfriTransferResponse> AfriTransferAsync(AfriTransferRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriEncrypt", ReplyAction = "*")]
//        AfriEncryptResponse AfriEncrypt(AfriEncryptRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriEncrypt", ReplyAction = "*")]
//        Task<AfriEncryptResponse> AfriEncryptAsync(AfriEncryptRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriProductPurchase", ReplyAction = "*")]
//        AfriProductPurchaseResponse AfriProductPurchase(AfriProductPurchaseRequest request);

//        [OperationContract(Action = "http://tempuri.org/AfriProductPurchase", ReplyAction = "*")]
//        Task<AfriProductPurchaseResponse> AfriProductPurchaseAsync(AfriProductPurchaseRequest request);

//        [OperationContract(Action = "http://tempuri.org/GurooRegisterFromDealer", ReplyAction = "*")]
//        GurooRegisterFromDealerResponse GurooRegisterFromDealer(GurooRegisterFromDealerRequest request);

//        [OperationContract(Action = "http://tempuri.org/GurooRegisterFromDealer", ReplyAction = "*")]
//        Task<GurooRegisterFromDealerResponse> GurooRegisterFromDealerAsync(GurooRegisterFromDealerRequest request);


//    }
//}
