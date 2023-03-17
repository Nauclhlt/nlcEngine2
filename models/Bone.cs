namespace nlcEngine.Models;

using Ai = Assimp;

internal struct KeyPosition
{
    public Vector3 Position;
    public float Timestamp;
}

internal struct KeyRotation
{
    public Quaternion Orientation;
    public float Timestamp;
}

internal struct KeyScale
{
    public Vector3 Scale;
    public float Timestamp;
}

internal class Bone
{
    List<KeyPosition> _positions = new List<KeyPosition>();
    List<KeyRotation> _rotations = new List<KeyRotation>();
    List<KeyScale> _scales = new List<KeyScale>();
    int _positionCount = 0;
    int _rotationCount = 0;
    int _scaleCount = 0;

    Matrix4 _localTransform;
    string _name;
    int _id;

    public string Name => _name;
    public int Id => _id;
    public Matrix4 LocalTransform => _localTransform;
    public int PositionCount => _positionCount;
    public int RotationCount => _rotationCount;
    public int ScaleCount => _scaleCount;
    public List<KeyPosition> Positions => _positions;
    public List<KeyRotation> Rotations => _rotations;
    public List<KeyScale> Scales => _scales;

    public Bone(string name, int id, Ai::NodeAnimationChannel channel)
    {
        _name = name;
        _id = id;
        _localTransform = Matrix4.Identity;

        _positionCount = channel.PositionKeyCount;
        for (int positionIndex = 0; positionIndex < channel.PositionKeyCount; positionIndex++)
        {
            Ai::Vector3D aiPosition = channel.PositionKeys[positionIndex].Value;
            float timestamp = (float)channel.PositionKeys[positionIndex].Time;
            KeyPosition data;
            data.Position = aiPosition.ToOpenTK();
            data.Timestamp = timestamp;
            _positions.Add(data);
        }

        _rotationCount = channel.RotationKeyCount;
        for (int rotationIndex = 0; rotationIndex < channel.RotationKeyCount; rotationIndex++)
        {
            Ai::Quaternion aiOrientation = channel.RotationKeys[rotationIndex].Value;
            float timestamp = (float)channel.RotationKeys[rotationIndex].Time;
            KeyRotation data;
            data.Orientation = aiOrientation.ToOpenTK();
            data.Timestamp = timestamp;
            _rotations.Add(data);
        }

        _scaleCount = channel.ScalingKeyCount;
        for (int scaleIndex = 0; scaleIndex < channel.ScalingKeyCount; scaleIndex++)
        {
            Ai::Vector3D aiScale = channel.ScalingKeys[scaleIndex].Value;
            float timestamp = (float)channel.ScalingKeys[scaleIndex].Time;
            KeyScale data;
            data.Scale = aiScale.ToOpenTK();
            data.Timestamp = timestamp;
            _scales.Add(data);
        }
    }

    public void Update(float animationTime)
    {
        Matrix4 translation = InterpolatePosition(animationTime);
        Matrix4 rotation = InterpolateRotation(animationTime);
        Matrix4 scale = InterpolateScale(animationTime);
        _localTransform = scale * rotation * translation;
        //_localTransform = Matrix4.Identity;
    }

    int GetPositionIndex(float animationTime)
    {
        for (int index = 0; index < _positionCount - 1; index++)
        {
            if (animationTime < _positions[index + 1].Timestamp)
            {
                return index;
            }
        }

        return 0;
    }

    int GetRotationIndex(float animationTime)
    {
        for (int index = 0; index < _rotationCount - 1; index++)
        {
            if (animationTime < _rotations[index + 1].Timestamp)
            {
                return index;
            }
        }

        return 0;
    }

    int GetScaleIndex(float animationTime)
    {
        for (int index = 0; index < _scaleCount - 1; index++)
        {
            if (animationTime < _scales[index + 1].Timestamp)
            {
                return index;
            }
        }

        return 0;
    }

    float GetScaleFactor(float lastTimeStamp, float nextTimeStamp, float animationTime)
    {
        float scaleFactor = 0.0f;
        float midwayLength = animationTime - lastTimeStamp;
        float framesDiff = nextTimeStamp - lastTimeStamp;
        scaleFactor = midwayLength / framesDiff;
        return scaleFactor;
    }

    Matrix4 InterpolatePosition(float animationTime)
    {
        if (_positionCount == 1)
        {
            return Matrix4.CreateTranslation(_positions[0].Position);
        }

        int p0Index = GetPositionIndex(animationTime);
        int p1Index = p0Index + 1;
        float scaleFactor = GetScaleFactor(_positions[p0Index].Timestamp, _positions[p1Index].Timestamp, animationTime);
        Vector3 finalPosition = ModelHelper.Mix(_positions[p0Index].Position, _positions[p1Index].Position, scaleFactor);
        return Matrix4.CreateTranslation(finalPosition);
    }

    Matrix4 InterpolateRotation(float animationTime)
    {
        if (_rotationCount == 1)
        {
            Matrix4 m = Matrix4.CreateFromQuaternion(_rotations[0].Orientation.Normalized());
            return m;
        }

        int p0Index = GetRotationIndex(animationTime);
        int p1Index = p0Index + 1;
        
        float scaleFactor = GetScaleFactor(_rotations[p0Index].Timestamp, _rotations[p1Index].Timestamp, animationTime);
        Quaternion finalRotation = ModelHelper.Mix(_rotations[p0Index].Orientation, _rotations[p1Index].Orientation, scaleFactor);
        return Matrix4.CreateFromQuaternion(finalRotation);
    }

    Matrix4 InterpolateScale(float animationTime)
    {
        if (_scaleCount == 1)
        {
            return Matrix4.CreateScale(_scales[0].Scale);
        }

        int p0Index = GetScaleIndex(animationTime);
        int p1Index = p0Index + 1;
        float scaleFactor = GetScaleFactor(_scales[p0Index].Timestamp, _scales[p1Index].Timestamp, animationTime);
        Vector3 finalScale = ModelHelper.Mix(_scales[p0Index].Scale, _scales[p1Index].Scale, scaleFactor);
        return Matrix4.CreateTranslation(finalScale);
    }
}