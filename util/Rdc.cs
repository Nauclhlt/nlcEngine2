namespace nlcEngine;

internal class Rdc
{
    private static int _vao = -1;
    private static int[] _vbo = Enumerable.Repeat(-1, 32).ToArray();
    private static int[] _size = Enumerable.Repeat(2, 32).ToArray();
    private static int[] _memSize = Enumerable.Repeat(0, 32).ToArray();
    private static VertexAttribPointerType[] _type = Enumerable.Repeat(VertexAttribPointerType.Float, 32).ToArray();
    private static List<int> _buffered = new List<int>();

    public static void Initialize()
    {
        _vao = GL.GenVertexArray();

        NlcEngineGame.EnsureRelease(() =>
        {
            Destroy();
        });
    }

    public static void VertexAttribPointer<T>(int index, int size, VertexAttribPointerType type, bool normalized, int stride, T[] array) where T : struct
    {
        if (_vbo[index] == -1)
        {
            //Create new vbo if specified index in _vbo is null
            _vbo[index] = GL.GenBuffer();
        }

        int ms = 0;
        if (type == VertexAttribPointerType.Float) ms = sizeof(float);
        else if (type == VertexAttribPointerType.Double) ms = sizeof(double);
        else if (type == VertexAttribPointerType.Byte) ms = sizeof(byte);
        else if (type == VertexAttribPointerType.Int) ms = sizeof(int);
        else if (type == VertexAttribPointerType.Short) ms = sizeof(short);
        _size[index] = size;
        _type[index] = type;
        _memSize[index] = ms;

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[index]);

        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(ms * array.Length), array, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        _buffered.Add(index);
    }

    public static void DrawArrays(PrimitiveType type, int first, int count)
    {
        Bind();
        GL.BindVertexArray(_vao);
        GL.DrawArrays(type, first, count);
        _buffered.Clear();
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    public static void DrawElements<T>(PrimitiveType mode, int count, DrawElementsType type, T[] indices) where T : struct
    {
        Bind();
        GL.BindVertexArray(_vao);
        GL.DrawElements(mode, count, type, indices);
        _buffered.Clear();
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    private static void Bind()
    {
        GL.BindVertexArray(_vao);

        for (int i = 0; i < _buffered.Count; i++)
        {
            int index = _buffered[i];
            GL.EnableVertexAttribArray(index);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo[index]);
            GL.VertexAttribPointer(index, _size[index], _type[index], false, 0, 0);
        }
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        GL.BindVertexArray(0);
    }

    public static void Destroy()
    {
        for (int i = 0; i < _vbo.Length; i++)
        {
            if (_vbo[i] != -1)
            {
                GL.DeleteBuffer(_vbo[i]);
            }
        }

        GL.DeleteVertexArray(_vao);
    }
}