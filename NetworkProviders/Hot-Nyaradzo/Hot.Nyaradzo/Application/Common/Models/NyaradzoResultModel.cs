using AutoMapper;
using Hot.Application.Common.Exceptions; 
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo; 
using Hot.Nyaradzo.Application.Common.Exceptions;
using Hot.Nyaradzo.Domain.Entities;
using System.Text.Json;

namespace Hot.Nyaradzo.Application.Common.Models;

public class NyaradzoResultModel
{
    public bool ValidResponse { get; set; } = false;
    public string ResponseData { get; set; } = "";
    public string ErrorData { get; set; } = "";
    public AccountSummary Item { get; set; } = new();
    public TransactionResult Result { get; set; } = new();


    public NyaradzoResultModel(APIException error)
    {
        ErrorData = "Nyaradzo Platform Error";
        ResponseData = error.Message;
        ValidResponse = false;
    }

    public NyaradzoResultModel(AccountSummary item)
    {
        ResponseData = JsonSerializer.Serialize(item);
        ValidResponse = true;
        Item = item;

    }

    public NyaradzoResultModel(ReversalResult item)
    {
        ResponseData = JsonSerializer.Serialize(item);
        ValidResponse = true;
        Result = item;
    }

    public NyaradzoResultModel(PaymentResult item)
    {
        ResponseData = JsonSerializer.Serialize(item);
        ValidResponse = true;
        Result = item;
    }

    public NyaradzoResultModel(string data)
    {
        try
        {
            var error = JsonSerializer.Deserialize<NyaradzoException>(data);
            ErrorData = error?.text ?? "";
            ResponseData = data;
            ValidResponse = false;
        }
        catch (Exception)
        {
            ErrorData = "Nyaradzo Platform Error";
            ResponseData = data;
            ValidResponse = false;
        }
    }
 

}
