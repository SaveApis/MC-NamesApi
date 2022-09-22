#region

using McNamesApi.Base;
using McNamesApi.Data.Context;
using McNamesApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace McNamesApi.Data.Controller;

/// <summary>
///     Controller to determine the uuid.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class UuidsController : ControllerBase
{
    private readonly DataContext _context;

    /// <summary>
    ///     Default Constructor
    /// </summary>
    /// <param name="context">Database Context</param>
    public UuidsController(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Method to determine the player’s UUID based on their name.
    /// </summary>
    /// <param name="name">The current name of the player.</param>
    /// <returns>The UUID of the specified player. (Is ’null' if the player is not registered in the database)</returns>
    [HttpGet("Current/{name}")]
    public async Task<ActionResult<IRestResult<Guid>>> ByCurrentName(string name)
    {
        var model =
            await _context.Players.FirstOrDefaultAsync(it => it.Name.ToLower() == name.ToLower());
        if (model is null)
            return NotFound(new BaseRestResult<Guid>(true,
                "No player with this 'current' name was found in the database."));

        return Ok(new BaseRestResult<Guid>(false, "The UUID was successfully read.", model.Uuid));
    }

    /// <summary>
    ///     Method to determine the player’s UUID based on their HistoryName.
    /// </summary>
    /// <param name="name">A HistoryName of the player.</param>
    /// <returns>The UUID of the specified player. (Is ’null' if the player is not registered in the database)</returns>
    [HttpGet("History/{name}")]
    public async Task<ActionResult<IRestResult<IEnumerable<Guid>>>> ByHistoryName(string name)
    {
        var historyModels =
            await _context.NameHistory.Where(it => it.Name.ToLower() == name.ToLower()).ToListAsync();

        return Ok(new BaseRestResult<IEnumerable<Guid>>(false,
            "The UUID was successfully read. (Several UUIDs may be read if two or more players had the same name)",
            historyModels.Select(it => it.Player.Uuid).Distinct()));
    }
}