using System.Diagnostics;
using Godot;

//TODO: mirror sprite based on attack direction

namespace Hopper_Godot.View
{
    public abstract class GetHitAnim : Node2D
    {
        private Stopwatch animStopwatch = new Stopwatch();
        private Sprite entitySprite;
        private bool isRunning;
        
        public float Duration = 0.33f;
        public float Peak = 1f;

        public override void _Process(float delta)
        {
            if (isRunning)
                CycleAttack();
        }

        public void SetEntitySprite(Sprite entitySprite)
        {
            this.entitySprite = entitySprite;
        }

        public void StartAttack()
        {
            animStopwatch.Reset();
            animStopwatch.Start();
            isRunning = true;
        }

        private void CycleAttack()
        {
            var time = (float) animStopwatch.ElapsedTicks / Stopwatch.Frequency;
            
            if (time > Duration)
            {
                StopAnim();
                return;
            }


            entitySprite.SelfModulate = new Color(1, 1, 1, 1 - Helper.SquareInterpolation(Peak, Duration, time));
        }

        public void StopAnim()
        {
            animStopwatch.Stop();
            animStopwatch.Reset();
            isRunning = false;
            entitySprite.SelfModulate = new Color(1, 1, 1, 0);
        }
    }
}