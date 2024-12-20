using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.ViewModel.ApiModels
{
    public class BrandIdentitySearchModel
    {
        public byte NetworkId { get; set; }
        [Column(TypeName = "char"), StringLength(1)]
        public string BrandSuffix { get; set; }
    }
}
