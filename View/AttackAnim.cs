using System.Diagnostics;
using Godot;

namespace Hopper_Godot.View
{
    public abstract class AttackAnim : Node2D
    {
        private Stopwatch animStopwatch = new Stopwatch();
        private Sprite slashSprite;
        private bool isRunning;
        
        public float Duration = 0.33f;
        public float Peak = 1f;

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

        private void StopAttack()
        {
            animStopwatch.Stop();
            animStopwatch.Reset();
            isRunning = false;
            slashSprite.SelfModulate = new Color(1, 1, 1, 0);
        }
    }
}