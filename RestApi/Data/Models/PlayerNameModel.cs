using System.ComponentModel.DataAnnotations;

namespace RestApi.Data.Models;

public class PlayerNameModel
{
    [Key]
    [Required] 
    public Guid Uuid { get; set; }

    [Required] public string Name { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "dd-MM-yyyy")]
    public DateTime Since { get; set; }
}