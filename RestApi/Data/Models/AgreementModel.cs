#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace RestApi.Data.Models;

public class AgreementModel
{
    [Key] [Required] public Guid Uuid { get; set; }

    [Required] public bool AgreeValue { get; set; } = false;
}