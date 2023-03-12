namespace nlcEngine;

/// <summary>
/// Provides the interfaces to deal with the scenes.
/// </summary>
public sealed class SceneService
{
    internal SceneService()
    {
    }

    private Scene _current = new Scene();

    /// <summary>
    /// Gets the current scene.
    /// </summary>
    public Scene CurrentScene => _current;

    /// <summary>
    /// Navigates the specified scene.
    /// </summary>
    /// <param name="scene">scene</param>
    public void Navigate(Scene scene)
    {
        NlcArgException.NullThrow(nameof(scene), scene);

        if (!scene.Created)
        {
            scene.OnUserCreate();
            scene.Created = true;
        }
        scene.OnUserLoad();

        _current = scene;
    }

    internal void OnUserUpdate(float elapsedTime)
    {
        _current.OnUserUpdate(elapsedTime);
    }

    internal void OnUserRender(float elapsedTime)
    {
        _current.OnUserRender(elapsedTime);
    }

    internal void OnUserConstUpdate(float elapsedTime)
    {
        _current.OnUserConstUpdate(elapsedTime);
    }

    internal Color GetBackgroundColor()
    {
        return _current.BackgroundColor;
    }
}