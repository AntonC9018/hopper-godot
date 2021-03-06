﻿using System;
using System.Diagnostics;
using Godot;
using Hopper.Core;
using Hopper.Core.Components;
using Hopper.Shared.Attributes;
using Hopper.TestContent.SlidingNS;
using Hopper.Utils.Vector;
using Hopper.View.Utils;
using Transform = Hopper.Core.WorldNS.Transform;
using Vector2 = Godot.Vector2;

namespace Hopper.View.Animations
{
    public partial class MovementAnim : Animator, IComponent
    {
        public enum EMovementType
        {
            Walk,
            Jump,
            Slide
        }
        
        private Vector2 startPos;
        private Vector2 destPos;
        private Sprite entitySprite;
        private EMovementType eMovementType;

        public static float Peak = -25f;
        public bool isLookingRight = true;

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
                CycleMovement();
        }
        */
        
        [Hopper.Shared.Attributes.Export(Chain = "Displaceable.After", Dynamic = true)]
        public void SetupAnim(Entity actor, EntityAnimator entityAnimator, IntVector2 initialPosition, IntVector2 newPosition)
        {
            StopAnim();

            entitySprite = entityAnimator.entitySprite;
            
            startPos = initialPosition.ToSceneVector();
            destPos = newPosition.ToSceneVector();

            eMovementType = actor.HasSlidingEntityModifier() ? EMovementType.Slide : EMovementType.Walk;
            
            StartAnim();
            
            GD.Print("Started moving from " + initialPosition + " towards " + newPosition);
        }

        public void SetupJump()
        {
            StopAnim();

            eMovementType = EMovementType.Jump;
            
            StartAnim();
        }

        public override void CycleAnim()
        {
            base.CycleAnim();
            
            Console.WriteLine("Cycling movement");

            var time = GetElapsed();
            
            if (time > Duration)
            {
                StopAnim();
                return;
            }

            switch (eMovementType)
            {
                case EMovementType.Walk:
                    var walkFramePos = Helper.LinearInterpolation(startPos, destPos, Duration, time);

                    walkFramePos.y += Helper.SquareInterpolation(Peak, Duration, time);    // little bit of bobbing for the jump

                    entitySprite.Position = walkFramePos;
                    break;
                
                case EMovementType.Slide:
                    //var slideFramePos = LinearInterpolation(time);
                    var slideFramePos = Helper.LinearInterpolation(startPos, destPos, Duration, time);
                    
                    entitySprite.Position = slideFramePos;
                    break;
                
                case EMovementType.Jump:
                    var jumpFramePos = startPos;

                    jumpFramePos.y += Helper.SquareInterpolation(Peak, Duration, time);    // little bit of bobbing for the jump

                    entitySprite.Position = jumpFramePos;
                    break;
            }
        }

        public override void StopAnim()
        {
            base.StopAnim();
            
            if (entitySprite is null)
                return;
            
            entitySprite.Position = destPos;
        }
        
        
        // TODO: implement these
        public void FlipEntity()
        {
            entitySprite.Scale = new Vector2(-entitySprite.Scale.x, entitySprite.Scale.y);
            isLookingRight = !isLookingRight;
        }

        public void LookAt(bool lookRight)
        {
            if (isLookingRight != lookRight)
                FlipEntity();
        }

        public void LookAt(Vector2 direction)
        {
            LookAt(GetRelativeDirection(direction));
        }

        public bool GetRelativeDirection(Vector2 direction)
        {
            if (Math.Abs(direction.x - entitySprite.Position.x) < 10f)
                return isLookingRight;
            
            return direction.x - entitySprite.Position.x > 0;
        }
        
        public void DefaultPreset(Hopper.Core.Entity subject)
        {
            SetupAnimHandlerWrapper.HookTo(subject);
        }

    }
}