using Godot;

namespace Hopper.Godot.View
{
    public class EntityAnimator : Node2D
    {
        private struct ComponentPath
        {
            public static readonly string EntitySpritePath = "EntitySprite";
            public static readonly string SlashSpritePath = "SlashSprite";
            public static readonly string MovementAnimPath = "MovementAnim";
            public static readonly string AttackAnimPath = "AttackAnim";
            public static readonly string GetHitAnimPath = "GetHitAnim";
        }

        private SpriteSet entitySpriteSet;
        private Sprite entitySprite;
        private Sprite slashSprite;
        private static Node2D backupNode;

        private MovementAnim movementAnim;
        private AttackAnim attackAnim;
        private GetHitAnim getHitAnim;

        public Vector2 actualPosition;

        public override void _Ready()
        {
            if (backupNode is null)
            {
                backupNode = (Node2D) GetParent().FindNode("BackupNode");
                GD.Print("Loaded backup node " + backupNode.Name);
            }
        }

        public void SkipAnimations()
        {
            movementAnim?.StopMovement();
            attackAnim?.StopAttack();
            getHitAnim?.StopAnim();

            // ? before function call to make sure the object is not null
        }

        public void Move(Vector2 destination, MovementAnim.EMovementType movementType)
        {
            if (entitySprite is null)
                entitySprite = (Sprite)LoadNode(ComponentPath.EntitySpritePath);
            
            if (movementAnim is null)
            {
                movementAnim = (MovementAnim) LoadNode(ComponentPath.MovementAnimPath);
                movementAnim.SetEntitySprite(entitySprite);
            }

            movementAnim.StartMovement(actualPosition, destination, movementType);
            actualPosition = destination;
        }

        public void SetIdle()
        {
            if (entitySprite is null)
                entitySprite = (Sprite) LoadNode(ComponentPath.EntitySpritePath);

            if (entitySpriteSet is null)
                entitySpriteSet = new SpriteSet();

            entitySprite.Texture = entitySpriteSet.IdleTexture;
        }

        public void SetTelegraph()
        {
            if (entitySprite is null)
                entitySprite = (Sprite) LoadNode(ComponentPath.EntitySpritePath);

            if (entitySpriteSet is null)
                entitySpriteSet = new SpriteSet();

            entitySprite.Texture = entitySpriteSet.TelegraphTexture;
        }

        public void Attack(Vector2 targetPos)
        {
            SetIdle();
            
            if (slashSprite is null)
                slashSprite = (Sprite) LoadNode(ComponentPath.SlashSpritePath);

            if (attackAnim is null)
            {
                attackAnim = (AttackAnim) LoadNode(ComponentPath.AttackAnimPath);
                attackAnim.SetSlashSprite(slashSprite);
            }
            
            movementAnim.LookAt(targetPos);

            attackAnim.SetDirection(movementAnim.isLookingRight);
            attackAnim.StartAttack(targetPos);
        }

        public void GetHit()
        {
            if (entitySprite is null)
                entitySprite = (Sprite)LoadNode(ComponentPath.EntitySpritePath);
            
            if (getHitAnim is null)
            {
                getHitAnim = (GetHitAnim) LoadNode(ComponentPath.GetHitAnimPath);
                getHitAnim.SetEntitySprite(entitySprite);
            }
            
            getHitAnim.StartAnim();
        }

        public void DeleteEntity()
        {
            QueueFree();
        }

        private Node2D LoadNode(string name)
        {
            var node2D = (Node2D) GetNodeOrNull(name);

            if (node2D is null)
            {
                node2D = (Node2D) backupNode.GetNode(name).Duplicate();
                AddChild(node2D);
            }

            return node2D;
        }
    }
}
