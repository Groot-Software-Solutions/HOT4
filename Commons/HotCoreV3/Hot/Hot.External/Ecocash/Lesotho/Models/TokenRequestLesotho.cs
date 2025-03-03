namespace Hot.Ecocash.Lesotho.Models;

public class TokenRequestLesotho
{
    public string grant_type { get; set; } = "password";
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;

}

public class TokenResultLesotho
{
    public string access_token { get; set; } = string.Empty;
    public string token_type { get; set; } = string.Empty;
    public int expires_in { get; set; }
    public string userName { get; set; } = string.Empty;
    public string issued { get; set; } = string.Empty;
    public string expires { get; set; } = string.Empty;
}
