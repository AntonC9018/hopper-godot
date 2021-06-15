using System;
using System.Linq;
using Godot;
using Hopper.Core.Components;
using Hopper.Core.Targeting;
using Hopper.View.Utils;
using Transform = Hopper.Core.WorldNS.Transform;


namespace Hopper.View.Animations
{
    public partial class AttackAnim : Animator, IComponent
    {
        private Sprite slashSprite;
        public float Peak = 1f;
        public bool isLookingRight = true;
        

        // TODO: replace this stuff
        // public override void _Ready()
        // {
        //     var parent = (EntityAnimator)GetParent();
        //     SetSlashSprite(parent.GetLazy<Sprite>(EntityAnimator.NodeIndex.Slash));
        // }
        //
        // public override void _Process(float delta)
        // {
        //     if (isRunning)
        //         CycleAttack();
        // }

        [Shared.Attributes.Export(Chain = "Attacking.Do")]
        public void SetupAnim(EntityAnimator entityAnimator, AttackTargetingContext targetingContext)
        {
            StopAnim();
            slashSprite = entityAnimator.slashSprite;

            // there's usually only one target per attack, for now at least
            var targetPos = targetingContext.targetContexts.First().position.ToSceneVector();

            slashSprite.Position = targetPos;
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

            slashSprite.SelfModulate = new Color(1, 1, 1, Helper.SquareInterpolation(Peak, Duration, time));
        }
        
        public override void StopAnim()
        {
            base.StopAnim();
            
            if (slashSprite is null)
                return;
            
            slashSprite.SelfModulate = new Color(1, 1, 1, 0);
        }
        
        
        // TODO: implement these
        public void FlipEntity()
        {
            slashSprite.Scale = new Vector2(-slashSprite.Scale.x, slashSprite.Scale.y);
            isLookingRight = !isLookingRight;
        }

        public void SetDirection(bool isAttackDirRight)
        {
            if (isAttackDirRight != isLookingRight)
                FlipEntity();
        }

        public void SetDirection(Vector2 targetPos, Vector2 entityPos)
        {
            if (Math.Abs(targetPos.x - entityPos.x) < 10f) 
                return;
            
            bool isAttackDirRight = targetPos.x - entityPos.x > 0;

            if (isAttackDirRight != isLookingRight)
                FlipEntity();
        }
    }
}