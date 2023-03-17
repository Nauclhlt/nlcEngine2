namespace nlcEngine;

/// <summary>
/// Wraps an OpenGL cube texture, which is commonly used to render the sky box.<br />
/// This object is made only for sky box rendering.
/// </summary>
public sealed class SkyCubeMap : IDisposable, INamed
{
    int _name;
    int _buffer;
    int _vertexArray;
    bool _disposed = false;

    /// <summary>
    /// Gets the texture name.
    /// </summary>
    public int Name => _name;


    /// <summary>
    /// Creates a new instance with the faces.
    /// </summary>
    /// <param name="faces">array that contains 6 faces</param>
    public SkyCubeMap(Texture[] faces)
    {
        NlcArgException.NullThrow(nameof(faces), faces);
        if (faces.Length != 6)
        {
            throw new NlcCommonException("The faces array must contain 6 items.");
        }

        _name = GL.GenTexture();
        GL.BindTexture(TextureTarget.TextureCubeMap, _name);

        int width, height;
        for (int i = 0; i < faces.Length; i++)
        {
            Texture texture = faces[i];

            width = texture.Width;
            height = texture.Height;

            byte[] buf = new byte[4 * width * height];
            GL.GetTextureImage(texture.Name, 0, PixelFormat.Rgba, PixelType.UnsignedByte, buf.Length, buf);

            GL.BindTexture(TextureTarget.TextureCubeMap, _name);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, buf);
        }

        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

        GL.BindTexture(TextureTarget.TextureCubeMap, 0);

        float[] vertices = new float[] {
            // positions          
            -1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            -1.0f,  1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f,  1.0f
        };

        _buffer = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * vertices.Length), vertices, BufferUsageHint.StaticDraw);

        _vertexArray = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        ResourceCollector.Add(this);
    }

    /// <summary>
    /// Renders the sky box. Order: right, left, top, bottom, front, back
    /// </summary>
    /// <param name="camera">camera</param>
    public void Render(Camera camera)
    {
        NlcHelper.DispThrow(_disposed);

        GL.DepthFunc(DepthFunction.Lequal);
        Shader shader = CoreShaders.SkyboxShader;
        shader.Activate();
        Matrix4 view = new Matrix4(new Matrix3(camera.CreateViewMatrix()));
        NlcHelper.SendMat(Matrix4.Identity, view, Viewer.ProjectionMatrix);


        GL.BindVertexArray(_vertexArray);
        GL.BindTexture(TextureTarget.TextureCubeMap, _name);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        GL.ActiveTexture(TextureUnit.Texture0);
        
        GL.BindTexture(TextureTarget.TextureCubeMap, 0);
        GL.BindVertexArray(0);

        GL.DepthFunc(DepthFunction.Less);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteTexture(_name);
            GL.DeleteBuffer(_buffer);
            GL.DeleteVertexArray(_vertexArray);

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the sky box.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// A destructor.
    /// </summary>
    ~SkyCubeMap()
    {
        Dispose(false);
    }
}