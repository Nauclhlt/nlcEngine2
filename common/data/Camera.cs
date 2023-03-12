namespace nlcEngine;

/// <summary>
/// Represents a camera.
/// </summary>
public struct Camera
{
    Vec3 _position;
    Vec3 _target;
    Vec3 _up;

    /// <summary>
    /// Gets or sets the position of the camera.
    /// </summary>
    public Vec3 Position
    {
        get => _position;
        set => _position = value;
    }

    /// <summary>
    /// Gets or sets the target point of the camera.
    /// </summary>
    public Vec3 Target
    {
        get => _target;
        set => _target = value;
    }

    /// <summary>
    /// Gets or sets the up vector of the camera.
    /// </summary>
    public Vec3 Up
    {
        get => _up;
        set => _up = value;
    }

    internal Matrix4 CreateViewMatrix()
    {
        return Matrix4.LookAt(new Vector3(_position.X, _position.Y, _position.Z),
                              new Vector3(_target.X, _target.Y, _target.Z),
                              new Vector3(_up.X, _up.Y, _up.Z));
    }
}