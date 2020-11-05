﻿using UnityEngine;

namespace FableCraft
{
    public interface IFableManager
    {
        void Play(StoryNode storyNode, int index, bool eventTriggered = false);
    }
}