using TelOne.Application.Common.Exceptions;
using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models; 
using MediatR; 
using System; 
using System.Threading;
using System.Threading.Tasks;
using OneOf;

namespace TelOne.Application.Accounts
{
    public class VerifyCustomerAccountCommand : IRequest<OneOf<CustomerAccountResponse, APIException, string>>
    {
        public CustomerAccountRequest Item { get; set; }

        public VerifyCustomerAccountCommand(CustomerAccountRequest item)
        {
            Item = item;
        }

        public class VerifyCustomerAccountCommandHandler : IRequestHandler<VerifyCustomerAccountCommand, OneOf<CustomerAccountResponse, APIException, string>>
        {            
            private readonly IAPIService _apiService;

            public VerifyCustomerAccountCommandHandler(IAPIService apiService)
            {
                _apiService = apiService;
            }
            public async Task<OneOf<CustomerAccountResponse,APIException,string>> Handle(VerifyCustomerAccountCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    return await _apiService
                        .Post<CustomerAccountResponse, CustomerAccountRequest>
                        ("verifyBroadbandAccount", request.Item);
                    
                }
                catch (Exception ex)
                {
                    throw new APIException("Telone", ex.Message);
                }
            }
        }
    }
}
