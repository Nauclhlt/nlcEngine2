namespace nlcEngine.Models;

internal sealed class MeshBuffer : IDisposable
{
    int _vertexArray;
    int _vertexBuffer;
    int _normalBuffer;
    int _texCoordBuffer;
    int _indexBuffer;
    bool _disposed = false;
    Mesh _mesh;


    public MeshBuffer(Mesh mesh)
    {
        _mesh = mesh;

        _vertexArray = GL.GenVertexArray();
        _vertexBuffer = GL.GenBuffer();
        _normalBuffer = GL.GenBuffer();
        _texCoordBuffer = GL.GenBuffer();
        _indexBuffer = GL.GenBuffer();
    }

    public unsafe void CreateBuffer()
    {
        int vec3size = sizeof(Vec3);
        int vec2size = sizeof(Vec2);
        int uintSize = sizeof(uint);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * _mesh.Vertices.Length), _mesh.Vertices, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec3size * _mesh.Normals.Length), _mesh.Normals, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vec2size * _mesh.TexCoords.Length), _mesh.TexCoords, BufferUsageHint.DynamicDraw);

        // TODO: implement index buffer
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(uintSize * _mesh.Indices.Length), _mesh.Indices, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _normalBuffer);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(2);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindVertexArray(0);
    }

    public void JustCallRender()
    {
        GL.BindVertexArray(_vertexArray);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);

        GL.DrawElements(PrimitiveType.Triangles, _mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteVertexArray(_vertexArray);
            GL.DeleteBuffer(_vertexBuffer);
            GL.DeleteBuffer(_normalBuffer);
            GL.DeleteBuffer(_texCoordBuffer);
            GL.DeleteBuffer(_indexBuffer);

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~MeshBuffer()
    {
        Dispose(false);
    }
}