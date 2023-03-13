namespace nlcEngine;

internal static class NlcHelper
{
    public static void InThrow()
    {
        if (NlcEngineGame.Window is null)
        {
            throw new NlcNotInitializedException("The game is not initialized.");
        }
    }

    public static void DispThrow(bool disposed)
    {
        if (disposed)
        {
            throw new NlcCommonException("Attempted to access the disposed object.");
        }
    }

    public static void FileThrow(string filename)
    {
        if (!File.Exists(filename))
        {
            throw new NlcCommonException("Specified file path not found.");
        }
    }
}