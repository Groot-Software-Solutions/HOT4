namespace Hot.Application.Common.Interfaces;

public interface IRechargeMobileQueryHandlerFactory
{
    public bool HasService(int BrandId);
    public IRechargeMobileQueryHandler GetService(int BrandId);
}