using Microsoft.AspNetCore.Mvc;
using RestApi.Data.Context;
using RestApi.Data.Controller.Base;
using RestApi.Data.Models.Base;
using RestApi.Data.Models.Rest;
using RestApi.Data.Models.Translation;
using RestApi.Extensions;
using RestApi.Services;

namespace RestApi.Data.Controller;

public class NameController : BaseController<NameController>
{
    public NameController(DataContext context, ILogger<NameController> logger) : base(context, logger)
    {
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<BaseRestResult<NameModel>>> ByUuid(Guid uuid, [FromQuery] string? lang = "en")
    {
        lang ??= "en";

        if (!await _context.CheckAgreement(uuid))
        {
            TranslationContent noAgreement =
                await TranslationService.LoadTranslation("controller.failed.noAgreement", lang);
            BaseRestResult<NameModel> errorModel = new BaseRestResult<NameModel>(true, noAgreement.Translation);
            return Ok(errorModel);
        }

        NameModel? name = await _context.Names.FindAsync(uuid);
        if (name is null)
        {
            TranslationContent noContent =
                await TranslationService.LoadTranslation("controller.failed.read", lang,
                    await TranslationService.LoadReason("database.noEntryFound", lang));
            BaseRestResult<NameModel> errorModel = new BaseRestResult<NameModel>(true, noContent.Translation);
            return Ok(errorModel);
        }

        TranslationContent successRead = await TranslationService.LoadTranslation("controller.success.read", lang);
        BaseRestResult<NameModel> result = new BaseRestResult<NameModel>(false, successRead.Translation, name);
        return Ok(result);
    }
}