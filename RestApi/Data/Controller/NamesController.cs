using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data.Context;
using RestApi.Data.Models;
using RestApi.Extensions;
using RestApi.Interfaces;

namespace RestApi.Data.Controller;

[ApiController]
[Route("[controller]")]
public class NamesController : ControllerBase
{
    private readonly DataContext _context;

    public NamesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<IRestResult<PlayerNameModel>>> ByUuid(Guid uuid)
    {
        if (!await _context.CheckAgreement(uuid))
            return Ok(IRestResult<PlayerNameModel>.Create(true,
                "Der Spieler hat die Einverständniserklärung nicht akzeptiert. Solange die Erklärung nicht akzeptiert sind, kann man nicht auf die Informationen zugreifen!"));
        PlayerNameModel? model = await _context.Names.FindAsync(uuid);
        return Ok(model is null
            ? IRestResult<PlayerNameModel>.Create(true, "Der Spieler konnte nicht gefunden werden.")
            : IRestResult<PlayerNameModel>.Create(false, "Der Name wurde erfolgreich ausgelesen.", model));
    }

    [HttpGet("{historyName}")]
    public async Task<ActionResult<IRestResult<PlayerNameModel>>> ByHistory(string historyName)
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

    [HttpPost("{uuid:guid}/{name}")]
    public async Task<ActionResult<IRestResult<PlayerNameModel>>> Add(Guid uuid, string name)
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