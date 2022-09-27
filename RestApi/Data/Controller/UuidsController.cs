using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data.Context;
using RestApi.Data.Models;
using RestApi.Extensions;
using RestApi.Interfaces;

namespace RestApi.Data.Controller;

/// <summary>
/// Controller to determine the current UUID’s.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class UuidsController : ControllerBase
{
    private readonly DataContext _context;

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="context"></param>
    public UuidsController(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Determines the UUID based on the current name.
    /// </summary>
    /// <param name="name">The current name of the Player</param>
    /// <returns></returns>
    [HttpGet("[action]/{name}")]
    public async Task<ActionResult<IRestResult<Guid>>> ByCurrentName(string name, [FromQuery] string? lang = "en")
    {
        PlayerNameModel? nameModel =
            await _context.Names.FirstOrDefaultAsync(it => it.Name.ToLower() == name.ToLower());

        if (nameModel is null)
            return Ok(IRestResult<Guid>.Create(true, "Der Spieler wurde nicht gefunden."));

        if (!await _context.CheckAgreement(nameModel.Uuid))
            return Ok(IRestResult<Guid>.Create(true,
                "Der Spieler hat die Einverständniserklärung nicht akzeptiert. Solange die Erklärung nicht akzeptiert sind, kann man nicht auf die Informationen zugreifen!"));

        return Ok(IRestResult<Guid>.Create(false, "Die Uuid wurde erfolgreich ausgelesen.", nameModel.Uuid));
    }

    /// <summary>
    /// Determines the UUID based on a past name.
    /// </summary>
    /// <param name="historyName">A past name of the Player</param>
    /// <returns></returns>
    [HttpGet("[action]/{historyName}")]
    public async Task<ActionResult<IRestResult<Guid>>> ByHistoryName(string historyName, [FromQuery] string? lang = "en")
    {
        PlayerNameHistoryModel? historyModel =
            await _context.Histories.FirstOrDefaultAsync(it => it.Name.ToLower() == historyName.ToLower());

        if (historyModel is null)
            return Ok(IRestResult<Guid>.Create(true, "Der Spieler wurde nicht gefunden."));

        if (!await _context.CheckAgreement(historyModel.Player.Uuid))
            return Ok(IRestResult<Guid>.Create(true,
                "Der Spieler hat die Einverständniserklärung nicht akzeptiert. Solange die Erklärung nicht akzeptiert sind, kann man nicht auf die Informationen zugreifen!"));

        return Ok(IRestResult<Guid>.Create(false, "Die Uuid wurde erfolgreich ausgelesen.", historyModel.Player.Uuid));
    }
}