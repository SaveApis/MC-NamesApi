#region

using McNamesApi.Base;
using McNamesApi.Data.Context;
using McNamesApi.Data.Models;
using McNamesApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace McNamesApi.Data.Controller;

/// <summary>
///     Controller to determine the name/name history.
/// </summary>
[ApiController]
[Route("[controller]")]
public class NamesController : ControllerBase
{
    private readonly DataContext _context;

    /// <summary>
    ///     Default Constructor
    /// </summary>
    /// <param name="context">Database Context</param>
    public NamesController(DataContext context)
    {
        _context = context;
    }


    /// <summary>
    ///     Method to read all previously stored names in connection with the UUID.
    /// </summary>
    /// <returns></returns>
    [HttpGet("[action]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<IRestResult<Dictionary<Guid, string>>>> AllNames()
    {
        Dictionary<Guid, string> pairs = await _context.Players.ToDictionaryAsync(it => it.Uuid, it => it.Name);
        return Ok(new BaseRestResult<Dictionary<Guid, string>>(false, "All names were successfully read.", pairs));
    }

    /// <summary>
    ///     Method to read all previously stored NameHistories in connection with the UUID.
    /// </summary>
    /// <returns></returns>
    [HttpGet("[action]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<IRestResult<Dictionary<Guid, IEnumerable<string>>>>> AllHistoryNames()
    {
        Dictionary<Guid, IEnumerable<string>> relations = new Dictionary<Guid, IEnumerable<string>>();
        List<NameHistoryModel> historyModels = await _context.NameHistory.ToListAsync();
        IEnumerable<Guid> uuids = historyModels.Select(it => it.Player.Uuid).Distinct();

        foreach (Guid uuid in uuids)
        {
            IEnumerable<string> names =
                historyModels.Where(it => it.Player.Uuid == uuid).Select(it => it.Name);
            relations.Add(uuid, names);
        }

        return Ok(
            new BaseRestResult<Dictionary<Guid, IEnumerable<string>>>(
                false,
                "All HistoryNames were successfully read.",
                relations));
    }


    /// <summary>
    ///     Method to determine the current name of the player based on the UUID.
    /// </summary>
    /// <param name="uuid">The UUID of the player.</param>
    /// <returns>The name of the player</returns>
    [HttpGet("Current/{uuid}")]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<IRestResult<string>>> NameByUuid(Guid uuid)
    {
        PlayerModel? model = await _context.Players.FindAsync(uuid);
        if (model is null)
            return NotFound(new BaseRestResult<string>(true,
                "No entry was found in the database for the specified UUID."));
        return Ok(new BaseRestResult<string>(false, "The name was successfully read out.", model.Name));
    }

    /// <summary>
    ///     Method to determine the NameHistory of the player based on the UUID.
    /// </summary>
    /// <param name="uuid">The UUID of the player.</param>
    /// <returns>The NameHistory of the player</returns>
    [HttpGet("History/{uuid}")]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<ActionResult<IRestResult<IEnumerable<string>>>> HistoryNameByUuid(Guid uuid)
    {
        List<NameHistoryModel> models = await _context.NameHistory.Where(it => it.Player.Uuid == uuid).ToListAsync();
        return Ok(new BaseRestResult<IEnumerable<string>>(false, "The NameHistory was successfully read.",
            models.Select(it => it.Name)));
    }
}