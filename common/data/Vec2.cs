namespace nlcEngine;

/// <summary>
/// A 2-dementional vector represented with 2 float values.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size=8)]
public struct Vec2
{
    /// <summary>
    /// The X element.
    /// </summary>
    [FieldOffset(0)]
    public float X;
    /// <summary>
    /// The Y element.
    /// </summary>
    [FieldOffset(4)]
    public float Y;

    /// <summary>
    /// Initializes a new structure with the X, and Y elements.
    /// </summary>
    /// <param name="x">X element</param>
    /// <param name="y">Y element</param>
    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Initializes a new structure with the X, and Y elements as the same value.
    /// </summary>
    /// <param name="d">value for XY elements</param>
    public Vec2(float d)
    {
        X = d;
        Y = d;
    }

    /// <summary>
    /// Returns the length of the vector.
    /// </summary>
    /// <returns>length of the vector</returns>
    public float Length()
    {
        return MathF.Sqrt(X * X + Y * Y);
    }

    /// <summary>
    /// Normalizes the vector.
    /// </summary>
    public void Normalize()
    {
        Vec2 n = Normalized();
        X = n.X;
        Y = n.Y;
    }

    /// <summary>
    /// Returns the normalised vector.
    /// </summary>
    /// <returns>normalized vector</returns>
    public Vec2 Normalized()
    {
        float len = Length();
        if (len == 0f)
            return this;
        return new Vec2(X / len, Y / len);
    }

    /// <summary>
    /// Returns the vector rotated.
    /// </summary>
    /// <param name="angle">rotation angle</param>
    /// <returns>rotated vector</returns>
    public Vec2 Rotate(float angle)
    {
        angle = (angle / 180f) * MathF.PI;

        return new Vec2(
            X * MathF.Cos(angle) - Y * MathF.Sin(angle),
            X * MathF.Sin(angle) + Y * MathF.Cos(angle)
        );
    }
}