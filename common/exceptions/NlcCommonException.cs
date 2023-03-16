namespace nlcEngine;

/// <summary>
/// An exception thrown when general errors.
/// </summary>
public sealed class NlcCommonException : Exception
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public NlcCommonException() : base("An argument was invalid.")
    {
    }

    /// <summary>
    /// Creates a new instance with the message.
    /// </summary>
    /// <param name="msg">message</param>
    public NlcCommonException(string msg) : base(msg)
    {
    }

    /// <summary>
    /// Creates a new instance with the message, and the inner exception, which caused this exception.
    /// </summary>
    /// <param name="msg">message</param>
    /// <param name="innerException">inner exception</param>
    public NlcCommonException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}