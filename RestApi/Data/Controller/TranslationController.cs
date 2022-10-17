using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestApi.Data.Models.Rest;
using RestApi.Data.Models.Translation;

namespace RestApi.Data.Controller;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class TranslationController
{
    private readonly ILogger<TranslationController> _logger;

    public TranslationController(ILogger<TranslationController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<BaseRestResult<List<TranslationInfo>>> All()
    {
        List<Translation> translations = await LoadTranslations();
        List<TranslationInfo> infos = translations.Select(it => it.Info).ToList();
        return new BaseRestResult<List<TranslationInfo>>(false, "", infos);
    }

    [HttpGet("{identifier}")]
    public async Task<BaseRestResult<Translation>> ByIdentifier(string identifier)
    {
        IEnumerable<Translation> translations = await LoadTranslations();
        Translation? translation = translations.FirstOrDefault(it =>
            it.Info.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase));
        return translation is null
            ? new BaseRestResult<Translation>(true, "")
            : new BaseRestResult<Translation>(false, "", translation);
    }

    private async Task<List<Translation>> LoadTranslations()
    {
        string[] langFiles = Directory.GetFiles("Resources/Lang");
        List<Translation> langs = new List<Translation>();
        foreach (string file in langFiles)
        {
            await using Stream stream = File.OpenRead(file);
            Translation? translation = await JsonSerializer.DeserializeAsync<Translation>(stream);
            if (translation is null)
            {
                _logger.LogWarning("Failed to load Translation {File}", file);
                continue;
            }

            langs.Add(translation);
        }

        return langs;
    }
}