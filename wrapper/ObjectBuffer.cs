namespace nlcEngine;

/// <summary>
/// Provides the wrapper for an OpenGL vertex array object, to render RenderObjects.
/// </summary>
public sealed class ObjectBuffer : IDisposable, INamed
{
    int _vertexBuffer;
    int _colorBuffer;
    int _normalBuffer;
    int _texCoordBuffer;
    int _vertexArray;
    int _vertexCount;
    Primitive _primitive;
    bool _disposed = false;

    /// <summary>
    /// Gets the name of vertex array.
    /// </summary>
    public int Name => _vertexArray;

    private ObjectBuffer()
    {
        
    }

    internal unsafe ObjectBuffer(Vec3[] v, Color[] c, Vec3[] n, Vec2[] t, Primitive primitive)
    {
        _vertexBuffer = GL.GenBuffer();
        _colorBuffer = GL.GenBuffer();
        _normalBuffer = GL.GenBuffer();
        _texCoordBuffer = GL.GenBuffer();
        _vertexCount = v.Length;
        _primitive = primitive;

        int vec3size = sizeof(Vec3);
        int colorSize = sizeof(Color);
        int vec2size = sizeof(Vec2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArray);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * v.Length), v, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colorSize * c.Length), c, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * n.Length), n, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec2size * t.Length), t, BufferUsageHint.StaticDraw);


        _vertexArray = GL.GenVertexArray();

        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(3);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);
    }

    /// <summary>
    /// Calls DrawArrays with the vertex array binded.
    /// </summary>
    public void JustCallRender()
    {
        GL.BindVertexArray(_vertexArray);

        GL.DrawArrays((PrimitiveType)_primitive, 0, _vertexCount);

        GL.BindVertexArray(0);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteBuffer(_vertexBuffer);
            GL.DeleteBuffer(_colorBuffer);
            GL.DeleteBuffer(_normalBuffer);
            GL.DeleteBuffer(_texCoordBuffer);
            GL.DeleteVertexArray(_vertexArray);

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the buffer.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// A destructor.
    /// </summary>
    ~ObjectBuffer()
    {
        Dispose(false);
    }
}