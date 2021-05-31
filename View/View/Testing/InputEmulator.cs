using Godot;

namespace Hopper.View.Testing
{
	public class InputEmulator : Node2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		private EntityAnimator entityAnimator;
		
		public override void _Ready()
		{
			entityAnimator = (EntityAnimator) GetParent();
		}

		public override void _Input(InputEvent inputEvent)
		{
			if (inputEvent is InputEventKey eventKey && !inputEvent.IsEcho() && eventKey.IsPressed())
			{
				// entityAnimator.SkipAnimations();
				
				switch (eventKey.Scancode)
				{
					case (uint)KeyList.Up:
						entityAnimator.Move(entityAnimator.actualPosition + Vector2.Up * 100, MovementAnim.EMovementType.Walk);
						break;
					case (uint)KeyList.Down:
						entityAnimator.Move(entityAnimator.actualPosition + Vector2.Down * 100, MovementAnim.EMovementType.Walk);
						break;
					case (uint)KeyList.Right:
						entityAnimator.Move(entityAnimator.actualPosition + Vector2.Right * 100, MovementAnim.EMovementType.Walk);
						break;
					case (uint)KeyList.Left:
						entityAnimator.Move(entityAnimator.actualPosition + Vector2.Left * 100, MovementAnim.EMovementType.Walk);
						break;
					
					case (uint)KeyList.W:
						entityAnimator.Attack(entityAnimator.actualPosition + Vector2.Up * 100);
						break;
					case (uint)KeyList.S:
						entityAnimator.Attack(entityAnimator.actualPosition + Vector2.Down * 100);
						break;
					case (uint)KeyList.D:
						entityAnimator.Attack(entityAnimator.actualPosition + Vector2.Right * 100);
						break;
					case (uint)KeyList.A:
						entityAnimator.Attack(entityAnimator.actualPosition + Vector2.Left * 100);
						break;
					
					case (uint)KeyList.Space:
						entityAnimator.GetHit();
						break;
					
					case (uint)KeyList.Z:
						entityAnimator.SetTelegraph();
						break;
					
					case (uint)KeyList.X:
						entityAnimator.SetIdle();
						break;
				}
			}
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
	}
}
