using System.Text.Json;
using RestApi.Data.Models.Translation;

namespace RestApi.Services;

/// <summary>
/// Service-Class for translations
/// </summary>
public static class TranslationService
{
    /// <summary>
    /// Method to get all translations
    /// </summary>
    /// <returns></returns>
    public static async Task<IEnumerable<TranslationFileModel>> AllTranslations()
    {
        List<TranslationFileModel> languageModel = new();
        string[] files = Directory.GetFiles("Resources/Lang");

        foreach (string file in files)
        {
            await using Stream stream = File.OpenRead(file);
            TranslationFileModel? model = await JsonSerializer.DeserializeAsync<TranslationFileModel>(stream);
            stream.Close();
            if (model is not null)
            {
                languageModel.Add(model);
                await SaveTranslation(model);
            }
        }

        return languageModel;
    }

    /// <summary>
    /// Method to get a specific language based on the identifier
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public static async Task<TranslationFileModel?> ByIdentifier(string identifier)
    {
        IEnumerable<TranslationFileModel> models = await AllTranslations();
        return models.FirstOrDefault(it =>
            it.Info.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Method to save the given translation
    /// </summary>
    /// <param name="model">A translation</param>
    private static async Task SaveTranslation(TranslationFileModel model)
    {
        string file = $"Resources/Lang/{model.Info.Identifier}.json";
        if (File.Exists(file))
            File.Delete(file);
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
        };
        await using Stream stream = File.Create(file);
        await JsonSerializer.SerializeAsync(stream, model, options);
    }
}