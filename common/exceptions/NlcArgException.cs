namespace nlcEngine;

/// <summary>
/// An exception when thrown an argument is invalid.
/// </summary>
public sealed class NlcArgException : Exception
{
    internal static void Throw(string name, string reason)
    {
        throw new NlcArgException("The value '" + name + "' was invalid. " + reason);
    }

    internal static void ZeroThrow(string name, int value)
    {
        if (value == 0)
        {
            Throw(name, "Can't be zero.");
        }
    }

    internal static void NullThrow(string name, object obj)
    {
        if (obj is null)
        {
            Throw(name, "Can't be null.");
        }
    }

    internal static void LeftZeroThrow(string name, float value)
    {
        if (value <= 0f)
        {
            Throw(name, "Can't be less or equals to 0.");
        }
    }


    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public NlcArgException() : base("An argument was invalid.")
    {
    }

    /// <summary>
    /// Creates a new instance with the message.
    /// </summary>
    /// <param name="msg">message</param>
    public NlcArgException(string msg) : base(msg)
    {
    }

    /// <summary>
    /// Creates a new instance with the message, and the inner exception, which caused this exception.
    /// </summary>
    /// <param name="msg">message</param>
    /// <param name="innerException">inner exception</param>
    public NlcArgException(string msg, Exception innerException) : base(msg, innerException)
    {
    }
}