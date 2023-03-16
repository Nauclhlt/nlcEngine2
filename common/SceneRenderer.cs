namespace nlcEngine;

/// <summary>
/// Provides methods to render objects.
/// </summary>
public static class SceneRenderer
{
    
    static GBuffer _gBuffer;
    static LightStorageBuffer _lightBuffer;
    static Lazy<DepthMapBuffer> _depthBuffer = new Lazy<DepthMapBuffer>(() => new DepthMapBuffer(1024, 1024));

    private static GBuffer GetOrCreateGBuffer()
    {
        if (_gBuffer is null)
        {
            _gBuffer = new GBuffer();
            return _gBuffer;
        }
        else
        {
            return _gBuffer;
        }
    }

    

    private static void EnsureLightBufferCreate()
    {
        if (_lightBuffer is null)
        {
            _lightBuffer = new LightStorageBuffer();
        }
    }

    /// <summary>
    /// Renders the objects with deferred lighting.<br />
    /// This process should be runned at the beginning of a frame, and other objects with no deferred lighting should be rendered after this process.
    /// </summary>
    /// <param name="camera">camera</param>
    /// <param name="renderer">renderer</param>
    /// <param name="env">light environment</param>
    public static void RenderWithLightDeferred( Camera camera, DeferredList renderer, LightEnvironment env )
    {
        Color backColor = NlcEngineGame.SceneService.GetBackgroundColor();

        GBuffer gbuffer = GetOrCreateGBuffer();
        EnsureLightBufferCreate();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, gbuffer.Name);
        GL.ClearColor(0, 0, 0, 0);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 projection = Viewer.ProjectionMatrix;
        Matrix4 view = camera.CreateViewMatrix();
        Matrix4 model = Matrix4.Identity;

        // CoreShaders.DeferGBufShader.Activate();
        // NlcHelper.SendMat(model, view, projection);
        // CoreShaders.DeferGBufShader.SetBoolean("textured", true);
        // CoreShaders.DeferGBufShader.SetInt("pTexture", 0);

