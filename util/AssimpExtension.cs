namespace nlcEngine;

using Ai = Assimp;

internal static class AssimpExtension
{
    public static Matrix4 ToOpenTK(this Ai::Matrix4x4 input)
    {
        return new Matrix4(input.A1, input.B1, input.C1, input.D1,
                                   input.A2, input.B2, input.C2, input.D2,
                                   input.A3, input.B3, input.C3, input.D3,
                                   input.A4, input.B4, input.C4, input.D4);
    }

    public static Vector3 ToOpenTK(this Ai::Vector3D vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }

    public static Quaternion ToOpenTK(this Assimp.Quaternion quaternion)
    {
        return new Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}