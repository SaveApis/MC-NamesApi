namespace RestApi.Data.Models.Translation;

/// <summary>
/// Model to specify the Language Info
/// </summary>
public class TranslationInfo
{
    /// <summary>
    /// Specify the Name of the Language
    /// </summary>
    public string Display { get; set; } = "English";

    /// <summary>
    /// Specify the Identifier of the Language
    /// </summary>
    public string Identifier { get; set; } = "en";

    /// <summary>
    /// Specify the Identifier <br />
    /// Increment with all Changes
    /// </summary>
    public int Version { get; set; } = 1;
}