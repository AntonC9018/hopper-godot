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

        public bool TrySetActionForAllPlayers(Hopper.Core.World world, InputEventKey eventKey)
        {
            if (IsInputValid(eventKey))
            {
                foreach (var player in world.State.Players)
                {
                    SetActionForPlayer(world, eventKey, player);
                }
                return true;
            }
            return false;
        }

        public bool IsInputValid(InputEventKey eventKey)
        {
            return eventKey.Pressed
                && (VectorMapping.ContainsKey(eventKey.Scancode) || InputMap.ContainsKey(eventKey.Scancode));
        }

        public void SetActionForPlayer(Hopper.Core.World world, InputEventKey eventKey, Entity player)
        {
            var input = player.Behaviors.Get<Controllable>();

            player.Behaviors.Get<Acting>().NextAction =
                InputMap.ContainsKey(eventKey.Scancode)
                    ? input.ConvertInputToAction(InputMap[eventKey.Scancode])
                    : input.ConvertVectorToAction(VectorMapping[eventKey.Scancode]);
        }

        public bool TrySetActionForPlayer(Hopper.Core.World world, InputEventKey eventKey, Entity player)
        {
            if (IsInputValid(eventKey))
            {
                SetActionForPlayer(world, eventKey, player);
                return true;
            }
            return false;
        }
    }
}