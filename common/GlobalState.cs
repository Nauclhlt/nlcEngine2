namespace nlcEngine;

internal static class GlobalState
{
    static Color _backgroundColor = Color.Black;

    public static Color BackgroundColor
    {
        get => _backgroundColor;
        set => _backgroundColor = value;
    }
}