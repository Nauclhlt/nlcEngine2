namespace nlcEngine;

/// <summary>
/// Provides a camera represented with X-axis rotation(pitch) and Y-axis rotation(yaw).
/// </summary>
public sealed class RotationCamera
{
    private float _yaw = 0f;
    private float _pitch = 0f;


    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public RotationCamera()
    {
    }

    /// <summary>
    /// Adds the delta value specified.
    /// </summary>
    /// <param name="yaw">yaw delta</param>
    /// <param name="pitch">pitch delta</param>
    public void Delta( float yaw, float pitch )
    {
        _yaw += yaw;
        _pitch += pitch;
        _yaw %= 360f;
        _pitch = Math.Clamp(_pitch, -89f, 89f);
    }

    /// <summary>
    /// Sets the target of the specified camera, based on the holding pitch and yaw values.
    /// </summary>
    /// <param name="camera">camera to modify</param>
    /// <returns>direction of the camera</returns>
    public Vec3 SetCameraTarget(ref Camera camera)
    {
        Vec3 position = camera.Position;
        Vec3 direction = new Vec3(0, 0, -1).RotateX(_pitch).RotateY(_yaw);

        camera.Target = position + direction;

        return direction;
    }

    /// <summary>
    /// Gets the direction vector in unit vector.
    /// </summary>
    /// <returns>direction</returns>
    public Vec3 GetDirection()
    {
        Vec3 direction = new Vec3(0, 0, -1).RotateX(_pitch).RotateY(_yaw);
        return direction;
    }
}