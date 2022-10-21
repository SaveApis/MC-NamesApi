namespace RestApi.Data.Models.Rest;

public class HistoryModel
{
    public int Id { get; set; }
    public virtual NameModel Player { get; set; }
    public string HistoryName { get; set; } = string.Empty;
    public DateTime From { get; set; } = DateTime.UtcNow;
    public DateTime To { get; set; } = DateTime.UtcNow;
}