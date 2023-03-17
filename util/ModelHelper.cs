namespace nlcEngine;

internal static class ModelHelper
{
    public static Vector3 Mix(Vector3 vector1, Vector3 vector2, float scaleFactor)
    {
        //x * (1.0 - a) + y * a
        return new Vector3(vector1.X * (1.0f - scaleFactor) + vector2.X * scaleFactor,
                        vector1.Y * (1.0f - scaleFactor) + vector2.Y * scaleFactor,
                        vector1.Z * (1.0f - scaleFactor) + vector2.Z * scaleFactor);
    }

    public static Quaternion Mix(Quaternion quat1, Quaternion quat2, float scaleFactor)
    {
        return new Quaternion(
            quat1.X * (1.0f - scaleFactor) + quat2.X * scaleFactor,
            quat1.Y * (1.0f - scaleFactor) + quat2.Y * scaleFactor,
            quat1.Z * (1.0f - scaleFactor) + quat2.Z * scaleFactor,
            quat1.W * (1.0f - scaleFactor) + quat2.W * scaleFactor
        );
    }
}