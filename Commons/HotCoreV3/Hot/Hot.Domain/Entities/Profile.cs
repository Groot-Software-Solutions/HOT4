using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hot.Domain.Entities
{
    public class Profile
    {
        [Key]
        [Display(Name = "Profile Id")]
        public int ProfileId { get; set; }

        [Required(ErrorMessage = "Profile Name is required")]
        [StringLength(50, ErrorMessage = "Profile Name may not exceed 50 characters")]
        [Display(Name = "Profile Name")]
        public string ProfileName { get; set; } = string.Empty;      
    }
}
