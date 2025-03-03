namespace Hot.Application.Common.Models.RechargeServiceModels.Telone;
public class TeloneRechargeResult : RechargeServiceResult
{

    public List<TeloneVoucher>? Vouchers { get; set; }
    public string ResponseCode { get; set; }
    public string Reference { get; set; }
}
