namespace nlcEngine;

/// <summary>
/// A base class for render-objects.
/// </summary>
public abstract class RenderObject : IDisposable
{
    private Vec3[] _vertices;
    private Color[] _colors;
    private Vec3[] _normals;
    private Vec2[] _texCoords;
    private Primitive _primitive;
    private ObjectBuffer _buffer;
    bool _disposed = false;


    /// <summary>
    /// Gets or sets the vertices.
    /// </summary>
    protected Vec3[] Vertices
    {
        get => _vertices;
        set => _vertices = value;
    }

    /// <summary>
    /// Gets or sets the colors.
    /// </summary>
    protected Color[] Colors
    {
        get => _colors;
        set => _colors = value;
    }

    /// <summary>
    /// Gets or sets the normals.
    /// </summary>
    protected Vec3[] Normals
    {
        get => _normals;
        set => _normals = value;
    }

    /// <summary>
    /// Gets or sets the texture coords.
    /// </summary>
    protected Vec2[] TexCoords
    {
        get => _texCoords;
        set => _texCoords = value;
    }

    /// <summary>
    /// Gets or sets the primitive type used to render the vertices.
    /// </summary>
    protected Primitive Primitive
    {
        get => _primitive;
        set => _primitive = value;
    }

    /// <summary>
    /// Forces to create the buffer.
    /// </summary>
    public void DoCreateBuffer()
    {
        CreateBuffer();
    }

    private void CreateBuffer()
    {
        if (_buffer is null)
        {
            _buffer = new ObjectBuffer(_vertices, _colors, _normals, _texCoords, _primitive);
        }
        else 
        {
            // TODO: implement rebuffering
        }
    }

    /// <summary>
    /// Just renders the object.
    /// </summary>
    /// <param name="camera">camera</param>
    public void RenderJust( Camera camera )
    {
        if (_buffer is null)
        {
            CreateBuffer();
        }
    
        Shader shader = ShaderProvider.StdShader;
        shader.Activate();

        GL.Uniform1(GL.GetUniformLocation(shader.Name, "textured"), 0);

        Matrix4 mm = Matrix4.Identity;
        Matrix4 vm = camera.CreateViewMatrix();
        Matrix4 pm = Viewer.ProjectionMatrix;

        GL.UniformMatrix4(0, false, ref mm);
        GL.UniformMatrix4(1, false, ref vm);
        GL.UniformMatrix4(2, false, ref pm);

        _buffer.JustCallRender();
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _buffer?.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the RenderObject.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// A destructor.
    /// </summary>
    ~RenderObject()
    {
        Dispose(false);
    }
}