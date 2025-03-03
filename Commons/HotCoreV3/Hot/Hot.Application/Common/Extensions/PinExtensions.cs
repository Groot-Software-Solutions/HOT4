using System.Text;

namespace Hot.Application.Common.Extensions;
public static class PinExtensions
{
   
    public static string ConvertByteArrayToString(byte[] byteArray)
    {
        return Encoding.ASCII.GetString(byteArray);
    }

    public static byte[] ConvertStringToByteArray(string byteString)
    {
        return Encoding.ASCII.GetBytes(byteString);
    }

    public static string DecryptPin(this string pin)
    {
        if (pin.Length < 20) return pin;

        return decryptedPin(pin);
    }

    private static string decryptedPin(string pin)
    {
        
        throw new NotImplementedException();
    }
}
