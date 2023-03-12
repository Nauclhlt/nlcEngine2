namespace nlcEngine;

/// <summary>
/// Wraps an OpenGL shader.
/// </summary>
public sealed class Shader : IDisposable, INamed
{
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
    }

    /// <summary>
    /// Activates the shader.
    /// </summary>
    public void Activate()
    {
        GL.UseProgram(_name);
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