namespace nlcEngine;

/// <summary>
/// Wraps an OpenGL texture.
/// </summary>
public sealed class Texture : ITexture, IDisposable
{
    int _name;
    int _width;
    int _height;
    Bound2 _crop;
    bool _disposed = false;

    /// <summary>
    /// Gets the texture name.
    /// </summary>
    public int Name => _name;
    
    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width => _width;

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height => _height;

    /// <summary>
    /// Gets the cropping bounds.
    /// </summary>
    public Bound2 Crop => _crop;

    /// <summary>
    /// Loads the image file.
    /// </summary>
    /// <param name="filename">file name</param>
    public Texture(string filename)
    {
        NlcHelper.InThrow();
        NlcArgException.NullThrow(nameof(filename), filename);
        NlcHelper.FileThrow(filename);

        using var imgSharpImage = SixLabors.ImageSharp.Image.Load<Rgba32>(filename);
        FromImage(imgSharpImage);

        ResourceCollector.Add(this);
    }

    /// <summary>
    /// Loads the image from the stream.
    /// </summary>
    /// <param name="image">image stream</param>
    public Texture(Stream image)
    {
        NlcHelper.InThrow();
        NlcArgException.NullThrow(nameof(image), image);

        using var imgSharpImage = SixLabors.ImageSharp.Image.Load<Rgba32>(image);
        FromImage(imgSharpImage);

        ResourceCollector.Add(this);
    }

    private void FromImage(SixLabors.ImageSharp.Image<Rgba32> image)
    {
        byte[] pixelData = new byte[4 * image.Width * image.Height];
        image.CopyPixelDataTo(pixelData);

        _name = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, _name);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixelData);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        _width = image.Width;
        _height = image.Height;
        _crop = new Bound2(0, 0, 1, 1);
    }

    /// <summary>
    /// Sets the cropping bounds.
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    /// <param name="w">width</param>
    /// <param name="h">height</param>
    public void SetCrop(float x, float y, float w, float h)
    {
        NlcHelper.DispThrow(_disposed);
        NlcArgException.LeftZeroThrow(nameof(w), w);
        NlcArgException.LeftZeroThrow(nameof(h), h);

        _crop = new Bound2(x / _width, y / _height, w / _width, h / _height);
    }

    /// <summary>
    /// Gets the cropped texture.
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    /// <param name="w">width</param>
    /// <param name="h">height</param>
    public Subtexture GetCropped(float x, float y, float w, float h)
    {
        NlcHelper.DispThrow(_disposed);
        NlcArgException.LeftZeroThrow(nameof(w), w);
        NlcArgException.LeftZeroThrow(nameof(h), h);

        Bound2 c = new Bound2(x / _width, y / _height, w / _width, h / _height);
        return new Subtexture(_name, (int)c.Width, (int)c.Height, c);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteTexture(_name);

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the texture.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// A destructor.
    /// </summary>
    ~Texture()
    {
        Dispose(false);
    }
}