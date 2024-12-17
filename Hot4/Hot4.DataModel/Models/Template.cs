using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class Template
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int TemplateId { get; set; }

    public required string TemplateName { get; set; }

    public required string TemplateText { get; set; }
}
