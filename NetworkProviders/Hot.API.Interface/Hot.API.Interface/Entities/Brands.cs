using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Entities
{
    public class Brand
    {
        public enum Brands
        {
            Buddie = 1,
            Text = 2,
            EasyCall = 3,
            Juice = 4,
            EconetPlatform = 5,
            Prepaid = 5,
            Econet078 = 6,
            EconetBB = 7,
            EconetTXT = 8,
            TelecelPyros = 10,
            TelecelTXT = 11,
            TelecelBB = 12,
            Africom = 16,
            EasyCallEVD = 17,
            OneFusion = 18,
            EconetDataBundle = 19,
            EconetWhatsappBundle = 20,
            EconetFacebookBundle = 21,
            EconetWifiBundles = 22,
            CPE = 23,

        }
        public byte BrandId { get; set; }
        public byte NetworkId { get; set; }
        public string BrandName { get; set; }
        public string BrandSuffix { get; set; }
    }
}
