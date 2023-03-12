namespace nlcEngine;

internal static class ResourceCollector
{
    static List<IDisposable> _resources = new List<IDisposable>();

    public static void Add(IDisposable resource)
    {
        _resources.Add(resource);
    }

    public static void Remove(IDisposable resource)
    {
        _resources.Remove(resource);
    }

    public static void CleanupAll()
    {
        for (int i = 0; i < _resources.Count; i++)
        {
            _resources[i].Dispose();
        }

        _resources.Clear();
    }
}