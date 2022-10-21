using System.ComponentModel.DataAnnotations;

namespace RestApi.Data.Models.Rest;

public class NameModel
{
    [Key] public Guid Uuid { get; set; }

    [Required] public string Name { get; set; } = string.Empty;

    [Required] [DataType(DataType.Date)] public DateTime Since { get; set; } = DateTime.UtcNow;
}