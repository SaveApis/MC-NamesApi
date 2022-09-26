using RestApi.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace RestApi.Data.Models.Examples.Agreement;

public class GetPlayerExample : IMultipleExamplesProvider<IRestResult<bool>>
{
    public IEnumerable<SwaggerExample<IRestResult<bool>>> GetExamples()
    {
        yield return SwaggerExample.Create("On Success (True)", "On Success (True)",
            IRestResult<bool>.Create(false, "Successfully found an entry for the UUID (yourUuid).", true));
        yield return SwaggerExample.Create("On Success (False)", "On Success (False)",
            IRestResult<bool>.Create(false, "Successfully found an entry for the UUID (yourUuid)."));
        yield return SwaggerExample.Create("On Error", "On Error",
            IRestResult<bool>.Create(true, "No entry for the UUID (yourUuid) was found in the database."));
    }
}