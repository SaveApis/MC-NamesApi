using System.ComponentModel.DataAnnotations;

namespace RestApi.Data.Models;

public class PlayerNameHistoryModel
{
    public int Id { get; set; }
    [Required] public PlayerNameModel Player { get; set; }

    [Required] public string Name { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "dd.MM.yyyy")]
    public DateTime From { get; set; }

    [Required]
    [DisplayFormat(DataFormatString = "dd.MM.yyyy")]
    public DateTime To { get; set; }
}