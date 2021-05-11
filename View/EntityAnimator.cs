using Godot;
using Hopper.Core.Retouchers;

namespace Hopper_Godot.View
{
    public class EntityAnimator : Node2D
    {
        private SpriteSet entitySpriteSet;
        private Sprite entitySprite;
        private Sprite slashSprite;

        private MovementAnim movementAnim;
        private AttackAnim attackAnim;
        private GetHitAnim getHitAnim;
        
        private Vector2 actualPosition;
        
        public void SetAllLinks()
        {
            entitySprite = (Sprite) GetNode("EnemySprite");
            slashSprite = (Sprite) GetNode("SlashSprite");
			
            movementAnim = (MovementAnim) GetNode("MovementAnim");
            movementAnim.SetSpriteNode(entitySprite);

            attackAnim = (AttackAnim) GetNode("AttackAnim");
            attackAnim.SetSlashSprite(slashSprite);
            
            getHitAnim = (GetHitAnim) GetNode("GetHitAnim");
            getHitAnim.SetEntitySprite(entitySprite);
        }
        
        public void SkipAnimations()
        {
            movementAnim.StopMovement();
            attackAnim.StopAttack();
            getHitAnim.StopAnim();
        }

        public void Move(Vector2 destination, MovementAnim.EMovementType movementType)
        {
            SkipAnimations();
            movementAnim.StartMovement(actualPosition, destination, movementType);
            actualPosition = destination;
        }
        
        public void SetIdle()
        {
            entitySprite.Texture = entitySpriteSet.IdleTexture;
        }
        
        public void SetTelegraph()
        {
            entitySprite.Texture = entitySpriteSet.TelegraphTexture;
        }

        public void Attack(Vector2 targetPos)
        {
            SkipAnimations();
            entitySprite.Texture = entitySpriteSet.IdleTexture;
            attackAnim.StartAttack(targetPos);
        }

        public void GetHit()
        {
            SkipAnimations();
            getHitAnim.StartAttack();
        }

        public void DeleteEntity()
        {
            QueueFree();
        }
    }
}