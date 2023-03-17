namespace nlcEngine.Models;

using Ai = Assimp;

/// <summary>
/// Contains mesh data.
/// </summary>
public sealed class Mesh
{
    internal static readonly int MAX_BONE_INFLUENCE = 4;

    Dictionary<string, BoneInfo> _boneInfoMap = new Dictionary<string, BoneInfo>();
    int _boneCount = 0;

    Vertex[] _vertices;
    uint[] _indices;
    List<Ai::Face> _faces;
    bool _hasTexture;

    internal Vertex[] Vertices => _vertices;

    /// <summary>
    /// Gets the array of the indices.
    /// </summary>
    public uint[] Indices => _indices;
    /// <summary>
    /// Gets whether the mesh has a texture.
    /// </summary>
    public bool HasTexture => _hasTexture;

    internal Dictionary<string, BoneInfo> GetBoneInfoMap()
    {
        return _boneInfoMap;
    }

    internal int GetBoneCount()
    {
        return _boneCount;
    }

    private Mesh()
    {
        // Hidden parameterless constructor
    }

    private Mesh(Ai::Scene scene, Ai::Mesh mesh)
    {
        Ai::Material material = scene.Materials[mesh.MaterialIndex];

        _vertices = new Vertex[mesh.VertexCount];
        
        for (int i = 0; i < mesh.VertexCount; i++)
        {
            Vertex vertex = new Vertex();
            SetVertexBoneDataToDefault(ref vertex);

            Ai.Vector3D vertexVector = mesh.Vertices[i];
            Ai.Color4D color = material.ColorDiffuse;
            Ai.Vector3D normalVector = mesh.Normals[i];
            if (mesh.HasTangentBasis)
            {
                Ai.Vector3D tangentVector = mesh.Tangents[i];
                Ai.Vector3D bitangentVector = mesh.BiTangents[i];

                vertex.Tangent = new Vector3(tangentVector.X, tangentVector.Y, tangentVector.Z);
                vertex.Bitangent = new Vector3(bitangentVector.X, bitangentVector.Y, bitangentVector.Z);
            }

            vertex.Position = new Vector3(vertexVector.X, vertexVector.Y, vertexVector.Z);
            vertex.Normal = new Vector3(normalVector.X, normalVector.Y, normalVector.Z);
            vertex.Color = Color.FromFloats(color.R, color.G, color.B, color.A);
            if (mesh.HasTextureCoords(0))
            {
                Ai.Vector3D coord = mesh.TextureCoordinateChannels[0][i];
                vertex.TexCoords = new Vector2(coord.X, coord.Y);
            }

            

            _vertices[i] = vertex;
        }

        

        _faces = mesh.Faces;

        _indices = mesh.GetUnsignedIndices();

        ExtractBoneWeights(_vertices, mesh, scene);
    }

    void SetVertexBoneData(ref Vertex vertex, int boneId, float weight)
    {
        if (vertex.BoneIds0 < 0)
        {
            vertex.BoneIds0 = boneId;
            vertex.Weights0 = weight;
        }

        if (vertex.BoneIds1 < 0)
        {
            vertex.BoneIds1 = boneId;
            vertex.Weights1 = weight;
        }

        if (vertex.BoneIds2 < 0)
        {
            vertex.BoneIds2 = boneId;
            vertex.Weights2 = weight;
        }

        if (vertex.BoneIds3 < 0)
        {
            vertex.BoneIds3 = boneId;
            vertex.Weights3 = weight;
        }

        //onsole.WriteLine(vertex.BoneIds0 + ", " + vertex.BoneIds1 + ", " + vertex.BoneIds2 + ", " + vertex.BoneIds3);
    }

    void ExtractBoneWeights(Vertex[] vertices, Ai::Mesh mesh, Ai::Scene scene)
    {
        for (int boneIndex = 0; boneIndex < mesh.BoneCount; boneIndex++)
        {
            int boneId = -1;
            string boneName = mesh.Bones[boneIndex].Name;
            if (!_boneInfoMap.ContainsKey(boneName))
            {
                BoneInfo newBoneInfo = new BoneInfo();
                newBoneInfo.Id = _boneCount;
                Matrix4 offset = mesh.Bones[boneIndex].OffsetMatrix.ToOpenTK();
                newBoneInfo.Offset = offset;
                _boneInfoMap[boneName] = newBoneInfo;
                boneId = _boneCount;
                _boneCount++;
            }
            else
            {
                boneId = _boneInfoMap[boneName].Id;
            }

            var weights = mesh.Bones[boneIndex].VertexWeights;
            int weightCount = mesh.Bones[boneIndex].VertexWeightCount;

            for (int weightIndex = 0; weightIndex < weightCount; weightIndex++)
            {
                int vertexId = weights[weightIndex].VertexID;
                float weight = weights[weightIndex].Weight;
                //Console.WriteLine(boneId);
                //Console.WriteLine(weight);
                SetVertexBoneData(ref vertices[vertexId], boneId, weight);
            }
        }
    }

    void SetVertexBoneDataToDefault(ref Vertex vertex)
    {
        vertex.BoneIds0 = -1;
        vertex.Weights0 = 0.0f;
        vertex.BoneIds1 = -1;
        vertex.Weights1 = 0.0f;
        vertex.BoneIds2 = -1;
        vertex.Weights2 = 0.0f;
        vertex.BoneIds3 = -1;
        vertex.Weights3 = 0.0f;
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
        //Ai::Scene scene = importer.ImportFile(filename, Ai.PostProcessPreset.TargetRealTimeQuality);

        if (scene.MeshCount == 0)
        {
            throw new NlcCommonException("The file has no mesh.");
        }

        Mesh[] outputs = new Mesh[scene.MeshCount];
        for (int i = 0; i < scene.MeshCount; i++ )
        {
            //Console.WriteLine("NAME=" + scene.Meshes[i].Name + "  VERTICES=" + scene.Meshes[i].VertexCount);
            outputs[i] = new Mesh(scene, scene.Meshes[i]);
        }

        return outputs;
    }
}