namespace nlcEngine;

/// <summary>
/// Represents a 2-dementional bounding box with float values.
/// </summary>
public struct Bound2
{
    float _x;
    float _y;
    float _width;
    float _height;

    /// <summary>
    /// Gets or sets the X.
    /// </summary>
    public float X
    {
        get => _x;
        set => _x = value;
    }

    /// <summary>
    /// Gets or sets the Y.
    /// </summary>
    public float Y
    {
        get => _y;
        set => _y = value;
    }

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    public float Width
    {
        get => _width;
        set => _width = value;
    }

    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    public float Height
    {
        get => _height;
        set => _height = value;
    }

    /// <summary>
    /// Gets the right X.
    /// </summary>
    public float RightX => _x + _width;

    /// <summary>
    /// Gets the bottom Y.
    /// </summary>
    public float BottomY => _y + _height;

    /// <summary>
    /// Initializes a new structure with the position and size.
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    /// <param name="width">width</param>
    /// <param name="height">height</param>
    public Bound2( float x, float y, float width, float height )
    {
        _x = x;
        _y = y;
        _width = width;
        _height = height;
    }
}