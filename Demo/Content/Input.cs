using System.Collections.Generic;
using System.Linq;
using Hopper.Core;
using Hopper.Core.Behaviors;
using Hopper.Core.Behaviors.Basic;
using Hopper.Utils.Vector;
using Godot;

namespace Hopper
{
    public class InputManager
    {
        private static Dictionary<uint, InputMapping> InputMap = new Dictionary<uint, InputMapping> {
            { (uint)KeyList.Space, InputMapping.Weapon_0 },
            { (uint)KeyList.X, InputMapping.Weapon_1 },
        };
        private static Dictionary<uint, IntVector2> VectorMapping = new Dictionary<uint, IntVector2>{
            { (uint)KeyList.Up, IntVector2.Up },
            { (uint)KeyList.Down, IntVector2.Down },
            { (uint)KeyList.Left, IntVector2.Left },
            { (uint)KeyList.Right, IntVector2.Right },
        };

        public bool TrySetAction(Hopper.Core.World world, InputEventKey eventKey)
        {
            if (eventKey.Pressed
                && (VectorMapping.ContainsKey(eventKey.Scancode) || InputMap.ContainsKey(eventKey.Scancode)))
            {
                foreach (var player in world.State.Players)
                {
                    Action action;
                    var input = player.Behaviors.Get<Controllable>();
                    if (InputMap.ContainsKey(eventKey.Scancode))
                    {
                        action = input.ConvertInputToAction(InputMap[eventKey.Scancode]);
                    }
                    else
                    {
                        action = input.ConvertVectorToAction(VectorMapping[eventKey.Scancode]);
                    }
                    player.Behaviors.Get<Acting>().NextAction = action;
                }
                return true;
            }
            return false;
        }
    }
}