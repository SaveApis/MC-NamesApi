using Microsoft.AspNetCore.Mvc;
using RestApi.Data.Context;

namespace RestApi.Data.Controller.Base;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class BaseController<TController> : ControllerBase
{
    protected readonly DataContext _context;
    protected readonly ILogger<TController> _logger;

    public BaseController(DataContext context, ILogger<TController> logger)
    {
        _context = context;
        _logger = logger;
    }
}