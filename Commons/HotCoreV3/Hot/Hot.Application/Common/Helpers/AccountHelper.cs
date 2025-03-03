namespace Hot.Application.Common.Helpers
{
    public static class AccountHelper
    {
        public  static string RandomPin()
        {
            return Random.Shared.Next(1, 9999).ToString().PadLeft(4, '0');
        }
    }
}
