namespace nlcEngine;

/// <summary>
/// Contains a setting to start a game.
/// </summary>
public sealed class Profile
{
    int _windowWidth;
    int _windowHeight;
    int _bufferWidth;
    int _bufferHeight;
    bool _fullscreen;
    string _title;
    double _fps;

    /// <summary>
    /// Gets the width of the window.
    /// </summary>
    public int WindowWidth => _windowWidth;
    /// <summary>
    /// Gets the height of the window.
    /// </summary>
    public int WindowHeight => _windowHeight;
    /// <summary>
    /// Gets the width of the buffer.
    /// </summary>
    public int BufferWidth => _bufferWidth;
    /// <summary>
    /// Gets the height of the buffer.
    /// </summary>
    public int BufferHeight => _bufferHeight;
    /// <summary>
    /// Gets whether the window is fullscreen.
    /// </summary>
    public bool Fullscreen => _fullscreen;
    /// <summary>
    /// Gets the title text.
    /// </summary>
    public string Title => _title;
    /// <summary>
    /// Gets the fps.
    /// </summary>
    public double Fps => _fps;

    // Hidden parameterless constructor
    private Profile()
    {
    }

    /// <summary>
    /// Creates a new profile with the window size, buffer size, and title.
    /// </summary>
    /// <param name="windowWidth">width of the window</param>
    /// <param name="windowHeight">height of the window</param>
    /// <param name="bufferWidth">width of the buffer</param>
    /// <param name="bufferHeight">height of the buffer</param>
    /// <param name="title">title text</param>
    public Profile(int windowWidth, int windowHeight, int bufferWidth, int bufferHeight, string title)
    {
        NlcArgException.ZeroThrow(nameof(windowWidth), windowWidth);
        NlcArgException.ZeroThrow(nameof(windowHeight), windowHeight);
        NlcArgException.ZeroThrow(nameof(bufferWidth), bufferWidth);
        NlcArgException.ZeroThrow(nameof(bufferHeight), bufferHeight);
        

        _windowWidth = windowWidth;
        _windowHeight = windowHeight;
        _bufferWidth = bufferWidth;
        _bufferHeight = bufferHeight;
        _title = title;
        _fps = 60.0;
        _fullscreen = false;
    }

    /// <summary>
    /// Creates a new instance with the window size, buffer size, title, and fps.
    /// </summary>
    /// <param name="windowWidth">width of the window</param>
    /// <param name="windowHeight">height of the window</param>
    /// <param name="bufferWidth">width of the buffer</param>
    /// <param name="bufferHeight">height of the buffer</param>
    /// <param name="title">title text</param>
    /// <param name="fps">frames-per-second</param>
    public Profile(int windowWidth, int windowHeight, int bufferWidth, int bufferHeight, string title, double fps)
    {
        NlcArgException.ZeroThrow(nameof(windowWidth), windowWidth);
        NlcArgException.ZeroThrow(nameof(windowHeight), windowHeight);
        NlcArgException.ZeroThrow(nameof(bufferWidth), bufferWidth);
        NlcArgException.ZeroThrow(nameof(bufferHeight), bufferHeight);
        if (fps <= 0)
        {
            NlcArgException.Throw("fps", "Must be greater than 0.");
        }


        _windowWidth = windowWidth;
        _windowHeight = windowHeight;
        _bufferWidth = bufferWidth;
        _bufferHeight = bufferHeight;
        _title = title;
        _fps = fps;
    }

    /// <summary>
    /// Creates a new profile with the buffer size, title, and fps for fullscreen game.
    /// </summary>
    /// <param name="bufferWidth">width of the buffer</param>
    /// <param name="bufferHeight">height of the buffer</param>
    /// <param name="title">title text</param>
    /// <param name="fps">frames-per-second</param>
    public Profile(int bufferWidth, int bufferHeight, string title, double fps)
    {
        if (fps <= 0)
        {
            NlcArgException.Throw("fps", "Must be greater than 0.");
        }

        _windowWidth = bufferWidth;
        _windowHeight = bufferHeight;
        _bufferWidth = bufferWidth;
        _bufferHeight = bufferHeight;
        _title = title;
        _fps = fps;
        _fullscreen = true;
    }
}