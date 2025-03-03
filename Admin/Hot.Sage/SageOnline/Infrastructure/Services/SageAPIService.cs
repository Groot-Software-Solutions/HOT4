using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sage.Application.Common.Interfaces;
using Sage.Application.Common.Models;

namespace Sage.Infrastructure.Services
{
    

    public class SageAPIService : ISageAPIService
    {
        readonly IAPIService service;
        readonly IOptions<SageServiceOptions> options;

        public SageAPIService(IAPIService service, IOptions<SageServiceOptions> options)
        {
            this.service = service;
            this.service.APIName = "Sage";
            this.options = options;
        }
          
        public async Task<SageResponse<T>> Get<T>(string OrderBy = "", int Skip = 0, int Limit = 0)
        {
          
            var url = $"{typeof(T).Name}/Get?{(Skip == 0 ? "" : $"$skip={Skip}&")}{(Limit == 0 ? "" : $"$top={Limit}&")}{(string.IsNullOrEmpty(OrderBy) ? "" : $"$orderby={OrderBy}&")}apikey={options.Value.APIKey}&CompanyId={options.Value.APICompanyID}";
            var result = new SageResponse<T>();
            try
            {
               result = await service.APIGetCall<SageResponse<T>>(url); 
            }
            catch (Exception ex)
            {
                result.ErrorData = ex.Message;
                result.ValidResponse = false;
            } 
            return result;
        }
         
        public async Task<SageResponse<T>> Get<T>(int Id)
        {
            var url = $"{typeof(T).Name}/Get/{Id}?apikey={options.Value.APIKey}&CompanyId={options.Value.APICompanyID}";
            var result = new SageResponse<T>();
            try
            {
                var item = await service.APIGetCall<T>(url);
                result.Result = item;
            }
            catch(Exception ex)
            {
                result.ErrorData = ex.Message;
                result.ValidResponse = false;
            }
            return result;
        }
        public async Task<SageResponse<T>> Get<T>(long Id)
        {
            var url = $"{typeof(T).Name}/Get/{Id}?apikey={options.Value.APIKey}&CompanyId={options.Value.APICompanyID}";
            var result = new SageResponse<T>();
            try
            {
                var item = await service.APIGetCall<T>(url);
                result.Result = item;
            }
            catch (Exception ex)
            {
                result.ErrorData = ex.Message;
                result.ValidResponse = false;
            }
            return result;
        }

        public async Task<SageResponse<T>> GetAll<T>()
        {
            var result = await Get<T>(OrderBy: "ID");

            if (result.ValidResponse && (result.TotalResults != result.ReturnedResults))
            {
                for (int x = 100; x <= result.TotalResults; x += 100)
                {
                    var fetchMore = await Get<T>(OrderBy:"ID",Skip: x);
                    if (fetchMore.ValidResponse)
                        result.Results.AddRange(fetchMore.Results);
                }
                result.ReturnedResults = result.Results.Count();
            }
            return result;
        }

        public async Task<SageResponse<T>> Save<T>(T item)
        {
            var url = $"{typeof(T).Name}/Save?apikey={options.Value.APIKey}&CompanyId={options.Value.APICompanyID}";

            var result = new SageResponse<T>();
            try
            {
                result = await service.APIPostCall<SageResponse<T>, T>(url, item);
            }
            catch (Exception ex)
            {
                result.ErrorData = ex.Message + $"-{url}-{JsonSerializer.Serialize(item)}";
                result.ValidResponse = false;
            }
            return result;
             
        }

        public async Task<SageResponse<T>> SaveSingle<T>(T item)
        {
            var url = $"{typeof(T).Name}/Save?apikey={options.Value.APIKey}&CompanyId={options.Value.APICompanyID}";
             
            var result = new SageResponse<T>();
            try
            {
                result.Result = await service.APIPostCall<T, T>(url, item);
            }
            catch (Exception ex)
            {
                result.ErrorData = ex.Message+$"-{url}-{JsonSerializer.Serialize(item)}";
                result.ValidResponse = false;
            }
            return result;  
        }


        public async Task<SageResponse<T>> SaveBatch<T>(List<T> list)
        {
            var url = $"{typeof(T).Name}/SaveBatch?apikey={options.Value.APIKey}&CompanyId={options.Value.APICompanyID}";

            var result = new SageResponse<T>();
            try
            {
                var data = await service.APIPostCall<List<T>, List<T>>(url, list);
                result.Results = data;
            }
            catch (Exception ex)
            {
                result.ErrorData = ex.Message + $"-{url}-{JsonSerializer.Serialize(list)}";
                result.ValidResponse = false;
            }
            return result;
        }

        public async Task<SageResponse<T>> Delete<T>(int Id)
        {
            var url = $"{typeof(T).Name}/Delete/{Id}?apikey={options.Value.APIKey}&CompanyId={options.Value.APICompanyID}";
            var result = new SageResponse<T>();
            try
            {
                var item = await service.APIDeleteCall<T>(url);
                result.Result = item;
            }
            catch (Exception ex)
            {
                result.ErrorData = ex.Message + $"-{url}-{Id}";
                result.ValidResponse = false;
            }
            return result;
        }

        public async Task<SageResponse<U>> Save<T, U>(T item)
        {
            var url = $"{typeof(U).Name}/Save?apikey={options.Value.APIKey}&CompanyId={options.Value.APICompanyID}";

            var result = new SageResponse<U>();
            try
            {
                result = await service.APIPostCall< SageResponse<U>,T>(url, item);
            }
            catch (Exception ex)
            {
                result.ErrorData = ex.Message + $"-{url}-{JsonSerializer.Serialize(item)}";
                result.ValidResponse = false;
            }
            return result;
        }
    }
}
