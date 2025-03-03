namespace Hot.Application.Common.Exceptions;

public class UnsupportedBrandException : Exception
{
    public UnsupportedBrandException(string name, object data)
     : base($"Unsupported Brand Exception - \"{name}\" {data}")
    {
    }
    public UnsupportedBrandException(string data, Exception error)
    : base($"Unsupported Brand Exception - \"{data}\" - {error.Message}", error)
    {
    }

    public UnsupportedBrandException(int? brandId, string? brandName, IDbContext dbContext)
: base($"Unsupported Brand Exception - {brandId}")
    {
        BrandId = brandId;
        BrandName = brandName;
        var template = TemplateExtensions.GetTemplate(dbContext, Domain.Enums.Templates.FailedRechargeVASDisabled);
        if (template != null) template.SetMessage("").SetBrand(BrandName ?? (brandId ?? 0).ToString());
        ResponseMessage = template?.TemplateText ?? "";
    }

    public int? BrandId { get; set; }
    public string? BrandName { get; set; }
    public string? ResponseMessage { get; set; }

}