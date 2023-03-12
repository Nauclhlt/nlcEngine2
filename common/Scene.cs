namespace nlcEngine;

/// <summary>
/// Represents a scene in the game.
/// </summary>
public class Scene
{
    bool _created = false;

    internal bool Created
    {
        get => _created;
        set => _created = value;
    }

    /// <summary>
    /// Updates the frame, called per frame, when overridden.
    /// </summary>
    public virtual void OnUserUpdate(float elapsedTime)
    {
    }

    /// <summary>
    /// Renders the frame, called per frame, when overridden.
    /// </summary>
    public virtual void OnUserRender(float elapsedTime)
    {
    }

    /// <summary>
    /// Updates the frame, called per frame, when overridden.
    /// </summary>
    public virtual void OnUserConstUpdate(float elapsedTime)
    {
    }

    /// <summary>
    /// Loads the frame, called when the scene is loaded, when overridden.
    /// </summary>
    public virtual void OnUserLoad()
    {
    }

    /// <summary>
    /// Initializes the scene, called first time when the scene is loaded, when overridden.
    /// </summary>
    public virtual void OnUserCreate()
    {
    }
}