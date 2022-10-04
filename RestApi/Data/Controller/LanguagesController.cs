using Microsoft.AspNetCore.Mvc;
using RestApi.Data.Models.Language;
using RestApi.Interfaces;
using RestApi.Services;

namespace RestApi.Data.Controller;

/// <summary>
/// Controller to list all available Languages
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LanguagesController : ControllerBase
{
    /// <summary>
    /// Method to list all LanguageInfo's
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IRestResult<IEnumerable<TranslationInfo>>>> All()
    {
        IEnumerable<TranslationInfo> langInfos = (await TranslationService.AllTranslations()).Select(it => it.Info).ToList();
        return Ok(IRestResult.Create(false, "", langInfos));
    }

    /// <summary>
    /// Method to list all Translations with this identifier
    /// </summary>
    /// <param name="identifier">The given Identifier</param>
    /// <returns></returns>
    [HttpGet("{identifier}")]
    public async Task<ActionResult<IRestResult<Translation>>> ByIdentifier(string identifier)
    {
        IEnumerable<TranslationFileModel> models = await TranslationService.AllTranslations();
        TranslationFileModel? model = models.FirstOrDefault(it =>
            it.Info.Identifier.Equals(identifier, StringComparison.InvariantCultureIgnoreCase));
        return Ok(model is null
            ? IRestResult.Create<Translation>(true, "")
            : IRestResult.Create(false, "", model.Translations));
    }
}