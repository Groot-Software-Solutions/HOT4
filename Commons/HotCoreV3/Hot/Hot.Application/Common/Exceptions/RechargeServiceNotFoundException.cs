namespace Hot.Application.Common.Exceptions
{
    public class RechargeServiceNotFoundException:Exception
    {
        public RechargeServiceNotFoundException(string name, object data)
           : base($"Recharge Service Not Found Exception \"{name}\" {data}")
        {
        }
    }

}
