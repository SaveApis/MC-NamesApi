using Microsoft.AspNetCore.Mvc;
using RestApi.Data.Context;
using RestApi.Data.Controller.Base;
using RestApi.Data.Models.Base;
using RestApi.Data.Models.Rest;
using RestApi.Data.Models.Translation;
using RestApi.Services;

namespace RestApi.Data.Controller;

public class AgreementController : BaseController<AgreementController>
{
    public AgreementController(DataContext context, ILogger<AgreementController> logger) : base(context, logger)
    {
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<BaseRestResult<AgreementModel>>> HasAgreement(Guid uuid,
        [FromQuery] string? lang = "en")
    {
        lang ??= "en";
        TranslationContent successRead =
            await TranslationService.LoadTranslation("controller.success.read", lang);
        TranslationContent successCreate =
            await TranslationService.LoadTranslation("controller.success.create", lang);
        AgreementModel? model = await _context.Agreements.FindAsync(uuid);
        if (model is not null)
            return Ok(new BaseRestResult<AgreementModel>(false, successRead.Translation, model));
        model = new AgreementModel
        {
            Uuid = uuid,
            AgreementValue = false
        };
        await _context.Agreements.AddAsync(model);
        await _context.SaveChangesAsync();
        return Ok(new BaseRestResult<AgreementModel>(false, successCreate.Translation, model));
    }

    [HttpPost("{uuid:guid}")]
    public async Task<ActionResult<BaseRestResult<AgreementModel>>> ToggleAgreement(Guid uuid,
        [FromQuery] string? lang = "en")
    {
        lang ??= "en";
        TranslationContent successUpdate =
            await TranslationService.LoadTranslation("controller.success.update", lang);
        TranslationContent successCreate =
            await TranslationService.LoadTranslation("controller.success.create", lang);

        AgreementModel? model = await _context.Agreements.FindAsync(uuid);
        if (model is not null)
        {
            model.AgreementValue = !model.AgreementValue;
            _context.Update(model);
            await _context.SaveChangesAsync();
            return Ok(new BaseRestResult<AgreementModel>(false, successUpdate.Translation, model));
        }

        model = new AgreementModel
        {
            Uuid = uuid,
            AgreementValue = false
        };
        await _context.Agreements.AddAsync(model);
        await _context.SaveChangesAsync();
        return Ok(new BaseRestResult<AgreementModel>(false, successCreate.Translation, model));
    }
}