namespace McNamesApi.Interfaces;

/// <summary>
///     Interface to specify a standard model for the return of the REST-Api
/// </summary>
/// <typeparam name="TComponent">Specifies the type of the Result</typeparam>
public interface IRestResult<out TComponent>
{
    /// <summary>
    ///     Specify the Error of the Result
    /// </summary>
    public bool Error { get; }

    /// <summary>
    ///     Specify the Message of the Result
    /// </summary>
    public string Message { get; }

    /// <summary>
    ///     Specify the Result
    /// </summary>
    public TComponent? Result { get; }
}