namespace RestApi.Data.Models.Translation;

/// <summary>
/// Model to specify the Language Info
/// </summary>
public class TranslationInfo
{
    /// <summary>
    /// Specify the Name of the Language
    /// </summary>
    public string DisplayName { get; set; } = "English";

    /// <summary>
    /// Specify the Identifier of the Language
    /// </summary>
    public string Identifier { get; set; } = "en";
}