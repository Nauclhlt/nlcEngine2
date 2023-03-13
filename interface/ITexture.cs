namespace nlcEngine;

/// <summary>
/// Provides the common and uniform information for OpenGL texture wrappers.
/// </summary>
public interface ITexture : INamed
{
    /// <summary>
    /// Gets the width of the texture.
    /// </summary>
    public int Width { get; }
    /// <summary>
    /// Gets the height of the texture.
    /// </summary>
    public int Height { get; }
    /// <summary>
    /// Gets the cropping bounds of the texture.
    /// </summary>
    public Bound2 Crop { get; }
}