using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Ecocash.Application.Common.Extensions;
using Hot.Ecocash.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Hot.Ecocash.Application.Actions
{
    public record SendEcocashConfirmationToAPIClient(Transaction Transaction, Account Account) : IRequest<bool>;
    public class SendEcocashConfirmationToAPIClientHandler : IRequestHandler<SendEcocashConfirmationToAPIClient, bool>
    {
        private readonly IAPIService apiService;
        private readonly ILogger<SendEcocashConfirmationToAPIClientHandler> logger;

        public SendEcocashConfirmationToAPIClientHandler(IAPIService apiService, ILogger<SendEcocashConfirmationToAPIClientHandler> logger)
        {
            this.apiService = apiService;
            this.logger = logger;
        }

        public async Task<bool> Handle(SendEcocashConfirmationToAPIClient request, CancellationToken cancellationToken)
        {
            apiService.APIName = "";
            var anonymizedTransaction = request.Transaction.GetAnonymizedTransaction();
            var url = request.Account.Email ?? "";

            if (url.StartsWith("http") == false) return false;
            var jsonData = JsonSerializer.Serialize(anonymizedTransaction);

            var response = await apiService.Post(url, anonymizedTransaction);

            var result = response.Match(
                 item => true,
                 error => { logger.LogInformation("", error.Message); return false; }
                 );

            return result;
        }

    }


}