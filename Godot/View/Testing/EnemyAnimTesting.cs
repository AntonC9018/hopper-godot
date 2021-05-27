using Godot;

namespace Hopper_Godot.View
{
	public class EnemyAnimTesting : Node2D
	{
		private SpriteSet enemySpriteSet;
		private Sprite enemySprite;
		private Sprite slashSprite;

		private MovementAnim movementAnim;
		private AttackAnim attackAnim;

		private int xPos = 0;

		public override void _Ready()
		{
			enemySprite = (Sprite) GetNode("EnemySprite");
			slashSprite = (Sprite) GetNode("SlashSprite");
			
			movementAnim = (MovementAnim) GetNode("MovementAnim");
			movementAnim.SetSpriteNode(enemySprite);

			attackAnim = (AttackAnim) GetNode("AttackAnim");
			attackAnim.SetSlashSprite(slashSprite);
		}

		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent.IsActionPressed("ui_right"))
			{
				var startPos = Vector2.Right * xPos + Vector2.Down * 300;
				xPos+= 100;
				var destPos = Vector2.Right * xPos + Vector2.Down * 300;
				
				movementAnim.StartMovement(startPos, destPos, MovementAnim.EMovementType.Walk);
			} else if (inputEvent.IsActionPressed("ui_left"))
			{
				var startPos = Vector2.Right * xPos + Vector2.Down * 300;
				xPos-= 100;
				var destPos = Vector2.Right * xPos + Vector2.Down * 300;
				
				movementAnim.StartMovement(startPos, destPos, MovementAnim.EMovementType.Slide);
			} else if (inputEvent.IsActionPressed("ui_select"))
			{
				var startPos = Vector2.Right * xPos + Vector2.Down * 300;
				
				movementAnim.StartMovement(startPos, startPos, MovementAnim.EMovementType.Jump);
			} else if (inputEvent.IsActionPressed("ui_down"))
			{
				var slashPos = Vector2.Right * xPos + Vector2.Down * 100;
				
				attackAnim.StartAttack(slashPos);
				
				GD.Print("Attack!");
			}
		}

		
		
		/*
		public void PlayIdle()
		{
			enemySprite.Texture = enemySpriteSet.IdleTexture;
		}

		public void PlayTelegraph()
		{
			enemySprite.Texture = enemySpriteSet.TelegraphTexture;
		}

		public void PlayAttack()
		{
			enemySprite.Texture = enemySpriteSet.IdleTexture;
		}

		public override void _Ready()
		{
			
		}
		*/
		
		
		
		
	}
}
