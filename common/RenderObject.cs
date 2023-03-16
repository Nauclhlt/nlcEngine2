namespace nlcEngine;

/// <summary>
/// A base class for render-objects.
/// </summary>
public abstract class RenderObject : IDisposable, IDefer
{
    private Vec3[] _vertices;
    private Color[] _colors;
    private Vec3[] _normals;
    private Vec2[] _texCoords;
    private Primitive _primitive;
    private ObjectBuffer _buffer;
    private ITexture _texture;
    Transform _transform;
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
    /// Gets or sets the texture used to render the vertices.
    /// </summary>
    protected ITexture Texture
    {
        get => _texture;
        set => _texture = value;
    }

    /// <summary>
    /// Gets or sets the transform applied to the object.
    /// </summary>
    public Transform Transform
    {
        get => _transform;
        set => _transform = value;
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
    public void RenderJust(Camera camera)
    {
        if (_buffer is null)
        {
            CreateBuffer();
        }

        Shader shader = CoreShaders.StdShader;
        shader.Activate();

        GL.Uniform1(GL.GetUniformLocation(shader.Name, "textured"), _texture is not null ? 1 : 0);
        if (_texture is not null)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _texture.Name);
        }

        Matrix4 mm = _transform.GetModelMatrix();
        Matrix4 vm = camera.CreateViewMatrix();
        Matrix4 pm = Viewer.ProjectionMatrix;

        NlcHelper.SendMat(mm, vm, pm);

        _buffer.JustCallRender();

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    /// <summary>
    /// Renders self. <br />
    /// THIS METHOD IS USED IN THE GAME ENGINE INTERNAL PROCESS. DO NOT CALL THIS FROM NORMAL CODE.
    /// </summary>
    public void DeferRender(Matrix4 model, Matrix4 view, Matrix4 proj)
    {
        Shader shader = CoreShaders.DeferGBufShader;
        shader.Activate();

        shader.SetBoolean("textured", _texture is not null);

        NlcHelper.SendMat(_transform.GetModelMatrix(), view, proj);



        if (_texture is not null)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _texture.Name);
            shader.SetInt("pTexture", 0);
        }

        CreateCallRender();

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    /// <summary>
    /// Renders self.<br />
    /// THIS METHOD IS USED IN THE GAME ENGINE INTERNAL PROCESS. DO NOT CALL THIS FROM NORMAL CODE.
    /// </summary>
    /// <param name="lightSpaceMatrix"></param>
    public void DepthRender(Matrix4 lightSpaceMatrix)
    {
        Shader shader = CoreShaders.StdDepthShader;
        shader.Activate();

        Matrix4 model = _transform.GetModelMatrix();
        GL.UniformMatrix4(0, false, ref lightSpaceMatrix);
        GL.UniformMatrix4(1, true, ref model);

        CreateCallRender();
    }

    internal void CreateCallRender()
    {
        if (_buffer is null)
        {
            CreateBuffer();
        }

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