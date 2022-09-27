#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestApi.Data.Context;
using RestApi.Data.Models;
using RestApi.Data.Models.Examples.Agreement;
using RestApi.Interfaces;
using Swashbuckle.AspNetCore.Filters;

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
    /// <returns></returns>
    [HttpGet("{uuid:guid}")]
    [SwaggerResponseExample(200, typeof(GetPlayerExample))]
    public async Task<ActionResult<IRestResult<bool>>> GetPlayer(Guid uuid, [FromQuery] string? lang = "en")
    {
        AgreementModel? model = await _context.Agreements.FindAsync(uuid);
        if (model is null)
            return Ok(
                IRestResult<bool>.Create(true, $"No entry for the UUID ({uuid}) was found in the database."));
        return Ok(IRestResult<bool>.Create(false, $"Successfully found an entry for the UUID ({uuid})",
            model.AgreeValue));
    }

    /// <summary>
    /// Method to manage a player’s consent
    /// </summary>
    /// <param name="uuid">The player's UUID</param>
    /// <param name="agreed">True or false</param>
    /// <returns></returns>
    [HttpPost("{uuid:guid}/{agreed:bool}")]
    [SwaggerResponseExample(200, typeof(UpdatePlayerExample))]
    public async Task<ActionResult<IRestResult<bool>>> UpdatePlayer(Guid uuid, bool agreed, [FromQuery] string? lang = "en")
    {
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
            logModel = new LogModel()
            {
                Message =
                    $"Create AgreementEntry ({uuid} -> {agreed})",
                IpAddress = address
            };
            await _context.Agreements.AddAsync(existingModel);
            await _context.Logs.AddAsync(logModel);
            await _context.SaveChangesAsync();

            return Ok(IRestResult<bool>.Create(false, $"The entry for the UUID ({uuid}) was successfully created.",
                agreed));
        }

        if (agreed == existingModel.AgreeValue)
            return Ok(IRestResult<bool>.Create(false,
                "The specified value is the same as in the database. No changes will be made.", agreed));

        bool tmpAgree = existingModel.AgreeValue;
        existingModel.AgreeValue = agreed;
        _context.Agreements.Update(existingModel);
        logModel = new LogModel()
        {
            Message =
                $"Update AgreementEntry ({uuid} -> ({tmpAgree} -> {agreed}))",
            IpAddress = address
        };
        await _context.Logs.AddAsync(logModel);
        await _context.SaveChangesAsync();
        return Ok(IRestResult<bool>.Create(false, $"The entry for the UUID ({uuid}) has been successfully updated.",
            agreed));
    }
}