using McNamesApi.Base.RestResult;
using McNamesApi.Base.RestResult.Examples;
using McNamesApi.Data.Context;
using McNamesApi.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace McNamesApi.Data.Controller;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AgreementController : ControllerBase
{
    private readonly DataContext _context;

    public AgreementController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("{uuid:guid}")]
    [SwaggerRequestExample(typeof(BoolRestResult), typeof(BoolRestResultExample))]
    public async Task<ActionResult<BoolRestResult>> AddAgreementEntry(Guid uuid)
    {
        // TODO Add Messages
        var existingAgreement = await _context.Agreements.FirstOrDefaultAsync(it => it.Uuid == uuid);

        if (existingAgreement is not null)
            return Ok(new BoolRestResult(false, ""));

        var agreement = new PlayerAgreement();
        agreement.Uuid = uuid;
        agreement.Agreed = false;
        await _context.Agreements.AddAsync(agreement);
        await _context.SaveChangesAsync();
        return Ok(new BoolRestResult(false, ""));
    }

    [HttpPost("Update")]
    [SwaggerRequestExample(typeof(BoolRestResult), typeof(BoolRestResultExample))]
    public async Task<ActionResult<BoolRestResult>> UpdateAgreemententry(PlayerAgreementUpdateDto updateDto)
    {
        // TODO Add Messages
        var existingAgreement =
            await _context.Agreements.FirstOrDefaultAsync(it => it.Uuid == updateDto.Uuid);
        if (existingAgreement is null)
        {
            var newUrl = Url.Action("AddAgreementEntry", updateDto);
            if (newUrl is null)
                return BadRequest(new BoolRestResult(true, ""));
            return Redirect(newUrl);
        }

        existingAgreement.Agreed = updateDto.AgreeValue;
        _context.Agreements.Update(existingAgreement);
        await _context.SaveChangesAsync();
        return Ok(new BoolRestResult(false, "", updateDto.AgreeValue));
    }
}