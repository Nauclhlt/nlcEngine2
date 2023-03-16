namespace nlcEngine;

/// <summary>
/// An exception thrown when the game is not initialized.
/// </summary>
public sealed class NlcNotInitializedException : Exception
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public NlcNotInitializedException() : base()
    {
    }

    /// <summary>
    /// Creates a new instance with the message.
    /// </summary>
    /// <param name="msg">message</param>
    public NlcNotInitializedException(string msg) : base(msg)
    {

    }
}