namespace nlcEngine;

/// <summary>
/// Contains the list of the objects used to render.
/// </summary>
public sealed class DeferredList
{
    List<IDefer> _objects;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public DeferredList()
    {
        _objects = new List<IDefer>();
    }

    /// <summary>
    /// Gets the list of the objects.
    /// </summary>
    public List<IDefer> Objects
    {
        get => _objects;
    }

    internal List<IDefer> GetListOfObjects()
    {
        return _objects;
    }
}