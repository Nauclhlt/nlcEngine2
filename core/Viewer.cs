namespace nlcEngine;

/// <summary>
/// Provides settings of viewer.
/// </summary>
public static class Viewer
{
    private static Matrix4 _projectionMatrix;
    private static float _fieldOfView = 90f;
    private static float _depthNear = 0.5f;
    private static float _depthFar = 1200f;

    internal static Matrix4 ProjectionMatrix => _projectionMatrix;

    internal static void CreateProjection()
    {
        _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((_fieldOfView / 180f) * MathF.PI, NlcEngineGame.Profile.BufferWidth / NlcEngineGame.Profile.BufferHeight, _depthNear, _depthFar);
    }

    /// <summary>
    /// Sets the FOV of Y.
    /// </summary>
    /// <param name="degrees">angle in degrees</param>
    public static void SetFieldOfViewY(float degrees)
    {
        _fieldOfView = degrees;

        CreateProjection();
    }

    /// <summary>
    /// Sets the depth near and far.
    /// </summary>
    /// <param name="near">depth near</param>
    /// <param name="far">depth far</param>
    public static void SetDepthNearFar(float near, float far)
    {
        _depthNear = near;
        _depthFar = far;

        CreateProjection();
    }
}