namespace McNamesApi.Base.RestResult;

public class BoolRestResult : BaseRestResult<bool>
{
    public BoolRestResult(bool error, string message, bool result = default) : base(error, message, result)
    {
    }
}