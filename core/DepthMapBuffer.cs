namespace nlcEngine;

internal sealed class DepthMapBuffer : IDisposable, INamed
{
    bool _disposed = false;
    int _name;
    int _texture;
    int _width;
    int _height;

    public int Framebuffer => _name;
    public int MapTexture => _texture;
    public int Width => _width;
    public int Height => _height;

    public DepthMapBuffer(int width, int height)
    {
        _width = width;
        _height = height;

        _name = GL.GenFramebuffer();
        _texture = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, _texture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
        float[] borderColor = new float[] { 1, 1, 1, 1 };
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _name);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, _texture, 0);
        GL.DrawBuffer(DrawBufferMode.None);
        GL.ReadBuffer(ReadBufferMode.None);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        ResourceCollector.Add(this);
    }

    public int Name => _name;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteFramebuffer(_name);
            GL.DeleteTexture(_texture);

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DepthMapBuffer()
    {
        Dispose(false);
    }
}