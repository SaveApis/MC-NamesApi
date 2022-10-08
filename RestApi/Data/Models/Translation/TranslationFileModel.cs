#nullable disable
namespace RestApi.Data.Models.Translation;

/// <summary>
/// Specify the format of the language model (Used in API and Lang-File)
/// </summary>
public class TranslationFileModel
{
    /// <summary>
    /// Specify the Info
    /// </summary>
    public TranslationInfo Info { get; set; }

    /// <summary>
    /// Specify the Translations
    /// </summary>
    public Translation Translations { get; set; }
}