using Hot.Ecocash.Application.Common.Exceptions;
using Hot.Ecocash.Domain.Entities;
using Hot.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Hot.Ecocash.Application.Common
{
    public class EcocashResult
    {
         
        public EcocashResult(List<Transaction> list)
        {
            ValidResponse = true;
            List = list;
        }

        public EcocashResult(APIException error)
        {
            ErrorData = "Ecocash Platform Error";
            ResponseData = error.Message;
            ValidResponse = false;
        }

        public EcocashResult(string data)
        {
            try
            {
                var error = JsonSerializer.Deserialize<EcoCashAppException>(data); 
                ErrorData = error.text;
                ResponseData = data;
                ValidResponse = false; 
            }
            catch (Exception)
            { 
                ErrorData = "Ecocash Platform Error";
                ResponseData = data;
                ValidResponse = false; 
            }
        }

        public EcocashResult(Transaction item)
        {
            ValidResponse = true;
            Item = item;
        }

        public bool ValidResponse { get; set; } = false;
        public string ResponseData { get; set; } = "";
        public string ErrorData { get; set; } = "";
        public Transaction Item { get; set; }
        public List<Transaction> List { get; set; }
    }
}
