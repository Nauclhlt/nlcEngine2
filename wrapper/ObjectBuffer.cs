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
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * v.Length), v, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colorSize * c.Length), c, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * n.Length), n, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec2size * t.Length), t, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        _vertexArray = GL.GenVertexArray();

        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(3);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);

        ResourceCollector.Add(this);
    }

    internal unsafe ObjectBuffer(Vec3[] v, int vlen, Color[] c, int clen, Vec3[] n, int nlen, Vec2[] t, int tlen, Primitive primitive)
    {
        _vertexBuffer = GL.GenBuffer();
        _colorBuffer = GL.GenBuffer();
        _normalBuffer = GL.GenBuffer();
        _texCoordBuffer = GL.GenBuffer();
        _vertexCount = vlen;
        _primitive = primitive;

        int vec3size = sizeof(Vec3);
        int colorSize = sizeof(Color);
        int vec2size = sizeof(Vec2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * vlen), v, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colorSize * clen), c, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * nlen), n, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec2size * tlen), t, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        _vertexArray = GL.GenVertexArray();

        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(3);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);

        ResourceCollector.Add(this);
    }

    internal unsafe void Rebuffer(Vec3[] v, Color[] c, Vec3[] n, Vec2[] t)
    {
        _vertexCount = v.Length;

        int vec3size = sizeof(Vec3);
        int colorSize = sizeof(Color);
        int vec2size = sizeof(Vec2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * v.Length), v, BufferUsageHint.StaticDraw);
        //GL.BufferSubData(BufferTarget.ArrayBuffer, 0, new IntPtr(vec3size * v.Length), v);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colorSize * c.Length), c, BufferUsageHint.StaticDraw);
        //GL.BufferSubData(BufferTarget.ArrayBuffer, 0, new IntPtr(colorSize * c.Length), c);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * n.Length), n, BufferUsageHint.StaticDraw);
        //GL.BufferSubData(BufferTarget.ArrayBuffer, 0, new IntPtr(vec3size * n.Length), n);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec2size * t.Length), t, BufferUsageHint.StaticDraw);
        //GL.BufferSubData(BufferTarget.ArrayBuffer, 0, new IntPtr(vec2size * t.Length), t);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(3);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);
    }

    internal unsafe void Rebuffer(Vec3[] v, int vlen, Color[] c, int clen, Vec3[] n, int nlen, Vec2[] t, int tlen)
    {
        _vertexCount = vlen;

        int vec3size = sizeof(Vec3);
        int colorSize = sizeof(Color);
        int vec2size = sizeof(Vec2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * v.Length), v, BufferUsageHint.StaticDraw);
        //GL.BufferSubData(BufferTarget.ArrayBuffer, 0, new IntPtr(vec3size * v.Length), v);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colorSize * c.Length), c, BufferUsageHint.StaticDraw);
        //GL.BufferSubData(BufferTarget.ArrayBuffer, 0, new IntPtr(colorSize * c.Length), c);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * n.Length), n, BufferUsageHint.StaticDraw);
        //GL.BufferSubData(BufferTarget.ArrayBuffer, 0, new IntPtr(vec3size * n.Length), n);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec2size * t.Length), t, BufferUsageHint.StaticDraw);
        //GL.BufferSubData(BufferTarget.ArrayBuffer, 0, new IntPtr(vec2size * t.Length), t);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(3);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);
    }

    internal unsafe void RebufferMapping(Vec3[] v, int vlen, Color[] c, int clen, Vec3[] n, int nlen, Vec2[] t, int tlen)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        IntPtr ptr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);

        int vsize = sizeof(Vec3) * vlen;
        int csize = sizeof(Color) * clen;
        int nsize = sizeof(Vec3) * nlen;
        int tsize = sizeof(Vec2) * tlen;

        IntPtr vptr = Marshal.UnsafeAddrOfPinnedArrayElement(v, 0);
        System.Buffer.MemoryCopy((void*)vptr, (void*)ptr, vsize, vsize);

        GL.UnmapBuffer(BufferTarget.ArrayBuffer);



        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        ptr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);

        IntPtr cptr = Marshal.UnsafeAddrOfPinnedArrayElement(c, 0);
        System.Buffer.MemoryCopy((void*)cptr, (void*)ptr, csize, csize);

        GL.UnmapBuffer(BufferTarget.ArrayBuffer);



        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        ptr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);

        IntPtr nptr = Marshal.UnsafeAddrOfPinnedArrayElement(n, 0);
        System.Buffer.MemoryCopy((void*)nptr, (void*)ptr, nsize, nsize);

        GL.UnmapBuffer(BufferTarget.ArrayBuffer);


        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        ptr = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);

        IntPtr tptr = Marshal.UnsafeAddrOfPinnedArrayElement(t, 0);
        System.Buffer.MemoryCopy((void*)tptr, (void*)ptr, tsize, tsize);

        GL.UnmapBuffer(BufferTarget.ArrayBuffer);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

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
        NlcHelper.InThrow();

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