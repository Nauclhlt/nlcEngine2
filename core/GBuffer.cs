namespace nlcEngine;

internal sealed class GBuffer : IDisposable
{
    bool _disposed = false;
    int _gBuffer;
    int _gPosition;
    int _gNormal;
    int _gColorSpec;
    int _rboDepth;

    public int Name => _gBuffer;
    public int GPosition => _gPosition;
    public int GNormal => _gNormal;
    public int GColorSpec => _gColorSpec;
    public int RBODepth => _rboDepth;

    public GBuffer()
    {
        _gBuffer = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _gBuffer);

        _gPosition = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _gPosition);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _gPosition, 0);


        _gNormal = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _gNormal);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, _gNormal, 0);


        _gColorSpec = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _gColorSpec);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, _gColorSpec, 0);


        DrawBuffersEnum[] attachments = new DrawBuffersEnum[] { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1, DrawBuffersEnum.ColorAttachment2 };
        GL.DrawBuffers(3, attachments);

        _rboDepth = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rboDepth);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, _rboDepth);
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        ResourceCollector.Add(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteBuffer(_gBuffer);
            GL.DeleteTexture(_gPosition);
            GL.DeleteTexture(_gNormal);
            GL.DeleteTexture(_gColorSpec);

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~GBuffer()
    {
        Dispose(false);
    }
}