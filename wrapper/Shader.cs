namespace nlcEngine;

/// <summary>
/// Wraps an OpenGL shader.
/// </summary>
public sealed class Shader : IDisposable, INamed
{
#region Built-in shaders

    private static Shader _standard;

    /// <summary>
    /// Gets the Standard shader.
    /// </summary>
    public static Shader Standard => _standard;

    internal static void Load()
    {
        Assembly asm = typeof(NlcEngineGame).Assembly;
        _standard = LoadShader(asm, "std_shader_vert", "std_shader_frag");
    }

    private static Shader LoadShader(Assembly asm, string vertName, string fragName)
    {
        string v = Read(asm.GetManifestResourceStream(vertName));
        string f = Read(asm.GetManifestResourceStream(fragName));
        return new Shader(v, f);
    }

    private static string Read(Stream stream)
    {
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

#endregion







    string _vertexShader;
    string _fragmentShader;
    int _name;
    bool _disposed = false;

    /// <summary>
    /// Gets the shader name.
    /// </summary>
    public int Name => _name;
    /// <summary>
    /// Gets the source code of the vertex shader.
    /// </summary>
    public string VertexShader => _vertexShader;
    /// <summary>
    /// Gets the source code of the fragment shader.
    /// </summary>
    public string FragmentShader => _fragmentShader;

    /// <summary>
    /// Compiles the shaders, and attaches and link them on the shader program.
    /// </summary>
    /// <param name="vertexShaderSource">vertex shader source</param>
    /// <param name="fragmentShaderSource">fragment shader source</param>
    public Shader(string vertexShaderSource, string fragmentShaderSource)
    {
        NlcHelper.InThrow();

        _vertexShader = vertexShaderSource;
        _fragmentShader = fragmentShaderSource;

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);

        GL.CompileShader(vertexShader);

        GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vc);
        if (vc == 0)
        {
            string log = GL.GetShaderInfoLog(vertexShader);

            throw new NlcCommonException("A compilation error in the vertex shader:" + Environment.NewLine + log);
        }

        GL.CompileShader(fragmentShader);

        GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fc);
        if (fc == 0)
        {
            string log = GL.GetShaderInfoLog(fragmentShader);

            throw new NlcCommonException("A compilation error in the fragment shader:" + Environment.NewLine + log);
        }

        _name = GL.CreateProgram();

        GL.AttachShader(_name, vertexShader);
        GL.AttachShader(_name, fragmentShader);

        GL.LinkProgram(_name);

        GL.DetachShader(_name, vertexShader);
        GL.DetachShader(_name, fragmentShader);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        ResourceCollector.Add(this);
    }

    /// <summary>
    /// Activates the shader.
    /// </summary>
    public void Activate()
    {
        NlcHelper.DispThrow(_disposed);

        GL.UseProgram(_name);
    }

    /// <summary>
    /// Sets the uniform int.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="value">uniform</param>
    public void SetInt(string name, int value)
    {
        GL.Uniform1(GL.GetUniformLocation(_name, name), value);
    }

    /// <summary>
    /// Sets the uniform float.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="value">value</param>
    public void SetFloat(string name, float value)
    {
        GL.Uniform1(GL.GetUniformLocation(_name, name), value);
    }

    /// <summary>
    /// Sets the uniform boolean.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="value">value</param>
    public void SetBoolean(string name, bool value)
    {
        GL.Uniform1(GL.GetUniformLocation(_name, name), value ? 1 : 0);
    }

    /// <summary>
    /// Sets the uniform vector.
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="vector">vector</param>
    public void SetVec3(string name, Vec3 vector)
    {
        GL.Uniform3(GL.GetUniformLocation(_name, name), vector.X, vector.Y, vector.Z);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteProgram(_name);

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the shader.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// A destructor.
    /// </summary>
    ~Shader()
    {
        Dispose(false);
    }
}