using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface
{
    [Serializable]
    public class HotAPIException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } 
        public HotAPIException(int StatusCode, string ExceptionMessage) : base(ExceptionMessage)
        {
            this.StatusCode = (HttpStatusCode)StatusCode; 
        }

        public static string GetMessage(string data)
        {
            var result = data;
            if (IsValidJson(data))
            {
                dynamic returned = JObject.Parse(data);
                try
                {
                    result = (returned.Message ??(returned.ReplyMessage ?? "")); 
                }
                catch 
                { 
                }
            } 
            return result;
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                } 
                catch 
                { 
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public HotAPIException()
        {
        }

        public HotAPIException(string message) : base(message)
        {
        }

        public HotAPIException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HotAPIException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
