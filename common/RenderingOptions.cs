namespace nlcEngine;

/// <summary>
/// Handles the rendering options.
/// </summary>
public static class RenderingOptions
{
    /// <summary>
    /// Sets whether the face culling is enabled.
    /// </summary>
    /// <param name="ability">ability</param>
    public static void SetFaceCullingEnabled(bool ability)
    {
        if (ability)
        {
            GL.Enable(EnableCap.CullFace);
        }
        else
        {
            GL.Disable(EnableCap.CullFace);
        }
    }
}