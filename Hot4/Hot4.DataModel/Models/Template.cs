using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Template
{
    [Key]
    public int TemplateId { get; set; }

    public required string TemplateName { get; set; }

    public required string TemplateText { get; set; }
}
