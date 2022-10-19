using RestApi.Data.Controller;
using RestApi.Data.Models.Translation;

namespace RestApi.Services;

public class TranslationService
{
    public static async Task<TranslationContent> LoadTranslation(string key, string lang)
    {
        List<Translation> translations = await TranslationController.LoadTranslations();
        Translation? translation = translations.FirstOrDefault(it =>
            it.Info.Identifier.Equals(lang, StringComparison.InvariantCultureIgnoreCase));
        TranslationContent? content =
            translation?.Content.FirstOrDefault(it => it.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));

        if (content is null)
            return new TranslationContent()
            {
                Translation = "No Translation found!"
            };
        return content;
    }
}