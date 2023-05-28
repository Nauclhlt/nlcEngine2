namespace nlcEngine;

/// <summary>
/// Represents a RenderObject with variable-length vertices.
/// </summary>
public sealed class VectorObject : RenderObject
{
    Primitive _primitive;
    List<Vec3> _vertices;
    List<Color> _colors;
    List<Vec3> _normals;
    List<Vec2> _texCoords;
    int _vertexCount;

    /// <summary>
    /// Gets whether the buffer is created.
    /// </summary>
    public bool BufferCreated => _buffer is not null;

    /// <summary>
    /// Sets the texture.
    /// </summary>
    public new ITexture Texture
    {
        get => base.Texture;
        set => base.Texture = value;
    }

    /// <summary>
    /// Creates a new instance with the primitive type.
    /// </summary>
    /// <param name="primitive">primitive type</param>
    /// <param name="size">initial size</param>
    public VectorObject( Primitive primitive, int size )
    {
        _primitive = primitive;

        _vertexCount = 0;
        _vertices = new List<Vec3>(size);
        _colors = new List<Color>(size);
        _normals = new List<Vec3>(size);
        _texCoords = new List<Vec2>(size);
    }

    /// <summary>
    /// Adds a triangle.
    /// </summary>
    /// <param name="v1">vertex 1</param>
    /// <param name="v2">vertex 2</param>
    /// <param name="v3">vertex 3</param>
    /// <param name="normal">normal vector</param>
    /// <param name="color">color</param>
    public void AddTriangle(Vec3 v1, Vec3 v2, Vec3 v3, Vec3 normal, Color color)
    {
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);

        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);

        _normals.Add(normal);
        _normals.Add(normal);
        _normals.Add(normal);

        _texCoords.Add(default);
        _texCoords.Add(default);
        _texCoords.Add(default);
    }

    /// <summary>
    /// Adds a triangle.
    /// </summary>
    /// <param name="v1">vertex 1</param>
    /// <param name="v2">vertex 2</param>
    /// <param name="v3">vertex 3</param>
    /// <param name="normal">normal vector</param>
    /// <param name="color1">color</param>
    /// <param name="color2">color</param>
    /// <param name="color3">color</param>
    public void AddTriangle(Vec3 v1, Vec3 v2, Vec3 v3, Vec3 normal, Color color1, Color color2, Color color3)
    {
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);

        _colors.Add(color1);
        _colors.Add(color2);
        _colors.Add(color3);

        _normals.Add(normal);
        _normals.Add(normal);
        _normals.Add(normal);

        _texCoords.Add(default);
        _texCoords.Add(default);
        _texCoords.Add(default);
    }

    /// <summary>
    /// Adds a quadrangle.
    /// </summary>
    /// <param name="v1">vertex 1</param>
    /// <param name="v2">vertex 2</param>
    /// <param name="v3">vertex 3</param>
    /// <param name="v4">vertex 4</param>
    /// <param name="normal1">normal 1</param>
    /// <param name="normal2">normal 2</param>
    /// <param name="normal3">normal 3</param>
    /// <param name="normal4">normal 4</param>
    /// <param name="color">color</param>
    public void AddQuad(Vec3 v1, Vec3 v2, Vec3 v3, Vec3 v4, Vec3 normal1, Vec3 normal2, Vec3 normal3, Vec3 normal4, Color color)
    {
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _vertices.Add(v4);

        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);

        _normals.Add(normal1);
        _normals.Add(normal2);
        _normals.Add(normal3);
        _normals.Add(normal4);

        _texCoords.Add(default);
        _texCoords.Add(default);
        _texCoords.Add(default);
        _texCoords.Add(default);
    }

    /// <summary>
    /// Adds a quadrangle.
    /// </summary>
    /// <param name="v1">vertex 1</param>
    /// <param name="v2">vertex 2</param>
    /// <param name="v3">vertex 3</param>
    /// <param name="v4">vertex 4</param>
    /// <param name="normal1">normal 1</param>
    /// <param name="normal2">normal 2</param>
    /// <param name="normal3">normal 3</param>
    /// <param name="normal4">normal 4</param>
    /// <param name="color1">color 1</param>
    /// <param name="color2">color 2</param>
    /// <param name="color3">color 3</param>
    /// <param name="color4">color 4</param>
    public void AddQuad(Vec3 v1, Vec3 v2, Vec3 v3, Vec3 v4, Vec3 normal1, Vec3 normal2, Vec3 normal3, Vec3 normal4, Color color1, Color color2, Color color3, Color color4)
    {
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);
        _vertices.Add(v4);

        _colors.Add(color1);
        _colors.Add(color2);
        _colors.Add(color3);
        _colors.Add(color4);

        _normals.Add(normal1);
        _normals.Add(normal2);
        _normals.Add(normal3);
        _normals.Add(normal4);

        _texCoords.Add(default);
        _texCoords.Add(default);
        _texCoords.Add(default);
        _texCoords.Add(default);
    }

    /// <summary>
    /// Adds a line.
    /// </summary>
    /// <param name="v1">vertex 1</param>
    /// <param name="v2">vertex 2</param>
    /// <param name="normal1">normal 1</param>
    /// <param name="normal2">normal 2</param>
    /// <param name="color1">color 1</param>
    /// <param name="color2">color 1</param>
    public void AddLine(Vec3 v1, Vec3 v2, Vec3 normal1, Vec3 normal2, Color color1, Color color2)
    {
        _vertices.Add(v1);
        _vertices.Add(v2);

        _colors.Add(color1);
        _colors.Add(color2);

        _normals.Add(normal1);
        _normals.Add(normal2);

        _texCoords.Add(default);
        _texCoords.Add(default);
    }

    /// <summary>
    /// Adds a line.
    /// </summary>
    /// <param name="v1">vertex 1</param>
    /// <param name="v2">vertex 2</param>
    /// <param name="normal1">normal 1</param>
    /// <param name="normal2">normal 2</param>
    /// <param name="color">color</param>
    public void AddLine(Vec3 v1, Vec3 v2, Vec3 normal1, Vec3 normal2, Color color)
    {
        _vertices.Add(v1);
        _vertices.Add(v2);

        _colors.Add(color);
        _colors.Add(color);

        _normals.Add(normal1);
        _normals.Add(normal2);

        _texCoords.Add(default);
        _texCoords.Add(default);
    }

    /// <summary>
    /// Adds a point.
    /// </summary>
    /// <param name="v">vertex</param>
    /// <param name="normal">normal</param>
    /// <param name="color">color</param>
    public void AddPoint(Vec3 v, Vec3 normal, Color color)
    {
        _vertices.Add(v);

        _colors.Add(color);

        _normals.Add(normal);

        _texCoords.Add(default);
    }

    /// <summary>
    /// Adds a textured quadrangle.
    /// </summary>
    /// <param name="v1">vertex 1, upper left</param>
    /// <param name="v2">vertex 2, lower left</param>
    /// <param name="v3">vertex 3, upper right</param>
    /// <param name="v4">vertex 4, lower right</param>
    /// <param name="normal">normal vector</param>
    /// <param name="color">color</param>
    /// <param name="texture">texture</param>
    public void AddTextureQuad(Vec3 v1, Vec3 v2, Vec3 v3, Vec3 v4, Vec3 normal, Color color, ITexture texture)
    {
        NlcArgException.NullThrow(nameof(texture), texture);

        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);

        _vertices.Add(v3);
        _vertices.Add(v2);
        _vertices.Add(v4);

        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);

        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);

        _normals.Add(normal);
        _normals.Add(normal);
        _normals.Add(normal);

        _normals.Add(normal);
        _normals.Add(normal);
        _normals.Add(normal);

        Bound2 crop = texture.Crop;
        _texCoords.Add(new Vec2(crop.X, crop.Y));
        _texCoords.Add(new Vec2(crop.X, crop.BottomY));
        _texCoords.Add(new Vec2(crop.RightX, crop.Y));

        _texCoords.Add(new Vec2(crop.RightX, crop.Y));
        _texCoords.Add(new Vec2(crop.X, crop.BottomY));
        _texCoords.Add(new Vec2(crop.RightX, crop.BottomY));
    }

    /// <summary>
    /// Adds a textured quadrangle.
    /// </summary>
    /// <param name="v1">vertex 1, upper left</param>
    /// <param name="v2">vertex 2, lower left</param>
    /// <param name="v3">vertex 3, upper right</param>
    /// <param name="v4">vertex 4, lower right</param>
    /// <param name="normal1">normal 1</param>
    /// <param name="normal2">normal 2</param>
    /// <param name="normal3">normal 3</param>
    /// <param name="normal4">normal 4</param>
    /// <param name="color">color</param>
    /// <param name="texture">texture</param>
    public void AddTextureQuad(Vec3 v1, Vec3 v2, Vec3 v3, Vec3 v4, Vec3 normal1, Vec3 normal2, Vec3 normal3, Vec3 normal4, Color color, ITexture texture)
    {
        NlcArgException.NullThrow(nameof(texture), texture);

        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);

        _vertices.Add(v3);
        _vertices.Add(v2);
        _vertices.Add(v4);

        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);

        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);

        _normals.Add(normal1);
        _normals.Add(normal2);
        _normals.Add(normal3);

        _normals.Add(normal3);
        _normals.Add(normal2);
        _normals.Add(normal4);

        Bound2 crop = texture.Crop;
        _texCoords.Add(new Vec2(crop.X, crop.Y));
        _texCoords.Add(new Vec2(crop.X, crop.BottomY));
        _texCoords.Add(new Vec2(crop.RightX, crop.Y));

        _texCoords.Add(new Vec2(crop.RightX, crop.Y));
        _texCoords.Add(new Vec2(crop.X, crop.BottomY));
        _texCoords.Add(new Vec2(crop.RightX, crop.BottomY));
    }

    /// <summary>
    /// Clears the whole vertices.
    /// </summary>
    public void Clear()
    {
        _vertices.Clear();
        _colors.Clear();
        _normals.Clear();
        _texCoords.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void FirstGenerateBuffer()
    {
        Vec3[] v = NlcHelper.ExtractArray(_vertices);
        Color[] c = NlcHelper.ExtractArray(_colors);
        Vec3[] n = NlcHelper.ExtractArray(_normals);
        Vec2[] t = NlcHelper.ExtractArray(_texCoords);

        _vertexCount = _vertices.Count;
        _buffer = new ObjectBuffer(v, _vertices.Count, c, _colors.Count, n, _normals.Count, t, _texCoords.Count, _primitive);
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void Rebuffer()
    {
        Vec3[] v = NlcHelper.ExtractArray(_vertices);
        Color[] c = NlcHelper.ExtractArray(_colors);
        Vec3[] n = NlcHelper.ExtractArray(_normals);
        Vec2[] t = NlcHelper.ExtractArray(_texCoords);

        _vertexCount = _vertices.Count;
        _buffer.Rebuffer(v, _vertices.Count, c, _colors.Count, n, _normals.Count, t, _texCoords.Count);
    }

    /// <summary>
    /// Creates a buffer with mapping.
    /// </summary>
    public void DoCreateBufferMapped()
    {
        if (_buffer is null)
            return;
        Vec3[] v = NlcHelper.ExtractArray(_vertices);
        Color[] c = NlcHelper.ExtractArray(_colors);
        Vec3[] n = NlcHelper.ExtractArray(_normals);
        Vec2[] t = NlcHelper.ExtractArray(_texCoords);

        _vertexCount = _vertices.Count;
        _buffer.RebufferMapping(v, _vertices.Count, c, _colors.Count, n, _normals.Count, t, _texCoords.Count);
    }
}
