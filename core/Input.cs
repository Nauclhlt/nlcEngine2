namespace nlcEngine;

/// <summary>
/// Provides the API to handle user input.
/// </summary>
public static class Input
{
    /// <summary>
    /// Checks if the specified key is currently pressed.
    /// </summary>
    /// <param name="key">key</param>
    /// <returns>whether the key is pressed</returns>
    public static bool IsKeyDown(Key key)
    {
        NlcHelper.InThrow();

        return NlcEngineGame.Window.IsKeyDown((OpenTK.Windowing.GraphicsLibraryFramework.Keys)key);
    }

    /// <summary>
    /// Checks if the specified key is started to be pressed in this frame.
    /// </summary>
    /// <param name="key">key</param>
    /// <returns>whether the key is pressed</returns>
    public static bool IsKeyPressed(Key key)
    {
        NlcHelper.InThrow();

        return NlcEngineGame.Window.IsKeyPressed((OpenTK.Windowing.GraphicsLibraryFramework.Keys)key);
    }

    /// <summary>
    /// Checks if the specified key is released in this frame.
    /// </summary>
    /// <param name="key">key</param>
    /// <returns>whether the key is released</returns>
    public static bool IsKeyReleased(Key key)
    {
        NlcHelper.InThrow();

        return NlcEngineGame.Window.IsKeyReleased((OpenTK.Windowing.GraphicsLibraryFramework.Keys)key);
    }

    /// <summary>
    /// Gets the mouse point relative to the window.
    /// </summary>
    /// <returns>relative mouse point</returns>
    public static Vec2 GetMousePoint()
    {
        NlcHelper.InThrow();

        Vector2 p = NlcEngineGame.Window.MousePosition;
        return new Vec2(p.X, p.Y);
    }

    /// <summary>
    /// Sets the mouse point relative to the window.
    /// </summary>
    /// <param name="pos">relative mouse point</param>
    public static void SetMousePoint(Vec2 pos)
    {
        NlcHelper.InThrow();

        NlcEngineGame.Window.MousePosition = new Vector2(pos.X, pos.Y);
    }

    /// <summary>
    /// Sets the visibility of the mouse cursor.
    /// </summary>
    /// <param name="visibility">visibility</param>
    public static void SetCursorVisibility(bool visibility)
    {
        NlcEngineGame.Window.CursorState = visibility ? CursorState.Normal : CursorState.Hidden;
    }
}