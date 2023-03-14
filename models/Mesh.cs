namespace nlcEngine.Models;

using Ai = Assimp;

/// <summary>
/// Contains mesh data.
/// </summary>
public sealed class Mesh
{
    Vec3[] _vertices;
    Vec3[] _normals;
    Vec2[] _texCoords;
    uint[] _indices;
    List<Ai::Face> _faces;
    bool _hasTexture;

    /// <summary>
    /// Gets the array of the vertices.
    /// </summary>
    public Vec3[] Vertices => _vertices;
    /// <summary>
    /// Gets the array of the normals.
    /// </summary>
    public Vec3[] Normals => _normals;
    /// <summary>
    /// Gets the array of the texture coords.
    /// </summary>
    public Vec2[] TexCoords => _texCoords;
    /// <summary>
    /// Gets the array of the indices.
    /// </summary>
    public uint[] Indices => _indices;
    /// <summary>
    /// Gets whether the mesh has a texture.
    /// </summary>
    public bool HasTexture => _hasTexture;

    private Mesh()
    {
        // Hidden parameterless constructor
    }

    private Mesh(Ai::Scene scene, Ai::Mesh mesh)
    {
        _vertices = new Vec3[mesh.VertexCount];
        for (int i = 0; i < mesh.VertexCount; i++)
        {
            Ai.Vector3D vertexVector = mesh.Vertices[i];
            _vertices[i] = new Vec3(vertexVector.X, vertexVector.Y, vertexVector.Z);
        }

        _normals = new Vec3[mesh.VertexCount];
        for (int i = 0; i < mesh.VertexCount; i++)
        {
            Ai.Vector3D normalVector = mesh.Normals[i];
            _normals[i] = new Vec3(normalVector.X, normalVector.Y, normalVector.Z);
        }

        if (mesh.TextureCoordinateChannels.Length > 0)
        {
            _texCoords = new Vec2[mesh.TextureCoordinateChannels[0].Count];
            for (int i = 0; i < mesh.TextureCoordinateChannels[0].Count; i++)
            {
                Ai.Vector3D texCoords = mesh.TextureCoordinateChannels[0][i];
                _texCoords[i] = new Vec2(texCoords.X, texCoords.Y);
            }
            _hasTexture = true;
        }
        else
        {
            _texCoords = new Vec2[_vertices.Length];
        }

        _faces = mesh.Faces;

        _indices = mesh.GetUnsignedIndices();
    }

    /// <summary>
    /// Loads the model in the file and returns the loaded mesh.
    /// </summary>
    /// <param name="filename">file name</param>
    /// <returns>created mesh instance</returns>
    public static Mesh[] LoadFile(string filename)
    {
        NlcHelper.InThrow();
        NlcArgException.NullThrow(nameof(filename), filename);
        NlcHelper.FileThrow(filename);

        Ai::AssimpContext importer = new Ai::AssimpContext();
        Ai::Scene scene = importer.ImportFile(filename, Ai.PostProcessSteps.CalculateTangentSpace |
                                      Ai.PostProcessSteps.GenerateSmoothNormals |
                                      Ai.PostProcessSteps.JoinIdenticalVertices |
                                      Ai.PostProcessSteps.LimitBoneWeights |
                                      Ai.PostProcessSteps.RemoveRedundantMaterials |
                                      Ai.PostProcessSteps.SplitLargeMeshes |
                                      Ai.PostProcessSteps.Triangulate |
                                      Ai.PostProcessSteps.GenerateUVCoords |
                                      Ai.PostProcessSteps.SortByPrimitiveType |
                                      Ai.PostProcessSteps.FindDegenerates |
                                      Ai.PostProcessSteps.FindInvalidData);

        if (scene.MeshCount == 0)
        {

        }

        Mesh[] outputs = new Mesh[scene.MeshCount];
        for (int i = 0; i < scene.MeshCount; i++ )
        {
            outputs[i] = new Mesh(scene, scene.Meshes[i]);
        }

        return outputs;
    }
}