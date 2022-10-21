using RestApi.Data.Controller;
using RestApi.Data.Models.Translation;

namespace RestApi.Services;

public class TranslationService
{
    public static async Task<TranslationContent> LoadTranslation(string key, string lang,
        params TranslationContent[] replacements)
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

        object[] replaceStrings = replacements.Select(it => (object) it.Translation).ToArray();
        content.Translation = string.Format(content.Translation, replaceStrings);
        return content;
    }

    public static async Task<TranslationContent> LoadReason(string key, string lang)
    {
        return await LoadTranslation("reason." + key, lang);
    }
}