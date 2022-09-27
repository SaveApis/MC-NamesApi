using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data.Context;
using RestApi.Data.Models;
using RestApi.Extensions;
using RestApi.Interfaces;

namespace RestApi.Data.Controller;

/// <summary>
/// Controller to ermittion the past names of a player.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class HistoryController : ControllerBase
{
    private readonly DataContext _context;

    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="context"></param>
    public HistoryController(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Method to determine all past names based on the UUID.
    /// </summary>
    /// <param name="uuid">The UUID of the Player</param>
    /// <returns></returns>
    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<IRestResult<IEnumerable<PlayerNameHistoryModel>>>> AllHistoriesByUuid(Guid uuid, [FromQuery] string? lang = "en")
    {
        if (!await _context.CheckAgreement(uuid))
            return Ok(IRestResult<IEnumerable<PlayerNameHistoryModel>>.Create(true,
                "Die NameHistory konnte nicht abgerufen werden."));
        List<PlayerNameHistoryModel> history =
            await _context.Histories.Where(it => it.Player.Uuid == uuid).ToListAsync();
        return Ok(IRestResult<IEnumerable<PlayerNameHistoryModel>>.Create(false,
            "HistoryNames wurden erfolgreich ausgelesen.", history));
    }

    /// <summary>
    /// Method to determine all past names based on its name.
    /// </summary>
    /// <param name="name">The current name of the Player</param>
    /// <returns></returns>
    [HttpGet("{name}")]
    public async Task<ActionResult<IRestResult<IEnumerable<PlayerNameHistoryModel>>>> AllHistoriesByName(string name, [FromQuery] string? lang = "en")
    {
        PlayerNameModel? existingPlayerModel =
            await _context.Names.FirstOrDefaultAsync(it => it.Name.ToLower() == name.ToLower());

        if (existingPlayerModel is null)
            return Ok(IRestResult<IEnumerable<PlayerNameHistoryModel>>.Create(true, "Es wurde kein Spieler gefunden."));

        if (!await _context.CheckAgreement(existingPlayerModel.Uuid))
            return Ok(IRestResult<IEnumerable<PlayerNameHistoryModel>>.Create(true,
                "Die NameHistory konnte nicht abgerufen werden."));

        IEnumerable<PlayerNameHistoryModel> history =
            _context.Histories.Where(it => it.Player == existingPlayerModel);
        return Ok(IRestResult<IEnumerable<PlayerNameHistoryModel>>.Create(false,
            "HistoryNames wurden erfolgreich ausgelesen.", history));
    }
}