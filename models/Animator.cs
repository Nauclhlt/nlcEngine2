namespace nlcEngine.Models;

using Ai = Assimp;

/// <summary>
/// Provides an animator.
/// </summary>
public class Animator
{
    List<Matrix4> _finalBoneMatrices = new List<Matrix4>();
    Animation _currentAnimation;
    float _currentTime;
    float _deltaTime;

    internal List<Matrix4> GetFinalBoneMatrices()
    {
        return _finalBoneMatrices;
    }

    /// <summary>
    /// Creates a new instance with the animation.
    /// </summary>
    /// <param name="animation">animation</param>
    public Animator(Animation animation)
    {
        _currentTime = 0.0f;
        _currentAnimation = animation;

        for (int i = 0; i < 100; i++)
        {
            _finalBoneMatrices.Add(Matrix4.Identity);
        }
    }

    /// <summary>
    /// Updates the animation.
    /// </summary>
    /// <param name="dt">deltaTime</param>
    public void UpdateAnimation(float dt)
    {
        _deltaTime = dt;
        if (_currentAnimation is not null)
        {
            _currentTime += _currentAnimation.TicksPerSecond * dt;
            _currentTime = _currentTime % _currentAnimation.Duration;

            
            CalculateBoneTransform(_currentAnimation.RootNode, Matrix4.Identity);
        }
    }

    /// <summary>
    /// Plays the animation.
    /// </summary>
    /// <param name="animation">animation</param>
    public void PlayAnimation(Animation animation)
    {
        _currentAnimation = animation;
        _currentTime = 0.0f;
    }

    void CalculateBoneTransform(AssimpNodeData node, Matrix4 parentTransform)
    {
        string nodeName = node.Name;
        Matrix4 nodeTransform = node.Transformation;

        Bone bone = _currentAnimation.FindBone(nodeName);

        if (bone is not null)
        {
            bone.Update(_currentTime);
            nodeTransform = bone.LocalTransform;
        }

        Matrix4 globalTransform = nodeTransform * parentTransform;
        //Matrix4 globalTransform = Matrix4.Identity;

        var boneInfoMap = _currentAnimation.BoneInfoMap;
        if (boneInfoMap.ContainsKey(nodeName))
        {
            int index = boneInfoMap[nodeName].Id;
            Matrix4 offset = boneInfoMap[nodeName].Offset;
            if (index >= 0 && index < 100)
            {
                _finalBoneMatrices[index] = offset * globalTransform;
                //_finalBoneMatrices[index] = Matrix4.Identity;
            }
            
        }

        for (int i = 0; i < node.ChildrenCount; i++)
        {
            CalculateBoneTransform(node.Children[i], globalTransform);
        }
    }
}