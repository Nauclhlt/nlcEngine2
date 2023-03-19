namespace nlcEngine;

/// <summary>
/// Contains an editable image on the memory.
/// </summary>
public sealed class MemoryImage : IDisposable
{
    private SixLabors.ImageSharp.Image<Rgba32> _image;
    private bool _disposed = false;

    internal SixLabors.ImageSharp.Image<Rgba32> Image => _image;

    /// <summary>
    /// Creates a new instance from the file name.
    /// </summary>
    /// <param name="filename">file name</param>
    public MemoryImage(string filename)
    {
        NlcArgException.NullThrow(nameof(filename), filename);
        NlcHelper.FileThrow(filename);

        _image = SixLabors.ImageSharp.Image.Load<Rgba32>(filename);
    }

    /// <summary>
    /// Creates a new instance from the stream.
    /// </summary>
    /// <param name="stream">stream that contains image data</param>
    public MemoryImage(Stream stream)
    {
        NlcArgException.NullThrow(nameof(stream), stream);


        _image = SixLabors.ImageSharp.Image.Load<Rgba32>(stream);
    }

    /// <summary>
    /// Creates a new texture from this image's data.
    /// </summary>
    /// <returns>created texture</returns>
    public Texture CreateTexture()
    {
        NlcHelper.DispThrow(_disposed);

        return new Texture(_image);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _image.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by this image.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// A destructor.
    /// </summary>
    ~MemoryImage()
    {
        Dispose(false);
    }
}