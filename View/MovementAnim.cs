using System;
using Godot;
using System.Diagnostics;

namespace Hopper_Godot.View
{
    public abstract class MovementAnim : Node2D
    {
        public enum EMovementType
        {
            Walk,
            Jump,
            Slide
        }
        
        private Stopwatch animStopwatch = new Stopwatch();

        private Vector2 startPos;
        private Vector2 destPos;
        private bool isRunning = false;
        private Node2D spriteNode;
        private EMovementType eMovementType;

        public static float Duration = 0.33f;
        public static float Peak = 25f;

        public void SetSpriteNode(Node2D spriteNode)
        {
            this.spriteNode = spriteNode;
        }

        public override void _Process(float delta)
        {
            if (isRunning)
                CycleMovement();
        }

        public void StartMovement(Vector2 source, Vector2 destination, EMovementType eMovementType)
        {
            startPos = spriteNode.Position = source; 
            destPos = eMovementType == EMovementType.Jump ? source : destination;    // set destination equal to source if jumping
            this.eMovementType = eMovementType;
            isRunning = true;
            
            animStopwatch.Reset();
            animStopwatch.Start();
        }

        private void CycleMovement()
        {
            var time = (float) animStopwatch.ElapsedTicks / Stopwatch.Frequency;
            
            if (time > Duration)
            {
                StopMovement();
                return;
            }

            switch (eMovementType)
            {
                case EMovementType.Walk:
                    var walkFramePos = Helper.LinearInterpolation(startPos, destPos, Duration, time);

                    walkFramePos.y += Helper.SquareInterpolation(Peak, Duration, time);    // little bit of bobbing for the jump

                    spriteNode.Position = walkFramePos;
                    break;
                
                case EMovementType.Slide:
                    //var slideFramePos = LinearInterpolation(time);
                    var slideFramePos = Helper.LinearInterpolation(startPos, destPos, Duration, time);
                    
                    spriteNode.Position = slideFramePos;
                    break;
                
                case EMovementType.Jump:
                    var jumpFramePos = startPos;

                    jumpFramePos.y += Helper.SquareInterpolation(Peak, Duration, time);    // little bit of bobbing for the jump

                    spriteNode.Position = jumpFramePos;
                    break;
            }
        }

        private void StopMovement()
        {
            animStopwatch.Stop();
            animStopwatch.Reset();
            isRunning = false;
            spriteNode.Position = destPos;
        }
    }
}