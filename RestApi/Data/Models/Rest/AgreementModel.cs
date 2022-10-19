using System.ComponentModel.DataAnnotations;

namespace RestApi.Data.Models.Rest;

public class AgreementModel
{
    [Required] [Key] public Guid Uuid { get; set; }
    [Required] public bool AgreementValue { get; set; }
}