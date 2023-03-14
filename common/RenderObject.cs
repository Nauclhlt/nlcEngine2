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
    /// <param name="transform">transformation</param>
    public void RenderJust( Camera camera, Transform transform = default )
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

        Matrix4 mm = transform.GetModelMatrix();
        Matrix4 vm = camera.CreateViewMatrix();
        Matrix4 pm = Viewer.ProjectionMatrix;

        GL.UniformMatrix4(0, true, ref mm);
        GL.UniformMatrix4(1, false, ref vm);
        GL.UniformMatrix4(2, false, ref pm);

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

        NlcHelper.SendMat(model, view, proj);

        shader.SetBoolean("textured", _texture is not null);
        if (_texture is not null)
        {
            shader.SetInt("pTexture", 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _texture.Name);
        }

        CreateCallRender();

        GL.BindTexture(TextureTarget.Texture2D, 0);
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



    #region statics

    static GBuffer _gBuffer;
    static LightStorageBuffer _lightBuffer;

    private static GBuffer GetOrCreateGBuffer()
    {
        if (_gBuffer is null)
        {
            _gBuffer = new GBuffer();
            return _gBuffer;
        }
        else
        {
            return _gBuffer;
        }
    }

    private static void EnsureLightBufferCreate()
    {
        if (_lightBuffer is null)
        {
            _lightBuffer = new LightStorageBuffer();
        }
    }

    /// <summary>
    /// Renders the objects with deferred lighting.<br />
    /// This process should be runned at the beginning of a frame, and other objects with no deferred lighting should be rendered after this process.
    /// </summary>
    /// <param name="camera">camera</param>
    /// <param name="renderer">renderer</param>
    /// <param name="env">light environment</param>
    public static void RenderWithLightDeferred( Camera camera, DeferredList renderer, LightEnvironment env, Transform transform = default )
    {
        Color backColor = NlcEngineGame.SceneService.GetBackgroundColor();

        GBuffer gbuffer = GetOrCreateGBuffer();
        EnsureLightBufferCreate();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, gbuffer.Name);
        GL.ClearColor(0, 0, 0, 0);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 projection = Viewer.ProjectionMatrix;
        Matrix4 view = camera.CreateViewMatrix();
        Matrix4 model = transform.GetModelMatrix();

        var objList = renderer.GetListOfObjects();
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].DeferRender(model, view, projection);
        }

        //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        DefaultBuffer.Bind();

        //Color b = Color.Red;
        GL.ClearColor(backColor.Rf, backColor.Gf, backColor.Bf, backColor.Af);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Shader shader = CoreShaders.DeferLightShader;
        shader.Activate();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GPosition);
        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GNormal);
        GL.ActiveTexture(TextureUnit.Texture2);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GColorSpec);

        _lightBuffer.Buffer(env.Lights);

        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _lightBuffer.Name);
        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, _lightBuffer.Name);

        shader.SetFloat("ambientIntensity", env.AmbientIntensity);
        shader.SetVec3("ambientColor", new Vec3(env.AmbientColor.Rf, env.AmbientColor.Gf, env.AmbientColor.Bf));
        shader.SetInt("lightCount", Math.Min(env.Lights.Count, 128));
        
        shader.SetVec3("viewPos", camera.Position);

        shader.SetInt("gPosition", 0);
        shader.SetInt("gNormal", 1);
        shader.SetInt("gColorSpec", 2);
        shader.SetVec3("backColor", new Vec3(backColor.Rf, backColor.Gf, backColor.Bf));

        RenderDefQuad();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, gbuffer.Name);
        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, DefaultBuffer.Framebuffer);

        GL.BlitFramebuffer(0, 0, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, 0, 0, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        DefaultBuffer.Bind();

        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, 0);
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
    }

    private static void RenderDefQuad()
    {
        float[] va = new float[]
        {
            -1f, 1f, 0f,
            -1f, -1f, 0f,
            1f, 1f, 0f,
            1f, -1f, 0f
        };

        float[] ta = new float[]
        {
            0f, 1f,
            0f, 0f,
            1f, 1f,
            1f, 0f
        };

        Rdc.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, va);
        Rdc.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, ta);

        Rdc.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
    }

    #endregion
}