using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data.Context;
using RestApi.Data.Controller.Base;
using RestApi.Data.Models.Base;
using RestApi.Data.Models.Rest;
using RestApi.Data.Models.Translation;
using RestApi.Extensions;
using RestApi.Services;

namespace RestApi.Data.Controller;

public class HistoryController : BaseController<HistoryController>
{
    public HistoryController(DataContext context, ILogger<HistoryController> logger) : base(context, logger)
    {
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<BaseRestResult<List<HistoryModel>>>> ByUuid(Guid uuid,
        [FromQuery] string? lang = "en")
    {
        lang ??= "en";
        if (!await _context.CheckAgreement(uuid))
        {
            TranslationContent noAgreement =
                await TranslationService.LoadTranslation("controller.failed.noAgreement", lang);
            BaseRestResult<NameModel> errorModel = new BaseRestResult<NameModel>(true, noAgreement.Translation);
            return Ok(errorModel);
        }

        List<HistoryModel> histories = await _context.History.ToListAsync();
        List<HistoryModel> filteredHistory = histories.Where(it => it.Player.Uuid == uuid).ToList();
        if (filteredHistory.Count == 0)
        {
            TranslationContent noContent =
                await TranslationService.LoadTranslation("controller.failed.read", lang,
                    await TranslationService.LoadReason("database.noEntryFound", lang));
            BaseRestResult<List<HistoryModel>> errorModel =
                new BaseRestResult<List<HistoryModel>>(false, noContent.Translation, new List<HistoryModel>());
            return Ok(errorModel);
        }

        TranslationContent successRead = await TranslationService.LoadTranslation("controller.success.read", lang);
        BaseRestResult<List<HistoryModel>> result =
            new BaseRestResult<List<HistoryModel>>(false, successRead.Translation, filteredHistory);
        return Ok(result);
    }
}