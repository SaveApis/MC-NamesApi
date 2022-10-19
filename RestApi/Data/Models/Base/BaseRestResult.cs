namespace RestApi.Data.Models.Base;

public class BaseRestResult<TResult>
{
    public bool Error { get; }
    public string Message { get; }
    public TResult? Result { get; }

    public BaseRestResult(bool error, string message, TResult? result = default)
    {
        Error = error;
        Message = message;
        Result = result;
    }
}