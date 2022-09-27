using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data.Context;
using RestApi.Data.Models;
using RestApi.Extensions;
using RestApi.Interfaces;

namespace RestApi.Data.Controller;

/// <summary>
/// Controller to manage the current name of a player within the database.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class NamesController : ControllerBase
{
    private readonly DataContext _context;

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="context"></param>
    public NamesController(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Method to determine the current name from the UUID.
    /// </summary>
    /// <param name="uuid">The UUID of the Player</param>
    /// <returns></returns>
    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<IRestResult<PlayerNameModel>>> ByUuid(Guid uuid, [FromQuery] string? lang = "en")
    {
        if (!await _context.CheckAgreement(uuid))
            return Ok(IRestResult<PlayerNameModel>.Create(true,
                "Der Spieler hat die Einverständniserklärung nicht akzeptiert. Solange die Erklärung nicht akzeptiert sind, kann man nicht auf die Informationen zugreifen!"));
        PlayerNameModel? model = await _context.Names.FindAsync(uuid);
        return Ok(model is null
            ? IRestResult<PlayerNameModel>.Create(true, "Der Spieler konnte nicht gefunden werden.")
            : IRestResult<PlayerNameModel>.Create(false, "Der Name wurde erfolgreich ausgelesen.", model));
    }

    /// <summary>
    /// Method to determine the current name from a past name.
    /// </summary>
    /// <param name="historyName">A past name of the Player</param>
    /// <returns></returns>
    [HttpGet("{historyName}")]
    public async Task<ActionResult<IRestResult<PlayerNameModel>>> ByHistory(string historyName, [FromQuery] string? lang = "en")
    {
        PlayerNameHistoryModel? historyModel =
            await _context.Histories.FirstOrDefaultAsync(it => it.Name.ToLower() == historyName.ToLower());
        if (historyModel is null)
            return Ok(IRestResult<PlayerNameModel>.Create(true,
                "Der Spieler konnte nicht gefunden werden. Entweder gibt es keine Eintrag in der Datenbank oder er hat bisher seinen Namen nicht gewechselt."));

        if (!await _context.CheckAgreement(historyModel.Player.Uuid))
            return Ok(IRestResult<PlayerNameModel>.Create(true,
                "Der Spieler hat die Einverständniserklärung nicht akzeptiert. Solange die Erklärung nicht akzeptiert sind, kann man nicht auf die Informationen zugreifen!"));
        return Ok(IRestResult<PlayerNameModel>.Create(false, "Der Name wurde erfolgreich ausgelesen.",
            historyModel.Player));
    }

    /// <summary>
    /// Method to update the current name in the database.
    /// </summary>
    /// <param name="uuid">The UUID of the Player</param>
    /// <param name="name">The new name of the Player</param>
    /// <returns></returns>
    [HttpPost("{uuid:guid}/{name}")]
    public async Task<ActionResult<IRestResult<PlayerNameModel>>> Add(Guid uuid, string name, [FromQuery] string? lang = "en")
    {
        if (!await _context.CheckAgreement(uuid))
            return Ok(IRestResult<PlayerNameModel>.Create(true,
                "Der Spieler hat die Einverständniserklärung nicht akzeptiert. Solange die Erklärung nicht akzeptiert sind, kann man nicht auf die Informationen zugreifen!"));

        DateTime time = DateTime.UtcNow;
        PlayerNameModel? nameModel = await _context.Names.FindAsync(uuid);
        if (nameModel is null)
        {
            nameModel = new PlayerNameModel()
            {
                Name = name,
                Since = time,
                Uuid = uuid
            };
            await _context.Names.AddAsync(nameModel);
            await _context.SaveChangesAsync();
            return Ok(IRestResult<PlayerNameModel>.Create(false,
                $"Der Name wurde erfolgreich in die Datenbank geschrieben. ({uuid} -> {name})", nameModel));
        }

        PlayerNameHistoryModel historyModel = new PlayerNameHistoryModel()
        {
            From = nameModel.Since,
            Name = nameModel.Name,
            Player = nameModel,
            To = time
        };
        string tmpName = nameModel.Name;
        nameModel.Name = name;
        nameModel.Since = time;

        await _context.Histories.AddAsync(historyModel);
        _context.Names.Update(nameModel);
        await _context.SaveChangesAsync();

        return Ok(IRestResult<PlayerNameModel>.Create(false,
            $"Der Name wurde erfolgreich in die Datenbank geschrieben. ({uuid} -> ({tmpName} -> {name}))", nameModel));
    }
}