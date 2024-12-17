using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class Configs
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte ConfigId { get; set; }

    public int ProfileIdNewSmsdealer { get; set; }

    public int ProfileIdNewWebDealer { get; set; }

    public decimal MinRecharge { get; set; }

    public decimal MaxRecharge { get; set; }

    public bool PrepaidEnabled { get; set; }

    public decimal MinTransfer { get; set; }
}
