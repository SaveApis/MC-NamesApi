using RestApi.Data.Context;
using RestApi.Data.Controller.Base;

namespace RestApi.Data.Controller;

public class NameController : BaseController<NameController>
{
    public NameController(DataContext context, ILogger<NameController> logger) : base(context, logger)
    {
    }
}