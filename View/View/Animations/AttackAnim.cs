using System;
using System.Diagnostics;
using Godot;


namespace Hopper.View.Animations
{
    public class AttackAnim : Animator
    {
        private Sprite slashSprite;
        private EntityAnimator parent;
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

        public void InitAnimator(EntityAnimator parent, Sprite slashSprite)
        {
            this.parent = parent;
            this.slashSprite = slashSprite;
        }

        public void SetupAttack(Vector2 targetPos)
        {
            slashSprite.Position = targetPos;
            StartAnim();
        }
        
        
        public override void CycleAnim()
        {
            var time = GetElapsed();
            
            if (time > Duration)
            {
                StopAnim();
                return;
            }

            // TODO: deal with these casts
            slashSprite.SelfModulate = new Color(1, 1, 1, Helper.SquareInterpolation(Peak, (float)Duration, (float)time));
        }

        protected override void StopAnim()
        {
            base.StopAnim();
            slashSprite.SelfModulate = new Color(1, 1, 1, 0);
        }
        
        
        // TODO: probably get rid of these
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