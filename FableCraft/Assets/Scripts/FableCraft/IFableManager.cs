namespace FableCraft
{
    public interface IFableManager
    {
        void Play(StoryNode storyNode, int index, float duration, bool eventTriggered = false);
    }
}
