using Godot;

namespace Hopper.Godot.View.Testing
{
	public class SwitchSpriteOnInput : Node2D
	{
		private Sprite enemySprite;
		private bool isIdle;
		public override void _Ready()
		{
			enemySprite = (Sprite)GetNode("Sprite");
			enemySprite.Texture = AssetManager.ZombieSpriteSet.IdleTexture;
			isIdle = true;
		}

		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionPressed("ui_select"))
			{
				enemySprite.Texture = isIdle ? 
					AssetManager.ZombieSpriteSet.TelegraphTexture : 
					AssetManager.ZombieSpriteSet.IdleTexture;

				isIdle = !isIdle;
			}
		}
	}
}
