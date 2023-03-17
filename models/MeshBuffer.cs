namespace nlcEngine.Models;

internal sealed class MeshBuffer : IDisposable
{
    int _vertexArray;
    int _vertexBuffer;
    int _indexBuffer;
    Texture _texture;
    bool _disposed = false;
    Mesh _mesh;

    public bool HasTexture => _texture is not null;


    public MeshBuffer(Mesh mesh)
    {
        _mesh = mesh;

        _vertexArray = GL.GenVertexArray();
        _vertexBuffer = GL.GenBuffer();
        _indexBuffer = GL.GenBuffer();

        if (mesh.HasTexture)
        {
            
        }

        ResourceCollector.Add(this);
    }

    public unsafe void CreateBuffer()
    {
        int vertexSize = sizeof(Vertex);
        int uintSize = sizeof(uint);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexSize * _mesh.Vertices.Length), _mesh.Vertices, BufferUsageHint.DynamicDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        // TODO: implement index buffer
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);
        GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(uintSize * _mesh.Indices.Length), _mesh.Indices, BufferUsageHint.DynamicDraw);

        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        GL.BindVertexArray(_vertexArray);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("Position"));

        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, vertexSize, Marshal.OffsetOf<Vertex>("Color"));

        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("Normal"));

        GL.EnableVertexAttribArray(3);
        GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("TexCoords"));

        GL.EnableVertexAttribArray(4);
        GL.VertexAttribIPointer(4, 4, VertexAttribIntegerType.Int, vertexSize, Marshal.OffsetOf<Vertex>("BoneIds0"));

        GL.EnableVertexAttribArray(5);
        GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("Weights0"));

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);

        
    }

    public void JustCallRender()
    {
        
        GL.BindVertexArray(_vertexArray);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);

        GL.DrawElements(PrimitiveType.Triangles, _mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);

        // OpenTK.Graphics.OpenGL4.ErrorCode code = GL.GetError();
        // Console.WriteLine(code);

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