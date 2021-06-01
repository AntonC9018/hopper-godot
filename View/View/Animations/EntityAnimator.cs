using Godot;
using Godot.Collections;

// TODO: check entity sprite set for textures in case of sprite lazy loading
// TODO: load entity sprite in _Ready out of the entity sprite set

namespace Hopper.View.Animations
{
	public class EntityAnimator : Node2D
	{
		private static string[] ComponentPathArr =
		{
			"EntitySprite",
			"SlashSprite",
			"MovementAnim",
			"AttackAnim",
			"GetHitAnim"
		};
		
		public class Index<T>
		{
			public readonly int Id;

			public Index(int Id)
			{
				this.Id = Id;
			}

			public override bool Equals(object obj)
			{
				return Id.Equals((obj as Index<T>).Id);
			}

			public override int GetHashCode()
			{
				return Id.GetHashCode();
			}

			public override string ToString()
			{
				return $"Index<{typeof(T).Name}>({Id.ToString()})";
			}

			public static bool operator ==(Index<T> index1, Index<T> index2)
			{
				return index1.Id == index2.Id;
			}

			public static bool operator !=(Index<T> index1, Index<T> index2)
			{
				return index1.Id != index2.Id;
			}
		}
		
		public static class NodeIndex
		{
			public static readonly Index<Sprite> Entity = new Index<Sprite>(0);
			public static readonly Index<Sprite> Slash = new Index<Sprite>(1);
			public static readonly Index<MovementAnim> Movement = new Index<MovementAnim>(2);
			public static readonly Index<AttackAnim> Attack = new Index<AttackAnim>(3);
			public static readonly Index<GetHitAnim> GetHit = new Index<GetHitAnim>(4);
		}
		
		
		private SpriteSet entitySpriteSet;

		private static Node2D backupNode;

		public Vector2 actualPosition;

		private Dictionary<int, Node> store = new Dictionary<int, Node>();


		public T GetLazy<T>(Index<T> nodeIndex) where T:Node
		{
			if (!store.TryGetValue(nodeIndex.Id, out var node))
			{
				node = LoadNode(ComponentPathArr[nodeIndex.Id]);
				store.Add(nodeIndex.Id, node);
			}
			
			GD.Print(ComponentPathArr[nodeIndex.Id]);
			GD.Print(node.Name);

			return (T)node;
		}

		public T GetOrNull<T>(Index<T> nodeIndex) where T:Node
		{
			if (!store.TryGetValue(nodeIndex.Id, out var node))
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
			GetOrNull(NodeIndex.Movement)?.StopMovement();
			GetOrNull(NodeIndex.Attack)?.StopAttack();
			GetOrNull(NodeIndex.GetHit)?.StopAnim();
			
			
			// movementAnim?.StopMovement();
			// attackAnim?.StopAttack();
			// getHitAnim?.StopAnim();

			// ? before function call to make sure the object is not null
		}

		public void Move(Vector2 destination, MovementAnim.EMovementType movementType)
		{
			var movementAnim = GetLazy(NodeIndex.Movement);

			movementAnim.StartMovement(actualPosition, destination, movementType);
			actualPosition = destination;
		}

		public void SetIdle()
		{
			var entitySprite = GetLazy(NodeIndex.Entity);

			if (entitySpriteSet is null)
				entitySpriteSet = new SpriteSet();

			entitySprite.Texture = entitySpriteSet.IdleTexture;
		}

		public void SetTelegraph()
		{
			var entitySprite = GetLazy(NodeIndex.Entity);

			if (entitySpriteSet is null)
				entitySpriteSet = new SpriteSet();

			entitySprite.Texture = entitySpriteSet.TelegraphTexture;
		}

		public void Attack(Vector2 targetPos)
		{
			SetIdle();

			var movementAnim = GetLazy(NodeIndex.Movement);
			var attackAnim   = GetLazy(NodeIndex.Attack);
			
			movementAnim.LookAt(targetPos);

			attackAnim.SetDirection(movementAnim.isLookingRight);
			attackAnim.StartAttack(targetPos);
		}

		public void GetHit()
		{
			var getHitAnim = GetLazy(NodeIndex.GetHit);
			
			getHitAnim.StartAnim();
		}

		public void DeleteEntity()
		{
			QueueFree();
		}

		private Node LoadNode(string name)
		{
			var node2D = GetNodeOrNull(name);

			if (node2D is null)
			{
				node2D = backupNode.GetNode(name).Duplicate();
				AddChild(node2D);
			}

			return node2D;
		}
	}
}
