namespace nlcEngine;

/// <summary>
/// Represents a transformation.
/// </summary>
public struct Transform
{
    Vec3 _translate;
    Vec3 _rotate;
    float _rotationAngle;
    Vec3 _scale;

    /// <summary>
    /// Gets or sets the translate.
    /// </summary>
    public Vec3 Translate
    {
        get => _translate;
        set => _translate = value;
    }

    /// <summary>
    /// Gets or sets the rotate.
    /// </summary>
    public Vec3 Rotate
    {
        get => _rotate;
        set => _rotate = value;
    }

    /// <summary>
    /// Gets or sets the rotation angle.
    /// </summary>
    public float RotationAngle
    {
        get => _rotationAngle;
        set => _rotationAngle = value;
    }

    /// <summary>
    /// Gets or sets the scale.
    /// </summary>
    public Vec3 Scale
    {
        get => _scale;
        set => _scale = value;
    }

    /// <summary>
    /// Initializes a new structure with the parameters.
    /// </summary>
    /// <param name="tx"></param>
    /// <param name="ty"></param>
    /// <param name="tz"></param>
    /// <param name="rx"></param>
    /// <param name="ry"></param>
    /// <param name="rz"></param>
    /// <param name="a"></param>
    /// <param name="sx"></param>
    /// <param name="sy"></param>
    /// <param name="sz"></param>
    public Transform(float tx, float ty, float tz, float rx, float ry, float rz, float a, float sx, float sy, float sz)
    {
        _translate = new Vec3(tx, ty, tz);
        _rotate = new Vec3(rx, ry, rz);
        _rotationAngle = a;
        _scale = new Vec3(sx, sy, sz);
    }

    /// <summary>
    /// Gets the translated transform.
    /// </summary>
    /// <param name="x">translation X</param>
    /// <param name="y">translation Y</param>
    /// <param name="z">translation Z</param>
    /// <returns>translated transform</returns>
    public static Transform Translated(float x, float y, float z)
    {
        Transform transform = new Transform();
        transform._translate = new Vec3(x, y, z);
        return transform;
    }

    /// <summary>
    /// Gets the rotated transform
    /// </summary>
    /// <param name="x">rotation X</param>
    /// <param name="y">rotation Y</param>
    /// <param name="z">rotation Z</param>
    /// <param name="angle">rotation angle</param>
    /// <returns>rotated transform</returns>
    public static Transform Rotated(float x, float y, float z, float angle)
    {
        Transform transform = new Transform();
        transform._rotate = new Vec3(x, y, z);
        transform._rotationAngle = angle;
        return transform;
    }

    /// <summary>
    /// Gets the scaled transform.
    /// </summary>
    /// <param name="x">scale X</param>
    /// <param name="y">scale Y</param>
    /// <param name="z">scale Z</param>
    /// <returns>scaled transform</returns>
    public static Transform Scaled(float x, float y, float z)
    {
        Transform transform = new Transform();
        transform._scale = new Vec3(x, y, z);
        return transform;
    }

    internal Matrix4 GetModelMatrix()
    {
        Matrix4 m = Matrix4.Identity;

        if (_translate.X != 0f || _translate.Y != 0f || _translate.Z != 0f)
        {
            m.M14 = _translate.X;
            m.M24 = _translate.Y;
            m.M34 = _translate.Z;
        }

        if (_scale.X != 0f || _scale.Y != 0f || _scale.Z != 0f)
        {
            m.M11 = _scale.X;
            m.M22 = _scale.Y;
            m.M33 = _scale.Z;
        }

        if (_rotationAngle != 0f)
        {
            Matrix4 r = GetRotationMatrix();
            m *= r;
        }

        return m;
    }

    private Matrix4 GetRotationMatrix()
    {
        Matrix4 m = Matrix4.Identity;
        float r = _rotationAngle * (MathF.PI / 180f);
        float c = MathF.Cos(r);
        float s = MathF.Sin(r);
        float x = _rotate.X;
        float y = _rotate.Y;
        float z = _rotate.Z;

        m.M11 = x * x * (1 - c) + c;
        m.M12 = x * y * (1 - c) - z * s;
        m.M13 = x * z * (1 - c) + y * s;

        m.M21 = y * x * (1 - c) + z * s;
        m.M22 = y * y * (1 - c) + c;
        m.M23 = y * z * (1 - c) - x * s;

        m.M31 = x * z * (1 - c) - y * s;
        m.M32 = y * z * (1 - c) + x * s;
        m.M33 = z * z * (1 - c) + c;

        return m;
    }
}