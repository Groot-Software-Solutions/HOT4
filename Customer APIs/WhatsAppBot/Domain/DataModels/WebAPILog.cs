using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataModels
{
    public class WebAPILog
{
    public long Id { get; set; }

    public string Module { get; set; }

    public string Headers { get; set; }

    public string Method { get; set; }

    public string Body { get; set; }

    public string StatusCode { get; set; }

    public DateTime RequestTime { get; set; }

}
}
