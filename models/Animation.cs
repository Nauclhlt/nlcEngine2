namespace nlcEngine.Models;

using Ai = Assimp;

internal class AssimpNodeData
{
    public Matrix4 Transformation;
    public string Name;
    public int ChildrenCount = 0;
    public List<AssimpNodeData> Children = new List<AssimpNodeData>();
}

/// <summary>
/// Represents an animation.
/// </summary>
public class Animation
{
    
    float _duration;
    int _ticksPerSecond;
    List<Bone> _bones = new List<Bone>();
    AssimpNodeData _rootNode;
    Dictionary<string, BoneInfo> _boneInfoMap;
    internal float Duration => _duration;
    internal int TicksPerSecond => _ticksPerSecond;
    internal List<Bone> Bones => _bones;
    internal AssimpNodeData RootNode => _rootNode;
    internal Dictionary<string, BoneInfo> BoneInfoMap => _boneInfoMap;

    internal Animation(string animationPath, Mesh mesh)
    {
        Ai::AssimpContext importer = new Ai.AssimpContext();

        Ai::Scene scene = importer.ImportFile(animationPath, Ai::PostProcessPreset.TargetRealTimeQuality);
        var animation = scene.Animations[0];
        _duration = (float)animation.DurationInTicks;
        _ticksPerSecond = (int)animation.TicksPerSecond;

        _rootNode = new AssimpNodeData();

        ReadHierarchyData(_rootNode, scene.RootNode);
        ReadMissingBones(animation, mesh);
    }

    internal Bone FindBone(string name)
    {
        return _bones.FirstOrDefault(item => item.Name == name);
    }

    void ReadMissingBones(Ai::Animation animation, Mesh mesh)
    {
        int size = animation.NodeAnimationChannelCount;

        var boneInfoMap = mesh.GetBoneInfoMap();
        int boneCount = mesh.GetBoneCount();

        for (int i = 0; i < size; i++)
        {
            var channel = animation.NodeAnimationChannels[i];
            string boneName = channel.NodeName;

            if (!boneInfoMap.ContainsKey(boneName))
            {
                boneInfoMap[boneName] = new BoneInfo(){Id=boneCount};
                boneCount++;
            }

            _bones.Add(new Bone(channel.NodeName, boneInfoMap[channel.NodeName].Id, channel));
        }

        _boneInfoMap = boneInfoMap;
    }

    void ReadHierarchyData(AssimpNodeData dest, Ai::Node src)
    {
        dest.Name = src.Name;
        dest.Transformation = src.Transform.ToOpenTK();
        dest.ChildrenCount = src.ChildCount;

        for (int i = 0; i < src.ChildCount; i++)
        {
            AssimpNodeData newData = new AssimpNodeData();
            ReadHierarchyData(newData, src.Children[i]);
            dest.Children.Add(newData);
        }
    }
}