namespace Hot.Ecocash.Lesotho.Models;

public class QueryTransactionResponseLesothoModel
{
    public int ID { get; set; }
    public string TXNID { get; set; } = string.Empty;
    public string TXNSTATUS { get; set; } = string.Empty;
    public string MESSAGE { get; set; } = string.Empty;
    public string EXTTRANSACTIONID { get; set; } = string.Empty;
    public string TRID { get; set; } = string.Empty;
}
