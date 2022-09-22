using Swashbuckle.AspNetCore.Filters;

namespace McNamesApi.Base.RestResult.Examples;

public class BoolRestResultExample : IMultipleExamplesProvider<BoolRestResult>
{
    public IEnumerable<SwaggerExample<BoolRestResult>> GetExamples()
    {
        yield return SwaggerExample.Create("Success", "On Success", new BoolRestResult(false, "Info Message", true));
        yield return SwaggerExample.Create("Error", "On Error", new BoolRestResult(true, "Error Message"));
    }
}