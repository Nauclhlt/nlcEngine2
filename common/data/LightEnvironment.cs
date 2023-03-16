namespace nlcEngine;

/// <summary>
/// Contains the light setting of Phong-shading-model.
/// </summary>
public sealed class LightEnvironment
{
    float _ambientIntensity;
    Color _ambientColor;
    List<Light> _lights;

    /// <summary>
    /// Gets or sets the intensity of the ambient light.
    /// </summary>
    public float AmbientIntensity
    {
        get => _ambientIntensity;
        set => _ambientIntensity = value;
    }
    /// <summary>
    /// Gets or sets the ambient color.
    /// </summary>
    public Color AmbientColor
    {
        get => _ambientColor;
        set => _ambientColor = value;
    }
    /// <summary>
    /// Gets the collection that contains the lights in the environment.<br />
    /// Maximum number of the lights is 128. Items indexed over 128 will be ignored when the scene this object is used to render.
    /// </summary>
    public List<Light> Lights => _lights;

    /// <summary>
    /// Creates a new instance from the ambient light setting.
    /// </summary>
    /// <param name="ambientIntensity">intensity of the ambient light</param>
    /// <param name="ambientColor">color of the ambient color</param>
    public LightEnvironment( float ambientIntensity, Color ambientColor )
    {
        _ambientIntensity = ambientIntensity;
        _ambientColor = ambientColor;
        _lights = new List<Light>();
    }
}