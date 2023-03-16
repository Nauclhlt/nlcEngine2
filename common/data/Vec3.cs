namespace nlcEngine;

/// <summary>
/// A 3-dementional vector represented with 3 float values.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size=12)]
public struct Vec3
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
    /// The Z element.
    /// </summary>
    [FieldOffset(8)]
    public float Z;

    /// <summary>
    /// Initializes a new structure with the X, Y, and Z elements.
    /// </summary>
    /// <param name="x">X element</param>
    /// <param name="y">Y element</param>
    /// <param name="z">Z element</param>
    public Vec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Initializes a new structure with the X, Y, and Z elements as the same value.
    /// </summary>
    /// <param name="d">value for XYZ elements</param>
    public Vec3(float d)
    {
        X = d;
        Y = d;
        Z = d;
    }

    /// <summary>
    /// Initializes a new structure with the X and Y element by the 2d vector, and the Z element by the float value.
    /// </summary>
    /// <param name="vec">vector</param>
    /// <param name="z">Z element</param>
    public Vec3(Vec2 vec, float z)
    {
        X = vec.X;
        Y = vec.Y;
        Z = z;
    }

    /// <summary>
    /// Returns the length of the vector.
    /// </summary>
    /// <returns>length of the vector</returns>
    public float Length()
    {
        return MathF.Sqrt(X * X + Y * Y + Z * Z);
    }

    /// <summary>
    /// Normalizes the vector.
    /// </summary>
    public void Normalize()
    {
        Vec3 n = Normalized();
        X = n.X;
        Y = n.Y;
        Z = n.Z;
    }

    /// <summary>
    /// Gets the normalized vector.
    /// </summary>
    /// <returns>normalized vector</returns>
    public Vec3 Normalized()
    {
        float len = Length();
        if (len == 0f)
            return this;
        return new Vec3(X / len, Y / len, Z / len);
    }

    /// <summary>
    /// Returns the vector rotated around the X axis.
    /// </summary>
    /// <param name="angle">rotation angle</param>
    /// <returns>rotated vector</returns>
    public Vec3 RotateX(float angle)
    {
        angle = (angle / 180f) * MathF.PI;

        return new Vec3(
            X,
            Y * MathF.Cos(angle) - Z * MathF.Sin(angle),
            Y * MathF.Sin(angle) + Z * MathF.Cos(angle)
        );
    }

    /// <summary>
    /// Returns the vector rotated around the Y axis.
    /// </summary>
    /// <param name="angle">rotation angle</param>
    /// <returns>rotated vector</returns>
    public Vec3 RotateY(float angle)
    {
        angle = (angle / 180f) * MathF.PI;

        return new Vec3(
            X * MathF.Cos(angle) - Z * MathF.Sin(angle),
            Y,
            X * MathF.Sin(angle) + Z * MathF.Cos(angle)
        );
    }

    /// <summary>
    /// Returns the vector rotated around the Z axis.
    /// </summary>
    /// <param name="angle">rotation angle</param>
    /// <returns>rotated vector</returns>
    public Vec3 RotateZ(float angle)
    {
        angle = (angle / 180f) * MathF.PI;

        return new Vec3(
            X * MathF.Cos(angle) - Y * MathF.Sin(angle),
            X * MathF.Sin(angle) + Y * MathF.Cos(angle),
            Z
        );
    }

    /// <summary>
    /// Adds the 2 vectors.
    /// </summary>
    /// <param name="left">left</param>
    /// <param name="right">right</param>
    /// <returns>resulting vector</returns>
    public static Vec3 operator +(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    /// <summary>
    /// Gets the minus vector.
    /// </summary>
    /// <param name="vec">vector</param>
    /// <returns>resulting vector</returns>
    public static Vec3 operator -(Vec3 vec)
    {
        return new Vec3(-vec.X, -vec.Y, -vec.Z);
    }

    /// <summary>
    /// Subtracts the 2 vectors.
    /// </summary>
    /// <param name="left">left</param>
    /// <param name="right">right</param>
    /// <returns>resulting vector</returns>
    public static Vec3 operator -(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    /// Returns the scalar product.
    /// </summary>
    /// <param name="left">vector</param>
    /// <param name="scalar">scalar</param>
    /// <returns>resulting vector</returns>
    public static Vec3 operator *(Vec3 left, float scalar)
    {
        return new Vec3(left.X * scalar, left.Y * scalar, left.Z * scalar);
    }

    /// <summary>
    /// Returns the scalar division.
    /// </summary>
    /// <param name="left">vector</param>
    /// <param name="scalar">scalar</param>
    /// <returns>resulting vector</returns>
    public static Vec3 operator /(Vec3 left, float scalar)
    {
        return new Vec3(left.X / scalar, left.Y / scalar, left.Z / scalar);
    }
}