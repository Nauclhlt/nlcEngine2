namespace nlcEngine;

/// <summary>
/// Represents a camera.
/// </summary>
public struct Camera
{
    /// <summary>
    /// Camera position.
    /// </summary>
    public Vec3 Position;
    /// <summary>
    /// Camera target point.
    /// </summary>
    public Vec3 Target;
    /// <summary>
    /// Camera up vector.
    /// </summary>
    public Vec3 Up;
    internal Matrix4 CreateViewMatrix()
    {
        return Matrix4.LookAt(new Vector3(Position.X, Position.Y, Position.Z),
                              new Vector3(Target.X, Target.Y, Target.Z),
                              new Vector3(Up.X, Up.Y, Up.Z));
    }
}