namespace nlcEngine;

/// <summary>
/// Provides the shaders based on the types.
/// </summary>
public static class ShaderProvider
{
    private static Shader _stdShader = Shader.Standard;

    /// <summary>
    /// Gets or sets the StdShader.
    /// </summary>
    public static Shader StdShader
    {
        get => _stdShader;
        set
        {
            NlcArgException.NullThrow(nameof(value), value);

            _stdShader = value;
        }
    }
}