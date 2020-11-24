using UnityEngine;

namespace FableCraft
{
    public interface IFableManager
    {
        void Play(StoryNode storyNode, int nodeIndex, int connectedOption, int index);
    }
}
