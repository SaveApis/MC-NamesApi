namespace RestApi.Data.Models.Translation;

/// <summary>
/// Model to specify the Language Info
/// </summary>
public class TranslationInfo
{
    /// <summary>
    /// Specify the name of the language
    /// </summary>
    public string DisplayName { get; set; } = "English";

    /// <summary>
    /// Specify the identifier of the language
    /// </summary>
    public string Identifier { get; set; } = "en";
}