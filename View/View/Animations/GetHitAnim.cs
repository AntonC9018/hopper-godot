using Godot;
using Hopper.Core.Components;

namespace Hopper.View.Animations
{
    public class GetHitAnim : Animator, IComponent
    {
        private Sprite entitySprite;
        
        public float Peak = 1f;
        
        // TODO: replace this
        /*
        public override void _Ready()
        {
            var parent = (EntityAnimator)GetParent();
            SetEntitySprite(parent.GetLazy(EntityAnimator.NodeIndex.Entity));
        }


        public override void _Process(float delta)
        {
            if (isRunning)
                CycleAttack();
        }
        */
        
        public override void InitAnimator(EntityAnimator entityAnimator)
        {
            entitySprite = entityAnimator.GetLazy(EntityAnimator.NodeIndex.Entity);
        }

        public void SetupAnim()
        {
            StartAnim();
        }
        
        public override void CycleAnim()
        {
            base.CycleAnim();

            var time = GetElapsed();
            
            if (time > Duration)
            {
                StopAnim();
                return;
            }

            entitySprite.SelfModulate = new Color(1, 1, 1, 1 - Helper.SquareInterpolation(Peak, Duration, time));
        }

        public override void StopAnim()
        {
            base.StopAnim();
            entitySprite.SelfModulate = new Color(1, 1, 1, 1);
        }
    }
}