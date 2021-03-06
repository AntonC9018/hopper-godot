using Godot;
using Transform = Hopper.Core.WorldNS.Transform;
using Hopper.Core;
using Hopper.Core.Components;
using Hopper.Shared.Attributes;
using Hopper.Utils.Vector;
using Hopper.View.Utils;

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
		public Node2D instanceNode;

		// do keep in mind this node should be a editor-defined prefab, as this constructor removes the redundant sprite
		public EntityAnimator(Node2D prefabNode)
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

			instanceNode = (Node2D) copy.instanceNode.Duplicate();
			Demo.RootNode.AddChild(instanceNode);

			entitySprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Entity);
			slashSprite = (Sprite) instanceNode.GetNodeOrNull(SpritePath.Slash);
				
			GD.Print(instanceNode.Name + " | " + instanceNode.GetParent().Name);
		}

		public void InitInWorld(Hopper.Core.WorldNS.Transform transform)
		{
			instanceNode.Position = transform.position.ToSceneVector();
			GD.Print("Set position " + transform.position + " for " + instanceNode.Name);
		}

		public void SetIdle()
		{
			entitySprite.Texture = idleTexture;
		}

		public void SetTelegraph()
		{
			entitySprite.Texture = telegraphTexture;
		}
		
		// // TODO: delete it at animation end rather than turn start
		// [Hopper.Shared.Attributes.Export(Chain = "+Entity.Death", Dynamic = true)]
		// public void DeleteEntity()
		// {
		// 	instanceNode.QueueFree();
		// }
		//
		// public void DefaultPreset(Hopper.Core.Entity subject)
		// {
		// 	DeleteEntityHandlerWrapper.HookTo(subject);
		// }
	}
}
