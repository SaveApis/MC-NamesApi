using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestApi.Data.Models.Base;
using RestApi.Data.Models.Translation;

namespace RestApi.Data.Controller;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class TranslationController : ControllerBase
{
    private readonly ILogger<TranslationController> _logger;

    public TranslationController(ILogger<TranslationController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<BaseRestResult<List<TranslationInfo>>>> All()
    {
        List<Translation> translations = await LoadTranslations();
        List<TranslationInfo> infos = translations.Select(it => it.Info).ToList();
        return Ok(new BaseRestResult<List<TranslationInfo>>(false,
            $"Loaded {(infos.Count is not 1 ? $"{infos.Count} translations" : "one translation")}!", infos));
    }

    [HttpGet("{identifier}")]
    public async Task<ActionResult<BaseRestResult<Translation>>> ByIdentifier(string identifier)
    {
        IEnumerable<Translation> translations = await LoadTranslations();
        Translation? translation = translations.FirstOrDefault(it =>
            it.Info.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase));
        return Ok(translation is null
            ? new BaseRestResult<Translation>(true, "")
            : new BaseRestResult<Translation>(false, "", translation));
    }

    public static async Task<List<Translation>> LoadTranslations()
    {
        string[] langFiles = Directory.GetFiles("Resources/Lang");
        List<Translation> langs = new List<Translation>();
        foreach (string file in langFiles)
        {
            await using Stream stream = System.IO.File.OpenRead(file);
            Translation? translation = await JsonSerializer.DeserializeAsync<Translation>(stream);
            if (translation is null)
            {
                continue;
            }

            langs.Add(translation);
        }

        return langs;
    }
}