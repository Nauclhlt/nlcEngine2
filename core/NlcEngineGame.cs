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
    static List<Action> _releases = new List<Action>();

    static CopyingBuffer _copyingBuffer;

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


        GL.Enable(EnableCap.Texture2D);
        GL.Enable(EnableCap.TextureCubeMap);
        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.DebugOutput);
        // GL.DebugMessageCallback((source, type, id, severity, length, message, userParam) => {
        //     string msg = Marshal.PtrToStringUTF8(message);
        //     Console.Error.WriteLine("ID=" + id + " " + msg);
        // }, 0);

        DefaultBuffer.CreateBuffer(_profile.BufferWidth, _profile.BufferHeight);
        _copyingBuffer = CopyingBuffer.CreateBuffer();
        CoreShaders.Load();
        Rdc.Initialize();
        DefaultBuffer.Bind();

        Viewer.CreateProjection();
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("STACK_TRACE:  " + e.StackTrace);
        }
        finally
        {
            

            _window.Dispose();

            Cleanup();
        }
    }

    /// <summary>
    /// Ensures the action that releases some resources will be runned when the resources are disposed.
    /// </summary>
    /// <param name="action">releasing action</param>
    public static void EnsureRelease(Action action)
    {
        _releases.Add(action);
    }

    private static void Cleanup()
    {
        ResourceCollector.CleanupAll();

        for (int i = 0; i < _releases.Count; i++)
        {
            _releases[i]();
        }

        _releases.Clear();
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

        DefaultBuffer.Bind();

        GL.Enable(EnableCap.DepthTest);

        GL.Viewport(0, 0, _profile.BufferWidth, _profile.BufferHeight);

        Color backColor = _sceneService.GetBackgroundColor();
        GL.ClearColor(backColor.Rf, backColor.Gf, backColor.Bf, backColor.Af);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        _sceneService.OnUserRender(elapsed);

        _window.MakeCurrent();

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        CopyScreen();

        GL.Flush();
        _window.SwapBuffers();
    }

    private static void CopyScreen()
    {
        GL.Viewport(0, 0, _window.Size.X, _window.Size.Y);

        Shader shader = CoreShaders.CopyShader;
        shader.Activate();

        GL.Disable(EnableCap.DepthTest);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, DefaultBuffer.Texture);

        _copyingBuffer.Draw();

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }
}