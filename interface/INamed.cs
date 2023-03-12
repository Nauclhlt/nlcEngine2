namespace nlcEngine;

/// <summary>
/// Provides the property to retrieve the resource name held in the wrapper.
/// </summary>
public interface INamed
{
    /// <summary>
    /// Gets the resource name.
    /// </summary>
    public int Name{ get; }
}