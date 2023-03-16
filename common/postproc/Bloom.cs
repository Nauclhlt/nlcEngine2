using SixLabors.ImageSharp.PixelFormats;

namespace nlcEngine;

/// <summary>
/// Provides the bloom post process.
/// </summary>
public sealed class Bloom : IDisposable
{
    int _brightBuffer;
    int _brightTexture;
    int _resultBuffer;
    int _resultTexture;
    bool _disposed = false;
    BloomOption _options = new BloomOption();

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public Bloom()
    {
        _brightBuffer = GL.GenFramebuffer();
        _resultBuffer = GL.GenFramebuffer();
        _brightTexture = GL.GenTexture();
        _resultTexture = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, _brightTexture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _brightBuffer);
        GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, _brightTexture, 0);

        GL.BindTexture(TextureTarget.Texture2D, _resultTexture);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, NlcEngineGame.Profile.BufferWidth, NlcEngineGame.Profile.BufferHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _resultBuffer);
        GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, _resultTexture, 0);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        GL.BindTexture(TextureTarget.Texture2D, 0);


        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _brightBuffer);

        ResourceCollector.Add(this);
    }

    /// <summary>
    /// Sets the blooming options.
    /// </summary>
    /// <param name="options">a structure that contains options</param>
    public void SetOption( BloomOption options )
    {
        _options = options;
    }

    /// <summary>
    /// Runs the process.
    /// </summary>
    public void Run()
    {
        //_options.Intensity = 12;

        int screenBuffer;
        int screenTexture;

        int drawScreenBuffer = DefaultBuffer.Framebuffer;
        int drawScreenTexture = DefaultBuffer.Texture;

        {
            // default buffer
            screenBuffer = DefaultBuffer.Framebuffer;
            screenTexture = DefaultBuffer.Texture;
        }

        // テクスチャを現在のバッファいっぱいに描画
        void drawTextureOnScreen()
        {
            float[] verts = new float[]
            {
                -1f, 1f,
                -1f, -1f,
                1f, 1f,
                1f, -1f
            };
            float[] ts = new float[]
            {
                0f, 1f,
                0f, 0f,
                1f, 1f,
                1f, 0f
            };

            Rdc.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, verts);
            Rdc.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, ts);

            Rdc.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }

        // ウェイトをユニフォーム変数に設定
        void uniformWeights(float[] weights, Shader shader)
        {
            GL.Uniform1(GL.GetUniformLocation(shader.Name, "weights"), weights.Length, weights);
        }

        GL.Disable(EnableCap.Blend);


        


        // 輝度抽出シェーダー
        Shader shader = CoreShaders.BrightShader;
        shader.Activate();
        GL.Uniform1(GL.GetUniformLocation(shader.Name, "applyMin"), _options.MinBrightness);

        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _brightBuffer);

        GL.ClearColor(0, 0, 0, 0);
        GL.ClearDepth(1);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        // スクリーンテクスチャをバインドし、輝度を抽出
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, screenTexture);

        GL.Uniform1(GL.GetUniformLocation(shader.Name, "pTexture"), 0);

        drawTextureOnScreen();


        



        // ぼかしシェーダーを有効化
        shader = CoreShaders.BlurShader;
        shader.Activate();

        // ウェイトを生成
        float[] weights = GenerateWeights(_options.Intensity / 2 + 2, _options.BlurExp);
        uniformWeights(weights, shader);
        GL.Uniform1(GL.GetUniformLocation(shader.Name, "pTexture"), 0);
        GL.Uniform1(GL.GetUniformLocation(shader.Name, "samples"), _options.Intensity);

        {
            for (int i = 0; i < _options.Count; i++)
            {
                // リザルトバッファに描画する
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, _resultBuffer);

                // 輝度テクスチャをバインド
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, _brightTexture);

                GL.ClearColor(0, 0, 0, 0);
                GL.ClearDepth(1);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.Uniform1(GL.GetUniformLocation(shader.Name, "vertical"), 0);

                drawTextureOnScreen();


                // 先程の結果(リザルトバッファ)をもとに輝度バッファに描画
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, _brightBuffer);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, _resultTexture);

                GL.ClearColor(0, 0, 0, 0);
                GL.ClearDepth(1);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.Uniform1(GL.GetUniformLocation(shader.Name, "pTexture"), 0);
                GL.Uniform1(GL.GetUniformLocation(shader.Name, "vertical"), 1);

                drawTextureOnScreen();
            }
        }

        // 加算合成シェーダー
        shader = CoreShaders.AddShader;
        shader.Activate();

        DefaultBuffer.Bind();
        

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, _brightTexture);

        GL.Uniform1(GL.GetUniformLocation(shader.Name, "pTexture"), 0);

        GL.ActiveTexture(TextureUnit.Texture1);
        GL.BindTexture(TextureTarget.Texture2D, drawScreenTexture);

        GL.Uniform1(GL.GetUniformLocation(shader.Name, "destTexture"), 1);
        GL.Uniform1(GL.GetUniformLocation(shader.Name, "blurFactor"), _options.BlurFactor);
        GL.Uniform1(GL.GetUniformLocation(shader.Name, "baseFactor"), _options.BaseFactor);

        drawTextureOnScreen();

        GL.BindTexture(TextureTarget.Texture2D, 0);

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private float[] GenerateWeights( int count, float blur )
    {
        float[] weights = new float[count];

        float d = blur * blur * 0.001f;
        float total = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            float x = i;
            float w = MathF.Exp(-0.5f * (x * x) / d);
            weights[i] = w;

            if (i > 0)
            {
                w *= 2f;
            }

            total += w;
        }

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] /= total;
        }

        return weights;
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {

            }

            GL.DeleteFramebuffer(_brightBuffer);
            GL.DeleteFramebuffer(_resultBuffer);

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by <see cref="Bloom"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// A destructor.
    /// </summary>
    ~Bloom()
    {
        Dispose(false);
    }
}