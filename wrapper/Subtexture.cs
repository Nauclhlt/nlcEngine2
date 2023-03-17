namespace nlcEngine;

/// <summary>
/// Provides a texture wrapper that contains minimal information. <br />
/// Instances of subtextures can be created from only Texture class.
/// </summary>
public sealed class Subtexture : ITexture
{
    int _width;
    int _height;
    int _name;
    Bound2 _crop;

    /// <summary>
    /// Gets the width of the texture. Returns the cropping size if this subtexture is cropped.
    /// </summary>
    public int Width => _width;
    /// <summary>
    /// Gets the height of the texture. Returns the cropping size if this subtexture is cropped.
    /// </summary>
    public int Height => _height;
    /// <summary>
    /// Gets the texture name. Returns the original texture name if this subtexture is subset of a texture.
    /// </summary>
    public int Name => _name;
    /// <summary>
    /// Gets the cropping bounds.
    /// </summary>
    public Bound2 Crop => _crop;

    private Subtexture()
    {
        // hidden parameterless constructor
    }

    internal Subtexture(int name, int width, int height, Bound2 crop)
    {
        _name = name;
        _width = width;
        _height = height;
        _crop = crop;
    }
}