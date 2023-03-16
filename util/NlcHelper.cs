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

    public static void SendMat(Matrix4 model, Matrix4 view, Matrix4 proj)
    {
        GL.UniformMatrix4(0, true, ref model);
        GL.UniformMatrix4(1, false, ref view);
        GL.UniformMatrix4(2, false, ref proj);
    }

    public static Vector3 Conv(Vec3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
}