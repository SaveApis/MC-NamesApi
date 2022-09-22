using System.ComponentModel.DataAnnotations;

namespace McNamesApi.Data.Models;

public class PlayerAgreement
{
    public int Id { get; set; }
    [Required] public virtual Guid Uuid { get; set; }

    [Required] public bool Agreed { get; set; } = false;
}

public class PlayerAgreementUpdateDto
{
    public Guid Uuid { get; set; }
    public bool AgreeValue { get; set; }
}