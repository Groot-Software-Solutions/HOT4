using MediatR;
using Sage.Application.Common.Exceptions;
using Sage.Application.Common.Interfaces;
using Sage.Domain.Entities; 

namespace Sage.Application.Entities.Companys;

public class CompanyGetAllQuery : IRequest<List<Company>>
{
    public class CompanyGetAllQueryHandler : IRequestHandler<CompanyGetAllQuery, List<Company>>
    {
        readonly ISageAPIService service;

        public CompanyGetAllQueryHandler(ISageAPIService service)
        {
            this.service = service;
        }

        public async Task<List<Company>> Handle(CompanyGetAllQuery request, CancellationToken cancellationToken)
        {
            var result = await service.GetAll<Company>();
            if (result.ValidResponse) return result.Results;
            throw new SageAPIException("Sage-Company-GetAll", result.ErrorData);
        }
    }
}
