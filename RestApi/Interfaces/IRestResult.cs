namespace RestApi.Interfaces;

/// <summary>
/// Interface to specify a Rest-Result
/// </summary>
/// <typeparam name="TType">Result-Type</typeparam>
public interface IRestResult<out TType>
{
    /// <summary>
    /// Specify if the request throw an error
    /// </summary>
    public bool Error { get; }

    /// <summary>
    /// Specify the message of the result
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Specify the result (The result is null if the request throws an error)
    /// </summary>
    public TType? Result { get; }
}

/// <summary>
/// Temp Interface to provide a Create-Method
/// </summary>
public interface IRestResult
{
    /// <summary>
    /// Method to Create a Rest-Result
    /// </summary>
    /// <param name="error">Value for the Error-Property</param>
    /// <param name="message">Value for the Message-Property</param>
    /// <param name="result">Value for the Result-Property</param>
    /// <typeparam name="TType">Result-Type</typeparam>
    /// <returns></returns>
    public static IRestResult<TType> Create<TType>(bool error, string message, TType? result = default)
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