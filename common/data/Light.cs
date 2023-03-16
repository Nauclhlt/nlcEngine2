namespace nlcEngine;

/// <summary>
/// Represents a light.
/// </summary>
public sealed class Light
{
    Color _diffuseColor;
    Color _specularColor;
    Vec3 _position;
    float _radius;
    float _attenuation;
    float _intensity;

    /// <summary>
    /// Gets the diffuse color.
    /// </summary>
    public Color DiffuseColor => _diffuseColor;
    /// <summary>
    /// Gets the specular color.
    /// </summary>
    public Color SpecularColor => _specularColor;
    /// <summary>
    /// Gets the position of the light.
    /// </summary>
    public Vec3 Position => _position;
    /// <summary>
    /// Gets the radius of the light.
    /// </summary>
    public float Radius => _radius;
    /// <summary>
    /// Gets the attenuation.
    /// </summary>
    public float Attenuation => _attenuation;
    /// <summary>
    /// Gets the intensity of the light.
    /// </summary>
    public float Intensity => _intensity;

    /// <summary>
    /// Creates a new instance with the light colors, position, radius, attenuation, and intensity values.
    /// </summary>
    /// <param name="diffuse">diffuse color</param>
    /// <param name="spec">specular color</param>
    /// <param name="position">position of the light</param>
    /// <param name="radius">radius of the light</param>
    /// <param name="attenuation">attenuation</param>
    /// <param name="intensity">intensity of the light</param>
    public Light(Color diffuse, Color spec, Vec3 position, float radius, float attenuation, float intensity)
    {
        _diffuseColor = diffuse;
        _specularColor = spec;
        _position = position;
        _radius = radius;
        _attenuation = attenuation;
        _intensity = intensity;
    }
}