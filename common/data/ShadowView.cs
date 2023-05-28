namespace nlcEngine;

/// <summary>
/// Contains the settings of Shadow Mapping, which is used to make shadows of objects.
/// </summary>
public sealed class ShadowView
{
    float _shadowIntensity;
    Camera _lightPerspective;
    float _perspectiveScale;
    float _farPlane;
    float _nearPlane;

    /// <summary>
    /// Gets or sets the intensity of the shadows.
    /// </summary>
    public float ShadowIntensity
    {
        get => _shadowIntensity;
        set => _shadowIntensity = value;
    }

    /// <summary>
    /// Gets or sets the perspective of the light that makes the shadows.
    /// </summary>
    public Camera LightPerspective
    {
        get => _lightPerspective;
        set => _lightPerspective = value;
    }

    /// <summary>
    /// Gets or sets the scale of the light view matrix.
    /// </summary>
    public float PerspectiveScale
    {
        get => _perspectiveScale;
        set => _perspectiveScale = value;
    }
    /// <summary>
    /// Gets or sets the far plane of the light view matrix.
    /// </summary>
    public float FarPlane
    {
        get => _farPlane;
        set => _farPlane = value;
    }
    /// <summary>
    /// Gets or sets the near plane of the light view matrix.
    /// </summary>
    public float NearPlane
    {
        get => _nearPlane;
        set => _nearPlane = value;
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public ShadowView()
    {
        _shadowIntensity = 0.7f;
        _lightPerspective = new Camera();
        _lightPerspective.Position = new Vec3(0, 0, 0);
        _lightPerspective.Target = new Vec3(0, -64, -64);
        _lightPerspective.Up = new Vec3(0, 1, 0);
    }
}