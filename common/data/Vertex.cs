namespace nlcEngine.Models;

[StructLayout(LayoutKind.Sequential)]
internal struct Vertex
{
    public Vector3 Position;
    public Vector3 Normal;
    public Color Color;
    public Vector2 TexCoords;
    public Vector3 Tangent;
    public Vector3 Bitangent;
    public int BoneIds0;
    public int BoneIds1;
    public int BoneIds2;
    public int BoneIds3;
    public float Weights0;
    public float Weights1;
    public float Weights2;
    public float Weights3;
}