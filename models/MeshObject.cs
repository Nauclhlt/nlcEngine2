namespace nlcEngine.Models;

/// <summary>
/// A base class for render-objects that contains mesh data.
/// </summary>
public abstract class MeshObject : IDisposable, IDefer
{
    Mesh[] _meshes;
    MeshBuffer[] _buffers;
    Transform _transform;
    bool _disposed = false;
    Animator _animator;

    /// <summary>
    /// Gets the array that contains meshes.
    /// </summary>
    protected Mesh[] Meshes
    {
        get => _meshes;
    }

    /// <summary>
    /// Gets or sets the transform applied to the object.
    /// </summary>
    public Transform Transform
    {
        get => _transform;
        set => _transform = value;
    }

    /// <summary>
    /// Loads the meshes.
    /// </summary>
    /// <param name="meshes">meshes to load</param>
    protected void LoadMeshes(Mesh[] meshes)
    {
        NlcArgException.NullThrow(nameof(meshes), meshes);
        if (meshes.Length == 0)
        {
            return;
        }

        if (_meshes is not null)
            return;

        _meshes = meshes;
        _buffers = new MeshBuffer[_meshes.Length];
    }

    /// <summary>
    /// Creates a new animation.
    /// </summary>
    /// <param name="filename">filename</param>
    public Animation CreateAnimation(string filename)
    {
        return new Animation(filename, _meshes[0]);
    }

    /// <summary>
    /// Applies the animator.
    /// </summary>
    /// <param name="animator">animator</param>
    public void ApplyAnimation(Animator animator)
    {
        NlcArgException.NullThrow(nameof(animator), animator);

        _animator = animator;
    }

    /// <summary>
    /// Renders self. <br />
    /// THIS METHOD IS USED IN THE GAME ENGINE INTERNAL PROCESS. DO NOT CALL THIS FROM NORMAL CODE.
    /// </summary>
    public void DeferRender(Matrix4 model, Matrix4 view, Matrix4 proj)
    {
        if (_animator is not null)
        {
            Shader shader = CoreShaders.ModelAnimDeferGShader;
            shader.Activate();

            NlcHelper.SendMat(_transform.GetModelMatrix(), view, proj);
            var matrices = _animator.GetFinalBoneMatrices();

            for (int i = 0; i < matrices.Count; i++)
            {
                int loc = GL.GetUniformLocation(shader.Name, "finalBoneMatrices[" + i + "]");
                Matrix4 m = matrices[i];
                GL.UniformMatrix4(loc, false, ref m);
            }

            shader.SetBoolean("textured", false);

            for (int i = 0; i < _buffers.Length; i++)
            {
                CreateCallRender();
            }
        }
        else
        {
            Shader shader = CoreShaders.ModelDeferGShader;
            shader.Activate();

            NlcHelper.SendMat(_transform.GetModelMatrix(), view, proj);

            shader.SetBoolean("textured", false);

            for (int i = 0; i < _buffers.Length; i++)
            {
                CreateCallRender();
            }
        }

        _animator = null;
    }

    /// <summary>
    /// Renders self. <br />
    /// THIS METHOD IS USED IN THE GAME ENGINE INTERNAL PROCESS. DO NOT CALL THIS FROM NORMAL CODE.
    /// </summary>
    public void DepthRender(Matrix4 lightSpaceMatrix)
    {
        if (_animator is not null)
        {
            // animated

        }
        else
        {
            Shader shader = CoreShaders.ModelDepthShader;
            shader.Activate();

            Matrix4 model = _transform.GetModelMatrix();
            GL.UniformMatrix4(0, false, ref lightSpaceMatrix);
            GL.UniformMatrix4(1, true, ref model);

            CreateCallRender();
        }

        
    }

    /// <summary>
    /// Just renders the mesh.
    /// </summary>
    /// <param name="camera">camera</param>
    /// <param name="transform">transformation</param>
    public void RenderJust(Camera camera, Transform transform = default)
    {
        Shader shader = CoreShaders.ModelStdShader;
        shader.Activate();

        shader.SetBoolean("textured", false);

        NlcHelper.SendMat(transform.GetModelMatrix(), camera.CreateViewMatrix(), Viewer.ProjectionMatrix);

        CreateBuffer();

        for (int i = 0; i < _buffers.Length; i++)
        {
            
            _buffers[i].JustCallRender();
        }
    }

    private void DoCreateBuffer()
    {
        for (int i = 0; i < _buffers.Length; i++)
        {
            _buffers[i] = new MeshBuffer(_meshes[i]);
            _buffers[i].CreateBuffer();
        }
    }

    private void CreateBuffer()
    {
        if (_buffers[0] is null)
        {
            DoCreateBuffer();
        }
        else
        {
            // for (int i = 0; i < _buffers.Length; i++)
            // {
            //     _buffers[i].CreateBuffer();
            // }
        }
    }

    internal void CreateCallRender()
    {
        if (_buffers[0] is null)
        {
            CreateBuffer();
        }

        

        for (int i = 0; i < _buffers.Length; i++)
        {
            _buffers[i].JustCallRender();
        }

        
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                for (int i = 0; i < _buffers.Length; i++)
                    _buffers[i].Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the mesh buffer.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// A destructor.
    /// </summary>
    ~MeshObject()
    {
        Dispose(false);
    }
}