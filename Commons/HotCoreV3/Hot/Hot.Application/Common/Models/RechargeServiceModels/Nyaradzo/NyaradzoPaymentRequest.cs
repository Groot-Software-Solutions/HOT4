namespace Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;

public record NyaradzoPaymentRequest(string PolicyNumber, string Reference, decimal AmountPaid, decimal MonthlyPremium, int NumberOfMonthsPaid, DateTime Date);