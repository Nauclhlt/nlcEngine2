namespace nlcEngine;

internal static class DefaultBuffer
{
    static int _framebuffer;
    static int _texture;

    public static int Framebuffer => _framebuffer;
    public static int Texture => _texture;

    public static void CreateBuffer( int width, int height )
    {
        _framebuffer = GL.GenFramebuffer();
        _texture = GL.GenTexture();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);
        GL.BindTexture(TextureTarget.Texture2D, _texture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _texture, 0);
        GL.BindTexture(TextureTarget.Texture2D, 0);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        NlcEngineGame.EnsureRelease(() =>
        {
            GL.DeleteFramebuffer(_framebuffer);
            GL.DeleteTexture(_texture);
        });
    }

    public static void Bind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);
    }
}