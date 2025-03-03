namespace Hot.Application.Common.Exceptions
{
    public class NotAllowedToSellBrandException : Exception
    {
        public NotAllowedToSellBrandException(string name, object data)
         : base($"Not Allowed To Sell Exception \"{name}\" {data}")
        {
            Data.Add("ObjectData", data);
            Data.Add("Message", name);
        }
        public NotAllowedToSellBrandException(string data, Exception error)
     : base($"Not Allowed To Sell Exception - \"{data}\" - {error.Message}", error)
        {
        }

        public NotAllowedToSellBrandException(int? brandId, decimal? accountId, string? accountName, string? brandName, Exception error, Interfaces.IDbContext dbContext)
            : base("Not Allowed to sell Exception", error)
        {
            BrandId = brandId;
            AccountId = accountId;
            AccountName = accountName;
            BrandName = brandName;
            var template = TemplateExtensions.GetTemplate(dbContext, Domain.Enums.Templates.FailedRechargeVASDisabled);
            if (template != null) template.SetBrand(brandName ?? "").SetAccountName(accountName ?? "");
            ResponseMessage = template?.TemplateText ?? "";
        }

        public int? BrandId { get; set; }
        public decimal? AccountId { get; set; }
        public string? AccountName { get; set; }

        public string? BrandName { get; set; }
        public string? ResponseMessage { get; init; }
    }
}
