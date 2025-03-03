using System.Threading.Tasks;

namespace BillPayments.Application.Services
{
    public interface IBackgroundPaymentService
    {
        Task Run();
    }
}