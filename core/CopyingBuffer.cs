namespace nlcEngine;

internal sealed class CopyingBuffer
{
    int _vao;
    int _vert;
    int _tex;

    private CopyingBuffer( float[] vs, float[] ts )
    {
        _vert = GL.GenBuffer();
        _tex = GL.GenBuffer();

        int fsize = sizeof(float);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vert);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(fsize * vs.Length), vs, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _tex);
        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(fsize * ts.Length), ts, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        _vao = GL.GenVertexArray();

        GL.BindVertexArray(_vao);

        GL.EnableVertexAttribArray(0);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vert);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.EnableVertexAttribArray(1);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _tex);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);

        NlcEngineGame.EnsureRelease(() =>
        {
            GL.DeleteBuffer(_vert);
            GL.DeleteBuffer(_tex);
            GL.DeleteVertexArray(_vao);
        });
    }

    public static CopyingBuffer CreateBuffer()
    {
        float[] vs = new float[]
        {
            -1f, 1f,
            -1f, -1f,
            1f, 1f,
            1f, -1f
        };

        float[] ts = new float[]
        {
            0f, 0f,
            0f, 1f,
            1f, 0f,
            1f, 1f
        };

        return new CopyingBuffer(vs, ts);
    }

    public void Draw()
    {
        GL.BindVertexArray(_vao);

        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);

        GL.BindVertexArray(0);
    }
}