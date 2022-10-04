#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestApi.Data.Context;
using RestApi.Data.Models.Db;
using RestApi.Data.Models.Language;
using RestApi.Interfaces;
using RestApi.Services;

#endregion

namespace RestApi.Data.Controller;

/// <summary>
/// Controllers to manage the consent of the players.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AgreementController : ControllerBase
{
    private readonly DataContext _context;

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="context"></param>
    public AgreementController(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Method to read a player’s consent.
    /// </summary>
    /// <param name="uuid">The player’s UUID</param>
    /// <param name="lang">The identifier of the Response-Language</param>
    /// <returns></returns>
    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<IRestResult<bool>>> GetPlayer(Guid uuid, [FromQuery] string lang = "en")
    {
        TranslationFileModel? langModel = await TranslationService.ByIdentifier(lang);
        AgreementModel? model = await _context.Agreements.FindAsync(uuid);
        if (model is null)
            return Ok(
                IRestResult.Create<bool>(true,
                    langModel?.Translations.ControllerAgreementNoEntry ??
                    $"No entry for the UUID ({uuid}) was found in the database."));
        return Ok(
            IRestResult.Create(false,
                langModel?.Translations.ControllerAgreementEntryFound ??
                $"Successfully found an entry for the UUID ({uuid})",
                model.AgreeValue));
    }

    /// <summary>
    /// Method to manage a player’s consent
    /// </summary>
    /// <param name="uuid">The player's UUID</param>
    /// <param name="agreed">True or false</param>
    /// <param name="lang">The identifier of the Response-Language</param>
    /// <returns></returns>
    [HttpPost("{uuid:guid}/{agreed:bool}")]
    public async Task<ActionResult<IRestResult<bool>>> UpdatePlayer(Guid uuid, bool agreed,
        [FromQuery] string lang = "en")
    {
        TranslationFileModel? langModel = await TranslationService.ByIdentifier(lang);
        LogModel logModel;
        IPAddress? address = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4();
        AgreementModel? existingModel = await _context.Agreements.FindAsync(uuid);
        if (existingModel is null)
        {
            existingModel = new AgreementModel()
            {
                Uuid = uuid,
                AgreeValue = agreed
            };
            logModel = new LogModel
            {
                Message =
                    $"Create AgreementEntry ({uuid} -> {agreed})",
                IpAddress = address
            };
            await _context.Agreements.AddAsync(existingModel);
            await _context.Logs.AddAsync(logModel);
            await _context.SaveChangesAsync();

            return Ok(IRestResult.Create(
                false,
                $"The entry for the UUID ({uuid}) was successfully created.",
                agreed));
        }

        if (agreed == existingModel.AgreeValue)
            return Ok(IRestResult.Create(false,
                langModel?.Translations.ControllerAgreementNoChanges ??
                "The specified value is the same as in the database. No changes will be made.",
                agreed));

        bool tmpAgree = existingModel.AgreeValue;
        existingModel.AgreeValue = agreed;
        _context.Agreements.Update(existingModel);
        logModel = new LogModel
        {
            Message =
                $"Update AgreementEntry ({uuid} -> ({tmpAgree} -> {agreed}))",
            IpAddress = address
        };
        await _context.Logs.AddAsync(logModel);
        await _context.SaveChangesAsync();
        return Ok(IRestResult.Create(false, $"The entry for the UUID ({uuid}) has been successfully updated.",
            agreed));
    }
}