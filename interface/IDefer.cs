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
    /// <summary>
    /// Renders self on a shadow map.
    /// </summary>
    /// <param name="lightSpaceMatrix">light space matrix</param>
    /// <param name="nearPlane">near plane</param>
    /// <param name="farPlane">far plane</param>
    public void DepthRender(Matrix4 lightSpaceMatrix, float nearPlane, float farPlane);
}