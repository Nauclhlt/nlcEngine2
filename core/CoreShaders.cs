namespace nlcEngine;

internal static class CoreShaders
{
    private static Shader _copyShader;
    private static Shader _stdShader;
    private static Shader _deferGBufShader;
    private static Shader _deferLightShader;
    private static Shader _stdLightShader;
    private static Shader _modelDeferGShader;

    public static Shader CopyShader => _copyShader;
    public static Shader StdShader => _stdShader;
    public static Shader DeferGBufShader => _deferGBufShader;
    public static Shader DeferLightShader => _deferLightShader;
    public static Shader StdLightShader => _stdLightShader;
    public static Shader ModelDeferGShader => _modelDeferGShader;

    public static void Load()
    {
        Assembly asm = typeof(NlcEngineGame).Assembly;

        _copyShader = LoadFrom(asm, "copy_shader_vert", "copy_shader_frag");
        _stdShader = LoadFrom(asm, "std_shader_vert", "std_shader_frag");
        _deferGBufShader = LoadFrom(asm, "defer_g_vert", "defer_g_frag");
        _deferLightShader = LoadFrom(asm, "defer_light_vert", "defer_light_frag");
        _stdLightShader = LoadFrom(asm, "std_light_vert", "std_light_frag");
        _modelDeferGShader = LoadFrom(asm, "model_defer_g_vert", "model_defer_g_frag");
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