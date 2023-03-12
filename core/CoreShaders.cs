namespace nlcEngine;

internal static class CoreShaders
{
    private static Shader _copyShader;

    public static Shader CopyShader => _copyShader;

    public static void Load()
    {
        Assembly asm = typeof(NlcEngineGame).Assembly;

        _copyShader = LoadFrom(asm, "copy_shader_vert", "copy_shader_frag");
    }

    private static Shader LoadFrom(Assembly asm, string vertName, string fragName)
    {
        string vs = Read(asm.GetManifestResourceStream(vertName));
        string fs = Read(asm.GetManifestResourceStream(fragName));

        return new Shader(vs, fs);
    }

    private static string Read( Stream stream )
    {
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}