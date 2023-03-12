global using System;
global using System.Reflection;
global using System.Runtime.InteropServices;
global using System.Runtime.CompilerServices;
global using System.IO;
global using System.Collections;
global using OpenTK;
global using OpenTK.Graphics.OpenGL4;
global using OpenTK.Audio.OpenAL;
global using OpenTK.Windowing;
global using OpenTK.Windowing.Desktop;
global using OpenTK.Windowing.Common;
global using OpenTK.Windowing.GraphicsLibraryFramework;
global using OpenTK.Input;
global using OpenTK.Mathematics;

namespace nlcEngine;

/// <summary>
/// Provides the main class for Nlc Game Engine.
/// </summary>
public static class NlcEngineGame
{
    static GameWindow _window;
    static Profile _profile;
    static SceneService _sceneService = new SceneService();

    internal static GameWindow Window => _window;
    internal static Profile Profile => _profile;
    /// <summary>
    /// Gets the SceneService instance.
    /// </summary>
    public static SceneService SceneService => _sceneService;

    /// <summary>
    /// Initializes the new game window and loads the all features that must be loaded on the start.
    /// </summary>
    /// <param name="profile">profile</param>
    public static void Init( Profile profile )
    {
        NlcArgException.NullThrow(nameof(profile), profile);
        _profile = profile;

        NativeWindowSettings ns = new NativeWindowSettings();
        GameWindowSettings gs = new GameWindowSettings();

        gs.UpdateFrequency = profile.Fps;
        gs.RenderFrequency = profile.Fps;

        ns.Size = new Vector2i(profile.WindowWidth, profile.WindowHeight);
        ns.APIVersion = new Version(4, 6, 0);
        ns.API = ContextAPI.OpenGL;

        ns.WindowBorder = profile.Fullscreen ? WindowBorder.Hidden : WindowBorder.Fixed;
        ns.WindowState = profile.Fullscreen ? WindowState.Fullscreen : WindowState.Normal;

        ns.Title = profile.Title;

        _window = new GameWindow(gs, ns);
        _window.CenterWindow();

        _window.UpdateFrame += OnUpdateWindow;
        _window.RenderFrame += OnRenderWindow;

    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    public static void Start()
    {
        try
        {
            _window.Run();
        }
        finally
        {
            _window.Dispose();

            Cleanup();
        }
    }

    private static void Cleanup()
    {
        ResourceCollector.CleanupAll();
    }

    private static void OnUpdateWindow(FrameEventArgs e)
    {
        float elapsed = (float)e.Time;

        _sceneService.OnUserUpdate(elapsed);
        _sceneService.OnUserConstUpdate(elapsed);
    }

    private static void OnRenderWindow(FrameEventArgs e)
    {
        float elapsed = (float)e.Time;

        _window.MakeCurrent();

        Color backColor = GlobalState.BackgroundColor;
        GL.ClearColor(backColor.Rf, backColor.Gf, backColor.Bf, backColor.Af);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _sceneService.OnUserRender(elapsed);

        GL.Flush();
        _window.SwapBuffers();
    }
}