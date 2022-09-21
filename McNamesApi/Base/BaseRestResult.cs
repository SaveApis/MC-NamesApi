#region

using McNamesApi.Interfaces;

#endregion

namespace McNamesApi.Base;

/// <summary>
///     Class to create an IRestResult.
/// </summary>
/// <typeparam name="TComponent">Specifies the Type of the Result Property.</typeparam>
public class BaseRestResult<TComponent> : IRestResult<TComponent>
{
    /// <summary>
    ///     Specifies the Constructor
    /// </summary>
    /// <param name="error">Specifies the value of the Error Property.</param>
    /// <param name="message">Specifies the value of the Message Property.</param>
    /// <param name="result">Specifies the value of the Result Property.</param>
    public BaseRestResult(bool error, string message, TComponent? result = default)
    {
        Error = error;
        Message = message;
        Result = result;
    }

    /// <summary>
    ///     Specifies whether the action was successful.
    /// </summary>
    public bool Error { get; }

    /// <summary>
    ///     Specifies a message related to the action. <br />
    ///     (e. g. the message of an exception)
    /// </summary>
    public string Message { get; }

    /// <summary>
    ///     Specifies the result of the action. (Is ’null' if the action fails)
    /// </summary>
    public TComponent? Result { get; }
}