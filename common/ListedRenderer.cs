namespace nlcEngine;

/// <summary>
/// Contains the list of the objects used to render.
/// </summary>
public sealed class ListedRenderer
{
    List<RenderObject> _objects;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public ListedRenderer()
    {
        _objects = new List<RenderObject>();
    }

    /// <summary>
    /// Gets the list of the objects.
    /// </summary>
    public List<RenderObject> Objects
    {
        get => _objects;
    }

    internal List<RenderObject> GetListOfObjects()
    {
        return _objects;
    }
}