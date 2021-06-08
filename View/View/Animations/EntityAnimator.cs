using Godot;
using Godot.Collections;
using Hopper.Core.Components;
using Hopper.Shared.Attributes;

// TODO: check entity sprite set for textures in case of sprite lazy loading
// TODO: load entity sprite in _Ready out of the entity sprite set

namespace Hopper.View.Animations
{
	public class EntityAnimator : IComponent
	{
		private static readonly string[] ComponentPathArr =
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
		
		[Godot.Export]
		private Texture IdleTexture { get; set; }
		
		[Godot.Export]
		private Texture TelegraphTexture { get; set; }
		
		private static Node2D backupNode;
		
		[Inject]
		public Node instanceNode;

		public Vector2 actualPosition;

		private Dictionary<int, Node> store = new Dictionary<int, Node>();


		public T GetLazy<T>(Index<T> nodeIndex) where T:Node
		{
			if (!store.TryGetValue(nodeIndex.Id, out var node))
			{
				node = LoadNode(ComponentPathArr[nodeIndex.Id]);
				store.Add(nodeIndex.Id, node);
			}
			
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
			GetOrNull(NodeIndex.Movement)?.StopAnim();
			GetOrNull(NodeIndex.Attack)?.StopAnim();
			GetOrNull(NodeIndex.GetHit)?.StopAnim();
		}

		public void SetIdle()
		{
			var entitySprite = GetLazy(NodeIndex.Entity);

			// TODO: lazy load texture from backup

			entitySprite.Texture = IdleTexture;
		}

		public void SetTelegraph()
		{
			var entitySprite = GetLazy(NodeIndex.Entity);

			// TODO: lazy load texture from backup

			entitySprite.Texture = TelegraphTexture;
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
