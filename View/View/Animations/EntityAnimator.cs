using Godot;
using Godot.Collections;
using Hopper.Core;
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
			public static NodePath Telegraph = new NodePath("TelegraphSprite");
			public static NodePath Slash = new NodePath("SlashSprite");
		}

		public Texture idleTexture;
		public Texture telegraphTexture;

		public Sprite entitySprite;
		public Sprite slashSprite;
		
		// private static Node backupNode;
		
		[Inject]
		public Node instanceNode;

		// do keep in mind this node should be a editor-defined prefab, as this constructor removes the redundant sprite
		public EntityAnimator(Node prefabNode)
		{
			instanceNode = prefabNode;

			entitySprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Entity);
			slashSprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Slash);
			var telegraphSprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Telegraph);

			idleTexture = entitySprite?.Texture;
			telegraphTexture = telegraphSprite?.Texture;
			telegraphSprite?.QueueFree();

			
			// we're basically using the telegraph sprite as a proxy to get the texture
			
			GD.Print("\n" + instanceNode.Name + " | " + instanceNode.GetParent().Name);
		}
		
		public EntityAnimator(EntityAnimator copy)
		{
			idleTexture = copy.idleTexture;
			telegraphTexture = copy.telegraphTexture;

			instanceNode = copy.instanceNode.Duplicate();
			Demo.RootNode.AddChild(instanceNode);

			entitySprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Entity);
			slashSprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Slash);
				
			GD.Print(instanceNode.Name + " | " + instanceNode.GetParent().Name);
		}

		public void SetIdle()
		{
			entitySprite.Texture = idleTexture;
		}

		public void SetTelegraph()
		{
			entitySprite.Texture = telegraphTexture;
		}
		
		public void DeleteEntity()
		{
			instanceNode.QueueFree();
		}
	}
}
