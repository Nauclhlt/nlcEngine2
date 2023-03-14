namespace nlcEngine;

/// <summary>
/// Provides objects that can be rendered with deferred shading.
/// </summary>
public interface IDefer
{
    /// <summary>
    /// Renders self with the deferred shading.
    /// </summary>
    public void DeferRender(Matrix4 model, Matrix4 view, Matrix4 proj);
}