namespace nlcEngine;

internal sealed class LightStorageBuffer : IDisposable, INamed
{
    int _name;
    bool _disposed = false;

    public int Name => _name;

    public LightStorageBuffer()
    {
        _name = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _name);
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);

        ResourceCollector.Add(this);
    }

    public void Buffer(List<Light> lights)
    {
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _name);

        float[] data = new float[12 * lights.Count];
        for (int i = 0; i < lights.Count; i++)
        {
            Light light = lights[i];

            int head = i * 12;
            data[head] = light.DiffuseColor.Rf;
            data[head + 1] = light.DiffuseColor.Gf;
            data[head + 2] = light.DiffuseColor.Bf;
            data[head + 3] = light.SpecularColor.Rf;
            data[head + 4] = light.SpecularColor.Gf;
            data[head + 5] = light.SpecularColor.Bf;
            data[head + 6] = light.Position.X;
            data[head + 7] = light.Position.Y;
            data[head + 8] = light.Position.Z;
            data[head + 9] = light.Radius;
            data[head + 10] = light.Attenuation;
            data[head + 11] = light.Intensity;
        }

        
        IntPtr size = sizeof(float) * data.Length;

        
        GL.BufferData(BufferTarget.ShaderStorageBuffer, size, data, BufferUsageHint.DynamicDraw);


        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteBuffer(_name);

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~LightStorageBuffer()
    {
        Dispose(false);
    }
}