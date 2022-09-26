using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data.Context;
using RestApi.Data.Models;
using RestApi.Extensions;
using RestApi.Interfaces;

namespace RestApi.Data.Controller;

[ApiController]
[Route("[controller]")]
public class HistoryController : ControllerBase
{
    private readonly DataContext _context;

    public HistoryController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<IRestResult<IEnumerable<PlayerNameHistoryModel>>>> AllHistoriesByUuid(Guid uuid)
    {
        if (!await _context.CheckAgreement(uuid))
            return Ok(IRestResult<IEnumerable<PlayerNameHistoryModel>>.Create(true,
                "Die NameHistory konnte nicht abgerufen werden."));
        List<PlayerNameHistoryModel> history =
            await _context.Histories.Where(it => it.Player.Uuid == uuid).ToListAsync();
        return Ok(IRestResult<IEnumerable<PlayerNameHistoryModel>>.Create(false,
            "HistoryNames wurden erfolgreich ausgelesen.", history));
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<IRestResult<IEnumerable<PlayerNameHistoryModel>>>> AllHistoriesByName(string name)
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