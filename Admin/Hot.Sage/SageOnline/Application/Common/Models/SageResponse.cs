using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Application.Common.Models
{
    public class SageResponse<T>
    {
        public int TotalResults { get; set; }
        public int ReturnedResults { get; set; }
        public List<T> Results { get; set; } 
        public T Result { get; set; }

        public string ErrorData { get; set; }
        public bool ValidResponse { get; set; } = true;
    }
}
