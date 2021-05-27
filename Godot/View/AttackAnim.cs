using System;
using System.Diagnostics;
using Godot;

//TODO: mirror sprite based on attack direction

namespace Hopper_Godot.View
{
    public abstract class AttackAnim : Node2D
    {
        private Stopwatch animStopwatch = new Stopwatch();
        private Sprite slashSprite;
        private bool isRunning;
        
        public float Duration = 0.33f;
        public float Peak = 1f;
        public bool isLookingRight = true;

        public override void _Process(float delta)
        {
            if (isRunning)
                CycleAttack();
        }

        public void SetSlashSprite(Sprite slashSprite)
        {
            this.slashSprite = slashSprite;
        }

        public void StartAttack(Vector2 targetPos)
        {
            slashSprite.Position = targetPos;
            
            animStopwatch.Reset();
            animStopwatch.Start();
            isRunning = true;
        }

        private void CycleAttack()
        {
            var time = (float) animStopwatch.ElapsedTicks / Stopwatch.Frequency;
            
            if (time > Duration)
            {
                StopAttack();
                return;
            }


            slashSprite.SelfModulate = new Color(1, 1, 1, Helper.SquareInterpolation(Peak, Duration, time));
        }

        public void StopAttack()
        {
            animStopwatch.Stop();
            animStopwatch.Reset();
            isRunning = false;
            slashSprite.SelfModulate = new Color(1, 1, 1, 0);
        }
        
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