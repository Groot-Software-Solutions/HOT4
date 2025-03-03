namespace Hot.Application.Common.Models.RechargeServiceModels.NetOne;
public record NetOneRechargeRequest(string Mobile, decimal Amount, string Reference, DateTime Date);
