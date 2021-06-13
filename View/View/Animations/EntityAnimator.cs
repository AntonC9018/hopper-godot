using Godot;
using Godot.Collections;
using Hopper.Core.Components;
using Hopper.Shared.Attributes;

// TODO: check entity sprite set for textures in case of sprite lazy loading
// TODO: load entity sprite in _Ready out of the entity sprite set

namespace Hopper.View.Animations
{
	public partial class EntityAnimator : IComponent
	{
		public struct SpritePath
		{
			public static NodePath Entity = new NodePath("EntitySprite");
			public static NodePath Slash = new NodePath("SlashSprite");
		}

		[Godot.Export]
		private Texture IdleTexture { get; set; }
		
		[Godot.Export]
		private Texture TelegraphTexture { get; set; }

		// load these things
		public Sprite entitySprite;
		public Sprite slashSprite;
		
		// private static Node backupNode;
		
		[Inject]
		public Node instanceNode;

		// TODO: ya kinda need to update this after each movement
		public Vector2 actualPosition;

		public override void _Ready()
		{
			// if (backupNode is null)
			// {
			// 	backupNode = instanceNode.GetParent().FindNode("BackupNode");
			// 	GD.Print("Loaded backup node " + backupNode.Name);
			// }

			entitySprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Entity);
			slashSprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Slash);
		}

		public void SetIdle()
		{
			entitySprite.Texture = IdleTexture;
		}

		public void SetTelegraph()
		{
			entitySprite.Texture = TelegraphTexture;
		}
		
		public void DeleteEntity()
		{
			instanceNode.QueueFree();
		}
	}
}
