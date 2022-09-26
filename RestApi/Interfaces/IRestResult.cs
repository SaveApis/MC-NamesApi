namespace RestApi.Interfaces;

public interface IRestResult<TType>
{
    public bool Error { get; }
    public string Message { get; }
    public TType? Result { get; }

    public static IRestResult<TType> Create(bool error, string message, TType? result = default)
    {
        BaseRestResult<TType> restResult = new BaseRestResult<TType>(error, message, result);
        return restResult;
    }
}

internal class BaseRestResult<TType> : IRestResult<TType>
{
    public BaseRestResult(bool error, string message, TType? result)
    {
        Error = error;
        Message = message;
        Result = result;
    }

    public bool Error { get; }
    public string Message { get; }
    public TType? Result { get; }
}