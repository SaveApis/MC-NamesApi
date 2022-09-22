#region

using McNamesApi.Data.Context;
using McNamesApi.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace McNamesApi.Data.Controller;

/// <summary>
///     Controller to manage the Players.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PlayersController : ControllerBase
{
    private readonly DataContext _context;

    /// <summary>
    ///     Default Constructor.
    /// </summary>
    /// <param name="context">Database Context.</param>
    public PlayersController(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Method to add/edit the player’s data in the Database.
    /// </summary>
    /// <param name="model">Model where the necessary information is specified.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Add(PlayerModelAddDto model)
    {
        // TODO Add Message
        // TODO Add Result to Action
        // TODO Add Agreement Request
        try
        {
            var agreement = await _context.Agreements.FirstOrDefaultAsync(it => it.Uuid == model.Uuid);

            if (agreement is null || agreement.Agreed)
                return BadRequest();

            var playerModel = await _context.Players.FindAsync(model.Uuid);
            if (playerModel is null)
            {
                var newPlayerModel = new PlayerModel
                {
                    Uuid = model.Uuid,
                    Name = model.Name,
                    Created = DateTime.UtcNow,
                    LastChange = DateTime.UtcNow
                };
                await _context.Players.AddAsync(newPlayerModel);
                await _context.SaveChangesAsync();
                return Ok();
            }

            if (model.Name.Equals(playerModel.Name, StringComparison.InvariantCultureIgnoreCase))
                return Ok();
            var now = DateTime.UtcNow;
            var historyModel = new NameHistoryModel
            {
                Name = playerModel.Name,
                Player = playerModel,
                Since = playerModel.LastChange,
                To = now
            };

            playerModel.Name = model.Name;
            playerModel.LastChange = now;
            await _context.NameHistory.AddAsync(historyModel);
            _context.Players.Update(playerModel);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}