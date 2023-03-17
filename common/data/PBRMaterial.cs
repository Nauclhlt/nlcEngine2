namespace nlcEngine;

/// <summary>
/// Represents a material of PBR(Physically-Based-Rendering).
/// </summary>
public sealed class PBRMaterial
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public PBRMaterial()
    {

    }

    float _metalness;
    float _roughness;
    float _ao;

    /// <summary>
    /// Gets or sets the metalness.
    /// </summary>
    public float Metalness
    {
        get => _metalness;
        set => _metalness = value;
    }

    /// <summary>
    /// Gets or sets the roughness.
    /// </summary>
    public float Roughness
    {
        get => _roughness;
        set => _roughness = value;
    }

    /// <summary>
    /// Gets or sets the ambient occlusion.
    /// </summary>
    public float AO
    {
        get => _ao;
        set => _ao = value;
    }
}