namespace nlcEngine;

internal static class CoreShaders
{
    private static Shader _copyShader;
    private static Shader _stdShader;
    private static Shader _deferGBufShader;
    private static Shader _deferLightShader;
    private static Shader _stdLightShader;
    private static Shader _modelDeferGShader;
    private static Shader _modelStdShader;
    private static Shader _brightShader;
    private static Shader _blurShader;
    private static Shader _addShader;
    private static Shader _stdDepthShader;
    private static Shader _depthLightShader;
    private static Shader _modelDepthShader;
    private static Shader _depthDebugShader;
    private static Shader _skyboxShader;

    public static Shader CopyShader => _copyShader;
    public static Shader StdShader => _stdShader;
    public static Shader DeferGBufShader => _deferGBufShader;
    public static Shader DeferLightShader => _deferLightShader;
    public static Shader StdLightShader => _stdLightShader;
    public static Shader ModelDeferGShader => _modelDeferGShader;
    public static Shader ModelStdShader => _modelStdShader;
    public static Shader BrightShader => _brightShader;
    public static Shader BlurShader => _blurShader;
    public static Shader AddShader => _addShader;
    public static Shader StdDepthShader => _stdDepthShader;
    public static Shader DepthLightShader => _depthLightShader;
    public static Shader ModelDepthShader => _modelDepthShader;
    public static Shader DepthDebugShader => _depthDebugShader;
    public static Shader SkyboxShader => _skyboxShader;

    public static void Load()
    {
        Assembly asm = typeof(NlcEngineGame).Assembly;

        _copyShader = LoadFrom(asm, "copy_shader_vert", "copy_shader_frag");
        _stdShader = LoadFrom(asm, "std_shader_vert", "std_shader_frag");
        _deferGBufShader = LoadFrom(asm, "defer_g_vert", "defer_g_frag");
        _deferLightShader = LoadFrom(asm, "defer_light_vert", "defer_light_frag");
        _stdLightShader = LoadFrom(asm, "std_light_vert", "std_light_frag");
        _modelDeferGShader = LoadFrom(asm, "model_defer_g_vert", "model_defer_g_frag");
        _modelStdShader = LoadFrom(asm, "model_std_vert", "model_std_frag");
        _brightShader = LoadFrom(asm, "bloom_bright_vert", "bloom_bright_frag");
        _blurShader = LoadFrom(asm, "bloom_blur_vert", "bloom_blur_frag");
        _addShader = LoadFrom(asm, "bloom_add_vert", "bloom_add_frag");
        _stdDepthShader = LoadFrom(asm, "std_depth_vert", "std_depth_frag");
        _depthLightShader = LoadFrom(asm, "shadow_defer_vert", "shadow_defer_frag");
        _modelDepthShader = LoadFrom(asm, "model_depth_vert", "model_depth_frag");
        _depthDebugShader = LoadFrom(asm, "depth_debug_vert", "depth_debug_frag");
        _skyboxShader = LoadFrom(asm, "skybox_vert", "skybox_frag");
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