        var objList = renderer.GetListOfObjects();
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].DeferRender(model, view, projection);
        }

        //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        DefaultBuffer.Bind();

        //Color b = Color.Red;
        GL.ClearColor(backColor.Rf, backColor.Gf, backColor.Bf, backColor.Af);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Shader shader = CoreShaders.DeferLightShader;
        shader.Activate();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GPosition);
        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GNormal);
        GL.ActiveTexture(TextureUnit.Texture2);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GColorSpec);

        _lightBuffer.Buffer(env.Lights);

        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _lightBuffer.Name);
        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, _lightBuffer.Name);

        shader.SetFloat("ambientIntensity", env.AmbientIntensity);
        shader.SetVec3("ambientColor", new Vec3(env.AmbientColor.Rf, env.AmbientColor.Gf, env.AmbientColor.Bf));
        shader.SetInt("lightCount", Math.Min(env.Lights.Count, 128));
        
        shader.SetVec3("viewPos", camera.Position);

        shader.SetInt("gPosition", 0);
        shader.SetInt("gNormal", 1);
        shader.SetInt("gColorSpec", 2);
        shader.SetVec3("backColor", new Vec3(backColor.Rf, backColor.Gf, backColor.Bf));

        RenderDefQuad();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, gbuffer.Name);
        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, DefaultBuffer.Framebuffer);

        GL.BlitFramebuffer(0, 0, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, 0, 0, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        DefaultBuffer.Bind();

        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, 0);
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
    }

    /// <summary>
    /// Renders the objects with deferred lighting and shadow mapping.<br />
    /// This process should be runned at the beginning of a frame, and other objects with no deferred lighting should be rendered after this process.
    /// </summary>
    /// <param name="camera">camera</param>
    /// <param name="renderer">renderer</param>
    /// <param name="env">light environment</param>
    /// <param name="shadowView">shadow setting</param>
    public static void RenderWithLightAndShadowDeferred( Camera camera, DeferredList renderer, LightEnvironment env, ShadowView shadowView )
    {
        // depth map code

        DepthMapBuffer depthBuffer = _depthBuffer.Value;

        Camera depthLightCam = shadowView.LightPerspective;
        Matrix4 lightProjection, lightView, lightSpaceMatrix;
        lightProjection = Matrix4.CreateOrthographic(depthBuffer.Width, depthBuffer.Height, 0.02f, 7000f);
        lightView = Matrix4.LookAt(NlcHelper.Conv(depthLightCam.Position), NlcHelper.Conv(depthLightCam.Target), NlcHelper.Conv(depthLightCam.Up));
        lightSpaceMatrix = lightView * lightProjection;  // maybe wrong
        //lightSpaceMatrix.Transpose();

        GL.Viewport(0, 0, depthBuffer.Width, depthBuffer.Height);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthBuffer.Framebuffer);
        GL.Clear(ClearBufferMask.DepthBufferBit);

        // render on the depth map

        var objList = renderer.GetListOfObjects();
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].DepthRender(lightSpaceMatrix);
        }

        GL.Viewport(0, 0, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight);


        //---------------





        Color backColor = NlcEngineGame.SceneService.GetBackgroundColor();

        GBuffer gbuffer = GetOrCreateGBuffer();
        EnsureLightBufferCreate();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, gbuffer.Name);
        GL.ClearColor(0, 0, 0, 0);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Matrix4 projection = Viewer.ProjectionMatrix;
        Matrix4 view = camera.CreateViewMatrix();
        Matrix4 model = Matrix4.Identity;

        // CoreShaders.DeferGBufShader.Activate();
        // NlcHelper.SendMat(model, view, projection);
        // CoreShaders.DeferGBufShader.SetBoolean("textured", true);
        // CoreShaders.DeferGBufShader.SetInt("pTexture", 0);

        
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].DeferRender(model, view, projection);
        }

        //GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        DefaultBuffer.Bind();

        //Color b = Color.Red;
        GL.ClearColor(backColor.Rf, backColor.Gf, backColor.Bf, backColor.Af);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        Shader shader = CoreShaders.DepthLightShader;
        shader.Activate();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GPosition);
        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GNormal);
        GL.ActiveTexture(TextureUnit.Texture2);
        GL.BindTexture(TextureTarget.Texture2D, _gBuffer.GColorSpec);
        GL.ActiveTexture(TextureUnit.Texture3);
        GL.BindTexture(TextureTarget.Texture2D, depthBuffer.MapTexture);

        _lightBuffer.Buffer(env.Lights);

        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _lightBuffer.Name);
        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, _lightBuffer.Name);

        shader.SetFloat("ambientIntensity", env.AmbientIntensity);
        shader.SetVec3("ambientColor", new Vec3(env.AmbientColor.Rf, env.AmbientColor.Gf, env.AmbientColor.Bf));
        shader.SetInt("lightCount", Math.Min(env.Lights.Count, 128));
        
        shader.SetVec3("viewPos", camera.Position);
        shader.SetVec3("lightPos", depthLightCam.Position);
        GL.UniformMatrix4(GL.GetUniformLocation(shader.Name, "lightSpaceMatrix"), false, ref lightSpaceMatrix);

        shader.SetInt("gPosition", 0);
        shader.SetInt("gNormal", 1);
        shader.SetInt("gColorSpec", 2);
        shader.SetInt("shadowMap", 3);
        shader.SetVec3("backColor", new Vec3(backColor.Rf, backColor.Gf, backColor.Bf));

        RenderDefQuad();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, 0);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, gbuffer.Name);
        GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, DefaultBuffer.Framebuffer);

        GL.BlitFramebuffer(0, 0, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, 0, 0, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        DefaultBuffer.Bind();

        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 0, 0);
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);


        return;
        shader = CoreShaders.DepthDebugShader;
        shader.Activate();

        GL.BindTexture(TextureTarget.Texture2D, depthBuffer.MapTexture);

        RenderDepthDebug();

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private static void RenderDepthDebug()
    {
        float[] va = new float[]
        {
            -1f, 1f, 0f,
            -1f, 0f, 0f,
            0f, 1f, 0f,
            0f, 0f, 0f
        };

        float[] ta = new float[]
        {
            0f, 1f,
            0f, 0f,
            1f, 1f,
            1f, 0f
        };

        Rdc.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, va);
        Rdc.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, ta);

        Rdc.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
    }

    private static void RenderDefQuad()
    {
        float[] va = new float[]
        {
            -1f, 1f, 0f,
            -1f, -1f, 0f,
            1f, 1f, 0f,
            1f, -1f, 0f
        };

        float[] ta = new float[]
        {
            0f, 1f,
            0f, 0f,
            1f, 1f,
            1f, 0f
        };

        Rdc.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, va);
        Rdc.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, ta);

        Rdc.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
    }
}