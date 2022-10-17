namespace RestApi.Data.Models.Translation;

/// <summary>
/// Specify the LanguageTranslation
/// </summary>
public class Translation
{
    public TranslationInfo Info { get; set; } = new();
    public IList<TranslationContent> Content { get; set; } = new List<TranslationContent>();
}