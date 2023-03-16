namespace nlcEngine;

/// <summary>
/// Contains options for blooming process.
/// </summary>
public struct BloomOption
{
    private float _blurFactor;
    private float _baseFactor;
    private float _minBrightness;
    private int _intensity;
    private int _count;
    private float _blurExp;

    /// <summary>
    /// Gets or sets the factor for blur colors. The default value is 0.7.
    /// </summary>
    public float BlurFactor
    {
        get => _blurFactor;
        set => _blurFactor = value;
    }

    /// <summary>
    /// Gets or sets the factor for base colors. The default value is 1.
    /// </summary>
    public float BaseFactor
    {
        get => _baseFactor;
        set => _baseFactor = value;
    }

    /// <summary>
    /// Gets or sets the intensity of the emission, means the number of sample texels. The default value is 12.
    /// </summary>
    public int Intensity
    {
        get => _intensity;
        set
        {
            if (value < 1 || value > 127)
            {
                throw new NlcCommonException("The value must be in the range 1-127.");
            }

            _intensity = value;
        }
    }

    /// <summary>
    /// Gets or sets the exponent for the blur weights. The default value is 1400.
    /// </summary>
    public float BlurExp
    {
        get => _blurExp;
        set => _blurExp = value;
    }

    /// <summary>
    /// Gets or sets the minimum brightness of pixels that should be bloom-applied. The default value is 0.4.
    /// </summary>
    public float MinBrightness
    {
        get => _minBrightness;
        set => _minBrightness = value;
    }

    /// <summary>
    /// Gets or sets the number of blur processes. The default value is 1.
    /// </summary>
    public int Count
    {
        get => _count;
        set
        {
            if (value < 1 || value > 16)
            {
                throw new NlcCommonException("The value must be in the range 1-15");
            }

            _count = value;
        }
    }

    /// <summary>
    /// Initializes a new structure with default values.
    /// </summary>
    public BloomOption()
    {
        _blurFactor = 0.7f;
        _baseFactor = 1f;
        _intensity = 12;
        _minBrightness = 0.4f;
        _count = 1;
        _blurExp = 1400f;
    }
}