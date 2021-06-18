using System.Collections.Generic;
using System.Linq;
using Godot;
using Hopper.Core;
using Hopper.Core.ActingNS;
using Hopper.Utils.Vector;
using World = Hopper.Core.WorldNS.World;

namespace Hopper.View
{
    public class InputController : Node
    {
        private Entity playerEntity;
        private Controllable playerControllable;

        private Dictionary<uint, IntVector2> keybinds = new Dictionary<uint, IntVector2>()
        {
            {(uint) KeyList.Up, IntVector2.Up},
            {(uint) KeyList.Down, IntVector2.Down},
            {(uint) KeyList.Right, IntVector2.Right},
            {(uint) KeyList.Left, IntVector2.Left}
        };

        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent is InputEventKey eventKey && !inputEvent.IsEcho() && eventKey.IsPressed())
            {
                if (playerControllable is null)
                {
                    var allPlayers = Registry.Global.Queries.Faction.Get(Faction.Player);
                    
                    if (allPlayers.Any())
                    {
                        playerEntity = allPlayers.First();
                        playerEntity.TryGetControllable(out playerControllable);
                    }
                }
                
                playerControllable?.SelectVectorAction(playerEntity, keybinds[eventKey.Scancode]);
                
                World.Global.Loop();
                
                GD.Print("Input: " + keybinds[eventKey.Scancode]);
            }
        }
    }
}