using Godot;
using Hopper.Core.Components;

namespace Hopper.View.Animations
{
    public partial class GetHitAnim : Animator, IComponent
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
        
        // If it's blinking, it should be applied/removed when invincibility is applied/removed
        [Hopper.Shared.Attributes.Export(Chain = "Attackable.After", Dynamic = true)]
        public void SetupAnim(EntityAnimator entityAnimator)
        {
            StopAnim();

            entitySprite = entityAnimator.entitySprite;
            
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
            
            if (entitySprite is null)
                return;
            
            entitySprite.SelfModulate = new Color(1, 1, 1, 1);
        }
        
        public void DefaultPreset(Hopper.Core.Entity subject)
        {
            SetupAnimHandlerWrapper.HookTo(subject);
        }
    }
}