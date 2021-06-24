using System.Collections.Generic;
using Godot;
using Hopper.Core;
using Hopper.Shared.Attributes;
using Hopper.Utils;
using Hopper.View.Animations;

namespace Hopper.View
{
    public class AnimatorCycler : Node
    {
        private readonly DoubleList<Animator> allAnimators;

        public static readonly List<Animator> animators = new List<Animator>();
        
        public override void _Process(float delta)
        {
            foreach (var animator in animators)
            {
                animator.CycleAnim();
            }
        }
    }
}