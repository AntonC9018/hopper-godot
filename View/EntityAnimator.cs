using Godot;

namespace Hopper_Godot.View
{
    public class EntityAnimator : Node2D
    {
        private SpriteSet entitySpriteSet;
        private Sprite entitySprite;
        private Sprite slashSprite;

        private MovementAnim movementAnim;
        private AttackAnim attackAnim;
        private Vector2 actualPosition;
        
        public void SetAllLinks()
        {
            entitySprite = (Sprite) GetNode("EnemySprite");
            slashSprite = (Sprite) GetNode("SlashSprite");
			
            movementAnim = (MovementAnim) GetNode("MovementAnim");
            movementAnim.SetSpriteNode(entitySprite);

            attackAnim = (AttackAnim) GetNode("AttackAnim");
            attackAnim.SetSlashSprite(slashSprite);
        }
        
        public void SkipAnimations()
        {
            movementAnim.StopMovement();
            attackAnim.StopAttack();
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

        public void DeleteEntity()
        {
            QueueFree();
        }
    }
}