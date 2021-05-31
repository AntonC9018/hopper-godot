using Godot;
using Godot.Collections;

namespace Hopper.View.EntityAnimTesting
{
    public class EntityAnimatorV3 : Node
    {
        private struct ComponentPath
        {
            public static readonly string EntitySpritePath = "EntitySprite";
            public static readonly string SlashSpritePath = "SlashSprite";
            public static readonly string MovementAnimPath = "MovementAnim";
            public static readonly string AttackAnimPath = "AttackAnim";
            public static readonly string GetHitAnimPath = "GetHitAnim";
        }

        public enum NodeIndex
        {
            EntitySprite = 0,
            SlashSprite = 1,
            MovementAnim = 2,
            AttackAnim = 3,
            GetHitAnim = 4
        }

        private static string[] ComponentPathArr =
        {
            "EntitySprite",
            "SlashSprite",
            "MovementAnim",
            "AttackAnim",
            "GetHitAnim"
        };


        private SpriteSet entitySpriteSet;

        private static Node2D backupNode;

        
        
        
        public Vector2 actualPosition;

        private Dictionary<NodeIndex, Node> store = new Dictionary<NodeIndex, Node>();


        public T GetLazy<T>(NodeIndex nodeIndex) where T:Node
        {
            if (store.TryGetValue(nodeIndex, out var node))
            {
                node = LoadNode(ComponentPathArr[(int) nodeIndex]);
                store.Add(nodeIndex, node);
                
                /*
                // component init
                // ideally get rid of this 
                switch (nodeIndex)
                {
                    case NodeIndex.MovementAnim :
                        ((MovementAnim)node).SetEntitySprite(GetLazy<Sprite>(NodeIndex.EntitySprite));
                        break;
                
                    case NodeIndex.AttackAnim :
                        ((AttackAnim)node).SetSlashSprite(GetLazy<Sprite>(NodeIndex.SlashSprite));
                        break;
                
                    case NodeIndex.GetHitAnim :
                        ((GetHitAnim)node).SetEntitySprite(GetLazy<Sprite>(NodeIndex.EntitySprite));
                        break;
                }
            */
            }

            return (T)node;
        }

        public T GetOrNull<T>(NodeIndex nodeIndex) where T:Node
        {
            if (store.TryGetValue(nodeIndex, out var node))
                return (T) node;

            return null;
        }
        


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
            //the replacement looks kinda ugly even tho we have to use it only once
            GetOrNull<MovementAnim>(NodeIndex.MovementAnim)?.StopMovement();
            
            // movementAnim?.StopMovement();
            // attackAnim?.StopAttack();
            // getHitAnim?.StopAnim();

            // ? before function call to make sure the object is not null
        }

        public void Move(Vector2 destination, MovementAnimV3.EMovementType movementType)
        {
            if (entitySprite is null)
                entitySprite = (Sprite) LoadNode(ComponentPath.EntitySpritePath);

            if (movementAnim is null)
            {
                movementAnim = (MovementAnimV3) LoadNode(ComponentPath.MovementAnimPath);
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
                attackAnim = (AttackAnimV3) LoadNode(ComponentPath.AttackAnimPath);
                attackAnim.SetSlashSprite(slashSprite);
            }

            movementAnim.LookAt(targetPos);

            attackAnim.SetDirection(movementAnim.isLookingRight);
            attackAnim.StartAttack(targetPos);
        }

        public void GetHit()
        {
            if (entitySprite is null)
                entitySprite = (Sprite) LoadNode(ComponentPath.EntitySpritePath);

            if (getHitAnim is null)
            {
                getHitAnim = (GetHitAnimV3) LoadNode(ComponentPath.GetHitAnimPath);
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