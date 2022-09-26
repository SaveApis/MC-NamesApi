using RestApi.Interfaces;
using Swashbuckle.AspNetCore.Filters;

namespace RestApi.Data.Models.Examples.Agreement;

public class UpdatePlayerExample : IMultipleExamplesProvider<IRestResult<bool>>
{
    public IEnumerable<SwaggerExample<IRestResult<bool>>> GetExamples()
    {
        yield return SwaggerExample.Create("On Success (Create -> True)", "On Success (Create -> True)",
            IRestResult<bool>.Create(false, "The entry for the UUID (yourUuid) was successfully created.", true));
        yield return SwaggerExample.Create("On Success (Create -> False)", "On Success (Create -> False)",
            IRestResult<bool>.Create(false, "The entry for the UUID (yourUuid) was successfully created."));
        yield return SwaggerExample.Create("On Success (No Changes -> True)", "On Success (No Changes -> True)",
            IRestResult<bool>.Create(false,
                "The specified value is the same as in the database. No changes will be made.", true));
        yield return SwaggerExample.Create("On Success (No Changes -> False)", "On Success (No Changes -> False)",
            IRestResult<bool>.Create(false,
                "The specified value is the same as in the database. No changes will be made."));


        yield return SwaggerExample.Create("On Success (Update -> True)", "On Success (Update -> True)",
            IRestResult<bool>.Create(false, "The entry for the UUID ({uuid}) has been successfully updated.",
                true));
        yield return SwaggerExample.Create("On Success (Update -> False)", "On Success (Update -> False)",
            IRestResult<bool>.Create(false, "The entry for the UUID ({uuid}) has been successfully updated."));
    }
}